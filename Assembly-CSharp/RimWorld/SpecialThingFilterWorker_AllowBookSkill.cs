using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200350D RID: 13581
	public abstract class SpecialThingFilterWorker_AllowBookSkill : SpecialThingFilterWorker
	{
		// Token: 0x0601314A RID: 78154 RVA: 0x005A9121 File Offset: 0x005A7321
		protected SpecialThingFilterWorker_AllowBookSkill(SkillDef skill)
		{
			this.skill = skill;
		}

		// Token: 0x0601314B RID: 78155 RVA: 0x005A9130 File Offset: 0x005A7330
		public override bool Matches(Thing t)
		{
			Book book = t as Book;
			BookOutcomeDoerGainSkillExp bookOutcomeDoerGainSkillExp;
			return book != null && book.BookComp.TryGetDoer<BookOutcomeDoerGainSkillExp>(out bookOutcomeDoerGainSkillExp) && bookOutcomeDoerGainSkillExp.Values.ContainsKey(this.skill);
		}

		// Token: 0x0601314C RID: 78156 RVA: 0x00039A6E File Offset: 0x00037C6E
		public override bool CanEverMatch(ThingDef def)
		{
			return def.HasComp<CompBook>();
		}

		// Token: 0x0400C5E1 RID: 50657
		private readonly SkillDef skill;
	}
}
