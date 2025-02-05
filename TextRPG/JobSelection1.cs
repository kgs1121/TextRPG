using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG
{
    public class JobSelection   //직업 선택
    {
        // 직업을 선택하고 캐릭터를 반환하는 메서드
        public Character SelectCharacter()
        {
            Character player = null; // player 변수를 null로 초기화
            string jobChoice = null;
            bool isLoop = false;
            while (true)
            {
                // 화면 초기 출력 (단 한 번만 호출)
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("캐릭터 생성");
                Console.ResetColor();

                // 직업 목록을 출력하는 루프
                foreach (Job job in Enum.GetValues(typeof(Job)))
                {
                    Console.WriteLine($"{(int)job}. {job}");
                }
                Console.WriteLine();

                // 잘못된 입력 시 메시지를 출력
                if (isLoop)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                    Console.ResetColor();
                    isLoop = false;
                }

                Console.WriteLine("\n원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                jobChoice = Console.ReadLine();

                // 유효한 직업 번호가 입력되면 루프 종료
                if (int.TryParse(jobChoice, out int validJobChoice) && Enum.IsDefined(typeof(Job), validJobChoice))
                {
                    break;
                }
                else isLoop = true;
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
}
