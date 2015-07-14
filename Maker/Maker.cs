using Maker.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;

namespace DataFlow
{
	internal class Maker
	{
		public Maker()
		{ }

		public void GenerateCsFiles()
		{
			try
			{
				// Iterate through the collection to get each entity container.
				foreach (EntityContainer container in Utility.ContainersCSpace)
				{
					Console.WriteLine("EntityContainer Name: {0} ", container.Name);

					// EntitySetBase is a super type for EntitySets and RelationshipSets. Iterate
					// through the collection to get each EntitySetBase.
					foreach (EntitySetBase baseSet in container.BaseEntitySets)
					{
						string entityType = string.Empty;
						string entitySet = string.Empty;
						string entityKey = "p => ";
						string entityKey2 = "p => ";
						// Check if this instance is an EntitySet.
						if (baseSet is EntitySet)
						{
							Console.WriteLine(
							   "  EntitySet Name: {0} , EntityType Name: {1} ",
							   baseSet.Name, baseSet.ElementType.FullName);

							entityType = baseSet.ElementType.Name;
							entitySet = baseSet.Name;
							if (baseSet.ElementType.KeyMembers != null && baseSet.ElementType.KeyMembers.Count > 0)
							{
								List<string> temp = new List<string>();
								List<string> temp2 = new List<string>();
								foreach (var item in baseSet.ElementType.KeyMembers)
								{
									temp.Add("p." + item + " == item." + item.Name);
									temp2.Add("p.Attribute(\"" + item.Name + "\").Value == item." + item.Name + ".ToString()");
								}
								string tempExpression = string.Join(" && ", temp);
								entityKey += tempExpression;
								tempExpression = string.Join(" && ", temp2);
								entityKey2 += tempExpression;
							}
						}

						WriteCsFile("NorthwindEntities", entityType, entitySet, entityKey, entityKey2);
					}
				}
			}
			catch (MetadataException exceptionMetadata)
			{
				Console.WriteLine("MetadataException: {0}", exceptionMetadata.Message);
			}
			catch (System.Data.MappingException exceptionMapping)
			{
				Console.WriteLine("MappingException: {0}", exceptionMapping.Message);
			}
		}

		private string GenerateSingleFactory(string dbEntities, string entityType, string entitySet, string entityKey, string entityKey2)
		{
			string template = Settings.Default.FactoriesTemplates;
			template = template.Replace("$DBEntities$", dbEntities);
			template = template.Replace("$entityType$", entityType);
			template = template.Replace("$entitySet$", entitySet);
			template = template.Replace("$entityKey$", entityKey);
			template = template.Replace("$entityKey2$", entityKey2);

			return template;
		}

		private void WriteCsFile(string dbEntities, string entityType, string entitySet, string entityKey, string entityKey2)
		{
			string factoryClass = GenerateSingleFactory(dbEntities, entityType, entitySet, entityKey, entityKey2);
			Directory.CreateDirectory("Factories");
			File.WriteAllText("Factories\\" + entityType + "Factory.cs", factoryClass);
		}
	}
}