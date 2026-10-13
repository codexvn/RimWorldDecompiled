using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BBB RID: 7099
	public class QuestPart_Filter_PlayerWealth : QuestPart_Filter
	{
		// Token: 0x06009E0C RID: 40460 RVA: 0x002DBD4D File Offset: 0x002D9F4D
		protected override bool Pass(SignalArgs args)
		{
			return WealthUtility.PlayerWealth >= this.minPlayerWealth;
		}

		// Token: 0x06009E0D RID: 40461 RVA: 0x002DBD5F File Offset: 0x002D9F5F
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.minPlayerWealth, "minPlayerWealth", 0f, false);
		}

		// Token: 0x04006D6E RID: 28014
		public float minPlayerWealth;
	}
}
