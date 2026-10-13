using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DD8 RID: 11736
	public class CompRechargeable : ThingComp
	{
		// Token: 0x17002A21 RID: 10785
		// (get) Token: 0x0601080D RID: 67597 RVA: 0x004C2573 File Offset: 0x004C0773
		private CompProperties_Rechargeable Props
		{
			get
			{
				return (CompProperties_Rechargeable)this.props;
			}
		}

		// Token: 0x17002A22 RID: 10786
		// (get) Token: 0x0601080E RID: 67598 RVA: 0x004C2580 File Offset: 0x004C0780
		public bool Charged
		{
			get
			{
				return this.ticksUntilCharged == 0;
			}
		}

		// Token: 0x0601080F RID: 67599 RVA: 0x004C258B File Offset: 0x004C078B
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.ticksUntilCharged = this.Props.ticksToRecharge;
		}

		// Token: 0x06010810 RID: 67600 RVA: 0x004C25A5 File Offset: 0x004C07A5
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.WillReplace)
		{
			if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
		}

		// Token: 0x06010811 RID: 67601 RVA: 0x004C25C4 File Offset: 0x004C07C4
		public override void CompTick()
		{
			base.CompTick();
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.TryGetComp<CompPowerTrader>();
			}
			if (this.ticksUntilCharged > 0 && this.compPowerCached.PowerOn)
			{
				if (this.ticksUntilCharged == 1 && this.Props.chargedSoundDef != null)
				{
					this.Props.chargedSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
				}
				this.ticksUntilCharged--;
			}
			if (!this.Charged && this.parent.Spawned)
			{
				if (this.progressBar == null)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
				}
				this.progressBar.EffectTick(this.parent, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBar.children[0]).mote;
				if (mote != null)
				{
					mote.progress = 1f - Mathf.Clamp01((float)this.ticksUntilCharged / (float)this.Props.ticksToRecharge);
					mote.offsetZ = -0.5f;
					return;
				}
			}
			else if (this.progressBar != null)
			{
				this.progressBar.Cleanup();
				this.progressBar = null;
			}
		}

		// Token: 0x06010812 RID: 67602 RVA: 0x004C2714 File Offset: 0x004C0914
		public void Discharge()
		{
			this.ticksUntilCharged = this.Props.ticksToRecharge;
			if (this.Props.dischargeSoundDef != null)
			{
				this.Props.dischargeSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x06010813 RID: 67603 RVA: 0x004C2770 File Offset: 0x004C0970
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				if (!this.Charged)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Recharge",
						action = delegate()
						{
							if (this.Props.chargedSoundDef != null)
							{
								this.Props.chargedSoundDef.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
							}
							this.ticksUntilCharged = 0;
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Discharge",
						action = new Action(this.Discharge)
					};
				}
			}
			yield break;
		}

		// Token: 0x06010814 RID: 67604 RVA: 0x004C2780 File Offset: 0x004C0980
		public override string CompInspectStringExtra()
		{
			if (this.Charged)
			{
				return "RechargeableReady".Translate() + ".";
			}
			return "RechargeableCharging".Translate() + ": " + "DurationLeft".Translate(this.ticksUntilCharged.ToStringTicksToPeriod(true, true, true, true, false)) + ".";
		}

		// Token: 0x06010815 RID: 67605 RVA: 0x004C27F6 File Offset: 0x004C09F6
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksUntilCharged, "ticksUntilCharged", 0, false);
		}

		// Token: 0x0400AC1E RID: 44062
		private int ticksUntilCharged;

		// Token: 0x0400AC1F RID: 44063
		private Effecter progressBar;

		// Token: 0x0400AC20 RID: 44064
		private CompPowerTrader compPowerCached;
	}
}
