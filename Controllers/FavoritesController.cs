using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WeatherAPI.Data;
using WeatherAPI.Models;

namespace WeatherAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FavoritesController : ControllerBase
{
    private readonly AppDbContext _context;

    public FavoritesController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/favorites
    [HttpGet]
    public async Task<ActionResult<IEnumerable<FavoriteCity>>> GetFavorites()
    {
        return await _context.FavoriteCities
            .OrderByDescending(f => f.SavedAt)
            .ToListAsync();
    }

    // POST: api/favorites
    [HttpPost]
    public async Task<ActionResult<FavoriteCity>> AddFavorite(FavoriteCity favorite)
    {
        // Check if already exists
        var existing = await _context.FavoriteCities
            .FirstOrDefaultAsync(f => f.CityName.ToLower() == favorite.CityName.ToLower());
        
        if (existing != null)
        {
            return Ok(existing); // Already saved
        }

        _context.FavoriteCities.Add(favorite);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetFavorites), new { id = favorite.Id }, favorite);
    }

    // DELETE: api/favorites/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFavorite(int id)
    {
        var favorite = await _context.FavoriteCities.FindAsync(id);
        if (favorite == null)
        {
            return NotFound();
        }

        _context.FavoriteCities.Remove(favorite);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }
}