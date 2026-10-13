using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C8B RID: 11403
	[StaticConstructorOnStartup]
	public class CompRitualFireOverlay : CompFireOverlayBase
	{
		// Token: 0x17002867 RID: 10343
		// (get) Token: 0x0600FFEE RID: 65518 RVA: 0x004A233C File Offset: 0x004A053C
		public new CompProperties_FireOverlayRitual Props
		{
			get
			{
				return (CompProperties_FireOverlayRitual)this.props;
			}
		}

		// Token: 0x0600FFEF RID: 65519 RVA: 0x004A234C File Offset: 0x004A054C
		public override void PostDraw()
		{
			base.PostDraw();
			LordJob_Ritual lordJob_Ritual = this.parent.TargetOfRitual();
			if (lordJob_Ritual == null)
			{
				return;
			}
			if (lordJob_Ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			Vector3 loc = this.parent.TrueCenter();
			loc.y = AltitudeLayer.BuildingOnTop.AltitudeFor();
			CompFireOverlay.FireGraphic.Draw(loc, Rot4.North, this.parent, 0f);
		}

		// Token: 0x0600FFF0 RID: 65520 RVA: 0x004A23B8 File Offset: 0x004A05B8
		public override void CompTick()
		{
			LordJob_Ritual lordJob_Ritual = this.parent.TargetOfRitual();
			if (lordJob_Ritual == null)
			{
				return;
			}
			if (lordJob_Ritual.Progress < this.Props.minRitualProgress)
			{
				return;
			}
			if (this.startedGrowingAtTick < 0)
			{
				this.startedGrowingAtTick = GenTicks.TicksAbs;
			}
			if (GenTicks.TicksAbs % 30 == 0)
			{
				FleckMaker.ThrowFireGlow(this.parent.TrueCenter(), this.parent.Map, 1f);
			}
		}

		// Token: 0x0400A75B RID: 42843
		public const int FireGlowIntervalTicks = 30;
	}
}
