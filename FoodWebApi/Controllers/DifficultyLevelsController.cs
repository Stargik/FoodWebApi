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

namespace FoodWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DifficultyLevelsController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public DifficultyLevelsController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: api/DifficultyLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DifficultyLevel>>> GetDifficultyLevels([FromQuery] PaginationParams paginationParams)
        {
            if (_context.DifficultyLevels == null)
            {
                return NotFound();
            }
            var levels = await _context.DifficultyLevels.ToListAsync();
            if (paginationParams.PageSize is not null)
            {
                var paginationLevels = PaginationList<DifficultyLevel>.ToPaginationList(levels, paginationParams.numberPage, (int)paginationParams.PageSize);
                var metadata = new
                {
                    paginationLevels.TotalCount,
                    paginationLevels.PageSize,
                    paginationLevels.CurrentPage,
                    paginationLevels.TotalPages,
                    paginationLevels.HasNext,
                    paginationLevels.HasPrevious,
                    nextLink = $"{Url.PageLink()}?numberPage={paginationParams.numberPage + 1}&PageSize={paginationParams.PageSize}"
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
                return Ok(paginationLevels);
            }
            return Ok(levels);
        }

        // GET: api/DifficultyLevels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DifficultyLevel>> GetDifficultyLevel(int id)
        {
          if (_context.DifficultyLevels == null)
          {
              return NotFound();
          }
            var difficultyLevel = await _context.DifficultyLevels.FindAsync(id);

            if (difficultyLevel == null)
            {
                return NotFound();
            }

            return difficultyLevel;
        }
    }
}
