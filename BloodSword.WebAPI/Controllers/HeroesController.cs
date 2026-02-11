using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloodSword.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        // POST: api/heroes
        [HttpPost]
        public async Task<IActionResult> CreateHero([FromBody] CreateHeroDto createHeroDto)
        {
            var createdHero = await _heroService.CreateHeroAsync(createHeroDto);
            return Ok(createdHero);
        }

        // GET: api/heroes
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var heroes = await _heroService.GetAllHeroesAsync();
            return Ok(heroes);
        }

        // GET: api/heroes/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var hero = await _heroService.GetHeroByIdAsync(id);
            if (hero == null)
            {
                return NotFound();
            }
            return Ok(hero);
        }

        [HttpPost("{heroId}/items")]
        public async Task<IActionResult> AddItemToHero(Guid heroId, [FromBody] AddHeroItemDto dto)
        {
            try
            {
                await _heroService.AddItemToHeroAsync(heroId, dto);
                return Ok("Item added to inventory.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/heroes/{heroId}/equip/{itemId}
        [HttpPut("{heroId}/equip/{itemId}")]
        public async Task<IActionResult> EquipItem(Guid heroId, Guid itemId)
        {
            try
            {
                await _heroService.EquipItemAsync(heroId, itemId);
                return Ok("Item equipped successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/heroes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHeroDto dto)
        {
            try
            {
                await _heroService.UpdateHeroAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _heroService.DeleteHeroAsync(id);
            return NoContent();
        }
    }
}