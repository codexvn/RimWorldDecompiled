using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001171 RID: 4465
	public class JobDriver_FillIn : JobDriver_RemoveBuilding
	{
		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06006D3C RID: 27964 RVA: 0x00223CAF File Offset: 0x00221EAF
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.FillIn;
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06006D3D RID: 27965 RVA: 0x00223CB6 File Offset: 0x00221EB6
		protected override float TotalNeededWork
		{
			get
			{
				return base.Target.def.building.uninstallWork;
			}
		}

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06006D3E RID: 27966 RVA: 0x00223CCD File Offset: 0x00221ECD
		protected override EffecterDef WorkEffecter
		{
			get
			{
				if (!(base.Target is PitBurrow))
				{
					return EffecterDefOf.FillingInCrater;
				}
				return EffecterDefOf.FillingInPitGate;
			}
		}

		// Token: 0x06006D3F RID: 27967 RVA: 0x00223CE8 File Offset: 0x00221EE8
		protected override void FinishedRemoving()
		{
			Crater crater = base.Target as Crater;
			if (crater != null)
			{
				crater.FillIn();
			}
		}
	}
}
