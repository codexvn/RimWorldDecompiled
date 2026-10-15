using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse;

public class BattleLogEntry_StateTransition : LogEntry
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__12 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_StateTransition _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__12(int _003C_003E1__state)
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
			BattleLogEntry_StateTransition battleLogEntry_StateTransition = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_StateTransition.initiator != null)
				{
					_003C_003E2__current = battleLogEntry_StateTransition.initiator;
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
				if (battleLogEntry_StateTransition.subjectPawn != null)
				{
					_003C_003E2__current = battleLogEntry_StateTransition.subjectPawn;
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
			_003CGetConcerns_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__12(0)
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

	private RulePackDef transitionDef;

	private Pawn subjectPawn;

	private ThingDef subjectThing;

	private Pawn initiator;

	private HediffDef culpritHediffDef;

	private BodyPartRecord culpritHediffTargetPart;

	private BodyPartRecord culpritTargetPart;

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

	public BattleLogEntry_StateTransition()
	{
	}

	public BattleLogEntry_StateTransition(Thing subject, RulePackDef transitionDef, Pawn initiator, Hediff culpritHediff, BodyPartRecord culpritTargetDef)
	{
		if (subject is Pawn)
		{
			subjectPawn = subject as Pawn;
		}
		else if (subject != null)
		{
			subjectThing = subject.def;
		}
		this.transitionDef = transitionDef;
		this.initiator = initiator;
		if (culpritHediff != null)
		{
			culpritHediffDef = culpritHediff.def;
			if (culpritHediff.Part != null)
			{
				culpritHediffTargetPart = culpritHediff.Part;
			}
		}
		culpritTargetPart = culpritTargetDef;
	}

	public override bool Concerns(Thing t)
	{
		if (t != subjectPawn)
		{
			return t == initiator;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__12))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (pov != subjectPawn || !CameraJumper.CanJump(initiator))
		{
			if (pov == initiator)
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
			CameraJumper.TryJumpAndSelect(initiator);
			return;
		}
		if (pov == initiator)
		{
			CameraJumper.TryJumpAndSelect(subjectPawn);
			return;
		}
		throw new NotImplementedException();
	}

	public override Texture2D IconFromPOV(Thing pov)
	{
		if (pov == null || pov == subjectPawn)
		{
			if (transitionDef != RulePackDefOf.Transition_Downed)
			{
				return LogEntry.Skull;
			}
			return LogEntry.Downed;
		}
		if (pov == initiator)
		{
			if (transitionDef != RulePackDefOf.Transition_Downed)
			{
				return LogEntry.SkullTarget;
			}
			return LogEntry.DownedTarget;
		}
		return null;
	}

	protected override GrammarRequest GenerateGrammarRequest()
	{
		GrammarRequest result = base.GenerateGrammarRequest();
		if (subjectPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("SUBJECT", subjectPawn, result.Constants));
		}
		else if (subjectThing != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("SUBJECT", subjectThing));
		}
		result.Includes.Add(transitionDef);
		if (initiator != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiator, result.Constants));
		}
		if (culpritHediffDef != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForHediffDef("CULPRITHEDIFF", culpritHediffDef, culpritHediffTargetPart));
		}
		if (culpritHediffTargetPart != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_target", culpritHediffTargetPart));
		}
		if (culpritTargetPart != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForBodyPartRecord("CULPRITHEDIFF_originaltarget", culpritTargetPart));
		}
		return result;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref transitionDef, "transitionDef");
		Scribe_References.Look(ref subjectPawn, "subjectPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref subjectThing, "subjectThing");
		Scribe_References.Look(ref initiator, "initiator", saveDestroyedThings: true);
		Scribe_Defs.Look(ref culpritHediffDef, "culpritHediffDef");
		Scribe_BodyParts.Look(ref culpritHediffTargetPart, "culpritHediffTargetPart");
		Scribe_BodyParts.Look(ref culpritTargetPart, "culpritTargetPart");
	}

	public override string ToString()
	{
		return transitionDef.defName + ": " + subjectPawn;
	}
}
