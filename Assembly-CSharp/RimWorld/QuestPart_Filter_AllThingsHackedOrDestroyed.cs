using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BA0 RID: 7072
	public class QuestPart_Filter_AllThingsHackedOrDestroyed : QuestPart_Filter_AllThingsHacked
	{
		// Token: 0x06009DBB RID: 40379 RVA: 0x002DB17C File Offset: 0x002D937C
		protected override bool Pass(SignalArgs args)
		{
			if (this.things.Count == 0)
			{
				return true;
			}
			int num = 0;
			for (int i = 0; i < this.things.Count; i++)
			{
				if (this.things[i].DestroyedOrNull() || this.things[i].IsHacked())
				{
					num++;
				}
			}
			return num >= this.things.Count;
		}
	}
}
