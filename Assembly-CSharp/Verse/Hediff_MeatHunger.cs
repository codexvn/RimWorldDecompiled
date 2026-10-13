using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000753 RID: 1875
	public class Hediff_MeatHunger : Hediff
	{
		// Token: 0x06002DDA RID: 11738 RVA: 0x000EBE90 File Offset: 0x000EA090
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Meat hunger"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x000EBEB8 File Offset: 0x000EA0B8
		public override void TickInterval(int delta)
		{
			int num = Hediff_MeatHunger.StageIndexToBetrayalMTBHours[this.CurStageIndex];
			if (num > 0 && Rand.MTBEventOccurs((float)num, 2500f, 1f) && this.pawn.IsColonySubhuman)
			{
				this.pawn.SetFaction(Faction.OfEntities, null);
				Find.LetterStack.ReceiveLetter("GhoulBetrayalLabel".Translate(), "GhoulBetrayalText".Translate(this.pawn.Named("PAWN")), LetterDefOf.ThreatBig, this.pawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x04002428 RID: 9256
		private static readonly Dictionary<int, int> StageIndexToBetrayalMTBHours = new Dictionary<int, int>
		{
			{
				0,
				0
			},
			{
				1,
				0
			},
			{
				2,
				8
			},
			{
				3,
				2
			}
		};
	}
}
