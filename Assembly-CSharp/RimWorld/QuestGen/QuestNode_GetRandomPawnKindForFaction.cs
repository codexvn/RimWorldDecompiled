using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AC7 RID: 15047
	public class QuestNode_GetRandomPawnKindForFaction : QuestNode
	{
		// Token: 0x06014843 RID: 84035 RVA: 0x0061F647 File Offset: 0x0061D847
		protected override bool TestRunInt(Slate slate)
		{
			return this.SetVars(slate);
		}

		// Token: 0x06014844 RID: 84036 RVA: 0x0061F650 File Offset: 0x0061D850
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014845 RID: 84037 RVA: 0x0061F660 File Offset: 0x0061D860
		private bool SetVars(Slate slate)
		{
			Thing value = this.factionOf.GetValue(slate);
			if (value == null)
			{
				return false;
			}
			Faction faction = value.Faction;
			if (faction == null)
			{
				return false;
			}
			List<QuestNode_GetRandomPawnKindForFaction.Choice> value2 = this.choices.GetValue(slate);
			for (int i = 0; i < value2.Count; i++)
			{
				PawnKindDef var;
				if (((value2[i].factionDef != null && faction.def == value2[i].factionDef) || (!value2[i].categoryTag.NullOrEmpty() && value2[i].categoryTag == faction.def.categoryTag)) && value2[i].pawnKinds.TryRandomElement(out var))
				{
					slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), var, false);
					return true;
				}
			}
			if (this.fallback.GetValue(slate) != null)
			{
				slate.Set<PawnKindDef>(this.storeAs.GetValue(slate), this.fallback.GetValue(slate), false);
				return true;
			}
			return false;
		}

		// Token: 0x0400E35E RID: 58206
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E35F RID: 58207
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E360 RID: 58208
		public SlateRef<List<QuestNode_GetRandomPawnKindForFaction.Choice>> choices;

		// Token: 0x0400E361 RID: 58209
		public SlateRef<PawnKindDef> fallback;

		// Token: 0x02003AC8 RID: 15048
		public class Choice
		{
			// Token: 0x0400E362 RID: 58210
			public FactionDef factionDef;

			// Token: 0x0400E363 RID: 58211
			public string categoryTag;

			// Token: 0x0400E364 RID: 58212
			public List<PawnKindDef> pawnKinds;
		}
	}
}
