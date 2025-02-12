using EmployeeAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
    }
}
