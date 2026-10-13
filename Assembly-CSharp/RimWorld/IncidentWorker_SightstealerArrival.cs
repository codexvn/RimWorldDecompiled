using System;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DD5 RID: 7637
	public class IncidentWorker_SightstealerArrival : IncidentWorker
	{
		// Token: 0x0600AA57 RID: 43607 RVA: 0x0030B8E0 File Offset: 0x00309AE0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Slate slate = new Slate();
			Map map = QuestGen_Get.GetMap(false, null, false);
			slate.Set<Map>("map", map, false);
			slate.Set<float>("points", parms.points * IncidentWorker_SightstealerArrival.SightstealerPointsFactorCurve.Evaluate(parms.points), false);
			if (!QuestScriptDefOf.SightstealerArrival.CanRun(slate, map))
			{
				return false;
			}
			QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.SightstealerArrival, slate);
			return true;
		}

		// Token: 0x04007581 RID: 30081
		public static readonly SimpleCurve SightstealerPointsFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(4000f, 1f),
				true
			},
			{
				new CurvePoint(10000f, 0.6f),
				true
			}
		};
	}
}
