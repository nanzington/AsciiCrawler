using asciiCrawler.UI;
using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;

namespace asciiCrawler {
    public class GameLoop {
        public static Console con;
        public static Dictionary<Point, Tile> Map;

        public static int MapWidth = 10;
        public static int MapHeight = 10;
        public static Point PlayerPos = new Point(5, 5); 
        public static int PlayerFacing = 0; // 0 up, 1 right, 2 down, 3 left

        public static int GameWidth = 240;
        public static int GameHeight = 80;

        public static SadFont SquareFont;
        public static Random rand;

        public static UIManager UIManager;

        public static void Main(string[] args) {
            Settings.WindowTitle = "ASCII Crawl";
            Settings.UseDefaultExtendedFont = true;

            Game.Create(GameWidth, GameHeight, "./fonts/ThinExtended.font");
            
            Game.Instance.OnStart = Init;
            Game.Instance.FrameUpdate += Update;

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        public static void Update(object sender, GameHost e) { 
        }

        public static void Init() {
            SquareFont = (SadFont)GameHost.Instance.LoadFont("./fonts/CheepicusExtended.font");
            rand = new Random();

            UIManager = new UIManager();
            UIManager.Init();

            Map = new();

            for (int x = 0; x < 10; x++) {
                for (int y = 0; y < 10; y++) {
                    if (x == 0 || x == 9 || y == 0 || y == 9) {
                        Map.Add(new Point(x, y), new Tile('#', true));
                    } else {
                        Map.Add(new Point(x, y), new Tile('.'));
                    }
                }
            } 
        }
    }
}