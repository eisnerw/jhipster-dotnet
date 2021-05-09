
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
    [Route("api/locations")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private const string EntityName = "location";
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationsController> _log;

        public LocationsController(ILogger<LocationsController> log,
            ILocationService locationService)
        {
            _log = log;
            _locationService = locationService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Location>> CreateLocation([FromBody] Location location)
        {
            _log.LogDebug($"REST request to save Location : {location}");
            if (location.Id != 0)
                throw new BadRequestAlertException("A new location cannot already have an ID", EntityName, "idexists");

            await _locationService.Save(location);
            return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, location.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateLocation(long id, [FromBody] Location location)
        {
            _log.LogDebug($"REST request to update Location : {location}");
            if (location.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != location.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _locationService.Save(location);
            return Ok(location)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, location.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetAllLocations(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Locations");
            var result = await _locationService.FindAll(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLocation([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Location : {id}");
            var result = await _locationService.FindOne(id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Location : {id}");
            await _locationService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
