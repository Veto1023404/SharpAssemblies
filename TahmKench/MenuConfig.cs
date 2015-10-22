using LeagueSharp;
using LeagueSharp.Common;

namespace TahmKench
{
    internal class MenuConfig
    {
        public static Menu config;
        public static string menuName = "TahmKench";
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
                comboMenu.AddItem(new MenuItem("ComboW", "Use W")).SetValue(false);
                comboMenu.AddItem(new MenuItem("ComboE", "Use E")).SetValue(true);

                config.AddSubMenu(comboMenu);
            }

            var harassMenu = new Menu("Harass", "Harass");
            {
                harassMenu.AddItem(new MenuItem("HarassMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                harassMenu.AddItem(new MenuItem("HarassQ", "Use Q")).SetValue(true);
                harassMenu.AddItem(new MenuItem("HarassW2", "Spit Minions?")).SetValue(true);

                config.AddSubMenu(harassMenu);
            }

            var farmMenu = new Menu("Farming", "Farming");
            {
                /*
                var laneclearMenu = new Menu("LaneClear", "Lane Clear");
                {
                    laneclearMenu.AddItem(new MenuItem("Mana Percent", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    laneclearMenu.AddItem(new MenuItem("Use Q", "Use Q")).SetValue(true);
                    laneclearMenu.AddItem(new MenuItem("Use W", "Use W")).SetValue(true);

                    farmMenu.AddSubMenu(laneclearMenu);
                }
                */
                var lasthitMenu = new Menu("LastHit", "Last Hit");
                {
                    lasthitMenu.AddItem(new MenuItem("LastHitMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    lasthitMenu.AddItem(new MenuItem("LastHitQ", "Use Q")).SetValue(true);

                    farmMenu.AddSubMenu(lasthitMenu);
                }
                var jungleMenu = new Menu("JungleClear", "Jungle Clear");
                {
                    lasthitMenu.AddItem(new MenuItem("JungleMana", "Mana % Usage")).SetValue(new Slider(40, 1, 99));
                    jungleMenu.AddItem(new MenuItem("JungleQ", "Use Q")).SetValue(true);

                    farmMenu.AddSubMenu(jungleMenu);
                }

                config.AddSubMenu(farmMenu);
            }

            /*
            var fleeMenu = new Menu("Flee", "Flee");
            {
                fleeMenu.AddItem(new MenuItem("Flee", "Flee")).SetValue(new KeyBind('Z', KeyBindType.Press));
                fleeMenu.AddItem(new MenuItem("FleeQ", "Use Q")).SetValue(true);

                config.AddSubMenu(fleeMenu);
            }
            */

            var interuptMenu = new Menu("Interupt", "Interupt");
            {
                interuptMenu.AddItem(new MenuItem("InteruptQ", "Interupt with Q")).SetValue(true);
                interuptMenu.AddItem(new MenuItem("InteruptW", "Interupt with W")).SetValue(true);

                config.AddSubMenu(interuptMenu);
            }

            var killstealMenu = new Menu("Killsteal", "Killsteal");
            {
                killstealMenu.AddItem(new MenuItem("KillstealQ", "Killsteal with Q")).SetValue(true);

                config.AddSubMenu(killstealMenu);
            }

            var shieldMenu = new Menu("Auto Shield", "Auto Shield");
            {
                shieldMenu.AddItem(new MenuItem("AutoShieldToggle", "Toggle Auto Shield")).SetValue(true);
                shieldMenu.AddItem(new MenuItem("AutoShieldHP", "Auto Shield at HP %")).SetValue(new Slider(15, 1, 99));
            }

            var drawingMenu = new Menu("Drawings", "Drawings");
            {
                drawingMenu.AddItem(new MenuItem("DrawQ", "Draw Q")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawW", "Draw W")).SetValue(true);
                drawingMenu.AddItem(new MenuItem("DrawMaxW", "Draw Max W Movement Range")).SetValue(true);

                config.AddSubMenu(drawingMenu);
            }

            config.AddToMainMenu();
        }

        public static bool ComboQ { get { return config.Item("ComboQ").GetValue<bool>(); } }
        public static bool ComboW { get { return config.Item("ComboW").GetValue<bool>(); } }
        public static bool ComboE { get { return config.Item("ComboE").GetValue<bool>(); } }
        public static int HarassMana { get { return config.Item("HarassMana").GetValue<Slider>().Value; } }
        public static bool HarassQ { get { return config.Item("HarassQ").GetValue<bool>(); } }
        public static bool HarassW2 { get { return config.Item("HarassW2").GetValue<bool>(); } }
        public static int LastHitMana { get { return config.Item("LastHitMana").GetValue<Slider>().Value; } }
        public static bool LastHitQ { get { return config.Item("LastHitQ").GetValue<bool>(); } }
        public static int JungleMana { get { return config.Item("JungleMana").GetValue<Slider>().Value; } }
        public static bool JungleQ { get { return config.Item("JungleQ").GetValue<bool>(); } }
        public static bool InteruptQ { get { return config.Item("InteruptQ").GetValue<bool>(); } }
        public static bool InteruptW { get { return config.Item("InteruptW").GetValue<bool>(); } }
        public static bool KillstealQ { get { return config.Item("KillstealQ").GetValue<bool>(); } }
        public static bool AutoShieldToggle { get { return config.Item("AutoShieldToggle").GetValue<bool>(); } }
        public static int AutoShieldHP { get { return config.Item("AutoShieldHP").GetValue<Slider>().Value; } }
        public static bool DrawQ { get { return config.Item("DrawQ").GetValue<bool>(); } }
        public static bool DrawW { get { return config.Item("DrawW").GetValue<bool>(); } }
        public static bool DrawMaxW { get { return config.Item("DrawMaxW").GetValue<bool>(); } }
    }
}
