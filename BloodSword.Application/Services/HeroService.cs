using BloodSword.Application.Contracts;
using BloodSword.Application.DTOs;
using BloodSword.Domain.Entities;
using BloodSword.Domain.Enums;
using System;
using System.Threading.Tasks;

namespace BloodSword.Application.Services
{
    public class HeroService : IHeroService
    {
        private readonly IHeroRepository _heroRepository;

        // Инжектираме Репозиторито, не DbContext!
        public HeroService(IHeroRepository heroRepository)
        {
            _heroRepository = heroRepository;
        }

        public async Task<HeroDto> CreateHeroAsync(CreateHeroDto createHeroDto)
        {
            // --- 1. Ръчно Мапване (DTO -> Entity) ---
            var hero = new Hero
            {
                Id = Guid.NewGuid(), // Генерираме ID тук
                Name = createHeroDto.Name,
                Class = createHeroDto.Class,
                Level = 1,
                Experience = 0
            };

            // --- 2. Прилагане на Бизнес Логика (Правилата на играта) ---
            // Това е мястото, където Service-ът блести!
            switch (hero.Class)
            {
                case HeroClass.Warrior:
                    hero.FightingProwess = 10;
                    hero.PsychicAbility = 3;
                    hero.Awareness = 5;
                    hero.Endurance = 12;
                    break;
                case HeroClass.Sage:
                    hero.FightingProwess = 5;
                    hero.PsychicAbility = 10;
                    hero.Awareness = 8;
                    hero.Endurance = 8;
                    break;
                case HeroClass.Trickster:
                    hero.FightingProwess = 7;
                    hero.PsychicAbility = 5;
                    hero.Awareness = 10;
                    hero.Endurance = 9;
                    break;
                case HeroClass.Enchanter:
                    hero.FightingProwess = 4;
                    hero.PsychicAbility = 12;
                    hero.Awareness = 6;
                    hero.Endurance = 7;
                    break;
                default:
                    throw new ArgumentException("Invalid hero class selected.");
            }
            hero.CurrentEndurance = hero.Endurance; // Започва с пълна кръв

            // --- 3. Извикване на Репозиторито ---
            var newHero = await _heroRepository.CreateAsync(hero);

            // --- 4. Ръчно Мапване (Entity -> DTO) ---
            // Връщаме DTO, а не Entity-то!
            var heroDto = new HeroDto
            {
                Id = newHero.Id,
                Name = newHero.Name,
                Class = newHero.Class,
                Level = newHero.Level,
                Experience = newHero.Experience
            };

            return heroDto;
        }

        public async Task<IEnumerable<HeroDto>> GetAllHeroesAsync()
        {
            var heroes = await _heroRepository.GetAllAsync();

            // Ръчно мапване на списък (малко досадно, но полезно упражнение)
            var heroDtos = new List<HeroDto>();
            foreach (var hero in heroes)
            {
                heroDtos.Add(new HeroDto
                {
                    Id = hero.Id,
                    Name = hero.Name,
                    Class = hero.Class,
                    Level = hero.Level,
                    Experience = hero.Experience
                });
            }
            return heroDtos;
        }

        public async Task<HeroDto> GetHeroByIdAsync(Guid id)
        {
            var hero = await _heroRepository.GetByIdAsync(id);
            if (hero == null) return null; // Или хвърли Exception, ще го обсъдим

            return new HeroDto
            {
                Id = hero.Id,
                Name = hero.Name,
                Class = hero.Class,
                Level = hero.Level,
                Experience = hero.Experience
            };
        }
    }
}