using kittyshop.Models;
using kittyshop.Services;
using Microsoft.AspNetCore.Mvc;

namespace kittyshop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CatsController : ControllerBase
{
    private readonly CatsService _catsService;

    public CatsController(CatsService catsService) => _catsService = catsService;

    [HttpGet]
    public async Task<List<Cat>> Get() =>
        await _catsService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Cat>> Get(string id)
    {
        var cat = await _catsService.GetAsync(id);
        if (cat is null)
            return NotFound();
        return cat;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Cat newCat)
    {
        if (CheckCatEntry(newCat) is not null)
            return CheckCatEntry(newCat)!;
        await _catsService.CreateAsync(newCat);
        return CreatedAtAction(nameof(Get), new { id = newCat.Id }, newCat);
    }

    private IActionResult? CheckCatEntry(Cat newCat)
    {
        if (IsSexInvalid(newCat.Sex))
            return BadRequest("Value in field \"sex\" must be equal to either \"male\" or \"female\".");
        if (IsAgeInvalid(newCat.AgeMonths))
            return BadRequest("Value in field \"ageMonths\" must be more than 0.");
        return null;
    }

    private static bool IsAgeInvalid(int ageMonths) => ageMonths <= 0;

    private static bool IsSexInvalid(string sex) => !sex.Equals("male") && !sex.Equals("female");

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Cat updatedCat)
    {
        if (CheckCatEntry(updatedCat) is not null) return CheckCatEntry(updatedCat)!;
        var cat = await _catsService.GetAsync(id);
        if (cat is null)
            return NotFound();
        updatedCat.Id = cat.Id;
        await _catsService.UpdateAsync(id, updatedCat);
        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var cat = await _catsService.GetAsync(id);
        if (cat is null)
            return NotFound();
        await _catsService.RemoveAsync(id);
        return NoContent();
    }
}