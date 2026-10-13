using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200191D RID: 6429
	public class ApparelProperties : IRenderNodePropertiesParent
	{
		// Token: 0x1700161B RID: 5659
		// (get) Token: 0x0600921D RID: 37405 RVA: 0x002B1963 File Offset: 0x002AFB63
		public bool HasDefinedGraphicProperties
		{
			get
			{
				return !this.renderNodeProperties.NullOrEmpty<PawnRenderNodeProperties>();
			}
		}

		// Token: 0x1700161C RID: 5660
		// (get) Token: 0x0600921E RID: 37406 RVA: 0x002B1973 File Offset: 0x002AFB73
		public List<PawnRenderNodeProperties> RenderNodeProperties
		{
			get
			{
				return this.renderNodeProperties;
			}
		}

		// Token: 0x1700161D RID: 5661
		// (get) Token: 0x0600921F RID: 37407 RVA: 0x002B197B File Offset: 0x002AFB7B
		public ApparelLayerDef LastLayer
		{
			get
			{
				if (this.layers.Count > 0)
				{
					return this.layers[this.layers.Count - 1];
				}
				Log.ErrorOnce("Failed to get last layer on apparel item (see your config errors)", 31234937);
				return ApparelLayerDefOf.Belt;
			}
		}

		// Token: 0x1700161E RID: 5662
		// (get) Token: 0x06009220 RID: 37408 RVA: 0x002B19B8 File Offset: 0x002AFBB8
		public float HumanBodyCoverage
		{
			get
			{
				if (this.cachedHumanBodyCoverage < 0f)
				{
					this.cachedHumanBodyCoverage = 0f;
					List<BodyPartRecord> allParts = BodyDefOf.Human.AllParts;
					for (int i = 0; i < allParts.Count; i++)
					{
						if (this.CoversBodyPart(allParts[i]))
						{
							this.cachedHumanBodyCoverage += allParts[i].coverageAbs;
						}
					}
				}
				return this.cachedHumanBodyCoverage;
			}
		}

		// Token: 0x06009221 RID: 37409 RVA: 0x002B1A27 File Offset: 0x002AFC27
		public bool CorrectGenderForWearing(Gender wearerGender)
		{
			return this.gender == Gender.None || wearerGender == Gender.None || this.gender == wearerGender;
		}

		// Token: 0x06009222 RID: 37410 RVA: 0x002B1A3F File Offset: 0x002AFC3F
		public bool CorrectAgeForWearing(Pawn pawn)
		{
			return !ModsConfig.BiotechActive || this.developmentalStageFilter.Has(pawn.DevelopmentalStage);
		}

		// Token: 0x06009223 RID: 37411 RVA: 0x002B1A5E File Offset: 0x002AFC5E
		public bool PawnCanWear(Gender gender = Gender.None, DevelopmentalStage developmentalStage = DevelopmentalStage.Adult)
		{
			return this.CorrectGenderForWearing(gender) && this.developmentalStageFilter.Has(developmentalStage);
		}

		// Token: 0x06009224 RID: 37412 RVA: 0x002B1A7C File Offset: 0x002AFC7C
		public bool PawnCanWear(Pawn pawn, bool ignoreGender = false)
		{
			return this.PawnCanWear(ignoreGender ? Gender.None : pawn.gender, pawn.DevelopmentalStage);
		}

		// Token: 0x06009225 RID: 37413 RVA: 0x002B1A98 File Offset: 0x002AFC98
		public static void ResetStaticData()
		{
			ApparelProperties.apparelRelevantGroups = (from td in DefDatabase<ThingDef>.AllDefs
			where td.IsApparel
			select td).SelectMany((ThingDef td) => td.apparel.bodyPartGroups).Distinct<BodyPartGroupDef>().ToArray<BodyPartGroupDef>();
		}

		// Token: 0x06009226 RID: 37414 RVA: 0x002B1B01 File Offset: 0x002AFD01
		public IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			if (this.layers.NullOrEmpty<ApparelLayerDef>())
			{
				yield return parentDef.defName + " apparel has no layers.";
			}
			if (this.HasDefinedGraphicProperties)
			{
				foreach (PawnRenderNodeProperties pawnRenderNodeProperties in this.renderNodeProperties)
				{
					foreach (string text in pawnRenderNodeProperties.ConfigErrors())
					{
						yield return text;
					}
					IEnumerator<string> enumerator2 = null;
				}
				List<PawnRenderNodeProperties>.Enumerator enumerator = default(List<PawnRenderNodeProperties>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x06009227 RID: 37415 RVA: 0x000026EA File Offset: 0x000008EA
		public void PostLoadSpecial(ThingDef thingDef)
		{
		}

		// Token: 0x06009228 RID: 37416 RVA: 0x002B1B18 File Offset: 0x002AFD18
		public bool CoversBodyPart(BodyPartRecord partRec)
		{
			for (int i = 0; i < partRec.groups.Count; i++)
			{
				if (this.bodyPartGroups.Contains(partRec.groups[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009229 RID: 37417 RVA: 0x002B1B57 File Offset: 0x002AFD57
		public bool CoversBodyPartGroup(BodyPartGroupDef group)
		{
			return this.bodyPartGroups.NotNullAndContains(group);
		}

		// Token: 0x0600922A RID: 37418 RVA: 0x002B1B68 File Offset: 0x002AFD68
		public string GetCoveredOuterPartsString(BodyDef body)
		{
			return (from part in (from x in body.AllParts
			where x.depth == BodyPartDepth.Outside && x.groups.Any((BodyPartGroupDef y) => this.bodyPartGroups.Contains(y))
			select x).Distinct<BodyPartRecord>()
			select part.Label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0600922B RID: 37419 RVA: 0x002B1BC1 File Offset: 0x002AFDC1
		public string GetLayersString()
		{
			return (from layer in this.layers
			select layer.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0600922C RID: 37420 RVA: 0x002B1BFC File Offset: 0x002AFDFC
		public BodyPartGroupDef[] GetInterferingBodyPartGroups(BodyDef body)
		{
			if (this.interferingBodyPartGroups == null || this.interferingBodyPartGroups.Length != DefDatabase<BodyDef>.DefCount)
			{
				this.interferingBodyPartGroups = new BodyPartGroupDef[DefDatabase<BodyDef>.DefCount][];
			}
			if (this.interferingBodyPartGroups[(int)body.index] == null)
			{
				BodyPartGroupDef[] array = (from bpgd in (from part in body.AllParts
				where part.groups.Any((BodyPartGroupDef @group) => this.bodyPartGroups.Contains(@group))
				select part).ToArray<BodyPartRecord>().SelectMany((BodyPartRecord bpr) => bpr.groups).Distinct<BodyPartGroupDef>()
				where ApparelProperties.apparelRelevantGroups.Contains(bpgd)
				select bpgd).ToArray<BodyPartGroupDef>();
				this.interferingBodyPartGroups[(int)body.index] = array;
			}
			return this.interferingBodyPartGroups[(int)body.index];
		}

		// Token: 0x0600922D RID: 37421 RVA: 0x002B1CCC File Offset: 0x002AFECC
		public void ResolveReferencesSpecial()
		{
			if (this.renderNodeProperties != null)
			{
				for (int i = 0; i < this.renderNodeProperties.Count; i++)
				{
					this.renderNodeProperties[i].ResolveReferencesRecursive();
				}
			}
		}

		// Token: 0x04005D2E RID: 23854
		public List<BodyPartGroupDef> bodyPartGroups = new List<BodyPartGroupDef>();

		// Token: 0x04005D2F RID: 23855
		public List<ApparelLayerDef> layers = new List<ApparelLayerDef>();

		// Token: 0x04005D30 RID: 23856
		[NoTranslate]
		public string wornGraphicPath = "";

		// Token: 0x04005D31 RID: 23857
		[NoTranslate]
		public List<string> wornGraphicPaths;

		// Token: 0x04005D32 RID: 23858
		public WornGraphicData wornGraphicData;

		// Token: 0x04005D33 RID: 23859
		public bool useWornGraphicMask;

		// Token: 0x04005D34 RID: 23860
		private List<PawnRenderNodeProperties> renderNodeProperties;

		// Token: 0x04005D35 RID: 23861
		public List<RenderSkipFlagDef> renderSkipFlags;

		// Token: 0x04005D36 RID: 23862
		public DrawData drawData;

		// Token: 0x04005D37 RID: 23863
		public PawnRenderNodeTagDef parentTagDef;

		// Token: 0x04005D38 RID: 23864
		public List<int> forceEyesVisibleForRotations = new List<int>();

		// Token: 0x04005D39 RID: 23865
		public bool shellRenderedBehindHead;

		// Token: 0x04005D3A RID: 23866
		[NoTranslate]
		public List<string> tags = new List<string>();

		// Token: 0x04005D3B RID: 23867
		[NoTranslate]
		public List<string> defaultOutfitTags;

		// Token: 0x04005D3C RID: 23868
		public bool canBeGeneratedToSatisfyWarmth = true;

		// Token: 0x04005D3D RID: 23869
		public bool canBeGeneratedToSatisfyVacuumResistance = true;

		// Token: 0x04005D3E RID: 23870
		public bool canBeGeneratedToSatisfyToxicEnvironmentResistance = true;

		// Token: 0x04005D3F RID: 23871
		public bool canBeDesiredForIdeo = true;

		// Token: 0x04005D40 RID: 23872
		public List<string> ideoDesireAllowedFactionCategoryTags;

		// Token: 0x04005D41 RID: 23873
		public List<string> ideoDesireDisallowedFactionCategoryTags;

		// Token: 0x04005D42 RID: 23874
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x04005D43 RID: 23875
		public float wearPerDay = 0.4f;

		// Token: 0x04005D44 RID: 23876
		public bool careIfWornByCorpse = true;

		// Token: 0x04005D45 RID: 23877
		public bool careIfDamaged = true;

		// Token: 0x04005D46 RID: 23878
		public bool ignoredByNonViolent;

		// Token: 0x04005D47 RID: 23879
		public bool ai_pickUpOpportunistically;

		// Token: 0x04005D48 RID: 23880
		public bool blocksAddedPartWoundGraphics;

		// Token: 0x04005D49 RID: 23881
		public bool blocksVision;

		// Token: 0x04005D4A RID: 23882
		public bool immuneToToxGasExposure;

		// Token: 0x04005D4B RID: 23883
		public bool slaveApparel;

		// Token: 0x04005D4C RID: 23884
		public bool mechanitorApparel;

		// Token: 0x04005D4D RID: 23885
		public bool legsNakedUnlessCoveredBySomethingElse;

		// Token: 0x04005D4E RID: 23886
		public bool useDeflectMetalEffect;

		// Token: 0x04005D4F RID: 23887
		public bool countsAsClothingForNudity = true;

		// Token: 0x04005D50 RID: 23888
		public bool anyTechLevelCanUseForWarmth;

		// Token: 0x04005D51 RID: 23889
		public Gender gender;

		// Token: 0x04005D52 RID: 23890
		public float scoreOffset;

		// Token: 0x04005D53 RID: 23891
		public DevelopmentalStage developmentalStageFilter = DevelopmentalStage.Adult;

		// Token: 0x04005D54 RID: 23892
		public SoundDef soundWear;

		// Token: 0x04005D55 RID: 23893
		public SoundDef soundRemove;

		// Token: 0x04005D56 RID: 23894
		[Unsaved(false)]
		private float cachedHumanBodyCoverage = -1f;

		// Token: 0x04005D57 RID: 23895
		[Unsaved(false)]
		private BodyPartGroupDef[][] interferingBodyPartGroups;

		// Token: 0x04005D58 RID: 23896
		private static BodyPartGroupDef[] apparelRelevantGroups;
	}
}
