using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FCD RID: 12237
	public class Alert_UndercaveUnstable : Alert_Critical
	{
		// Token: 0x17002CB9 RID: 11449
		// (get) Token: 0x060113F0 RID: 70640 RVA: 0x004F110C File Offset: 0x004EF30C
		private PitGate PitGate
		{
			get
			{
				return this.targets[0].Thing as PitGate;
			}
		}

		// Token: 0x17002CBA RID: 11450
		// (get) Token: 0x060113F1 RID: 70641 RVA: 0x004F1132 File Offset: 0x004EF332
		protected override Color BGColor
		{
			get
			{
				PitGate pitGate = this.PitGate;
				if (pitGate != null && pitGate.CollapseStage == 1)
				{
					return Color.clear;
				}
				return base.BGColor;
			}
		}

		// Token: 0x060113F2 RID: 70642 RVA: 0x004F1158 File Offset: 0x004EF358
		public Alert_UndercaveUnstable()
		{
			this.defaultLabel = "Alert_UndercaveUnstable".Translate();
			this.defaultExplanation = "Alert_UndercaveUnstableDesc".Translate();
			this.requireAnomaly = true;
		}

		// Token: 0x060113F3 RID: 70643 RVA: 0x004F11A8 File Offset: 0x004EF3A8
		private void CalculateTargets()
		{
			this.targets.Clear();
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				foreach (Thing thing in maps[i].listerThings.ThingsOfDef(ThingDefOf.PitGate))
				{
					PitGate pitGate = (PitGate)thing;
					if (pitGate.IsCollapsing)
					{
						this.targets.Add(pitGate);
					}
				}
			}
		}

		// Token: 0x060113F4 RID: 70644 RVA: 0x004F1244 File Offset: 0x004EF444
		public override string GetLabel()
		{
			return this.defaultLabel + ": " + this.PitGate.TicksUntilCollapse.ToStringTicksToPeriodVerbose(true, true);
		}

		// Token: 0x060113F5 RID: 70645 RVA: 0x004F1268 File Offset: 0x004EF468
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B2F8 RID: 45816
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
