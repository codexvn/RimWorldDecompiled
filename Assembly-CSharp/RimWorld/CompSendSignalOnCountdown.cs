using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DFB RID: 11771
	public class CompSendSignalOnCountdown : ThingComp, IThingGlower
	{
		// Token: 0x17002A57 RID: 10839
		// (get) Token: 0x06010906 RID: 67846 RVA: 0x004C660F File Offset: 0x004C480F
		private CompProperties_SendSignalOnCountdown Props
		{
			get
			{
				return (CompProperties_SendSignalOnCountdown)this.props;
			}
		}

		// Token: 0x06010907 RID: 67847 RVA: 0x004C661C File Offset: 0x004C481C
		public bool ShouldBeLitNow()
		{
			return this.ticksLeft > 0;
		}

		// Token: 0x06010908 RID: 67848 RVA: 0x004C6627 File Offset: 0x004C4827
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksLeft = Mathf.CeilToInt(Rand.ByCurve(this.Props.countdownCurveTicks));
		}

		// Token: 0x06010909 RID: 67849 RVA: 0x004C665C File Offset: 0x004C485C
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode || !DebugSettings.godMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = "DEV: Activate",
				action = delegate()
				{
					Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
					SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
					this.ticksLeft = 0;
				}
			};
			yield break;
		}

		// Token: 0x0601090A RID: 67850 RVA: 0x004C666C File Offset: 0x004C486C
		public override void CompTick()
		{
			if (this.parent.IsHashIntervalTick(250))
			{
				this.TickRareWorker();
			}
		}

		// Token: 0x0601090B RID: 67851 RVA: 0x004C6686 File Offset: 0x004C4886
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.TickRareWorker();
		}

		// Token: 0x0601090C RID: 67852 RVA: 0x004C6694 File Offset: 0x004C4894
		public void TickRareWorker()
		{
			if (this.ticksLeft <= 0 || !this.parent.Spawned)
			{
				return;
			}
			this.ticksLeft -= 250;
			if (this.ticksLeft <= 0)
			{
				Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
				SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			}
		}

		// Token: 0x0601090D RID: 67853 RVA: 0x004C6724 File Offset: 0x004C4924
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return null;
			}
			if (this.ticksLeft <= 0)
			{
				return "expired".Translate().CapitalizeFirst();
			}
			return "SendSignalOnCountdownCompTime".Translate(this.ticksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
		}

		// Token: 0x0601090E RID: 67854 RVA: 0x004C6788 File Offset: 0x004C4988
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.ticksLeft = 0;
			}
		}

		// Token: 0x0601090F RID: 67855 RVA: 0x004C67FF File Offset: 0x004C49FF
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x0400ACA7 RID: 44199
		public string signalTag;

		// Token: 0x0400ACA8 RID: 44200
		public int ticksLeft;

		// Token: 0x0400ACA9 RID: 44201
		private const float MaxDistActivationByOther = 40f;
	}
}
