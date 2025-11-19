using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BloodSword.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroesController : ControllerBase
    {
        // Инжектираме Сървиса, а НЕ Репозиторито!
        // Контролерът си говори с Application слоя, не с Infrastructure.
        private readonly IHeroService _heroService;

        public HeroesController(IHeroService heroService)
        {
            _heroService = heroService;
        }

        // POST: api/heroes
        [HttpPost]
        public async Task<IActionResult> CreateHero([FromBody] CreateHeroDto createHeroDto)
        {
            // 1. Валидацията на DTO-то (Required, StringLength) се случва автоматично тук 
            // заради атрибута [ApiController]. Ако е невалидно, връща 400 Bad Request.

            // 2. Викаме сервиза да свърши работата
            var createdHero = await _heroService.CreateHeroAsync(createHeroDto);

            // 3. Връщаме 200 OK с резултата
            // (В идеалния REST свят трябва да е CreatedAtAction, но за сега Ok е достатъчно)
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
                return NotFound(); // Връща 404, ако няма такъв герой
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
                // 204 No Content е стандартен отговор за успешен PUT/UPDATE
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(); // 404 Not Found, ако ID-то не съществува
            }
        }

        // DELETE: api/heroes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            // Тук не проверяваме дали героят съществува, защото DeleteAsync на Repo-то
            // просто не прави нищо, ако не го намери, и ние връщаме 204 (No Content)
            // независимо от резултата (за по-добра сигурност).
            await _heroService.DeleteHeroAsync(id);
            return NoContent(); // 204 No Content
        }
    }
}