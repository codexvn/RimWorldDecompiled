using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AC4 RID: 15044
	public class QuestNode_ExtraFaction : QuestNode
	{
		// Token: 0x0601483A RID: 84026 RVA: 0x0061F460 File Offset: 0x0061D660
		protected override void RunInt()
		{
			Faction value = this.faction.GetValue(QuestGen.slate);
			if (value == null)
			{
				Thing value2 = this.factionOf.GetValue(QuestGen.slate);
				if (value2 != null)
				{
					value = value2.Faction;
				}
				if (value == null)
				{
					return;
				}
			}
			QuestGen.quest.AddPart(new QuestPart_ExtraFaction
			{
				affectedPawns = this.pawns.GetValue(QuestGen.slate).ToList<Pawn>(),
				extraFaction = new ExtraFaction(value, this.factionType.GetValue(QuestGen.slate)),
				areHelpers = this.areHelpers.GetValue(QuestGen.slate),
				inSignalRemovePawn = this.inSignalRemovePawn.GetValue(QuestGen.slate),
				inSignalsRemovePawn = this.inSignalsRemovePawn.GetValue(QuestGen.slate).ToList<string>()
			});
		}

		// Token: 0x0601483B RID: 84027 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0400E350 RID: 58192
		public SlateRef<Thing> factionOf;

		// Token: 0x0400E351 RID: 58193
		public SlateRef<Faction> faction;

		// Token: 0x0400E352 RID: 58194
		public SlateRef<IEnumerable<Pawn>> pawns;

		// Token: 0x0400E353 RID: 58195
		public SlateRef<ExtraFactionType> factionType;

		// Token: 0x0400E354 RID: 58196
		public SlateRef<bool> areHelpers;

		// Token: 0x0400E355 RID: 58197
		[NoTranslate]
		public SlateRef<string> inSignalRemovePawn;

		// Token: 0x0400E356 RID: 58198
		[NoTranslate]
		public SlateRef<IEnumerable<string>> inSignalsRemovePawn;
	}
}
