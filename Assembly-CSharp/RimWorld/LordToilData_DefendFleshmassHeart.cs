using System;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016EE RID: 5870
	public class LordToilData_DefendFleshmassHeart : LordToilData
	{
		// Token: 0x06008A7C RID: 35452 RVA: 0x00294CB1 File Offset: 0x00292EB1
		public override void ExposeData()
		{
			Scribe_References.Look<Building_FleshmassHeart>(ref this.heart, "heart", false);
		}

		// Token: 0x04005A37 RID: 23095
		public Building_FleshmassHeart heart;
	}
}
