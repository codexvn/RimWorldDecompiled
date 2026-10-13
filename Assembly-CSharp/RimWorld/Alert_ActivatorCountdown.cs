using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003002 RID: 12290
	public class Alert_ActivatorCountdown : Alert
	{
		// Token: 0x060114CD RID: 70861 RVA: 0x004F5A8C File Offset: 0x004F3C8C
		public Alert_ActivatorCountdown()
		{
			this.defaultPriority = AlertPriority.High;
			this.requireRoyalty = true;
		}

		// Token: 0x17002CD5 RID: 11477
		// (get) Token: 0x060114CE RID: 70862 RVA: 0x004F5AB0 File Offset: 0x004F3CB0
		private List<Thing> ActivatorCountdowns
		{
			get
			{
				this.activatorCountdownsResult.Clear();
				foreach (Map map in Find.Maps)
				{
					if (map.mapPawns.AnyColonistSpawned)
					{
						foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForDef(ThingDefOf.ActivatorCountdown)))
						{
							CompSendSignalOnCountdown compSendSignalOnCountdown = thing.TryGetComp<CompSendSignalOnCountdown>();
							if (compSendSignalOnCountdown != null && compSendSignalOnCountdown.ticksLeft > 0)
							{
								this.activatorCountdownsResult.Add(thing);
							}
						}
					}
				}
				return this.activatorCountdownsResult;
			}
		}

		// Token: 0x060114CF RID: 70863 RVA: 0x004F5B88 File Offset: 0x004F3D88
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ActivatorCountdowns);
		}

		// Token: 0x060114D0 RID: 70864 RVA: 0x004F5B98 File Offset: 0x004F3D98
		public override string GetLabel()
		{
			int count = this.activatorCountdownsResult.Count;
			if (count > 1)
			{
				return "ActivatorCountdownMultiple".Translate(count);
			}
			if (count == 0)
			{
				return string.Empty;
			}
			CompSendSignalOnCountdown compSendSignalOnCountdown = this.activatorCountdownsResult[0].TryGetComp<CompSendSignalOnCountdown>();
			return "ActivatorCountdown".Translate(compSendSignalOnCountdown.ticksLeft.ToStringTicksToPeriodVerbose(true, true));
		}

		// Token: 0x060114D1 RID: 70865 RVA: 0x004F5C08 File Offset: 0x004F3E08
		public override TaggedString GetExplanation()
		{
			int count = this.activatorCountdownsResult.Count;
			if (count > 1)
			{
				return "ActivatorCountdownDescMultiple".Translate(count);
			}
			if (count == 0)
			{
				return string.Empty;
			}
			return "ActivatorCountdownDesc".Translate();
		}

		// Token: 0x0400B355 RID: 45909
		private List<Thing> activatorCountdownsResult = new List<Thing>();
	}
}
