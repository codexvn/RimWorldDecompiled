using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001E69 RID: 7785
	public class StorytellerComp_ShipChunkDrop : StorytellerComp
	{
		// Token: 0x17001A67 RID: 6759
		// (get) Token: 0x0600ACF6 RID: 44278 RVA: 0x00315D58 File Offset: 0x00313F58
		private float ShipChunkDropMTBDays
		{
			get
			{
				float x = (float)Find.TickManager.TicksGame / 3600000f;
				return StorytellerComp_ShipChunkDrop.ShipChunkDropMTBDaysCurve.Evaluate(x);
			}
		}

		// Token: 0x0600ACF7 RID: 44279 RVA: 0x00315D82 File Offset: 0x00313F82
		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (Rand.MTBEventOccurs(this.ShipChunkDropMTBDays, 60000f, 1000f))
			{
				IncidentDef shipChunkDrop = IncidentDefOf.ShipChunkDrop;
				IncidentParms parms = this.GenerateParms(shipChunkDrop.category, target);
				if (shipChunkDrop.Worker.CanFireNow(parms))
				{
					yield return new FiringIncident(shipChunkDrop, this, parms);
				}
			}
			yield break;
		}

		// Token: 0x040076F9 RID: 30457
		private static readonly SimpleCurve ShipChunkDropMTBDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(1f, 40f),
				true
			},
			{
				new CurvePoint(2f, 80f),
				true
			},
			{
				new CurvePoint(2.75f, 135f),
				true
			}
		};
	}
}
