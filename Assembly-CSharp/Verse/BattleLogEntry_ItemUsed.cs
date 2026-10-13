using System;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x0200032A RID: 810
	public class BattleLogEntry_ItemUsed : BattleLogEntry_Event
	{
		// Token: 0x0600143E RID: 5182 RVA: 0x0005E735 File Offset: 0x0005C935
		public BattleLogEntry_ItemUsed()
		{
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x0005F27F File Offset: 0x0005D47F
		public BattleLogEntry_ItemUsed(Pawn caster, Thing target, ThingDef itemUsed, RulePackDef eventDef) : base(target, eventDef, caster)
		{
			this.itemUsed = itemUsed;
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x0005F292 File Offset: 0x0005D492
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.itemUsed, "itemUsed");
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x0005F2AC File Offset: 0x0005D4AC
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(GrammarUtility.RulesForDef("ITEM", this.itemUsed));
			return result;
		}

		// Token: 0x04001314 RID: 4884
		public ThingDef itemUsed;
	}
}
