using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001809 RID: 6153
	public class Thought_Situational_Precept_HighLife : Thought_Situational
	{
		// Token: 0x170015BF RID: 5567
		// (get) Token: 0x06008EA4 RID: 36516 RVA: 0x002A39B8 File Offset: 0x002A1BB8
		protected override float BaseMoodOffset
		{
			get
			{
				if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
				{
					return 0f;
				}
				float x = (float)(Find.TickManager.TicksGame - this.pawn.mindState.lastTakeRecreationalDrugTick) / 60000f;
				return (float)Mathf.RoundToInt(ThoughtWorker_Precept_HighLife.MoodOffsetFromDaysSinceLastDrugCurve.Evaluate(x));
			}
		}
	}
}
