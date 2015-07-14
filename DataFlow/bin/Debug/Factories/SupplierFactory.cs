using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class SupplierFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public SupplierFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Supplier>(typeof(Supplier).Name) as Supplier[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Suppliers.Any(p => p.SupplierID == item.SupplierID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Suppliers.Add(item);	//todo
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
				List<Supplier> entity = new List<Supplier>();
				foreach (var item in entitiesDbContext.Suppliers)
				{
					if (dataFromDB.Any(p => p.Attribute("SupplierID").Value == item.SupplierID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Supplier>(entity, typeof(Supplier).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}