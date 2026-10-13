using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003859 RID: 14425
	public class Thought_PsychicHarmonizer : Thought_Memory
	{
		// Token: 0x170032ED RID: 13037
		// (get) Token: 0x06013F1C RID: 81692 RVA: 0x005EAEE4 File Offset: 0x005E90E4
		public override string LabelCap
		{
			get
			{
				return base.CurStage.label.Formatted(this.harmonizer.pawn.Named("HARMONIZER")).CapitalizeFirst();
			}
		}

		// Token: 0x06013F1D RID: 81693 RVA: 0x005EAF23 File Offset: 0x005E9123
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Hediff>(ref this.harmonizer, "harmonizer", false);
		}

		// Token: 0x06013F1E RID: 81694 RVA: 0x005EAF3C File Offset: 0x005E913C
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			if (this.ShouldDiscard)
			{
				return 0f;
			}
			float num = base.MoodOffset();
			float num2 = Mathf.Lerp(-1f, 1f, this.harmonizer.pawn.needs.mood.CurLevel);
			float statValue = this.harmonizer.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, 1);
			return num * num2 * statValue;
		}

		// Token: 0x06013F1F RID: 81695 RVA: 0x003D6395 File Offset: 0x003D4595
		public override bool TryMergeWithExistingMemory(out bool showBubble)
		{
			showBubble = false;
			return false;
		}

		// Token: 0x06013F20 RID: 81696 RVA: 0x005EAFBC File Offset: 0x005E91BC
		public override bool GroupsWith(Thought other)
		{
			Thought_PsychicHarmonizer thought_PsychicHarmonizer = other as Thought_PsychicHarmonizer;
			return thought_PsychicHarmonizer != null && thought_PsychicHarmonizer.harmonizer == this.harmonizer && base.GroupsWith(other);
		}

		// Token: 0x170032EE RID: 13038
		// (get) Token: 0x06013F21 RID: 81697 RVA: 0x005EAFEC File Offset: 0x005E91EC
		public override bool ShouldDiscard
		{
			get
			{
				Hediff hediff = this.harmonizer;
				Pawn pawn = (hediff != null) ? hediff.pawn : null;
				if (pawn == null)
				{
					return true;
				}
				if (!pawn.health.Dead)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (((needs != null) ? needs.mood : null) != null)
					{
						return this.pawn.health.hediffSet.HasHediff(HediffDefOf.PsychicHarmonizer, false) || ((pawn.Spawned || this.pawn.Spawned || pawn.GetCaravan() != this.pawn.GetCaravan()) && (!pawn.Spawned || !this.pawn.Spawned || pawn.Map != this.pawn.Map || pawn.Position.DistanceTo(this.pawn.Position) > this.harmonizer.TryGetComp<HediffComp_PsychicHarmonizer>().Props.range));
					}
				}
				return true;
			}
		}

		// Token: 0x0400DC43 RID: 56387
		public Hediff harmonizer;
	}
}
