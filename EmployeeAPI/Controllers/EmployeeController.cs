using EmployeeAPI.Dto;
using EmployeeAPI.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    /// <summary>
    /// Для сотрудников.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        /// <summary>
        /// Сервис для сотрудников.
        /// </summary>
        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Инициализация контроллера для работы с сотрудниками.
        /// </summary>
        /// <param name="employeeService"> Сервис для сотрудников. </param>
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Создание сотрудника по <paramref name="dto"/>.
        /// </summary>
        /// <param name="dto"> Сотрудник. </param>
        /// <returns> Идентификатор нового сотрудника. </returns>
        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeDto dto)
        {
            try
            {
                var employeeId = await _employeeService.CreateEmployeeAsync(dto);

                return Ok(employeeId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Удаление сотрудника по идентификатору <paramref name="id"/>.
        /// </summary>
        /// <param name="id"> Идентификатор сотрудника. </param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeService.DeleteEmployeeByIdAsync(id);

                if (result)
                    return Ok();

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Обновление сотрудника.
        /// </summary>
        /// <param name="dto"> Сотрудник. </param>
        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeDto dto)
        {
            try
            {
                var result = await _employeeService.UpdateEmployeeAsync(dto);

                if (result)
                    return Ok();

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение сотрудников по идентификатору компании <paramref name="companyId"/>.
        /// </summary>
        /// <param name="companyId"> Идентификатор компании. </param>
        /// <returns> Сотрудников, которые принадлежат данной компании. </returns>
        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetEmployeesByCompanyId(int companyId)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByCompanyIdAsync(companyId);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение сотрудников по названию департамента <paramref name="departmentName"/>.
        /// </summary>
        /// <param name="departmentName"> Название департамента. </param>
        /// <returns> Сотрудников, которые находятся в данном департаменте. </returns>
        [HttpGet("department/{departmentName}")]
        public async Task<IActionResult> GetEmployeesByDepartmentName(string departmentName)
        {
            try
            {
                var employees = await _employeeService.GetEmployeesByDepartmentNameAsync(departmentName);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}