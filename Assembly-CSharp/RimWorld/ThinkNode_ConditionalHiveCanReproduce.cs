using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017BC RID: 6076
	public class ThinkNode_ConditionalHiveCanReproduce : ThinkNode_Conditional
	{
		// Token: 0x06008DC8 RID: 36296 RVA: 0x002A1328 File Offset: 0x0029F528
		protected override bool Satisfied(Pawn pawn)
		{
			Hive hive = pawn.mindState.duty.focus.Thing as Hive;
			return hive != null && hive.GetComp<CompSpawnerHives>().canSpawnHives;
		}
	}
}
