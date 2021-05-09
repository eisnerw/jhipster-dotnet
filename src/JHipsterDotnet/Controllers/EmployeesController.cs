
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Jhipster.Domain;
using Jhipster.Crosscutting.Exceptions;
using Jhipster.Domain.Repositories.Interfaces;
using Jhipster.Web.Extensions;
using Jhipster.Web.Filters;
using Jhipster.Web.Rest.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Jhipster.Controllers
{
    [Authorize]
    [Route("api/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private const string EntityName = "employee";
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeesController> _log;

        public EmployeesController(ILogger<EmployeesController> log,
            IEmployeeRepository employeeRepository)
        {
            _log = log;
            _employeeRepository = employeeRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            _log.LogDebug($"REST request to save Employee : {employee}");
            if (employee.Id != 0)
                throw new BadRequestAlertException("A new employee cannot already have an ID", EntityName, "idexists");

            await _employeeRepository.CreateOrUpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, employee.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateEmployee(long id, [FromBody] Employee employee)
        {
            _log.LogDebug($"REST request to update Employee : {employee}");
            if (employee.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != employee.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _employeeRepository.CreateOrUpdateAsync(employee);
            await _employeeRepository.SaveChangesAsync();
            return Ok(employee)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, employee.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Employees");
            var result = await _employeeRepository.QueryHelper()
                .Include(employee => employee.Manager)
                .Include(employee => employee.Department)
                .GetPageAsync(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Employee : {id}");
            var result = await _employeeRepository.QueryHelper()
                .Include(employee => employee.Manager)
                .Include(employee => employee.Department)
                .GetOneAsync(employee => employee.Id == id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Employee : {id}");
            await _employeeRepository.DeleteByIdAsync(id);
            await _employeeRepository.SaveChangesAsync();
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
