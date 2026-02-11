using BloodSword.Domain.Enums;

namespace BloodSword.Domain.Entities
{
    public class Hero
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public HeroClass Class { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;

        // Core statistics from the game
        public int FightingProwess { get; set; }
        public int PsychicAbility { get; set; }
        public int Awareness { get; set; }
        public int Endurance { get; set; }
        public int CurrentEndurance { get; set; }

        public virtual ICollection<InventoryItem> Inventory { get; set; } = new List<InventoryItem>();

        public bool IsAlive => CurrentEndurance > 0;

        public void TakeDamage(int damage)
        {
            if (damage < 0) damage = 0;
            CurrentEndurance -= damage;
            if (CurrentEndurance < 0) CurrentEndurance = 0;
        }

        public void Heal(int amount)
        {
            if (amount < 0) return;
            CurrentEndurance += amount;
            if (CurrentEndurance > Endurance) CurrentEndurance = Endurance;
        }
    }
}
