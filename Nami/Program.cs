using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Nami
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Nami.Load;
        }
    }
}
