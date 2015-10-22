using LeagueSharp;
using LeagueSharp.Common;

namespace Nami
{
    class SpellManager
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;

        public static Spell Q, W, E, R;
        public static SpellSlot Ignite = ObjectManager.Player.GetSpellSlot("SummonerDot");

        public static void Initialise()
        {
            Q = new Spell(SpellSlot.Q, 875);
            W = new Spell(SpellSlot.W, 725);
            E = new Spell(SpellSlot.E, 800);
            R = new Spell(SpellSlot.R, 2200);

            Q.SetSkillshot(1f, 150f, float.MaxValue, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(0.5f, 260f, 850f, false, SkillshotType.SkillshotLine);
        }

        public static void CastSpell(Spell spell, Obj_AI_Base target, HitChance hitchance)
        {
            if (target.IsValidTarget(spell.Range) && spell.GetPrediction(target).Hitchance >= hitchance)
                spell.Cast(target);
        }
    }
}
