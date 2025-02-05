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
        public int Price { get; set; }  // 가격
        public bool IsEquipped { get; set; }

        public Item(string name, int attackBonus, int armorBonus, string description, int price, bool isequipped)
        {
            Name = name;
            AttackBonus = attackBonus;
            ArmorBonus = armorBonus;
            Description = description;
            Price = price;
            IsEquipped = isequipped;
            int orginPrice = price;
        }
    }
}
