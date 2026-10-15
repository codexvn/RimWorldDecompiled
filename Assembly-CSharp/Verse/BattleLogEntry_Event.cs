using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse.Grammar;

namespace Verse;

public class BattleLogEntry_Event : LogEntry
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__10 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_Event _003C_003E4__this;

		Thing IEnumerator<Thing>.Current
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
		public _003CGetConcerns_003Ed__10(int _003C_003E1__state)
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
			BattleLogEntry_Event battleLogEntry_Event = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_Event.subjectPawn != null)
				{
					_003C_003E2__current = battleLogEntry_Event.subjectPawn;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_004d;
			case 1:
				_003C_003E1__state = -1;
				goto IL_004d;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_004d:
				if (battleLogEntry_Event.initiatorPawn != null)
				{
					_003C_003E2__current = battleLogEntry_Event.initiatorPawn;
					_003C_003E1__state = 2;
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetConcerns_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	protected RulePackDef eventDef;

	protected Pawn subjectPawn;

	protected ThingDef subjectThing;

	protected Pawn initiatorPawn;

	protected ThingDef initiatorThing;

	private string SubjectName
	{
		get
		{
			if (subjectPawn == null)
			{
				return "null";
			}
			return subjectPawn.LabelShort;
		}
	}

	public BattleLogEntry_Event()
	{
	}

	public BattleLogEntry_Event(Thing subject, RulePackDef eventDef, Thing initiator)
	{
		if (subject is Pawn)
		{
			subjectPawn = subject as Pawn;
		}
		else if (subject != null)
		{
			subjectThing = subject.def;
		}
		if (initiator is Pawn)
		{
			initiatorPawn = initiator as Pawn;
		}
		else if (initiator != null)
		{
			initiatorThing = initiator.def;
		}
		this.eventDef = eventDef;
	}

	public override bool Concerns(Thing t)
	{
		if (t != subjectPawn)
		{
			return t == initiatorPawn;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__10))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (pov != subjectPawn || !CameraJumper.CanJump(initiatorPawn))
		{
			if (pov == initiatorPawn)
			{
				return CameraJumper.CanJump(subjectPawn);
			}
			return false;
		}
		return true;
	}

	public override void ClickedFromPOV(Thing pov)
	{
		if (pov == subjectPawn)
		{
			CameraJumper.TryJumpAndSelect(initiatorPawn);
			return;
		}
		if (pov == initiatorPawn)
		{
			CameraJumper.TryJumpAndSelect(subjectPawn);
			return;
		}
		throw new NotImplementedException();
	}

	protected override GrammarRequest GenerateGrammarRequest()
	{
		GrammarRequest result = base.GenerateGrammarRequest();
		result.Includes.Add(eventDef);
		if (subjectPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", subjectPawn, result.Constants));
		}
		else if (subjectThing != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", subjectThing));
		}
		if (initiatorPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiatorPawn, result.Constants));
		}
		else if (initiatorThing != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", initiatorThing));
		}
		return result;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref eventDef, "eventDef");
		Scribe_References.Look(ref subjectPawn, "subjectPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref subjectThing, "subjectThing");
		Scribe_References.Look(ref initiatorPawn, "initiatorPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref initiatorThing, "initiatorThing");
	}

	public override string ToString()
	{
		return eventDef.defName + ": " + subjectPawn;
	}
}
