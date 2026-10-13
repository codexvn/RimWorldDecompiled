using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020026BF RID: 9919
	public class RitualStage_AnimaTreeLinking : RitualStage
	{
		// Token: 0x0600D972 RID: 55666 RVA: 0x00405DFB File Offset: 0x00403FFB
		public override TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return ritual.selectedTarget;
		}

		// Token: 0x0600D973 RID: 55667 RVA: 0x00405E04 File Offset: 0x00404004
		public override float ProgressPerTick(LordJob_Ritual ritual)
		{
			int num = 1;
			foreach (Pawn p in ritual.assignments.SpectatorsForReading)
			{
				if (ritual.IsParticipating(p))
				{
					num++;
				}
			}
			return RitualStage_AnimaTreeLinking.ProgressPerParticipantCurve.Evaluate((float)num);
		}

		// Token: 0x0400935D RID: 37725
		public static readonly SimpleCurve ProgressPerParticipantCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 1f),
				true
			},
			{
				new CurvePoint(2f, 1.2f),
				true
			},
			{
				new CurvePoint(4f, 1.5f),
				true
			},
			{
				new CurvePoint(6f, 2f),
				true
			},
			{
				new CurvePoint(8f, 3f),
				true
			}
		};
	}
}
