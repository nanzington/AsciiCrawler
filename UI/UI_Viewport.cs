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

        public void DrawWallFrontAt(int x, int y, int w) {
            // really simple, just make a square of the desired size at the specific coordinates (then clear the inside, just in case it overlapped something else we drew)
            Con.DrawLine(new Point(x, y), new Point(x + w, y), '#', Color.White, Color.Black);
            Con.DrawLine(new Point(x + w, y), new Point(x + w, y + w), '#', Color.White, Color.Black);
            Con.DrawLine(new Point(x, y + w), new Point(x + w, y + w), '#', Color.White, Color.Black);
            Con.DrawLine(new Point(x, y), new Point(x, y + w), '#', Color.White, Color.Black);
            Con.Fill(new Rectangle(x + 1, y + 1, w - 1, w - 1), Color.White, Color.Black, ' ');
        }

        public void DrawWallSideAt(int x, int y, int height, int diagLength, bool fromRight) {
            //draw a line for the edge it's going to, then draw lines from the current wall spot to the 
            
            Con.DrawLine(new Point(x, y), new Point(x, y + height), '#');

            if (fromRight) {
                Con.DrawLine(new Point(x - 1, y), new Point(x - 1 - diagLength + 1, y + diagLength - 1), '#', Color.White, Color.Black);
                Con.DrawLine(new Point(x - 2, y + height), new Point(x - diagLength, y + height - diagLength + 2), '#', Color.White, Color.Black);
            } else {
                Con.DrawLine(new Point(x + 1, y), new Point(x + 1 + diagLength, y + diagLength), '#', Color.White, Color.Black);
                Con.DrawLine(new Point(x + 1, y + height - 1), new Point(x + 1 + diagLength, y - 1 + height - diagLength), '#', Color.White, Color.Black);
            }
        }

        public void SimpleSideAt(int x1, int y1a, int y1b, int x2, int y2a, int y2b, bool fromLeft = true) {
            // honestly this probably does more harm than good but we shuffle the inputs so that the smaller one is first
            int x3 = System.Math.Min(x1, x2);
            int y3a = System.Math.Min(y1a, y2a);
            int y3b = System.Math.Min(y1b, y2b);

            x2 = System.Math.Max(x1, x2);
            y2a = System.Math.Max(y1a, y2a);
            y2b = System.Math.Max(y1b, y2b);

            x1 = x3;
            y1a = y3a;
            y1b = y3b;
              

            // then we clear space that is likely to have overlapping sprites from walls farther back, and draw our lines to the next tile back
            if (fromLeft) { 
                Con.Fill(new Rectangle(new Point(x1 + 1, y2a), new Point(x2 - 1, y1b)), Color.White, Color.Black, ' ');
                Con.DrawLine(new Point(x1, y1a), new Point(x2, y2a), '#', Color.White, Color.Black);
                Con.DrawLine(new Point(x1, y2b), new Point(x2, y1b), '#', Color.White, Color.Black);
                Con.DrawLine(new Point(x2, y2a), new Point(x2, y1b), '#', Color.White, Color.Black);
            } else { 
                Con.Fill(new Rectangle(new Point(x1 + 1, y2a), new Point(x2 - 1, y1b)), Color.White, Color.Black, ' ');
                Con.DrawLine(new Point(x2, y1a), new Point(x1, y2a), '#', Color.White, Color.Black);
                Con.DrawLine(new Point(x1, y1b), new Point(x2, y2b), '#', Color.White, Color.Black);
                Con.DrawLine(new Point(x1, y2a), new Point(x1, y1b), '#', Color.White, Color.Black);
            } 
        }


        public override void Update() {  
            Con.Clear();


            // Get the player position and calculate some useful locations with it (left, right, and forward axes)
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
               

            for (int y = 3; y >= 0; y--) { 
                int currentSize = (14 + ((3-y) * 11) + (3 - y)); 
                int twoSize = 14 + ((3 - (y + 1)) * 11) + (3 - (y + 1));
                int nextSize = 14 + ((3 - (y - 1)) * 11) + (3 - (y - 1));

                // draw some guiding lines on the floor and ceiling so we have a sense of depth
                // we should probably draw horizontal ones too but i didn't want to figure out the numbers
                Con.DrawLine(new Point(0, 20 - ((3 - y) * 6)), new Point(116, 20 - ((3 - y) * 6)), '-', Color.DarkSlateGray);
                Con.DrawLine(new Point(0, 20 - ((3 - y) * 6) + currentSize), new Point(116, 20 - ((3 - y) * 6) + currentSize), '-', Color.DarkSlateGray);

                for (int i = 4; i > 0; i--) {
                    // draw some tiles from the right
                    Point fromR = playerPos + (forward * y) + (right * i); 
                    if (GameLoop.Map.ContainsKey(fromR)) { 
                        // corner where we're printing this tile
                        int sX = 41 + (y * 3) + (i * currentSize);
                        int sY = 20 - ((3 - y) * 6);
                         
                        // corner-positioning of the next farther tile
                        int oX = 41 + ((y + 1) * 3) + (i * twoSize);
                        int oY = 20 - ((3 - (y + 1)) * 6);

                        // corner-positioning of the next closer tile
                        int nX = 41 + ((y - 1) * 3) + (i * nextSize);
                        int nY = 20 - ((3 - (y - 1)) * 6);

                        Tile tile = GameLoop.Map[fromR];
                        if (tile.IsWall) { 
                            DrawWallFrontAt(sX, sY, currentSize); 

                            // if the tile just to the left of this isn't a wall, draw a wall side for this tile
                            Point oneLeft = fromR + left;
                            if (GameLoop.Map.ContainsKey(oneLeft)) {
                                Tile lTile = GameLoop.Map[oneLeft];
                                if (!lTile.IsWall) {
                                    // if it's far back do a little "hand drawn" edge
                                    if (y == 3)
                                        DrawWallSideAt(sX + i + (3 - y) - 1, sY - 1, currentSize, 3 + (3 - y), true);
                                    else {
                                        // otherwise draw sides by drawing a line from the corner of the current wall to the one behind
                                        // (or where it would be, if there isn't a wall behind) and the one ahead (if it exists)
                                        if (y != 0) { 
                                            SimpleSideAt(sX, sY, sY + currentSize, oX, oY, oY + twoSize, false);
                                        } else {
                                            SimpleSideAt(sX, sY, sY + currentSize, oX, oY, oY + twoSize, false);
                                            SimpleSideAt(sX, sY, sY + currentSize, nX, nY, nY + nextSize, false);
                                            if (GameLoop.Map.ContainsKey(playerPos + right) && GameLoop.Map[playerPos + right].IsWall)
                                                Con.Fill(new Rectangle(new Point(100, 0), new Point(116, 57)), Color.White, Color.Black, ' ');
                                        }
                                    }
                                }
                            }
                        } 
                    } 

                    // now do it from the left
                    Point fromL = playerPos + (forward * y) + (left * i); 
                    if (GameLoop.Map.ContainsKey(fromL)) {
                        // corner where we're printing this tile
                        int sX = 50 - (i * currentSize) - (3 - y) * 3; 
                        int sY = 20 - ((3 - y) * 6);

                        // corner-positioning of the next farther tile
                        int oX = 50 - ((i - 1) * twoSize) - (3 - (y + 1)) * 3;
                        int oY = 20 - ((3 - (y + 1)) * 6);

                        // corner-positioning of the next closer tile
                        int nX = 50 - (i * nextSize) - (3 - (y - 1)) * 3;
                        int nY = 20 - ((3 - (y - 1)) * 6);


                        Tile tile = GameLoop.Map[fromL];
                        if (tile.IsWall) { 
                            DrawWallFrontAt(sX, sY, currentSize);

                            // if the tile just to the right of this isn't a wall, draw a wall side for this tile
                            Point oneRight = fromL + right;
                            if (GameLoop.Map.ContainsKey(oneRight)) {
                                Tile rTile = GameLoop.Map[oneRight];
                                if (!rTile.IsWall) {
                                    // if it's far back do a little "hand drawn" edge
                                    if (y == 3)
                                        DrawWallSideAt(sX + currentSize, sY, currentSize, 1, false);
                                    else {
                                        // otherwise draw sides by drawing a line from the corner of the current wall to the one behind
                                        // (or where it would be, if there isn't a wall behind) and the one ahead (if it exists)
                                        if (y != 0) {
                                            SimpleSideAt(sX + currentSize, sY, sY + currentSize, oX, oY, oY + (twoSize));
                                        } else {
                                            SimpleSideAt(sX + currentSize, sY, sY + currentSize, oX, oY, oY + (twoSize));
                                            SimpleSideAt(sX + currentSize, sY, sY + currentSize, nX + nextSize, nY, nY + nextSize);
                                            if (GameLoop.Map.ContainsKey(playerPos + left) && GameLoop.Map[playerPos + left].IsWall) {
                                                Con.Fill(new Rectangle(new Point(0, 0), new Point(38, 57)), Color.White, Color.Black, ' ');
                                            }
                                        }
                                    }
                                }
                            }
                        } 
                    } 
                }

                // finally draw the walls directly in front of the player after everything else, so it covers up wall sides we shouldn't be able to see
                if (GameLoop.Map.ContainsKey(playerPos + (forward * y))) {
                    Tile tile = GameLoop.Map[playerPos + (forward * y)];
                    if (tile.IsWall) {
                        int sX = 50 - ((3 - y) * 3);
                        int sY = 20 - ((3 - y) * 6);

                        DrawWallFrontAt(sX, sY, currentSize);
                    }
                }
            } 
        }

        public override void Input() {

            // establish the points for directly adjacent spaces we could possibly be moving to
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
             
            // super basic controls since this wasn't really the point of the exercise
            // WASD to move *relative to current facing*, Q and E to rotate left and right
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
