using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003D81 RID: 15745
	public static class CaravanNameGenerator
	{
		// Token: 0x0601573A RID: 87866 RVA: 0x0066D798 File Offset: 0x0066B998
		public static string GenerateCaravanName(Caravan caravan)
		{
			Pawn pawn;
			if ((pawn = IdeoUtility.FindFirstPawnWithLeaderRole(caravan)) == null && (pawn = BestCaravanPawnUtility.FindBestNegotiator(caravan, null, null)) == null)
			{
				pawn = (BestCaravanPawnUtility.FindBestDiplomat(caravan) ?? caravan.PawnsListForReading.Find((Pawn x) => caravan.IsOwner(x)));
			}
			Pawn pawn2 = pawn;
			TaggedString taggedString = (pawn2 != null) ? "CaravanLeaderCaravanName".Translate(pawn2.LabelShort, pawn2).CapitalizeFirst() : caravan.def.label;
			for (int i = 1; i <= 1000; i++)
			{
				TaggedString taggedString2 = taggedString;
				if (i != 1)
				{
					taggedString2 += " " + i.ToString();
				}
				if (!CaravanNameGenerator.CaravanNameInUse(taggedString2))
				{
					return taggedString2;
				}
			}
			Log.Error("Ran out of caravan names.");
			return caravan.def.label;
		}

		// Token: 0x0601573B RID: 87867 RVA: 0x0066D8A4 File Offset: 0x0066BAA4
		private static bool CaravanNameInUse(string name)
		{
			List<Caravan> caravans = Find.WorldObjects.Caravans;
			for (int i = 0; i < caravans.Count; i++)
			{
				if (caravans[i].Name == name)
				{
					return true;
				}
			}
			return false;
		}
	}
}
