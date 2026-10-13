using System;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020038E9 RID: 14569
	public class SymbolResolver_AncientComplex : SymbolResolver_AncientComplex_Base
	{
		// Token: 0x1700332B RID: 13099
		// (get) Token: 0x060141AC RID: 82348 RVA: 0x005FC448 File Offset: 0x005FA648
		protected override LayoutDef DefaultLayoutDef
		{
			get
			{
				return LayoutDefOf.AncientComplex;
			}
		}

		// Token: 0x060141AD RID: 82349 RVA: 0x005FC450 File Offset: 0x005FA650
		public override void Resolve(ResolveParams rp)
		{
			ResolveParams resolveParams = rp;
			resolveParams.filthDensity = new FloatRange?(SymbolResolver_AncientComplex.FilthDensity_DriedBlood);
			resolveParams.filthDef = ThingDefOf.Filth_DriedBlood;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			resolveParams.ignoreDoorways = new bool?(true);
			resolveParams.filthDensity = new FloatRange?(SymbolResolver_AncientComplex.FilthDensity_MoldyUniform);
			resolveParams.filthDef = ThingDefOf.Filth_MoldyUniform;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			resolveParams.filthDensity = new FloatRange?(SymbolResolver_AncientComplex.FilthDensity_ScatteredDocuments);
			resolveParams.filthDef = ThingDefOf.Filth_ScatteredDocuments;
			BaseGen.symbolStack.Push("filth", resolveParams, null);
			ResolveParams resolveParams2 = rp;
			resolveParams2.desiccatedCorpsePawnKind = PawnKindDefOf.AncientSoldier;
			resolveParams2.desiccatedCorpseRandomAgeRange = new IntRange?(SymbolResolver_AncientComplex.CorpseRandomAgeRange);
			BaseGen.symbolStack.Push("desiccatedCorpses", resolveParams2, null);
			ResolveParams resolveParams3 = rp;
			resolveParams3.floorDef = TerrainDefOf.PackedDirt;
			BaseGen.symbolStack.Push("outdoorsPath", resolveParams3, null);
			BaseGen.symbolStack.Push("ancientComplexDefences", rp, null);
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp, null);
			base.ResolveComplex(rp);
			ResolveParams resolveParams4 = rp;
			resolveParams4.rect = rp.rect.ExpandedBy(5);
			resolveParams4.floorDef = TerrainDefOf.Gravel;
			resolveParams4.chanceToSkipFloor = new float?(0.05f);
			resolveParams4.floorOnlyIfTerrainSupports = new bool?(true);
			BaseGen.symbolStack.Push("floor", resolveParams4, null);
			foreach (IntVec3 c in resolveParams4.rect)
			{
				Building edifice = c.GetEdifice(BaseGen.globalSettings.map);
				if (edifice != null && edifice.def.destroyable && edifice.def.IsBuildingArtificial)
				{
					edifice.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0400DE86 RID: 56966
		private static readonly FloatRange FilthDensity_DriedBlood = new FloatRange(0.01f, 0.025f);

		// Token: 0x0400DE87 RID: 56967
		private static readonly FloatRange FilthDensity_MoldyUniform = new FloatRange(0.005f, 0.01f);

		// Token: 0x0400DE88 RID: 56968
		private static readonly FloatRange FilthDensity_ScatteredDocuments = new FloatRange(0.005f, 0.015f);

		// Token: 0x0400DE89 RID: 56969
		private static readonly IntRange CorpseRandomAgeRange = new IntRange(1080000000, 1260000000);
	}
}
