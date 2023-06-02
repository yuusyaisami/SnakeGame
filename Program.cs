using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Program
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        static int[][] Map = {
            new int[15]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
            new int[15]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        };
        static int GameTimer,HeadDirectionIndex, NowScore, MaxScore;
        struct PLAYER
        {
            public int[] X;
            public int[] Y;
            public int Snakelengh;
        }
        struct SCENE
        {
            public int game;
            public int menu;
        }
        static PLAYER player = new PLAYER();
        static SCENE scene = new SCENE();
        static void Main(string[] args)
        {
            init();
            for(; ; )
            {
                update();
                controller();
                draw();
                Thread.Sleep(16);
            }
        }
        static void init()
        {
            Map[5][8] = 3;
            GameTimer = -1;
            HeadDirectionIndex = 1;
            player.X = new int[1000];
            player.Y = new int[1000];
            player.Snakelengh = 0;
            NowScore = 0;
            scene.game = 0;
            scene.menu = 1;
            //player.X の -1は最終地点のチェックに使う
            for (int i = 0; i < 1000; i++) 
            {
                player.X[i] = -1;
                player.Y[i] = -1;
            }
        }
        static void update()
        {
            if(GameTimer % 5 == 0)
            {
                if (CollisionBlock())
                {
                    GameOver();
                }
                else
                {
                    if (CollisionFeed())
                    {
                        player.Snakelengh++;
                        NowScore++;
                        CreateFood();
                    }
                    //現在の座標を左に挿入する
                    InsertNowPosition();

                    MoveSnake();
                    SetSnakeLengh();
                }
            }
            if(GameTimer == -1)
            {

                NowScore = 0;
                Map[1][1] = 4;
                HeadDirectionIndex = 1;
            }
            GameTimer++;
        }
        static void CreateFood()
        {
            int random1, random2;
            Random random = new Random();
            for (; ; )
            {
                random1 = random.Next() % 9;
                random2 = random.Next() % 14;
                if (Map[random1][random2] != 4 && Map[random1][random2] != 1 && Map[random1][random2] != 2 && Map[random1][random2] != 3)
                {
                    Map[random1][random2] = 3;
                    break;
                }
            }
        }
        static void GameOver()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Map[i][j] != 1)
                    {
                        Map[i][j] = 0;
                    }
                }
            }
            Map[5][8] = 3;
            GameTimer = -1;
            HeadDirectionIndex = 1;
            player.Snakelengh = 0;
            for (int i = 0; i < 1000; i++)
            {
                player.X[i] = -1;
                player.Y[i] = -1;
            }
            if(MaxScore < NowScore)
            {
                MaxScore = NowScore;
            }
            scene.game = 0;
            scene.menu = 1;
        }
        static void controller()
        {
            if ((GetAsyncKeyState((int)ConsoleKey.W) & 0x8000) != 0 && HeadDirectionIndex != 3)
            {
                HeadDirectionIndex = 0;
            }
            if ((GetAsyncKeyState((int)ConsoleKey.D) & 0x8000) != 0  && HeadDirectionIndex != 2)
            {
                HeadDirectionIndex = 1;
            }
            if ((GetAsyncKeyState((int)ConsoleKey.A) & 0x8000) != 0 && HeadDirectionIndex != 1)
            {
                HeadDirectionIndex = 2;
            }
            if ((GetAsyncKeyState((int)ConsoleKey.S) & 0x8000) != 0 && HeadDirectionIndex != 0)
            {
                HeadDirectionIndex = 3;
            }
            if ((GetAsyncKeyState((int)ConsoleKey.Enter) & 0x8000) != 0)
            {
                scene.menu = 0;
                scene.game = 1;
            }
        }
        static void draw()
        {
            if (scene.game == 1)
            {
                string linestring = "";
                Console.Clear();
                for (int i = 0; i < 9; i++)
                {
                    linestring = "";
                    for (int j = 0; j < 15; j++)
                    {
                        if (Map[i][j] == 0)
                        {
                            linestring += "・";
                        }
                        if (Map[i][j] == 1)
                        {
                            linestring += "[]";
                        }
                        if (Map[i][j] == 2)
                        {
                            linestring += "()";
                        }
                        if (Map[i][j] == 3)
                        {
                            linestring += "ｆ";
                        }
                        if (Map[i][j] == 4)
                        {
                            linestring += "ｓ";
                        }
                    }
                    Console.WriteLine(linestring);
                }
                Console.WriteLine("snakelengh : " + player.Snakelengh);
                Console.WriteLine("GameCount  : " + GameTimer);
            }
            else if(scene.menu == 1)
            {
                Console.Clear();
                Console.WriteLine("score  : " + MaxScore);
            }
        }
        static void InsertNowPosition()
        {
            for(int i  = 998; i != 0; i--)
            {
                player.X[i] = player.X[i - 1];
                player.Y[i] = player.Y[i - 1];
            }
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Map[i][j] == 4)
                    {
                        player.X[0] = j;
                        player.Y[0] = i;
                    }
                }
            }
        }
        static void MoveSnake()
        {
            int ESC = 0;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Map[i][j] == 4)
                    {
                        if (HeadDirectionIndex == 0)
                        {
                            Map[i][j] = 0;
                            Map[i - 1][j] = 4;
                            ESC = 1;
                            break;
                        }
                        if (HeadDirectionIndex == 1)
                        {
                            Map[i][j] = 0;
                            Map[i][j + 1] = 4;
                            ESC = 1;
                            break;
                        }
                        if (HeadDirectionIndex == 2)
                        {
                            Map[i][j] = 0;
                            Map[i][j - 1] = 4;
                            ESC = 1;
                            break;
                        }
                        if (HeadDirectionIndex == 3)
                        {
                            Map[i][j] = 0;
                            Map[i + 1][j] = 4;
                            ESC = 1;
                            break;
                        }
                    }
                }
                if(ESC == 1)
                {
                    break;
                }
            }
        }
        static void SetSnakeLengh()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Map[i][j] == 2)
                    {
                        Map[i][j] = 0;
                    }
                }
            }

            for (int i = 0; i < player.Snakelengh;i++)
            {
                Map[player.Y[i]][player.X[i]] = 2;
            }
        }
        static bool CollisionBlock()
        {
            for(int i = 0; i < 9;i++)
            {
                for(int j = 0; j < 14; j++)
                {
                    if (Map[i][j] == 4)
                    {
                        if (HeadDirectionIndex == 0)
                        {
                            if (Map[i - 1][j] == 1 || Map[i - 1][j] == 2)
                            {
                                return true;
                            }
                        }
                        if (HeadDirectionIndex == 1)
                        {
                            if (Map[i][j + 1] == 1 || Map[i][j + 1] == 2)
                            {
                                return true;
                            }
                        }
                        if (HeadDirectionIndex == 2)
                        {
                            if (Map[i][j - 1] == 1 || Map[i][j - 1] == 2)
                            {
                                return true;
                            }
                        }
                        if (HeadDirectionIndex == 3)
                        {
                            if (Map[i + 1][j] == 1 || Map[i + 1][j] == 2)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
        static bool CollisionFeed()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    if (Map[i][j] == 4)
                    {
                        if (HeadDirectionIndex == 0)
                        {
                            if (Map[i - 1][j] == 3)
                            {
                                return true;
                            }
                        }
                        if (HeadDirectionIndex == 1)
                        {
                            if (Map[i][j + 1] == 3)
                            {
                                return true;
                            }
                        }
                        if (HeadDirectionIndex == 2)
                        {
                            if (Map[i][j - 1] == 3)
                            {
                                return true;
                            }
                        }
                        if (HeadDirectionIndex == 3)
                        {
                            if (Map[i + 1][j] == 3)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
