// USED SOURCES:
// http://www.youtube.com/watch?v=bQexyufgclY
// http://stackoverflow.com/questions/10957854/how-to-generate-random-color
// http://msdn.microsoft.com/en-us/library/8hftfeyw(v=vs.110).aspx

namespace _11FallingRocks
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class FallingRocks
    {
        static int GetSpeed(int delay)
        {
            if (delay == 100)
            {
                return 5;
            }
            else if (delay == 150)
            {
                return 4;
            }
            else if (delay == 200)
            {
                return 3;
            }
            else if (delay == 250)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        static void PrintOnPosition(int x, int y, string c, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(c);

        }

        static void PrintStringOnPosition(int x, int y, string str, ConsoleColor color = ConsoleColor.Gray)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(str);

        }

        struct Object 
        {
            public int x;
            public int y;
            public string view;
            public ConsoleColor color;
        }

        static void Main()
        {
            int playfieldWidth = 30;
            int livesCount = 5;
            int score = 0;
            int delay = 150;
            int maxDelay = 300;
            int minDelay = 100;
            int acceleration = 50;

            Console.Title = "Falling Rocks";
            Console.BufferHeight = Console.WindowHeight;
            Console.BufferWidth = Console.WindowWidth = 75;

            Object dwarf = new Object();
            dwarf.x = playfieldWidth / 2;
            dwarf.y = Console.WindowHeight - 1;
            dwarf.view = "(0)";
            dwarf.color = ConsoleColor.Yellow;

            string[] rockChars = new string[] {"^", "@", "*", "&", "+", "%", "$", "#", "!", ".", ";", "-"};
            string[] rockColors = ConsoleColor.GetNames(typeof(ConsoleColor));

            Random randomGenerator = new Random();
            List<Object> rocks = new List<Object>();
            
            while (true)
            {
                bool hitted = false;
                Object newRock = new Object();

                string colorName = rockColors[randomGenerator.Next(0, rockColors.Length)];
                if (colorName != "Black")
                {
                    ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName);
                    newRock.color = color;
                }

                newRock.x = randomGenerator.Next(0, playfieldWidth);
                newRock.y = 0;
                int rockSymbolNumber = randomGenerator.Next(0, rockChars.Length);
                newRock.view = rockChars[rockSymbolNumber];
                rocks.Add(newRock);

                if (Console.KeyAvailable) // Checking for pressed key
                {
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);

                    // With this loop i'm clearing bufer from other pressed keys to prevent my dwarf from delay
                    while (Console.KeyAvailable)
                    {
                        Console.ReadKey(true);
                    }

                    // Move our dwarf left or right
                    if (pressedKey.Key == ConsoleKey.LeftArrow)
                    {
                        if (dwarf.x - 1 >= 0)
                        {
                            dwarf.x--;
                        }

                    }
                    else if (pressedKey.Key == ConsoleKey.RightArrow)
                    {
                        if (dwarf.x + 1 < playfieldWidth)
                        {
                            dwarf.x++;
                        }
                    }
                    else if (pressedKey.Key == ConsoleKey.Spacebar)
                    {
                        Console.ReadKey();
                    }
                    else if (pressedKey.Key == ConsoleKey.Add)
                    {
                        if (delay > minDelay)
                        {
                            delay -= acceleration;
                        }
                        
                    }
                    else if (pressedKey.Key == ConsoleKey.Subtract)
                    {
                        if (delay < maxDelay)
                        {
                            delay += acceleration;
                        }
                    }
                }

                // Move rocks
                List<Object> newListOfRocks = new List<Object>();

                for (int i = 0; i < rocks.Count; i++)
                {
                    Object oldRock = rocks[i];
                    Object newRockY = new Object();
                    newRockY.x = oldRock.x;
                    newRockY.y = oldRock.y + 1;
                    newRockY.view = oldRock.view;
                    newRockY.color = oldRock.color;

                    if (newRockY.y == dwarf.y && (newRockY.x == dwarf.x || newRockY.x == dwarf.x + 1 || newRockY.x == dwarf.x + 2))
                    {
                        if (newRockY.view == "+")
                        {
                            score++;
                        }
                        else if (newRockY.view == "-")
                        {
                            if (score > 0)
                            {
                                score--;
                            }

                        }
                        else
                        {
                            livesCount--;
                            hitted = true;
                        }
                    }

                    if (newRockY.y < Console.WindowHeight)
                    {
                        newListOfRocks.Add(newRockY);
                    }
                }

                rocks = newListOfRocks;

                // Clear the console
                Console.Clear();

                //Redraw playfield
                foreach (Object rock in rocks)
                {
                    PrintOnPosition(rock.x, rock.y, rock.view, rock.color);
                }

                if (hitted)
                {
                    if (livesCount > 0)
                    {
                        Console.Beep();
                    }
                    else if (livesCount == 0)
                    {
                        Console.Beep(800, 700);
                    }

                    rocks.Clear();
                    PrintOnPosition(dwarf.x, dwarf.y, dwarf.view, ConsoleColor.Red);

                    if (livesCount <= 0)
                    {
                        PrintStringOnPosition(playfieldWidth + 5, 3, "Lives: " + livesCount, ConsoleColor.White);
                        PrintStringOnPosition(playfieldWidth + 5, 4, "Score: " + score, ConsoleColor.White);
                        PrintStringOnPosition(playfieldWidth + 5, 6, "GAME OVER!!!", ConsoleColor.Red);
                        PrintStringOnPosition(playfieldWidth + 5, 8, "Press [enter] to exit", ConsoleColor.Red);
                        Console.ReadLine();
                        return;
                    }

                }
                else
                {
                    PrintOnPosition(dwarf.x, dwarf.y, dwarf.view, dwarf.color);
                }


                // Draw info
                //for (int i = 0; i < Console.WindowHeight; i++)
                //{
                //    PrintStringOnPosition(32, i, "|");
                //}
                PrintStringOnPosition(playfieldWidth + 5, 0, "use <- and -> to play", ConsoleColor.White);
                PrintStringOnPosition(playfieldWidth + 5, 2, "All symbols except + and - kills you", ConsoleColor.White);
                PrintStringOnPosition(playfieldWidth + 5, 4, "+ increases your score with 1", ConsoleColor.White);
                PrintStringOnPosition(playfieldWidth + 5, 6, "- decreases your score with 1", ConsoleColor.White);
                PrintStringOnPosition(playfieldWidth + 5, 8, "You can pause the game with Spacebar", ConsoleColor.White);
                PrintStringOnPosition(playfieldWidth + 5, 10, "You can change the speed with + and -", ConsoleColor.White);

                PrintStringOnPosition(playfieldWidth + 5, 13, "Lives: " + livesCount, ConsoleColor.Green);
                PrintStringOnPosition(playfieldWidth + 5, 14, "Score: " + score, ConsoleColor.Green);
                PrintStringOnPosition(playfieldWidth + 5, 15, "Speed: " + GetSpeed(delay), ConsoleColor.Green);

                // Slow down program 
                Thread.Sleep(delay);
            }
        }
    }
}
