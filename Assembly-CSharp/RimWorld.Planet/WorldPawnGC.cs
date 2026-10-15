using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet;

public class WorldPawnGC : IExposable
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public Dictionary<Pawn, string> keptPawns;

		public Func<Pawn, bool> _003C_003E9__0;

		internal bool _003CAccumulatePawnGCData_003Eb__0(Pawn p)
		{
			if (p.RaceProps.Humanlike && !keptPawns.ContainsKey(p))
			{
				return Rand.ChanceSeeded(0.25f, p.thingIDNumber ^ 0x3A73ECFD);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAccumulatePawnGCData_003Ed__10 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Dictionary<Pawn, string> keptPawns;

		public Dictionary<Pawn, string> _003C_003E3__keptPawns;

		public WorldPawnGC _003C_003E4__this;

		private _003C_003Ec__DisplayClass10_0 _003C_003E8__1;

		private Pawn[] _003CcriticalPawns_003E5__2;

		private Pawn[] _003C_003E7__wrap2;

		private int _003C_003E7__wrap3;

		object IEnumerator<object>.Current
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
		public _003CAccumulatePawnGCData_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CcriticalPawns_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			WorldPawnGC worldPawnGC = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass10_0();
				_003C_003E8__1.keptPawns = keptPawns;
				foreach (Pawn item in Find.WorldPawns.AllPawnsAliveOrDead)
				{
					string criticalPawnReason = worldPawnGC.GetCriticalPawnReason(item);
					if (!criticalPawnReason.NullOrEmpty())
					{
						_003C_003E8__1.keptPawns[item] = criticalPawnReason;
						if (worldPawnGC.logDotgraph != null)
						{
							worldPawnGC.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", DotgraphIdentifier(item), criticalPawnReason, (item.relations != null && item.relations.everSeenByPlayer) ? "black" : "grey", item.RaceProps.Humanlike ? "oval" : "box"));
						}
					}
					else if (worldPawnGC.logDotgraph != null)
					{
						worldPawnGC.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", DotgraphIdentifier(item), (item.relations != null && item.relations.everSeenByPlayer) ? "black" : "grey", item.RaceProps.Humanlike ? "oval" : "box"));
					}
				}
				foreach (Pawn item2 in Find.WorldPawns.AllPawnsAlive.Where((Pawn p) => p.RaceProps.Humanlike && !_003C_003E8__1.keptPawns.ContainsKey(p) && Rand.ChanceSeeded(0.25f, p.thingIDNumber ^ 0x3A73ECFD)).Take(10))
				{
					_003C_003E8__1.keptPawns[item2] = "RandomlyKept";
				}
				_003CcriticalPawns_003E5__2 = _003C_003E8__1.keptPawns.Keys.ToArray();
				_003C_003E7__wrap2 = _003CcriticalPawns_003E5__2;
				_003C_003E7__wrap3 = 0;
				goto IL_0278;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E7__wrap3++;
				goto IL_0278;
			case 2:
				{
					_003C_003E1__state = -1;
					_003C_003E7__wrap3++;
					break;
				}
				IL_0278:
				if (_003C_003E7__wrap3 < _003C_003E7__wrap2.Length)
				{
					Pawn pawn = _003C_003E7__wrap2[_003C_003E7__wrap3];
					worldPawnGC.AddAllRelationships(pawn, _003C_003E8__1.keptPawns);
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E7__wrap2 = null;
				_003C_003E7__wrap2 = _003CcriticalPawns_003E5__2;
				_003C_003E7__wrap3 = 0;
				break;
			}
			if (_003C_003E7__wrap3 < _003C_003E7__wrap2.Length)
			{
				Pawn pawn2 = _003C_003E7__wrap2[_003C_003E7__wrap3];
				worldPawnGC.AddAllMemories(pawn2, _003C_003E8__1.keptPawns);
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			}
			_003C_003E7__wrap2 = null;
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
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CAccumulatePawnGCData_003Ed__10 _003CAccumulatePawnGCData_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAccumulatePawnGCData_003Ed__ = this;
			}
			else
			{
				_003CAccumulatePawnGCData_003Ed__ = new _003CAccumulatePawnGCData_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAccumulatePawnGCData_003Ed__.keptPawns = _003C_003E3__keptPawns;
			return _003CAccumulatePawnGCData_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPawnGCPass_003Ed__13 : IEnumerable<object>, IEnumerable, IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public WorldPawnGC _003C_003E4__this;

		private Dictionary<Pawn, string> _003CkeptPawns_003E5__2;

		private Pawn[] _003CworldPawnsSnapshot_003E5__3;

		private IEnumerator _003C_003E7__wrap3;

		object IEnumerator<object>.Current
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
		public _003CPawnGCPass_003Ed__13(int _003C_003E1__state)
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
			_003CkeptPawns_003E5__2 = null;
			_003CworldPawnsSnapshot_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				WorldPawnGC worldPawnGC = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CkeptPawns_003E5__2 = new Dictionary<Pawn, string>();
					_003CworldPawnsSnapshot_003E5__3 = Find.WorldPawns.AllPawnsAliveOrDead.ToArray();
					_003C_003E7__wrap3 = worldPawnGC.AccumulatePawnGCData(_003CkeptPawns_003E5__2).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap3.MoveNext())
				{
					_ = _003C_003E7__wrap3.Current;
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap3 = null;
				for (int i = 0; i < _003CworldPawnsSnapshot_003E5__3.Length; i++)
				{
					Pawn pawn = _003CworldPawnsSnapshot_003E5__3[i];
					if (pawn.IsWorldPawn() && !_003CkeptPawns_003E5__2.ContainsKey(pawn))
					{
						pawn.markedForDiscard = true;
					}
				}
				for (int j = 0; j < _003CworldPawnsSnapshot_003E5__3.Length; j++)
				{
					Pawn pawn2 = _003CworldPawnsSnapshot_003E5__3[j];
					if (pawn2.IsWorldPawn() && !_003CkeptPawns_003E5__2.ContainsKey(pawn2))
					{
						Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn2);
					}
				}
				Find.RelationshipRecords.CleanupUnusedRecords();
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
			if (_003C_003E7__wrap3 is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<object> IEnumerable<object>.GetEnumerator()
		{
			_003CPawnGCPass_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPawnGCPass_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<object>)this).GetEnumerator();
		}
	}

	private int lastSuccessfulGCTick;

	private int currentGCRate = 1;

	private const int AdditionalRandomHumanlikes = 10;

	private const float AdditionalRandomHumanlikeKeepChance = 0.25f;

	private const int GCUpdateInterval = 15000;

	private IEnumerator activeGCProcess;

	private StringBuilder logDotgraph;

	private HashSet<string> logDotgraphUniqueLinks;

	public void WorldPawnGCTick()
	{
		if (lastSuccessfulGCTick >= Find.TickManager.TicksGame / 15000 * 15000)
		{
			return;
		}
		if (activeGCProcess == null)
		{
			activeGCProcess = PawnGCPass().GetEnumerator();
			if (DebugViewSettings.logWorldPawnGC)
			{
				Log.Message($"World pawn GC started at rate {currentGCRate}");
			}
		}
		if (activeGCProcess == null)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < currentGCRate; i++)
		{
			if (flag)
			{
				break;
			}
			flag = !activeGCProcess.MoveNext();
		}
		if (flag)
		{
			lastSuccessfulGCTick = Find.TickManager.TicksGame;
			currentGCRate = 1;
			activeGCProcess = null;
			if (DebugViewSettings.logWorldPawnGC)
			{
				Log.Message("World pawn GC complete");
			}
		}
	}

	public void CancelGCPass()
	{
		if (activeGCProcess != null)
		{
			activeGCProcess = null;
			currentGCRate = Mathf.Min(currentGCRate * 2, 16777216);
			if (DebugViewSettings.logWorldPawnGC)
			{
				Log.Message("World pawn GC cancelled");
			}
		}
	}

	[IteratorStateMachine(typeof(_003CAccumulatePawnGCData_003Ed__10))]
	private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAccumulatePawnGCData_003Ed__10(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__keptPawns = keptPawns
		};
	}

	public Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
	{
		Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
		AccumulatePawnGCData(dictionary).ExecuteEnumerable();
		return dictionary;
	}

	public string PawnGCDebugResults()
	{
		Dictionary<Pawn, string> dictionary = AccumulatePawnGCDataImmediate();
		Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
		foreach (Pawn item in Find.WorldPawns.AllPawnsAliveOrDead)
		{
			string text = "Discarded";
			if (dictionary.ContainsKey(item))
			{
				text = dictionary[item];
			}
			if (!dictionary2.ContainsKey(text))
			{
				dictionary2[text] = 0;
			}
			string key = text;
			int value = dictionary2[key] + 1;
			dictionary2[key] = value;
		}
		return (from kvp in dictionary2
			orderby kvp.Value descending
			select $"{kvp.Value}: {kvp.Key}").ToLineList();
	}

	[IteratorStateMachine(typeof(_003CPawnGCPass_003Ed__13))]
	public IEnumerable PawnGCPass()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPawnGCPass_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private string GetCriticalPawnReason(Pawn pawn)
	{
		if (pawn.Discarded)
		{
			return null;
		}
		if (PawnUtility.EverBeenColonistOrTameAnimal(pawn) && pawn.RaceProps.Humanlike)
		{
			return "Colonist";
		}
		if (pawn.RaceProps.Animal && TrainableUtility.GetAllColonistBondsFor(pawn).Any())
		{
			return "BondedAnimal";
		}
		if (PawnGenerator.IsBeingGenerated(pawn))
		{
			return "Generating";
		}
		if (PawnUtility.IsFactionLeader(pawn))
		{
			return "FactionLeader";
		}
		if (PawnUtility.IsKidnappedPawn(pawn))
		{
			return "Kidnapped";
		}
		if (pawn.IsCaravanMember())
		{
			return "CaravanMember";
		}
		if (PawnUtility.IsTravelingInTransportPodWorldObject(pawn))
		{
			return "TransportPod";
		}
		if (PawnUtility.ForSaleBySettlement(pawn))
		{
			return "ForSale";
		}
		if (Find.WorldPawns.ForcefullyKeptPawns.Contains(pawn))
		{
			return "ForceKept";
		}
		if (pawn.SpawnedOrAnyParentSpawned)
		{
			return "Spawned";
		}
		if (!pawn.Corpse.DestroyedOrNull())
		{
			return "CorpseExists";
		}
		if (pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing)
		{
			if (Find.PlayLog.AnyEntryConcerns(pawn))
			{
				return "InPlayLog";
			}
			if (Find.BattleLog.AnyEntryConcerns(pawn))
			{
				return "InBattleLog";
			}
		}
		if (Current.ProgramState == ProgramState.Playing && Find.TaleManager.AnyActiveTaleConcerns(pawn))
		{
			return "InActiveTale";
		}
		if (QuestUtility.IsReservedByQuestOrQuestBeingGenerated(pawn))
		{
			return "ReservedByQuest";
		}
		if (Find.WorldPawns.PawnSourced(pawn))
		{
			return "CompPawnSource";
		}
		return null;
	}

	public void AddAllRelationships(Pawn pawn, Dictionary<Pawn, string> keptPawns)
	{
		if (pawn.relations == null || (!pawn.Spawned && (!pawn.RaceProps.Humanlike || !pawn.Faction.IsPlayerSafe())))
		{
			return;
		}
		foreach (Pawn relatedPawn in pawn.relations.RelatedPawns)
		{
			if (logDotgraph != null)
			{
				string text = $"{DotgraphIdentifier(pawn)}->{DotgraphIdentifier(relatedPawn)} [label=<{pawn.GetRelations(relatedPawn).FirstOrDefault().ToString()}> color=\"purple\"];";
				if (!logDotgraphUniqueLinks.Contains(text))
				{
					logDotgraphUniqueLinks.Add(text);
					logDotgraph.AppendLine(text);
				}
			}
			if (!keptPawns.ContainsKey(relatedPawn))
			{
				keptPawns[relatedPawn] = "Relationship";
			}
		}
	}

	public void AddAllMemories(Pawn pawn, Dictionary<Pawn, string> keptPawns)
	{
		if (pawn.needs == null || pawn.needs.mood == null || pawn.needs.mood.thoughts == null || pawn.needs.mood.thoughts.memories == null || (!pawn.Spawned && (!pawn.RaceProps.Humanlike || !pawn.Faction.IsPlayerSafe())))
		{
			return;
		}
		foreach (Thought_Memory memory in pawn.needs.mood.thoughts.memories.Memories)
		{
			if (memory.otherPawn == null)
			{
				continue;
			}
			if (logDotgraph != null)
			{
				string text = $"{DotgraphIdentifier(pawn)}->{DotgraphIdentifier(memory.otherPawn)} [label=<{memory.def}> color=\"orange\"];";
				if (!logDotgraphUniqueLinks.Contains(text))
				{
					logDotgraphUniqueLinks.Add(text);
					logDotgraph.AppendLine(text);
				}
			}
			if (!keptPawns.ContainsKey(memory.otherPawn))
			{
				keptPawns[memory.otherPawn] = "Memory";
			}
		}
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref lastSuccessfulGCTick, "lastSuccessfulGCTick", 0);
		Scribe_Values.Look(ref currentGCRate, "nextGCRate", 1);
	}

	public void LogGC()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("======= GC =======");
		stringBuilder.AppendLine(PawnGCDebugResults());
		Log.Message(stringBuilder.ToString());
	}

	public void RunGC()
	{
		CancelGCPass();
		PerfLogger.Reset();
		foreach (object item in PawnGCPass())
		{
			_ = item;
		}
		float num = PerfLogger.Duration() * 1000f;
		PerfLogger.Flush();
		Log.Message($"World pawn GC run complete in {num} ms");
	}

	public void LogDotgraph()
	{
		logDotgraph = new StringBuilder();
		logDotgraphUniqueLinks = new HashSet<string>();
		logDotgraph.AppendLine("digraph { rankdir=LR;");
		AccumulatePawnGCDataImmediate();
		logDotgraph.AppendLine("}");
		GUIUtility.systemCopyBuffer = logDotgraph.ToString();
		Log.Message("Dotgraph copied to clipboard");
		logDotgraph = null;
		logDotgraphUniqueLinks = null;
	}

	public static string DotgraphIdentifier(Pawn pawn)
	{
		return new string(pawn.LabelShort.Where((char ch) => char.IsLetter(ch)).ToArray()) + "_" + pawn.thingIDNumber;
	}
}
