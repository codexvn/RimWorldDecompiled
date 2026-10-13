using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038E8 RID: 14568
	public abstract class SymbolResolver_AncientComplex_Base : SymbolResolver
	{
		// Token: 0x1700332A RID: 13098
		// (get) Token: 0x060141A9 RID: 82345
		protected abstract LayoutDef DefaultLayoutDef { get; }

		// Token: 0x060141AA RID: 82346 RVA: 0x005FC2E0 File Offset: 0x005FA4E0
		public void ResolveComplex(ResolveParams rp)
		{
			if (rp.ancientLayoutStructureSketch == null)
			{
				StructureGenParams parms = new StructureGenParams
				{
					size = new IntVec2(rp.rect.Width, rp.rect.Height)
				};
				rp.ancientLayoutStructureSketch = this.DefaultLayoutDef.Worker.GenerateStructureSketch(parms);
			}
			ResolveParams resolveParams = rp;
			resolveParams.ancientLayoutStructureSketch = rp.ancientLayoutStructureSketch;
			BaseGen.symbolStack.Push("ancientComplexSketch", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.floorDef = TerrainDefOf.AncientConcrete;
			resolveParams2.allowBridgeOnAnyImpassableTerrain = new bool?(true);
			resolveParams2.floorOnlyIfTerrainSupports = new bool?(false);
			foreach (LayoutRoom layoutRoom in rp.ancientLayoutStructureSketch.structureLayout.Rooms)
			{
				foreach (CellRect cellRect in layoutRoom.rects)
				{
					resolveParams2.rect = cellRect.MovedBy(rp.rect.Min);
					BaseGen.symbolStack.Push("floor", resolveParams2, null);
					BaseGen.symbolStack.Push("clear", resolveParams2, null);
				}
			}
		}
	}
}
