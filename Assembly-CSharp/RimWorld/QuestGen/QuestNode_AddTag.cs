using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020039F5 RID: 14837
	public class QuestNode_AddTag : QuestNode
	{
		// Token: 0x06014583 RID: 83331 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool TestRunInt(Slate slate)
		{
			return true;
		}

		// Token: 0x06014584 RID: 83332 RVA: 0x00614D18 File Offset: 0x00612F18
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			if (this.targets.GetValue(slate) == null)
			{
				return;
			}
			string questTagToAdd = QuestGenUtility.HardcodedTargetQuestTagWithQuestID(this.tag.GetValue(slate));
			foreach (object obj in this.targets.GetValue(slate))
			{
				Thing thing = obj as Thing;
				if (thing != null)
				{
					QuestUtility.AddQuestTag(ref thing.questTags, questTagToAdd);
				}
				else
				{
					WorldObject worldObject = obj as WorldObject;
					if (worldObject != null)
					{
						QuestUtility.AddQuestTag(ref worldObject.questTags, questTagToAdd);
					}
					else
					{
						TransportShip transportShip = obj as TransportShip;
						if (transportShip != null)
						{
							QuestUtility.AddQuestTag(ref transportShip.questTags, questTagToAdd);
						}
					}
				}
			}
		}

		// Token: 0x0400E086 RID: 57478
		[NoTranslate]
		public SlateRef<IEnumerable<object>> targets;

		// Token: 0x0400E087 RID: 57479
		[NoTranslate]
		public SlateRef<string> tag;
	}
}
