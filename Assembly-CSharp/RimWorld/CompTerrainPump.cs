using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E85 RID: 11909
	public abstract class CompTerrainPump : ThingComp
	{
		// Token: 0x17002B0E RID: 11022
		// (get) Token: 0x06010C4A RID: 68682 RVA: 0x004D4B07 File Offset: 0x004D2D07
		private CompProperties_TerrainPump Props
		{
			get
			{
				return (CompProperties_TerrainPump)this.props;
			}
		}

		// Token: 0x17002B0F RID: 11023
		// (get) Token: 0x06010C4B RID: 68683 RVA: 0x004D4B14 File Offset: 0x004D2D14
		private float ProgressDays
		{
			get
			{
				return (float)this.progressTicks / 60000f;
			}
		}

		// Token: 0x17002B10 RID: 11024
		// (get) Token: 0x06010C4C RID: 68684 RVA: 0x004D4B23 File Offset: 0x004D2D23
		protected float CurrentRadius
		{
			get
			{
				return Mathf.Min(this.Props.radius, this.ProgressDays / this.Props.daysToRadius * this.Props.radius);
			}
		}

		// Token: 0x17002B11 RID: 11025
		// (get) Token: 0x06010C4D RID: 68685 RVA: 0x004D4B53 File Offset: 0x004D2D53
		protected bool Working
		{
			get
			{
				return this.powerComp == null || this.powerComp.PowerOn;
			}
		}

		// Token: 0x17002B12 RID: 11026
		// (get) Token: 0x06010C4E RID: 68686 RVA: 0x004D4B6C File Offset: 0x004D2D6C
		private int TicksUntilRadiusInteger
		{
			get
			{
				float num = Mathf.Ceil(this.CurrentRadius) - this.CurrentRadius;
				if (num < 1E-05f)
				{
					num = 1f;
				}
				float num2 = this.Props.radius / this.Props.daysToRadius;
				return (int)(num / num2 * 60000f);
			}
		}

		// Token: 0x06010C4F RID: 68687 RVA: 0x004D4BBC File Offset: 0x004D2DBC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06010C50 RID: 68688 RVA: 0x004D4BCF File Offset: 0x004D2DCF
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			this.progressTicks = 0;
		}

		// Token: 0x06010C51 RID: 68689 RVA: 0x004D4BD8 File Offset: 0x004D2DD8
		public override void CompTickRare()
		{
			if (this.Working)
			{
				this.progressTicks += 250;
				int num = GenRadial.NumCellsInRadius(this.CurrentRadius);
				for (int i = 0; i < num; i++)
				{
					this.AffectCell(this.parent.Position + GenRadial.RadialPattern[i]);
				}
			}
		}

		// Token: 0x06010C52 RID: 68690
		protected abstract void AffectCell(IntVec3 c);

		// Token: 0x06010C53 RID: 68691 RVA: 0x004D4C38 File Offset: 0x004D2E38
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.progressTicks, "progressTicks", 0, false);
		}

		// Token: 0x06010C54 RID: 68692 RVA: 0x004D4C4C File Offset: 0x004D2E4C
		public override void PostDrawExtraSelectionOverlays()
		{
			if (this.CurrentRadius < this.Props.radius - 0.0001f)
			{
				GenDraw.DrawRadiusRing(this.parent.Position, this.CurrentRadius);
			}
		}

		// Token: 0x06010C55 RID: 68693 RVA: 0x004D4C80 File Offset: 0x004D2E80
		public override string CompInspectStringExtra()
		{
			string text = "TimePassed".Translate().CapitalizeFirst() + ": " + this.progressTicks.ToStringTicksToPeriod(true, false, true, true, false) + "\n" + "CurrentRadius".Translate().CapitalizeFirst() + ": " + this.CurrentRadius.ToString("F1");
			if (this.ProgressDays < this.Props.daysToRadius && this.Working)
			{
				text += "\n" + "RadiusExpandsIn".Translate().CapitalizeFirst() + ": " + this.TicksUntilRadiusInteger.ToStringTicksToPeriod(true, false, true, true, false);
			}
			return text;
		}

		// Token: 0x06010C56 RID: 68694 RVA: 0x004D4D6A File Offset: 0x004D2F6A
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Progress 1 day",
					action = delegate()
					{
						this.progressTicks += 60000;
					}
				};
			}
			yield break;
		}

		// Token: 0x0400AEAA RID: 44714
		private CompPowerTrader powerComp;

		// Token: 0x0400AEAB RID: 44715
		private int progressTicks;
	}
}
