using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace TextRPG
{
    public class SaveGameClass
    {
        public void SaveCharacter(Character character)
        {
            using (StreamWriter writer = new StreamWriter("character_data.txt"))
            {
                writer.WriteLine(character.Name);
                writer.WriteLine(character.CharacterJob);
                writer.WriteLine(character.Level);
                writer.WriteLine(character.MaxHealth);
                writer.WriteLine(character.NowHealth);
                writer.WriteLine(character.Attack);
                writer.WriteLine(character.Armor);
                writer.WriteLine(character.Gold);
                writer.WriteLine(character.NowEx);
                writer.WriteLine(character.EXP);
            }
        }

        public Character LoadCharacter()
        {
            if (!File.Exists("character_data.txt"))
            {
                Console.WriteLine("저장된 캐릭터 데이터가 없습니다.");
                return null;
            }

            using (StreamReader reader = new StreamReader("character_data.txt"))
            {
                string name = reader.ReadLine();
                Job job = (Job)Enum.Parse(typeof(Job), reader.ReadLine());
                int level = int.Parse(reader.ReadLine());
                int maxHealth = int.Parse(reader.ReadLine());
                int nowHealth = int.Parse(reader.ReadLine());
                int attack = int.Parse(reader.ReadLine());
                int armor = int.Parse(reader.ReadLine());
                int gold = int.Parse(reader.ReadLine());
                int nowEx = int.Parse(reader.ReadLine());
                double exp = double.Parse(reader.ReadLine());

                Character character = new Character(name, job)
                {
                    Level = level,
                    MaxHealth = maxHealth,
                    NowHealth = nowHealth,
                    Attack = attack,
                    Armor = armor,
                    Gold = gold,
                    NowEx = nowEx,
                    EXP = exp
                };

                return character;
            }
        }


        public void SaveInventory(Inventory inventory)
        {
            using (StreamWriter writer = new StreamWriter("inventory.txt"))
            {
                foreach (Item item in inventory.Items)
                {
                    writer.WriteLine($"{item.Name},{item.ArmorBonus},{item.AttackBonus},{item.Description}");
                }
            }
        }

        public void LoadInventory(Inventory inventory)
        {
            if (!File.Exists("inventory.txt"))
            {
                Console.WriteLine("저장된 인벤토리가 없습니다.");
                return;
            }

            using (StreamReader reader = new StreamReader("inventory.txt"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');

                    string name = parts[0];
                    int type = int.Parse(parts[1]);
                    int armorBonus = int.Parse(parts[2]);
                    int attackBonus = int.Parse(parts[3]);
                    string description = parts[4];
                    int price = int.Parse(parts[5]);

                    inventory.AddItem(new Item(name, armorBonus, attackBonus, description, price));
                }
            }
        }


        public void ResetSaveData()
        {
            if (File.Exists("character_data.txt"))
            {
                File.Delete("character_data.txt");
            }
            if (File.Exists("inventory.txt"))
            {
                File.Delete("inventory.txt");
            }
            Console.WriteLine("저장된 데이터가 초기화되었습니다.");
        }



    }
}
