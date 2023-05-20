using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;
using Sales.API.Helpers;
using Sales.Shared.DTOs;
using Sales.Shared.Entities;

namespace Sales.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
        {
            var querable = _context.Categories
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                querable = querable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            return Ok(await querable
                .OrderBy(c => c.Name)
                .Paginate(pagination)
                .ToListAsync());
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> GetPages([FromQuery] PaginationDTO pagination)
        {
            var querable = _context.Categories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(pagination.Filter))
            {
                querable = querable.Where(c => c.Name.ToLower().Contains(pagination.Filter.ToLower()));
            }

            double count = await querable.CountAsync();
            double totalPages = Math.Ceiling(count / pagination.RecordsNumber);
            return Ok(totalPages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Category category)
        {
            _context.Add(category);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is null)
                {
                    return BadRequest(dbUpdateException.Message);
                }

                if (dbUpdateException.InnerException!.Message!.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una categoría con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutAsync(Category category)
        {
            _context.Update(category);
            try
            {
                await _context.SaveChangesAsync();
                return Ok(category);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException is null)
                {
                    return BadRequest(dbUpdateException.Message);
                }

                if (dbUpdateException.InnerException!.Message!.Contains("duplicate"))
                {
                    return BadRequest("Ya existe una categoría con el mismo nombre.");
                }

                return BadRequest(dbUpdateException.InnerException.Message);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
