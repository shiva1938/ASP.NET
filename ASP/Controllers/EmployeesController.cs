using CRUDMVCDemo.Data;
using CRUDMVCDemo.Models;
using CRUDMVCDemo.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRUDMVCDemo.Controllers
{
	public class EmployeesController : Controller
	{
		private readonly MVCrudDemo mvcrudDemo;

		public EmployeesController(MVCrudDemo mvcrudDemo)
		{
			this.mvcrudDemo = mvcrudDemo;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var employees = await mvcrudDemo.Employees.ToListAsync();
			return View(employees);
		}

		[HttpGet]
		public IActionResult Add()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeViewModel)
		{
			var employee = new Employee()
			{
				Id = Guid.NewGuid(),
				Name = addEmployeeViewModel.Name,
				Email = addEmployeeViewModel.Email,
				Salary = addEmployeeViewModel.Salary,
				Department = addEmployeeViewModel.Department,
				DateOfBirth = addEmployeeViewModel.DateOfBirth,
			};
			await mvcrudDemo.Employees.AddAsync(employee);
			await mvcrudDemo.SaveChangesAsync();
			return RedirectToAction("Add");
		}

		public async Task<IActionResult> View(Guid id)
		{
			var employee = await mvcrudDemo.Employees.FirstOrDefaultAsync(x => x.Id == id);

			if(employee != null)
			{
				var viewModel = new UpdateEmployeesViewModel()
				{
					Id = employee.Id,
					Name = employee.Name,
					Email = employee.Email,
					Salary = employee.Salary,
					Department = employee.Department,
					DateOfBirth = employee.DateOfBirth,
				};
				return await Task.Run(() => View("View", viewModel));
			}
			return View("Index");
		}

		[HttpPost]
		public async Task<IActionResult> View(UpdateEmployeesViewModel model)
		{
			var employee = await mvcrudDemo.Employees.FindAsync(model.Id);
			if(employee != null)
			{
				employee.Name = model.Name;
				employee.Email = model.Email;	
				employee.Salary = model.Salary;
				employee.DateOfBirth = model.DateOfBirth;
				employee.Department = model.Department;

				await mvcrudDemo.SaveChangesAsync();

				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");	
		}
		public async Task<IActionResult> Delete(UpdateEmployeesViewModel model)
		{
			var employee = await mvcrudDemo.Employees.FindAsync(model.Id);
			if(employee != null)
			{
				mvcrudDemo.Employees.Remove(employee);
				await mvcrudDemo.SaveChangesAsync();

				return RedirectToAction("Index");
			}
			return RedirectToAction("Index");
		} 
	}
}
