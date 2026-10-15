using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class ChoiceLetter_AcceptJoiner : ChoiceLetter
{
	[CompilerGenerated]
	private sealed class _003Cget_Choices_003Ed__10 : IEnumerable<DiaOption>, IEnumerable, IEnumerator<DiaOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DiaOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ChoiceLetter_AcceptJoiner _003C_003E4__this;

		private DiaOption _003CoptionReject_003E5__2;

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
		public _003Cget_Choices_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CoptionReject_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ChoiceLetter_AcceptJoiner CS_0024_003C_003E8__locals10 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals10.ArchivedOnly)
				{
					_003C_003E2__current = CS_0024_003C_003E8__locals10.Option_Close;
					_003C_003E1__state = 1;
					return true;
				}
				DiaOption diaOption = new DiaOption("AcceptButton".Translate());
				_003CoptionReject_003E5__2 = new DiaOption("RejectLetter".Translate());
				diaOption.action = delegate
				{
					Find.SignalManager.SendSignal(new Signal(CS_0024_003C_003E8__locals10.signalAccept));
					Find.LetterStack.RemoveLetter(CS_0024_003C_003E8__locals10);
				};
				diaOption.resolveTree = true;
				_003CoptionReject_003E5__2.action = delegate
				{
					Find.SignalManager.SendSignal(new Signal(CS_0024_003C_003E8__locals10.signalReject));
					Find.LetterStack.RemoveLetter(CS_0024_003C_003E8__locals10);
				};
				_003CoptionReject_003E5__2.resolveTree = true;
				Map mapToUse = CS_0024_003C_003E8__locals10.MapToUse;
				if (mapToUse == null)
				{
					diaOption.Disable("CannotAcceptQuestNoMap".Translate());
				}
				else if (mapToUse.Tile.LayerDef.isSpace)
				{
					PlanetLayerDef layerDef = mapToUse.Tile.LayerDef;
					diaOption.Disable("CannotAcceptQuestFromLayer".Translate(layerDef.gerundLabel.Named("GERUND"), layerDef.Named("LAYER")));
				}
				_003C_003E2__current = diaOption;
				_003C_003E1__state = 2;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CoptionReject_003E5__2;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals10.lookTargets.IsValid())
				{
					_003C_003E2__current = CS_0024_003C_003E8__locals10.Option_JumpToLocationAndPostpone;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_019f;
			case 4:
				_003C_003E1__state = -1;
				goto IL_019f;
			case 5:
				{
					_003C_003E1__state = -1;
					_003CoptionReject_003E5__2 = null;
					break;
				}
				IL_019f:
				_003C_003E2__current = CS_0024_003C_003E8__locals10.Option_Postpone;
				_003C_003E1__state = 5;
				return true;
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
			_003Cget_Choices_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_Choices_003Ed__10(0)
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

	public string signalAccept;

	public string signalReject;

	public Map overrideMap;

	public override bool CanDismissWithRightClick => false;

	public override bool CanShowInLetterStack
	{
		get
		{
			if (base.CanShowInLetterStack && quest != null)
			{
				if (quest.State != QuestState.Ongoing)
				{
					return quest.State == QuestState.NotYetAccepted;
				}
				return true;
			}
			return false;
		}
	}

	public Map MapToUse => overrideMap ?? lookTargets.PrimaryTarget.Map ?? Find.AnyPlayerHomeMap;

	public override IEnumerable<DiaOption> Choices
	{
		[IteratorStateMachine(typeof(_003Cget_Choices_003Ed__10))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Choices_003Ed__10(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref signalAccept, "signalAccept");
		Scribe_Values.Look(ref signalReject, "signalReject");
		Scribe_References.Look(ref overrideMap, "overrideMap");
	}
}
