using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001761 RID: 5985
	public class TriggerData_FractionColonyDamageTaken : TriggerData
	{
		// Token: 0x06008C7D RID: 35965 RVA: 0x0029C866 File Offset: 0x0029AA66
		public override void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.startColonyDamage, "startColonyDamage", 0f, false);
		}

		// Token: 0x04005B4E RID: 23374
		public float startColonyDamage;
	}
}
