using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002981 RID: 10625
	public class Building_TurretFoam : Building_TurretGun
	{
		// Token: 0x170023A4 RID: 9124
		// (get) Token: 0x0600E97B RID: 59771 RVA: 0x00002501 File Offset: 0x00000701
		public override bool IsEverThreat
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E97C RID: 59772 RVA: 0x0043EDEC File Offset: 0x0043CFEC
		public override LocalTargetInfo TryFindNewTarget()
		{
			int num = GenRadial.NumCellsInRadius(this.AttackVerb.EffectiveRange);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = base.Position + GenRadial.RadialPattern[i];
				if (GenSight.LineOfSight(base.Position, intVec, base.Map, true, null, 0, 0))
				{
					List<Thing> thingList = intVec.GetThingList(base.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j] is Fire || thingList[j].HasAttachment(ThingDefOf.Fire))
						{
							return thingList[j].Position;
						}
					}
				}
			}
			return LocalTargetInfo.Invalid;
		}
	}
}
