using SadRogue.Primitives;
using SadConsole;
using System;
using Key = SadConsole.Input.Keys;
using SadConsole.UI;
using Color = SadRogue.Primitives.Color; 
using System.Collections.Generic; 
using SadConsole.Input;
using System.Linq;

namespace asciiCrawler.UI {
    public class UIManager : ScreenObject {
        public SadConsole.UI.Colors CustomColors;

        public UI_Viewport Viewport; 
        public UI_MessageLog MsgLog;
        public UI_Sidebar Sidebar;

        public Dictionary<string, InstantUI> Interfaces = new();

        public bool clientAndConnected = true;

        public UIManager() {
            IsVisible = true;
            IsFocused = true;
            Parent = GameHost.Instance.Screen;
        }

        public InstantUI? GetUI(string name) {
            if (Interfaces.ContainsKey(name))
                return Interfaces[name];
            return null;
        }

        public void ToggleUI(string name) {
            if (Interfaces.ContainsKey(name)) {
                Interfaces[name].Win.IsVisible = !Interfaces[name].Win.IsVisible;

                if (Interfaces[name].Win.IsVisible) {
                    Interfaces[name].Win.IsFocused = true;
                }
            }
        }


        public void AddMsg(string msg) { MsgLog.Log.Add(new ColoredString(msg)); MsgLog.Top = MsgLog.Log.Count - 1; }
        public void AddMsg(ColoredString msg) { MsgLog.Log.Add(msg); MsgLog.Top = MsgLog.Log.Count - 1; } 

        public override void Update(TimeSpan timeElapsed) {
            foreach (KeyValuePair<string, InstantUI> kv in Interfaces) {
                if (kv.Value.Win.IsVisible) {
                    kv.Value.Update();
                    kv.Value.Input();
                    kv.Value.Win.IsFocused = true;
                }
            }

            CheckKeyboard();
            Helper.ClearKeys();
            base.Update(timeElapsed);
        }

        public void Init() {
            SetupCustomColors();

            Viewport = new UI_Viewport(117, 60);
            MsgLog = new UI_MessageLog(200, 20);
            Sidebar = new UI_Sidebar(40, 80);

            UseMouse = true;

            ToggleUI("Viewport");
            ToggleUI("MessageLog");
            ToggleUI("Sidebar");
        }  

        private void CheckKeyboard() { 

        }

        private void SetupCustomColors() {
            CustomColors = SadConsole.UI.Colors.CreateAnsi();
            CustomColors.ControlHostBackground = new AdjustableColor(Color.Black, "Black");
            CustomColors.Lines = new AdjustableColor(Color.White, "White");
            CustomColors.Title = new AdjustableColor(Color.White, "White");

            CustomColors.RebuildAppearances();
            SadConsole.UI.Themes.Library.Default.Colors = CustomColors;
        }

    }
}
