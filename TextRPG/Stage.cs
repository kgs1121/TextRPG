using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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


        private Monster CreateMonsterForStage(int stageNumber)  // 스테이지 별 몬스터 생성
        {
            // 스테이지에 따라 몬스터의 종류와 능력치를 다르게 설정
            string name = "";
            int health = 0;
            int attack = 0;
            int armor = 0;
            switch ((stageNumber - 1) / 3)  // 3스테이지 마다 몬스터 변경
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


        public void DisplayStageInfo()  // 스테이지 정보 출력
        {
            string input;
            bool isLoop = false;
            while(true)
            {
                Console.Clear();
                Console.WriteLine($"스테이지 {currentStageNumber} 정보\n");
                Console.WriteLine($"몬스터: {Monster.Name}");
                Console.WriteLine($"레벨: {Monster.Level}");
                Console.WriteLine($"체력: {Monster.Health}");
                Console.WriteLine($"공격력: {Monster.Attack}");
                Console.WriteLine($"방어력: {Monster.Armor}");
                Console.WriteLine("\n 0. 나가기");
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
                input = Console.ReadLine();
                isLoop = true;
                if (input == "0") return;
            }
        }
        

        public void ShowStageNum(Character character)   // 스테이지 선택 메뉴
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
                string input = Console.ReadLine();

                if (input == "0") return;
                else if (int.TryParse(input, out int Stagenum) && (Stagenum > 0 && Stagenum < 12))
                {
                    currentStageNumber = Stagenum;
                    Monster = CreateMonsterForStage(currentStageNumber);
                    EnterStage(character);
                    if (character.IsDead) return;
                }
                else isLoop = true;
            }
        }


        public void EnterStage(Character character)   // 스테이지 입장
        {
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Stage{currentStageNumber}");
                Console.ResetColor();
                Console.WriteLine($"\n현재 체력: {character.NowHealth}");
                Console.WriteLine();
                Console.WriteLine("1. 싸운다");
                Console.WriteLine("2. 스테이지 정보");
                Console.WriteLine("\n0. 나간다");
                Console.WriteLine();
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                if (Game.isRest)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("체력을 회복했습니다");
                    Console.ResetColor();
                    Game.isRest = false;
                }
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();
                if (input == "0") return;
                else if (int.TryParse(input, out int Fightnum) && Fightnum == 1)
                {
                    Fight(character);
                    if (character.IsDead) return;  // 캐릭터 사밍 시 나가기
                }
                else if (int.TryParse(input, out int infonum) && infonum == 2)
                {
                    DisplayStageInfo();
                    continue;
                }
                else isLoop = true;
            }
        }


        public void Fight(Character character)   // 전투 로직
        {
            Console.Clear();
            bool isLoop = false;   // 잘못입력 시 Error메세지 출력
            int retrynum = 1;
            int monsterMaxHP = Monster.Health;
            int getEx = 0;
            Console.WriteLine($"당신의 체력: {character.NowHealth}, 몬스터의 체력: {Monster.Health}");
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Stage{currentStageNumber}");
                Console.ResetColor();
                // 전투 루프
                if (!isLoop)
                {
                    while (character.NowHealth > 0 && Monster.Health > 0)
                    {

                        Console.WriteLine();
                        int CAttack = Math.Max(0, character.Attack - Monster.Armor);   // 캐릭터의 공격값
                        int MAttack = Math.Max(0, Monster.Attack - character.Armor); // 몬스터의 공격값
                        int MrandAttack = randomA.Next(MAttack, MAttack + 10);  // 몬스터의 공격 랜덤값
                        int CrandAttack = randomA.Next(CAttack, CAttack + 10);  // 캐릭터의 공격 랜덤값

                        // 캐릭터 공격
                        Monster.TakeDamage(CrandAttack);
                        // 몬스터 공격
                        character.TakeDamage(MrandAttack);

                    }
                }
                // 전투 결과 출력
                Monster.Health = monsterMaxHP;  // 몬스터 체력 초기화
                
                if (!character.IsDead)
                {
                    int getGold = randomG.Next(currentStageNumber * 100, currentStageNumber * 200);
                    
                    getEx = currentStageNumber * 10 * retrynum;
                    double getExp = ((character.NowEx + getEx) / (double)character.NeedEx()) * 100;

                    Console.WriteLine($"스테이지{currentStageNumber} 클리어!");
                    
                    Console.WriteLine("\n[탐험 결과]");
                    Console.WriteLine($"체력 {character.NowHealth}");
                    if (getExp < 100) Console.WriteLine($"Level {character.Level} {character.EXP.ToString("F1")} % -> {getExp.ToString("F1")} %");
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("축하드립니다 레벨업하셨습니다!");
                        Console.ResetColor();
                        Console.WriteLine($"Level {character.Level} {character.EXP.ToString("F1")} % -> Level {character.Level + 1} 0 %");
                    }
                    character.AddEx(getEx);
                    Console.WriteLine($"보유 골드: {character.Gold}G -> {character.Gold + getGold}G");
                    character.Gold += getGold;
                }
                else if( character.IsDead )  // 캐릭터 사망 시 나가기
                {
                    SaveGameClass saveGame = new SaveGameClass();
                    saveGame.ResetSaveData();
                    return;
                }

                Console.WriteLine("\n1. 다시하기");
                Console.WriteLine("2. 휴식하기 (100G)");
                Console.WriteLine("\n0. 나가기");
                Console.WriteLine();
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }
                Console.WriteLine("원하시는 행동을 입력하세요.");
                Console.Write(">> ");
                string rtyinput = Console.ReadLine();
                if (rtyinput == "1")
                {
                    retrynum++;
                    Console.Clear();
                }
                else if (rtyinput == "2")
                {
                    character.Rset();
                    break;
                }
                else if (rtyinput == "0") return;
                else
                {
                    isLoop = true;
                }
            }
        }
    }
}
