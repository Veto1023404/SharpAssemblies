using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;


namespace Nami
{
    class Nami
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        private static Obj_AI_Base target;
        private static Obj_AI_Base sender;

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
            AntiGapcloser.OnEnemyGapcloser += Anti_GapCloser;
            Obj_AI_Hero.OnProcessSpellCast += OnProcessSpellCast;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead || args == null)
                return;

            if (MenuConfig.HealPlayer)
                SelfHeal();

            if (MenuConfig.HealAlly)
                AllyHeal();

            if (MenuConfig.QOnSlow)
                QOnSlow();

            if (MenuConfig.QOnStun)
                QOnStun();

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

            if (MenuConfig.ComboQ && SpellManager.Q.IsReady())
            {
                var qPredict = SpellManager.Q.GetPrediction(target).Hitchance;

                if (qPredict >= HitChance.Medium)
                    SpellManager.Q.Cast(target, true);
            }

            if (MenuConfig.ComboW && SpellManager.W.IsReady() && ally.Health <= 40 * 100)
                SpellManager.W.Cast(ally);
            else if (MenuConfig.ComboW && SpellManager.W.IsReady())
                SpellManager.W.Cast(target);

            if (MenuConfig.ComboE && SpellManager.E.IsReady())
            { 
                if (SpellManager.E.IsInRange(ally))
                    SpellManager.E.CastOnUnit(ally);
                else if (SpellManager.E.IsReady())
                    SpellManager.E.CastOnUnit(Player);
                else
                    return;
            }

            if (MenuConfig.ComboR && SpellManager.R.IsReady() && SpellManager.R.IsInRange(target))
                SpellManager.R.CastIfHitchanceEquals(target, HitChance.High, true);

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

            if (MenuConfig.HarassE && SpellManager.E.IsReady())
            {
                var ally = HeroManager.Allies.Where(hero => hero.IsAlly && MenuConfig.HarassE)
                    .OrderBy(closest => closest.Distance(target))
                    .FirstOrDefault();

                if (SpellManager.E.IsInRange(ally))
                    SpellManager.E.CastOnUnit(ally);
            }

            if (MenuConfig.HarassW && SpellManager.W.IsReady())
                SpellManager.W.Cast(target);
        }

        private static void LaneClear()
        {
            if (Player.ManaPercent > MenuConfig.LaneClearMana)
                return;

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
                    SpellManager.CastSpell(SpellManager.W, Minion, HitChance.High);
            }
        }

        private static void SelfHeal()
        {
            if (Player.IsRecalling())
                return;

            if (MenuConfig.HealPlayer && (Player.Health / Player.MaxHealth) * 100 <= MenuConfig.MinPlayerHP && SpellManager.W.IsReady()
                && Player.ManaPercent >= MenuConfig.HealMana)
                SpellManager.W.Cast(Player);

        }

        private static void AllyHeal()
        {
            if (ObjectManager.Player.IsRecalling())
                return;

            // var ally = HeroManager.Allies.Where(hero => !hero.IsMe);

            // if (MenuConfig.HealAlly && (ally.Health / ally.MaxHealth) * 100 <= MenuConfig.MinAllyHP && SpellManager.W.IsReady()
               //  && Player.ManaPercent >= MenuConfig.HealMana)
                // SpellManager.W.Cast(ally);

            foreach (var ally in ObjectManager.Get< Obj_AI_Hero>().Where(ally => ally.IsAlly && !ally.IsMe))
            {
                if (MenuConfig.HealAlly && (ally.Health / ally.MaxHealth) * 100 <= MenuConfig.MinAllyHP && SpellManager.W.IsReady()
                    && ally.Distance(Player.ServerPosition) <= SpellManager.W.Range && Player.ManaPercent >= MenuConfig.HealMana)
                    SpellManager.W.Cast(ally);
            }
        }

        private static void QOnSlow()
        {
            if (MenuConfig.QOnSlow && SpellManager.Q.IsReady())
            {
                var target = HeroManager.Enemies.FirstOrDefault(t => t.IsValidTarget(SpellManager.Q.Range)
                && t.HasBuffOfType(BuffType.Slow));

                var qHitchance = SpellManager.Q.GetPrediction(target);

                if (target != null)
                    if (qHitchance.Hitchance >= HitChance.High)
                        SpellManager.Q.Cast(target);
            }
        }

        private static void QOnStun()
        {
            if (MenuConfig.QOnStun && SpellManager.Q.IsReady())
            {
                var target = HeroManager.Enemies.FirstOrDefault(t => t.IsValidTarget(SpellManager.Q.Range)
                && t.IsStunned || t.HasBuffOfType(BuffType.Stun));

                var qHitchance = SpellManager.Q.GetPrediction(target);

                if (target != null)
                    if (qHitchance.Hitchance >= HitChance.High)
                        SpellManager.Q.Cast(target);
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

        private static void Anti_GapCloser(ActiveGapcloser gapCloser)
        {
            if (!MenuConfig.GapcloseQ)
                return;

            if (gapCloser.Sender.IsAlly || gapCloser.Sender.IsMe)
                return;

            if (Player.Distance(gapCloser.Sender) <= SpellManager.Q.Range)
                SpellManager.Q.Cast(gapCloser.Sender);
        }

        private static void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsAlly)
                return;
            if (sender.CharData.Equals("Tristana") && MenuConfig.AntiTristana)
                AntiTristana(sender, args);
        }

        private static void AntiTristana(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (args.Slot.Equals(SpellSlot.W) && args.End.Distance(Player.Position) <= SpellManager.Q.Range)
            {
                Vector3 TristPosition = new Vector3();

                if (args.End.Distance(args.Start) >= 825)
                    TristPosition = sender.Position.Extend(args.End, 825);

                if (args.Start.Distance(args.End) < 825)
                    TristPosition = args.End;

                if (MenuConfig.AntiTristana && SpellManager.Q.IsReady())
                {
                    if (TristPosition.Distance(Player.Position) <= SpellManager.Q.Range)
                    {
                        Utility.DelayAction.Add((int)(500 + Player.Distance(TristPosition) / args.SData.MissileSpeed -
                            (Player.Distance(TristPosition) / SpellManager.Q.Speed) - 250), () =>
                            {
                                if (TristPosition.Distance(Player.Position) < SpellManager.Q.Range)
                                {
                                    SpellManager.Q.Cast(TristPosition);
                                }
                            });
                    }
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (MenuConfig.DrawQ && SpellManager.Q.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.Q.Range, Color.LightBlue);

            if (MenuConfig.DrawW && SpellManager.W.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.W.Range, Color.Blue);

            if (MenuConfig.DrawE && SpellManager.E.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.E.Range, Color.DarkBlue);

            if (MenuConfig.DrawR && SpellManager.R.Level > 0)
                Render.Circle.DrawCircle(Player.Position, SpellManager.R.Range, Color.Blue);
        }
    }
}
