using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020039A1 RID: 14753
	public class SymbolResolver_Interior_ThroneRoom : SymbolResolver
	{
		// Token: 0x060143F4 RID: 82932 RVA: 0x0060B044 File Offset: 0x00609244
		public override void Resolve(ResolveParams rp)
		{
			Rot4 value;
			IntVec3 throneCell = this.GetThroneCell(rp.rect, out value);
			Pair<IntVec3, Rot4> pair;
			if (this.GetPossibleDrapeCells(throneCell, rp.rect).TryRandomElement(out pair))
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Drape;
				resolveParams.rect = CellRect.SingleCell(pair.First);
				resolveParams.thingRot = new Rot4?(pair.Second);
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
			foreach (IntVec3 intVec in rp.rect.Corners)
			{
				if (!(intVec == throneCell) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(intVec, BaseGen.globalSettings.map))
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.singleThingDef = ThingDefOf.Brazier;
					resolveParams2.rect = CellRect.SingleCell(intVec);
					resolveParams2.postThingSpawn = delegate(Thing x)
					{
						x.TryGetComp<CompRefuelable>().Refuel(9999f);
					};
					BaseGen.symbolStack.Push("thing", resolveParams2, null);
				}
			}
			ResolveParams resolveParams3 = rp;
			resolveParams3.singleThingDef = ThingDefOf.Throne;
			resolveParams3.thingRot = new Rot4?(value);
			resolveParams3.rect = CellRect.SingleCell(throneCell);
			BaseGen.symbolStack.Push("thing", resolveParams3, null);
		}

		// Token: 0x060143F5 RID: 82933 RVA: 0x0060B1B0 File Offset: 0x006093B0
		private IntVec3 GetThroneCell(CellRect rect, out Rot4 dir)
		{
			SymbolResolver_Interior_ThroneRoom.tmpCells.Clear();
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.CenterCell.x, 0, rect.maxZ), Rot4.South));
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.CenterCell.x, 0, rect.minZ), Rot4.North));
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.minX, 0, rect.CenterCell.z), Rot4.East));
			SymbolResolver_Interior_ThroneRoom.tmpCells.Add(new Pair<IntVec3, Rot4>(new IntVec3(rect.maxX, 0, rect.CenterCell.z), Rot4.West));
			Pair<IntVec3, Rot4> pair;
			if (!(from x in SymbolResolver_Interior_ThroneRoom.tmpCells
			where !BaseGenUtility.AnyDoorAdjacentCardinalTo(x.First, BaseGen.globalSettings.map)
			select x).TryRandomElement(out pair))
			{
				SymbolResolver_Interior_ThroneRoom.tmpCells.TryRandomElement(out pair);
			}
			dir = pair.Second;
			return pair.First;
		}

		// Token: 0x060143F6 RID: 82934 RVA: 0x0060B2CA File Offset: 0x006094CA
		private IEnumerable<Pair<IntVec3, Rot4>> GetPossibleDrapeCells(IntVec3 throneCell, CellRect rect)
		{
			int num;
			for (int d = 0; d < 4; d = num + 1)
			{
				foreach (IntVec3 intVec in rect.GetEdgeCells(new Rot4(d)))
				{
					bool flag = true;
					foreach (IntVec3 intVec2 in GenAdj.OccupiedRect(intVec, new Rot4(d), ThingDefOf.Drape.size))
					{
						if (intVec2 == throneCell || rect.IsCorner(intVec2) || BaseGenUtility.AnyDoorAdjacentCardinalTo(intVec2, BaseGen.globalSettings.map))
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						yield return new Pair<IntVec3, Rot4>(intVec, new Rot4(d));
					}
				}
				IEnumerator<IntVec3> enumerator = null;
				num = d;
			}
			yield break;
			yield break;
		}

		// Token: 0x0400DFB7 RID: 57271
		private static List<Pair<IntVec3, Rot4>> tmpCells = new List<Pair<IntVec3, Rot4>>();
	}
}
