using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200300B RID: 12299
	public class Alert_EmergingPitGate : Alert
	{
		// Token: 0x060114F1 RID: 70897 RVA: 0x004F64E9 File Offset: 0x004F46E9
		public Alert_EmergingPitGate()
		{
			this.defaultPriority = AlertPriority.High;
			this.requireAnomaly = true;
		}

		// Token: 0x17002CDC RID: 11484
		// (get) Token: 0x060114F2 RID: 70898 RVA: 0x004F64FF File Offset: 0x004F46FF
		private BuildingGroundSpawner EmergingPitGate
		{
			get
			{
				Map currentMap = Find.CurrentMap;
				this.pitGate = (((currentMap != null) ? currentMap.listerThings.ThingsOfDef(ThingDefOf.PitGateSpawner).FirstOrDefault<Thing>() : null) as BuildingGroundSpawner);
				return this.pitGate;
			}
		}

		// Token: 0x060114F3 RID: 70899 RVA: 0x004F6532 File Offset: 0x004F4732
		public override string GetLabel()
		{
			return "AlertEmergingPitGate".Translate() + ": " + this.pitGate.TicksUntilSpawn.ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x060114F4 RID: 70900 RVA: 0x004F6564 File Offset: 0x004F4764
		public override TaggedString GetExplanation()
		{
			return string.Format("AlertEmergingPitGateDesc".Translate(), Array.Empty<object>());
		}

		// Token: 0x060114F5 RID: 70901 RVA: 0x004F6584 File Offset: 0x004F4784
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritIs(this.EmergingPitGate);
		}

		// Token: 0x0400B365 RID: 45925
		private BuildingGroundSpawner pitGate;
	}
}
