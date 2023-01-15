using CRUDMVCDemo.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVCDemo.Data
{
	public class MVCrudDemo : DbContext
	{
		public MVCrudDemo(DbContextOptions options): base(options)
		{

		}
		public DbSet<Employee> Employees { get; set; }
	}
}
