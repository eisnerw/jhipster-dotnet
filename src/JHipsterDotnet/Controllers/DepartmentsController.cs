
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Jhipster.Domain;
using Jhipster.Crosscutting.Exceptions;
using Jhipster.Domain.Services.Interfaces;
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
    [Route("api/departments")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private const string EntityName = "department";
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentsController> _log;

        public DepartmentsController(ILogger<DepartmentsController> log,
            IDepartmentService departmentService)
        {
            _log = log;
            _departmentService = departmentService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Department>> CreateDepartment([FromBody] Department department)
        {
            _log.LogDebug($"REST request to save Department : {department}");
            if (department.Id != 0)
                throw new BadRequestAlertException("A new department cannot already have an ID", EntityName, "idexists");

            await _departmentService.Save(department);
            return CreatedAtAction(nameof(GetDepartment), new { id = department.Id }, department)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, department.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateDepartment(long id, [FromBody] Department department)
        {
            _log.LogDebug($"REST request to update Department : {department}");
            if (department.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != department.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _departmentService.Save(department);
            return Ok(department)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, department.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Department>>> GetAllDepartments(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Departments");
            var result = await _departmentService.FindAll(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDepartment([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Department : {id}");
            var result = await _departmentService.FindOne(id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Department : {id}");
            await _departmentService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
