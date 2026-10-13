using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BE0 RID: 15328
	public class QuestNode_Filter_AnyColonistAlive : QuestNode_Filter
	{
		// Token: 0x06014CE7 RID: 85223 RVA: 0x0063BAC4 File Offset: 0x00639CC4
		protected override QuestPart_Filter MakeFilterQuestPart()
		{
			Slate slate = QuestGen.slate;
			Map map = this.map.GetValue(slate) ?? slate.Get<Map>("map", null, false);
			return new QuestPart_Filter_AnyColonistAlive
			{
				mapParent = map.Parent
			};
		}

		// Token: 0x0400E7A3 RID: 59299
		public SlateRef<Map> map;
	}
}
