using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E7A RID: 3706
	public class JobDriver_Carried : JobDriver
	{
		// Token: 0x06005D5E RID: 23902 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x001DCFCE File Offset: 0x001DB1CE
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
			yield return JobDriver_Carried.CarryToil();
			yield break;
		}

		// Token: 0x06005D60 RID: 23904 RVA: 0x001DCFDE File Offset: 0x001DB1DE
		private static Toil CarryToil()
		{
			Toil toil = ToilMaker.MakeToil("CarryToil");
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x06005D61 RID: 23905 RVA: 0x001DCFF1 File Offset: 0x001DB1F1
		public override string GetReport()
		{
			return JobDriver_Carried.GetReport(this.pawn, this.pawn.SpawnedParentOrMe);
		}

		// Token: 0x06005D62 RID: 23906 RVA: 0x001DD00C File Offset: 0x001DB20C
		public static string GetReport(Pawn pawn, Thing spawnedParentOrMe)
		{
			if (spawnedParentOrMe == null || spawnedParentOrMe == pawn)
			{
				return "";
			}
			Pawn pawn2 = spawnedParentOrMe as Pawn;
			if (pawn2 != null)
			{
				if (ModsConfig.AnomalyActive && pawn2.CurJobDef == JobDefOf.DevourerDigest)
				{
					return "DigestedBy".Translate(pawn2);
				}
				return "CarriedBy".Translate(pawn2);
			}
			else
			{
				if (spawnedParentOrMe is PawnFlyer)
				{
					return spawnedParentOrMe.LabelCap + ".";
				}
				return "InContainer".Translate(spawnedParentOrMe);
			}
		}
	}
}
