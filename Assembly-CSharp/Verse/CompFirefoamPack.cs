using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020008F6 RID: 2294
	public class CompFirefoamPack : CompAIUsablePack
	{
		// Token: 0x06003B43 RID: 15171 RVA: 0x00139998 File Offset: 0x00137B98
		protected override float ChanceToUse(Pawn wearer)
		{
			if (wearer.GetAttachment(ThingDefOf.Fire) != null)
			{
				return 1f;
			}
			int num = GenRadial.NumCellsInRadius(1.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = wearer.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(wearer.Map))
				{
					List<Thing> thingList = c.GetThingList(wearer.Map);
					int j = 0;
					while (j < thingList.Count)
					{
						if (thingList[j] is Fire || thingList[j].HasAttachment(ThingDefOf.Fire))
						{
							if (i == 0)
							{
								return 1f;
							}
							return 0.2f;
						}
						else
						{
							j++;
						}
					}
				}
			}
			return 0f;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x00139A50 File Offset: 0x00137C50
		protected override void UsePack(Pawn wearer)
		{
			Verb_FirefoamPop.Pop(wearer, this.parent.TryGetComp<CompExplosive>(), this.parent.TryGetComp<CompApparelReloadable>());
		}

		// Token: 0x04002AE5 RID: 10981
		private const float ChanceToUseWithNearbyFire = 0.2f;
	}
}
