using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020008F7 RID: 2295
	public class CompToxPack : CompAIUsablePack
	{
		// Token: 0x06003B46 RID: 15174 RVA: 0x00139A78 File Offset: 0x00137C78
		protected override float ChanceToUse(Pawn wearer)
		{
			if (!ModsConfig.BiotechActive)
			{
				return 0f;
			}
			int num = GenRadial.NumCellsInRadius(1.9f);
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = wearer.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(wearer.Map))
				{
					List<Thing> thingList = c.GetThingList(wearer.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn = thingList[j] as Pawn;
						if (pawn != null && pawn != wearer && pawn.HostileTo(wearer) && GasUtility.IsAffectedByExposure(pawn) && !pawn.IsPsychologicallyInvisible())
						{
							num2 += pawn.BodySize;
							if (num2 >= 1f)
							{
								break;
							}
						}
					}
				}
			}
			return num2;
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x00139B46 File Offset: 0x00137D46
		protected override void UsePack(Pawn wearer)
		{
			Verb_DeployToxPack.TryDeploy(this.parent.TryGetComp<CompApparelReloadable>(), this.parent.TryGetComp<CompReleaseGas>());
		}
	}
}
