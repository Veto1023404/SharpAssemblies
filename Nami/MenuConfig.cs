using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Nami
{
    class MenuConfig
    {
        public static Menu config;
        public static string menuName = "Nami";
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

                config.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("Harass", "Harass");
            {
                harassMenu.AddItem(new MenuItem("HarassMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                harassMenu.AddItem(new MenuItem("HarassQ", "Use Q")).SetValue(true);
                harassMenu.AddItem(new MenuItem("HarassW", "Use W")).SetValue(true);
                harassMenu.AddItem(new MenuItem("HarassE", "Use E")).SetValue(true);

                config.AddSubMenu(harassMenu);
            }

            var farmMenu = new Menu("Farming", "Farming");
            {
                /*
                var laneclearMenu = new Menu("LaneClear", "Lane Clear");
                {
                    laneclearMenu.AddItem(new MenuItem("LaneClearMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    laneclearMenu.AddItem(new MenuItem("LaneClearQ", "Use Q")).SetValue(true);
                    laneclearMenu.AddItem(new MenuItem("LaneClearW", "Use W")).SetValue(true);
                    farmMenu.AddSubMenu(laneclearMenu);
                }
                */
                
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

            var interuptMenu = new Menu("Interupt", "Interupt");
            {
                interuptMenu.AddItem(new MenuItem("InteruptQ", "Interupt with Q")).SetValue(true);

                config.AddSubMenu(interuptMenu);
            }

            var autoHealMenu = new Menu("AutoHeal", "Autoheal");
            {
                autoHealMenu.AddItem(new MenuItem("HealMana", "Mana & Usage")).SetValue(new Slider(40, 1, 99));
                autoHealMenu.AddItem(new MenuItem("MinPlayerHP", "Minimum Player HP")).SetValue(new Slider(40, 1, 99));
                autoHealMenu.AddItem(new MenuItem("HealPlayer", "Heal yourself")).SetValue(true);
                autoHealMenu.AddItem(new MenuItem("MinAllyHP", "Minimum Ally HP")).SetValue(new Slider(40, 1, 99));
                autoHealMenu.AddItem(new MenuItem("HealAlly", "Heal Ally")).SetValue(true);
            }

            var drawingMenu = new Menu("Drawings", "Drawings");
            {
                drawingMenu.AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawE", "Draw E")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawR", "Draw R")).SetValue(true);

                config.AddSubMenu(drawingMenu);
            }

            config.AddToMainMenu();
        }

        public static bool ComboQ { get { return config.Item("ComboQ").GetValue<bool>(); } }
        public static bool ComboW { get { return config.Item("ComboW").GetValue<bool>(); } }
        public static bool ComboE { get { return config.Item("ComboE").GetValue<bool>(); } }
        public static bool ComboR { get { return config.Item("ComboR").GetValue<bool>(); } }
        public static int HarassMana { get { return config.Item("HarassMana").GetValue<Slider>().Value; } }
        public static bool HarassQ { get { return config.Item("HarassQ").GetValue<bool>(); } }
        public static bool HarassW { get { return config.Item("HarassW").GetValue<bool>(); } }
        public static bool HarassE { get { return config.Item("HarassE").GetValue<bool>(); } }
        public static int LaneClearMana { get { return config.Item("LaneClearMana").GetValue<Slider>().Value; } }
        public static bool LaneClearQ { get { return config.Item("LaneClearQ").GetValue<bool>(); } }
        public static bool LaneClearW { get { return config.Item("LaneClearW").GetValue<bool>(); } }
        public static int LastHitMana { get { return config.Item("LastHitMana").GetValue<Slider>().Value; } }
        public static bool LastHitQ { get { return config.Item("LastHitQ").GetValue<bool>(); } }
        public static bool LastHitW { get { return config.Item("LastHitW").GetValue<bool>(); } }
        public static int JungleMana { get { return config.Item("JungleMana").GetValue<Slider>().Value; } }
        public static bool JungleQ { get { return config.Item("JungleQ").GetValue<bool>(); } }
        public static bool JungleW { get { return config.Item("JungleW").GetValue<bool>(); } }
        public static bool InteruptQ { get { return config.Item("InteruptQ").GetValue<bool>(); } }
        public static int HealMana { get { return config.Item("HealMana").GetValue<Slider>().Value; } }
        public static int MinPlayerHP { get { return config.Item("MinPlayerHP").GetValue<Slider>().Value; } }
        public static bool HealPlayer { get { return config.Item("HealPlayer").GetValue<bool>(); } }
        public static int MinAllyHP { get { return config.Item("MinAllyHP").GetValue<Slider>().Value; } }
        public static bool HealAlly { get { return config.Item("HealAlly").GetValue<bool>(); } }
        public static bool DrawQ { get { return config.Item("DrawQ").GetValue<bool>(); } }
        public static bool DrawW { get { return config.Item("DrawW").GetValue<bool>(); } }
        public static bool DrawE { get { return config.Item("DrawE").GetValue<bool>(); } }
        public static bool DrawR { get { return config.Item("DrawR").GetValue<bool>(); } }
    }
}
