using System;
using RimWorld.Planet;

namespace RimWorld.QuestGen
{
	// Token: 0x02003B04 RID: 15108
	public class QuestNode_WorldObjectTimeout : QuestNode_Delay
	{
		// Token: 0x06014910 RID: 84240 RVA: 0x00622598 File Offset: 0x00620798
		protected override QuestPart_Delay MakeDelayQuestPart()
		{
			QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = new QuestPart_WorldObjectTimeout();
			questPart_WorldObjectTimeout.worldObject = this.worldObject.GetValue(QuestGen.slate);
			this.destroyOnCleanup.TryGetValue(QuestGen.slate, out questPart_WorldObjectTimeout.destroyOnCleanup);
			return questPart_WorldObjectTimeout;
		}

		// Token: 0x0400E45C RID: 58460
		public SlateRef<bool> destroyOnCleanup;

		// Token: 0x0400E45D RID: 58461
		public SlateRef<WorldObject> worldObject;
	}
}
