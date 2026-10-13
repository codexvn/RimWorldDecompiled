using System;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020016EC RID: 5868
	public class LordToil_DefendAndExpandHive : LordToil_HiveRelated
	{
		// Token: 0x06008A78 RID: 35448 RVA: 0x00294B98 File Offset: 0x00292D98
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
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.DefendAndExpandHive, hiveFor, this.distToHiveToAttack);
				}
			}
		}

		// Token: 0x04005A35 RID: 23093
		public float distToHiveToAttack = 10f;
	}
}
