using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Karma
{
    class Karma
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        private static Obj_AI_Base target;
        private static Obj_AI_Base sender;
        private static bool MantraActive
        {
            get { return ObjectManager.Player.HasBuff("KarmaMantra"); }
        }

        internal static void Load(EventArgs args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnLoad;
        }

        static void Game_OnLoad(EventArgs args)
        {
            if (Player.ChampionName != "Karma")
                return;

            SpellManager.Initialise();
            MenuConfig.LoadMenu();

            Game.OnUpdate += Game_OnUpdate;
            Interrupter2.OnInterruptableTarget += OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += Anti_GapCloser;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead || args == null)
                return;

            SpellManager.Q.Width = MantraActive ? 80f : 60f;
            SpellManager.Q.Range = MantraActive ? 1250f : 1050f;

            if (MenuConfig.WOnStun && SpellManager.W.IsReady())
                WOnStun();

            if (MenuConfig.UseShield && SpellManager.E.IsReady() && MenuConfig.ShieldAlly)
                ShieldAlly();

            if (MenuConfig.UseShield && SpellManager.E.IsReady() && MenuConfig.ShieldSelf)
                ShieldPlayer();

            switch (MenuConfig.Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo(target);
                    break;

                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    LastHit();
                    break;

                case Orbwalking.OrbwalkingMode.LaneClear:
                    // LaneClear();
                    break;

                case Orbwalking.OrbwalkingMode.LastHit:
                    LastHit();
                    break;
            }
        }

        private static void Combo(Obj_AI_Base target)
        { 
            if (target == null || !target.IsValidTarget())
                return;

            var ally = HeroManager.Allies.Where(hero => hero.IsAlly && MenuConfig.ComboE)
                    .OrderBy(closest => closest.Distance(target))
                    .FirstOrDefault();

            var nearbyAllies = HeroManager.Allies.Where(hero => hero.IsMe && MenuConfig.UltE)
                .OrderBy(closest => closest.Distance(Player))
                .FirstOrDefault();

            if (MenuConfig.ComboR && SpellManager.R.IsReady())
            {
                if (MenuConfig.UltW && Player.Health <= MenuConfig.UltWHP * 100 && SpellManager.W.IsInRange(target) && SpellManager.W.IsReady())
                {
                    SpellManager.R.Cast(Player);
                    SpellManager.W.Cast(target);
                }
                else if (MenuConfig.UltE && ally.Health <= MenuConfig.UltEMinAllyHP && Player.CountAlliesInRange(SpellManager.E.Range) >= MenuConfig.UltEMinAlly 
                    && SpellManager.E.IsInRange(ally) && SpellManager.E.IsReady())
                {
                    SpellManager.R.Cast(Player);
                    SpellManager.E.Cast(Player);
                }
                else if (MenuConfig.UltE && ally.Health <= MenuConfig.UltEMinAllyHP && SpellManager.E.IsInRange(ally) && SpellManager.E.IsReady())
                {
                    SpellManager.R.Cast(Player);
                    SpellManager.E.Cast(Player);
                }
                else
                {
                    SpellManager.R.Cast(Player);
                    SpellManager.Q.Cast(target);
                }
            }

            if (MenuConfig.ComboQ && SpellManager.Q.IsReady())
            {
                var qPredict = SpellManager.Q.GetPrediction(target).Hitchance;

                if (qPredict >= HitChance.Medium)
                    SpellManager.Q.Cast(target, true);
            }

            if (MenuConfig.ComboW && SpellManager.W.IsReady())
                SpellManager.W.Cast(target);

            if (MenuConfig.ComboE && SpellManager.E.IsReady())
            {
                ShieldAlly();
                ShieldPlayer();
            }

            Ignite();
        }

        private static void Harass()
        {
            if (Player.ManaPercent > MenuConfig.HarassMana)
                return;

            var target = TargetSelector.GetTarget(SpellManager.Q.Range, TargetSelector.DamageType.Magical);
            var qPredict = SpellManager.Q.GetPrediction(target).Hitchance;

            if (MenuConfig.HarassQ && SpellManager.Q.IsReady())
            {
                if (qPredict >= HitChance.High)
                    SpellManager.Q.Cast(target);
            }

            if (MenuConfig.HarassQ && SpellManager.W.IsReady())
                SpellManager.W.Cast(target);
        }

        private static void LaneClear()
        {

        }

        private static void LastHit()
        {
            if (Player.ManaPercent >= MenuConfig.LastHitMana)
            {
                var Minion = MinionManager.GetMinions(SpellManager.Q.Range, MinionTypes.All, MinionTeam.Enemy)
                        .FirstOrDefault(minion => minion.IsValidTarget(SpellManager.Q.Range) &&
                        Player.GetSpellDamage(minion, SpellSlot.Q) >= minion.Health);

                if (Minion != null)
                    SpellManager.CastSpell(SpellManager.Q, Minion, HitChance.High);
                else if (Minion != null)
                    SpellManager.CastSpell(SpellManager.W, Minion, HitChance.High));
            }
        }

        private static void ShieldPlayer()
        {
            if (Player.IsRecalling())
                return;

            if (MenuConfig.ShieldSelf && (Player.Health / Player.MaxHealth) * 100 <= MenuConfig.ShieldMinPlayerHP && SpellManager.W.IsReady()
                && Player.ManaPercent >= MenuConfig.ShieldMana)
                SpellManager.W.Cast(Player);

        }

        private static void ShieldAlly()
        {
            if (ObjectManager.Player.IsRecalling())
                return;

            foreach (var ally in ObjectManager.Get<Obj_AI_Hero>().Where(ally => ally.IsAlly && !ally.IsMe))
            {
                if (MenuConfig.UseShield && (ally.Health / ally.MaxHealth) * 100 <= MenuConfig.ShieldMinAllyHP && SpellManager.E.IsReady()
                    && ally.Distance(Player.ServerPosition) <= SpellManager.E.Range && Player.ManaPercent >= MenuConfig.ShieldMana)
                    SpellManager.E.Cast(ally);
            }
        }

        private static void WOnStun()
        {
            if (MenuConfig.WOnStun && SpellManager.W.IsReady())
            {
                var target = HeroManager.Enemies.FirstOrDefault(t => t.IsValidTarget(SpellManager.W.Range)
                && t.IsStunned || t.HasBuffOfType(BuffType.Stun));


                if (target != null)
                    SpellManager.W.Cast(target);
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
        private static void Anti_GapCloser(ActiveGapcloser gapCloser)
        {
            if (!MenuConfig.GapcloseQ)
                return;

            if (gapCloser.Sender.IsAlly || gapCloser.Sender.IsMe)
                return;

            if (Player.Distance(gapCloser.Sender) <= SpellManager.Q.Range)
            {
                SpellManager.Q.Cast(gapCloser.Sender);
                SpellManager.E.Cast(Player);
            }

            if (Player.Distance(gapCloser.Sender) <= SpellManager.W.Range)
                SpellManager.W.Cast(gapCloser.Sender);
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (MenuConfig.DrawQ && SpellManager.Q.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.Q.Range, Color.LightBlue);

            if (MenuConfig.DrawW && SpellManager.W.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.W.Range, Color.Blue);

            if (MenuConfig.DrawE && SpellManager.E.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.E.Range, Color.DarkBlue);
        }
    }
}
