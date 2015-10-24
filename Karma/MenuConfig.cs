using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Karma
{
    class MenuConfig
    {
        public static Menu config;
        public static string menuName = "Karma";
        public static Orbwalking.Orbwalker Orbwalker;

        public static Menu TSMenu = new Menu("Target Selector", "Target Selector");

        public static void LoadMenu()
        {
            config = new Menu(menuName, menuName, true);

            config.AddSubMenu(new Menu("Orbwalking", "Orbwalking"));

            TargetSelector.AddToMenu(TSMenu);
            config.AddSubMenu(TSMenu);

            Orbwalker = new Orbwalking.Orbwalker(config.SubMenu("Orbwalking"));

            var comboMenu = new Menu("Combo", "Combo");
            {
                comboMenu.AddItem(new MenuItem("ComboQ", "Use Q")).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboW", "Use W")).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboE", "Use E")).SetValue(true);
                comboMenu.AddItem(new MenuItem("ComboEHP", "Use E if Minimum Player HP %")).SetValue(new Slider(40, 1, 99));
                comboMenu.AddItem(new MenuItem("ComboR", "Use R"));

                config.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("Harass", "Harass");
            {
                harassMenu.AddItem(new MenuItem("HarassMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                harassMenu.AddItem(new MenuItem("HarassQ", "Use Q")).SetValue(true);
                harassMenu.AddItem(new MenuItem("HarassW", "Use W")).SetValue(true);

                config.AddSubMenu(harassMenu);
            }

            var farmMenu = new Menu("Farming", "Farming");
            {
                var laneclearMenu = new Menu("LaneClear", "Lane Clear");
                {
                    laneclearMenu.AddItem(new MenuItem("LaneClearMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    laneclearMenu.AddItem(new MenuItem("LaneClearQ", "Use Q")).SetValue(true);
                    laneclearMenu.AddItem(new MenuItem("LaneClearW", "Use W")).SetValue(true);
                    laneclearMenu.AddItem(new MenuItem("LaneClearWP", "Use W When HP%")).SetValue(new Slider(50, 1, 99));

                    farmMenu.AddSubMenu(laneclearMenu);
                }

                var lasthitMenu = new Menu("LastHit", "Last Hit");
                {
                    lasthitMenu.AddItem(new MenuItem("LastHitMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    lasthitMenu.AddItem(new MenuItem("LastHitQ", "Use Q")).SetValue(true);
                    lasthitMenu.AddItem(new MenuItem("LastHitW", "Use W")).SetValue(true);

                    farmMenu.AddSubMenu(lasthitMenu);
                }
                /*
                var jungleMenu = new Menu("JungleClear", "Jungle Clear");
                {
                    jungleMenu.AddItem(new MenuItem("JungleMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    jungleMenu.AddItem(new MenuItem("JungleQ", "Use Q")).SetValue(true);
                    jungleMenu.AddItem(new MenuItem("JungleW", "Use W")).SetValue(true);
                    
                    farmMenu.AddSubMenu(jungleMenu);
                }
                */

                config.AddSubMenu(farmMenu);
            }

            var ultMenu = new Menu("UltSettings", "Ult Settings");
            {
                ultMenu.AddItem(new MenuItem("UltQ", "Use Q")).SetValue(true);
                ultMenu.AddItem(new MenuItem("UltW", "Use W")).SetValue(true);
                ultMenu.AddItem(new MenuItem("UltWHP", "Use W When Player HP%")).SetValue(new Slider(30, 1, 99));
                ultMenu.AddItem(new MenuItem("UltE", "Use E")).SetValue(true);
                ultMenu.AddItem(new MenuItem("UltEMinAlly", "Use E Minimum Ally in Range")).SetValue(new Slider(3, 1, 5));
                ultMenu.AddItem(new MenuItem("UltEMinAllyHP", "Use E Minimum Ally HP %")).SetValue(new Slider(40, 1, 99));
            }

            var autoshieldMenu = new Menu("Autoshield", "Auto Shield");
            {
                autoshieldMenu.AddItem(new MenuItem("UseShield", "Use Auto Shield")).SetValue(true);
                autoshieldMenu.AddItem(new MenuItem("ShieldMana", "Shield Mana %")).SetValue(new Slider(40, 1, 99));
                autoshieldMenu.AddItem(new MenuItem("ShieldSelf", "Shield Self")).SetValue(true);
                autoshieldMenu.AddItem(new MenuItem("ShieldMinPlayerHP", "Minimum Player HP%")).SetValue(new Slider(40, 1, 99));
                autoshieldMenu.AddItem(new MenuItem("ShieldAlly", "Shield Ally")).SetValue(true);
                autoshieldMenu.AddItem(new MenuItem("ShieldMinAllyHP", "Minimum Ally HP%")).SetValue(new Slider(40, 1, 99));
            }

            var gapcloseMenu = new Menu("Gapclose", "Gapclose");
            {
                gapcloseMenu.AddItem(new MenuItem("UseGapclose", "Use Gapclosers")).SetValue(true);
                gapcloseMenu.AddItem(new MenuItem("GapcloseQ", "Use Q"));
            }

            var miscMenu = new Menu("MiscSettings", "Misc Settings");
            {
                miscMenu.AddItem(new MenuItem("WOnStun", "WOnStun")).SetValue(true);
            }

            var drawingMenu = new Menu("Drawings", "Drawings");
            {
                drawingMenu.AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawDMG", "Draw Damage")).SetValue(true);

                config.AddSubMenu(drawingMenu);
            }

            config.AddToMainMenu();
        }

        public static bool ComboQ { get { return config.Item("ComboQ").GetValue<bool>(); } }
        public static bool ComboW { get { return config.Item("ComboW").GetValue<bool>(); } }
        public static bool ComboE { get { return config.Item("ComboE").GetValue<bool>(); } }
        public static int ComboEHP { get { return config.Item("ComboEHP").GetValue<Slider>().Value; } }
        public static bool ComboR { get { return config.Item("ComboR").GetValue<bool>(); } }
        public static int HarassMana { get { return config.Item("HarassMana").GetValue<Slider>().Value; } }
        public static bool HarassQ { get { return config.Item("HarassQ").GetValue<bool>(); } }
        public static bool HarassW { get { return config.Item("HarassW").GetValue<bool>(); } }
        public static int LaneClearMana { get { return config.Item("LaneClearMana").GetValue<Slider>().Value; } }
        public static bool LaneClearQ { get { return config.Item("LaneClearQ").GetValue<bool>(); } }
        public static bool LaneClearW { get { return config.Item("LaneClearW").GetValue<bool>(); } }
        public static bool LaneClearWHP { get { return config.Item("LaneClearWHP").GetValue<bool>(); } }
        public static int LastHitMana { get { return config.Item("LastHitMana").GetValue<Slider>().Value; } }
        public static bool LastHitQ { get { return config.Item("LastHitQ").GetValue<bool>(); } }
        public static bool LastHitW { get { return config.Item("LastHitW").GetValue<bool>(); } }
        public static bool UltQ { get { return config.Item("UltQ").GetValue<bool>(); } }
        public static bool UltW { get { return config.Item("UltW").GetValue<bool>(); } }
        public static int UltWHP { get { return config.Item("UltWHP").GetValue<Slider>().Value; } }
        public static int UltWMinHP { get { return config.Item("UltWMinHP").GetValue<Slider>().Value; } }
        public static bool UltE { get { return config.Item("UltE").GetValue<bool>(); } }
        public static int UltEMinAlly { get { return config.Item("UltEMinAlly").GetValue<Slider>().Value; } }
        public static int UltEMinAllyHP { get { return config.Item("UltEMinAllyHP").GetValue<Slider>().Value; } }
        public static bool UseShield { get { return config.Item("UseShield").GetValue<bool>(); } }
        public static int ShieldMana { get { return config.Item("ShieldMana").GetValue<Slider>().Value; } }
        public static bool ShieldSelf { get { return config.Item("ShieldSelf").GetValue<bool>(); } }
        public static int ShieldMinPlayerHP { get { return config.Item("ShieldMinPlayerHP").GetValue<Slider>().Value; } }
        public static bool ShieldAlly { get { return config.Item("ShieldAlly").GetValue<bool>(); } }
        public static int ShieldMinAllyHP { get { return config.Item("ShieldMinAllyHP").GetValue<Slider>().Value; } }
        public static bool UseGapclose { get { return config.Item("UseGapclose").GetValue<bool>(); } }
        public static bool GapcloseQ { get { return config.Item("GapcloseQ").GetValue<bool>(); } }
        public static bool WOnStun { get { return config.Item("WOnStun").GetValue<bool>(); } }
        public static bool DrawQ { get { return config.Item("DrawQ").GetValue<bool>(); } }
        public static bool DrawW { get { return config.Item("DrawW").GetValue<bool>(); } }
        public static bool DrawE { get { return config.Item("DrawE").GetValue<bool>(); } }
        public static bool DrawDMG { get { return config.Item("DrawDMG").GetValue<bool>(); } }

    }
}
