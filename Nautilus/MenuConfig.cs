using LeagueSharp;
using LeagueSharp.Common;

namespace Nautilus
{
    class MenuConfig
    {
        public static Menu config;
        public static string menuName = "Nautilus";
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
                comboMenu.AddItem(new MenuItem("ComboR", "Use R")).SetValue(true);

                config.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("Harass", "Harass");
            {
                harassMenu.AddItem(new MenuItem("HarassMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                harassMenu.AddItem(new MenuItem("HarassQ", "Use Q")).SetValue(false);
                harassMenu.AddItem(new MenuItem("HarassE", "Use E")).SetValue(true);

                config.AddSubMenu(harassMenu);
            }

            var farmMenu = new Menu("Farming", "Farming");
            {
                /*
                var laneclearMenu = new Menu("LaneClear", "Lane Clear");
                {
                    laneclearMenu.AddItem(new MenuItem("LaneClearMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    laneclearMenu.AddItem(new MenuItem("LaneClearE", "Use E")).SetValue(true);
                    farmMenu.AddSubMenu(laneclearMenu);
                }
                */

                var lasthitMenu = new Menu("LastHit", "Last Hit");
                {
                    lasthitMenu.AddItem(new MenuItem("LastHitMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    lasthitMenu.AddItem(new MenuItem("LastHitE", "Use E")).SetValue(true);

                    farmMenu.AddSubMenu(lasthitMenu);
                }
                /*
                var jungleMenu = new Menu("JungleClear", "Jungle Clear");
                {
                    jungleMenu.AddItem(new MenuItem("JungleMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    jungleMenu.AddItem(new MenuItem("JungleE", "Use E")).SetValue(true);
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

            var autoShieldMenu = new Menu("AutoShield", "Auto Shield");
            {
                autoShieldMenu.AddItem(new MenuItem("AutoShield", "Auto Shield")).SetValue(true);
                autoShieldMenu.AddItem(new MenuItem("AutoShieldMana", "Auto Shield Mana %")).SetValue(new Slider(25, 1, 99));
                autoShieldMenu.AddItem(new MenuItem("AutoShieldHP", "Auto Shield HP %")).SetValue(new Slider(40, 1, 99));
            }

            var autoEHarass = new Menu("AutoEHarass", "Auto E Harass");
            {
                autoShieldMenu.AddItem(new MenuItem("AutoEHarassMana", "Auto E Harass Mana %")).SetValue(new Slider(30, 1, 99));
                autoEHarass.AddItem(new MenuItem("AutoEHarass", "Auto E Harass")).SetValue(true);
            }

            var ultSettingsMenu = new Menu("UltSettings", "Ult Settings");
            {
                ultSettingsMenu.AddItem(new MenuItem("REnemyHealth", "R when Enemy Health %")).SetValue(new Slider(50, 1, 99));
            }

            var drawingMenu = new Menu("Drawings", "Drawings");
            {
                drawingMenu.AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
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
        public static bool HarassE { get { return config.Item("HarassE").GetValue<bool>(); } }
        public static int LastHitMana { get { return config.Item("LastHitMana").GetValue<Slider>().Value; } }
        public static bool LastHitE { get { return config.Item("LastHitE").GetValue<bool>(); } }
        public static bool InteruptQ { get { return config.Item("InteruptQ").GetValue<bool>(); } }
        public static bool AutoShield { get { return config.Item("AutoShield").GetValue<bool>(); } }
        public static int AutoShieldMana { get { return config.Item("AutoShieldMana").GetValue<Slider>().Value; } }
        public static int AutoShieldHP { get { return config.Item("AutoShieldHP").GetValue<Slider>().Value; } }
        public static int AutoEHarassMana { get { return config.Item("AutoEHarassMana").GetValue<Slider>().Value; } }
        public static bool AutoEHarass { get { return config.Item("AutoEHarass").GetValue<bool>(); } }
        public static int REnemyHealth { get { return config.Item("REnemyHealth").GetValue<Slider>().Value; } }
        public static bool DrawQ { get { return config.Item("DrawQ").GetValue<bool>(); } }
        public static bool DrawE { get { return config.Item("DrawE").GetValue<bool>(); } }
        public static bool DrawR { get { return config.Item("DrawR").GetValue<bool>(); } }
    }
}
