using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class QuestPart_RandomRaid : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_InvolvedFactions_003Ed__17 : IEnumerable<Faction>, IEnumerable, IEnumerator<Faction>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Faction _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RandomRaid _003C_003E4__this;

		private IEnumerator<Faction> _003C_003E7__wrap1;

		Faction IEnumerator<Faction>.Current
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
		public _003Cget_InvolvedFactions_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_RandomRaid questPart_RandomRaid = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_RandomRaid).InvolvedFactions.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Faction current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_RandomRaid.faction != null)
					{
						_003C_003E2__current = questPart_RandomRaid.faction;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Faction> IEnumerable<Faction>.GetEnumerator()
		{
			_003Cget_InvolvedFactions_003Ed__17 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_InvolvedFactions_003Ed__17(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Faction>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__15 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_RandomRaid _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		GlobalTargetInfo IEnumerator<GlobalTargetInfo>.Current
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
		public _003Cget_QuestLookTargets_003Ed__15(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				QuestPart_RandomRaid questPart_RandomRaid = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_RandomRaid).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_006f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_006f;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_006f:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_RandomRaid.mapParent != null)
					{
						_003C_003E2__current = questPart_RandomRaid.mapParent;
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GlobalTargetInfo>)this).GetEnumerator();
		}
	}

	public string inSignal;

	public MapParent mapParent;

	public FloatRange pointsRange;

	public Faction faction;

	public bool useCurrentThreatPoints;

	public float currentThreatPointsFactor = 1f;

	public PawnsArrivalModeDef arrivalMode;

	public RaidStrategyDef raidStrategy;

	public string customLetterLabel;

	public string customLetterText;

	public List<Thing> attackTargets;

	public bool generateFightersOnly;

	public bool fallbackToPlayerHomeMap;

	public bool sendLetter = true;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__15))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__15(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override IEnumerable<Faction> InvolvedFactions
	{
		[IteratorStateMachine(typeof(_003Cget_InvolvedFactions_003Ed__17))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_InvolvedFactions_003Ed__17(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (fallbackToPlayerHomeMap && (this.mapParent == null || !this.mapParent.HasMap || !quest.IsParentSuitableForQuest(this.mapParent)))
		{
			MapParent mapParent = quest.TryFindNewSuitableMapParentForRetarget() ?? Find.AnyPlayerHomeMap.Parent;
			if (mapParent != null)
			{
				this.mapParent = mapParent;
			}
		}
		if (signal.tag == inSignal && this.mapParent != null && this.mapParent.HasMap)
		{
			IncidentParms incidentParms = new IncidentParms();
			incidentParms.forced = true;
			incidentParms.quest = quest;
			incidentParms.target = this.mapParent.Map;
			incidentParms.points = (useCurrentThreatPoints ? (StorytellerUtility.DefaultThreatPointsNow(this.mapParent.Map) * currentThreatPointsFactor) : pointsRange.RandomInRange);
			incidentParms.faction = faction;
			incidentParms.customLetterLabel = signal.args.GetFormattedText(customLetterLabel);
			incidentParms.customLetterText = signal.args.GetFormattedText(customLetterText).Resolve();
			incidentParms.attackTargets = attackTargets;
			incidentParms.generateFightersOnly = generateFightersOnly;
			incidentParms.sendLetter = sendLetter;
			if (arrivalMode != null)
			{
				incidentParms.raidArrivalMode = arrivalMode;
			}
			IncidentDef incidentDef = ((faction != null && !faction.HostileTo(Faction.OfPlayer)) ? IncidentDefOf.RaidFriendly : IncidentDefOf.RaidEnemy);
			if (raidStrategy != null)
			{
				incidentParms.raidStrategy = raidStrategy;
			}
			if (faction != null)
			{
				incidentParms.points = Mathf.Max(incidentParms.points, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat));
			}
			if (incidentDef.Worker.CanFireNow(incidentParms))
			{
				incidentDef.Worker.TryExecute(incidentParms);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_Values.Look(ref pointsRange, "pointsRange");
		Scribe_References.Look(ref faction, "faction");
		Scribe_Values.Look(ref useCurrentThreatPoints, "useCurrentThreatPoints", defaultValue: false);
		Scribe_Values.Look(ref currentThreatPointsFactor, "currentThreatPointsFactor", 1f);
		Scribe_Defs.Look(ref arrivalMode, "arrivalMode");
		Scribe_Values.Look(ref customLetterLabel, "customLetterLabel");
		Scribe_Values.Look(ref customLetterText, "customLetterText");
		Scribe_Defs.Look(ref raidStrategy, "raidStrategy");
		Scribe_Collections.Look(ref attackTargets, "attackTargets", LookMode.Reference);
		Scribe_Values.Look(ref generateFightersOnly, "generateFightersOnly", defaultValue: false);
		Scribe_Values.Look(ref sendLetter, "sendLetter", defaultValue: true);
		Scribe_Values.Look(ref fallbackToPlayerHomeMap, "fallbackToPlayerHomeMap", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && attackTargets != null)
		{
			attackTargets.RemoveAll((Thing x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			mapParent = Find.RandomPlayerHomeMap.Parent;
			pointsRange = new FloatRange(500f, 1500f);
		}
	}
}
