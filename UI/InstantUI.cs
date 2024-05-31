using SadConsole;
using SadConsole.Input;
using SadConsole.UI;
using SadRogue.Primitives;
using System.Collections.Generic;
using Key = SadConsole.Input.Keys; 

namespace asciiCrawler.UI {
    public class InstantUI {
        public SadConsole.Console Con;
        public Window Win;

        public InstantUI(int width, int height, string uniqueID, string windowTitle = "", bool subUI = false) {
            Win = new(width, height);

            Win.CanDrag = true;
            Win.Position = new((GameLoop.GameWidth - width) / 2, (GameLoop.GameHeight - height) / 2);

            int conWidth = width - 2;
            int conHeight = height - 2;

            Con = new(conWidth, conHeight);
            Con.Position = new(1, 1);
            Win.Title = windowTitle.Align(HorizontalAlignment.Center, conWidth, (char)196);


            Win.Children.Add(Con);

            Win.Show();
            Win.IsVisible = false;

            if (!subUI) {
                GameLoop.UIManager.Children.Add(Win);
                GameLoop.UIManager.Interfaces.Add(uniqueID, this);
            }
        }


        public virtual void Update() {
            Con.Clear();
        }

        public virtual void Input() {
            Point mousePos = new MouseScreenObjectState(Con, GameHost.Instance.Mouse).CellPosition;
        }

        public virtual void UI_Clicks(string ID) {

        }
    }
}
