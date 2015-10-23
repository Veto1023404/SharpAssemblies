using LeagueSharp;
using LeagueSharp.Common;

namespace Nautilus
{
    class SpellManager
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;

        public static Spell Q, W, E, R;
        public static SpellSlot Ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");

        public static void Initialise()
        {
            Q = new Spell(SpellSlot.Q, 900);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 550);
            R = new Spell(SpellSlot.R, 755);

            Q.SetSkillshot(250, 90, 2000, true, SkillshotType.SkillshotLine);
        }

        public static void CastSpell(Spell spell, Obj_AI_Base target, HitChance hitchance)
        {
            if (target.IsValidTarget(spell.Range) && spell.GetPrediction(target).Hitchance >= hitchance)
                spell.Cast(target);
        }
    }
}
