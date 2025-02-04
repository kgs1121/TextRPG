using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;


namespace TextRPG
{
    /*
    public class SaveGameClass
    {
        public static void SaveCharacterData(Character character)
        {
            string data = $"{character.Name}\n{character.Level}\n{character.Gold}\n{character.MaxHealth}\n{character.NowHealth}\n{character.EXP}";

            File.WriteAllText("characterData.txt", data);
            Console.WriteLine("게임 데이터가 저장되었습니다.");
        }

        public static Character LoadCharacterData()
        {
            if (File.Exists("characterData.txt"))
            {
                string[] data = File.ReadAllLines("characterData.txt");

                Character character = new Character()
                {
                    Name = data[0],
                    Level = int.Parse(data[1]),
                    Gold = int.Parse(data[2]),
                    MaxHealth = int.Parse(data[3]),
                    NowHealth = int.Parse(data[4]),
                    EXP = double.Parse(data[5])
                };
                return character;
            }
            else
            {
                Console.WriteLine("저장된 데이터가 없습니다.");
                return new Character();
            }
        }
    }
    */
}
