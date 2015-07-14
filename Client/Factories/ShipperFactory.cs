using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class ShipperFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public ShipperFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Shipper>(typeof(Shipper).Name) as Shipper[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Shippers.Any(p => p.ShipperID == item.ShipperID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Shippers.Add(item);	//todo
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
				List<Shipper> entity = new List<Shipper>();
				foreach (var item in entitiesDbContext.Shippers)
				{
					if (dataFromDB.Any(p => p.Attribute("ShipperID").Value == item.ShipperID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Shipper>(entity, typeof(Shipper).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}