using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001706 RID: 5894
	public class LordToil_HuntDownColonists : LordToil
	{
		// Token: 0x06008AE2 RID: 35554 RVA: 0x00296A08 File Offset: 0x00294C08
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].mindState != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.HuntDownColonists);
					CompCanBeDormant compCanBeDormant = this.lord.ownedPawns[i].TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
				}
			}
		}
	}
}
