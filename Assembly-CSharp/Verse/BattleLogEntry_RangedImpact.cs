using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse;

public class BattleLogEntry_RangedImpact : LogEntry_DamageResult
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__18 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_RangedImpact _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__18(int _003C_003E1__state)
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
			BattleLogEntry_RangedImpact battleLogEntry_RangedImpact = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_RangedImpact.initiatorPawn != null)
				{
					_003C_003E2__current = battleLogEntry_RangedImpact.initiatorPawn;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0051;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0051;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0075;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0075:
				if (battleLogEntry_RangedImpact.originalTargetPawn != null)
				{
					_003C_003E2__current = battleLogEntry_RangedImpact.originalTargetPawn;
					_003C_003E1__state = 3;
					return true;
				}
				break;
				IL_0051:
				if (battleLogEntry_RangedImpact.recipientPawn != null)
				{
					_003C_003E2__current = battleLogEntry_RangedImpact.recipientPawn;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0075;
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
			_003CGetConcerns_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__18(0)
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

	private Pawn originalTargetPawn;

	private ThingDef originalTargetThing;

	private bool originalTargetMobile;

	private ThingDef weaponDef;

	private ThingDef projectileDef;

	private ThingDef coverDef;

	[TweakValue("LogFilter", 0f, 1f)]
	private static float DisplayChanceOnMiss = 0.25f;

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

	public BattleLogEntry_RangedImpact()
	{
	}

	public BattleLogEntry_RangedImpact(Thing initiator, Thing recipient, Thing originalTarget, ThingDef weaponDef, ThingDef projectileDef, ThingDef coverDef)
	{
		if (initiator is Pawn pawn)
		{
			initiatorPawn = pawn;
		}
		else if (initiator != null)
		{
			initiatorThing = initiator.def;
		}
		if (recipient is Pawn pawn2)
		{
			recipientPawn = pawn2;
		}
		else if (recipient != null)
		{
			recipientThing = recipient.def;
		}
		if (originalTarget is Pawn pawn3)
		{
			originalTargetPawn = pawn3;
			originalTargetMobile = !originalTargetPawn.Downed && !originalTargetPawn.Dead && originalTargetPawn.Awake();
		}
		else if (originalTarget != null)
		{
			originalTargetThing = originalTarget.def;
		}
		this.weaponDef = weaponDef;
		this.projectileDef = projectileDef;
		this.coverDef = coverDef;
	}

	public override bool Concerns(Thing t)
	{
		if (t != initiatorPawn && t != recipientPawn)
		{
			return t == originalTargetPawn;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__18))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__18(-2)
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

	public override Texture2D IconFromPOV(Thing pov)
	{
		if (damagedParts.NullOrEmpty())
		{
			return null;
		}
		if (deflected)
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
		return recipientPawn?.RaceProps.body;
	}

	protected override GrammarRequest GenerateGrammarRequest()
	{
		GrammarRequest result = base.GenerateGrammarRequest();
		if (recipientPawn != null || recipientThing != null)
		{
			result.Includes.Add(deflected ? RulePackDefOf.Combat_RangedDeflect : RulePackDefOf.Combat_RangedDamage);
		}
		else
		{
			result.Includes.Add(RulePackDefOf.Combat_RangedMiss);
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
		if (originalTargetPawn != recipientPawn || originalTargetThing != recipientThing)
		{
			if (originalTargetPawn != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForPawn("ORIGINALTARGET", originalTargetPawn, result.Constants, addRelationInfoSymbol: false));
				result.Constants["ORIGINALTARGET_mobile"] = originalTargetMobile.ToString();
			}
			else if (originalTargetThing != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("ORIGINALTARGET", originalTargetThing));
			}
			else
			{
				result.Constants["ORIGINALTARGET_missing"] = "True";
			}
		}
		if (weaponDef != null)
		{
			result.Rules.AddRange(PlayLogEntryUtility.RulesForOptionalWeapon("WEAPON", weaponDef, projectileDef));
		}
		else
		{
			result.Constants["WEAPON_missing"] = "True";
			if (projectileDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef("PROJECTILE", projectileDef));
			}
		}
		if (initiatorPawn != null && initiatorPawn.skills != null)
		{
			result.Constants["INITIATOR_skill"] = initiatorPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
		}
		if (recipientPawn != null && recipientPawn.skills != null)
		{
			result.Constants["RECIPIENT_skill"] = recipientPawn.skills.GetSkill(SkillDefOf.Shooting).Level.ToStringCached();
		}
		result.Constants["COVER_missing"] = ((coverDef != null) ? "False" : "True");
		if (coverDef != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("COVER", coverDef));
		}
		return result;
	}

	public override bool ShowInCompactView()
	{
		if (!deflected)
		{
			if (recipientPawn != null)
			{
				return true;
			}
			if (originalTargetThing != null && originalTargetThing == recipientThing)
			{
				return true;
			}
		}
		int num = 1;
		if (weaponDef != null && !weaponDef.Verbs.NullOrEmpty())
		{
			num = weaponDef.Verbs[0].burstShotCount;
		}
		return Rand.ChanceSeeded(DisplayChanceOnMiss / (float)num, logID);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			if (initiatorPawn != null && initiatorPawn.Discarded)
			{
				initiatorPawn = null;
			}
			if (recipientPawn != null && recipientPawn.Discarded)
			{
				recipientPawn = null;
			}
			if (originalTargetPawn != null && originalTargetPawn.Discarded)
			{
				originalTargetPawn = null;
			}
		}
		Scribe_References.Look(ref initiatorPawn, "initiatorPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref initiatorThing, "initiatorThing");
		Scribe_References.Look(ref recipientPawn, "recipientPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref recipientThing, "recipientThing");
		Scribe_References.Look(ref originalTargetPawn, "originalTargetPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref originalTargetThing, "originalTargetThing");
		Scribe_Values.Look(ref originalTargetMobile, "originalTargetMobile", defaultValue: false);
		Scribe_Defs.Look(ref weaponDef, "weaponDef");
		Scribe_Defs.Look(ref projectileDef, "projectileDef");
		Scribe_Defs.Look(ref coverDef, "coverDef");
	}

	public override string ToString()
	{
		return "BattleLogEntry_RangedImpact: " + InitiatorName + "->" + RecipientName;
	}
}
