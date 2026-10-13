using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A28 RID: 14888
	public class QuestNode_GenerateMonumentMarker : QuestNode
	{
		// Token: 0x0601462E RID: 83502 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x0601462F RID: 83503 RVA: 0x0061761C File Offset: 0x0061581C
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			MonumentMarker monumentMarker = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
			monumentMarker.sketch = this.sketch.GetValue(slate);
			slate.Set<MonumentMarker>(this.storeAs.GetValue(slate), monumentMarker, false);
		}

		// Token: 0x0400E12B RID: 57643
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E12C RID: 57644
		public SlateRef<Sketch> sketch;
	}
}
