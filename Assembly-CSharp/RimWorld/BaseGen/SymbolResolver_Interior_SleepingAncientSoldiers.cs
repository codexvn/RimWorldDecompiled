using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003999 RID: 14745
	public class SymbolResolver_Interior_SleepingAncientSoldiers : SymbolResolver
	{
		// Token: 0x060143DD RID: 82909 RVA: 0x0060ABA1 File Offset: 0x00608DA1
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && rp.threatPoints != null && rp.threatPoints.Value >= PawnKindDefOf.AncientSoldier.combatPower;
		}

		// Token: 0x060143DE RID: 82910 RVA: 0x0060ABD8 File Offset: 0x00608DD8
		public override void Resolve(ResolveParams rp)
		{
			int num = Mathf.RoundToInt(rp.threatPoints.Value / PawnKindDefOf.AncientSoldier.combatPower);
			SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Clear();
			ThingDef ancientCryptosleepCasket = ThingDefOf.AncientCryptosleepCasket;
			CellRect cellRect = new CellRect(0, 0, ancientCryptosleepCasket.size.x, ancientCryptosleepCasket.size.z).ExpandedBy(1);
			for (int i = 0; i < num; i++)
			{
				CellRect item;
				if (rp.rect.TryFindRandomInnerRect(new IntVec2(cellRect.Width, cellRect.Height), out item, (CellRect rect) => !SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Any((CellRect r) => r.Overlaps(rect))))
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = item.ExpandedBy(-1);
					resolveParams.thingRot = new Rot4?(ancientCryptosleepCasket.defaultPlacingRot);
					resolveParams.podContentsType = new PodContentsType?(PodContentsType.AncientHostile);
					resolveParams.faction = Faction.OfAncientsHostile;
					BaseGen.symbolStack.Push("ancientCryptosleepCasket", resolveParams, null);
					SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Add(item);
				}
			}
			SymbolResolver_Interior_SleepingAncientSoldiers.tmpCellRects.Clear();
		}

		// Token: 0x0400DFAE RID: 57262
		private static List<CellRect> tmpCellRects = new List<CellRect>();
	}
}
