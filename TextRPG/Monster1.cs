using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Monster : ICharacter
    {
        public string Name { get; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
        public int Level { get; set; }
        public bool IsDead => Health <= 0;

        public Monster(string name, int health, int level, int attack, int armor)
        {
            Name = name;
            Health = health;
            Level = level;
            Attack = attack;
            Armor = armor;

            // 스테이지 레벨에 따라 속성 설정
            SetStatsBasedOnLevel();
        }

        public void SetStatsBasedOnLevel()
        {
            // 스테이지 레벨에 따른 능력치 증가
            Attack += (int)((Level * 2));  // 예시: 레벨마다 공격력 2 증가
            Health += (int)((Level * 5)); // 예시: 레벨마다 체력 10 증가
            Armor += (int)((Level * 1));    // 예시: 레벨마다 방어력 1 증가
        }


        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else
            {
                Console.Write($"{Name}이(가) {damage}의 데미지를 받았습니다. ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} 남은 체력: {Health}");
                Console.ResetColor();
            }
        }
    }
}
