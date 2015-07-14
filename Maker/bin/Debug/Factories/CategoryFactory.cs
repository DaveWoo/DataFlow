using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class CategoryFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public CategoryFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Category>(typeof(Category).Name) as Category[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Categories.Any(p => p.CategoryID == item.CategoryID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Categories.Add(item);	//todo
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
				List<Category> entity = new List<Category>();
				foreach (var item in entitiesDbContext.Categories)
				{
					if (dataFromDB.Any(p => p.Attribute("CategoryID").Value == item.CategoryID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Category>(entity, typeof(Category).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion Interface implements
	}
}