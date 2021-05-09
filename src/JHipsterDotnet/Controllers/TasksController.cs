
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
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private const string EntityName = "task";
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _log;

        public TasksController(ILogger<TasksController> log,
            ITaskService taskService)
        {
            _log = log;
            _taskService = taskService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Task>> CreateTask([FromBody] Task task)
        {
            _log.LogDebug($"REST request to save Task : {task}");
            if (task.Id != 0)
                throw new BadRequestAlertException("A new task cannot already have an ID", EntityName, "idexists");

            await _taskService.Save(task);
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, task.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateTask(long id, [FromBody] Task task)
        {
            _log.LogDebug($"REST request to update Task : {task}");
            if (task.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != task.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _taskService.Save(task);
            return Ok(task)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, task.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetAllTasks(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Tasks");
            var result = await _taskService.FindAll(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Task : {id}");
            var result = await _taskService.FindOne(id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Task : {id}");
            await _taskService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
