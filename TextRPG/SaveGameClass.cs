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
                    writer.WriteLine($"{item.Name}, {item.AttackBonus}, {item.ArmorBonus}, {item.Description}, {item.Price}, {item.IsEquipped}");
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
                    int armorBonus = int.Parse(parts[1]);
                    int attackBonus = int.Parse(parts[2]);
                    string description = parts[3];
                    int price = int.Parse(parts[4]);
                    bool isequipped = bool.Parse(parts[5]);
                    inventory.AddItem(new Item(name, armorBonus, attackBonus, description, price, isequipped));
                }
            }
        }


        public void SaveEquippedItems(Character character)
        {
            using (StreamWriter writer = new StreamWriter("equippedItems.txt"))
            {
                foreach (var item in character.EquippedItems)
                {
                    writer.WriteLine($"{item.Name},{item.AttackBonus},{item.ArmorBonus},{item.Description}, {item.IsEquipped}");
                }
            }
        }


        public void LoadEquippedItems(Character character)
        {
            if (File.Exists("equippedItems.txt"))
            {
                using (StreamReader reader = new StreamReader("equippedItems.txt"))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] itemData = line.Split(',');

                        // 아이템을 생성하고 장착
                        string itemName = itemData[0];
                        int attackBonus = int.Parse(itemData[1]);
                        int armorBonus = int.Parse(itemData[2]);
                        string description = itemData[3];
                        bool isEquipped = bool.Parse(itemData[4]);

                        // 해당 아이템을 생성
                        IItem item = new Item(itemName, attackBonus, armorBonus, description, 0, isEquipped);  // 가격은 0으로 설정하거나 적절히 처리
                        if (item != null) character.EquippedItems.Add(item);  // 장착된 아이템 리스트에 추가                        
                    }
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
            if (File.Exists("equippedItems.txt"))
            {
                File.Delete("equippedItems.txt");
            }
        }


        /*
         
        // 캐릭터 텍스트 파일 내용
        용사
        Warrior
        5
        100
        75
        20
        10
        500
        30
        150.5

        // LoadCharacter() 실행 후:
        Character character = new Character("용사", Job.Warrior);
        character.Level = 5;
        character.MaxHealth = 100;
        character.NowHealth = 75;
        character.Attack = 20;
        character.Armor = 10;
        character.Gold = 500;
        character.NowEx = 30;
        character.EXP = 150.5;


        // 인벤토리 텍스트 파일 내용:
        철검,0,15,튼튼한 철로 만들어진 검,200
        가죽 갑옷,10,0,가죽으로 만든 갑옷,150

        // LoadInventory() 실행 후:
        inventory.AddItem(new Item("철검", 0, 15, "튼튼한 철로 만들어진 검", 200));
        inventory.AddItem(new Item("가죽 갑옷", 10, 0, "가죽으로 만든 갑옷", 150));


        */
    }
}
