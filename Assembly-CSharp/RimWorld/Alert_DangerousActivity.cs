using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FCC RID: 12236
	public class Alert_DangerousActivity : Alert_Critical
	{
		// Token: 0x17002CB7 RID: 11447
		// (get) Token: 0x060113E9 RID: 70633 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool DoMessage
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002CB8 RID: 11448
		// (get) Token: 0x060113EA RID: 70634 RVA: 0x004F0E4C File Offset: 0x004EF04C
		private List<Thing> HighActivity
		{
			get
			{
				this.highActivity.Clear();
				foreach (Map map in Find.Maps)
				{
					foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Suppressable))
					{
						ThingWithComps thingWithComps = (ThingWithComps)thing;
						if (Alert_DangerousActivity.DangerousActivity(thingWithComps))
						{
							this.highActivity.Add(thingWithComps);
						}
					}
					foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder))
					{
						Building_HoldingPlatform building_HoldingPlatform = thing2 as Building_HoldingPlatform;
						if (building_HoldingPlatform != null && building_HoldingPlatform.Occupied && Alert_DangerousActivity.DangerousActivity(building_HoldingPlatform.HeldPawn))
						{
							this.highActivity.Add(building_HoldingPlatform.HeldPawn);
						}
					}
				}
				return this.highActivity;
			}
		}

		// Token: 0x060113EB RID: 70635 RVA: 0x004F0F80 File Offset: 0x004EF180
		public Alert_DangerousActivity()
		{
			this.defaultLabel = "ActivityMultipleDangerous".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x060113EC RID: 70636 RVA: 0x004F0FB0 File Offset: 0x004EF1B0
		private static bool DangerousActivity(ThingWithComps thing)
		{
			CompActivity comp = thing.GetComp<CompActivity>();
			return comp != null && comp.ActivityLevel > comp.Props.warning && comp.State == ActivityState.Passive;
		}

		// Token: 0x060113ED RID: 70637 RVA: 0x004F0FE8 File Offset: 0x004EF1E8
		public override string GetLabel()
		{
			if (this.highActivity.Count == 1)
			{
				return this.highActivity[0].LabelNoParenthesisCap + ": " + this.highActivity[0].TryGetComp<CompActivity>().ActivityLevel.ToStringPercent("0");
			}
			return this.defaultLabel;
		}

		// Token: 0x060113EE RID: 70638 RVA: 0x004F1048 File Offset: 0x004EF248
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Thing thing in this.highActivity)
			{
				CompActivity compActivity = thing.TryGetComp<CompActivity>();
				stringBuilder.AppendLine("  - " + thing.LabelNoParenthesisCap + ": " + compActivity.ActivityLevel.ToStringPercent("0"));
			}
			return string.Format("{0}:\n{1}\n{2}", "ActivityDangerousDesc".Translate(), stringBuilder, "ActivityDangerousDescAppended".Translate());
		}

		// Token: 0x060113EF RID: 70639 RVA: 0x004F10FC File Offset: 0x004EF2FC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.HighActivity);
		}

		// Token: 0x0400B2F7 RID: 45815
		private readonly List<Thing> highActivity = new List<Thing>();
	}
}
