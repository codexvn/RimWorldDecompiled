using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003AFF RID: 15103
	public class QuestNode_SpawnSkyfaller : QuestNode
	{
		// Token: 0x06014901 RID: 84225 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014902 RID: 84226 RVA: 0x0062212C File Offset: 0x0062032C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Map map = QuestGen.slate.Get<Map>("map", null, false);
			Skyfaller thing = SkyfallerMaker.MakeSkyfaller(this.skyfallerDef.GetValue(slate), this.innerThings.GetValue(slate));
			QuestPart_SpawnThing questPart_SpawnThing = new QuestPart_SpawnThing();
			questPart_SpawnThing.thing = thing;
			questPart_SpawnThing.mapParent = map.Parent;
			if (this.factionOfForSafeSpot.GetValue(slate) != null)
			{
				questPart_SpawnThing.factionForFindingSpot = this.factionOfForSafeSpot.GetValue(slate).Faction;
			}
			if (this.cell.GetValue(slate) != null)
			{
				questPart_SpawnThing.cell = this.cell.GetValue(slate).Value;
			}
			questPart_SpawnThing.inSignal = (QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false));
			questPart_SpawnThing.lookForSafeSpot = this.lookForSafeSpot.GetValue(slate);
			questPart_SpawnThing.tryLandInShipLandingZone = this.tryLandInShipLandingZone.GetValue(slate);
			QuestGen.quest.AddPart(questPart_SpawnThing);
		}

		// Token: 0x0400E444 RID: 58436
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E445 RID: 58437
		public SlateRef<ThingDef> skyfallerDef;

		// Token: 0x0400E446 RID: 58438
		public SlateRef<IEnumerable<Thing>> innerThings;

		// Token: 0x0400E447 RID: 58439
		public SlateRef<IntVec3?> cell;

		// Token: 0x0400E448 RID: 58440
		public SlateRef<Pawn> factionOfForSafeSpot;

		// Token: 0x0400E449 RID: 58441
		public SlateRef<bool> lookForSafeSpot;

		// Token: 0x0400E44A RID: 58442
		public SlateRef<bool> tryLandInShipLandingZone;
	}
}
