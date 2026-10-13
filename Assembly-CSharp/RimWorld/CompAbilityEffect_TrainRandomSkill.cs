using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002297 RID: 8855
	public class CompAbilityEffect_TrainRandomSkill : CompAbilityEffect
	{
		// Token: 0x17001D7B RID: 7547
		// (get) Token: 0x0600C0C4 RID: 49348 RVA: 0x0037D253 File Offset: 0x0037B453
		public new CompProperties_AbilityTrainRandomSkill Props
		{
			get
			{
				return (CompProperties_AbilityTrainRandomSkill)this.props;
			}
		}

		// Token: 0x0600C0C5 RID: 49349 RVA: 0x0037D260 File Offset: 0x0037B460
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			if (target.Pawn != null)
			{
				SkillDef skillDef = (from x in DefDatabase<SkillDef>.AllDefs
				where !target.Pawn.skills.GetSkill(x).TotallyDisabled
				select x).RandomElement<SkillDef>();
				int level = target.Pawn.skills.GetSkill(skillDef).GetLevel(false);
				target.Pawn.skills.Learn(skillDef, 50000f, true, false);
				if (base.SendLetter)
				{
					int value = target.Pawn.skills.GetSkill(skillDef).GetLevel(false) - level;
					Find.LetterStack.ReceiveLetter(this.Props.customLetterLabel.Formatted(skillDef.LabelCap), this.Props.customLetterText.Formatted(this.parent.pawn, target.Pawn, skillDef, value), LetterDefOf.PositiveEvent, new LookTargets(target.Pawn), null, null, null, null, 0, true);
				}
			}
		}

		// Token: 0x040083EE RID: 33774
		private const float XPGainAmount = 50000f;
	}
}
