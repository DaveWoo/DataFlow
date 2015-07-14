using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class TerritoryFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public TerritoryFactory(IEnumerable<XElement> dataFromDB)
		{
			entitiesDbContext = new NorthwindEntities();
			this.dataFromDB = dataFromDB;
		}

		#endregion Prepare basic data

		#region Interface implements

		public void ReadEntityFileToDB()
		{
			try
			{
				var primary = Utility.Deserialize<Territory>(typeof(Territory).Name) as Territory[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Territories.Any(p => p.TerritoryID == item.TerritoryID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Territories.Add(item);	//todo
						entitiesDbContext.SaveChanges();
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		public void CreateEntityFileToDestination()
		{
			try
			{
				List<Territory> entity = new List<Territory>();
				foreach (var item in entitiesDbContext.Territories)
				{
					if (dataFromDB.Any(p => p.Attribute("TerritoryID").Value == item.TerritoryID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Territory>(entity, typeof(Territory).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion Interface implements
	}
}