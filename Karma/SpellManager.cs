using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Karma
{
    class SpellManager
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;

        public static Spell Q, W, E, R;
        public static SpellSlot Ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");

        public static void Initialise()
        {
            Q = new Spell(SpellSlot.Q, 1050f);
            W = new Spell(SpellSlot.W, 700f);
            E = new Spell(SpellSlot.E, 800f);
            R = new Spell(SpellSlot.R);

            Q.SetSkillshot(0.25f, 60f, 1700f, true, SkillshotType.SkillshotLine);
            W.SetTargetted(0.25f, 2200f);
            E.SetTargetted(0.25f, float.MaxValue);
        }

        public static void CastSpell(Spell spell, Obj_AI_Base target, HitChance hitchance)
        {
            if (target.IsValidTarget(spell.Range) && spell.GetPrediction(target).Hitchance >= hitchance)
                spell.Cast(target);
        }
    }
}
