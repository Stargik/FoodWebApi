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
    public class DifficultyLevelsController : ControllerBase
    {
        private readonly FoodDbContext _context;

        public DifficultyLevelsController(FoodDbContext context)
        {
            _context = context;
        }

        // GET: api/DifficultyLevels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DifficultyLevel>>> GetDifficultyLevels()
        {
          if (_context.DifficultyLevels == null)
          {
              return NotFound();
          }
            return await _context.DifficultyLevels.ToListAsync();
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
