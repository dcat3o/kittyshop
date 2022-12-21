using kittyshop.Models;
using kittyshop.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace kittyshop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PositionsController : ControllerBase
{
    private readonly PositionsService _positionsService;

    public PositionsController(PositionsService positionsService) => _positionsService = positionsService;

    [HttpGet]
    public async Task<List<Position>> Get() =>
        await _positionsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Position>> Get(string id)
    {
        var position = await _positionsService.GetAsync(id);
        if (position is null)
            return NotFound();
        return position;
    }

    [HttpPost]
    public async Task<IActionResult?> Post(Position newPosition)
    {
        if (CheckPositionEntry(newPosition) is not null) return CheckPositionEntry(newPosition);
        await _positionsService.CreateAsync(newPosition);
        return CreatedAtAction(nameof(Get), new { id = newPosition.Id }, newPosition);
    }

    private IActionResult? CheckPositionEntry(Position newPosition)
    {
        if (newPosition.Price < 0) return BadRequest("Value in field \"price\" must not be less than zero.");
        return null;
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Position updatedPosition)
    {
        if (CheckPositionEntry(updatedPosition) is not null) return CheckPositionEntry(updatedPosition)!;
        var position = await _positionsService.GetAsync(id);
        if (position is null)
            return NotFound();
        updatedPosition.Id = position.Id;
        await _positionsService.UpdateAsync(id, updatedPosition);
        return NoContent();
    }
    
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var position = await _positionsService.GetAsync(id);
        if (position is null)
            return NotFound();
        await _positionsService.RemoveAsync(id);
        return NoContent();
    }
}