using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002F4A RID: 12106
	public class CompLoudspeaker : CompAutoPowered, IThingGlower
	{
		// Token: 0x17002C24 RID: 11300
		// (get) Token: 0x06011125 RID: 69925 RVA: 0x004E76E3 File Offset: 0x004E58E3
		private CompProperties_Loudspeaker Props
		{
			get
			{
				return (CompProperties_Loudspeaker)this.props;
			}
		}

		// Token: 0x17002C25 RID: 11301
		// (get) Token: 0x06011126 RID: 69926 RVA: 0x004E76F0 File Offset: 0x004E58F0
		public bool Active
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x17002C26 RID: 11302
		// (get) Token: 0x06011127 RID: 69927 RVA: 0x004E76F0 File Offset: 0x004E58F0
		public override bool WantsToBeOn
		{
			get
			{
				return this.inRitual;
			}
		}

		// Token: 0x06011128 RID: 69928 RVA: 0x004E76F8 File Offset: 0x004E58F8
		public bool ShouldBeLitNow()
		{
			return this.Active;
		}

		// Token: 0x06011129 RID: 69929 RVA: 0x004E7700 File Offset: 0x004E5900
		public override void CompTick()
		{
			base.CompTick();
			if (!ModLister.CheckIdeology("Speaker"))
			{
				return;
			}
			if (this.compPowerCached == null)
			{
				this.compPowerCached = this.parent.GetComp<CompPowerTrader>();
			}
			if (this.parent.IsHashIntervalTick(30) || !this.initialized)
			{
				this.initialized = true;
				this.UpdateInRitual();
				base.UpdateOverlays();
			}
			if (this.compPowerCached.PowerOn && this.inRitual)
			{
				if (this.parent.Rotation == Rot4.North && (this.lightsMote == null || this.lightsMote.Destroyed))
				{
					this.lightsMote = MoteMaker.MakeStaticMote(this.parent.TrueCenter(), this.parent.Map, ThingDefOf.Mote_LoudspeakerLights, 1f, false, 0f);
				}
				Mote mote = this.lightsMote;
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			else
			{
				Mote mote2 = this.lightsMote;
				if (mote2 != null)
				{
					mote2.Destroy(DestroyMode.Vanish);
				}
				this.lightsMote = null;
			}
			if (this.compPowerCached.PowerOn && !this.inRitual)
			{
				this.parent.BroadcastCompSignal("AutoPoweredWantsOff");
			}
		}

		// Token: 0x0601112A RID: 69930 RVA: 0x004E7826 File Offset: 0x004E5A26
		public override void Notify_SignalReceived(Signal signal)
		{
			if (signal.tag == "RitualStarted")
			{
				this.UpdateInRitual();
			}
		}

		// Token: 0x0601112B RID: 69931 RVA: 0x004E7840 File Offset: 0x004E5A40
		private void UpdateInRitual()
		{
			this.inRitual = false;
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (Lord lord in this.parent.Map.lordManager.lords)
			{
				LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null && lordJob_Ritual.selectedTarget.Thing != null && lordJob_Ritual.selectedTarget.Thing.def == ThingDefOf.LightBall && this.parent.GetRoom(RegionType.Set_All) == lordJob_Ritual.selectedTarget.Thing.GetRoom(RegionType.Set_All))
				{
					this.inRitual = true;
					break;
				}
			}
		}

		// Token: 0x0601112C RID: 69932 RVA: 0x004E749B File Offset: 0x004E569B
		public override void PostDrawExtraSelectionOverlays()
		{
			PlaceWorker_ShowSpeakerConnections.DrawConnections(this.parent.def, this.parent.Position, this.parent.Map);
		}

		// Token: 0x0400B14C RID: 45388
		public const int Range = 12;

		// Token: 0x0400B14D RID: 45389
		private Mote lightsMote;

		// Token: 0x0400B14E RID: 45390
		private bool inRitual;

		// Token: 0x0400B14F RID: 45391
		private bool initialized;
	}
}
