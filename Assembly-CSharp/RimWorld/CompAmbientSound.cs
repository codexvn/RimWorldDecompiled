using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002BCB RID: 11211
	public class CompAmbientSound : ThingComp
	{
		// Token: 0x1700274E RID: 10062
		// (get) Token: 0x0600FAD1 RID: 64209 RVA: 0x0048E1CF File Offset: 0x0048C3CF
		private CompProperties_AmbientSound Props
		{
			get
			{
				return (CompProperties_AmbientSound)this.props;
			}
		}

		// Token: 0x0600FAD2 RID: 64210 RVA: 0x0048E1DC File Offset: 0x0048C3DC
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			LongEventHandler.ExecuteWhenFinished(new Action(this.StartSustanier));
		}

		// Token: 0x0600FAD3 RID: 64211 RVA: 0x0048E1EF File Offset: 0x0048C3EF
		public override void CompTick()
		{
			Sustainer sustainer = this.sustainer;
			if (sustainer == null)
			{
				return;
			}
			sustainer.Maintain();
		}

		// Token: 0x0600FAD4 RID: 64212 RVA: 0x0048E204 File Offset: 0x0048C404
		public override void ReceiveCompSignal(string signal)
		{
			if (this.Props.disableOnHacked && signal == "Hacked")
			{
				this.EndSustainer();
			}
			if (this.Props.disabledOnUnpowered && signal == "PowerTurnedOff")
			{
				this.EndSustainer();
			}
			if (this.Props.disabledOnUnpowered && signal == "PowerTurnedOn")
			{
				this.StartSustanier();
			}
			if (this.Props.disableOnInteracted && signal == "Interacted")
			{
				this.EndSustainer();
			}
		}

		// Token: 0x0600FAD5 RID: 64213 RVA: 0x0048E291 File Offset: 0x0048C491
		public void StartSustanier()
		{
			if (this.sustainer == null)
			{
				this.sustainer = this.Props.sound.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
			}
		}

		// Token: 0x0600FAD6 RID: 64214 RVA: 0x0048E2C2 File Offset: 0x0048C4C2
		public void EndSustainer()
		{
			Sustainer sustainer = this.sustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.sustainer = null;
		}

		// Token: 0x0600FAD7 RID: 64215 RVA: 0x0048E2DC File Offset: 0x0048C4DC
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			Sustainer sustainer = this.sustainer;
			if (sustainer != null)
			{
				sustainer.End();
			}
			this.sustainer = null;
		}

		// Token: 0x0400A4B4 RID: 42164
		private Sustainer sustainer;
	}
}
