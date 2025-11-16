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
    }
}