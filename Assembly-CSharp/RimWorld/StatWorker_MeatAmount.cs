using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003759 RID: 14169
	public class StatWorker_MeatAmount : StatWorker
	{
		// Token: 0x06013A37 RID: 80439 RVA: 0x005D553C File Offset: 0x005D373C
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn = req.Pawn;
			bool? flag;
			if (pawn == null)
			{
				flag = null;
			}
			else
			{
				RaceProperties raceProps = pawn.RaceProps;
				flag = ((raceProps != null) ? new bool?(raceProps.hasMeat) : null);
			}
			bool? flag2 = flag;
			return !flag2.GetValueOrDefault() && base.ShouldShowFor(req);
		}

		// Token: 0x06013A38 RID: 80440 RVA: 0x005D5590 File Offset: 0x005D3790
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			foreach (Dialog_InfoCard.Hyperlink hyperlink in base.GetInfoCardHyperlinks(statRequest))
			{
				yield return hyperlink;
			}
			IEnumerator<Dialog_InfoCard.Hyperlink> enumerator = null;
			if (!statRequest.HasThing || statRequest.Thing.def.race == null || statRequest.Thing.def.race.meatDef == null)
			{
				yield break;
			}
			yield return new Dialog_InfoCard.Hyperlink(statRequest.Thing.def.race.meatDef, -1);
			yield break;
			yield break;
		}
	}
}
