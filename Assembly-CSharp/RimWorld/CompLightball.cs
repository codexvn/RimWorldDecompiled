using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F46 RID: 12102
	public class CompLightball : CompAutoPowered
	{
		// Token: 0x17002C1E RID: 11294
		// (get) Token: 0x0601110D RID: 69901 RVA: 0x004E717F File Offset: 0x004E537F
		private CompProperties_Lightball Props
		{
			get
			{
				return (CompProperties_Lightball)this.props;
			}
		}

		// Token: 0x17002C1F RID: 11295
		// (get) Token: 0x0601110E RID: 69902 RVA: 0x004E718C File Offset: 0x004E538C
		public bool Playing
		{
			get
			{
				return this.soundToPlay != null;
			}
		}

		// Token: 0x17002C20 RID: 11296
		// (get) Token: 0x0601110F RID: 69903 RVA: 0x004E7197 File Offset: 0x004E5397
		public SoundDef SoundToPlay
		{
			get
			{
				return this.soundToPlay;
			}
		}

		// Token: 0x17002C21 RID: 11297
		// (get) Token: 0x06011110 RID: 69904 RVA: 0x004E719F File Offset: 0x004E539F
		public override bool WantsToBeOn
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x06011111 RID: 69905 RVA: 0x004E71A8 File Offset: 0x004E53A8
		public override void CompTick()
		{
			base.CompTick();
			if (!ModLister.CheckIdeology("Lightball"))
			{
				return;
			}
			if (this.parent.Spawned && (this.parent.IsHashIntervalTick(20) || this.numActiveSpeakers == -1))
			{
				this.numActiveSpeakers = 0;
				foreach (Thing thing in this.parent.Map.listerBuldingOfDefInProximity.GetForCell(this.parent.Position, (float)this.Props.maxSpeakerDistance, ThingDefOf.Loudspeaker, null))
				{
					CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
					if (thing.GetRoom(RegionType.Set_All) == this.parent.GetRoom(RegionType.Set_All) && compPowerTrader.PowerOn)
					{
						this.numActiveSpeakers++;
					}
				}
				base.UpdateOverlays();
			}
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.GetComp<CompPowerTrader>();
			}
			if (this.parent.IsHashIntervalTick(30))
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
			if (this.compPowerCached.PowerOn && !this.inRitual)
			{
				this.parent.BroadcastCompSignal("AutoPoweredWantsOff");
			}
			if (!this.compPowerCached.PowerOn || !this.inRitual)
			{
				Mote mote = this.rotationMote;
				if (mote != null)
				{
					mote.Destroy(DestroyMode.Vanish);
				}
				this.rotationMote = null;
				Mote mote2 = this.lightsMote;
				if (mote2 != null)
				{
					mote2.Destroy(DestroyMode.Vanish);
				}
				this.lightsMote = null;
				return;
			}
			if (this.numActiveSpeakers > 0)
			{
				this.soundToPlay = ((!this.Props.soundDefsPerSpeakerCount.NullOrEmpty<SoundDef>()) ? this.Props.soundDefsPerSpeakerCount[Math.Min(this.numActiveSpeakers, this.Props.soundDefsPerSpeakerCount.Count) - 1] : null);
			}
			if (this.rotationMote == null || this.rotationMote.Destroyed)
			{
				this.rotationMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LightBall, 1f, false, 0f);
			}
			Mote mote3 = this.rotationMote;
			if (mote3 != null)
			{
				mote3.Maintain();
			}
			if (this.lightsMote == null || this.lightsMote.Destroyed)
			{
				this.lightsMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LightBallLights, 1f, false, 0f);
				if (this.lightsMote != null)
				{
					this.lightsMote.rotationRate = -3f;
				}
			}
			Mote mote4 = this.lightsMote;
			if (mote4 == null)
			{
				return;
			}
			mote4.Maintain();
		}

		// Token: 0x06011112 RID: 69906 RVA: 0x004E7460 File Offset: 0x004E5660
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == "RitualStarted")
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
		}

		// Token: 0x06011113 RID: 69907 RVA: 0x004E7485 File Offset: 0x004E5685
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				this.inRitual = this.parent.IsRitualTarget();
			}
		}

		// Token: 0x06011114 RID: 69908 RVA: 0x004E749B File Offset: 0x004E569B
		public override void PostDrawExtraSelectionOverlays()
		{
			PlaceWorker_ShowSpeakerConnections.DrawConnections(this.parent.def, this.parent.Position, this.parent.Map);
		}

		// Token: 0x0400B13E RID: 45374
		private Mote rotationMote;

		// Token: 0x0400B13F RID: 45375
		private Mote lightsMote;

		// Token: 0x0400B140 RID: 45376
		private bool inRitual;

		// Token: 0x0400B141 RID: 45377
		private int numActiveSpeakers = -1;

		// Token: 0x0400B142 RID: 45378
		private SoundDef soundToPlay;
	}
}
