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

public class BattleLogEntry_MeleeCombat : LogEntry_DamageResult
{
	[CompilerGenerated]
	private sealed class _003CGetConcerns_003Ed__20 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public BattleLogEntry_MeleeCombat _003C_003E4__this;

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
		public _003CGetConcerns_003Ed__20(int _003C_003E1__state)
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
			BattleLogEntry_MeleeCombat battleLogEntry_MeleeCombat = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (battleLogEntry_MeleeCombat.initiator != null)
				{
					_003C_003E2__current = battleLogEntry_MeleeCombat.initiator;
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
				if (battleLogEntry_MeleeCombat.recipientPawn != null)
				{
					_003C_003E2__current = battleLogEntry_MeleeCombat.recipientPawn;
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
			_003CGetConcerns_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetConcerns_003Ed__20(0)
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

	private RulePackDef ruleDef;

	private Pawn initiator;

	private Pawn recipientPawn;

	private ThingDef recipientThing;

	private ImplementOwnerTypeDef implementType;

	private ThingDef ownerEquipmentDef;

	private HediffDef ownerHediffDef;

	private string toolLabel;

	public bool alwaysShowInCompact;

	[TweakValue("LogFilter", 0f, 1f)]
	private static float DisplayChanceOnMiss = 0.5f;

	private string InitiatorName
	{
		get
		{
			if (initiator == null)
			{
				return "null";
			}
			return initiator.LabelShort;
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

	public RulePackDef RuleDef
	{
		get
		{
			return ruleDef;
		}
		set
		{
			ruleDef = value;
			ResetCache();
		}
	}

	public BattleLogEntry_MeleeCombat()
	{
	}

	public BattleLogEntry_MeleeCombat(RulePackDef ruleDef, bool alwaysShowInCompact, Pawn initiator, Thing recipient, ImplementOwnerTypeDef implementType, string toolLabel, ThingDef ownerEquipmentDef = null, HediffDef ownerHediffDef = null, LogEntryDef def = null)
		: base(def)
	{
		this.ruleDef = ruleDef;
		this.alwaysShowInCompact = alwaysShowInCompact;
		this.initiator = initiator;
		this.implementType = implementType;
		this.ownerEquipmentDef = ownerEquipmentDef;
		this.ownerHediffDef = ownerHediffDef;
		this.toolLabel = toolLabel;
		if (recipient is Pawn)
		{
			recipientPawn = recipient as Pawn;
		}
		else if (recipient != null)
		{
			recipientThing = recipient.def;
		}
		if (ownerEquipmentDef != null && ownerHediffDef != null)
		{
			Log.ErrorOnce($"Combat log owned by both equipment {ownerEquipmentDef.label} and hediff {ownerHediffDef.label}, may produce unexpected results", 96474669);
		}
	}

	public override bool Concerns(Thing t)
	{
		if (t != initiator)
		{
			return t == recipientPawn;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetConcerns_003Ed__20))]
	public override IEnumerable<Thing> GetConcerns()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConcerns_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool CanBeClickedFromPOV(Thing pov)
	{
		if (pov != initiator || recipientPawn == null || !CameraJumper.CanJump(recipientPawn))
		{
			if (pov == recipientPawn)
			{
				return CameraJumper.CanJump(initiator);
			}
			return false;
		}
		return true;
	}

	public override void ClickedFromPOV(Thing pov)
	{
		if (pov == initiator && recipientPawn != null)
		{
			CameraJumper.TryJumpAndSelect(recipientPawn);
		}
		else if (pov == recipientPawn)
		{
			CameraJumper.TryJumpAndSelect(initiator);
		}
		else if (recipientPawn != null)
		{
			throw new NotImplementedException();
		}
	}

	public override Texture2D IconFromPOV(Thing pov)
	{
		if (damagedParts.NullOrEmpty())
		{
			return def.iconMissTex;
		}
		if (deflected)
		{
			return def.iconMissTex;
		}
		if (pov == null || pov == recipientPawn)
		{
			return def.iconDamagedTex;
		}
		if (pov == initiator)
		{
			return def.iconDamagedFromInstigatorTex;
		}
		return def.iconDamagedTex;
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
		result.Rules.AddRange(GrammarUtility.RulesForPawn("INITIATOR", initiator, result.Constants));
		if (recipientPawn != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForPawn("RECIPIENT", recipientPawn, result.Constants));
		}
		else if (recipientThing != null)
		{
			result.Rules.AddRange(GrammarUtility.RulesForDef("RECIPIENT", recipientThing));
		}
		result.Includes.Add(ruleDef);
		if (!toolLabel.NullOrEmpty())
		{
			result.Rules.Add(new Rule_String("TOOL_label", toolLabel));
			result.Rules.Add(new Rule_String("TOOL_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(toolLabel)));
			result.Rules.Add(new Rule_String("TOOL_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(toolLabel)));
			result.Constants["TOOL_gender"] = LanguageDatabase.activeLanguage.ResolveGender(toolLabel).ToString();
		}
		if (implementType != null && !implementType.implementOwnerRuleName.NullOrEmpty())
		{
			if (ownerEquipmentDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef(implementType.implementOwnerRuleName, ownerEquipmentDef));
			}
			else if (ownerHediffDef != null)
			{
				result.Rules.AddRange(GrammarUtility.RulesForDef(implementType.implementOwnerRuleName, ownerHediffDef));
			}
		}
		if (initiator != null && initiator.skills != null)
		{
			result.Constants["INITIATOR_skill"] = initiator.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
		}
		if (recipientPawn != null && recipientPawn.skills != null)
		{
			result.Constants["RECIPIENT_skill"] = recipientPawn.skills.GetSkill(SkillDefOf.Melee).Level.ToStringCached();
		}
		if (implementType != null && !implementType.implementOwnerTypeValue.NullOrEmpty())
		{
			result.Constants["IMPLEMENTOWNER_type"] = implementType.implementOwnerTypeValue;
		}
		return result;
	}

	public override bool ShowInCompactView()
	{
		if (alwaysShowInCompact)
		{
			return true;
		}
		return Rand.ChanceSeeded(DisplayChanceOnMiss, logID);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Defs.Look(ref ruleDef, "ruleDef");
		Scribe_Values.Look(ref alwaysShowInCompact, "alwaysShowInCompact", defaultValue: false);
		Scribe_References.Look(ref initiator, "initiator", saveDestroyedThings: true);
		Scribe_References.Look(ref recipientPawn, "recipientPawn", saveDestroyedThings: true);
		Scribe_Defs.Look(ref recipientThing, "recipientThing");
		Scribe_Defs.Look(ref implementType, "implementType");
		Scribe_Defs.Look(ref ownerEquipmentDef, "ownerDef");
		Scribe_Values.Look(ref toolLabel, "toolLabel");
		BackCompatibility.PostExposeData(this);
	}

	public override string ToString()
	{
		return ruleDef.defName + ": " + InitiatorName + "->" + RecipientName;
	}
}
