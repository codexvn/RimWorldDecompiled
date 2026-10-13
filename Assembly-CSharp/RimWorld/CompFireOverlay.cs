using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C8A RID: 11402
	[StaticConstructorOnStartup]
	public class CompFireOverlay : CompFireOverlayBase
	{
		// Token: 0x17002866 RID: 10342
		// (get) Token: 0x0600FFE8 RID: 65512 RVA: 0x004A21E1 File Offset: 0x004A03E1
		public new CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x0600FFE9 RID: 65513 RVA: 0x004A2270 File Offset: 0x004A0470
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.03658537f;
			CompFireOverlay.FireGraphic.Draw(drawPos, this.parent.Rotation, this.parent, 0f);
		}

		// Token: 0x0600FFEA RID: 65514 RVA: 0x004A22D6 File Offset: 0x004A04D6
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x0600FFEB RID: 65515 RVA: 0x004A22F0 File Offset: 0x004A04F0
		public override void CompTick()
		{
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			if (this.startedGrowingAtTick < 0)
			{
				this.startedGrowingAtTick = GenTicks.TicksAbs;
			}
		}

		// Token: 0x0400A759 RID: 42841
		protected CompRefuelable refuelableComp;

		// Token: 0x0400A75A RID: 42842
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
