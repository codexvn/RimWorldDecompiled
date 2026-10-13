using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018D1 RID: 6353
	public class Thought_Situational_Recluse : Thought_Situational
	{
		// Token: 0x060090C7 RID: 37063 RVA: 0x002AA2A8 File Offset: 0x002A84A8
		public override float MoodOffset()
		{
			int freeColonistsAndPrisonersSpawnedCount = this.pawn.Map.mapPawns.FreeColonistsAndPrisonersSpawnedCount;
			return Thought_Situational_Recluse.MoodOffsetCurveFromPopulation.Evaluate((float)freeColonistsAndPrisonersSpawnedCount);
		}

		// Token: 0x04005C5B RID: 23643
		public const int NumPawns_Alone = 1;

		// Token: 0x04005C5C RID: 23644
		public const int NumPawns_NotCrowded = 4;

		// Token: 0x04005C5D RID: 23645
		public const int NumPawns_Inactive = 10;

		// Token: 0x04005C5E RID: 23646
		public const int NumPawns_Crowded = 15;

		// Token: 0x04005C5F RID: 23647
		private static readonly SimpleCurve MoodOffsetCurveFromPopulation = new SimpleCurve
		{
			{
				new CurvePoint(1f, 12f),
				true
			},
			{
				new CurvePoint(2f, 8f),
				true
			},
			{
				new CurvePoint(3f, 6f),
				true
			},
			{
				new CurvePoint(4f, 4f),
				true
			},
			{
				new CurvePoint(5f, 0f),
				true
			},
			{
				new CurvePoint(10f, 0f),
				true
			},
			{
				new CurvePoint(11f, -2f),
				true
			},
			{
				new CurvePoint(12f, -3f),
				true
			},
			{
				new CurvePoint(13f, -4f),
				true
			},
			{
				new CurvePoint(14f, -5f),
				true
			},
			{
				new CurvePoint(15f, -6f),
				true
			},
			{
				new CurvePoint(16f, -8f),
				true
			}
		};
	}
}
