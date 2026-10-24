using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using Verse.Grammar;

namespace Verse;

public class BattleLogEntry_RangedFire : LogEntry
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__15 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_RangedFire _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__15(int _003C_003E1__state)
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
			BattleLogEntry_RangedFire battleLogEntry_RangedFire = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_RangedFire.initiatorPawn != null)
				{
					_003C_003E2__current = battleLogEntry_RangedFire.initiatorPawn;
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
				if (battleLogEntry_RangedFire.recipientPawn != null)
				{
					_003C_003E2__current = battleLogEntry_RangedFire.recipientPawn;
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
			_003CGetConcerns_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__15(0)
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

	private bool burst;

	[TweakValue("LogFilter", 0f, 1f)]
	private static float DisplayChance = 0.25f;

	private string InitiatorName
	{
		get
		{
			if (initiatorPawn == null)
			{
				return "null";
			}
			return initiatorPawn.LabelShort;
		}
	}

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

	public BattleLogEntry_RangedFire()
	{
	}

	public BattleLogEntry_RangedFire(Thing initiator, Thing target, ThingDef weaponDef, ThingDef projectileDef, bool burst)
	{
		if (initiator is Pawn)
		{
			initiatorPawn = initiator as Pawn;
		}
		else if (initiator != null)
		{
			initiatorThing = initiator.def;
		}
		if (target is Pawn)
		{
			recipientPawn = target as Pawn;
		}
		else if (target != null)
		{
			recipientThing = target.def;
		}
		this.weaponDef = weaponDef;
		this.projectileDef = projectileDef;
		this.burst = burst;
	}

	public override bool Concerns(Thing t)
	{
		if (t != initiatorPawn)
		{
			return t == recipientPawn;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__15))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (recipientPawn != null)
		{
			if (pov != initiatorPawn || !CameraJumper.CanJump(recipientPawn))
			{
				if (pov == recipientPawn)
				{
					return CameraJumper.CanJump(initiatorPawn);
				}
				return false;
			}
			return true;
		}
		return false;
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

	protected override GrammarRequest GenerateGrammarRequest()
	{
		GrammarRequest result = base.GenerateGrammarRequest();
		if (initiatorPawn == null && initiatorThing == null)
		{
			Log.ErrorOnce("BattleLogEntry_RangedFire has a null initiator.", 60465709);
		}
		if (weaponDef != null && weaponDef.Verbs[0].rangedFireRulepack != null)
		{
			result.Includes.Add(weaponDef.Verbs[0].rangedFireRulepack);
		}
		else
		{
			result.Includes.Add(RulePackDefOf.Combat_RangedFire);
		}
		if (initiatorPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiatorPawn, result.Constants, addRelationInfoSymbol: false));
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
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", recipientPawn, result.Constants, addRelationInfoSymbol: false));
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
		if (initiatorPawn != null && initiatorPawn.skills != null)
		{
			result.Constants["INITIATOR_skill"] = initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
		}
		if (recipientPawn != null && recipientPawn.skills != null)
		{
			result.Constants["RECIPIENT_skill"] = recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
		}
		result.Constants["BURST"] = burst.ToString();
		return result;
	}

	public override bool ShowInCompactView()
	{
		return Rand.ChanceSeeded(DisplayChance, logID);
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
		Scribe_Values.Look(ref burst, "burst", defaultValue: false);
	}

	public override string ToString()
	{
		return "BattleLogEntry_RangedFire: " + InitiatorName + "->" + RecipientName;
	}
}
