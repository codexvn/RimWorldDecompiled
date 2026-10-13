using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C94 RID: 7316
	public class QuestPart_RequirementsToAcceptPlayerWealth : QuestPart_RequirementsToAccept
	{
		// Token: 0x0600A302 RID: 41730 RVA: 0x002ED4F3 File Offset: 0x002EB6F3
		public override AcceptanceReport CanAccept()
		{
			if (WealthUtility.PlayerWealth < this.requiredPlayerWealth)
			{
				return new AcceptanceReport("QuestRequiredPlayerWealth".Translate(this.requiredPlayerWealth.ToStringMoney(null)));
			}
			return true;
		}

		// Token: 0x0600A303 RID: 41731 RVA: 0x002ED52E File Offset: 0x002EB72E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.requiredPlayerWealth, "requiredPlayerWealth", 0f, false);
		}

		// Token: 0x040070B3 RID: 28851
		public float requiredPlayerWealth = -1f;
	}
}
