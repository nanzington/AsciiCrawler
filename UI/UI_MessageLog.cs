using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using Key = SadConsole.Input.Keys;

namespace asciiCrawler.UI {
    public class UI_MessageLog : InstantUI {
        public List<ColoredString> Log = new();
        public int Top = 0;

        public UI_MessageLog(int width, int height) : base(width, height, "MessageLog") {
            Win.Position = new Point(40, 60);
            Win.CanDrag = false;
        }


        public override void Update() {
            Point mousePos = new MouseScreenObjectState(Con, GameHost.Instance.Mouse).CellPosition;

            Con.Clear();

            Win.PrintVertical(0, 1, new ColoredString(9.AsString() + "|", Color.Lime, Color.Black) + new ColoredString("new", Color.Gray, Color.Black));

            Win.PrintVertical(0, 18, new ColoredString(10.AsString() + "|", Color.Lime, Color.Black) + new ColoredString("dlo", Color.Gray, Color.Black), false);

            if (Log.Count > 0) {
                for (int i = Top; i >= 0 && i >= Top - 17; i--) {
                    int line = Top - i;

                    Con.Print(0, line, Log[i]);
                }

                if (Top != Log.Count - 1) {
                    Con.PrintClickable(140, 1, 9.AsString(), LogClicks, "top");
                }

                if (Top > 0 && Top > Log.Count - 16) {
                    Con.PrintClickable(140, 17, 10.AsString(), LogClicks, "bottom");
                }

                if (mousePos != new Point(0, 0)) {
                    if (GameHost.Instance.Mouse.ScrollWheelValueChange < 0) {
                        if (!Helper.EitherShift()) {
                            if (Top < Log.Count - 1) {
                                Top++;
                            }
                        }
                        else {
                            if (Top + 10 < Log.Count - 1) {
                                Top += 10;
                            }
                            else {
                                Top = Log.Count - 1;
                            }
                        }
                    }
                    else if (GameHost.Instance.Mouse.ScrollWheelValueChange > 0) {
                        if (!Helper.EitherShift()) {
                            if (Top > Log.Count - 16 && Top > 0)
                                Top--;
                        }
                        else {
                            if (Top - 10 > Log.Count - 16 && Top - 10 > 0)
                                Top -= 10;
                            else
                                Top = Math.Max(Log.Count - 16, 0);
                        }
                    }
                }
            }
        }

        public void LogClicks(string ID) {
            if (ID == "top") {
                Top = Log.Count - 1;
            }

            else if (ID == "bottom") {
                Top = Math.Max(Log.Count - 16, 0);
            }
        }

        public override void Input() {
            Point mousePos = new MouseScreenObjectState(Con, GameHost.Instance.Mouse).CellPosition;
        }
    }
}
