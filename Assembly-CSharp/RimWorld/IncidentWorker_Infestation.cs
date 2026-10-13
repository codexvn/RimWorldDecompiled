using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAC RID: 7596
	public class IncidentWorker_Infestation : IncidentWorker
	{
		// Token: 0x0600A9C9 RID: 43465 RVA: 0x0030957C File Offset: 0x0030777C
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			return base.CanFireNowSub(parms) && Faction.OfInsects != null && HiveUtility.TotalSpawnedHivesCount(map, false) < 30 && InfestationCellFinder.TryFindCell(out intVec, map);
		}

		// Token: 0x0600A9CA RID: 43466 RVA: 0x003095BC File Offset: 0x003077BC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			parms.points *= IncidentWorker_Infestation.PointsFactorCurve.Evaluate(parms.points);
			Thing t = InfestationUtility.SpawnTunnels(Mathf.Max(GenMath.RoundRandom(parms.points / 220f), 1), map, false, parms.infestationLocOverride != null, null, parms.infestationLocOverride, null);
			base.SendStandardLetter(parms, t, Array.Empty<NamedArgument>());
			Find.TickManager.slower.SignalForceNormalSpeedShort();
			return true;
		}

		// Token: 0x04007556 RID: 30038
		public const float HivePoints = 220f;

		// Token: 0x04007557 RID: 30039
		public static readonly SimpleCurve PointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.7f),
				true
			},
			{
				new CurvePoint(5000f, 0.45f),
				true
			}
		};
	}
}
