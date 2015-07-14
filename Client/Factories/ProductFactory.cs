using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class ProductFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public ProductFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Product>(typeof(Product).Name) as Product[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Products.Any(p => p.ProductID == item.ProductID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Products.Add(item);	//todo
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
				List<Product> entity = new List<Product>();
				foreach (var item in entitiesDbContext.Products)
				{
					if (dataFromDB.Any(p => p.Attribute("ProductID").Value == item.ProductID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Product>(entity, typeof(Product).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}