using SadConsole;
using SadRogue.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace asciiCrawler {
    public class Tile {
        public int Glyph;
        public Color Fore;
        public Color Back;
        public bool IsWall = false;

        public Tile(int g, bool wall = false) : this(g, Color.White, Color.Black, wall) { } 
        public Tile(int g, Color f, bool wall = false) : this(g, f, Color.Black, wall) { }

        public Tile(int g, Color f, Color b, bool wall = false) {
            Glyph = g;
            Fore = f;
            Back = b;
            IsWall = wall;
        }

        public ColoredString AsCS() {
            return new ColoredString(Glyph.AsString(), Fore, Back);
        }
    }
}
