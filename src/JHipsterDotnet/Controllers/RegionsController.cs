
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
    [Route("api/regions")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private const string EntityName = "region";
        private readonly IRegionService _regionService;
        private readonly ILogger<RegionsController> _log;

        public RegionsController(ILogger<RegionsController> log,
            IRegionService regionService)
        {
            _log = log;
            _regionService = regionService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Region>> CreateRegion([FromBody] Region region)
        {
            _log.LogDebug($"REST request to save Region : {region}");
            if (region.Id != 0)
                throw new BadRequestAlertException("A new region cannot already have an ID", EntityName, "idexists");

            await _regionService.Save(region);
            return CreatedAtAction(nameof(GetRegion), new { id = region.Id }, region)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, region.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateRegion(long id, [FromBody] Region region)
        {
            _log.LogDebug($"REST request to update Region : {region}");
            if (region.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != region.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _regionService.Save(region);
            return Ok(region)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, region.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetAllRegions(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Regions");
            var result = await _regionService.FindAll(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegion([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Region : {id}");
            var result = await _regionService.FindOne(id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Region : {id}");
            await _regionService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
