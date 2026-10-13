using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003949 RID: 14665
	public class SymbolResolver_AncientShrinesGroup : SymbolResolver
	{
		// Token: 0x060142EE RID: 82670 RVA: 0x00604474 File Offset: 0x00602674
		public override void Resolve(ResolveParams rp)
		{
			Map map = BaseGen.globalSettings.map;
			int num = (rp.rect.Width + Mathf.Max(-1, 0)) / (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x + -1);
			int num2 = (rp.rect.Height + Mathf.Max(-1, 0)) / (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z + -1);
			IntVec3 min = rp.rect.Min;
			PodContentsType? podContentsType = rp.podContentsType;
			if (podContentsType == null)
			{
				float value = Rand.Value;
				if (value < 0.5f)
				{
					podContentsType = null;
				}
				else if (value < 0.7f)
				{
					podContentsType = new PodContentsType?(PodContentsType.Slave);
				}
				else
				{
					podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
				}
			}
			int value2 = rp.ancientCryptosleepCasketGroupID ?? Find.UniqueIDsManager.GetNextAncientCryptosleepCasketGroupID();
			int num3 = 0;
			for (int i = 0; i < num2; i++)
			{
				for (int j = 0; j < num; j++)
				{
					if (!Rand.Chance(0.25f))
					{
						if (num3 >= 6)
						{
							break;
						}
						CellRect cellRect = new CellRect(min.x + j * (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x + -1), min.z + i * (SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z + -1), SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.x, SymbolResolver_AncientShrinesGroup.StandardAncientShrineSize.z);
						if (cellRect.FullyContainedWithin(rp.rect))
						{
							IntVec3 center = new IntVec3(cellRect.minX + cellRect.Width / 2 - 1, 0, cellRect.minZ + cellRect.Height / 2);
							if (ThingUtility.InteractionCellWhenAt(ThingDefOf.AncientCryptosleepCasket, center, Rot4.East, map).Standable(map))
							{
								ResolveParams resolveParams = rp;
								resolveParams.rect = cellRect;
								resolveParams.ancientCryptosleepCasketGroupID = new int?(value2);
								resolveParams.podContentsType = podContentsType;
								BaseGen.symbolStack.Push("ancientShrine", resolveParams, null);
								num3++;
							}
						}
					}
				}
			}
		}

		// Token: 0x0400DF1C RID: 57116
		public static readonly IntVec2 StandardAncientShrineSize = new IntVec2(4, 3);

		// Token: 0x0400DF1D RID: 57117
		private const int MaxNumCaskets = 6;

		// Token: 0x0400DF1E RID: 57118
		private const float SkipShrineChance = 0.25f;

		// Token: 0x0400DF1F RID: 57119
		public const int MarginCells = -1;
	}
}
