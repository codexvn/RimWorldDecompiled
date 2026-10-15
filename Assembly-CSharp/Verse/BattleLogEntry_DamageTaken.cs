using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse.Grammar;

namespace Verse;

public class BattleLogEntry_DamageTaken : LogEntry_DamageResult
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__8 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_DamageTaken _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__8(int _003C_003E1__state)
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
			BattleLogEntry_DamageTaken battleLogEntry_DamageTaken = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_DamageTaken.initiatorPawn != null)
				{
					_003C_003E2__current = battleLogEntry_DamageTaken.initiatorPawn;
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
				if (battleLogEntry_DamageTaken.recipientPawn != null)
				{
					_003C_003E2__current = battleLogEntry_DamageTaken.recipientPawn;
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
			_003CGetConcerns_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__8(0)
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

	private Pawn initiatorPawn;

	private Pawn recipientPawn;

	private RulePackDef ruleDef;

	private string RecipientName
	{
		get
		{
			if (recipientPawn == null)
			{
				return "null";
			}
			return recipientPawn.LabelShort;
		}
	}

	public BattleLogEntry_DamageTaken()
	{
	}

	public BattleLogEntry_DamageTaken(Pawn recipient, RulePackDef ruleDef, Pawn initiator = null)
	{
		initiatorPawn = initiator;
		recipientPawn = recipient;
		this.ruleDef = ruleDef;
	}

	public override bool Concerns(Thing t)
	{
		if (t != initiatorPawn)
		{
			return t == recipientPawn;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__8))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		return CameraJumper.CanJump(recipientPawn);
	}

	public override void ClickedFromPOV(Thing pov)
	{
		CameraJumper.TryJumpAndSelect(recipientPawn);
	}

	public override Texture2D IconFromPOV(Thing pov)
	{
		return LogEntry.Blood;
	}

	protected override BodyDef DamagedBody()
	{
		if (recipientPawn == null)
		{
			return null;
		}
		return recipientPawn.RaceProps.body;
	}

	protected override GrammarRequest GenerateGrammarRequest()
	{
		GrammarRequest result = base.GenerateGrammarRequest();
		if (recipientPawn == null)
		{
			Log.ErrorOnce("BattleLogEntry_DamageTaken has a null recipient.", 60465709);
		}
		result.Includes.Add(ruleDef);
		result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", recipientPawn, result.Constants));
		return result;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref initiatorPawn, "initiatorPawn", saveDestroyedThings: true);
		Scribe_References.Look(ref recipientPawn, "recipientPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref ruleDef, "ruleDef");
	}

	public override string ToString()
	{
		return "BattleLogEntry_DamageTaken: " + RecipientName;
	}
}
