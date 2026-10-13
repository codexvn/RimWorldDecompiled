using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002440 RID: 9280
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x0600C8D1 RID: 51409 RVA: 0x003AABC0 File Offset: 0x003A8DC0
		static PawnNameDatabaseSolid()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
			}
		}

		// Token: 0x0600C8D2 RID: 51410 RVA: 0x003AAC38 File Offset: 0x003A8E38
		public static void Clear()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames[key].Clear();
			}
		}

		// Token: 0x0600C8D3 RID: 51411 RVA: 0x003AACA4 File Offset: 0x003A8EA4
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x0600C8D4 RID: 51412 RVA: 0x003AACB7 File Offset: 0x003A8EB7
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x0600C8D5 RID: 51413 RVA: 0x003AACC4 File Offset: 0x003A8EC4
		public static IEnumerable<NameTriple> AllNames()
		{
			foreach (KeyValuePair<GenderPossibility, List<NameTriple>> keyValuePair in PawnNameDatabaseSolid.solidNames)
			{
				foreach (NameTriple nameTriple in keyValuePair.Value)
				{
					yield return nameTriple;
				}
				List<NameTriple>.Enumerator enumerator2 = default(List<NameTriple>.Enumerator);
			}
			Dictionary<GenderPossibility, List<NameTriple>>.Enumerator enumerator = default(Dictionary<GenderPossibility, List<NameTriple>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x040088AE RID: 34990
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x040088AF RID: 34991
		private const float PreferredNameChance = 0.5f;
	}
}
