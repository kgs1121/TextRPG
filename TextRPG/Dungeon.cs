using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class Dungeon
    {
        public int stageNum = 1;

        public Dungeon()
        {

        }

        public void ShowDungeon(Character character)
        {
            bool isLoop = false;
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("던전");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("1. 스테이지 선택");
                Console.WriteLine("0. 나간다.");
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

                if (input == "1")
                {
                    // ShowStageNum 호출 (character를 매개변수로 넘김)
                    Stage currentStage = new Stage(stageNum);  // Stage 객체를 생성
                    currentStage.ShowStageNum(character);
                    continue;
                }
                else if (input == "0")
                {
                    return;
                }
                else
                {
                    isLoop = true;
                }
            }
        }
    }
}
