using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class OrderFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public OrderFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Order>(typeof(Order).Name) as Order[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Orders.Any(p => p.OrderID == item.OrderID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Orders.Add(item);	//todo
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
				List<Order> entity = new List<Order>();
				foreach (var item in entitiesDbContext.Orders)
				{
					if (dataFromDB.Any(p => p.Attribute("OrderID").Value == item.OrderID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Order>(entity, typeof(Order).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}