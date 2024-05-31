using Newtonsoft.Json;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using Console = SadConsole.Console;

namespace asciiCrawler {
    public static class Helper {
        public static ColoredString GetDarker(this ColoredString instance) {
            for (int i = 0; i < instance.Length; i++) {
                instance[i].Foreground = instance[i].Foreground.GetDarker();
            }
            return instance;
        }

        public static bool KeyPressed(Keys key) {
            return GameHost.Instance.Keyboard.IsKeyPressed(key);
        }

        static HashSet<Keys> TriggeredHotkeys = new();
        static HashSet<Keys> SecondaryList = new();
        public static bool HotkeyDown(Keys key) {
            if (!TriggeredHotkeys.Contains(key) && GameHost.Instance.Keyboard.IsKeyPressed(key)) {
                TriggeredHotkeys.Add(key);
                return true;
            }

            return false;
        }

        public static void ClearKeys() {
            SecondaryList.Clear();
            foreach (Keys key in TriggeredHotkeys) {
                if (GameHost.Instance.Keyboard.IsKeyDown(key)) {
                    SecondaryList.Add(key);
                }
            }
            TriggeredHotkeys.Clear();

            foreach (Keys key in SecondaryList) {
                TriggeredHotkeys.Add(key);
            }
        }

        public static bool EitherShift() {
            if (GameHost.Instance.Keyboard.IsKeyDown(Keys.LeftShift) || GameHost.Instance.Keyboard.IsKeyDown(Keys.RightShift))
                return true;
            return false;
        }
        public static bool EitherControl() {
            if (GameHost.Instance.Keyboard.IsKeyDown(Keys.LeftControl) || GameHost.Instance.Keyboard.IsKeyDown(Keys.RightControl))
                return true;
            return false;
        }

        public static T Clone<T>(this T source) {
            if (Object.ReferenceEquals(source, null)) {
                return default(T);
            }

            var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
            var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, serializeSettings), deserializeSettings);
        }

        public static void DrawBox(Console con, int LeftX, int TopY, int w, int h, int r = 255, int g = 255, int b = 255) { 
            int RightX = LeftX + w;
            int BottomY = TopY + h;  

            Color fg = new Color(r, g, b);

            con.DrawLine(new Point(LeftX, TopY), new Point(RightX - 1, TopY), 196, fg);
            con.DrawLine(new Point(LeftX, BottomY - 1), new Point(RightX - 1, BottomY - 1), 196, fg);
            con.DrawLine(new Point(LeftX, TopY), new Point(LeftX, BottomY - 1), 179, fg);
            con.DrawLine(new Point(RightX - 1, TopY), new Point(RightX - 1, BottomY - 1), 179, fg);
            con.Print(LeftX, TopY, 218.AsString(), fg);
            con.Print(RightX - 1, BottomY - 1, 217.AsString(), fg);
            con.Print(LeftX, BottomY - 1, 216.AsString(), fg);
            con.Print(RightX - 1, TopY, 215.AsString(), fg);
        } 
    }

    public static class MiscExtensions {
        public static string AsString(this int instance) {
            return ((char)instance).ToString();
        }

        public static void PrintVertical(this Console instance, int x, int y, string str, bool down = true) {
            instance.PrintVertical(x, y, new ColoredString(str), down);
        }

        public static void PrintVertical(this Console instance, int x, int y, ColoredString str, bool down = true) {
            for (int i = 0; i < str.Length; i++) {
                int printY = y + (down ? i : -i);
                if (printY >= 0 && printY < instance.Height) {
                    instance.Print(x, printY, str[i].Glyph.AsString(), str[i].Foreground, str[i].Background);
                }
            }
        }

        public static void PrintClickable(this SadConsole.Console instance, int x, int y, string str, Action OnClick) {
            instance.PrintClickable(x, y, new ColoredString(str), OnClick);
        }

        public static void PrintClickable(this SadConsole.Console instance, int x, int y, ColoredString str, Action OnClick) {
            Point mousePos = new MouseScreenObjectState(instance, GameHost.Instance.Mouse).CellPosition;
            int length = str.Length - 1;

            instance.Print(x, y, mousePos.X >= x && mousePos.X <= x + length && mousePos.Y == y ? str.GetDarker() : str);

            if (GameHost.Instance.Mouse.LeftClicked) {
                if (mousePos.X >= x && mousePos.X <= x + length && mousePos.Y == y) {
                    OnClick();
                }
            }
        }

        public static void PrintClickable(this SadConsole.Console instance, int x, int y, string str, Action<string> OnClick, string ID) {
            instance.PrintClickable(x, y, new ColoredString(str), OnClick, ID);
        }

        public static void PrintClickable(this SadConsole.Console instance, int x, int y, ColoredString str, Action<string> OnClick, string ID) {
            Point mousePos = new MouseScreenObjectState(instance, GameHost.Instance.Mouse).CellPosition;
            int length = str.Length - 1;

            instance.Print(x, y, mousePos.X >= x && mousePos.X <= x + length && mousePos.Y == y ? str.GetDarker() : str);

            if (GameHost.Instance.Mouse.LeftClicked) {
                if (mousePos.X >= x && mousePos.X <= x + length && mousePos.Y == y) {
                    OnClick(ID);
                }
            }
        }
    }
}
