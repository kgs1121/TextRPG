using System.Runtime.Intrinsics.Arm;
using static System.Net.Mime.MediaTypeNames;
using System.Threading;
using System.Diagnostics;

namespace TextRPG
{
    public enum Job
    {
        전사,
        마법사,
        궁수
    }

    public interface ICharacter
    {
        string Name { get; }
        int Health { get; set; }
        int Attack { get; set; }
        int Armor { get; set; }
        bool IsDead { get; }
        void TakeDamage(int damage);
        void ShowStatus();
    }

    // 캐릭터 클래스
    public class Character : ICharacter
    {
        public string Name { get; }
        public Job CharacterJob { get; }
        public int Health { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
        public int Gold { get; set; }
        public IItem EquippedItem { get; set; }  // 장착 중인 아이템

        public bool IsDead => Health <= 0;

        // 생성자에서 직업을 선택하고, 직업에 맞는 기본 속성 설정
        public Character(string name, Job job)
        {
            Name = name;
            CharacterJob = job;

            // 직업별 기본 속성 설정
            switch (job)
            {
                case Job.전사:
                    Health = 120;
                    Attack = 15;
                    Armor = 10;
                    Gold = 1500;
                    EquippedItem = null;
                    break;
                case Job.마법사:
                    Health = 80;
                    Attack = 25;
                    Armor = 3;
                    Gold = 1500;
                    EquippedItem = null;
                    break;
                case Job.궁수:
                    Health = 90;
                    Attack = 20;
                    Armor = 5;
                    Gold = 1500;
                    EquippedItem = null;
                    break;
            }
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead)
                Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else
                Console.WriteLine($"{Name}이(가) {damage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }

        // 상태 보기 메서드
        public void ShowStatus()
        {
            //Console.WriteLine();
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine($"Lv. 01");
            Console.WriteLine($"{Name} ({CharacterJob})");
            Console.WriteLine($"체력 : {Health}");
            Console.WriteLine($"공격력 : {Attack}");
            Console.WriteLine($"방어력 : {Armor}");
            Console.WriteLine($"Gold : {Gold} G");
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string choice = Console.ReadLine();
                if (choice == "0")
                {
                    break; // 0을 입력하면 메뉴로 돌아가도록 종료
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다. 0을 입력해주세요.");
                }
            }
        }
    }


    public class JobSelection   //직업 선택
    {
        // 직업을 선택하고 캐릭터를 반환하는 메서드
        public ICharacter SelectCharacter()
        {
            ICharacter player = null; // player 변수를 null로 초기화
            string jobChoice;

            while (true)
            {
                Console.WriteLine("직업을 선택해주세요.");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.WriteLine("3. 궁수");
                Console.WriteLine();
                Console.WriteLine("원하는 직업 번호를 입력해주세요. ");
                Console.Write(">> ");

                jobChoice = Console.ReadLine();

                // 유효한 입력이 들어오면 탈출
                if (jobChoice == "1" || jobChoice == "2" || jobChoice == "3")
                {
                    break;
                }
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
            }

            // 직업 선택에 맞게 직업을 설정하고, Character 객체를 생성합니다.
            switch (jobChoice)
            {
                case "1":
                    Console.Write("\n전사 이름을 입력해주세요: ");
                    string warriorName = Console.ReadLine();
                    player = new Character(warriorName, Job.전사);
                    break;
                case "2":
                    Console.Write("\n마법사 이름을 입력해주세요: ");
                    string mageName = Console.ReadLine();
                    player = new Character(mageName, Job.마법사);
                    break;
                case "3":
                    Console.Write("\n궁수 이름을 입력해주세요: ");
                    string archerName = Console.ReadLine();
                    player = new Character(archerName, Job.궁수);
                    break;
            }
            return player;
        }
    }




    // 몬스터 클래스
    /*
    public class Monster : ICharacter
    {
        public string Name { get; }
        public int Health { get; set; }
        public int Attack => new Random().Next(10, 20); // 공격력은 랜덤

        public bool IsDead => Health <= 0;

        public Monster(string name, int health)
        {
            Name = name;
            Health = health;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else Console.WriteLine($"{Name}이(가) {damage}의 데미지를 받았습니다. 남은 체력: {Health}");
        }
    }

    // 고블린 클래스
    public class Goblin : Monster
    {
        public Goblin(string name) : base(name, 50) { } // 체력 50
    }

    // 드래곤 클래스
    public class Dragon : Monster
    {
        public Dragon(string name) : base(name, 100) { } // 체력 100
    }
    */


    // 아이템 인터페이스
    public interface IItem
    {
        string Name { get; }
        void Use(Character character); // 캐릭터에게 아이템을 사용하는 메서드
    }


    public class Item : IItem
    {
        public string Name { get; }
        public int AttackBonus { get; }
        public int ArmorBonus { get; }
        public string Description { get; }
        public int Price { get; }  // 가격 추가

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
            //Console.WriteLine($"{Name}을(를) 사용하여 {character.Name}의 공격력 +{AttackBonus}, 방어력 +{ArmorBonus} 증가!");
        }
    }



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
            //Console.WriteLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("인벤토리");
            Console.ResetColor();  // 색상 초기화
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");

            
            Console.WriteLine("\n[아이템 목록]");
            Console.WriteLine();
            if (Items.Count == 0) Console.WriteLine("아이템이 비어있습니다");

            // 아이템 목록 출력
            for (int i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                string equippedMark = character.EquippedItem != null && character.EquippedItem.Name == item.Name ? "[E]" : "";
                string bonus = string.Empty;

                if (item.AttackBonus > 0)
                    bonus += $" 공격력 +{item.AttackBonus}";
                if (item.ArmorBonus > 0)
                    bonus += $" 방어력 +{item.ArmorBonus}";

                // 아이템 정보 출력
                Console.WriteLine($"- {equippedMark}{item.Name} |{bonus} | {item.Description}");
            }

            Console.WriteLine();
            Console.WriteLine("\n1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            string actionChoice = Console.ReadLine();

            if (actionChoice == "1") // 장착 관리
            {
                ShowEquipMenu(character);
            }
        }

        // 장착 관리 화면 출력
        public void ShowEquipMenu(Character character)
        {
            //Console.WriteLine();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.ResetColor();  // 색상 초기화
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();

            if (Items.Count == 0)
            {
                Console.WriteLine("아이템이 없습니다. 장착할 아이템이 없습니다.");
            }
            else
            {
                Console.WriteLine("\n[아이템 목록]");
                for (int i = 0; i < Items.Count; i++)
                {
                    var item = Items[i];
                    string equippedMark = character.EquippedItem != null && character.EquippedItem.Name == item.Name ? "[E]" : "";
                    string bonus = string.Empty;

                    if (item.AttackBonus > 0)
                        bonus += $" 공격력 +{item.AttackBonus}";
                    if (item.ArmorBonus > 0)
                        bonus += $" 방어력 +{item.ArmorBonus}";

                    Console.WriteLine($"{i + 1}. {equippedMark}{item.Name} |{bonus} | {item.Description}");
                }
            }

            Console.WriteLine("\n0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요: ");
            string equipChoice = Console.ReadLine();

            if (equipChoice == "0")
            {
                return;
            }
            else if (int.TryParse(equipChoice, out int itemIndex) && itemIndex > 0 && itemIndex <= Items.Count)
            {
                var selectedItem = Items[itemIndex - 1];
                character.EquippedItem = selectedItem;  // 선택한 아이템을 장착
                Console.WriteLine($"{character.Name}이(가) {selectedItem.Name}을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine("잘못된 선택입니다.");
            }
        }
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

    /*
    // 스테이지 클래스
    public class Stage
    {
        private ICharacter player; // 플레이어
        private ICharacter monster; // 몬스터
        private List<IItem> rewards; // 보상 아이템들

        // 이벤트 델리게이트 정의
        public delegate void GameEvent(ICharacter character);
        public event GameEvent OnCharacterDeath; // 캐릭터가 죽었을 때 발생하는 이벤트

        public Stage(ICharacter player, ICharacter monster, List<IItem> rewards)
        {
            this.player = player;
            this.monster = monster;
            this.rewards = rewards;
            OnCharacterDeath += StageClear; // 캐릭터가 죽었을 때 StageClear 메서드 호출
        }

        // 스테이지 시작 메서드
        public void Start()
        {
            Console.WriteLine($"스테이지 시작! 플레이어 정보: 체력({player.Health}), 공격력({player.Attack})");
            Console.WriteLine($"몬스터 정보: 이름({monster.Name}), 체력({monster.Health}), 공격력({monster.Attack})");
            Console.WriteLine("----------------------------------------------------");

            while (!player.IsDead && !monster.IsDead) // 플레이어나 몬스터가 죽을 때까지 반복
            {
                // 플레이어의 턴
                Console.WriteLine($"{player.Name}의 턴!");
                monster.TakeDamage(player.Attack);
                Console.WriteLine();
                Thread.Sleep(1000);  // 턴 사이에 1초 대기

                if (monster.IsDead) break;  // 몬스터가 죽었다면 턴 종료

                // 몬스터의 턴
                Console.WriteLine($"{monster.Name}의 턴!");
                player.TakeDamage(monster.Attack);
                Console.WriteLine();
                Thread.Sleep(1000);  // 턴 사이에 1초 대기
            }

            // 플레이어나 몬스터가 죽었을 때 이벤트 호출
            if (player.IsDead)
            {
                OnCharacterDeath?.Invoke(player);
            }
            else if (monster.IsDead)
            {
                OnCharacterDeath?.Invoke(monster);
            }
        }

        // 스테이지 클리어 메서드
        private void StageClear(ICharacter character)
        {
            if (character is Monster)
            {
                Console.WriteLine($"스테이지 클리어! {character.Name}를 물리쳤습니다!");

                // 플레이어에게 아이템 보상
                if (rewards != null)
                {
                    int i = 1;
                    Console.WriteLine("아래의 보상 아이템 중 하나를 선택하여 사용할 수 있습니다:");
                    foreach (var item in rewards)
                    {
                        Console.WriteLine($"{i}. " + item.Name);
                        i++;
                    }

                    Console.WriteLine("사용할 아이템 이름을 입력하세요:");
                    string input = Console.ReadLine();

                    // 선택된 아이템 사용
                    IItem selectedItem = rewards.Find(item => item.Name == input);
                    if (selectedItem != null)
                    {
                        selectedItem.Use((Warrior)player);
                    }
                }

                //player.Health = 100; // 각 스테이지마다 체력 회복
            }
            else
            {
                Console.WriteLine("게임 오버! 패배했습니다...");
            }
        }
    }
    */


    public class Shop
    {
        public List<Item> ItemsSale { get; private set; }

        public Shop()
        {
            ItemsSale = new List<Item>
            {
                new Item("수련자 갑옷", 0, 5, "수련에 도움을 주는 갑옷입니다.", 1000),
                new Item("무쇠갑옷", 0, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", 100),
                new Item("스파르타의 갑옷", 0, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", 3500),
                new Item("낡은 검", 2, 0, "쉽게 볼 수 있는 낡은 검입니다.", 600),
                new Item("청동 도끼", 5, 0, "어디선가 사용됐던 거 같은 도끼입니다.", 1500),
                new Item("스파르타의 창", 7, 0, "스파르타의 전사들이 사용했다는 전설의 창입니다.", 200)
            };
        }

        public void ShowShop(Character character)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("상점");
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

                    Console.WriteLine($"- {item.Name} | {bonus} | {item.Description} | {status}");
                }


                Console.WriteLine("\n1. 아이템 구매");
                Console.WriteLine("0. 나가기");
                Console.Write("원하시는 행동을 입력해주세요. ");
                Console.Write(">> ");

                string action = Console.ReadLine();

                if (action == "1") PurchaseItem(character);
                else if (action == "0") break;
            }
        }

        // 아이템 구매 처리
        private void PurchaseItem(Character character)
        {
            Console.Clear ();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("상점 - 구매");
            Console.ResetColor();  // 색상 초기화
            Console.WriteLine();
            string input;
            int inputnum;
            int i = 1;
            foreach (var item in ItemsSale)
            {
                string status = item.Price == 0 ? "구매완료" : $"{item.Price} G";
                string bonus = item.ArmorBonus > 0 ? $"방어력 +{item.ArmorBonus}" : $"공격력 +{item.AttackBonus}";

                Console.WriteLine($"{i}. {item.Name} | {bonus} | {item.Description} | {status}");
                i++;
            }
            do
            {                 
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력하세요. ");
                Console.Write(">> ");

                input = Console.ReadLine();
                inputnum = int.Parse(input);
                if (input == "0")
                {
                    break;
                }

                if (int.TryParse(input, out int itemIndex) && itemIndex > 0 && itemIndex <= ItemsSale.Count)
                {
                    var selectedItem = ItemsSale[itemIndex - 1];

                    if (selectedItem.Price == 0)
                    {
                        Console.WriteLine($"이 아이템은 이미 구매되었습니다: {selectedItem.Name}");
                        return;
                    }

                    if (character.Gold >= selectedItem.Price)
                    {
                        character.Gold -= selectedItem.Price;  // 구매 시 금액 차감
                        Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다.");
                    }
                    else
                    {
                        Console.WriteLine("골드가 부족합니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 선택입니다.");
                }
            }
            while (true);
        }


    }

    


    // 메인 메서드
    class Game
    {
        static void Main(string[] args)
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            Console.WriteLine();
            JobSelection jobSelection = new JobSelection();
            ICharacter player = jobSelection.SelectCharacter();
            var inventory = new Inventory();
            var shop = new Shop();

            while (true)
            {
                Console.Clear();
                // 메뉴 출력
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("0. 종료");
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                string input = Console.ReadLine();

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
                        shop.ShowShop((Character)player);
                        // 상점 관련 로직 추가
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