using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200341F RID: 13343
	public static class ResearchPrerequisitesUtility
	{
		// Token: 0x06012C6F RID: 76911 RVA: 0x0059377C File Offset: 0x0059197C
		private static Dictionary<Def, List<ResearchProjectDef>> ComputeResearchPrerequisites()
		{
			Dictionary<Def, List<ResearchProjectDef>> dictionary = new Dictionary<Def, List<ResearchProjectDef>>();
			foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
			{
				foreach (Def key in researchProjectDef.UnlockedDefs)
				{
					List<ResearchProjectDef> list;
					if (!dictionary.TryGetValue(key, out list))
					{
						list = new List<ResearchProjectDef>();
						dictionary.Add(key, list);
					}
					list.Add(researchProjectDef);
				}
			}
			return dictionary;
		}

		// Token: 0x06012C70 RID: 76912 RVA: 0x0059382C File Offset: 0x00591A2C
		public static List<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>> UnlockedDefsGroupedByPrerequisites(ResearchProjectDef rd)
		{
			Dictionary<Def, List<ResearchProjectDef>> dictionary = ResearchPrerequisitesUtility.ComputeResearchPrerequisites();
			List<Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>> list = new List<Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>>();
			foreach (Def def in rd.UnlockedDefs)
			{
				list.Add(new Pair<Def, ResearchPrerequisitesUtility.UnlockedHeader>(def, new ResearchPrerequisitesUtility.UnlockedHeader(dictionary[def].Except(rd).ToList<ResearchProjectDef>())));
			}
			return (from pair in list
			orderby pair.Second.unlockedBy.Count
			group pair.First by pair.Second).Select(new Func<IGrouping<ResearchPrerequisitesUtility.UnlockedHeader, Def>, Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>(GenCollection.ConvertIGroupingToPair<ResearchPrerequisitesUtility.UnlockedHeader, Def>)).ToList<Pair<ResearchPrerequisitesUtility.UnlockedHeader, List<Def>>>();
		}

		// Token: 0x02003420 RID: 13344
		public class UnlockedHeader : IEquatable<ResearchPrerequisitesUtility.UnlockedHeader>
		{
			// Token: 0x06012C71 RID: 76913 RVA: 0x00593928 File Offset: 0x00591B28
			public UnlockedHeader(List<ResearchProjectDef> unlockedBy)
			{
				this.unlockedBy = unlockedBy;
			}

			// Token: 0x06012C72 RID: 76914 RVA: 0x00593937 File Offset: 0x00591B37
			public bool Equals(ResearchPrerequisitesUtility.UnlockedHeader other)
			{
				return other != null && (this == other || this.unlockedBy.SequenceEqual(other.unlockedBy));
			}

			// Token: 0x06012C73 RID: 76915 RVA: 0x00593955 File Offset: 0x00591B55
			public override bool Equals(object obj)
			{
				return obj != null && (this == obj || (!(obj.GetType() != base.GetType()) && this.Equals((ResearchPrerequisitesUtility.UnlockedHeader)obj)));
			}

			// Token: 0x06012C74 RID: 76916 RVA: 0x00593983 File Offset: 0x00591B83
			public override int GetHashCode()
			{
				if (!this.unlockedBy.Any<ResearchProjectDef>())
				{
					return 23;
				}
				return this.unlockedBy.First<ResearchProjectDef>().GetHashCode();
			}

			// Token: 0x0400C32D RID: 49965
			public List<ResearchProjectDef> unlockedBy;
		}
	}
}
