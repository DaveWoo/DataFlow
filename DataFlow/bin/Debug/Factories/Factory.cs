using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class Factory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public Factory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<>(typeof().Name) as [];

				foreach (var item in primary)
				{
					if (entitiesDbContext..Any(p => ))
					{
						//todo
					}
					else
					{
						entitiesDbContext..Add(item);	//todo
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
				List<> entity = new List<>();
				foreach (var item in entitiesDbContext.)
				{
					if (dataFromDB.Any(p => ))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<>(entity, typeof().Name);
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		#endregion Interface implements
	}
}