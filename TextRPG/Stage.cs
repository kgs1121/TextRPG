using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Stage
    {
        public int StageNumber { get; set; }
        public Monster Monster { get; set; }
        private Random randomG = new Random();
        private Random randomA = new Random();
        private int currentStageNumber;
        public Stage currentStage;

        public Stage(int stageNumber)
        {
            StageNumber = stageNumber;
        }


        private Monster CreateMonsterForStage(int stageNumber)
        {
            // 스테이지에 따라 몬스터의 종류와 능력치를 다르게 설정
            string name = "";
            int health = 0;
            int attack = 0;
            int armor = 0;
            switch ((stageNumber - 1) / 3)
            {
                case 0:
                    name = $"고블린{stageNumber}";
                    health = 50;
                    attack = 10;
                    armor = 5;
                    break;
                case 1:
                    name = $"오크{stageNumber - 3}";
                    health = 100;
                    attack = 15;
                    armor = 7;
                    break;
                case 2:
                    name = $"트롤{stageNumber - 6}";
                    health = 150;
                    attack = 20;
                    armor = 9;
                    break;
                default:
                    name = $"드래곤";
                    health = 200;
                    attack = 30;
                    armor = 15;
                    break;
            }

            // 몬스터 생성
            int monsterLevel = stageNumber; // 몬스터 레벨은 스테이지와 같음
            Monster monster = new Monster(name, health, monsterLevel, attack, armor);
            return monster;
        }


        public void DisplayStageInfo()
        {
            string input;
            bool isLoop = false;
            do
            {
                Console.Clear();
                Console.WriteLine($"스테이지 {currentStageNumber} 정보\n");
                Console.WriteLine($"몬스터: {Monster.Name}");
                Console.WriteLine($"레벨: {Monster.Level}");
                Console.WriteLine($"체력: {Monster.Health}");
                Console.WriteLine($"공격력: {Monster.Attack}");
                Console.WriteLine($"방어력: {Monster.Armor}");
                Console.WriteLine("\n 0. 나가기\n");
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("\n 원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                input = Console.ReadLine();
                isLoop = true;
                if (input == "0") return;
            }
            while (input != "0");
        }


        public void ShowStageNum(Character character)
        {
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("스테이지 선택");
                Console.ResetColor();
                Console.WriteLine();
                for (int i = 1; i < 11; i++)
                {
                    //if (clearStage) Console.WriteLine($"{i}. Stage {i} (clear)");
                    Console.WriteLine($"{i}. Stage {i}"); //else
                }
                Console.WriteLine("\n0. 나가기");
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();

                if (input == "0") return;
                else if (int.TryParse(input, out int Stagenum) && (Stagenum > 0 && Stagenum < 12))
                {
                    currentStageNumber = Stagenum;
                    Monster = CreateMonsterForStage(currentStageNumber);
                    EnterStage(character);
                }
                else isLoop = true;
            }
        }


        public void EnterStage(Character character)
        {
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Stage{currentStageNumber}");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. 싸운다");
                Console.WriteLine("2. 스테이지 정보");
                Console.WriteLine("0. 나간다");
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") return;
                else if (int.TryParse(input, out int Fightnum) && Fightnum == 1)
                {
                    Fight(character);
                }
                else if (int.TryParse(input, out int infonum) && infonum == 2)
                {

                    DisplayStageInfo();
                    continue;
                }
            }
        }


        public void Fight(Character character)
        {
            bool isLoop = false;
            int retrynum = 1;
            int monsterMaxHP = Monster.Health;
            
            Console.WriteLine($"당신의 체력: {character.NowHealth}, 몬스터의 체력: {Monster.Health}");
            while (true)
            {
                // 전투 루프
                while (character.NowHealth > 0 && Monster.Health > 0)
                {

                    int CAttack = Math.Max(0, character.Attack - Monster.Armor);   // 캐릭터의 공격값
                    int MAttack = Math.Max(0, Monster.Attack - character.Armor); // 몬스터의 공격값
                    int MrandAttack = randomA.Next(MAttack, MAttack + 10);  // 몬스터의 공격 랜덤값
                    int CrandAttack = randomA.Next(CAttack, CAttack + 10);  // 캐릭터의 공격 랜덤값

                    // 캐릭터 공격
                    Monster.TakeDamage(CrandAttack);
                    // 몬스터 공격
                    character.TakeDamage(MrandAttack);
                }

                // 전투 결과 출력                ///////////////////////////////////////////////////////////////////////
                Monster.Health = monsterMaxHP;  // 몬스터 체력 초기화
                
                if (!character.IsDead)
                {
                    int getGold = randomG.Next(currentStageNumber * 100, currentStageNumber * 200);
                    character.Gold += getGold;
                    Console.WriteLine($"스테이지{currentStageNumber} 클리어!");
                    Console.WriteLine($"골드 획득: {getGold}");
                }
                else
                {

                }

                Console.WriteLine("\n1. 다시하기");
                Console.WriteLine("0. 나가기");
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("\n원하시는 행동을 입력하세요.");
                Console.WriteLine(">> ");
                string rtyinput = Console.ReadLine();
                if (rtyinput == "1")
                {
                    retrynum++;
                    continue;
                }
                else if (rtyinput == "0") return;
                else isLoop = true;
            }
        }
    }
}
