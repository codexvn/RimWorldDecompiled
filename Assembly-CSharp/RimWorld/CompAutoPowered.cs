using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F40 RID: 12096
	public abstract class CompAutoPowered : ThingComp
	{
		// Token: 0x17002C16 RID: 11286
		// (get) Token: 0x060110F2 RID: 69874 RVA: 0x004E6D28 File Offset: 0x004E4F28
		public bool AppearsPowered
		{
			get
			{
				return this.compPowerCached != null && this.compPowerCached.PowerNet != null && this.compPowerCached.PowerNet.HasActivePowerSource && this.compPowerCached.PowerNet.CanPowerNow(this.compPowerCached) && !this.parent.MapHeld.gameConditionManager.ElectricityDisabled(this.parent.MapHeld);
			}
		}

		// Token: 0x17002C17 RID: 11287
		// (get) Token: 0x060110F3 RID: 69875 RVA: 0x004E6D9C File Offset: 0x004E4F9C
		public CompFlickable Flickable
		{
			get
			{
				CompFlickable result;
				if ((result = this.compFlickableCached) == null)
				{
					result = (this.compFlickableCached = this.parent.TryGetComp<CompFlickable>());
				}
				return result;
			}
		}

		// Token: 0x17002C18 RID: 11288
		// (get) Token: 0x060110F4 RID: 69876
		public abstract bool WantsToBeOn { get; }

		// Token: 0x060110F5 RID: 69877 RVA: 0x004E6DC8 File Offset: 0x004E4FC8
		protected void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayNeedsPower);
			if (!this.parent.IsBrokenDown() && this.Flickable.SwitchIsOn && !this.AppearsPowered && this.overlayNeedsPower == null)
			{
				this.overlayNeedsPower = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.NeedsPower));
			}
		}

		// Token: 0x0400B12D RID: 45357
		public const string AutoPoweredWantsOffSignal = "AutoPoweredWantsOff";

		// Token: 0x0400B12E RID: 45358
		protected CompPowerTrader compPowerCached;

		// Token: 0x0400B12F RID: 45359
		protected CompFlickable compFlickableCached;

		// Token: 0x0400B130 RID: 45360
		private OverlayHandle? overlayNeedsPower;
	}
}
