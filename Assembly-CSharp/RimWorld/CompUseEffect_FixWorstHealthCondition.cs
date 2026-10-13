using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F5D RID: 12125
	public class CompUseEffect_FixWorstHealthCondition : CompUseEffect
	{
		// Token: 0x06011174 RID: 70004 RVA: 0x004E857C File Offset: 0x004E677C
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			Hediff hediff;
			BodyPartRecord bodyPartRecord;
			if (!HealthUtility.TryGetWorstHealthCondition(p, out hediff, out bodyPartRecord, Array.Empty<HediffDef>()))
			{
				return "AbilityCannotCastNoHealableInjury".Translate(p.Named("PAWN")).Resolve().StripTags() ?? "";
			}
			return true;
		}

		// Token: 0x06011175 RID: 70005 RVA: 0x004E85D4 File Offset: 0x004E67D4
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			TaggedString taggedString = HealthUtility.FixWorstHealthCondition(usedBy, Array.Empty<HediffDef>());
			if (PawnUtility.ShouldSendNotificationAbout(usedBy))
			{
				Messages.Message(taggedString, usedBy, MessageTypeDefOf.PositiveEvent, true);
			}
		}
	}
}
