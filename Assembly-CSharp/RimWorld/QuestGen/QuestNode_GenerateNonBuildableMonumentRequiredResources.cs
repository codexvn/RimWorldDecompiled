using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A29 RID: 14889
	public class QuestNode_GenerateNonBuildableMonumentRequiredResources : QuestNode
	{
		// Token: 0x06014631 RID: 83505 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014632 RID: 83506 RVA: 0x00617668 File Offset: 0x00615868
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker value = this.monumentMarker.GetValue(slate);
			for (int i = 0; i < value.sketch.Things.Count; i++)
			{
				ThingDef def = value.sketch.Things[i].def;
				ThingDef stuff = value.sketch.Things[i].stuff;
				if (def.category == ThingCategory.Building && !def.BuildableByPlayer)
				{
					MinifiedThing obj = ThingMaker.MakeThing(def, stuff).MakeMinified(DestroyMode.Vanish);
					QuestGenUtility.AddToOrMakeList(QuestGen.slate, this.addToList.GetValue(slate), obj);
				}
			}
		}

		// Token: 0x0400E12D RID: 57645
		[NoTranslate]
		public SlateRef<string> addToList;

		// Token: 0x0400E12E RID: 57646
		public SlateRef<MonumentMarker> monumentMarker;
	}
}
