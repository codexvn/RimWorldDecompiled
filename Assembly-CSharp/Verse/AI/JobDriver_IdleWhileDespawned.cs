using System;
using System.Collections.Generic;

namespace Verse.AI
{
	// Token: 0x02000E85 RID: 3717
	public class JobDriver_IdleWhileDespawned : JobDriver
	{
		// Token: 0x06005DB3 RID: 23987 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005DB4 RID: 23988 RVA: 0x001DE233 File Offset: 0x001DC433
		protected override IEnumerable<Toil> MakeNewToils()
		{
			base.AddEndCondition(delegate
			{
				if (!this.pawn.Spawned)
				{
					return JobCondition.Ongoing;
				}
				return JobCondition.Succeeded;
			});
			yield return JobDriver_IdleWhileDespawned.IdleWhileDespawned();
			yield break;
		}

		// Token: 0x06005DB5 RID: 23989 RVA: 0x001DE243 File Offset: 0x001DC443
		private static Toil IdleWhileDespawned()
		{
			Toil toil = ToilMaker.MakeToil("IdleWhileDespawned");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06005DB6 RID: 23990 RVA: 0x001DE256 File Offset: 0x001DC456
		public override string GetReport()
		{
			Log.ErrorOnce(this.pawn.ToStringSafe<Pawn>() + " reported job string for IdleWhileDespawned.  An inspectible despawned pawn should have a different job.", 1560125928);
			return base.GetReport();
		}
	}
}
