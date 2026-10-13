using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BB0 RID: 11184
	public class CompAnalyzableUnlockResearch : CompAnalyzable
	{
		// Token: 0x1700272F RID: 10031
		// (get) Token: 0x0600FA0A RID: 64010 RVA: 0x0048B4A8 File Offset: 0x004896A8
		public new CompProperties_CompAnalyzableUnlockResearch Props
		{
			get
			{
				return (CompProperties_CompAnalyzableUnlockResearch)this.props;
			}
		}

		// Token: 0x17002730 RID: 10032
		// (get) Token: 0x0600FA0B RID: 64011 RVA: 0x0048B4B8 File Offset: 0x004896B8
		public List<ResearchProjectDef> ResearchUnlocked
		{
			get
			{
				if (this.researchUnlocked == null)
				{
					this.researchUnlocked = new List<ResearchProjectDef>();
					foreach (ResearchProjectDef researchProjectDef in DefDatabase<ResearchProjectDef>.AllDefs)
					{
						if (!researchProjectDef.requiredAnalyzed.NullOrEmpty<ThingDef>() && researchProjectDef.requiredAnalyzed.Contains(this.parent.def))
						{
							this.researchUnlocked.Add(researchProjectDef);
						}
					}
				}
				return this.researchUnlocked;
			}
		}

		// Token: 0x17002731 RID: 10033
		// (get) Token: 0x0600FA0C RID: 64012 RVA: 0x0048B548 File Offset: 0x00489748
		public override NamedArgument? ExtraNamedArg
		{
			get
			{
				return new NamedArgument?((from r in this.ResearchUnlocked
				select r.label).ToCommaList(true, false).Named("RESEARCH"));
			}
		}

		// Token: 0x17002732 RID: 10034
		// (get) Token: 0x0600FA0D RID: 64013 RVA: 0x0048B595 File Offset: 0x00489795
		public override int AnalysisID
		{
			get
			{
				return this.Props.analysisID;
			}
		}

		// Token: 0x0600FA0E RID: 64014 RVA: 0x0048B5A4 File Offset: 0x004897A4
		public override AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			AcceptanceReport result = base.CanInteract(activateBy, checkOptionalItems);
			if (!result.Accepted)
			{
				return result;
			}
			if (activateBy != null && this.Props.requiresMechanitor && !MechanitorUtility.IsMechanitor(activateBy))
			{
				return "RequiresMechanitor".Translate();
			}
			return true;
		}

		// Token: 0x0400A463 RID: 42083
		private List<ResearchProjectDef> researchUnlocked;
	}
}
