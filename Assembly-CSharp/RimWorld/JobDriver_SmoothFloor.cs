using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001163 RID: 4451
	public class JobDriver_SmoothFloor : JobDriver_AffectFloor
	{
		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x06006CE4 RID: 27876 RVA: 0x00222E31 File Offset: 0x00221031
		protected override int BaseWorkAmount
		{
			get
			{
				return 2800;
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x06006CE5 RID: 27877 RVA: 0x00222E38 File Offset: 0x00221038
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06006CE6 RID: 27878 RVA: 0x00222E3F File Offset: 0x0022103F
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.SmoothingSpeed;
			}
		}

		// Token: 0x06006CE7 RID: 27879 RVA: 0x00222D64 File Offset: 0x00220F64
		public JobDriver_SmoothFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06006CE8 RID: 27880 RVA: 0x00222E48 File Offset: 0x00221048
		protected override void DoEffect(IntVec3 c)
		{
			TerrainDef smoothedTerrain = base.TargetLocA.GetTerrain(base.Map).smoothedTerrain;
			base.Map.terrainGrid.SetTerrain(base.TargetLocA, smoothedTerrain);
			FilthMaker.RemoveAllFilth(base.TargetLocA, base.Map);
		}
	}
}
