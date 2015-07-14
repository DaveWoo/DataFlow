using DataFlow.Model;
using DataFlow.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DataFlow.Factories
{
	internal class EmployeeFactory : IBuilder
	{
		#region Prepare basic data

		private NorthwindEntities entitiesDbContext = null;
		private IEnumerable<XElement> dataFromDB = null;

		public EmployeeFactory(IEnumerable<XElement> dataFromDB)
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
				var primary = Utility.Deserialize<Employee>(typeof(Employee).Name) as Employee[];

				foreach (var item in primary)
				{
					if (entitiesDbContext.Employees.Any(p => p.EmployeeID == item.EmployeeID))
					{
						//todo
					}
					else
					{
						entitiesDbContext.Employees.Add(item);	//todo
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
				List<Employee> entity = new List<Employee>();
				foreach (var item in entitiesDbContext.Employees)
				{
					if (dataFromDB.Any(p => p.Attribute("EmployeeID").Value == item.EmployeeID.ToString()))
					{
						entity.Add(item);
					}
				}

				Utility.Serialize<Employee>(entity, typeof(Employee).Name);
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion Interface implements
	}
}