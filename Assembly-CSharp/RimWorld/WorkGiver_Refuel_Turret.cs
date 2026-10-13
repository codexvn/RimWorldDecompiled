using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160A RID: 5642
	public class WorkGiver_Refuel_Turret : WorkGiver_Refuel
	{
		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x0600851C RID: 34076 RVA: 0x0027D83F File Offset: 0x0027BA3F
		public override JobDef JobStandard
		{
			get
			{
				return JobDefOf.RearmTurret;
			}
		}

		// Token: 0x17001469 RID: 5225
		// (get) Token: 0x0600851D RID: 34077 RVA: 0x0027D846 File Offset: 0x0027BA46
		public override JobDef JobAtomic
		{
			get
			{
				return JobDefOf.RearmTurretAtomic;
			}
		}

		// Token: 0x0600851E RID: 34078 RVA: 0x0027D84D File Offset: 0x0027BA4D
		public override bool CanRefuelThing(Thing t)
		{
			return t is Building_Turret;
		}
	}
}
