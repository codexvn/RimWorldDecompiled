using System;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002DFE RID: 11774
	[StaticConstructorOnStartup]
	public class CompSendSignalOnMotion : ThingComp, IThingGlower
	{
		// Token: 0x17002A5A RID: 10842
		// (get) Token: 0x0601091B RID: 67867 RVA: 0x004C698F File Offset: 0x004C4B8F
		public CompProperties_SendSignalOnMotion Props
		{
			get
			{
				return (CompProperties_SendSignalOnMotion)this.props;
			}
		}

		// Token: 0x17002A5B RID: 10843
		// (get) Token: 0x0601091C RID: 67868 RVA: 0x004C699C File Offset: 0x004C4B9C
		public bool Sent
		{
			get
			{
				return this.sent;
			}
		}

		// Token: 0x17002A5C RID: 10844
		// (get) Token: 0x0601091D RID: 67869 RVA: 0x004C69A4 File Offset: 0x004C4BA4
		public bool Enabled
		{
			get
			{
				return this.ticksUntilEnabled <= 0;
			}
		}

		// Token: 0x0601091E RID: 67870 RVA: 0x004C69B2 File Offset: 0x004C4BB2
		public bool ShouldBeLitNow()
		{
			return !this.Sent;
		}

		// Token: 0x0601091F RID: 67871 RVA: 0x004C69BD File Offset: 0x004C4BBD
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.signalTag = this.Props.signalTag;
			this.ticksUntilEnabled = this.Props.enableAfterTicks;
		}

		// Token: 0x06010920 RID: 67872 RVA: 0x004C69E8 File Offset: 0x004C4BE8
		public override void CompTick()
		{
			base.CompTick();
			if (this.sent || !this.parent.Spawned)
			{
				return;
			}
			if (this.Enabled && Find.TickManager.TicksGame % 250 == 0)
			{
				this.CompTickRare();
			}
			if (this.ticksUntilEnabled > 0)
			{
				this.ticksUntilEnabled--;
			}
		}

		// Token: 0x06010921 RID: 67873 RVA: 0x004C6A48 File Offset: 0x004C4C48
		public override void CompTickRare()
		{
			base.CompTickRare();
			Predicate<Thing> predicate = null;
			if (this.Props.onlyHumanlike)
			{
				predicate = delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					return pawn != null && pawn.RaceProps.Humanlike;
				};
			}
			Thing thing = null;
			if (this.Props.triggerOnPawnInRoom)
			{
				foreach (Thing thing2 in this.parent.GetRoom(RegionType.Set_All).ContainedAndAdjacentThings)
				{
					if (predicate(thing2))
					{
						thing = thing2;
						break;
					}
				}
			}
			if (thing == null && this.Props.radius > 0f)
			{
				thing = GenClosest.ClosestThingReachable(this.parent.Position, this.parent.Map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), PathEndMode.OnCell, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), this.Props.radius, predicate, null, 0, -1, false, RegionType.Set_Passable, false, false);
			}
			if (thing != null)
			{
				this.Trigger(thing);
			}
		}

		// Token: 0x06010922 RID: 67874 RVA: 0x004C6B54 File Offset: 0x004C4D54
		protected void Trigger(Thing initiator)
		{
			Effecter effecter = new Effecter(EffecterDefOf.ActivatorProximityTriggered);
			effecter.Trigger(this.parent, TargetInfo.Invalid, -1);
			effecter.Cleanup();
			Messages.Message("MessageActivatorProximityTriggered".Translate(initiator), this.parent, MessageTypeDefOf.ThreatBig, true);
			Find.SignalManager.SendSignal(new Signal(this.signalTag, this.parent.Named("SUBJECT")));
			SoundDefOf.MechanoidsWakeUp.PlayOneShot(new TargetInfo(this.parent.Position, this.parent.Map, false));
			this.sent = true;
		}

		// Token: 0x06010923 RID: 67875 RVA: 0x004C6C0A File Offset: 0x004C4E0A
		public void Expire()
		{
			this.sent = true;
		}

		// Token: 0x06010924 RID: 67876 RVA: 0x004C6C14 File Offset: 0x004C4E14
		public override void Notify_SignalReceived(Signal signal)
		{
			Thing thing;
			if (signal.tag == "CompCanBeDormant.WakeUp" && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing != this.parent && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(thing.Position) <= 40f)
			{
				this.sent = true;
			}
			LocalTargetInfo localTargetInfo;
			if (!this.sent && signal.tag == CompAbilityEffect_Teleport.SkipUsedSignalTag && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && signal.args.TryGetArg<LocalTargetInfo>("POSITION", out localTargetInfo) && thing != null && thing.Map == this.parent.Map && this.parent.Position.DistanceTo(localTargetInfo.Cell) <= 40f)
			{
				this.Trigger(thing);
			}
		}

		// Token: 0x06010925 RID: 67877 RVA: 0x004C6D0C File Offset: 0x004C4F0C
		public override string CompInspectStringExtra()
		{
			if (!this.Enabled)
			{
				return "SendSignalOnCountdownCompTime".Translate(this.ticksUntilEnabled.ToStringTicksToPeriod(true, false, true, false, false)).Resolve();
			}
			if (!this.sent)
			{
				return "radius".Translate().CapitalizeFirst() + ": " + this.Props.radius.ToString("F0");
			}
			return "expired".Translate().CapitalizeFirst();
		}

		// Token: 0x06010926 RID: 67878 RVA: 0x004C6DA4 File Offset: 0x004C4FA4
		public override void PostExposeData()
		{
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
			Scribe_Values.Look<bool>(ref this.sent, "sent", false, false);
			Scribe_Values.Look<int>(ref this.ticksUntilEnabled, "ticksUntilEnabled", 0, false);
		}

		// Token: 0x0400ACB4 RID: 44212
		public string signalTag;

		// Token: 0x0400ACB5 RID: 44213
		private bool sent;

		// Token: 0x0400ACB6 RID: 44214
		private int ticksUntilEnabled;

		// Token: 0x0400ACB7 RID: 44215
		private const float MaxDistActivationByOther = 40f;
	}
}
