
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
    [Route("api/jobs")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private const string EntityName = "job";
        private readonly IJobRepository _jobRepository;
        private readonly ILogger<JobsController> _log;

        public JobsController(ILogger<JobsController> log,
            IJobRepository jobRepository)
        {
            _log = log;
            _jobRepository = jobRepository;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Job>> CreateJob([FromBody] Job job)
        {
            _log.LogDebug($"REST request to save Job : {job}");
            if (job.Id != 0)
                throw new BadRequestAlertException("A new job cannot already have an ID", EntityName, "idexists");

            await _jobRepository.CreateOrUpdateAsync(job);
            await _jobRepository.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJob), new { id = job.Id }, job)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, job.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateJob(long id, [FromBody] Job job)
        {
            _log.LogDebug($"REST request to update Job : {job}");
            if (job.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != job.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _jobRepository.CreateOrUpdateAsync(job);
            await _jobRepository.SaveChangesAsync();
            return Ok(job)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, job.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetAllJobs(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Jobs");
            var result = await _jobRepository.QueryHelper()
                .Include(job => job.Tasks)
                .Include(job => job.Employee)
                .GetPageAsync(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJob([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Job : {id}");
            var result = await _jobRepository.QueryHelper()
                .Include(job => job.Tasks)
                .Include(job => job.Employee)
                .GetOneAsync(job => job.Id == id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Job : {id}");
            await _jobRepository.DeleteByIdAsync(id);
            await _jobRepository.SaveChangesAsync();
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
