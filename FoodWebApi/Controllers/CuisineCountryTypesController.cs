using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;
using FoodWebApi.Models;
using Newtonsoft.Json;
using FoodWebApi.Interfaces;

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineCountryTypesController : ControllerBase
    {
        private readonly FoodDbContext _context;
        private readonly ICacheService cacheService;

        public CuisineCountryTypesController(FoodDbContext context, ICacheService cacheService)
        {
            _context = context;
            this.cacheService = cacheService;
        }

        // GET: api/CuisineCountryTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuisineCountryType>>> GetCuisineCountryTypes([FromQuery] PaginationParams paginationParams)
        {
            if (_context.CuisineCountryTypes == null)
            {
                return NotFound();
            }
            var cuisineCountryTypes = await cacheService.GetData<IEnumerable<CuisineCountryType>>(typeof(CuisineCountryType).ToString());
            if (cuisineCountryTypes is null)
            {
                cuisineCountryTypes = await _context.CuisineCountryTypes.ToListAsync();
                await cacheService.SetData(typeof(CuisineCountryType).ToString(), cuisineCountryTypes, TimeSpan.FromSeconds(60));
            }

            if (paginationParams.PageSize is not null)
            {
                var paginationCuisineCountryTypes = PaginationList<CuisineCountryType>.ToPaginationList(cuisineCountryTypes, paginationParams.numberPage, (int)paginationParams.PageSize);
                var metadata = new
                {
                    paginationCuisineCountryTypes.TotalCount,
                    paginationCuisineCountryTypes.PageSize,
                    paginationCuisineCountryTypes.CurrentPage,
                    paginationCuisineCountryTypes.TotalPages,
                    paginationCuisineCountryTypes.HasNext,
                    paginationCuisineCountryTypes.HasPrevious,
                    nextLink = $"{Url.PageLink()}?numberPage={paginationParams.numberPage + 1}&PageSize={paginationParams.PageSize}"
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(paginationCuisineCountryTypes);
            }
            return Ok(cuisineCountryTypes);
        }

        // GET: api/CuisineCountryTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuisineCountryType>> GetCuisineCountryType(int id)
        {
            if (_context.CuisineCountryTypes == null)
            {
                return NotFound();
            }
            var cuisineCountryTypes = await cacheService.GetData<IEnumerable<CuisineCountryType>>(typeof(CuisineCountryType).ToString());
            CuisineCountryType cuisineCountryType;
            if (cuisineCountryTypes is not null)
            {
                cuisineCountryType = cuisineCountryTypes.FirstOrDefault(c => c.Id == id);
                if (cuisineCountryType is not null)
                {
                    return Ok(cuisineCountryType);
                }
            }
            cuisineCountryType = await _context.CuisineCountryTypes.FindAsync(id);

            if (cuisineCountryType == null)
            {
                return NotFound();
            }

            return Ok(cuisineCountryType);
        }

        // PUT: api/CuisineCountryTypes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuisineCountryType(int id, CuisineCountryType cuisineCountryType)
        {
            if (id != cuisineCountryType.Id)
            {
                return BadRequest();
            }

            _context.Entry(cuisineCountryType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await cacheService.RemoveData(typeof(CuisineCountryType).ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuisineCountryTypeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CuisineCountryTypes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CuisineCountryType>> PostCuisineCountryType(CuisineCountryType cuisineCountryType)
        {
            if (_context.CuisineCountryTypes == null)
            {
                return Problem("Entity set 'FoodDbContext.CuisineCountryTypes'  is null.");
            }
            _context.CuisineCountryTypes.Add(cuisineCountryType);
            await _context.SaveChangesAsync();
            await cacheService.RemoveData(typeof(CuisineCountryType).ToString());

            return CreatedAtAction("GetCuisineCountryType", new { id = cuisineCountryType.Id }, cuisineCountryType);
        }

        // DELETE: api/CuisineCountryTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuisineCountryType(int id)
        {
            if (_context.CuisineCountryTypes == null)
            {
                return NotFound();
            }
            var cuisineCountryType = await _context.CuisineCountryTypes.FindAsync(id);
            if (cuisineCountryType == null)
            {
                return NotFound();
            }

            _context.CuisineCountryTypes.Remove(cuisineCountryType);
            await _context.SaveChangesAsync();
            await cacheService.RemoveData(typeof(CuisineCountryType).ToString());

            return NoContent();
        }

        private bool CuisineCountryTypeExists(int id)
        {
            return (_context.CuisineCountryTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
