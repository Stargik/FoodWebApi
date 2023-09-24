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
using System.Drawing.Printing;
using FoodWebApi.Interfaces;

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly FoodDbContext _context;
        private readonly ICacheService cacheService;

        public CategoriesController(FoodDbContext context, ICacheService cacheService)
        {
            _context = context;
            this.cacheService = cacheService;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories([FromQuery] PaginationParams paginationParams)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var categories = await cacheService.GetData<IEnumerable<Category>>(typeof(Category).ToString());
            if (categories is null)
            {
                categories = await _context.Categories.ToListAsync();
                await cacheService.SetData(typeof(Category).ToString(), categories, TimeSpan.FromSeconds(60));
            }


            if (paginationParams.PageSize is not null)
            {
                var paginationCategories = PaginationList<Category>.ToPaginationList(categories, paginationParams.numberPage, (int)paginationParams.PageSize);
                var metadata = new
                {
                    paginationCategories.TotalCount,
                    paginationCategories.PageSize,
                    paginationCategories.CurrentPage,
                    paginationCategories.TotalPages,
                    paginationCategories.HasNext,
                    paginationCategories.HasPrevious,
                    nextLink = $"{Url.PageLink()}?numberPage={paginationParams.numberPage + 1}&PageSize={paginationParams.PageSize}"
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(paginationCategories);
            }
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var categories = await cacheService.GetData<IEnumerable<Category>>(typeof(Category).ToString());
            Category category;
            if (categories is not null)
            {
                category = categories.FirstOrDefault(c => c.Id == id);
                if (category is not null)
                {
                    return Ok(category);
                }
            }
            category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                await cacheService.RemoveData(typeof(Category).ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'FoodDbContext.Categories'  is null.");
            }
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            await cacheService.RemoveData(typeof(Category).ToString());

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_context.Categories == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            await cacheService.RemoveData(typeof(Category).ToString());

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
