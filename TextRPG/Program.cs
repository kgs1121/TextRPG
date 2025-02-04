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
        

        static void Main(string[] args)
        {
            JobSelection jobSelection = new JobSelection();
            Character player = jobSelection.SelectCharacter();
            var inventory = new Inventory();
            var shop = new Shop();
            var dungeon = new Dungeon();
            string input = null;
            bool isRest = false;
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
                Console.WriteLine("0. 종료");
                // 만약 잘못된 입력이 있었다면, 그 메시지를 화면에 남깁니다.
                if (!string.IsNullOrEmpty(input) && input != "1" && input != "2" && input != "3" && input != "4" && input != "5" &&input != "0")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                }
                if (isRest)
                {
                    Console.ForegroundColor= ConsoleColor.Yellow;
                    Console.WriteLine("\n체력을 회복했습니다");
                    Console.ResetColor();
                    isRest = false;
                }
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                input = Console.ReadLine();

                // 입력된 번호에 맞는 동작
                switch (input)
                {
                    case "1":
                        player.ShowStatus();  // 상태 보기 함수 호출
                        break;
                    case "2":
                        inventory.ShowInventory((Character)player);  // 인벤토리 보여주기
                        break;
                    case "3":
                        //Console.WriteLine("상점 메뉴입니다.");
                        shop.ShowShop((Character)player, inventory);
                        // 상점 관련 로직 추가
                        break;
                    case "4":
                        dungeon.ShowDungeon(player);
                        break;
                    case "5":
                        player.Rset();
                        isRest = true;
                        break;
                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        return;  // 프로그램 종료
                    default:
                        Console.WriteLine("잘못된 입력입니다. 1, 2, 3, 0 중 하나를 입력해주세요.");
                        break;
                }
            }
        }
    }
}