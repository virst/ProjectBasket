using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Syntaxer
{
    struct Lin
    {
        public string start;
        public string finish;
        public Color TextColor;

        public Lin(string Start, string Finish, Color c)
        {
            this.start = Start;
            this.finish = Finish;
            this.TextColor = c;
        }
    }
}
