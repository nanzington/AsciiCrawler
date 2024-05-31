using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using Console = SadConsole.Console;
using Key = SadConsole.Input.Keys; 

namespace asciiCrawler.UI {
    public class UI_Sidebar : InstantUI {
        Console minimap;

        public UI_Sidebar(int width, int height) : base(width, height, "Sidebar") {
            Win.CanDrag = false;
            Win.Position = new Point(0, 0);

            minimap = new(new CellSurface(10, 10), GameLoop.SquareFont);
            Win.Children.Add(minimap);
            minimap.Position = new Point(13, 1);
        }


        public override void Update() {
            Point mousePos = new MouseScreenObjectState(Con, GameHost.Instance.Mouse).CellPosition;
            Con.Clear();

            minimap.Clear();

            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    if (GameLoop.Map.ContainsKey(new Point(x, y))) {
                        minimap.PrintClickable(x, y, GameLoop.Map[new Point(x, y)].AsCS(), UI_Clicks, x + ";" + y);
                    }
                }
            } 

            if (GameLoop.PlayerFacing == 0)
                minimap.Print(GameLoop.PlayerPos.X, GameLoop.PlayerPos.Y, 24.AsString());
            else if (GameLoop.PlayerFacing == 1)
                minimap.Print(GameLoop.PlayerPos.X, GameLoop.PlayerPos.Y, 26.AsString());
            else if (GameLoop.PlayerFacing == 2)
                minimap.Print(GameLoop.PlayerPos.X, GameLoop.PlayerPos.Y, 25.AsString());
            else if (GameLoop.PlayerFacing == 3)
                minimap.Print(GameLoop.PlayerPos.X, GameLoop.PlayerPos.Y, 27.AsString());
        }


        public override void UI_Clicks(string ID) {
            string[] split = ID.Split(";");
            int x = int.Parse(split[0]);
            int y = int.Parse(split[1]);

            if (GameLoop.Map.ContainsKey(new Point(x, y))) {
                Tile tile = GameLoop.Map[new Point(x, y)];
                tile.IsWall = !tile.IsWall;

                if (tile.IsWall) {
                    tile.Glyph = '#';
                } else {
                    tile.Glyph = '.';
                }
            }
        }
    }
}
