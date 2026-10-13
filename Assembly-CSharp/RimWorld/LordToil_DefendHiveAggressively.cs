using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016F0 RID: 5872
	public class LordToil_DefendHiveAggressively : LordToil_HiveRelated
	{
		// Token: 0x06008A86 RID: 35462 RVA: 0x00294FA8 File Offset: 0x002931A8
		public override void UpdateAllDuties()
		{
			base.FilterOutUnspawnedHives();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Hive hiveFor = base.GetHiveFor(this.lord.ownedPawns[i]);
				if (hiveFor == null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
				}
				else
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendHiveAggressively, hiveFor, this.distToHiveToAttack);
				}
			}
		}

		// Token: 0x04005A3B RID: 23099
		public float distToHiveToAttack = 40f;
	}
}
