using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002972 RID: 10610
	[StaticConstructorOnStartup]
	public class Building_PowerSwitch : Building
	{
		// Token: 0x17002381 RID: 9089
		// (get) Token: 0x0600E8FF RID: 59647 RVA: 0x0043D7D9 File Offset: 0x0043B9D9
		public override bool TransmitsPowerNow
		{
			get
			{
				return FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x17002382 RID: 9090
		// (get) Token: 0x0600E900 RID: 59648 RVA: 0x0043D7E1 File Offset: 0x0043B9E1
		public override Graphic Graphic
		{
			get
			{
				return this.flickableComp.CurrentGraphic;
			}
		}

		// Token: 0x0600E901 RID: 59649 RVA: 0x0043D7EE File Offset: 0x0043B9EE
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.flickableComp = base.GetComp<CompFlickable>();
		}

		// Token: 0x0600E902 RID: 59650 RVA: 0x0043D804 File Offset: 0x0043BA04
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.flickableComp == null)
				{
					this.flickableComp = base.GetComp<CompFlickable>();
				}
				this.wantsOnOld = !FlickUtility.WantsToBeOn(this);
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600E903 RID: 59651 RVA: 0x0043D83D File Offset: 0x0043BA3D
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "FlickedOff" || signal == "FlickedOn" || signal == "ScheduledOn" || signal == "ScheduledOff")
			{
				this.UpdatePowerGrid();
			}
		}

		// Token: 0x0600E904 RID: 59652 RVA: 0x0043D87C File Offset: 0x0043BA7C
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.Append("PowerSwitch_Power".Translate() + ": ");
			if (FlickUtility.WantsToBeOn(this))
			{
				stringBuilder.Append("On".Translate().ToLower());
			}
			else
			{
				stringBuilder.Append("Off".Translate().ToLower());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600E905 RID: 59653 RVA: 0x0043D917 File Offset: 0x0043BB17
		private void UpdatePowerGrid()
		{
			if (FlickUtility.WantsToBeOn(this) != this.wantsOnOld)
			{
				if (base.Spawned)
				{
					base.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(base.PowerComp);
				}
				this.wantsOnOld = FlickUtility.WantsToBeOn(this);
			}
		}

		// Token: 0x04009B3C RID: 39740
		private bool wantsOnOld = true;

		// Token: 0x04009B3D RID: 39741
		private CompFlickable flickableComp;
	}
}
