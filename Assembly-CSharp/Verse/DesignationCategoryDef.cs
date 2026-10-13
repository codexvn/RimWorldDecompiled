using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse
{
	// Token: 0x0200026B RID: 619
	public class DesignationCategoryDef : Def
	{
		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000FB9 RID: 4025 RVA: 0x0004A647 File Offset: 0x00048847
		public IEnumerable<Designator> ResolvedAllowedDesignators
		{
			get
			{
				GameRules rules = Current.Game.Rules;
				int num;
				for (int i = 0; i < this.resolvedDesignators.Count; i = num + 1)
				{
					Designator designator = this.resolvedDesignators[i];
					if (rules == null || rules.DesignatorAllowed(designator))
					{
						yield return designator;
					}
					num = i;
				}
				foreach (Designator designator2 in this.AllIdeoDesignators)
				{
					yield return designator2;
				}
				IEnumerator<Designator> enumerator = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000FBA RID: 4026 RVA: 0x0004A657 File Offset: 0x00048857
		public List<Designator> AllResolvedDesignators
		{
			get
			{
				return this.resolvedDesignators;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x0004A65F File Offset: 0x0004885F
		public IEnumerable<Designator> AllIdeoDesignators
		{
			get
			{
				if (ModsConfig.IdeologyActive)
				{
					if (this.cachedPlayerFaction != Faction.OfPlayer)
					{
						this.ideoBuildingDesignatorsCached.Clear();
						this.ideoDropdownsCached.Clear();
						this.cachedPlayerFaction = Faction.OfPlayer;
					}
					foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
					{
						int num;
						for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
						{
							Precept precept = ideo.PreceptsListForReading[i];
							bool flag = precept is Precept_Building;
							bool flag2 = precept is Precept_RitualSeat;
							if (flag || flag2)
							{
								Precept_ThingDef precept_ThingDef = (Precept_ThingDef)precept;
								if (precept_ThingDef.ThingDef.designationCategory == this)
								{
									yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|18_0(precept_ThingDef.ThingDef, precept_ThingDef as Precept_Building);
								}
							}
							num = i;
						}
						for (int i = 0; i < ideo.thingStyleCategories.Count; i = num + 1)
						{
							ThingStyleCategoryWithPriority thingStyleCategoryWithPriority = ideo.thingStyleCategories[i];
							foreach (Designator designator in this.<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2(thingStyleCategoryWithPriority.category))
							{
								yield return designator;
							}
							IEnumerator<Designator> enumerator2 = null;
							num = i;
						}
						for (int i = 0; i < ideo.memes.Count; i = num + 1)
						{
							MemeDef meme = ideo.memes[i];
							if (meme.addDesignators != null)
							{
								for (int j = 0; j < meme.addDesignators.Count; j = num + 1)
								{
									if (meme.addDesignators[j].designationCategory == this)
									{
										yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|18_0(meme.addDesignators[j], null);
									}
									num = j;
								}
							}
							if (meme.addDesignatorGroups != null)
							{
								for (int j = 0; j < meme.addDesignatorGroups.Count; j = num + 1)
								{
									Designator_Dropdown designator_Dropdown = this.<get_AllIdeoDesignators>g__GetCachedDropdown|18_1(meme.addDesignatorGroups[j]);
									if (designator_Dropdown != null)
									{
										yield return designator_Dropdown;
									}
									num = j;
								}
							}
							meme = null;
							num = i;
						}
						ideo = null;
					}
					IEnumerator<Ideo> enumerator = null;
					if (Find.IdeoManager.classicMode)
					{
						foreach (StyleCategoryDef categoryDef in Find.IdeoManager.selectedStyleCategories)
						{
							foreach (Designator designator2 in this.<get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2(categoryDef))
							{
								yield return designator2;
							}
							IEnumerator<Designator> enumerator2 = null;
						}
						List<StyleCategoryDef>.Enumerator enumerator3 = default(List<StyleCategoryDef>.Enumerator);
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000FBC RID: 4028 RVA: 0x0004A66F File Offset: 0x0004886F
		public IEnumerable<Designator> AllResolvedAndIdeoDesignators
		{
			get
			{
				foreach (Designator designator in this.resolvedDesignators)
				{
					yield return designator;
				}
				List<Designator>.Enumerator enumerator = default(List<Designator>.Enumerator);
				foreach (Designator designator2 in this.AllIdeoDesignators)
				{
					yield return designator2;
				}
				IEnumerator<Designator> enumerator2 = null;
				yield break;
				yield break;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x0004A680 File Offset: 0x00048880
		public bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (ModsConfig.AnomalyActive && Find.Anomaly.HighestLevelReached < this.minMonolithLevel && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				if (this.researchPrerequisites == null)
				{
					return true;
				}
				using (List<ResearchProjectDef>.Enumerator enumerator = this.researchPrerequisites.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.IsFinished)
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0004A714 File Offset: 0x00048914
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.ResolveDesignators();
			});
			this.cachedHighlightClosedTag = "DesignationCategoryButton-" + this.defName + "-Closed";
		}

		// Token: 0x06000FBF RID: 4031 RVA: 0x0004A748 File Offset: 0x00048948
		private void ResolveDesignators()
		{
			this.resolvedDesignators.Clear();
			foreach (Type type in this.specialDesignatorClasses)
			{
				Designator designator = null;
				try
				{
					designator = (Designator)Activator.CreateInstance(type);
					designator.isOrder = true;
				}
				catch (Exception ex)
				{
					string[] array = new string[6];
					array[0] = "DesignationCategoryDef";
					array[1] = this.defName;
					array[2] = " could not instantiate special designator from class ";
					int num = 3;
					Type type2 = type;
					array[num] = ((type2 != null) ? type2.ToString() : null);
					array[4] = ".\n Exception: \n";
					array[5] = ex.ToString();
					Log.Error(string.Concat(array));
				}
				if (designator != null)
				{
					this.resolvedDesignators.Add(designator);
				}
			}
			IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
			where tDef.designationCategory == this && tDef.canGenerateDefaultDesignator
			select tDef;
			Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> dictionary = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();
			foreach (BuildableDef buildableDef in enumerable)
			{
				if (buildableDef.designatorDropdown != null)
				{
					if (!dictionary.ContainsKey(buildableDef.designatorDropdown))
					{
						dictionary[buildableDef.designatorDropdown] = new Designator_Dropdown();
						dictionary[buildableDef.designatorDropdown].Order = buildableDef.uiOrder;
						this.resolvedDesignators.Add(dictionary[buildableDef.designatorDropdown]);
					}
					dictionary[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				else
				{
					this.resolvedDesignators.Add(new Designator_Build(buildableDef));
				}
			}
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x0004A918 File Offset: 0x00048B18
		public void DirtyCache()
		{
			this.ideoBuildingDesignatorsCached.Clear();
			this.ideoDropdownsCached.Clear();
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x0004A930 File Offset: 0x00048B30
		private static bool RequirementSatisfied(string requirement)
		{
			if (!ModLister.HasActiveModWithName(requirement))
			{
				ResearchProjectDef namedSilentFail = DefDatabase<ResearchProjectDef>.GetNamedSilentFail(requirement);
				return namedSilentFail != null && namedSilentFail.IsFinished;
			}
			return true;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0004A984 File Offset: 0x00048B84
		[CompilerGenerated]
		private Designator <get_AllIdeoDesignators>g__GetCachedDesignator|18_0(BuildableDef def, Precept_Building buildingPrecept = null)
		{
			DesignationCategoryDef.BuildablePreceptBuilding key = new DesignationCategoryDef.BuildablePreceptBuilding(def, buildingPrecept);
			Designator designator;
			if (!this.ideoBuildingDesignatorsCached.TryGetValue(key, out designator))
			{
				Designator_Build designator_Build = new Designator_Build(def);
				designator = designator_Build;
				if (buildingPrecept != null)
				{
					designator_Build.sourcePrecept = buildingPrecept;
				}
				this.ideoBuildingDesignatorsCached[key] = designator;
			}
			return designator;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x0004A9CC File Offset: 0x00048BCC
		[CompilerGenerated]
		private Designator_Dropdown <get_AllIdeoDesignators>g__GetCachedDropdown|18_1(DesignatorDropdownGroupDef group)
		{
			Designator_Dropdown result;
			if (!this.ideoDropdownsCached.TryGetValue(group, out result))
			{
				IEnumerable<BuildableDef> enumerable = from tDef in DefDatabase<TerrainDef>.AllDefs.Cast<BuildableDef>().Concat(DefDatabase<ThingDef>.AllDefs.Cast<BuildableDef>())
				where tDef.designationCategory == this && !tDef.canGenerateDefaultDesignator && tDef.designatorDropdown == @group
				select tDef;
				if (!enumerable.Any<BuildableDef>())
				{
					this.ideoDropdownsCached[group] = null;
					return this.ideoDropdownsCached[group];
				}
				foreach (BuildableDef buildableDef in enumerable)
				{
					if (!this.ideoDropdownsCached.ContainsKey(buildableDef.designatorDropdown))
					{
						this.ideoDropdownsCached[buildableDef.designatorDropdown] = new Designator_Dropdown();
						this.ideoDropdownsCached[buildableDef.designatorDropdown].Order = buildableDef.uiOrder;
					}
					this.ideoDropdownsCached[buildableDef.designatorDropdown].Add(new Designator_Build(buildableDef));
				}
				result = this.ideoDropdownsCached[group];
			}
			return result;
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x0004AB0C File Offset: 0x00048D0C
		[CompilerGenerated]
		private IEnumerable<Designator> <get_AllIdeoDesignators>g__GetDesignatorsFromStyleCategory|18_2(StyleCategoryDef categoryDef)
		{
			if (categoryDef.addDesignators != null)
			{
				int num;
				for (int i = 0; i < categoryDef.addDesignators.Count; i = num + 1)
				{
					if (categoryDef.addDesignators[i].designationCategory == this)
					{
						yield return this.<get_AllIdeoDesignators>g__GetCachedDesignator|18_0(categoryDef.addDesignators[i], null);
					}
					num = i;
				}
			}
			if (categoryDef.addDesignatorGroups != null)
			{
				int num;
				for (int i = 0; i < categoryDef.addDesignatorGroups.Count; i = num + 1)
				{
					Designator_Dropdown designator_Dropdown = this.<get_AllIdeoDesignators>g__GetCachedDropdown|18_1(categoryDef.addDesignatorGroups[i]);
					if (designator_Dropdown != null)
					{
						yield return designator_Dropdown;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x04000C07 RID: 3079
		public List<Type> specialDesignatorClasses = new List<Type>();

		// Token: 0x04000C08 RID: 3080
		public int order;

		// Token: 0x04000C09 RID: 3081
		public bool showPowerGrid;

		// Token: 0x04000C0A RID: 3082
		public List<ResearchProjectDef> researchPrerequisites;

		// Token: 0x04000C0B RID: 3083
		public int minMonolithLevel;

		// Token: 0x04000C0C RID: 3084
		public int? preferredColumn;

		// Token: 0x04000C0D RID: 3085
		[Unsaved(false)]
		private List<Designator> resolvedDesignators = new List<Designator>();

		// Token: 0x04000C0E RID: 3086
		[Unsaved(false)]
		public KeyBindingCategoryDef bindingCatDef;

		// Token: 0x04000C0F RID: 3087
		[Unsaved(false)]
		public string cachedHighlightClosedTag;

		// Token: 0x04000C10 RID: 3088
		[Unsaved(false)]
		private Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator> ideoBuildingDesignatorsCached = new Dictionary<DesignationCategoryDef.BuildablePreceptBuilding, Designator>();

		// Token: 0x04000C11 RID: 3089
		[Unsaved(false)]
		private Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown> ideoDropdownsCached = new Dictionary<DesignatorDropdownGroupDef, Designator_Dropdown>();

		// Token: 0x04000C12 RID: 3090
		[Unsaved(false)]
		private Faction cachedPlayerFaction;

		// Token: 0x0200026C RID: 620
		public struct BuildablePreceptBuilding
		{
			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000FC8 RID: 4040 RVA: 0x0004AB3E File Offset: 0x00048D3E
			public BuildableDef Buildable
			{
				get
				{
					return this.buildable;
				}
			}

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0004AB46 File Offset: 0x00048D46
			public Precept_Building Precept
			{
				get
				{
					return this.precept;
				}
			}

			// Token: 0x06000FCA RID: 4042 RVA: 0x0004AB4E File Offset: 0x00048D4E
			public BuildablePreceptBuilding(BuildableDef buildable, Precept_Building precept)
			{
				this.buildable = buildable;
				this.precept = precept;
			}

			// Token: 0x04000C13 RID: 3091
			private BuildableDef buildable;

			// Token: 0x04000C14 RID: 3092
			private Precept_Building precept;
		}
	}
}
