using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BAC RID: 2988
	public class StandardLetter : ChoiceLetter
	{
		// Token: 0x17000BBB RID: 3003
		// (get) Token: 0x06004C5E RID: 19550 RVA: 0x0018416F File Offset: 0x0018236F
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				yield return base.Option_Close;
				if (this.lookTargets.IsValid())
				{
					yield return base.Option_JumpToLocation;
				}
				if (this.quest != null && !this.quest.hidden)
				{
					yield return base.Option_ViewInQuestsTab("ViewRelatedQuest", false);
				}
				if (!this.hyperlinkThingDefs.NullOrEmpty<ThingDef>())
				{
					int num;
					for (int i = 0; i < this.hyperlinkThingDefs.Count; i = num + 1)
					{
						yield return base.Option_ViewInfoCard(i);
						num = i;
					}
				}
				if (!this.hyperlinkHediffDefs.NullOrEmpty<HediffDef>())
				{
					int i = (this.hyperlinkThingDefs == null) ? 0 : this.hyperlinkThingDefs.Count;
					int num;
					for (int j = 0; j < this.hyperlinkHediffDefs.Count; j = num + 1)
					{
						yield return base.Option_ViewInfoCard(i + j);
						num = j;
					}
				}
				yield break;
			}
		}
	}
}
