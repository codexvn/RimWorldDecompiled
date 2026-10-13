using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000455 RID: 1109
	public static class NameUseChecker
	{
		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06001CDE RID: 7390 RVA: 0x00091663 File Offset: 0x0008F863
		public static IEnumerable<Name> AllPawnsNamesEverUsed
		{
			get
			{
				foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					if (pawn.Name != null)
					{
						yield return pawn.Name;
					}
				}
				List<Pawn>.Enumerator enumerator = default(List<Pawn>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x0009166C File Offset: 0x0008F86C
		public static bool NameWordIsUsed(string singleName)
		{
			foreach (Name name in NameUseChecker.AllPawnsNamesEverUsed)
			{
				NameTriple nameTriple = name as NameTriple;
				if (nameTriple != null && (singleName == nameTriple.First || singleName == nameTriple.Nick || singleName == nameTriple.Last))
				{
					return true;
				}
				NameSingle nameSingle = name as NameSingle;
				if (nameSingle != null && nameSingle.Name == singleName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x0009170C File Offset: 0x0008F90C
		public static bool NameSingleIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				NameSingle nameSingle = pawn.Name as NameSingle;
				if (nameSingle != null && nameSingle.Name == candidate)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x0009177C File Offset: 0x0008F97C
		public static bool XenotypeNameIsUsed(string candidate)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn.genes != null && pawn.genes.UniqueXenotype)
				{
					string xenotypeName = pawn.genes.xenotypeName;
					if (candidate == xenotypeName)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
