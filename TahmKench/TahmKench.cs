using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace TahmKench
{
    class TahmKench
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static SwallowedTarget current = SwallowedTarget.None; // Credits to Nouser
        public static string tahmPassive = "TahmKenchPDebuffCounter";

        public enum SwallowedTarget
        {
            Ally,
            Enemy,
            Minion,
            None
        }

        internal static void Load(EventArgs args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "TahmKench")
                return;

            SpellManager.Initialise();
            MenuConfig.LoadMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead || args == null)
                return;

            KillSteal();

            switch (MenuConfig.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    // LaneClear();
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }

        private static void Combo()
        {
            var target = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Physical);
            var closestMinion = ObjectManager.Get<Obj_AI_Minion>().Where(enemy => enemy.IsEnemy && enemy.Distance(Player) < 250).FirstOrDefault();

            if (target != null)
            {
                int buffStack = target.GetBuffCount(tahmPassive);
                switch (buffStack)
                {
                    case -1:
                        if (MenuConfig.ComboQ)
                            SpellManager.Q.Cast(target);
                        else if (target.Distance(Player) <= 200)
                            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        break; 

                    case 1:
                        if (MenuConfig.ComboQ)
                            SpellManager.Q.Cast(target);
                        else if (target.Distance(Player) <= 200)
                            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        break;
                        

                    case 2:
                        if (MenuConfig.ComboQ)
                            SpellManager.Q.Cast(target);
                        else if (target.Distance(Player) <= 200)
                            Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                        else
                        {
                            if (current == SwallowedTarget.None)
                                SpellManager.W.CastOnUnit(closestMinion);
                            else if (current == SwallowedTarget.Minion)
                                SpellManager.W2.CastIfHitchanceEquals(target, HitChance.High);
                        }
                        break;

                    case 3:
                        if (current == SwallowedTarget.None && MenuConfig.ComboW && target.Distance(Player) <= 250)
                            SpellManager.W.CastOnUnit(target);
                        else if (MenuConfig.ComboQ && target.Distance(Player) > 250)
                            SpellManager.Q.Cast(target);

                        break;
                }
            }
        }

        private static void Harass()
        {
            if (Player.ManaPercent < MenuConfig.HarassMana)
                return;

            if (SpellManager.Q.IsReady() && MenuConfig.HarassQ)
            {
                var target = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Physical);

                if (target != null)
                    SpellManager.CastSpell(SpellManager.Q, target, HitChance.High);
            }

        }

        /*
        private static void LaneClear()
        {
            // Gonna figure out a laneclear method for Tahm
        }
        */

        private static void LastHit()
        {
            if (Player.ManaPercent >= MenuConfig.LastHitMana)
            {
                var Minion = MinionManager.GetMinions(SpellManager.Q.Range, MinionTypes.All, MinionTeam.Enemy)
                        .FirstOrDefault(minion => minion.IsValidTarget(SpellManager.Q.Range) &&
                        Player.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health);

                if (SpellManager.Q.IsReady() && MenuConfig.LastHitQ && Minion.Distance(Player) >= Player.AttackRange)
                {
                    if (Minion != null)
                        SpellManager.CastSpell(SpellManager.Q, Minion, HitChance.High);
                }
                else
                    return;
            }
        }

        private static void KillSteal()
        {
            var target = ObjectManager.Get<Obj_AI_Hero>()
                .FirstOrDefault(enemy => enemy.IsValidTarget(SpellManager.Q.Range) && enemy.Health < Player.GetSpellDamage(enemy, SpellSlot.Q));

            if (target.IsValidTarget(SpellManager.Q.Range))
                SpellManager.Q.CastIfHitchanceEquals(target, HitChance.High);
        }

        private static void AutoShield()
        {
            if (MenuConfig.AutoShieldToggle && Player.Health <= MenuConfig.AutoShieldHP)
                SpellManager.E.Cast();
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (MenuConfig.DrawQ)
                Render.Circle.DrawCircle(Player.Position, SpellManager.Q.Range, Color.LightBlue);

            if (MenuConfig.DrawW)
            {
                if (current == SwallowedTarget.None)
                    Render.Circle.DrawCircle(Player.Position, SpellManager.W.Range, Color.Blue);
                else if (current == SwallowedTarget.Minion)
                    Render.Circle.DrawCircle(Player.Position, SpellManager.W2.Range, Color.DarkBlue);
            }
        }
    }
}