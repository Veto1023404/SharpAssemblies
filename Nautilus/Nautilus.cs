using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Nautilus
{
    class Nautilus
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        private static Obj_AI_Base target;

        internal static void Load(EventArgs args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnLoad;
        }

        static void Game_OnLoad(EventArgs args)
        {
            if (Player.ChampionName != "Nami")
                return;

            SpellManager.Initialise();
            MenuConfig.LoadMenu();

            Game.OnUpdate += Game_OnUpdate;
            Interrupter2.OnInterruptableTarget += OnInterruptableTarget;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead || args == null)
                return;

            AutoShield();
            AutoEHarass();

            switch (MenuConfig.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo(target);
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    // LaneClear();
                    LastHit();
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }

        private static void Combo(Obj_AI_Base target)
        {
            var qTarget = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Magical);
            var eTarget = TargetSelector.GetTarget(SpellManager.E.Range, TargetSelector.DamageType.Magical);
            var rTarget = TargetSelector.GetTarget(SpellManager.R.Range, TargetSelector.DamageType.Magical);

            if (target == null || !target.IsValidTarget())
                return;

            if (MenuConfig.ComboQ && SpellManager.Q.IsReady())
            {
                var qPredict = SpellManager.Q.GetPrediction(target).Hitchance;

                if (qPredict >= HitChance.High)
                    SpellManager.Q.Cast(target, true);
            }

            if (MenuConfig.ComboE && SpellManager.E.IsInRange(target) && SpellManager.E.IsReady())
                SpellManager.E.Cast();

            if (MenuConfig.ComboR && SpellManager.R.IsReady() && target.Health <= MenuConfig.REnemyHealth)
                SpellManager.R.Cast(target);

            Ignite();
        }

        private static void Harass()
        {
            if (Player.ManaPercent > MenuConfig.HarassMana)
                return;

            var qTarget = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Magical);
            var qPredict = SpellManager.Q.GetPrediction(qTarget).Hitchance;
            var eTarget = TargetSelector.GetTarget(SpellManager.E.Range, TargetSelector.DamageType.Magical);
            var ePredict = SpellManager.E.GetPrediction(eTarget).Hitchance;

            if (MenuConfig.HarassQ && SpellManager.Q.IsReady())
            {
                if (qPredict >= HitChance.High)
                    SpellManager.Q.Cast(qTarget);
            }

            if (MenuConfig.HarassE && SpellManager.E.IsReady())
            {
                if (ePredict >= HitChance.Medium)
                    SpellManager.E.Cast(eTarget);
            }
        }

        private static void LastHit()
        {
            if (Player.ManaPercent >= MenuConfig.LastHitMana)
            {
                var Minion = MinionManager.GetMinions(SpellManager.E.Range, MinionTypes.All, MinionTeam.Enemy)
                        .FirstOrDefault(minion => minion.IsValidTarget(SpellManager.Q.Range) &&
                        Player.GetSpellDamage(minion, SpellSlot.E) >= minion.Health);

                if (Minion != null)
                    SpellManager.CastSpell(SpellManager.E, Minion, HitChance.High);
            }
        }

        private static void AutoShield()
        {
            if (Player.IsRecalling() || Player.InFountain())
                return;

            if (MenuConfig.AutoShield && (Player.Health / Player.MaxHealth) * 100 <= MenuConfig.AutoShieldHP && SpellManager.W.IsReady() 
                && Player.ManaPercent >= MenuConfig.AutoShieldMana)
                SpellManager.W.Cast(Player);
        }

        private static void AutoEHarass()
        {
            var eTarget = TargetSelector.GetTarget(SpellManager.E.Range, TargetSelector.DamageType.Magical);
            var ePredict = SpellManager.E.GetPrediction(eTarget).Hitchance;

            if (MenuConfig.AutoEHarass && SpellManager.E.IsReady() && Player.ManaPercent >= MenuConfig.AutoEHarassMana)
            {
                if (ePredict >= HitChance.Medium)
                    SpellManager.E.Cast(eTarget);
            }
        }

        private static void Ignite()
        {
            if (target != null && SpellManager.Ignite != SpellSlot.Unknown &&
                ObjectManager.Player.Spellbook.CanUseSpell(SpellManager.Ignite) == SpellState.Ready)
            {
                if (Player.Distance(target) < 650 && Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite) >= target.Health)
                    Player.Spellbook.CastSpell(SpellManager.Ignite, target);
            }
        }

        private static void OnInterruptableTarget(Obj_AI_Hero enemy, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (args.DangerLevel != Interrupter2.DangerLevel.High || enemy.Distance(Player) > SpellManager.Q.Range)
                return;

            if (MenuConfig.InteruptQ && enemy.IsValidTarget(SpellManager.Q.Range) && args.DangerLevel == Interrupter2.DangerLevel.High && SpellManager.Q.IsReady())
                SpellManager.Q.Cast(enemy);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (MenuConfig.DrawQ && SpellManager.Q.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.Q.Range, Color.LightBlue);

            if (MenuConfig.DrawE && SpellManager.E.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.E.Range, Color.DarkBlue);

            if (MenuConfig.DrawR && SpellManager.R.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.R.Range, Color.Blue);
        }
    }
}
