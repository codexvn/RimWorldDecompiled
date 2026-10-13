using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B00 RID: 15104
	public class QuestNode_SpawnWorldObjects : QuestNode
	{
		// Token: 0x06014904 RID: 84228 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014905 RID: 84229 RVA: 0x0062223C File Offset: 0x0062043C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.worldObjects.GetValue(slate) == null)
			{
				return;
			}
			string text = QuestGenUtility.HardcodedSignalWithQuestID(this.inSignal.GetValue(slate)) ?? QuestGen.slate.Get<string>("inSignal", null, false);
			foreach (WorldObject worldObject in this.worldObjects.GetValue(slate))
			{
				QuestPart_SpawnWorldObject questPart_SpawnWorldObject = new QuestPart_SpawnWorldObject();
				questPart_SpawnWorldObject.worldObject = worldObject;
				questPart_SpawnWorldObject.inSignal = text;
				questPart_SpawnWorldObject.defsToExcludeFromHyperlinks = this.defsToExcludeFromHyperlinks.GetValue(slate);
				if (this.tile.GetValue(slate) != null)
				{
					worldObject.Tile = (this.tile.GetValue(slate) ?? PlanetTile.Invalid);
				}
				QuestGen.quest.AddPart(questPart_SpawnWorldObject);
			}
		}

		// Token: 0x0400E44B RID: 58443
		[NoTranslate]
		public SlateRef<string> inSignal;

		// Token: 0x0400E44C RID: 58444
		public SlateRef<IEnumerable<WorldObject>> worldObjects;

		// Token: 0x0400E44D RID: 58445
		public SlateRef<PlanetTile?> tile;

		// Token: 0x0400E44E RID: 58446
		public SlateRef<List<ThingDef>> defsToExcludeFromHyperlinks;
	}
}
