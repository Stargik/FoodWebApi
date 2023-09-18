using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FoodMVCWebApp.Data;
using FoodMVCWebApp.Entities;
using FoodMVCWebApp;
using FoodMVCWebApp.Interfaces;
using Microsoft.Extensions.Options;
using FoodMVCWebApp.Models;
using FoodWebApi.Models;
using Newtonsoft.Json;

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly FoodDbContext _context;
        private readonly IImageService imageService;
        private readonly StaticFilesSettings imgSettings;

        public DishesController(FoodDbContext context, IImageService imageService, IOptions<StaticFilesSettings> imgSettings)
        {
            _context = context;
            this.imageService = imageService;
            this.imgSettings = imgSettings.Value;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes([FromQuery] PaginationParams paginationParams)
        {
          if (_context.Dishes == null)
          {
              return NotFound();
          }
            var storagePath = await imageService.GetStoragePath();
            var dishes = await _context.Dishes.Select(dish => new Dish
            {
                Id = dish.Id,
                Title = dish.Title,
                Recipe = dish.Recipe,
                Category = dish.Category,
                CategoryId = dish.CategoryId,
                DifficultyLevel = dish.DifficultyLevel,
                DifficultyLevelId = dish.DifficultyLevelId,
                CuisineCountryType = dish.CuisineCountryType,
                CuisineCountryTypeId = dish.CuisineCountryTypeId,
                Image = storagePath + "/" + dish.Image
            }).ToListAsync();

            if (paginationParams.PageSize is not null)
            {
                var paginationDishes = PaginationList<Dish>.ToPaginationList(dishes, paginationParams.numberPage, (int)paginationParams.PageSize);
                var metadata = new
                {
                    paginationDishes.TotalCount,
                    paginationDishes.PageSize,
                    paginationDishes.CurrentPage,
                    paginationDishes.TotalPages,
                    paginationDishes.HasNext,
                    paginationDishes.HasPrevious,
                    nextLink = $"{Url.PageLink()}?numberPage={paginationParams.numberPage + 1}&PageSize={paginationParams.PageSize}"
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(paginationDishes);
            }
            return Ok(dishes);
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
          if (_context.Dishes == null)
          {
              return NotFound();
          }
            var dish = await _context.Dishes.FindAsync(id);
            dish.Image = (await imageService.GetStoragePath()) + "/" + dish.Image;

            if (dish == null)
            {
                return NotFound();
            }

            return dish;
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, [FromForm] DishDTO dishDTO)
        {
            if (id != dishDTO.Id)
            {
                return BadRequest();
            }

            var dish = new Dish
            {
                Id = dishDTO.Id,
                Title = dishDTO.Title,
                Recipe = dishDTO.Recipe,
                CategoryId = dishDTO.CategoryId,
                DifficultyLevelId = dishDTO.DifficultyLevelId,
                CuisineCountryTypeId = dishDTO.CuisineCountryTypeId,
            };
            if (dishDTO.Image is null)
            {
                dish.Image = (await _context.Dishes.AsNoTracking().FirstOrDefaultAsync(dish => dish.Id == dishDTO.Id)).Image;
            }
            if (dishDTO.Image is not null)
            {
                await imageService.Upload(dishDTO.Image);
            }
            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
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

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dish>> PostDish([FromForm] DishDTO dishDTO)
        {
          if (_context.Dishes == null)
          {
              return Problem("Entity set 'FoodDbContext.Dishes'  is null.");
          }
            var dish = new Dish
            {
                Title = dishDTO.Title,
                Recipe = dishDTO.Recipe,
                CategoryId = dishDTO.CategoryId,
                DifficultyLevelId = dishDTO.DifficultyLevelId,
                CuisineCountryTypeId = dishDTO.CuisineCountryTypeId,
                Image = dishDTO.Image.FileName
            };
            await imageService.Upload(dishDTO.Image);
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();
            dish.Image = (await imageService.GetStoragePath()) + "/" + dish.Image;
            return CreatedAtAction("GetDish", new { id = dish.Id }, dish);
        }

        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }

            _context.Dishes.Remove(dish);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DishExists(int id)
        {
            return (_context.Dishes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
