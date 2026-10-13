using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003006 RID: 12294
	public class Alert_ToxicBuildup : Alert
	{
		// Token: 0x060114DE RID: 70878 RVA: 0x004F5F8F File Offset: 0x004F418F
		public Alert_ToxicBuildup()
		{
			this.defaultLabel = "AlertToxicBuildup".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireBiotech = true;
		}

		// Token: 0x17002CD9 RID: 11481
		// (get) Token: 0x060114DF RID: 70879 RVA: 0x004F5FC8 File Offset: 0x004F41C8
		private List<Pawn> ToxicBuildupColonists
		{
			get
			{
				this.toxicBuildupColonists.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned_FreeColonists_NoSuspended)
				{
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity >= 0.6f)
					{
						this.toxicBuildupColonists.Add(pawn);
					}
				}
				return this.toxicBuildupColonists;
			}
		}

		// Token: 0x060114E0 RID: 70880 RVA: 0x004F6058 File Offset: 0x004F4258
		public override TaggedString GetExplanation()
		{
			string value = (from p in this.toxicBuildupColonists
			select p.NameShortColored.Resolve()).ToLineList(" - ", false);
			return "AlertToxicBuildupDesc".Translate(value);
		}

		// Token: 0x060114E1 RID: 70881 RVA: 0x004F60AB File Offset: 0x004F42AB
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ToxicBuildupColonists);
		}

		// Token: 0x0400B35A RID: 45914
		private const float ToxicBuildupSeverityThreshold = 0.6f;

		// Token: 0x0400B35B RID: 45915
		private List<Pawn> toxicBuildupColonists = new List<Pawn>();
	}
}
