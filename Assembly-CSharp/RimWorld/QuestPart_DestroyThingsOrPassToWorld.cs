using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__4 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_DestroyThingsOrPassToWorld _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003Cget_QuestLookTargets_003Ed__4(int _003C_003E1__state)
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
			QuestPart_DestroyThingsOrPassToWorld questPart_DestroyThingsOrPassToWorld = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
			}
			else
			{
				_003C_003E1__state = -1;
				if (!questPart_DestroyThingsOrPassToWorld.questLookTargets)
				{
					goto IL_007e;
				}
				_003Ci_003E5__2 = 0;
			}
			if (_003Ci_003E5__2 < questPart_DestroyThingsOrPassToWorld.things.Count)
			{
				_003C_003E2__current = questPart_DestroyThingsOrPassToWorld.things[_003Ci_003E5__2];
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_007e;
			IL_007e:
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
		IEnumerator<GlobalTargetInfo> IEnumerable<GlobalTargetInfo>.GetEnumerator()
		{
			_003Cget_QuestLookTargets_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__4(0)
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

	public List<Thing> things = new List<Thing>();

	public bool questLookTargets = true;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__4))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__4(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void Notify_QuestSignalReceived(Signal signal)
	{
		base.Notify_QuestSignalReceived(signal);
		if (signal.tag == inSignal)
		{
			Destroy(things);
		}
	}

	public static void Destroy(List<Thing> things)
	{
		for (int num = things.Count - 1; num >= 0; num--)
		{
			Destroy(things[num]);
		}
	}

	public static void Destroy(Thing thing)
	{
		Thing thing2 = ((!(thing.ParentHolder is MinifiedThing)) ? thing : ((Thing)thing.ParentHolder));
		if (!thing2.Destroyed)
		{
			thing2.DestroyOrPassToWorld(DestroyMode.QuestLogic);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref inSignal, "inSignal");
		Scribe_Collections.Look(ref things, "things", LookMode.Reference);
		Scribe_Values.Look(ref questLookTargets, "questLookTargets", defaultValue: true);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			things.RemoveAll((Thing x) => x == null);
		}
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		inSignal = "DebugSignal" + Rand.Int;
		if (Find.AnyPlayerHomeMap != null)
		{
			List<Thing> source = Find.RandomPlayerHomeMap.listerThings.ThingsInGroup(ThingRequestGroup.Plant);
			things.Clear();
			things.Add(source.FirstOrDefault());
		}
	}
}
