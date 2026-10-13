using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200243E RID: 9278
	[CaseInsensitiveXMLParsing]
	public class PawnBio
	{
		// Token: 0x17001E50 RID: 7760
		// (get) Token: 0x0600C8C7 RID: 51399 RVA: 0x003AA703 File Offset: 0x003A8903
		public PawnBioType BioType
		{
			get
			{
				if (this.pirateKing)
				{
					return PawnBioType.PirateKing;
				}
				if (this.adulthood != null)
				{
					return PawnBioType.BackstoryInGame;
				}
				return PawnBioType.Undefined;
			}
		}

		// Token: 0x0600C8C8 RID: 51400 RVA: 0x003AA71C File Offset: 0x003A891C
		public void ResolveReferences()
		{
			if (this.adulthood.spawnCategories.Count == 1 && this.adulthood.spawnCategories[0] == "Trader")
			{
				this.adulthood.spawnCategories.Add("Civil");
			}
		}

		// Token: 0x0600C8C9 RID: 51401 RVA: 0x003AA76E File Offset: 0x003A896E
		public override string ToString()
		{
			string str = "PawnBio(";
			NameTriple nameTriple = this.name;
			return str + ((nameTriple != null) ? nameTriple.ToString() : null) + ")";
		}

		// Token: 0x040088A6 RID: 34982
		public GenderPossibility gender;

		// Token: 0x040088A7 RID: 34983
		public NameTriple name;

		// Token: 0x040088A8 RID: 34984
		public BackstoryDef childhood;

		// Token: 0x040088A9 RID: 34985
		public BackstoryDef adulthood;

		// Token: 0x040088AA RID: 34986
		public bool pirateKing;

		// Token: 0x040088AB RID: 34987
		public bool rare;
	}
}
