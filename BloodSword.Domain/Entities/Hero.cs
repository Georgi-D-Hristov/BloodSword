using BloodSword.Domain.Enums;
using System;
using System.Collections.Generic;

namespace BloodSword.Domain.Entities
{
    public class Hero
    {
        // Уникален идентификатор (Primary Key в базата)
        public int Id { get; set; }

        // Връзка към Identity потребителя (кой е собственикът на този герой)
        // Ще го добавим по-късно като string, защото IdentityUser Id е string по подразбиране.
        // public string OwnerId { get; set; }

        public string Name { get; set; } = string.Empty;
        public HeroClass Class { get; set; }

        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;

        // Основни статистики от книгата
        public int FightingProwess { get; set; }
        public int PsychicAbility { get; set; }
        public int Awareness { get; set; }
        public int Endurance { get; set; }       // Максимална кръв
        public int CurrentEndurance { get; set; } // Текуща кръв

        // Навигационни property-та за бъдещи връзки (EF Core ги обича)
        // public ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();

        // Метод за валидация на бизнес правило: Героят жив ли е?
        public bool IsAlive => CurrentEndurance > 0;

        // Метод за нанасяне на щети (Бизнес логика в Домейна!)
        public void TakeDamage(int damage)
        {
            if (damage < 0) damage = 0; // Не може да лекуваме с отрицателни щети
            CurrentEndurance -= damage;
            if (CurrentEndurance < 0) CurrentEndurance = 0;
        }

        // Метод за лекуване
        public void Heal(int amount)
        {
            if (amount < 0) return;
            CurrentEndurance += amount;
            if (CurrentEndurance > Endurance) CurrentEndurance = Endurance;
        }
    }
}
