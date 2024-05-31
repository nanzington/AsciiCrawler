using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadRogue.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace asciiCrawler.UI {
    public class UI_Viewport : InstantUI {
        public List<int> DistFilled = new();

        public UI_Viewport(int width, int height) : base(width, height, "Viewport") {
            Win = new Window(new CellSurface(width, height), GameLoop.SquareFont);
            Con = new Console(new CellSurface(width - 2, height - 2), GameLoop.SquareFont);
            Win.Children.Add(Con);
            Win.Position = new Point(23, 0);
            Con.Position = new Point(1, 1);

            Helper.DrawBox(Win, 0, 0, width, height);

            for (int i = 0; i < 10; i++) { 
                DistFilled.Add(256 + i);
            } 
        }


        public override void Update() {  
            Con.Clear();

            Point playerPos = GameLoop.PlayerPos;
            Point forward = new Point(0, 0);

            switch (GameLoop.PlayerFacing) {
                case 0: forward = new Point(0, -1); break;
                case 1: forward = new Point(1, 0); break;
                case 2: forward = new Point(0, 1); break;
                case 3: forward = new Point(-1, 0); break;
            }

            Point left = GameLoop.PlayerFacing == 1 || GameLoop.PlayerFacing == 3 ? new Point(forward.Y, -forward.X) : new Point(forward.Y, forward.X);
            Point right = GameLoop.PlayerFacing == 1 || GameLoop.PlayerFacing == 3 ? new Point(forward.Y, forward.X) : new Point(-forward.Y, forward.X);
            
            for (int i = 0; i < 4; i++) {
                Con.DrawLine(new Point(73 + (i * 18), 0), new Point(68 + (i * 18), 20), '#');
                Con.DrawLine(new Point(73 + (i * 18), Con.Height - 1), new Point(68 + (i * 18), Con.Height - 20), '#');
            } 

            //Con.DrawLine(new Point(58, 0), new Point(58, 14), '#'); 
            for (int i = 0; i < 3; i++) {
                Con.DrawLine(new Point(5 + (i * 18), 0), new Point(10 + (i * 18), 20), '#');
                Con.DrawLine(new Point(5 + (i * 18), Con.Height - 1), new Point(10 + (i * 18), Con.Height - 20), '#');
            }

            for (int i = 0; i < 3; i++) { 
                Con.DrawLine(new Point(0, i * 7 + 7), new Point(117, i * 7 + 7), '#'); 
                Con.DrawLine(new Point(0, Con.Height - (i * 7) - 7), new Point(117, Con.Height - (i * 7) - 7), '#');
            }

            Con.DrawLine(new Point(58, 54), new Point(58, 56), '|', Color.White);
            Con.Print(58, 53, 30.AsString()); 

            // leftmost column
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3) + (left * 3)) && GameLoop.Map[playerPos + (forward * 3) + (left * 3)].Glyph == '#') {
                    Con.Fill(new Rectangle(0, 22, 10, 15), new Color(50, 50, 50), Color.Black, '=');
                    Con.DrawLine(new Point(10, 22), new Point(10, 36), '#', new Color(50, 50, 50));
                }

                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2) + (left * 3)) && GameLoop.Map[playerPos + (forward * 2) + (left * 3)].Glyph == '#') {
                    Con.Fill(new Rectangle(0, 15, 9, 29), new Color(75, 75, 75), Color.Black, '=');
                    Con.DrawLine(new Point(9, 15), new Point(9, 43), '#', new Color(75, 75, 75)); 
                }

                if (GameLoop.Map.ContainsKey(playerPos + forward + (left * 3)) && GameLoop.Map[playerPos + forward + (left * 3)].Glyph == '#') {
                    Con.Fill(new Rectangle(0, 8, 7, 43), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(7, 8), new Point(7, 50), '#', new Color(100, 100, 100)); 
                }

                if (GameLoop.Map.ContainsKey(playerPos + (left * 3)) && GameLoop.Map[playerPos + (left * 3)].Glyph == '#') {
                    Con.Fill(new Rectangle(0, 0, 5, 58), new Color(125, 125, 125), Color.Black, '=');
                    Con.DrawLine(new Point(5, 0), new Point(5, 57), '#', new Color(125, 125, 125)); 
                }
            }

            // 2 left column
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3) + (left * 2)) && GameLoop.Map[playerPos + (forward * 3) + (left * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(10, 22), new Point(10, 36), '#', new Color(50, 50, 50));
                    Con.Fill(new Rectangle(11, 22, 17, 15), new Color(50, 50, 50), Color.Black, '=');
                    Con.DrawLine(new Point(28, 22), new Point(28, 36), '#', new Color(50, 50, 50));
                }

                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2) + (left * 2)) && GameLoop.Map[playerPos + (forward * 2) + (left * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(9, 15), new Point(9, 43), '#', new Color(75, 75, 75));
                    Con.Fill(new Rectangle(10, 15, 17, 29), new Color(75, 75, 75), Color.Black, '=');
                    Con.DrawLine(new Point(27, 15), new Point(27, 43), '#', new Color(75, 75, 75));
                }

                if (GameLoop.Map.ContainsKey(playerPos + forward + (left * 2)) && GameLoop.Map[playerPos + forward + (left * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(7, 8), new Point(7, 50), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(8, 8, 17, 43), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(25, 8), new Point(25, 50), '#', new Color(100, 100, 100));
                }

                if (GameLoop.Map.ContainsKey(playerPos + (left * 2)) && GameLoop.Map[playerPos + (left * 2)].Glyph == '#') {
                    Con.Fill(new Rectangle(0, 0, 5, 57), Color.White, Color.Black, ' ');
                    Con.DrawLine(new Point(5, 0), new Point(5, 57), '#', new Color(125, 125, 125));
                    Con.Fill(new Rectangle(6, 0, 17, 58), new Color(125, 125, 125), Color.Black, '=');
                    Con.DrawLine(new Point(23, 0), new Point(23, 57), '#', new Color(125, 125, 125));
                }
            }

            // left column
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3) + left) && GameLoop.Map[playerPos + (forward * 3) + left].Glyph == '#') {
                    Con.DrawLine(new Point(28, 22), new Point(28, 36), '#', new Color(50, 50, 50));
                    Con.Fill(new Rectangle(29, 22, 17, 15), new Color(50, 50, 50), Color.Black, '='); 
                }

                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2) + left) && GameLoop.Map[playerPos + (forward * 2) + left].Glyph == '#') {
                    Con.DrawLine(new Point(27, 15), new Point(27, 43), '#', new Color(75, 75, 75));
                    Con.Fill(new Rectangle(28, 15, 17, 29), new Color(75, 75, 75), Color.Black, '='); 
                }

                if (GameLoop.Map.ContainsKey(playerPos + forward + left) && GameLoop.Map[playerPos + forward + left].Glyph == '#') {
                    Con.DrawLine(new Point(25, 8), new Point(25, 50), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(26, 8, 17, 43), new Color(100, 100, 100), Color.Black, '='); 
                }

                if (GameLoop.Map.ContainsKey(playerPos + left) && GameLoop.Map[playerPos + left].Glyph == '#') {
                    Con.Fill(new Rectangle(0, 0, 23, 58), Color.White, Color.Black, ' ');
                    Con.DrawLine(new Point(23, 0), new Point(23, 57), '#', new Color(125, 125, 125));
                    Con.Fill(new Rectangle(24, 0, 16, 58), new Color(125, 125, 125), Color.Black, '=');
                    Con.DrawLine(new Point(40, 0), new Point(40, 57), '#', new Color(125, 125, 125)); 
                }
            }

            // 3 right
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3) + (right * 3)) && GameLoop.Map[playerPos + (forward * 3) + (right * 3)].Glyph == '#') {
                    Con.DrawLine(new Point(104, 22), new Point(104, 36), '#', new Color(50, 50, 50));
                    Con.Fill(new Rectangle(105, 22, 10, 15), new Color(50, 50, 50), Color.Black, '=');
                }

                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2) + (right * 3)) && GameLoop.Map[playerPos + (forward * 2) + (right * 3)].Glyph == '#') {
                    Con.DrawLine(new Point(105, 15), new Point(105, 43), '#', new Color(75, 75, 75));
                    Con.Fill(new Rectangle(106, 15, 9, 29), new Color(75, 75, 75), Color.Black, '=');
                }

                if (GameLoop.Map.ContainsKey(playerPos + forward + (right * 3)) && GameLoop.Map[playerPos + forward + (right * 3)].Glyph == '#') {
                    Con.DrawLine(new Point(107, 8), new Point(107, 50), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(108, 8, 7, 43), new Color(100, 100, 100), Color.Black, '=');
                }

                if (GameLoop.Map.ContainsKey(playerPos + (right * 3)) && GameLoop.Map[playerPos + (right * 3)].Glyph == '#') {
                    Con.DrawLine(new Point(109, 0), new Point(109, 57), '#', new Color(125, 125, 125));
                    Con.Fill(new Rectangle(110, 0, 5, 58), new Color(100, 100, 100), Color.Black, '='); 
                }
            }

            // 2 right
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3) + (right * 2)) && GameLoop.Map[playerPos + (forward * 3) + (right * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(86, 22), new Point(86, 36), '#', new Color(50, 50, 50));
                    Con.Fill(new Rectangle(87, 22, 17, 15), new Color(50, 50, 50), Color.Black, '=');
                    Con.DrawLine(new Point(104, 22), new Point(104, 36), '#', new Color(50, 50, 50));
                }
                
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2) + (right * 2)) && GameLoop.Map[playerPos + (forward * 2) + (right * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(87, 15), new Point(87, 43), '#', new Color(75, 75, 75));
                    Con.Fill(new Rectangle(88, 15, 17, 29), new Color(75, 75, 75), Color.Black, '=');
                    Con.DrawLine(new Point(105, 15), new Point(105, 43), '#', new Color(75, 75, 75));
                }
                
                if (GameLoop.Map.ContainsKey(playerPos + forward + (right * 2)) && GameLoop.Map[playerPos + forward + (right * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(89, 8), new Point(89, 50), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(90, 8, 17, 43), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(107, 8), new Point(107, 50), '#', new Color(100, 100, 100));
                }

                
                if (GameLoop.Map.ContainsKey(playerPos + (right * 2)) && GameLoop.Map[playerPos + (right * 2)].Glyph == '#') {
                    Con.DrawLine(new Point(91, 0), new Point(91, 57), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(92, 0, 17, 58), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(109, 0), new Point(109, 58), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(110, 0, 5, 57), new Color(100, 100, 100), Color.Black, ' ');
                }
            }

            // 1 right
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3) + right) && GameLoop.Map[playerPos + (forward * 3) + right].Glyph == '#') {
                    Con.DrawLine(new Point(68, 22), new Point(68, 36), '#', new Color(50, 50, 50));
                    Con.Fill(new Rectangle(69, 22, 17, 15), new Color(50, 50, 50), Color.Black, '=');
                    Con.DrawLine(new Point(86, 22), new Point(86, 36), '#', new Color(50, 50, 50));
                }

                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2) + right) && GameLoop.Map[playerPos + (forward * 2) + right].Glyph == '#') {
                    Con.DrawLine(new Point(69, 15), new Point(69, 43), '#', new Color(75, 75, 75));
                    Con.Fill(new Rectangle(70, 15, 17, 29), new Color(75, 75, 75), Color.Black, '=');
                    Con.DrawLine(new Point(87, 15), new Point(87, 43), '#', new Color(75, 75, 75));
                }

                if (GameLoop.Map.ContainsKey(playerPos + forward + right) && GameLoop.Map[playerPos + forward + right].Glyph == '#') {
                    Con.DrawLine(new Point(71, 8), new Point(71, 50), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(72, 8, 17, 43), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(89, 8), new Point(89, 50), '#', new Color(100, 100, 100));
                }

                if (GameLoop.Map.ContainsKey(playerPos + right) && GameLoop.Map[playerPos + right].Glyph == '#') {
                    Con.DrawLine(new Point(73, 0), new Point(73, 57), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(74, 0, 17, 58), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(91, 0), new Point(91, 57), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(92, 0, 23, 58), new Color(100, 100, 100), Color.Black, ' ');
                }
            }


             

            // 3 forward
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 3)) && GameLoop.Map[playerPos + (forward * 3)].Glyph == '#') { 
                    Con.DrawLine(new Point(46, 22), new Point(46, 36), '#', new Color(50, 50, 50));
                    Con.Fill(new Rectangle(47, 22, 21, 15), new Color(50, 50, 50), Color.Black, '=');
                    Con.DrawLine(new Point(68, 22), new Point(68, 36), '#', new Color(50, 50, 50));
                } 
            }

            // 2 forward
            {
                if (GameLoop.Map.ContainsKey(playerPos + (forward * 2)) && GameLoop.Map[playerPos + (forward * 2)].Glyph == '#') { 
                    Con.DrawLine(new Point(45, 15), new Point(45, 43), '#', new Color(75, 75, 75));
                    Con.Fill(new Rectangle(46, 15, 23, 29), new Color(75, 75, 75), Color.Black, '=');
                    Con.DrawLine(new Point(69, 15), new Point(69, 43), '#', new Color(75, 75, 75));
                }
                  
            }
            
            // 1 forward
            {
                if (GameLoop.Map.ContainsKey(playerPos + forward) && GameLoop.Map[playerPos + forward].Glyph == '#') {
                    Con.DrawLine(new Point(43, 8), new Point(43, 50), '#', new Color(100, 100, 100));
                    Con.Fill(new Rectangle(44, 8, 27, 43), new Color(100, 100, 100), Color.Black, '=');
                    Con.DrawLine(new Point(71, 8), new Point(71, 50), '#', new Color(100, 100, 100));
                }
                 
            } 

        }

        public override void Input() {
            Point forward = new Point(0, 0);

            switch (GameLoop.PlayerFacing) {
                case 0: forward = new Point(0, -1); break;
                case 1: forward = new Point(1, 0); break;
                case 2: forward = new Point(0, 1); break;
                case 3: forward = new Point(-1, 0); break;
            }

            Point left = GameLoop.PlayerFacing == 1 || GameLoop.PlayerFacing == 3 ? new Point(forward.Y, -forward.X) : new Point(forward.Y, forward.X);
            Point right = GameLoop.PlayerFacing == 1 || GameLoop.PlayerFacing == 3 ? new Point(forward.Y, forward.X) : new Point(-forward.Y, forward.X);
            Point back = new Point(-forward.X, -forward.Y);
             

            if (Helper.KeyPressed(Keys.W)) {
                if (GameLoop.Map.ContainsKey(GameLoop.PlayerPos + forward) && !GameLoop.Map[GameLoop.PlayerPos + forward].IsWall)
                    GameLoop.PlayerPos = GameLoop.PlayerPos + forward;
            } else if (Helper.KeyPressed(Keys.A)) {
                if (GameLoop.Map.ContainsKey(GameLoop.PlayerPos + left) && !GameLoop.Map[GameLoop.PlayerPos + left].IsWall)
                    GameLoop.PlayerPos = GameLoop.PlayerPos + left;
            } else if (Helper.KeyPressed(Keys.D)) {
                if (GameLoop.Map.ContainsKey(GameLoop.PlayerPos + right) && !GameLoop.Map[GameLoop.PlayerPos + right].IsWall)
                    GameLoop.PlayerPos = GameLoop.PlayerPos + right;
            } else if (Helper.KeyPressed(Keys.S)) {
                if (GameLoop.Map.ContainsKey(GameLoop.PlayerPos + back) && !GameLoop.Map[GameLoop.PlayerPos + back].IsWall)
                    GameLoop.PlayerPos = GameLoop.PlayerPos + back;
            } else if (Helper.HotkeyDown(Keys.Q)) {
                GameLoop.PlayerFacing -= 1;
                if (GameLoop.PlayerFacing < 0)
                    GameLoop.PlayerFacing = 3;
            } else if (Helper.HotkeyDown(Keys.E)) {
                GameLoop.PlayerFacing += 1;
                if (GameLoop.PlayerFacing > 3)
                    GameLoop.PlayerFacing = 0;
            }
        } 
    }
}
