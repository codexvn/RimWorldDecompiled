using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AE4 RID: 6884
	public class SitePartDef : Def
	{
		// Token: 0x1700179A RID: 6042
		// (get) Token: 0x060099D8 RID: 39384 RVA: 0x002CC5E3 File Offset: 0x002CA7E3
		public SitePartWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (SitePartWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x1700179B RID: 6043
		// (get) Token: 0x060099D9 RID: 39385 RVA: 0x002CC618 File Offset: 0x002CA818
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (!this.expandingIconTexture.NullOrEmpty())
					{
						this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
					}
					else
					{
						this.expandingIconTextureInt = BaseContent.BadTex;
					}
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x1700179C RID: 6044
		// (get) Token: 0x060099DA RID: 39386 RVA: 0x002CC668 File Offset: 0x002CA868
		public List<GenStepDef> ExtraGenSteps
		{
			get
			{
				if (this.extraGenSteps == null)
				{
					this.extraGenSteps = new List<GenStepDef>();
					List<GenStepDef> allDefsListForReading = DefDatabase<GenStepDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (allDefsListForReading[i].linkWithSite == this)
						{
							this.extraGenSteps.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.extraGenSteps;
			}
		}

		// Token: 0x060099DB RID: 39387 RVA: 0x002CC6C8 File Offset: 0x002CA8C8
		public SitePartDef()
		{
			this.workerClass = typeof(SitePartWorker);
		}

		// Token: 0x060099DC RID: 39388 RVA: 0x002CC740 File Offset: 0x002CA940
		public bool FactionCanOwn(Faction faction)
		{
			return (!this.requiresFaction || faction != null) && (this.minFactionTechLevel == TechLevel.Undefined || (faction != null && faction.def.techLevel >= this.minFactionTechLevel)) && (faction == null || (!faction.IsPlayer && !faction.defeated && !faction.Hidden)) && this.Worker.FactionCanOwn(faction);
		}

		// Token: 0x060099DD RID: 39389 RVA: 0x002CC7A8 File Offset: 0x002CA9A8
		public bool CompatibleWith(SitePartDef part)
		{
			for (int i = 0; i < part.excludesTags.Count; i++)
			{
				if (this.tags.Contains(part.excludesTags[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < this.excludesTags.Count; j++)
			{
				if (part.tags.Contains(this.excludesTags[j]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0400697D RID: 27005
		public ThingDef conditionCauserDef;

		// Token: 0x0400697E RID: 27006
		public float activeThreatDisturbanceFactor = 1f;

		// Token: 0x0400697F RID: 27007
		public bool defaultHidden;

		// Token: 0x04006980 RID: 27008
		public Type workerClass = typeof(SitePartWorker);

		// Token: 0x04006981 RID: 27009
		[NoTranslate]
		public string siteTexture;

		// Token: 0x04006982 RID: 27010
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04006983 RID: 27011
		public List<TileMutatorDef> forceMutators = new List<TileMutatorDef>();

		// Token: 0x04006984 RID: 27012
		public bool applyFactionColorToSiteTexture;

		// Token: 0x04006985 RID: 27013
		public bool showFactionInInspectString;

		// Token: 0x04006986 RID: 27014
		public bool requiresFaction;

		// Token: 0x04006987 RID: 27015
		public bool disallowsAutomaticDetectionTimerStart;

		// Token: 0x04006988 RID: 27016
		public TechLevel minFactionTechLevel;

		// Token: 0x04006989 RID: 27017
		[MustTranslate]
		public string approachOrderString;

		// Token: 0x0400698A RID: 27018
		[MustTranslate]
		public string approachingReportString;

		// Token: 0x0400698B RID: 27019
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x0400698C RID: 27020
		[NoTranslate]
		public List<string> excludesTags = new List<string>();

		// Token: 0x0400698D RID: 27021
		[MustTranslate]
		public string arrivedLetter;

		// Token: 0x0400698E RID: 27022
		[MustTranslate]
		public string arrivedLetterLabelPart;

		// Token: 0x0400698F RID: 27023
		public List<HediffDef> arrivedLetterHediffHyperlinks;

		// Token: 0x04006990 RID: 27024
		public LetterDef arrivedLetterDef;

		// Token: 0x04006991 RID: 27025
		public bool wantsThreatPoints;

		// Token: 0x04006992 RID: 27026
		public float minThreatPoints;

		// Token: 0x04006993 RID: 27027
		public bool increasesPopulation;

		// Token: 0x04006994 RID: 27028
		public bool badEvenIfNoMap;

		// Token: 0x04006995 RID: 27029
		public float forceExitAndRemoveMapCountdownDurationDays = 4f;

		// Token: 0x04006996 RID: 27030
		public bool handlesWorldObjectTimeoutInspectString;

		// Token: 0x04006997 RID: 27031
		public string mainPartAllThreatsLabel;

		// Token: 0x04006998 RID: 27032
		public IntVec3? minMapSize;

		// Token: 0x04006999 RID: 27033
		public float selectionWeight;

		// Token: 0x0400699A RID: 27034
		public bool considerEnteringAsAttack = true;

		// Token: 0x0400699B RID: 27035
		public bool gravShipsCanLandOn;

		// Token: 0x0400699C RID: 27036
		public bool copyQuestName;

		// Token: 0x0400699D RID: 27037
		public bool leaveAbandonedSettlement;

		// Token: 0x0400699E RID: 27038
		public bool displayOnInspectPane = true;

		// Token: 0x0400699F RID: 27039
		public List<SitePartDef.WorkSiteLootThing> lootTable;

		// Token: 0x040069A0 RID: 27040
		[Unsaved(false)]
		private SitePartWorker workerInt;

		// Token: 0x040069A1 RID: 27041
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x040069A2 RID: 27042
		[Unsaved(false)]
		private List<GenStepDef> extraGenSteps;

		// Token: 0x02001AE5 RID: 6885
		public class WorkSiteLootThing
		{
			// Token: 0x040069A3 RID: 27043
			public ThingDef thing;

			// Token: 0x040069A4 RID: 27044
			public float weight;
		}
	}
}
