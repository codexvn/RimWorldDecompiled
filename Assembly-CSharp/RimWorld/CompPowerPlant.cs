using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002136 RID: 8502
	public class CompPowerPlant : CompPowerTrader
	{
		// Token: 0x17001C33 RID: 7219
		// (get) Token: 0x0600B993 RID: 47507 RVA: 0x00358BAB File Offset: 0x00356DAB
		protected virtual float DesiredPowerOutput
		{
			get
			{
				return -base.Props.PowerConsumption;
			}
		}

		// Token: 0x0600B994 RID: 47508 RVA: 0x00358BBC File Offset: 0x00356DBC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
			this.autoPoweredComp = this.parent.GetComp<CompAutoPowered>();
			this.toxifier = this.parent.GetComp<CompToxifier>();
			if (base.Props.PowerConsumption < 0f && !this.parent.IsBrokenDown() && FlickUtility.WantsToBeOn(this.parent))
			{
				base.PowerOn = true;
			}
		}

		// Token: 0x0600B995 RID: 47509 RVA: 0x00358C47 File Offset: 0x00356E47
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.sustainerProducingPower != null && !this.sustainerProducingPower.Ended)
			{
				this.sustainerProducingPower.End();
			}
		}

		// Token: 0x0600B996 RID: 47510 RVA: 0x00358C74 File Offset: 0x00356E74
		public override void CompTick()
		{
			base.CompTick();
			this.UpdateDesiredPowerOutput();
			if (base.Props.soundAmbientProducingPower != null)
			{
				if (base.PowerOutput > 0.01f)
				{
					if (this.sustainerProducingPower == null || this.sustainerProducingPower.Ended)
					{
						this.sustainerProducingPower = base.Props.soundAmbientProducingPower.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
					}
					this.sustainerProducingPower.Maintain();
					return;
				}
				if (this.sustainerProducingPower != null)
				{
					this.sustainerProducingPower.End();
					this.sustainerProducingPower = null;
				}
			}
		}

		// Token: 0x0600B997 RID: 47511 RVA: 0x00358D0C File Offset: 0x00356F0C
		public virtual void UpdateDesiredPowerOutput()
		{
			if ((this.breakdownableComp != null && this.breakdownableComp.BrokenDown) || (this.refuelableComp != null && !this.refuelableComp.HasFuel) || (this.flickableComp != null && !this.flickableComp.SwitchIsOn) || (this.autoPoweredComp != null && !this.autoPoweredComp.WantsToBeOn) || (this.toxifier != null && !this.toxifier.CanPolluteNow) || !base.PowerOn)
			{
				base.PowerOutput = 0f;
				return;
			}
			base.PowerOutput = this.DesiredPowerOutput;
		}

		// Token: 0x04007FEC RID: 32748
		protected CompRefuelable refuelableComp;

		// Token: 0x04007FED RID: 32749
		protected CompBreakdownable breakdownableComp;

		// Token: 0x04007FEE RID: 32750
		protected CompAutoPowered autoPoweredComp;

		// Token: 0x04007FEF RID: 32751
		protected CompToxifier toxifier;

		// Token: 0x04007FF0 RID: 32752
		private Sustainer sustainerProducingPower;
	}
}
