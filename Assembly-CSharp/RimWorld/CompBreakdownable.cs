using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002C2D RID: 11309
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x170027E3 RID: 10211
		// (get) Token: 0x0600FD91 RID: 64913 RVA: 0x00498EEB File Offset: 0x004970EB
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x0600FD92 RID: 64914 RVA: 0x00498EF3 File Offset: 0x004970F3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x0600FD93 RID: 64915 RVA: 0x00498F10 File Offset: 0x00497110
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayBrokenDown);
			if (this.brokenDownInt)
			{
				this.overlayBrokenDown = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BrokenDown));
			}
		}

		// Token: 0x0600FD94 RID: 64916 RVA: 0x00498F7C File Offset: 0x0049717C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
			this.UpdateOverlays();
		}

		// Token: 0x0600FD95 RID: 64917 RVA: 0x00498FB2 File Offset: 0x004971B2
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x0600FD96 RID: 64918 RVA: 0x00498FC8 File Offset: 0x004971C8
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13680000f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x0600FD97 RID: 64919 RVA: 0x00498FEE File Offset: 0x004971EE
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x0600FD98 RID: 64920 RVA: 0x00499010 File Offset: 0x00497210
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
			this.UpdateOverlays();
		}

		// Token: 0x0600FD99 RID: 64921 RVA: 0x00499072 File Offset: 0x00497272
		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			this.parent.BroadcastCompSignal("Breakdown");
			this.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(this.parent);
			this.UpdateOverlays();
		}

		// Token: 0x0600FD9A RID: 64922 RVA: 0x004990AC File Offset: 0x004972AC
		public override string CompInspectStringExtra()
		{
			if (this.BrokenDown)
			{
				return "BrokenDown".Translate();
			}
			return null;
		}

		// Token: 0x0600FD9B RID: 64923 RVA: 0x004990C7 File Offset: 0x004972C7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos && this.BrokenDown)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Fix breakdown",
					action = new Action(this.Notify_Repaired)
				};
			}
			yield break;
		}

		// Token: 0x0400A625 RID: 42533
		private bool brokenDownInt;

		// Token: 0x0400A626 RID: 42534
		private CompPowerTrader powerComp;

		// Token: 0x0400A627 RID: 42535
		private const int BreakdownMTBTicks = 13680000;

		// Token: 0x0400A628 RID: 42536
		public const string BreakdownSignal = "Breakdown";

		// Token: 0x0400A629 RID: 42537
		private OverlayHandle? overlayBrokenDown;
	}
}
