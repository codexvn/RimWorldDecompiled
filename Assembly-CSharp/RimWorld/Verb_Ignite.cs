using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200377F RID: 14207
	public class Verb_Ignite : Verb
	{
		// Token: 0x06013AEA RID: 80618 RVA: 0x005D8D05 File Offset: 0x005D6F05
		public Verb_Ignite()
		{
			this.verbProps = NativeVerbPropertiesDatabase.VerbWithCategory(VerbCategory.Ignite);
		}

		// Token: 0x06013AEB RID: 80619 RVA: 0x005D8D1C File Offset: 0x005D6F1C
		protected override bool TryCastShot()
		{
			Thing thing = this.currentTarget.Thing;
			Pawn casterPawn = this.CasterPawn;
			FireUtility.TryStartFireIn(thing.OccupiedRect().ClosestCellTo(casterPawn.Position), casterPawn.Map, 0.3f, casterPawn, null);
			if (casterPawn.Spawned)
			{
				casterPawn.Drawer.Notify_MeleeAttackOn(thing);
			}
			return true;
		}
	}
}
