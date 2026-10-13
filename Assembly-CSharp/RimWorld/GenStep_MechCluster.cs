using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002071 RID: 8305
	public class GenStep_MechCluster : GenStep
	{
		// Token: 0x17001BD4 RID: 7124
		// (get) Token: 0x0600B62B RID: 46635 RVA: 0x00345501 File Offset: 0x00343701
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x0600B62C RID: 46636 RVA: 0x00345508 File Offset: 0x00343708
		public override void Generate(Map map, GenStepParams parms)
		{
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.sitePart.parms.threatPoints, map, true, this.forceNoConditionCauser);
			IntVec3 center = IntVec3.Invalid;
			CellRect cellRect;
			if (MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				center = cellRect.ExpandedBy(this.extraRangeToRectOfInterest).MaxBy((IntVec3 x) => MechClusterUtility.GetClusterPositionScore(x, map, sketch));
			}
			if (!center.IsValid)
			{
				center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0f);
			}
			List<Thing> list = MechClusterUtility.SpawnCluster(center, map, sketch, false, false, null);
			List<Pawn> list2 = new List<Pawn>();
			foreach (Thing thing in list)
			{
				if (thing is Pawn)
				{
					list2.Add((Pawn)thing);
				}
			}
			if (!list2.Any<Pawn>())
			{
				return;
			}
			GenStep_SleepingMechanoids.SendMechanoidsToSleepImmediately(list2);
		}

		// Token: 0x04007CF2 RID: 31986
		public bool forceNoConditionCauser;

		// Token: 0x04007CF3 RID: 31987
		public int extraRangeToRectOfInterest = 20;
	}
}
