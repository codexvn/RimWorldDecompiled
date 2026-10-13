using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003997 RID: 14743
	public class SymbolResolver_Interior_DiningRoom : SymbolResolver
	{
		// Token: 0x060143D9 RID: 82905 RVA: 0x0060AA88 File Offset: 0x00608C88
		public override void Resolve(ResolveParams rp)
		{
			BaseGen.symbolStack.Push("indoorLighting", rp, null);
			BaseGen.symbolStack.Push("randomlyPlaceMealsOnTables", rp, null);
			BaseGen.symbolStack.Push("placeChairsNearTables", rp, null);
			int num = Mathf.Max(GenMath.RoundRandom((float)rp.rect.Area / 20f), 1);
			for (int i = 0; i < num; i++)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singleThingDef = ThingDefOf.Table2x2c;
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
		}
	}
}
