using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200271C RID: 10012
	public class RitualObligationTargetWorker_GravshipLaunch : RitualObligationTargetFilter
	{
		// Token: 0x0600DBC9 RID: 56265 RVA: 0x00408B0F File Offset: 0x00406D0F
		public RitualObligationTargetWorker_GravshipLaunch()
		{
		}

		// Token: 0x0600DBCA RID: 56266 RVA: 0x00408B17 File Offset: 0x00406D17
		public RitualObligationTargetWorker_GravshipLaunch(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x0600DBCB RID: 56267 RVA: 0x0040CBE7 File Offset: 0x0040ADE7
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			List<Thing> list = map.listerThings.ThingsOfDef(ThingDefOf.PilotConsole);
			foreach (Thing thing in list)
			{
				CompPilotConsole compPilotConsole;
				if (thing.TryGetComp(out compPilotConsole) && compPilotConsole.CanUseNow())
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600DBCC RID: 56268 RVA: 0x0040CBF8 File Offset: 0x0040ADF8
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!target.HasThing)
			{
				return false;
			}
			CompPilotConsole compPilotConsole;
			if (!target.Thing.TryGetComp(out compPilotConsole))
			{
				return false;
			}
			AcceptanceReport acceptanceReport = compPilotConsole.CanUseNow();
			if (!acceptanceReport.Accepted)
			{
				return acceptanceReport.Reason;
			}
			return true;
		}

		// Token: 0x0600DBCD RID: 56269 RVA: 0x0040CC4F File Offset: 0x0040AE4F
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetPilotConsole".Translate();
			yield break;
		}
	}
}
