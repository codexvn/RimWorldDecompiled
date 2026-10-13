using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002C8E RID: 11406
	public class CompFleckEmitter : ThingComp
	{
		// Token: 0x1700286A RID: 10346
		// (get) Token: 0x0600FFFC RID: 65532 RVA: 0x004A2513 File Offset: 0x004A0713
		private CompProperties_FleckEmitter Props
		{
			get
			{
				return (CompProperties_FleckEmitter)this.props;
			}
		}

		// Token: 0x0600FFFD RID: 65533 RVA: 0x004A2520 File Offset: 0x004A0720
		public override void CompTick()
		{
			CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
			if (comp != null && !comp.PowerOn)
			{
				return;
			}
			CompSendSignalOnCountdown comp2 = this.parent.GetComp<CompSendSignalOnCountdown>();
			if (comp2 != null && comp2.ticksLeft <= 0)
			{
				return;
			}
			CompInitiatable comp3 = this.parent.GetComp<CompInitiatable>();
			if (comp3 != null && !comp3.Initiated)
			{
				return;
			}
			if (this.Props.emissionInterval == -1)
			{
				return;
			}
			if (this.ticksSinceLastEmitted >= this.Props.emissionInterval)
			{
				this.Emit();
				this.ticksSinceLastEmitted = 0;
				return;
			}
			this.ticksSinceLastEmitted++;
		}

		// Token: 0x0600FFFE RID: 65534 RVA: 0x004A25B4 File Offset: 0x004A07B4
		protected void Emit()
		{
			FleckMaker.Static(this.parent.DrawPos + this.Props.offset, this.parent.MapHeld, this.Props.fleck, 1f);
			if (!this.Props.soundOnEmission.NullOrUndefined())
			{
				this.Props.soundOnEmission.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x0600FFFF RID: 65535 RVA: 0x004A2630 File Offset: 0x004A0830
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksSinceLastEmitted, ((this.Props.saveKeysPrefix != null) ? (this.Props.saveKeysPrefix + "_") : "") + "ticksSinceLastEmitted", 0, false);
		}

		// Token: 0x0400A765 RID: 42853
		public int ticksSinceLastEmitted;
	}
}
