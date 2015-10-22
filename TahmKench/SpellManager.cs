using LeagueSharp;
using LeagueSharp.Common;

namespace TahmKench
{
    public class SpellManager
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;

        public static Spell Q, W, W2, E, R;

        public static void Initialise()
        {
            Q = new Spell(SpellSlot.Q, 800);
            Q.SetSkillshot(.1f, 75, 2000, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 250);
            W2 = new Spell(SpellSlot.W, 900);
            W2.SetSkillshot(.1f, 75, 900, true, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E);

            R = new Spell(SpellSlot.R, 4000);
        }

        public static void CastSpell(Spell spell, Obj_AI_Base target, HitChance hitchance)
        {
            if (target.IsValidTarget(spell.Range) && spell.GetPrediction(target).Hitchance >= hitchance)
                spell.Cast(target);
        }
    }
}
