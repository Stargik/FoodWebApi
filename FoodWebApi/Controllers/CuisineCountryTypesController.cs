using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuisineCountryTypesController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public CuisineCountryTypesController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: api/CuisineCountryTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuisineCountryType>>> GetCuisineCountryTypes()
        {
          if (_context.CuisineCountryTypes == null)
          {
              return NotFound();
          }
            return await _context.CuisineCountryTypes.ToListAsync();
        }

        // GET: api/CuisineCountryTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuisineCountryType>> GetCuisineCountryType(int id)
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

            return cuisineCountryType;
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

            return NoContent();
        }

        private bool CuisineCountryTypeExists(int id)
        {
            return (_context.CuisineCountryTypes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
