using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001162 RID: 4450
	public class JobDriver_RemoveFoundation : JobDriver_AffectFloor
	{
		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x06006CDF RID: 27871 RVA: 0x00222DEB File Offset: 0x00220FEB
		protected override int BaseWorkAmount
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06006CE0 RID: 27872 RVA: 0x00222DF2 File Offset: 0x00220FF2
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFoundation;
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x06006CE1 RID: 27873 RVA: 0x00222D5D File Offset: 0x00220F5D
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06006CE2 RID: 27874 RVA: 0x00222D64 File Offset: 0x00220F64
		public JobDriver_RemoveFoundation()
		{
			this.clearSnow = true;
		}

		// Token: 0x06006CE3 RID: 27875 RVA: 0x00222DF9 File Offset: 0x00220FF9
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveFoundationAt(c))
			{
				base.Map.terrainGrid.RemoveFoundation(base.TargetLocA, true);
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
