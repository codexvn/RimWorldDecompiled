using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DB0 RID: 11696
	public class CompPolluteOverTime : ThingComp
	{
		// Token: 0x170029EB RID: 10731
		// (get) Token: 0x06010716 RID: 67350 RVA: 0x004BE5FC File Offset: 0x004BC7FC
		private CompProperties_PolluteOverTime Props
		{
			get
			{
				return (CompProperties_PolluteOverTime)this.props;
			}
		}

		// Token: 0x170029EC RID: 10732
		// (get) Token: 0x06010717 RID: 67351 RVA: 0x004BE609 File Offset: 0x004BC809
		private int TicksToPolluteCell
		{
			get
			{
				return 60000 / this.Props.cellsToPollutePerDay;
			}
		}

		// Token: 0x06010718 RID: 67352 RVA: 0x004BE61C File Offset: 0x004BC81C
		public override void CompTick()
		{
			if (this.parent.Spawned && this.parent.IsHashIntervalTick(this.TicksToPolluteCell))
			{
				this.Pollute();
			}
		}

		// Token: 0x06010719 RID: 67353 RVA: 0x004BE644 File Offset: 0x004BC844
		private void Pollute()
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			int num = GenRadial.NumCellsInRadius(GenRadial.MaxRadialPatternRadius - 1f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (!intVec.IsPolluted(this.parent.Map) && intVec.CanPollute(this.parent.Map))
				{
					intVec.Pollute(this.parent.Map, false);
					this.parent.Map.effecterMaintainer.AddEffecterToMaintain(EffecterDefOf.CellPollution.Spawn(intVec, this.parent.Map, Vector3.zero, 1f), intVec, 45);
					return;
				}
			}
		}

		// Token: 0x0601071A RID: 67354 RVA: 0x004BE709 File Offset: 0x004BC909
		public override string CompInspectStringExtra()
		{
			return "TilePollution".Translate() + ": " + "CellsPerDay".Translate(this.Props.cellsToPollutePerDay);
		}

		// Token: 0x0601071B RID: 67355 RVA: 0x004BE743 File Offset: 0x004BC943
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Pollute",
					action = new Action(this.Pollute)
				};
			}
			yield break;
		}
	}
}
