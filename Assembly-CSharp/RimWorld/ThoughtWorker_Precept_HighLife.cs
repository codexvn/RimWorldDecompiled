using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001807 RID: 6151
	public class ThoughtWorker_Precept_HighLife : ThoughtWorker_Precept, IPreceptCompDescriptionArgs
	{
		// Token: 0x06008E98 RID: 36504 RVA: 0x002A37B8 File Offset: 0x002A19B8
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (ThoughtUtility.ThoughtNullified(p, this.def))
			{
				return false;
			}
			float num = (float)(Find.TickManager.TicksGame - p.mindState.lastTakeRecreationalDrugTick) / 60000f;
			if (num > 1f && this.def.minExpectationForNegativeThought != null && p.MapHeld != null && ExpectationsUtility.CurrentExpectationFor(p.MapHeld).order < this.def.minExpectationForNegativeThought.order)
			{
				return false;
			}
			if (num < 1f)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (num < 2f)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num < 11f)
			{
				return ThoughtState.ActiveAtStage(2);
			}
			return ThoughtState.ActiveAtStage(3);
		}

		// Token: 0x06008E99 RID: 36505 RVA: 0x002A3874 File Offset: 0x002A1A74
		public IEnumerable<NamedArgument> GetDescriptionArgs()
		{
			yield return 0.75f.ToString("F2").Named("DAYSSATISIFED");
			yield break;
		}

		// Token: 0x04005BFB RID: 23547
		private const float DaysSatisfied = 0.75f;

		// Token: 0x04005BFC RID: 23548
		private const float DaysNoBonus = 1f;

		// Token: 0x04005BFD RID: 23549
		private const float DaysMissing = 2f;

		// Token: 0x04005BFE RID: 23550
		private const float DaysMissing_Major = 11f;

		// Token: 0x04005BFF RID: 23551
		public static readonly SimpleCurve MoodOffsetFromDaysSinceLastDrugCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.75f, 3f),
				true
			},
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(2f, -1f),
				true
			},
			{
				new CurvePoint(11f, -10f),
				true
			}
		};
	}
}
