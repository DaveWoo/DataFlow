using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class RegionFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public RegionFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Region>(typeof(Region).Name) as Region[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Regions.Any(p => p.RegionID == item.RegionID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Regions.Add(item);	//todo
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
				List<Region> entity = new List<Region>();
				foreach (var item in entitiesDbContext.Regions)
				{
					if (dataFromDB.Any(p => p.Attribute("RegionID").Value == item.RegionID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Region>(entity, typeof(Region).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}