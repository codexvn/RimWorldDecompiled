using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class QuestPart_WorkDisabled : QuestPartActivable
{
	[CompilerGenerated]
	private sealed class _003Cget_DisabledWorkTypes_003Ed__3 : IEnumerable<WorkTypeDef>, IEnumerable, IEnumerator<WorkTypeDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private WorkTypeDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public QuestPart_WorkDisabled _003C_003E4__this;

		private List<WorkTypeDef> _003Clist_003E5__2;

		private int _003Ci_003E5__3;

		WorkTypeDef IEnumerator<WorkTypeDef>.Current
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
		public _003Cget_DisabledWorkTypes_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			QuestPart_WorkDisabled questPart_WorkDisabled = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0084;
			}
			_003C_003E1__state = -1;
			if (questPart_WorkDisabled.State == QuestPartState.Enabled)
			{
				_003Clist_003E5__2 = DefDatabase<WorkTypeDef>.AllDefsListForReading;
				_003Ci_003E5__3 = 0;
				goto IL_0094;
			}
			goto IL_00ae;
			IL_0084:
			_003Ci_003E5__3++;
			goto IL_0094;
			IL_0094:
			if (_003Ci_003E5__3 < _003Clist_003E5__2.Count)
			{
				if ((questPart_WorkDisabled.disabledWorkTags & _003Clist_003E5__2[_003Ci_003E5__3].workTags) != 0)
				{
					_003C_003E2__current = _003Clist_003E5__2[_003Ci_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0084;
			}
			_003Clist_003E5__2 = null;
			goto IL_00ae;
			IL_00ae:
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
		IEnumerator<WorkTypeDef> IEnumerable<WorkTypeDef>.GetEnumerator()
		{
			_003Cget_DisabledWorkTypes_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_DisabledWorkTypes_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<WorkTypeDef>)this).GetEnumerator();
		}
	}

	public List<Pawn> pawns = new List<Pawn>();

	public WorkTags disabledWorkTags;

	public IEnumerable<WorkTypeDef> DisabledWorkTypes
	{
		[IteratorStateMachine(typeof(_003Cget_DisabledWorkTypes_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_DisabledWorkTypes_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	protected override void Enable(SignalArgs receivedArgs)
	{
		base.Enable(receivedArgs);
		ClearPawnWorkTypesAndSkillsCache();
	}

	public override void Cleanup()
	{
		base.Cleanup();
		ClearPawnWorkTypesAndSkillsCache();
	}

	private void ClearPawnWorkTypesAndSkillsCache()
	{
		for (int i = 0; i < pawns.Count; i++)
		{
			if (pawns[i] != null)
			{
				pawns[i].Notify_DisabledWorkTypesChanged();
				if (pawns[i].skills != null)
				{
					pawns[i].skills.Notify_SkillDisablesChanged();
				}
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
		Scribe_Values.Look(ref disabledWorkTags, "disabledWorkTags", WorkTags.None);
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			pawns.RemoveAll((Pawn x) => x == null);
		}
	}
}
