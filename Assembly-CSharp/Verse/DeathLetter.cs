using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using RimWorld.Planet;

namespace Verse;

public class DeathLetter : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__3 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public DeathLetter _003C_003E4__this;

		DiaOption IEnumerator<DiaOption>.Current
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
		public _003Cget_Choices_003Ed__3(int _003C_003E1__state)
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
			DeathLetter deathLetter = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = deathLetter.Option_Close;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				if (deathLetter.lookTargets.IsValid())
				{
					_003C_003E2__current = deathLetter.Option_ReadMore;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0072;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0072;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0072:
				if (deathLetter.quest != null)
				{
					_003C_003E2__current = deathLetter.Option_ViewInQuestsTab();
					_003C_003E1__state = 3;
					return true;
				}
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
		IEnumerator<DiaOption> IEnumerable<DiaOption>.GetEnumerator()
		{
			_003Cget_Choices_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DiaOption>)this).GetEnumerator();
		}
	}

	protected DiaOption Option_ReadMore
	{
		get
		{
			GlobalTargetInfo target = lookTargets.TryGetPrimaryTarget();
			DiaOption diaOption = new DiaOption("ReadMore".Translate());
			diaOption.action = delegate
			{
				CameraJumper.TryJumpAndSelect(target);
				Find.LetterStack.RemoveLetter(this);
				InspectPaneUtility.OpenTab(typeof(ITab_Pawn_Log));
			};
			diaOption.resolveTree = true;
			if (!target.IsValid)
			{
				diaOption.Disable(null);
			}
			return diaOption;
		}
	}

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void OpenLetter()
	{
		Pawn targetPawn = lookTargets.TryGetPrimaryTarget().Thing as Pawn;
		TaggedString taggedString = base.Text;
		string text = (from entry in (from entry in Find.BattleLog.Battles.Where((Battle battle) => battle.Concerns(targetPawn)).SelectMany((Battle battle) => battle.Entries.Where((LogEntry entry) => entry.Concerns(targetPawn) && entry.ShowInCompactView()))
				orderby entry.Age
				select entry).Take(5).Reverse()
			select "  " + entry.ToGameStringFromPOV(null)).ToLineList();
		if (text.Length > 0)
		{
			taggedString = string.Format("{0}\n\n{1}\n{2}", taggedString, "LastEventsInLife".Translate(targetPawn.LabelDefinite(), targetPawn.Named("PAWN")).Resolve() + ":", text);
		}
		DiaNode diaNode = new DiaNode(taggedString);
		diaNode.options.AddRange(Choices);
		Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, relatedFaction, delayInteractivity: false, radioMode, title));
	}
}
