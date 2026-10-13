using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200238F RID: 9103
	public static class PawnNamingUtility
	{
		// Token: 0x0600C544 RID: 50500 RVA: 0x00396660 File Offset: 0x00394860
		public static Dialog_NamePawn NamePawnDialog(this Pawn pawn, string initialFirstNameOverride = null)
		{
			Dictionary<NameFilter, List<string>> suggestedNames = null;
			NameFilter editableNames;
			NameFilter visibleNames;
			if (pawn.babyNamingDeadline >= Find.TickManager.TicksGame || DebugSettings.ShowDevGizmos)
			{
				editableNames = (NameFilter.First | NameFilter.Nick | NameFilter.Last);
				visibleNames = (NameFilter.First | NameFilter.Nick | NameFilter.Last);
				List<string> list = new List<string>();
				Pawn mother = pawn.GetMother();
				if (mother != null)
				{
					list.Add(PawnNamingUtility.GetLastName(mother));
				}
				Pawn father = pawn.GetFather();
				if (father != null)
				{
					list.Add(PawnNamingUtility.GetLastName(father));
				}
				Pawn birthParent = pawn.GetBirthParent();
				if (birthParent != null)
				{
					list.Add(PawnNamingUtility.GetLastName(birthParent));
				}
				list.RemoveDuplicates(null);
				suggestedNames = new Dictionary<NameFilter, List<string>>
				{
					{
						NameFilter.Last,
						list
					}
				};
			}
			else
			{
				visibleNames = (NameFilter.First | NameFilter.Nick | NameFilter.Last | NameFilter.Title);
				editableNames = (NameFilter.Nick | NameFilter.Title);
			}
			return new Dialog_NamePawn(pawn, visibleNames, editableNames, suggestedNames, initialFirstNameOverride, null, null, null);
		}

		// Token: 0x0600C545 RID: 50501 RVA: 0x00396708 File Offset: 0x00394908
		public static string GetLastName(Pawn pawn)
		{
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple != null)
			{
				return nameTriple.Last;
			}
			NameSingle nameSingle = pawn.Name as NameSingle;
			if (nameSingle != null)
			{
				return nameSingle.Name;
			}
			throw new NotImplementedException(pawn.Name.GetType().ToString());
		}
	}
}
