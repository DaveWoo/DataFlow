using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class CustomerFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public CustomerFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Customer>(typeof(Customer).Name) as Customer[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Customers.Any(p => p.CustomerID == item.CustomerID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Customers.Add(item);	//todo
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
				List<Customer> entity = new List<Customer>();
				foreach (var item in entitiesDbContext.Customers)
				{
					if (dataFromDB.Any(p => p.Attribute("CustomerID").Value == item.CustomerID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Customer>(entity, typeof(Customer).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion Interface implements
	}
}