using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CE5 RID: 3301
	public static class GenClamor
	{
		// Token: 0x06005462 RID: 21602 RVA: 0x001B27B0 File Offset: 0x001B09B0
		public static void DoClamor(Thing source, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, source.Position, radius, type);
		}

		// Token: 0x06005463 RID: 21603 RVA: 0x001B27C0 File Offset: 0x001B09C0
		public static void DoClamor(Thing source, IntVec3 position, float radius, ClamorDef type)
		{
			GenClamor.DoClamor(source, position, radius, delegate(Thing _, Pawn hearer)
			{
				hearer.HearClamor(source, type);
			});
		}

		// Token: 0x06005464 RID: 21604 RVA: 0x001B27FA File Offset: 0x001B09FA
		public static void DoClamor(Thing source, float radius, GenClamor.ClamorEffect clamorEffect)
		{
			GenClamor.DoClamor(source, source.Position, radius, clamorEffect);
		}

		// Token: 0x06005465 RID: 21605 RVA: 0x001B280C File Offset: 0x001B0A0C
		public static void DoClamor(Thing source, IntVec3 position, float radius, GenClamor.ClamorEffect clamorEffect)
		{
			if (source.MapHeld == null)
			{
				return;
			}
			Region region = position.GetRegion(source.MapHeld, RegionType.Set_Passable);
			if (region == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(region, (Region from, Region r) => r.door == null || r.door.Open, delegate(Region r)
			{
				List<Thing> list = r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i] as Pawn;
					float num = Mathf.Clamp01(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Hearing));
					if (num > 0f && pawn.Position.InHorDistOf(position, radius * num))
					{
						clamorEffect(source, pawn);
					}
				}
				return false;
			}, 15, RegionType.Set_Passable);
		}

		// Token: 0x02000CE6 RID: 3302
		// (Invoke) Token: 0x06005467 RID: 21607
		public delegate void ClamorEffect(Thing source, Pawn hearer);
	}
}
