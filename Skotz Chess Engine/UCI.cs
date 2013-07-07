﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

// Scott Clayton 2013

namespace Skotz_Chess_Engine
{
    class UCI
    {
        Game game;

        string engine = "Skotz";
        string version = "v0.1";

        public UCI()
        {
            StartNewGame();
        }

        public void StartNewGame()
        {
            game = new Game();
            game.ResetBoard();
        }

        public void Start()
        {
            Console.WriteLine(engine + " " + version);

            bool player = true;
            bool go = false;
            bool quit = false;

            int time = 10;
            int depth = 99;

            string move;

            do
            {
                if (go)
                {
                    Move best = game.GetBestMove();
                    game.MakeMove(best);

                    move = "bestmove " + best.ToString();

                    Console.WriteLine(move);

                    go = false;

                    //game.Ponder(!player);
                }

                // Wait for other player
                do
                {
                    string cmd = Console.ReadLine();
                    //Console.WriteLine("CMD: " + cmd);

                    for (int tries = 3; tries >= 0; tries--)
                    {
                        try
                        {
                            StreamWriter w = new StreamWriter((player ? "W" : "B") + ".out.txt", true);
                            w.WriteLine(cmd);
                            w.Close();

                            break;
                        }
                        catch (IOException)
                        {
                            System.Threading.Thread.Sleep(10);
                        }
                    }

                    try
                    {
                        char[] breakit = cmd.ToCharArray();
                        if (breakit.Length >= 4 && breakit.Length <= 5)
                            if (breakit[0] >= 'a' && breakit[0] <= 'h')
                                if (breakit[2] >= 'a' && breakit[2] <= 'h')
                                    if (breakit[1] >= '1' && breakit[1] <= '8')
                                        if (breakit[3] >= '1' && breakit[3] <= '8')
                                        {
                                            //game.StopPondering();

                                            game.MakeMove(cmd);

                                            player = !player;

                                            break;
                                        }

                        if (cmd == "uci")
                        {
                            Console.WriteLine("id name " + engine);
                            Console.WriteLine("id author Scott Clayton");
                            Console.WriteLine("uciok");
                        }

                        if (cmd == "isready")
                        {
                            Console.WriteLine("readyok");
                        }

                        if (cmd.StartsWith("ucinewgame"))
                        {
                            player = true;
                            game = new Game();
                            game.ResetBoard();
                        }

                        if (cmd.StartsWith("position startpos moves "))
                        {
                            List<string> moves = cmd.ToLower().Replace("position startpos moves ", "").Split(' ').ToList();

                            player = true;
                            game = new Game();
                            game.ResetBoard();
                            foreach (string m in moves)
                            {
                                if (m == "f2f1n")
                                {
                                    int i = 1;
                                }

                                game.MakeMove(m);
                                //Console.WriteLine("~move " + m + " ~player " + (player ? "W" : "B"));
                                player = !player;
                            }
                            go = false;

                            break;
                        }

                        if (cmd == "go" || cmd.StartsWith("go "))
                        {
                            go = true;

                            //try
                            //{
                            //    if (cmd.Split(' ')[1] == "movetime")
                            //    {
                            //        time = Int32.Parse(cmd.Split(' ')[2]) / 1000;
                            //        depth = 99;
                            //        Console.WriteLine("~time = " + time.ToString());
                            //    }
                            //    if (cmd.Split(' ')[1] == "depth")
                            //    {
                            //        time = 999999;
                            //        depth = Int32.Parse(cmd.Split(' ')[2]) / 2;
                            //        Console.WriteLine("~depth = " + depth);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //    Console.WriteLine(ex.ToString());
                            //}

                            break;
                        }

                        if (cmd == "?")
                        {
                            Console.WriteLine("Forcing a move...");
                            // TODO
                        }

                        if (cmd == "quit")
                        {
                            quit = true;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Unknown Command: " + cmd);
                    }
                } while (!quit);
            } while (!quit);
        }
    }
}