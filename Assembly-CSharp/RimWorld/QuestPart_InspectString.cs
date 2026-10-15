using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class QuestPart_InspectString : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_QuestLookTargets_003Ed__5 : IEnumerable<GlobalTargetInfo>, IEnumerable, IEnumerator<GlobalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GlobalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_InspectString _003C_003E4__this;

		private IEnumerator<GlobalTargetInfo> _003C_003E7__wrap1;

		private int _003Ci_003E5__3;

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
		public _003Cget_QuestLookTargets_003Ed__5(int _003C_003E1__state)
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
				QuestPart_InspectString questPart_InspectString = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((QuestPart)questPart_InspectString).QuestLookTargets.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					goto IL_010b;
				case 3:
					{
						_003C_003E1__state = -1;
						goto IL_010b;
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
					_003Ci_003E5__3 = 0;
					goto IL_011d;
					IL_011d:
					if (_003Ci_003E5__3 < questPart_InspectString.targets.Count)
					{
						ISelectable selectable = questPart_InspectString.targets[_003Ci_003E5__3];
						if (selectable is Thing)
						{
							_003C_003E2__current = (Thing)selectable;
							_003C_003E1__state = 2;
							return true;
						}
						if (selectable is WorldObject)
						{
							_003C_003E2__current = (WorldObject)selectable;
							_003C_003E1__state = 3;
							return true;
						}
						goto IL_010b;
					}
					return false;
					IL_010b:
					_003Ci_003E5__3++;
					goto IL_011d;
				}
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
			_003Cget_QuestLookTargets_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_QuestLookTargets_003Ed__5(0)
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

	public List<ISelectable> targets = new List<ISelectable>();

	public string inspectString;

	private string resolvedInspectString;

	private ILoadReferenceable targetRef;

	public override IEnumerable<GlobalTargetInfo> QuestLookTargets
	{
		[IteratorStateMachine(typeof(_003Cget_QuestLookTargets_003Ed__5))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_QuestLookTargets_003Ed__5(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected override void Enable(SignalArgs receivedArgs)
	{
		base.Enable(receivedArgs);
		resolvedInspectString = receivedArgs.GetFormattedText(inspectString);
	}

	public override string ExtraInspectString(ISelectable target)
	{
		if (targets.Contains(target))
		{
			return resolvedInspectString;
		}
		return null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref targets, "targets", LookMode.Reference);
		Scribe_Values.Look(ref inspectString, "inspectString");
		Scribe_Values.Look(ref resolvedInspectString, "resolvedInspectString");
	}

	public override void AssignDebugData()
	{
		base.AssignDebugData();
		if (Find.AnyPlayerHomeMap != null)
		{
			targets.Add(Find.RandomPlayerHomeMap.mapPawns.FreeColonists.FirstOrDefault());
			inspectString = "Debug inspect string.";
		}
	}
}
