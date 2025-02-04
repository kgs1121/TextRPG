using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Item : IItem
    {
        public string Name { get; }
        public int AttackBonus { get; set; }
        public int ArmorBonus { get; set; }
        public string Description { get; }
        public int Price { get; set; }  // 가격 추가

        public Item(string name, int attackBonus, int armorBonus, string description, int price)
        {
            Name = name;
            AttackBonus = attackBonus;
            ArmorBonus = armorBonus;
            Description = description;
            Price = price;
        }

        public void Use(Character character)
        {
            // 예시로 아이템을 사용했을 때 캐릭터의 능력치를 증가시키는 방식으로 처리
            character.Attack += AttackBonus;
            character.Armor += ArmorBonus;
        }
    }
}
