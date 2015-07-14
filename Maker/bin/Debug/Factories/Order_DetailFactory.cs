using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class Order_DetailFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public Order_DetailFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Order_Detail>(typeof(Order_Detail).Name) as Order_Detail[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Order_Details.Any(p => p.OrderID == item.OrderID && p.ProductID == item.ProductID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Order_Details.Add(item);	//todo
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
				List<Order_Detail> entity = new List<Order_Detail>();
				foreach (var item in entitiesDbContext.Order_Details)
				{
					if (dataFromDB.Any(p => p.Attribute("OrderID").Value == item.OrderID.ToString() && p.Attribute("ProductID").Value == item.ProductID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Order_Detail>(entity, typeof(Order_Detail).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion Interface implements
	}
}