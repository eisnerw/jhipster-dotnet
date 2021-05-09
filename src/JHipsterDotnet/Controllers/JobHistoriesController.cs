
using System.Collections.Generic;
using System.Threading.Tasks;
using JHipsterNet.Core.Pagination;
using Jhipster.Domain;
using Jhipster.Crosscutting.Enums;
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
    [Route("api/job-histories")]
    [ApiController]
    public class JobHistoriesController : ControllerBase
    {
        private const string EntityName = "jobHistory";
        private readonly IJobHistoryService _jobHistoryService;
        private readonly ILogger<JobHistoriesController> _log;

        public JobHistoriesController(ILogger<JobHistoriesController> log,
            IJobHistoryService jobHistoryService)
        {
            _log = log;
            _jobHistoryService = jobHistoryService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<JobHistory>> CreateJobHistory([FromBody] JobHistory jobHistory)
        {
            _log.LogDebug($"REST request to save JobHistory : {jobHistory}");
            if (jobHistory.Id != 0)
                throw new BadRequestAlertException("A new jobHistory cannot already have an ID", EntityName, "idexists");

            await _jobHistoryService.Save(jobHistory);
            return CreatedAtAction(nameof(GetJobHistory), new { id = jobHistory.Id }, jobHistory)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, jobHistory.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateJobHistory(long id, [FromBody] JobHistory jobHistory)
        {
            _log.LogDebug($"REST request to update JobHistory : {jobHistory}");
            if (jobHistory.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != jobHistory.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _jobHistoryService.Save(jobHistory);
            return Ok(jobHistory)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, jobHistory.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobHistory>>> GetAllJobHistories(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of JobHistories");
            var result = await _jobHistoryService.FindAll(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJobHistory([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get JobHistory : {id}");
            var result = await _jobHistoryService.FindOne(id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJobHistory([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete JobHistory : {id}");
            await _jobHistoryService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
