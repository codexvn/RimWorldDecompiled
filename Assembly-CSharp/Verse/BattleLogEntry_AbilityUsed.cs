using System;
using RimWorld;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000323 RID: 803
	public class BattleLogEntry_AbilityUsed : BattleLogEntry_Event
	{
		// Token: 0x060013FF RID: 5119 RVA: 0x0005E735 File Offset: 0x0005C935
		public BattleLogEntry_AbilityUsed()
		{
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0005E73D File Offset: 0x0005C93D
		public BattleLogEntry_AbilityUsed(Pawn caster, Thing target, AbilityDef ability, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.abilityUsed = ability;
		}

		// Token: 0x06001401 RID: 5121 RVA: 0x0005E750 File Offset: 0x0005C950
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<AbilityDef>(ref this.abilityUsed, "abilityUsed");
		}

		// Token: 0x06001402 RID: 5122 RVA: 0x0005E768 File Offset: 0x0005C968
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ABILITY", this.abilityUsed));
			if (this.subjectPawn == null && this.subjectThing == null)
			{
				result.Rules.Add(new Rule_String("SUBJECT_definite", "AreaLower".Translate()));
			}
			return result;
		}

		// Token: 0x040012F8 RID: 4856
		public AbilityDef abilityUsed;
	}
}
