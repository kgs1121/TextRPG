using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    // 캐릭터 클래스
    public class Character : ICharacter
    {
        public string Name { get; }
        public Job CharacterJob { get; }
        public int MaxHealth { get; set; }
        public int NowHealth { get; set; }
        public int Attack { get; set; }
        public int Armor { get; set; }
        public int Gold { get; set; }
        public int Level = 1;
        public int NowEx { get; set; }
        public double EXP { get; set; }
        public IItem EquippedItem { get; set; }  // 장착 중인 아이템
        public List<IItem> EquippedItems { get; set; }

        private Random randomstat = new Random();

        public bool IsDead => NowHealth <= 0;

        public bool islevelup;

        // 생성자에서 직업을 선택하고, 직업에 맞는 기본 속성 설정
        public Character(string name, Job job)
        {
            Name = name;
            CharacterJob = job;
            EquippedItems = new List<IItem>();

            // 직업별 기본 속성 설정
            switch (job)
            {
                case Job.전사:
                    MaxHealth = 120;
                    NowHealth = MaxHealth;
                    Attack = 15;
                    Armor = 10;
                    Gold = 0;
                    EquippedItem = null;
                    break;
                case Job.마법사:
                    MaxHealth = 80;
                    NowHealth = MaxHealth;
                    Attack = 25;
                    Armor = 3;
                    Gold = 1500;
                    EquippedItem = null;
                    break;
                case Job.궁수:
                    MaxHealth = 90;
                    NowHealth = MaxHealth;
                    Attack = 20;
                    Armor = 5;
                    Gold = 1500;
                    EquippedItem = null;
                    break;
            }
        }


        public void TakeDamage(int damage)  // 캐릭터 받는 데미지 출력
        {
            
            NowHealth -= damage;
            if (IsDead) Console.WriteLine($"{Name}이(가) 죽었습니다.");
            else
            {
                Console.Write($"{Name}이(가) {damage}의 데미지를 받았습니다. ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Name} 남은 체력: {NowHealth}");
                Console.ResetColor();
            }
        }


        // 아이템 장착 및 효과 처리
        public void EquipItem(IItem item)
        {
            // 기존에 장착된 아이템의 효과 제거
            if (EquippedItem != null)
            {
                Attack -= EquippedItem.AttackBonus;
                Armor -= EquippedItem.ArmorBonus;
            }

            // 새로운 아이템 장착
            EquippedItem = item;

            // 새로운 아이템의 효과 추가
            Attack += item.AttackBonus;
            Armor += item.ArmorBonus;
        }


        // 상태 보기 메서드
        public void ShowStatus()  // 상태 보기 실행 함수
        {
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Statusconsol();
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string choice = Console.ReadLine();
                if (choice == "0")
                {
                    break; // 0을 입력하면 메뉴로 돌아가도록 종료
                }
                else
                {
                    isLoop = true;
                    continue;
                }
            }
        }


        public void Statusconsol()  // 상태 보기 함수
        {
            int totalAttack = Attack;
            int totalArmor = Armor;

            // 장착한 아이템의 효과 반영
            int attackBonus = 0;
            int armorBonus = 0;



            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("상태 보기");
            Console.ResetColor();
            if(Level < 10) Console.WriteLine($"Lv. 0{Level} : {EXP} %");
            else Console.WriteLine($"Lv. {Level} : {EXP} %");
            Console.WriteLine($"{Name} ({CharacterJob})");
            Console.WriteLine($"체력 : {NowHealth}");

            if (EquippedItems.Count > 0)
            {
                foreach (var item in EquippedItems)
                {
                    totalAttack += item.AttackBonus;
                    totalArmor += item.ArmorBonus;
                    attackBonus += item.AttackBonus;
                    armorBonus += item.ArmorBonus;
                }
            }
            // 공격력, 방어력 계산 후 아이템 효과가 있으면 (+x) 표시
            if (EquippedItems.Count > 0)
            {
                Console.WriteLine($"공격력 : {totalAttack} (+{attackBonus})");
                Console.WriteLine($"방어력 : {totalArmor} (+{armorBonus})");
            }
            else
            {
                Console.WriteLine($"공격력 : {totalAttack}");
                Console.WriteLine($"방어력 : {totalArmor}");
            }

            Console.WriteLine($"Gold : {Gold} G");
        }


        public int NeedEx()  //레벨업 조건
        {
            if(Level == 1) return 40 * Level;
            else if (Level > 1 && Level < 11) return 100 * Level;
            else return 200 * Level;
        }

        public void AddEx(int xp)   // 경험치 획득
        {
            islevelup = false;
            NowEx += xp;
            EXP = (NowEx / (double)NeedEx()) * 100;
            if (EXP >= 100)
            {
                islevelup = true;
                NowEx = 0;
                EXP = 0;
                Levelup();
            }
        }

        public void Levelup()  // 레벨업 적용
        {
            Level++;
            MaxHealth += randomstat.Next(5, 11);
            Attack += randomstat.Next(1, 6);
            Armor += randomstat.Next(1, 5);
        }


        public void Rset()  // 휴식
        {
            if (Gold >= 50)
            {
                Game.isRest = true;
                Gold -= 50;
                NowHealth = MaxHealth;
            }
            else Console.WriteLine("골드가 부족합니다");
        }
    }
}
