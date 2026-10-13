using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A61 RID: 10849
	public class Building_WastepackAtomizer : Building
	{
		// Token: 0x170025A0 RID: 9632
		// (get) Token: 0x0600F12F RID: 61743 RVA: 0x0045F6C3 File Offset: 0x0045D8C3
		public CompAtomizer Atomizer
		{
			get
			{
				if (this.atomizer == null)
				{
					this.atomizer = base.GetComp<CompAtomizer>();
				}
				return this.atomizer;
			}
		}

		// Token: 0x170025A1 RID: 9633
		// (get) Token: 0x0600F130 RID: 61744 RVA: 0x0045F6DF File Offset: 0x0045D8DF
		private CompPowerTrader PowerTrader
		{
			get
			{
				if (this.powerTrader == null)
				{
					this.powerTrader = base.GetComp<CompPowerTrader>();
				}
				return this.powerTrader;
			}
		}

		// Token: 0x0600F131 RID: 61745 RVA: 0x0045F6FC File Offset: 0x0045D8FC
		protected override void Tick()
		{
			base.Tick();
			if (this.Atomizer.Empty || !this.PowerTrader.PowerOn)
			{
				Effecter effecter = this.operatingEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.operatingEffecter = null;
				return;
			}
			if (this.operatingEffecter == null)
			{
				this.operatingEffecter = this.def.building.wastepackAtomizerOperationEffecter.Spawn();
				this.operatingEffecter.Trigger(this, new TargetInfo(this.InteractionCell, base.Map, false), -1);
			}
			this.operatingEffecter.EffectTick(this, new TargetInfo(this.InteractionCell, base.Map, false));
		}

		// Token: 0x0600F132 RID: 61746 RVA: 0x0045F7B0 File Offset: 0x0045D9B0
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DrawAt(drawLoc, flip);
			Vector3 loc = drawLoc;
			loc.y -= 0.054878052f;
			this.def.building.wastepackAtomizerBottomGraphic.Graphic.Draw(loc, base.Rotation, this, 0f);
			Vector3 loc2 = drawLoc;
			loc2.y -= 0.018292684f;
			this.def.building.wastepackAtomizerWindowGraphic.Graphic.Draw(loc2, base.Rotation, this, 0f);
		}

		// Token: 0x04009F14 RID: 40724
		private CompAtomizer atomizer;

		// Token: 0x04009F15 RID: 40725
		private CompPowerTrader powerTrader;

		// Token: 0x04009F16 RID: 40726
		private Effecter operatingEffecter;

		// Token: 0x04009F17 RID: 40727
		private Graphic contentsGraphic;
	}
}
