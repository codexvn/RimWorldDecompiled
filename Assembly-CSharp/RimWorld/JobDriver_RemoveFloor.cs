using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001161 RID: 4449
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06006CDA RID: 27866 RVA: 0x00222D4F File Offset: 0x00220F4F
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06006CDB RID: 27867 RVA: 0x00222D56 File Offset: 0x00220F56
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06006CDC RID: 27868 RVA: 0x00222D5D File Offset: 0x00220F5D
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06006CDD RID: 27869 RVA: 0x00222D64 File Offset: 0x00220F64
		public JobDriver_RemoveFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06006CDE RID: 27870 RVA: 0x00222D74 File Offset: 0x00220F74
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				TerrainDef terrainDef = base.Map.terrainGrid.TempTerrainAt(c);
				if (terrainDef != null && terrainDef.Removable)
				{
					base.Map.terrainGrid.RemoveTempTerrain(c, false, false);
				}
				else
				{
					base.Map.terrainGrid.RemoveTopLayer(base.TargetLocA, true);
				}
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
