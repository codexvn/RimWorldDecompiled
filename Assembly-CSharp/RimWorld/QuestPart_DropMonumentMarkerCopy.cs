using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_DropMonumentMarkerCopy : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__6 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_DropMonumentMarkerCopy _003C_003E4__this;

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
		public _003Cget_QuestLookTargets_003Ed__6(int _003C_003E1__state)
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
				QuestPart_DropMonumentMarkerCopy questPart_DropMonumentMarkerCopy = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_DropMonumentMarkerCopy).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00bb;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						GlobalTargetInfo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (questPart_DropMonumentMarkerCopy.mapParent != null)
					{
						_003C_003E2__current = questPart_DropMonumentMarkerCopy.mapParent;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00bb;
					IL_00bb:
					if (questPart_DropMonumentMarkerCopy.copy != null)
					{
						_003C_003E2__current = questPart_DropMonumentMarkerCopy.copy;
						_003C_003E1__state = 3;
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
			_003Cget_QuestLookTargets_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__6(0)
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

	public MapParent mapParent;

	public string inSignal;

	public string outSignalResult;

	public bool destroyOrPassToWorldOnCleanup;

	private MonumentMarker copy;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__6))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__6(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (!(signal.tag == inSignal))
		{
			return;
		}
		copy = null;
		MonumentMarker arg = signal.args.GetArg<MonumentMarker>("SUBJECT");
		if (mapParent == null || !mapParent.HasMap || !quest.IsParentSuitableForQuest(mapParent))
		{
			mapParent = quest.TryFindNewSuitableMapParentForRetarget();
		}
		if (arg != null && mapParent != null && mapParent.HasMap)
		{
			Map map = mapParent.Map;
			IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map);
			copy = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker);
			copy.sketch = arg.sketch.DeepCopy();
			if (!arg.questTags.NullOrEmpty())
			{
				copy.questTags = new List<string>();
				copy.questTags.AddRange(arg.questTags);
			}
			DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle((Thing)copy.MakeMinified()), 110, canInstaDropDuringInit: false, leaveSlag: false, canRoofPunch: true, forbid: false);
		}
		if (!outSignalResult.NullOrEmpty())
		{
			if (copy != null)
			{
				Find.SignalManager.SendSignal(new Signal(outSignalResult, copy.Named("SUBJECT")));
			}
			else
			{
				Find.SignalManager.SendSignal(new Signal(outSignalResult));
			}
		}
	}

	public override void Cleanup()
	{
		base.Cleanup();
		if (destroyOrPassToWorldOnCleanup && copy != null)
		{
			QuestPart_DestroyThingsOrPassToWorld.Destroy(copy);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Values.Look(ref outSignalResult, "outSignalResult");
		Scribe_References.Look(ref mapParent, "mapParent");
		Scribe_References.Look(ref copy, "copy");
		Scribe_Values.Look(ref destroyOrPassToWorldOnCleanup, "destroyOrPassToWorldOnCleanup", defaultValue: false);
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			mapParent = Find.RandomPlayerHomeMap.Parent;
		}
	}
}
