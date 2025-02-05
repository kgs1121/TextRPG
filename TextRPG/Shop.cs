using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Shop
    {
        public List<Item> ItemsSale { get; private set; }
        public List<int> itemPrices = new List<int>();  // 원래 가격
        public static List<int> itemdiscount = new List<int>();  // 판매 시 가격 리스트
        public static bool isSaleItem = false;
        public Shop()
        {
            ItemsSale = new List<Item>
            {
                new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000, false),
                new Item("무쇠갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 2800, false),
                new Item("스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500, false),
                new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 700, false),
                new Item("청동 도끼", 5, 0, "어디선가 사용됐던 거 같은 도끼입니다.", 1500, false),
                new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 2500, false)
            };

            foreach (var item in ItemsSale)
            {
                itemPrices.Add(item.Price); // 가격만 추가
            }
        }



        public void ShowShop(Character character, Inventory inventory)
        {
            Console.Clear();
            Shopconsol(character);

            Console.WriteLine("원하시는 행동을 입력해주세요. ");
            Console.Write(">> ");

            while (true)
            {
                string action = Console.ReadLine();

                if (action == "1")
                {
                    PurchaseItem(character, inventory);
                    Console.Clear();
                    Shopconsol(character);
                    Console.WriteLine("원하시는 행동을 입력해주세요. ");
                    Console.Write(">> ");
                    continue;
                }
                else if (action == "2")
                {
                    isSaleItem = true;
                    Selitem(character, inventory);
                    Console.Clear();
                    Shopconsol(character);
                    Console.WriteLine("원하시는 행동을 입력해주세요. ");
                    Console.Write(">> ");
                }
                else if (action == "0") break;
                else
                {
                    Console.Clear();
                    Shopconsol(character);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    Console.WriteLine("원하시는 행동을 입력해주세요. ");
                    Console.Write(">> ");
                }
            }
        }

        public void Shopconsol(Character character)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("상점");
            Console.ResetColor();  // 색상 초기화
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{character.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            int i = 0;
            foreach (var item in ItemsSale)
            {
                string status = item.Price == 0 ? "구매완료" : $"{item.Price} G";
                string bonus = item.ArmorBonus > 0 ? $"방어력 +{item.ArmorBonus}" : $"공격력 +{item.AttackBonus}";

                if (item.Price == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"- {item.Name} | {bonus} | {item.Description} | {status}");
                    Console.ResetColor();
                }
                else Console.WriteLine($"- {item.Name} | {bonus} | {item.Description} | {status}");
                i++;
            }

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기\n");
        }


        // 아이템 구매 처리
        private void PurchaseItem(Character character, Inventory inventory)
        {
            string input;
            int inputnum;
            
            Console.Clear();
            Invenconsol(character);
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요. ");
            Console.Write(">> ");
            do
            {
                input = Console.ReadLine();

                if (input == "0")
                {
                    return;
                }

                if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= ItemsSale.Count)
                {
                    var selectedItem = ItemsSale[itemIndex - 1];

                    if (selectedItem.Price == 0)
                    {
                        Console.Clear();
                        Invenconsol(character);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"\n{selectedItem.Name} 아이템은 이미 구매되었습니다.");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("원하시는 행동을 입력해주세요. ");
                        Console.Write(">> ");
                    }

                    else if (character.Gold >= selectedItem.Price)
                    {
                        itemdiscount.Add((int)(selectedItem.Price * 4.0 / 5));
                        character.Gold -= itemPrices[itemIndex - 1];  // 구매 시 금액 차감
                        selectedItem.Price = 0;
                        inventory.AddItem(selectedItem);
                        Console.Clear();
                        Invenconsol(character);
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"\n{selectedItem.Name}을(를) 구매했습니다.");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("원하시는 행동을 입력해주세요. ");
                        Console.Write(">> ");
                    }
                    else
                    {
                        Console.Clear();
                        Invenconsol(character);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n골드가 부족합니다.");
                        Console.ResetColor();
                        Console.WriteLine();
                        Console.WriteLine("원하시는 행동을 입력해주세요. ");
                        Console.Write(">> ");
                    }
                }
                else
                {
                    Console.Clear();
                    Invenconsol(character);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    Console.WriteLine("원하시는 행동을 입력해주세요. ");
                    Console.Write(">> ");
                }
            }
            while (true);
        }


        public void Invenconsol(Character character)
        {
            int i = 1;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("상점 - 아이템 구매");
            Console.ResetColor();  // 색상 초기화
            Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.");
            Console.WriteLine();
            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{character.Gold} G");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");

            foreach (var item in ItemsSale)
            {
                string status = item.Price == 0 ? "구매완료" : $"{item.Price} G";
                string bonus = item.ArmorBonus > 0 ? $"방어력 +{item.ArmorBonus}" : $"공격력 +{item.AttackBonus}";

                if (item.Price == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"- {item.Name} | {bonus} | {item.Description} | {status}");
                    Console.ResetColor();
                }
                else Console.WriteLine($"{i}. {item.Name} | {bonus} | {item.Description} | {status}");
                i++;
            }

            Console.WriteLine();
            Console.WriteLine("0. 나가기");
        }


        public void Selitem(Character character, Inventory inventory)   //아이템 판매
        {
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("상점 - 아이템 판매");
                Console.ResetColor();
                Console.WriteLine("아이템을 판매할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{character.Gold} G");
                Console.WriteLine();
                inventory.DisplayItemList(character);
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine();
                if (isLoop) // 오류 번호 입력 시 메시지 출력
                {
                    Console.ForegroundColor= ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("원하시는 행동을 입력해주세요. ");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") return;
                else if (int.TryParse(input, out int Selnum) && (Selnum > 0 && Selnum <= inventory.Items.Count)) // 인벤토리 아이템 번호 확인
                {
                    var selectedItem = ItemsSale[Selnum];
                    int getGold = itemdiscount[Selnum - 1];
                    character.Gold += getGold;
                    
                    itemdiscount.RemoveAt(Selnum - 1);
                    inventory.RemoveItem(Selnum - 1);
                    selectedItem.Price = itemPrices[Selnum];
                }
                else isLoop = true;   // 오류 번호 입력 시 메시지 출력
            }
        }
    }
}
