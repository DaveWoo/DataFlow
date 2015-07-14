using DataFlow;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Client
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				//Template
				//Security
				string entityTypeName = "Template";

				#region Generate db data to a file

				var inputFileList = Directory.GetFiles(Utility.BasicDirInput);
				foreach (var item in inputFileList)
				{
					string fileName = Path.GetFileNameWithoutExtension(item);
					// Invoke respective method
					Run(fileName, DataFlowType.Out);
				}

				#endregion Generate db data to a file

				#region Import data to db

				var outputFileList = Directory.GetFiles(Utility.BasicDirOutput);
				foreach (var item in outputFileList)
				{
					string fileName = Path.GetFileNameWithoutExtension(item);
					// Invoke respective method
					Run(fileName, DataFlowType.In);
				}

				#endregion Import data to db

				Run(entityTypeName, DataFlowType.In);
			}
			catch (Exception)
			{
			}
		}

		private static void Run(string entityType, DataFlowType dataFlowType)
		{
			try
			{
				// Mapping generated db by bcp
				IEnumerable<XElement> dataFromDB = Utility.GenerateMappedDataFile(entityType);

				// Get necessary type of Assemble for invoke specific method
				string targetClass = string.Format("{0}.{1}{2}", Utility.FactoryName, entityType, "Factory");
				var assembly = Assembly.GetAssembly(typeof(IBuilder));
				var type = assembly.GetType(targetClass);
				if (type == null)
				{
					throw new ArgumentNullException("Cannot find type: " + targetClass);
				}

				object[] argsGenerator = new object[1] { dataFromDB };
				object entitiesObject = Activator.CreateInstance(type, argsGenerator);

				// Invoke Constructor
				ConstructorInfo ctor = type.GetConstructor(new[] { typeof(IEnumerable<XElement>) });
				object instance = ctor.Invoke(argsGenerator);

				switch (dataFlowType)
				{
					case DataFlowType.Out:
						// Invoke Method
						type.InvokeMember(Utility.FactoryCreateEntityFileToDestination, BindingFlags.InvokeMethod, null, entitiesObject, null);
						break;

					case DataFlowType.In:
						type.InvokeMember(Utility.FactoryReadEntityFileToDB, BindingFlags.InvokeMethod, null, entitiesObject, null);
						break;

					default:
						break;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
	}

	[Flags]
	internal enum DataFlowType
	{
		Out,
		In,
	}
}