﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Inventory
    {
        public List<Item> Items { get; private set; }
        
        public Inventory()
        {
            Items = new List<Item>();  // 인벤토리 아이템 리스트
        }

        // 아이템 추가
        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        // 인벤토리 출력
        public void ShowInventory(Character character)
        {
            string actionChoice = null;


            Console.Clear();
            DisplayInventoryHeader();

            // 아이템 목록 출력
            DisplayItemList(character);

            Console.WriteLine("\n1. 아이템 장착");
            Console.WriteLine("2. 아이템 장착 헤제");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                actionChoice = Console.ReadLine();

                // 잘못된 입력 처리
                if (actionChoice != "1" && actionChoice != "0" && actionChoice != "2")
                {
                    Console.Clear();
                    DisplayInventoryHeader();
                    DisplayItemList(character);

                    Console.WriteLine("\n1. 아이템 장착");
                    Console.WriteLine("2. 아이템 장착 헤제");
                    Console.WriteLine("0. 나가기\n");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    // 잘못된 입력이 들어오면 다시 입력 받기
                }
                else
                {
                    // 유효한 입력일 경우
                    if (actionChoice == "1") // 장착 관리
                    {
                        ShowEquipMenu(character);
                        Console.Clear();
                        DisplayInventoryHeader();
                        DisplayItemList(character);
                        Console.WriteLine("\n1. 아이템 장착");
                        Console.WriteLine("2. 아이템 장착 헤제");
                        Console.WriteLine("0. 나가기\n");

                    }
                    else if (actionChoice == "2") // 아이템 장착 해제
                    {
                        UnEquipMenu(character);
                        Console.Clear();
                        DisplayInventoryHeader();
                        DisplayItemList(character);
                        Console.WriteLine("\n1. 아이템 장착");
                        Console.WriteLine("2. 아이템 장착 헤제");
                        Console.WriteLine("0. 나가기\n");
                    }
                    else if (actionChoice == "0") return;  // 루프 종료 (인벤토리 종료)
                }
            }

        }

        private void DisplayInventoryHeader()  //인벤토리 메인 화면 머리
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("인벤토리");
            Console.ResetColor();  // 색상 초기화
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
        }

        public void DisplayItemList(Character character)  //인벤토리 메인
        {
            
            Console.WriteLine();
            int j = 0;
            if (Items.Count == 0)
            {
                Console.WriteLine("아이템이 비어있습니다");
            }
            else
            {
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    string equippedMark = "";

                    // 장착된 아이템이 있으면 "[E]" 표시
                    if (item.IsEquipped == true)
                    {
                        equippedMark = "[E]";
                        Console.ForegroundColor = ConsoleColor.Green;
                    }

                    string bonus = string.Empty;
                    
                    if (item.AttackBonus > 0) bonus += $" 공격력 +{item.AttackBonus}";
                    if (item.ArmorBonus > 0) bonus += $" 방어력 +{item.ArmorBonus}";

                    // 아이템 정보 출력
                    if (!Shop.isSaleItem) // 아이템 판매 메뉴가 아니면
                    {
                        Console.WriteLine($"- {equippedMark}{item.Name} |{bonus} | {item.Description}");
                    }
                    else 
                    {
                        if (j <  Shop.itemdiscount.Count) Console.WriteLine($"{i + 1} {equippedMark}{item.Name} |{bonus} | {item.Description} | 판매 골드: {Shop.itemdiscount[j]}");
                    }
                    j++;
                    Console.ResetColor();
                }
            }
        }


        // 아이템 장착 메뉴
        public void ShowEquipMenu(Character character)
        {
            string errorText = "잘못된 입력입니다. 다시 입력해주세요.";
            bool isLoop = false;
            bool isEQ = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("인벤토리 - 아이템 장착");
                Console.ResetColor();  // 색상 초기화
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();

                if (Items.Count == 0)
                {
                    Console.WriteLine("아이템이 없습니다.");
                }
                else
                {
                    Console.WriteLine("\n[아이템 목록]");
                    for (int i = 0; i < Items.Count; i++)
                    {
                        var item = Items[i];
                        string equippedMark = "";

                        // 장착된 아이템이 있으면 "[E]" 표시
                        if (item.IsEquipped == true)
                        {
                            equippedMark = "[E]";
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                        string bonus = string.Empty;

                        if (item.AttackBonus > 0) bonus += $" 공격력 +{item.AttackBonus}";
                        if (item.ArmorBonus > 0) bonus += $" 방어력 +{item.ArmorBonus}";

                        // 아이템 정보 출력
                        Console.WriteLine($"{i + 1}. {equippedMark}{item.Name} |{bonus} | {item.Description}");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine("\n0. 나가기");
                Console.WriteLine();
                if (isEQ)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("이 아이템은 이미 장착되어 있습니다.");
                    Console.ResetColor();
                    isEQ = false;
                }
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string equipChoice = Console.ReadLine();

                if (equipChoice == "0")
                {
                    return;
                }
                else if (int.TryParse(equipChoice, out int itemIndex) && itemIndex > 0 && itemIndex <= Items.Count)
                {
                    var selectedItem = Items[itemIndex - 1];
                    // 이미 장착되어 있지 않으면 장착
                    if (selectedItem.IsEquipped == false)
                    {
                        selectedItem.IsEquipped = true;
                        character.EquippedItems.Add(selectedItem);
                    }
                    else
                    {
                        isEQ = true;                       
                    }
                }
                else isLoop = true;
            }
        }

        // 장착 해제 메뉴
        public void UnEquipMenu(Character character)
        {
            
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("인벤토리 - 장착 해제");
                Console.ResetColor();
                Console.WriteLine("장착된 아이템을 해제할 수 있습니다.");
                Console.WriteLine();

                if (character.EquippedItems.Count == 0)
                {
                    Console.WriteLine("장착된 아이템이 없습니다.");
                }
                else
                {
                    Console.WriteLine("[장착된 아이템 목록]");
                    for (int i = 0; i < character.EquippedItems.Count; i++)
                    {
                        var item = character.EquippedItems[i];
                        string bonus = string.Empty;

                        if (item.AttackBonus > 0) bonus += $" 공격력 +{item.AttackBonus}";
                        if (item.ArmorBonus > 0) bonus += $" 방어력 +{item.ArmorBonus}";

                        string equippedDisplay = item.IsEquipped ? "[E]" : "";

                        Console.WriteLine($"{i + 1}. [E] {item.Name} |{bonus} | {item.Description}");
                    }
                }

                Console.WriteLine("\n0. 나가기");

                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string equipChoice = Console.ReadLine();

                if (equipChoice == "0")
                {
                    return;  // 메뉴 종료
                }
                else if (int.TryParse(equipChoice, out int itemIndex) && itemIndex > 0 && itemIndex <= character.EquippedItems.Count)
                {
                    var selectedItem = character.EquippedItems[itemIndex - 1];
                    var sss = Items[itemIndex - 1];
                    selectedItem.IsEquipped = false;
                    sss.IsEquipped = false;
                    character.EquippedItems.Remove(selectedItem);  // 장착 해제
                }
                else
                {
                    isLoop = true;  // 잘못된 입력 시 재입력
                }
            }
        }

        public void RemoveItem(int num)
        {
            Items.RemoveAt(num);
        }
    }
}
