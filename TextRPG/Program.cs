using System.Runtime.Intrinsics.Arm;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace TextRPG
{
    public enum Job
    {
        전사 = 1,
        마법사,
        궁수
    }

    public interface ICharacter
    {
        string Name { get; }
        int Attack { get; }
        int Armor { get; set; }
        bool IsDead { get; }
        
        void TakeDamage(int damage);
    }

    // 아이템 인터페이스
    public interface IItem
    {
        string Name { get; }
        int AttackBonus { get; set; }
        int ArmorBonus {  get; set; }
        string Description {  get; }
        void Use(Character character); // 캐릭터에게 아이템을 사용하는 메서드
    }


    /*
    // 체력 포션 클래스
    public class HealthPotion : IItem
    {
        public string Name => "체력 포션";

        public void Use(Character character)
        {
            Console.WriteLine("체력 포션을 사용합니다. 체력이 50 증가합니다.");
            character.Health += 50;
            //if (warrior.Health > 100) warrior.Health = 100;
        }
    }

    // 공격력 포션 클래스
    public class StrengthPotion : IItem
    {
        public string Name => "공격력 포션";

        public void Use(Character character)
        {
            Console.WriteLine("공격력 포션을 사용합니다. 공격력이 10 증가합니다.");
            character.Attack += 10;
        }
    }
    */


    // 게임 클래스
    class Game
    {
        public static bool isRest = false;
        static void Main(string[] args)
        {
            SaveGameClass saveGame = new SaveGameClass(); // 저장 기능 클래스 생성
                                                          // 저장된 캐릭터 로드
            Character player = saveGame.LoadCharacter();
            if (player == null)
            {
                JobSelection jobSelection = new JobSelection();
                player = jobSelection.SelectCharacter(); // 새 캐릭터 생성
            }

            // 저장된 인벤토리 로드
            Inventory inventory = new Inventory();
            saveGame.LoadInventory(inventory);

            var shop = new Shop();
            var dungeon = new Dungeon();
            bool fch = false;
            bool isLoop = false;
            bool isSave = false;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
                Console.WriteLine();

                // 메뉴 출력
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("스파르타 마을");
                Console.ResetColor();  // 색상 초기화
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식하기 (50G)");
                Console.WriteLine("6. 저장하기");
                Console.WriteLine("\n초기화하기 입력 시 초기화");
                Console.WriteLine("종료 입력 시 종료");
                Console.WriteLine();

                if (isSave)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine("저장되었습니다.");
                    Console.ResetColor();
                    isSave = false;
                }
                //오류 메시지 출력
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                if (isRest)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("체력을 회복했습니다");
                    Console.ResetColor();
                    isRest = false;
                }
                else if (fch)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("골드가 부족합니다.");
                    Console.ResetColor();
                    fch = false;
                }

                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();  // 행동 입력

                switch (input)
                {
                    case "1":
                        player.ShowStatus();  // 상태 보기 함수 호출
                        break;
                    case "2":
                        inventory.ShowInventory((Character)player);
                        break;
                    case "3":
                        shop.ShowShop((Character)player, inventory);
                        break;
                    case "4":
                        dungeon.ShowDungeon(player);
                        break;
                    case "5":
                        player.Rset();
                        fch = true;
                        break;
                    case "6":
                        isSave = true;
                        saveGame.SaveCharacter(player);
                        saveGame.SaveInventory(inventory);
                        break;
                    case "초기화하기":
                        Console.WriteLine("\n아무거나 입력 시 초기화");
                        Console.WriteLine("0. 취소");
                        Console.WriteLine("\n정말 초기화 하시셌습니까?");
                        Console.Write(">> ");
                        string cancle = Console.ReadLine();
                        if (cancle == "0") break;
                        else
                        {
                            saveGame.ResetSaveData();
                            return;
                        }
                    case "종료":
                        Console.WriteLine("게임을 종료합니다.");
                        return;  // 프로그램 종료
                    default:
                        isLoop = true;
                        break;
                }
            }
        }
    }
}