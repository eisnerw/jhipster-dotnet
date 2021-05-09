
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
    [Route("api/countries")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private const string EntityName = "country";
        private readonly ICountryService _countryService;
        private readonly ILogger<CountriesController> _log;

        public CountriesController(ILogger<CountriesController> log,
            ICountryService countryService)
        {
            _log = log;
            _countryService = countryService;
        }

        [HttpPost]
        [ValidateModel]
        public async Task<ActionResult<Country>> CreateCountry([FromBody] Country country)
        {
            _log.LogDebug($"REST request to save Country : {country}");
            if (country.Id != 0)
                throw new BadRequestAlertException("A new country cannot already have an ID", EntityName, "idexists");

            await _countryService.Save(country);
            return CreatedAtAction(nameof(GetCountry), new { id = country.Id }, country)
                .WithHeaders(HeaderUtil.CreateEntityCreationAlert(EntityName, country.Id.ToString()));
        }

        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateCountry(long id, [FromBody] Country country)
        {
            _log.LogDebug($"REST request to update Country : {country}");
            if (country.Id == 0) throw new BadRequestAlertException("Invalid Id", EntityName, "idnull");
            if (id != country.Id) throw new BadRequestAlertException("Invalid Id", EntityName, "idinvalid");
            await _countryService.Save(country);
            return Ok(country)
                .WithHeaders(HeaderUtil.CreateEntityUpdateAlert(EntityName, country.Id.ToString()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries(IPageable pageable)
        {
            _log.LogDebug("REST request to get a page of Countries");
            var result = await _countryService.FindAll(pageable);
            return Ok(result.Content).WithHeaders(result.GeneratePaginationHttpHeaders());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCountry([FromRoute] long id)
        {
            _log.LogDebug($"REST request to get Country : {id}");
            var result = await _countryService.FindOne(id);
            return ActionResultUtil.WrapOrNotFound(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCountry([FromRoute] long id)
        {
            _log.LogDebug($"REST request to delete Country : {id}");
            await _countryService.Delete(id);
            return Ok().WithHeaders(HeaderUtil.CreateEntityDeletionAlert(EntityName, id.ToString()));
        }
    }
}
