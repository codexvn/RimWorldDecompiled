using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020020C1 RID: 8385
	public class TileMutatorWorker_AncientRuins_Frozen : TileMutatorWorker_AncientRuins
	{
		// Token: 0x0600B775 RID: 46965 RVA: 0x0034D3EE File Offset: 0x0034B5EE
		public TileMutatorWorker_AncientRuins_Frozen(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x0600B776 RID: 46966 RVA: 0x0034D3F8 File Offset: 0x0034B5F8
		public override void GenerateNonCriticalStructures(Map map)
		{
			base.GenerateNonCriticalStructures(map);
			GenMorphology.GenPatchParms value = GenMorphology.GenPatchParms.For(ThingDefOf.SolidIce, TerrainDefOf.Ice);
			value.edificeThreshold = 0.75f;
			value.roofThreshold = 0.8f;
			GenStep_FrozenRuins.EncaseStructuresInIce(map, TileMutatorWorker_AncientRuins.GenStep.structureSketches, new GenMorphology.GenPatchParms?(value));
		}
	}
}
