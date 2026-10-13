using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x0200394F RID: 14671
	public class SymbolResolver_ArchonexusResearchBuildings : SymbolResolver
	{
		// Token: 0x060142FD RID: 82685 RVA: 0x0060510C File Offset: 0x0060330C
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			ResolveParams resolveParams2 = rp;
			resolveParams2.dessicatedCorpseDensityRange = new FloatRange?(new FloatRange(0.003f, 0.006f));
			BaseGen.symbolStack.Push("desiccatedCorpses", resolveParams2, null);
			ThingDef thingDef = rp.centralBuilding ?? ThingDefOf.MajorArchotechStructureStudiable;
			resolveParams.singleThingDef = thingDef;
			resolveParams.rect = CellRect.CenteredOn(rp.rect.CenterCell, thingDef.Size.x, thingDef.Size.z);
			BaseGen.symbolStack.Push("thing", resolveParams, null);
			int num = rp.minorBuildingCount ?? Rand.Range(4, 8);
			float num2 = 360f / (float)num;
			Vector3 v = IntVec3.North.ToVector3();
			int num3 = Mathf.Max(rp.minorBuildingRadialDistance ?? 10, 10);
			for (int i = 0; i < num; i++)
			{
				float angle = (float)i * num2;
				Vector3 vect = v.RotatedBy(angle) * (float)num3;
				IntVec3 intVec = rp.rect.CenterCell + vect.ToIntVec3();
				CellRect rect = CellRect.CenteredOn(intVec, ThingDefOf.ArchotechTower.size.x, ThingDefOf.ArchotechTower.size.z);
				if (rect.InBounds(BaseGen.globalSettings.map))
				{
					ResolveParams resolveParams3 = rp;
					resolveParams3.singleThingDef = ThingDefOf.ArchotechTower;
					resolveParams3.rect = rect;
					BaseGen.symbolStack.Push("thing", resolveParams3, null);
					BaseGenUtility.DoPathwayBetween(resolveParams.rect.CenterCell, intVec, TerrainDefOf.Sandstone_Smooth, 3);
				}
			}
		}
	}
}
