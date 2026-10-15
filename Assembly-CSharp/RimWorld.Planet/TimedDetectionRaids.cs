using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class TimedDetectionRaids : WorldObjectComp
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__27 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public TimedDetectionRaids _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetGizmos_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			TimedDetectionRaids CS_0024_003C_003E8__locals3 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Set raid timer to 1 hour",
						action = delegate
						{
							CS_0024_003C_003E8__locals3.ticksLeftToSendRaid = 2500;
						}
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Disable raid timer",
					action = delegate
					{
						CS_0024_003C_003E8__locals3.ticksLeftToSendRaid = -1;
					}
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set notify raid timer to 1 hour",
					action = delegate
					{
						CS_0024_003C_003E8__locals3.ticksLeftTillNotifyPlayer = 2500;
					}
				};
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public bool alertRaidsArrivingIn;

	public const float RaidThreatPointsMultiplier = 1.5f;

	private static readonly FloatRange DefaultDelayRangeHours = new FloatRange(18f, 24f);

	private int ticksLeftToSendRaid = -1;

	private int ticksLeftTillNotifyPlayer = -1;

	private int raidsSentCount;

	public FloatRange delayRangeHours = DefaultDelayRangeHours;

	public bool NextRaidCountdownActiveAndVisible
	{
		get
		{
			if (ticksLeftToSendRaid >= 0)
			{
				return ticksLeftTillNotifyPlayer == 0;
			}
			return false;
		}
	}

	public string DetectionCountdownTimeLeftString
	{
		get
		{
			if (!NextRaidCountdownActiveAndVisible)
			{
				return "";
			}
			return GetDetectionCountdownTimeLeftString(ticksLeftToSendRaid);
		}
	}

	private Faction RaidFaction
	{
		get
		{
			if (parent.Faction != null && !parent.Faction.IsPlayer && !parent.Faction.def.raidsForbidden)
			{
				return parent.Faction;
			}
			return Faction.OfMechanoids;
		}
	}

	public int TicksLeftToSendRaids => ticksLeftToSendRaid;

	public int RaidsSentCount => raidsSentCount;

	public bool DetectionCountdownStarted => ticksLeftToSendRaid >= 0;

	public void StartDetectionCountdown(int ticks, int notifyTicks = -1)
	{
		ticksLeftToSendRaid = ticks;
		ticksLeftTillNotifyPlayer = ((notifyTicks == -1) ? Mathf.Min((int)(60000f * Rand.Range(0.8f, 1.2f)), ticks / 2) : notifyTicks);
	}

	public void ResetCountdown()
	{
		ticksLeftTillNotifyPlayer = (ticksLeftToSendRaid = -1);
	}

	public void SetNotifiedSilently()
	{
		ticksLeftTillNotifyPlayer = 0;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref ticksLeftToSendRaid, "ticksLeftToForceExitAndRemoveMap", -1);
		Scribe_Values.Look(ref ticksLeftTillNotifyPlayer, "ticksLeftTillNotifyPlayer", -1);
		Scribe_Values.Look(ref alertRaidsArrivingIn, "alertRaidsArrivingIn", defaultValue: false);
		Scribe_Values.Look(ref raidsSentCount, "raidsSentCount", 0);
		Scribe_Values.Look(ref delayRangeHours, "delayRangeHours", DefaultDelayRangeHours);
	}

	public override string CompInspectStringExtra()
	{
		string text = null;
		if (NextRaidCountdownActiveAndVisible)
		{
			text += "CaravanDetectedRaidCountdown".Translate(DetectionCountdownTimeLeftString) + ".\n";
		}
		if (Prefs.DevMode)
		{
			if (ticksLeftToSendRaid != -1)
			{
				text = text + "[DEV]: Time left to send raid: " + ticksLeftToSendRaid.ToStringTicksToPeriod() + "\n";
			}
			if (ticksLeftTillNotifyPlayer != -1)
			{
				text = text + "[DEV]: Time left till notify player about incoming raid: " + ticksLeftTillNotifyPlayer.ToStringTicksToPeriod() + "\n";
			}
		}
		if (text != null)
		{
			text = text.TrimEndNewlines();
		}
		return text;
	}

	public override void CompTickInterval(int delta)
	{
		MapParent mapParent = (MapParent)parent;
		if (mapParent.HasMap)
		{
			if (ticksLeftTillNotifyPlayer > 0)
			{
				ticksLeftTillNotifyPlayer -= delta;
				if (ticksLeftTillNotifyPlayer <= 0)
				{
					NotifyPlayer();
				}
			}
			if (ticksLeftToSendRaid <= 0)
			{
				return;
			}
			ticksLeftToSendRaid -= delta;
			if (ticksLeftToSendRaid <= 0)
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = mapParent.Map;
				incidentParms.points = StorytellerUtility.DefaultThreatPointsNow(incidentParms.target) * 1.5f;
				incidentParms.faction = RaidFaction;
				ticksLeftToSendRaid = (int)(delayRangeHours.RandomInRange * 2500f);
				if (IncidentDefOf.RaidEnemy.Worker.TryExecute(incidentParms))
				{
					Messages.Message("MessageCaravanDetectedRaidArrived".Translate(incidentParms.faction.def.pawnsPlural, incidentParms.faction, ticksLeftToSendRaid.ToStringTicksToDays()), MessageTypeDefOf.ThreatBig);
					raidsSentCount++;
				}
			}
		}
		else
		{
			ResetCountdown();
		}
	}

	private void NotifyPlayer()
	{
		Find.LetterStack.ReceiveLetter("LetterLabelSiteCountdownStarted".Translate(), "LetterTextSiteCountdownStarted".Translate(ticksLeftToSendRaid.ToStringTicksToDays(), RaidFaction.def.pawnsPlural, RaidFaction), LetterDefOf.ThreatBig, parent);
		alertRaidsArrivingIn = true;
	}

	public static string GetDetectionCountdownTimeLeftString(int ticksLeft)
	{
		if (ticksLeft < 0)
		{
			return "";
		}
		return ticksLeft.ToStringTicksToPeriod();
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__27))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void CopyFrom(TimedDetectionRaids other)
	{
		ticksLeftToSendRaid = other.ticksLeftToSendRaid;
		ticksLeftTillNotifyPlayer = other.ticksLeftTillNotifyPlayer;
		delayRangeHours = other.delayRangeHours;
	}
}
