using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E31 RID: 15921
	public class TimedDetectionRaids : WorldObjectComp
	{
		// Token: 0x17003650 RID: 13904
		// (get) Token: 0x06015BFE RID: 89086 RVA: 0x0068092C File Offset: 0x0067EB2C
		public bool NextRaidCountdownActiveAndVisible
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0 && this.ticksLeftTillNotifyPlayer == 0;
			}
		}

		// Token: 0x17003651 RID: 13905
		// (get) Token: 0x06015BFF RID: 89087 RVA: 0x00680942 File Offset: 0x0067EB42
		public string DetectionCountdownTimeLeftString
		{
			get
			{
				if (!this.NextRaidCountdownActiveAndVisible)
				{
					return "";
				}
				return TimedDetectionRaids.GetDetectionCountdownTimeLeftString(this.ticksLeftToSendRaid);
			}
		}

		// Token: 0x17003652 RID: 13906
		// (get) Token: 0x06015C00 RID: 89088 RVA: 0x00680960 File Offset: 0x0067EB60
		private Faction RaidFaction
		{
			get
			{
				if (this.parent.Faction != null && !this.parent.Faction.IsPlayer && !this.parent.Faction.def.raidsForbidden)
				{
					return this.parent.Faction;
				}
				return Faction.OfMechanoids;
			}
		}

		// Token: 0x17003653 RID: 13907
		// (get) Token: 0x06015C01 RID: 89089 RVA: 0x006809B4 File Offset: 0x0067EBB4
		public int TicksLeftToSendRaids
		{
			get
			{
				return this.ticksLeftToSendRaid;
			}
		}

		// Token: 0x17003654 RID: 13908
		// (get) Token: 0x06015C02 RID: 89090 RVA: 0x006809BC File Offset: 0x0067EBBC
		public int RaidsSentCount
		{
			get
			{
				return this.raidsSentCount;
			}
		}

		// Token: 0x17003655 RID: 13909
		// (get) Token: 0x06015C03 RID: 89091 RVA: 0x006809C4 File Offset: 0x0067EBC4
		public bool DetectionCountdownStarted
		{
			get
			{
				return this.ticksLeftToSendRaid >= 0;
			}
		}

		// Token: 0x06015C04 RID: 89092 RVA: 0x006809D2 File Offset: 0x0067EBD2
		public void StartDetectionCountdown(int ticks, int notifyTicks = -1)
		{
			this.ticksLeftToSendRaid = ticks;
			this.ticksLeftTillNotifyPlayer = ((notifyTicks == -1) ? Mathf.Min((int)(60000f * Rand.Range(0.8f, 1.2f)), ticks / 2) : notifyTicks);
		}

		// Token: 0x06015C05 RID: 89093 RVA: 0x00680A08 File Offset: 0x0067EC08
		public void ResetCountdown()
		{
			this.ticksLeftTillNotifyPlayer = (this.ticksLeftToSendRaid = -1);
		}

		// Token: 0x06015C06 RID: 89094 RVA: 0x00680A25 File Offset: 0x0067EC25
		public void SetNotifiedSilently()
		{
			this.ticksLeftTillNotifyPlayer = 0;
		}

		// Token: 0x06015C07 RID: 89095 RVA: 0x00680A30 File Offset: 0x0067EC30
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToSendRaid, "ticksLeftToForceExitAndRemoveMap", -1, false);
			Scribe_Values.Look<int>(ref this.ticksLeftTillNotifyPlayer, "ticksLeftTillNotifyPlayer", -1, false);
			Scribe_Values.Look<bool>(ref this.alertRaidsArrivingIn, "alertRaidsArrivingIn", false, false);
			Scribe_Values.Look<int>(ref this.raidsSentCount, "raidsSentCount", 0, false);
			Scribe_Values.Look<FloatRange>(ref this.delayRangeHours, "delayRangeHours", TimedDetectionRaids.DefaultDelayRangeHours, false);
		}

		// Token: 0x06015C08 RID: 89096 RVA: 0x00680AA4 File Offset: 0x0067ECA4
		public override string CompInspectStringExtra()
		{
			string text = null;
			if (this.NextRaidCountdownActiveAndVisible)
			{
				text += "CaravanDetectedRaidCountdown".Translate(this.DetectionCountdownTimeLeftString) + ".\n";
			}
			if (Prefs.DevMode)
			{
				if (this.ticksLeftToSendRaid != -1)
				{
					text = text + "[DEV]: Time left to send raid: " + this.ticksLeftToSendRaid.ToStringTicksToPeriod(true, false, true, true, false) + "\n";
				}
				if (this.ticksLeftTillNotifyPlayer != -1)
				{
					text = text + "[DEV]: Time left till notify player about incoming raid: " + this.ticksLeftTillNotifyPlayer.ToStringTicksToPeriod(true, false, true, true, false) + "\n";
				}
			}
			if (text != null)
			{
				text = text.TrimEndNewlines();
			}
			return text;
		}

		// Token: 0x06015C09 RID: 89097 RVA: 0x00680B4C File Offset: 0x0067ED4C
		public override void CompTickInterval(int delta)
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (this.ticksLeftTillNotifyPlayer > 0)
				{
					this.ticksLeftTillNotifyPlayer -= delta;
					if (this.ticksLeftTillNotifyPlayer <= 0)
					{
						this.NotifyPlayer();
					}
				}
				if (this.ticksLeftToSendRaid > 0)
				{
					this.ticksLeftToSendRaid -= delta;
					if (this.ticksLeftToSendRaid <= 0)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = mapParent.Map;
						incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 2.5f;
						incidentParms.faction = this.RaidFaction;
						if (IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms))
						{
							Messages.Message("MessageCaravanDetectedRaidArrived".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction, this.ticksLeftToSendRaid.ToStringTicksToDays("F1")), MessageTypeDefOf.ThreatBig, true);
							this.raidsSentCount++;
						}
						this.ticksLeftToSendRaid = (int)(this.delayRangeHours.RandomInRange * 2500f);
						return;
					}
				}
			}
			else
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x06015C0A RID: 89098 RVA: 0x00680C80 File Offset: 0x0067EE80
		private void NotifyPlayer()
		{
			Find.LetterStack.ReceiveLetter("LetterLabelSiteCountdownStarted".Translate(), "LetterTextSiteCountdownStarted".Translate(this.ticksLeftToSendRaid.ToStringTicksToDays("F1"), this.RaidFaction.def.pawnsPlural, this.RaidFaction), LetterDefOf.ThreatBig, this.parent, null, null, null, null, 0, true);
			this.alertRaidsArrivingIn = true;
		}

		// Token: 0x06015C0B RID: 89099 RVA: 0x00680CFD File Offset: 0x0067EEFD
		public static string GetDetectionCountdownTimeLeftString(int ticksLeft)
		{
			if (ticksLeft < 0)
			{
				return "";
			}
			return ticksLeft.ToStringTicksToPeriod(true, false, true, true, false);
		}

		// Token: 0x06015C0C RID: 89100 RVA: 0x00680D14 File Offset: 0x0067EF14
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set raid timer to 1 hour",
					action = delegate()
					{
						this.ticksLeftToSendRaid = 2500;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Disable raid timer",
					action = delegate()
					{
						this.ticksLeftToSendRaid = -1;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Set notify raid timer to 1 hour",
					action = delegate()
					{
						this.ticksLeftTillNotifyPlayer = 2500;
					}
				};
			}
			yield break;
		}

		// Token: 0x06015C0D RID: 89101 RVA: 0x00680D24 File Offset: 0x0067EF24
		public void CopyFrom(TimedDetectionRaids other)
		{
			this.ticksLeftToSendRaid = other.ticksLeftToSendRaid;
			this.ticksLeftTillNotifyPlayer = other.ticksLeftTillNotifyPlayer;
			this.delayRangeHours = other.delayRangeHours;
		}

		// Token: 0x0400F152 RID: 61778
		public bool alertRaidsArrivingIn;

		// Token: 0x0400F153 RID: 61779
		public const float RaidThreatPointsMultiplier = 2.5f;

		// Token: 0x0400F154 RID: 61780
		private static readonly FloatRange DefaultDelayRangeHours = new FloatRange(18f, 24f);

		// Token: 0x0400F155 RID: 61781
		private int ticksLeftToSendRaid = -1;

		// Token: 0x0400F156 RID: 61782
		private int ticksLeftTillNotifyPlayer = -1;

		// Token: 0x0400F157 RID: 61783
		private int raidsSentCount;

		// Token: 0x0400F158 RID: 61784
		public FloatRange delayRangeHours = TimedDetectionRaids.DefaultDelayRangeHours;
	}
}
