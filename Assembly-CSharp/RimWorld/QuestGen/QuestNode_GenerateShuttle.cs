using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A2E RID: 14894
	public class QuestNode_GenerateShuttle : QuestNode
	{
		// Token: 0x0601463F RID: 83519 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014640 RID: 83520 RVA: 0x00617B3C File Offset: 0x00615D3C
		protected override void RunInt()
		{
			if (!ModLister.CheckRoyaltyOrIdeology("Shuttle"))
			{
				return;
			}
			Slate slate = QuestGen.slate;
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Shuttle, null);
			if (this.owningFaction.GetValue(slate) != null)
			{
				thing.SetFaction(this.owningFaction.GetValue(slate), null);
			}
			CompShuttle compShuttle = thing.TryGetComp<CompShuttle>();
			if (this.requiredPawns.GetValue(slate) != null)
			{
				compShuttle.requiredPawns.AddRange(this.requiredPawns.GetValue(slate));
			}
			if (this.requiredItems.GetValue(slate) != null)
			{
				compShuttle.requiredItems.AddRange(this.requiredItems.GetValue(slate));
			}
			compShuttle.acceptColonists = this.acceptColonists.GetValue(slate);
			compShuttle.acceptChildren = (this.acceptChildren.GetValue(slate) ?? true);
			compShuttle.onlyAcceptColonists = this.onlyAcceptColonists.GetValue(slate);
			compShuttle.onlyAcceptHealthy = this.onlyAcceptHealthy.GetValue(slate);
			compShuttle.requiredColonistCount = this.requireColonistCount.GetValue(slate);
			compShuttle.permitShuttle = this.permitShuttle.GetValue(slate);
			compShuttle.minAge = this.minAge.GetValue(slate).GetValueOrDefault();
			float num;
			if (this.overrideMass.TryGetValue(slate, out num) && num > 0f)
			{
				compShuttle.Transporter.massCapacityOverride = num;
			}
			QuestGen.slate.Set<Thing>(this.storeAs.GetValue(slate), thing, false);
		}

		// Token: 0x0400E148 RID: 57672
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E149 RID: 57673
		public SlateRef<IEnumerable<Pawn>> requiredPawns;

		// Token: 0x0400E14A RID: 57674
		public SlateRef<IEnumerable<ThingDefCount>> requiredItems;

		// Token: 0x0400E14B RID: 57675
		public SlateRef<int> requireColonistCount;

		// Token: 0x0400E14C RID: 57676
		public SlateRef<bool> acceptColonists;

		// Token: 0x0400E14D RID: 57677
		public SlateRef<bool?> acceptChildren;

		// Token: 0x0400E14E RID: 57678
		public SlateRef<bool> onlyAcceptColonists;

		// Token: 0x0400E14F RID: 57679
		public SlateRef<bool> onlyAcceptHealthy;

		// Token: 0x0400E150 RID: 57680
		public SlateRef<Faction> owningFaction;

		// Token: 0x0400E151 RID: 57681
		public SlateRef<bool> permitShuttle;

		// Token: 0x0400E152 RID: 57682
		public SlateRef<float> overrideMass;

		// Token: 0x0400E153 RID: 57683
		public SlateRef<float?> minAge;
	}
}
