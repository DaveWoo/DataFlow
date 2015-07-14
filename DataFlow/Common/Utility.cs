using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace DataFlow
{
	public class Utility
	{
		public const string ExtensionName = ".bak";

		public const string FactoryCreateEntityFileToDestination = "CreateEntityFileToDestination";
		public const string FactoryReadEntityFileToDB = "ReadEntityFileToDB";
		public const string FactoryName = "DataFlow.Factories";

		public static string BasicDir
		{
			get
			{
				return AppDomain.CurrentDomain.BaseDirectory;
			}
		}

		public static string BasicDirOutput
		{
			get
			{
				return Path.Combine(BasicDir, "Output");
			}
		}

		public static string BasicDirInput
		{
			get
			{
				return Path.Combine(BasicDir, "Input");
			}
		}

		public static EntityConnectionX connection = null;

		public static EntityConnection Connection
		{
			get
			{
				if (connection == null)
					connection = new EntityConnectionX();
				return connection.Connection;

			}
		}

		public static ReadOnlyCollection<EntityContainer> ContainersCSpace
		{
			get
			{
				MetadataWorkspace workspace = Connection.GetMetadataWorkspace();

				// Get a collection of the entity containers.
				var containers = workspace.GetItems<EntityContainer>(DataSpace.CSpace);

				return containers;
			}
		}

		public static ReadOnlyCollection<EntityContainer> ContainersSSpace
		{
			get
			{
				MetadataWorkspace workspace = Connection.GetMetadataWorkspace();

				// Get a collection of the entity containers.
				var containers = workspace.GetItems<EntityContainer>(DataSpace.SSpace);

				return containers;
			}
		}

		public static EntitySet EntitySet(string name)
		{
			return ContainersCSpace[0].BaseEntitySets.Where(p => p.Name == name).First() as EntitySet;
		}

		public static IEnumerable<XElement> GenerateMappedDataFile(string entityType)
		{
			IEnumerable<XElement> dataFromDB = null;
			try
			{
				string pathSecurity = Path.Combine(BasicDirInput, entityType + ".xml");
				var dbFile = File.ReadAllLines(pathSecurity);
				string contents = string.Empty;
				string schema = string.Empty;
				foreach (var item in dbFile)
				{
					string elemenetEntityType = item.Substring(item.IndexOf(entityType));
					string elemenetEntityTypeSchema = item.Substring(0, item.IndexOf(entityType) - 1);
					if (string.IsNullOrWhiteSpace(schema))
					{
						schema = elemenetEntityTypeSchema.Substring(elemenetEntityTypeSchema.LastIndexOf(".") + 1);
					}
					contents += "<" + elemenetEntityType;
				}
				TextReader textReader = new StringReader("<Root>" + contents + "</Root>");
				XDocument xDocSecurity = XDocument.Load(textReader);
				dataFromDB = xDocSecurity.Descendants(entityType);
			}
			catch (Exception)
			{
				throw;
			}
			return dataFromDB;
		}

		public static void Serialize<T>(List<T> obj, string fileName)
		{
			string outputFileFullPath = Path.Combine(Utility.BasicDirOutput, fileName + Utility.ExtensionName);
			Directory.CreateDirectory(Path.GetDirectoryName(outputFileFullPath));

			using (MemoryStream memoryStream = new MemoryStream())
			using (StreamReader reader = new StreamReader(memoryStream))
			{
				DataContractSerializer serializer = new DataContractSerializer(typeof(T[]), null, Int32.MaxValue, false, false, null, new SharedTypeResolver());
				serializer.WriteObject(memoryStream, obj.ToArray());
				memoryStream.Position = 0;
				string contents = reader.ReadToEnd();
				File.WriteAllText(outputFileFullPath, contents);
			}
		}

		public static void Serialize<TEntity>(DbSet<TEntity> obj, string fileName) where TEntity : class
		{
			string outputFileFullPath = Path.Combine(Utility.BasicDirOutput, fileName + Utility.ExtensionName);

			using (MemoryStream memoryStream = new MemoryStream())
			using (StreamReader reader = new StreamReader(memoryStream))
			{
				DataContractSerializer serializer = new DataContractSerializer(typeof(TEntity[]), null, Int32.MaxValue, false, false, null, new SharedTypeResolver());
				serializer.WriteObject(memoryStream, obj.ToArray());
				memoryStream.Position = 0;
				string contents = reader.ReadToEnd();
				File.WriteAllText(outputFileFullPath, contents);
			}
		}

		public static object Deserialize<T>(string fileName)
		{
			string outputFileFullPath = Path.Combine(Utility.BasicDirOutput, fileName + Utility.ExtensionName);

			if (!File.Exists(outputFileFullPath))
			{
				throw new FileNotFoundException("File not found: " + outputFileFullPath);
			}

			XDocument xDocSecurityBak = XDocument.Load(outputFileFullPath);

			using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(xDocSecurityBak.ToString())))
			{
				XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(memoryStream, Encoding.UTF8, new XmlDictionaryReaderQuotas(), null);
				DataContractSerializer serializer = new DataContractSerializer(typeof(T[]), null, Int32.MaxValue, false, false, null, new SharedTypeResolver());

				return serializer.ReadObject(reader);
			}
		}
	}
}