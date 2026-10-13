using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001187 RID: 4487
	public class JobDriver_Uninstall : JobDriver_RemoveBuilding
	{
		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x06006DC6 RID: 28102 RVA: 0x0022523F File Offset: 0x0022343F
		protected override DesignationDef Designation
		{
			get
			{
				return DesignationDefOf.Uninstall;
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x06006DC7 RID: 28103 RVA: 0x00225248 File Offset: 0x00223448
		protected override float TotalNeededWork
		{
			get
			{
				return base.TargetA.Thing.def.building.uninstallWork;
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x06006DC8 RID: 28104 RVA: 0x00002C42 File Offset: 0x00000E42
		protected override EffecterDef WorkEffecter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06006DC9 RID: 28105 RVA: 0x00225272 File Offset: 0x00223472
		protected override void FinishedRemoving()
		{
			base.Building.Uninstall();
			this.pawn.records.Increment(RecordDefOf.ThingsUninstalled);
		}
	}
}
