using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class CustomerDemographicFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public CustomerDemographicFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<CustomerDemographic>(typeof(CustomerDemographic).Name) as CustomerDemographic[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.CustomerDemographics.Any(p => p.CustomerTypeID == item.CustomerTypeID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.CustomerDemographics.Add(item);	//todo
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
				List<CustomerDemographic> entity = new List<CustomerDemographic>();
				foreach (var item in entitiesDbContext.CustomerDemographics)
				{
					if (dataFromDB.Any(p => p.Attribute("CustomerTypeID").Value == item.CustomerTypeID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<CustomerDemographic>(entity, typeof(CustomerDemographic).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}