using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001883 RID: 6275
	public class ThoughtWorker_Hediff : ThoughtWorker
	{
		// Token: 0x06008FF4 RID: 36852 RVA: 0x002A7AB0 File Offset: 0x002A5CB0
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (((firstHediffOfDef != null) ? firstHediffOfDef.def.stages : null) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(Mathf.Min(new int[]
			{
				firstHediffOfDef.CurStageIndex,
				firstHediffOfDef.def.stages.Count - 1,
				this.def.stages.Count - 1
			}));
		}

		// Token: 0x06008FF5 RID: 36853 RVA: 0x002A7B38 File Offset: 0x002A5D38
		public override string PostProcessDescription(Pawn p, string description)
		{
			string text = base.PostProcessDescription(p, description);
			Hediff firstHediffOfDef = p.health.hediffSet.GetFirstHediffOfDef(this.def.hediff, false);
			if (firstHediffOfDef == null || !firstHediffOfDef.Visible)
			{
				return text;
			}
			return text + "\n\n" + "CausedBy".Translate() + ": " + firstHediffOfDef.LabelBase.CapitalizeFirst();
		}
	}
}
