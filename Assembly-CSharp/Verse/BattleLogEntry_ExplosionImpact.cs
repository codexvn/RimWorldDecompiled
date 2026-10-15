using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse;

public class BattleLogEntry_ExplosionImpact : LogEntry_DamageResult
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__14 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_ExplosionImpact _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__14(int _003C_003E1__state)
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
			BattleLogEntry_ExplosionImpact battleLogEntry_ExplosionImpact = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_ExplosionImpact.initiatorPawn != null)
				{
					_003C_003E2__current = battleLogEntry_ExplosionImpact.initiatorPawn;
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
				if (battleLogEntry_ExplosionImpact.recipientPawn != null)
				{
					_003C_003E2__current = battleLogEntry_ExplosionImpact.recipientPawn;
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
			_003CGetConcerns_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__14(0)
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

	private ThingDef initiatorThing;

	private Pawn recipientPawn;

	private ThingDef recipientThing;

	private ThingDef weaponDef;

	private ThingDef projectileDef;

	private DamageDef damageDef;

	private string InitiatorName
	{
		get
		{
			if (initiatorPawn != null)
			{
				return initiatorPawn.LabelShort;
			}
			if (initiatorThing != null)
			{
				return initiatorThing.defName;
			}
			return "null";
		}
	}

	private string RecipientName
	{
		get
		{
			if (recipientPawn != null)
			{
				return recipientPawn.LabelShort;
			}
			if (recipientThing != null)
			{
				return recipientThing.defName;
			}
			return "null";
		}
	}

	public BattleLogEntry_ExplosionImpact()
	{
	}

	public BattleLogEntry_ExplosionImpact(Thing initiator, Thing recipient, ThingDef weaponDef, ThingDef projectileDef, DamageDef damageDef)
	{
		if (initiator is Pawn)
		{
			initiatorPawn = initiator as Pawn;
		}
		else if (initiator != null)
		{
			initiatorThing = initiator.def;
		}
		if (recipient is Pawn)
		{
			recipientPawn = recipient as Pawn;
		}
		else if (recipient != null)
		{
			recipientThing = recipient.def;
		}
		this.weaponDef = weaponDef;
		this.projectileDef = projectileDef;
		this.damageDef = damageDef;
	}

	public override bool Concerns(Thing t)
	{
		if (t != initiatorPawn)
		{
			return t == recipientPawn;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__14))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (pov != initiatorPawn || recipientPawn == null || !CameraJumper.CanJump(recipientPawn))
		{
			if (pov == recipientPawn)
			{
				return CameraJumper.CanJump(initiatorPawn);
			}
			return false;
		}
		return true;
	}

	public override void ClickedFromPOV(Thing pov)
	{
		if (recipientPawn == null)
		{
			return;
		}
		if (pov == initiatorPawn)
		{
			CameraJumper.TryJumpAndSelect(recipientPawn);
			return;
		}
		if (pov == recipientPawn)
		{
			CameraJumper.TryJumpAndSelect(initiatorPawn);
			return;
		}
		throw new NotImplementedException();
	}

	public override Texture2D IconFromPOV(Thing pov)
	{
		if (damagedParts.NullOrEmpty())
		{
			return null;
		}
		if (pov == null || pov == recipientPawn)
		{
			return LogEntry.Blood;
		}
		if (pov == initiatorPawn)
		{
			return LogEntry.BloodTarget;
		}
		return null;
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
		result.Includes.Add(RulePackDefOf.Combat_ExplosionImpact);
		if (initiatorPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiatorPawn, result.Constants));
		}
		else if (initiatorThing != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("INITIATOR", initiatorThing));
		}
		else
		{
			result.Constants["INITIATOR_missing"] = "True";
		}
		if (recipientPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", recipientPawn, result.Constants));
		}
		else if (recipientThing != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", recipientThing));
		}
		else
		{
			result.Constants["RECIPIENT_missing"] = "True";
		}
		result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", weaponDef, projectileDef));
		if (projectileDef != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", projectileDef));
		}
		if (damageDef != null && damageDef.combatLogRules != null)
		{
			result.Includes.Add(damageDef.combatLogRules);
		}
		return result;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref initiatorPawn, "initiatorPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref initiatorThing, "initiatorThing");
		Scribe_References.Look(ref recipientPawn, "recipientPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref recipientThing, "recipientThing");
		Scribe_Defs.Look(ref weaponDef, "weaponDef");
		Scribe_Defs.Look(ref projectileDef, "projectileDef");
		Scribe_Defs.Look(ref damageDef, "damageDef");
	}

	public override string ToString()
	{
		return "BattleLogEntry_ExplosionImpact: " + InitiatorName + "->" + RecipientName;
	}
}
