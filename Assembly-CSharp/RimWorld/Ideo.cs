using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020025D7 RID: 9687
	public class Ideo : IExposable, ILoadReferenceable
	{
		// Token: 0x17002092 RID: 8338
		// (get) Token: 0x0600D425 RID: 54309 RVA: 0x003E979A File Offset: 0x003E799A
		public List<Precept> PreceptsListForReading
		{
			get
			{
				return this.precepts;
			}
		}

		// Token: 0x17002093 RID: 8339
		// (get) Token: 0x0600D426 RID: 54310 RVA: 0x003E97A4 File Offset: 0x003E79A4
		public Texture2D Icon
		{
			get
			{
				Texture2D result;
				if ((result = this.icon) == null)
				{
					result = (this.icon = ContentFinder<Texture2D>.Get((this.iconDef != null) ? this.iconDef.iconPath : BaseContent.BadTexPath, true));
				}
				return result;
			}
		}

		// Token: 0x17002094 RID: 8340
		// (get) Token: 0x0600D427 RID: 54311 RVA: 0x003E97E4 File Offset: 0x003E79E4
		public List<Precept_Role> RolesListForReading
		{
			get
			{
				return this.cachedPossibleRoles;
			}
		}

		// Token: 0x17002095 RID: 8341
		// (get) Token: 0x0600D428 RID: 54312 RVA: 0x003E97EC File Offset: 0x003E79EC
		public bool WarnPlayerOnDesignateChopTree
		{
			get
			{
				return this.warnPlayerOnDesignateChopTree;
			}
		}

		// Token: 0x17002096 RID: 8342
		// (get) Token: 0x0600D429 RID: 54313 RVA: 0x003E97F4 File Offset: 0x003E79F4
		public bool WarnPlayerOnDesignateMine
		{
			get
			{
				return this.warnPlayerOnDesignateMine;
			}
		}

		// Token: 0x17002097 RID: 8343
		// (get) Token: 0x0600D42A RID: 54314 RVA: 0x003E97FC File Offset: 0x003E79FC
		public Color Color
		{
			get
			{
				if (this.classicMode)
				{
					return Color.white;
				}
				Color? color = this.primaryFactionColor;
				if (color != null)
				{
					return color.GetValueOrDefault();
				}
				ColorDef colorDef = this.colorDef;
				if (colorDef == null)
				{
					return Color.white;
				}
				return colorDef.color;
			}
		}

		// Token: 0x17002098 RID: 8344
		// (get) Token: 0x0600D42B RID: 54315 RVA: 0x003E9844 File Offset: 0x003E7A44
		public Color ApparelColor
		{
			get
			{
				float h;
				float b;
				float b2;
				Color.RGBToHSV(this.Color, out h, out b, out b2);
				return Color.HSVToRGB(h, Mathf.Min(0.5f, b), Mathf.Min(0.8235294f, b2));
			}
		}

		// Token: 0x17002099 RID: 8345
		// (get) Token: 0x0600D42C RID: 54316 RVA: 0x003E9880 File Offset: 0x003E7A80
		public Color TextColor
		{
			get
			{
				float h;
				float s;
				float b;
				Color.RGBToHSV(this.Color, out h, out s, out b);
				return Color.HSVToRGB(h, s, Mathf.Max(0.5f, b));
			}
		}

		// Token: 0x1700209A RID: 8346
		// (get) Token: 0x0600D42D RID: 54317 RVA: 0x003E98B0 File Offset: 0x003E7AB0
		public List<ThingDef> VeneratedAnimals
		{
			get
			{
				if (this.cachedVeneratedAnimals == null)
				{
					this.cachedVeneratedAnimals = new List<ThingDef>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Animal precept_Animal = this.precepts[i] as Precept_Animal;
						if (precept_Animal != null)
						{
							this.cachedVeneratedAnimals.Add(precept_Animal.ThingDef);
						}
					}
				}
				return this.cachedVeneratedAnimals;
			}
		}

		// Token: 0x1700209B RID: 8347
		// (get) Token: 0x0600D42E RID: 54318 RVA: 0x003E9914 File Offset: 0x003E7B14
		public List<XenotypeDef> PreferredXenotypes
		{
			get
			{
				if (this.cachedPreferredXenotypes == null)
				{
					this.cachedPreferredXenotypes = new List<XenotypeDef>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Xenotype precept_Xenotype = this.precepts[i] as Precept_Xenotype;
						if (precept_Xenotype != null && precept_Xenotype.xenotype != null)
						{
							this.cachedPreferredXenotypes.Add(precept_Xenotype.xenotype);
						}
					}
				}
				return this.cachedPreferredXenotypes;
			}
		}

		// Token: 0x1700209C RID: 8348
		// (get) Token: 0x0600D42F RID: 54319 RVA: 0x003E9980 File Offset: 0x003E7B80
		public List<CustomXenotype> PreferredCustomXenotypes
		{
			get
			{
				if (this.cachedPreferredCustomXenotypes == null)
				{
					this.cachedPreferredCustomXenotypes = new List<CustomXenotype>();
					for (int i = 0; i < this.precepts.Count; i++)
					{
						Precept_Xenotype precept_Xenotype = this.precepts[i] as Precept_Xenotype;
						if (precept_Xenotype != null && precept_Xenotype.customXenotype != null)
						{
							this.cachedPreferredCustomXenotypes.Add(precept_Xenotype.customXenotype);
						}
					}
				}
				return this.cachedPreferredCustomXenotypes;
			}
		}

		// Token: 0x1700209D RID: 8349
		// (get) Token: 0x0600D430 RID: 54320 RVA: 0x003E99EC File Offset: 0x003E7BEC
		public IntRange DeityCountRange
		{
			get
			{
				int num = 0;
				int num2 = int.MaxValue;
				for (int i = 0; i < this.memes.Count; i++)
				{
					if (this.memes[i].deityCount.min >= 0)
					{
						num = Mathf.Max(num, this.memes[i].deityCount.min);
					}
					if (this.memes[i].deityCount.max >= 0)
					{
						num2 = Mathf.Min(num2, this.memes[i].deityCount.max);
					}
				}
				return new IntRange(num, num2);
			}
		}

		// Token: 0x1700209E RID: 8350
		// (get) Token: 0x0600D431 RID: 54321 RVA: 0x003E9A8C File Offset: 0x003E7C8C
		public MemeDef StructureMeme
		{
			get
			{
				for (int i = 0; i < this.memes.Count; i++)
				{
					if (this.memes[i].category == MemeCategory.Structure)
					{
						return this.memes[i];
					}
				}
				return null;
			}
		}

		// Token: 0x1700209F RID: 8351
		// (get) Token: 0x0600D432 RID: 54322 RVA: 0x003E9AD1 File Offset: 0x003E7CD1
		public Gender SupremeGender
		{
			get
			{
				if (this.HasMeme(MemeDefOf.MaleSupremacy))
				{
					return Gender.Male;
				}
				if (this.HasMeme(MemeDefOf.FemaleSupremacy))
				{
					return Gender.Female;
				}
				return Gender.None;
			}
		}

		// Token: 0x170020A0 RID: 8352
		// (get) Token: 0x0600D433 RID: 54323 RVA: 0x003E9AF4 File Offset: 0x003E7CF4
		public int RequiredScars
		{
			get
			{
				if (this.precepts.NullOrEmpty<Precept>())
				{
					return 0;
				}
				if (this.requiredScarsCached == -1)
				{
					this.requiredScarsCached = 0;
					foreach (Precept precept in this.precepts)
					{
						if (precept.def.requiredScars > this.requiredScarsCached)
						{
							this.requiredScarsCached = precept.def.requiredScars;
						}
					}
				}
				return this.requiredScarsCached;
			}
		}

		// Token: 0x170020A1 RID: 8353
		// (get) Token: 0x0600D434 RID: 54324 RVA: 0x003E9B8C File Offset: 0x003E7D8C
		public float BlindPawnChance
		{
			get
			{
				if (this.precepts.NullOrEmpty<Precept>())
				{
					return 0f;
				}
				float num = 0f;
				int num2 = 0;
				foreach (Precept precept in this.precepts)
				{
					if (precept.def.blindPawnChance >= 0f)
					{
						num += precept.def.blindPawnChance;
						num2++;
					}
				}
				if (num2 <= 0)
				{
					return 0f;
				}
				return num / (float)num2;
			}
		}

		// Token: 0x170020A2 RID: 8354
		// (get) Token: 0x0600D435 RID: 54325 RVA: 0x003E9C28 File Offset: 0x003E7E28
		public string KeyDeityName
		{
			get
			{
				IdeoFoundation_Deity ideoFoundation_Deity = this.foundation as IdeoFoundation_Deity;
				if (ideoFoundation_Deity != null && ideoFoundation_Deity.DeitiesListForReading.Any<IdeoFoundation_Deity.Deity>())
				{
					return ideoFoundation_Deity.DeitiesListForReading[0].name;
				}
				return null;
			}
		}

		// Token: 0x170020A3 RID: 8355
		// (get) Token: 0x0600D436 RID: 54326 RVA: 0x003E9C64 File Offset: 0x003E7E64
		public bool ObligationsActive
		{
			get
			{
				if (this.colonistBelieverCountCached == -1)
				{
					this.RecacheColonistBelieverCount();
				}
				return this.colonistBelieverCountCached >= Ideo.MinBelieversToEnableObligations || Faction.OfPlayer.ideos.IsPrimary(this);
			}
		}

		// Token: 0x170020A4 RID: 8356
		// (get) Token: 0x0600D437 RID: 54327 RVA: 0x003E9C98 File Offset: 0x003E7E98
		public SoundDef SoundOngoingRitual
		{
			get
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.category.soundOngoingRitual != null)
					{
						return thingStyleCategoryWithPriority.category.soundOngoingRitual;
					}
				}
				return SoundDefOf.RitualSustainer_Theist;
			}
		}

		// Token: 0x170020A5 RID: 8357
		// (get) Token: 0x0600D438 RID: 54328 RVA: 0x003E9D08 File Offset: 0x003E7F08
		public RitualVisualEffectDef RitualEffect
		{
			get
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.category.ritualVisualEffectDef != null)
					{
						return thingStyleCategoryWithPriority.category.ritualVisualEffectDef;
					}
				}
				return RitualVisualEffectDefOf.Basic;
			}
		}

		// Token: 0x170020A6 RID: 8358
		// (get) Token: 0x0600D439 RID: 54329 RVA: 0x003E9D78 File Offset: 0x003E7F78
		public ThingDef RitualSeatDef
		{
			get
			{
				foreach (Precept precept in this.PreceptsListForReading)
				{
					Precept_RitualSeat precept_RitualSeat = precept as Precept_RitualSeat;
					if (precept_RitualSeat != null)
					{
						return precept_RitualSeat.ThingDef;
					}
				}
				return null;
			}
		}

		// Token: 0x170020A7 RID: 8359
		// (get) Token: 0x0600D43A RID: 54330 RVA: 0x003E9DD8 File Offset: 0x003E7FD8
		// (set) Token: 0x0600D43B RID: 54331 RVA: 0x003E9E14 File Offset: 0x003E8014
		public string WorshipRoomLabel
		{
			get
			{
				if (this.overriddenWorshipRoomLabel != null)
				{
					return this.overriddenWorshipRoomLabel;
				}
				MemeDef structureMeme = this.StructureMeme;
				if (structureMeme != null)
				{
					return structureMeme.worshipRoomLabel;
				}
				return RoomRoleDefOf.WorshipRoom.label.CapitalizeFirst();
			}
			set
			{
				this.overriddenWorshipRoomLabel = value;
			}
		}

		// Token: 0x170020A8 RID: 8360
		// (get) Token: 0x0600D43C RID: 54332 RVA: 0x003E9E1D File Offset: 0x003E801D
		public string MemberNamePlural
		{
			get
			{
				if (this.memberNamePluralCached.NullOrEmpty())
				{
					this.memberNamePluralCached = Find.ActiveLanguageWorker.Pluralize(this.memberName, -1);
				}
				return this.memberNamePluralCached;
			}
		}

		// Token: 0x170020A9 RID: 8361
		// (get) Token: 0x0600D43D RID: 54333 RVA: 0x003E9E49 File Offset: 0x003E8049
		// (set) Token: 0x0600D43E RID: 54334 RVA: 0x003E9E51 File Offset: 0x003E8051
		public bool Fluid
		{
			get
			{
				return this.fluid;
			}
			set
			{
				if (value && this.development == null)
				{
					this.development = new IdeoDevelopmentTracker(this);
				}
				this.fluid = value;
			}
		}

		// Token: 0x170020AA RID: 8362
		// (get) Token: 0x0600D43F RID: 54335 RVA: 0x003E9E74 File Offset: 0x003E8074
		public bool LikesHumanLeatherApparel
		{
			get
			{
				using (List<Precept>.Enumerator enumerator = this.precepts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.likesHumanLeatherApparel)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600D440 RID: 54336 RVA: 0x003E9ED4 File Offset: 0x003E80D4
		public bool IsWorkTypeConsideredDangerous(WorkTypeDef workType)
		{
			if (!ModsConfig.IdeologyActive)
			{
				return false;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.opposedWorkTypes.Count; j++)
				{
					if (this.precepts[i].def.opposedWorkTypes[j] == workType)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D441 RID: 54337 RVA: 0x003E9F48 File Offset: 0x003E8148
		public void SortStyleCategories()
		{
			this.thingStyleCategories.SortBy((ThingStyleCategoryWithPriority x) => -x.priority);
			this.RecachePossibleBuildables();
		}

		// Token: 0x0600D442 RID: 54338 RVA: 0x003E9F7C File Offset: 0x003E817C
		public Ideo()
		{
			this.style = new IdeoStyleTracker(this);
		}

		// Token: 0x0600D443 RID: 54339 RVA: 0x003EA04C File Offset: 0x003E824C
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.overriddenWorshipRoomLabel, "overriddenWorshipRoomLabel", null, false);
			Scribe_Values.Look<bool>(ref this.createdFromNoExpansionGame, "createdFromNoExpansionGame", false, false);
			Scribe_Deep.Look<IdeoFoundation>(ref this.foundation, "foundation", Array.Empty<object>());
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.adjective, "adjective", null, false);
			Scribe_Values.Look<string>(ref this.memberName, "memberName", null, false);
			Scribe_Values.Look<string>(ref this.leaderTitleMale, "leaderTitleMale", null, false);
			Scribe_Values.Look<string>(ref this.leaderTitleFemale, "leaderTitleFemale", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<string>(ref this.descriptionTemplate, "descriptionTemplate", null, false);
			Scribe_Values.Look<bool>(ref this.descriptionLocked, "descriptionLocked", false, false);
			Scribe_Values.Look<bool>(ref this.classicExtraMode, "classic", false, false);
			Scribe_Values.Look<bool>(ref this.classicMode, "hiddenIdeoMode", false, false);
			Scribe_Defs.Look<CultureDef>(ref this.culture, "culture");
			Scribe_Defs.Look<IdeoIconDef>(ref this.iconDef, "iconDef");
			Scribe_Defs.Look<ColorDef>(ref this.colorDef, "colorDef");
			Scribe_Values.Look<Color?>(ref this.primaryFactionColor, "primaryFactionColor", null, false);
			Scribe_Collections.Look<MemeDef>(ref this.memes, "memes", LookMode.Def, Array.Empty<object>());
			Scribe_Collections.Look<Precept>(ref this.precepts, "precepts", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<ThingStyleCategoryWithPriority>(ref this.thingStyleCategories, "thingStyleCategories", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.usedSymbols, "usedSymbols", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.usedSymbolPacks, "usedSymbolPacks", LookMode.Value, Array.Empty<object>());
			Scribe_Deep.Look<IdeoStyleTracker>(ref this.style, "style", new object[]
			{
				this
			});
			Scribe_Values.Look<bool>(ref this.fluid, "fluid", false, false);
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Values.Look<bool>(ref this.solid, "solid", false, false);
			if (!GameDataSaveLoader.IsSavingOrLoadingExternalIdeo)
			{
				Scribe_Values.Look<int>(ref this.id, "id", 0, false);
				Scribe_Values.Look<bool>(ref this.relicsCollected, "relicsCollected", false, false);
				Scribe_Values.Look<bool>(ref this.initialPlayerIdeo, "initialPlayerIdeo", false, false);
				Scribe_Deep.Look<IdeoDevelopmentTracker>(ref this.development, "development", new object[]
				{
					this
				});
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				if (this.foundation != null)
				{
					this.foundation.ideo = this;
				}
				if (this.style == null)
				{
					this.style = new IdeoStyleTracker(this);
				}
				if (this.style.ideo == null)
				{
					this.style.ideo = this;
				}
				if (this.development != null && this.development.ideo == null)
				{
					this.development.ideo = this;
				}
				if (this.fluid && this.development == null)
				{
					this.development = new IdeoDevelopmentTracker(this);
				}
				if (this.memes.RemoveAll((MemeDef x) => x == null) != 0)
				{
					Log.Error("Some ideoligion memes were null after loading.");
				}
				if (this.precepts.RemoveAll(delegate(Precept x)
				{
					if (x != null && x.def != null)
					{
						Precept_Ritual precept_Ritual = x as Precept_Ritual;
						return precept_Ritual != null && precept_Ritual.behavior == null;
					}
					return true;
				}) != 0)
				{
					Log.Error("Some ideoligion precepts were null after loading.");
				}
				if (this.thingStyleCategories == null)
				{
					this.thingStyleCategories = new List<ThingStyleCategoryWithPriority>();
				}
				if (this.thingStyleCategories.RemoveAll((ThingStyleCategoryWithPriority x) => x == null || x.category == null) != 0)
				{
					Log.Error("Some thing style categories were null after loading.");
				}
				if (this.culture == null)
				{
					this.culture = DefDatabase<CultureDef>.AllDefsListForReading.RandomElement<CultureDef>();
					Log.Error("Ideoligion had null culture. Assigning random.");
				}
				if (this.usedSymbols == null)
				{
					this.usedSymbols = new List<string>();
				}
				if (this.usedSymbolPacks == null)
				{
					this.usedSymbolPacks = new List<string>();
				}
				for (int i = 0; i < this.precepts.Count; i++)
				{
					if (this.precepts[i] != null)
					{
						this.precepts[i].ideo = this;
					}
				}
				if (this.description == null)
				{
					this.RegenerateDescription(false);
				}
				if (this.createdFromNoExpansionGame && ModsConfig.IdeologyActive && this.iconDef == null)
				{
					this.SetIcon(IdeoFoundation.GetRandomIconDef(this), IdeoFoundation.GetRandomColorDef(this), false);
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.memberName.NullOrEmpty())
				{
					this.memberName = FactionDefOf.PlayerColony.basicMemberKind.label;
				}
				if (ModsConfig.IdeologyActive && !this.classicMode && this.RitualSeatDef == null)
				{
					PreceptDef def;
					if ((from x in DefDatabase<PreceptDef>.AllDefs
					where x.issue == IssueDefOf.IdeoRitualSeat
					select x).TryRandomElementByWeight((PreceptDef x) => x.selectionWeight, out def))
					{
						this.AddPrecept(PreceptMaker.MakePrecept(def), true, null, null);
						Log.Warning("Adding missing ritual seat precept " + this.RitualSeatDef.LabelCap);
					}
				}
				using (List<PreceptDef>.Enumerator enumerator = DefDatabase<PreceptDef>.AllDefsListForReading.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PreceptDef p = enumerator.Current;
						if ((p.takeNameFrom == null || this.precepts.Any((Precept _p) => _p.def == p.takeNameFrom)) && !(p.preceptClass != typeof(Precept_Ritual)) && !p.visible && !this.precepts.Any((Precept o) => o.def.ritualPatternBase == p.ritualPatternBase) && (!Find.IdeoManager.classicMode || p.classic))
						{
							RitualPatternDef ritualPatternBase = p.ritualPatternBase;
							Precept precept = PreceptMaker.MakePrecept(p);
							this.AddPrecept(precept, true, null, ritualPatternBase);
							Log.Warning("A hidden ritual precept was missing, adding: " + precept.def.LabelCap);
						}
					}
				}
				List<Precept> list = (from x in this.precepts
				where x.def == PreceptDefOf.FuneralNoCorpse
				select x).ToList<Precept>();
				if (list.Count > 1)
				{
					Log.Warning("Multiple FuneralNoCorpse precepts found, removing extras.");
					for (int j = 1; j < list.Count; j++)
					{
						this.precepts.Remove(list[j]);
					}
				}
				if (this.classicMode && this.createdFromNoExpansionGame)
				{
					this.classicMode = false;
				}
				this.RecachePrecepts();
			}
		}

		// Token: 0x0600D444 RID: 54340 RVA: 0x003EA714 File Offset: 0x003E8914
		public bool HasMeme(MemeDef meme)
		{
			return this.memes.Contains(meme);
		}

		// Token: 0x0600D445 RID: 54341 RVA: 0x003EA722 File Offset: 0x003E8922
		public bool EnablesNeed(NeedDef def)
		{
			return this.cachedEnabledNeeds.Contains(def);
		}

		// Token: 0x0600D446 RID: 54342 RVA: 0x003EA730 File Offset: 0x003E8930
		public bool DisablesNeed(NeedDef def)
		{
			return this.cachedDisabledNeeds.Contains(def);
		}

		// Token: 0x0600D447 RID: 54343 RVA: 0x003EA740 File Offset: 0x003E8940
		public void IdeoTick()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				this.precepts[i].Tick();
			}
			if (this.colonistBelieverCountCached == -1)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x0600D448 RID: 54344 RVA: 0x003EA784 File Offset: 0x003E8984
		public void Notify_AddBedThoughts(Pawn pawn)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_AddBedThoughts(pawn, this.precepts[i]);
				}
			}
		}

		// Token: 0x0600D449 RID: 54345 RVA: 0x003EA7FC File Offset: 0x003E89FC
		public void Notify_MemberTookAction(HistoryEvent ev, bool canApplySelfTookThoughts)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_MemberTookAction(ev, this.precepts[i], canApplySelfTookThoughts);
				}
			}
		}

		// Token: 0x0600D44A RID: 54346 RVA: 0x003EA874 File Offset: 0x003E8A74
		public void Notify_MemberKnows(HistoryEvent ev, Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					this.precepts[i].def.comps[j].Notify_MemberWitnessedAction(ev, this.precepts[i], member);
				}
			}
		}

		// Token: 0x0600D44B RID: 54347 RVA: 0x003EA8EC File Offset: 0x003E8AEC
		public void Notify_MemberGuestStatusChanged(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGuestStatusChanged(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberGuestStatusChanged(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D44C RID: 54348 RVA: 0x003EA954 File Offset: 0x003E8B54
		public void Notify_MemberDied(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberDied(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberDied(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
			if (Faction.OfPlayerSilentFail != null && member.Faction == Faction.OfPlayerSilentFail)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x0600D44D RID: 54349 RVA: 0x003EA9D8 File Offset: 0x003E8BD8
		public void Notify_MemberLost(Pawn member, Map map)
		{
			if (member.IsColonist && map != null && member.workSettings != null && member.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
			{
				bool flag = false;
				foreach (Pawn pawn in map.mapPawns.FreeColonists)
				{
					if (pawn != member && pawn.Ideo == this && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					foreach (Pawn pawn2 in map.mapPawns.PrisonersOfColonySpawned)
					{
						pawn2.guest.Notify_WardensOfIdeoLost(this);
					}
				}
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberLost(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberLost(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D44E RID: 54350 RVA: 0x003EAB24 File Offset: 0x003E8D24
		public void Notify_MemberGained(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGained(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberGained(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D44F RID: 54351 RVA: 0x003EAB8C File Offset: 0x003E8D8C
		public void Notify_MemberCorpseDestroyed(Pawn member)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberCorpseDestroyed(member);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberCorpseDestroyed(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D450 RID: 54352 RVA: 0x003EABF4 File Offset: 0x003E8DF4
		public void Notify_MemberGenerated(Pawn member, bool newborn, bool ignoreApparel = false)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_MemberGenerated(member, newborn, ignoreApparel);
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_MemberGenerated(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D451 RID: 54353 RVA: 0x003EAC60 File Offset: 0x003E8E60
		public void Notify_GameStarted()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				try
				{
					this.precepts[i].Notify_GameStarted();
				}
				catch (Exception ex)
				{
					string str = "Error in Precept.Notify_GameStarted(): ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
			}
		}

		// Token: 0x0600D452 RID: 54354 RVA: 0x003EACC8 File Offset: 0x003E8EC8
		public void Notify_RelicSeenByPlayer(Thing relic)
		{
			if (this.AllRelicsNewlyCollected())
			{
				Ideo.tmpPlayerIdeoFollowers.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMapsAndWorld_Alive)
				{
					if (pawn.Ideo == this && pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && pawn.needs.mood.thoughts.memories != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.RelicsCollected, null, null);
						if (pawn.Faction == Faction.OfPlayer)
						{
							Ideo.tmpPlayerIdeoFollowers.Add(pawn);
						}
					}
				}
				if (Ideo.tmpPlayerIdeoFollowers.Count > 0)
				{
					TaggedString t = this.classicMode ? "LetterTextRelicsCollectedClassic".Translate() : "LetterTextRelicsCollected".Translate(this);
					ChoiceLetter let = LetterMaker.MakeLetter("LetterLabelRelicsCollected".Translate() + ": " + this.name.ApplyTag(this).Resolve(), t + ":\n\n" + (from p in Ideo.tmpPlayerIdeoFollowers
					select p.LabelNoCountColored.Resolve()).ToList<string>().ToLineList("- "), LetterDefOf.PositiveEvent, Ideo.tmpPlayerIdeoFollowers, null, null, null);
					Find.LetterStack.ReceiveLetter(let, null, 0, true);
					Ideo.tmpPlayerIdeoFollowers.Clear();
				}
				this.relicsCollected = true;
			}
		}

		// Token: 0x0600D453 RID: 54355 RVA: 0x003EAE90 File Offset: 0x003E9090
		public void Notify_MemberGainedByConversion()
		{
			if (this.Fluid)
			{
				this.development.Notify_MemberGainedByConversion();
			}
		}

		// Token: 0x0600D454 RID: 54356 RVA: 0x003EAEA8 File Offset: 0x003E90A8
		private bool AllRelicsNewlyCollected()
		{
			if (this.relicsCollected)
			{
				return false;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Relic precept_Relic = this.precepts[i] as Precept_Relic;
				if (precept_Relic != null && !precept_Relic.RelicInPlayerPossession)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600D455 RID: 54357 RVA: 0x003EAEF8 File Offset: 0x003E90F8
		public bool MemberWillingToDo(HistoryEvent ev)
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					if (!this.precepts[i].def.comps[j].MemberWillingToDo(ev))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D456 RID: 54358 RVA: 0x003EAF68 File Offset: 0x003E9168
		public Pair<Precept, Precept> FirstIncompatiblePreceptPair()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts.Count; j++)
				{
					if (this.precepts[i] != this.precepts[j] && !this.precepts[i].CompatibleWith(this.precepts[j]))
					{
						return new Pair<Precept, Precept>(this.precepts[i], this.precepts[j]);
					}
				}
			}
			return default(Pair<Precept, Precept>);
		}

		// Token: 0x0600D457 RID: 54359 RVA: 0x003EB004 File Offset: 0x003E9204
		public Precept FirstPreceptWithWarning()
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				string text;
				string text2;
				if (this.precepts[i].GetPlayerWarning(out text, out text2))
				{
					return this.precepts[i];
				}
			}
			return null;
		}

		// Token: 0x0600D458 RID: 54360 RVA: 0x003EB04C File Offset: 0x003E924C
		public Tuple<Precept_Ritual, List<string>> FirstRitualMissingTarget()
		{
			foreach (Precept precept in this.precepts)
			{
				Precept_Ritual precept_Ritual = precept as Precept_Ritual;
				if (precept_Ritual != null && precept_Ritual.obligationTargetFilter != null)
				{
					List<string> list = precept_Ritual.obligationTargetFilter.MissingTargetBuilding(this);
					if (!list.NullOrEmpty<string>())
					{
						return new Tuple<Precept_Ritual, List<string>>(precept_Ritual, list);
					}
				}
			}
			return null;
		}

		// Token: 0x0600D459 RID: 54361 RVA: 0x003EB0CC File Offset: 0x003E92CC
		public Precept_Building FirstConsumableBuildingMissingRitual()
		{
			foreach (Precept precept in this.precepts)
			{
				Precept_Building precept_Building = precept as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.ritualFocus != null && precept_Building.ThingDef.ritualFocus.consumable && !this.HasRequiredRitualForBuilding(precept_Building))
				{
					return precept_Building;
				}
			}
			return null;
		}

		// Token: 0x0600D45A RID: 54362 RVA: 0x003EB150 File Offset: 0x003E9350
		private bool HasRequiredRitualForBuilding(Precept_Building building)
		{
			if (this.cachedRitualTargetFilters == null)
			{
				this.cachedRitualTargetFilters = new List<RitualObligationTargetFilter>();
				foreach (Precept precept in this.precepts)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null && precept_Ritual.obligationTargetFilter != null)
					{
						this.cachedRitualTargetFilters.Add(precept_Ritual.obligationTargetFilter.def.GetInstance());
					}
				}
			}
			foreach (RitualObligationTargetFilter ritualObligationTargetFilter in this.cachedRitualTargetFilters)
			{
				if (!ritualObligationTargetFilter.def.thingDefs.NullOrEmpty<ThingDef>() && ritualObligationTargetFilter.def.thingDefs.Contains(building.ThingDef))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D45B RID: 54363 RVA: 0x003EB248 File Offset: 0x003E9448
		public bool IsPreferredXenotype(Pawn pawn)
		{
			if (!this.PreferredXenotypes.Any<XenotypeDef>() && !this.PreferredCustomXenotypes.Any<CustomXenotype>())
			{
				return false;
			}
			if (pawn.genes == null)
			{
				return false;
			}
			if (pawn.genes.CustomXenotype != null)
			{
				foreach (CustomXenotype custom in this.PreferredCustomXenotypes)
				{
					if (GeneUtility.PawnIsCustomXenotype(pawn, custom))
					{
						return true;
					}
				}
				return false;
			}
			return this.PreferredXenotypes.Contains(pawn.genes.Xenotype);
		}

		// Token: 0x0600D45C RID: 54364 RVA: 0x003EB2F0 File Offset: 0x003E94F0
		public void ClearPrecepts()
		{
			this.precepts.Clear();
			this.RecachePrecepts();
			this.anyPreceptEdited = false;
		}

		// Token: 0x0600D45D RID: 54365 RVA: 0x003EB30C File Offset: 0x003E950C
		public void RecachePrecepts()
		{
			this.allPreceptDefs.Clear();
			foreach (Precept precept in this.precepts)
			{
				this.allPreceptDefs.Add(precept.def);
			}
			this.requiredScarsCached = -1;
			this.cachedVeneratedAnimals = null;
			this.cachedRitualTargetFilters = null;
			this.cachedPreferredXenotypes = null;
			this.cachedPreferredCustomXenotypes = null;
			this.RecachePossibleSituationalThoughts();
			this.RecachePossibleGoodwillSituations();
			this.RecachePossibleBuildings();
			this.RecachePossibleBuildables();
			this.RecachePossibleRoles();
			this.RecachePossibleMentalBreaks();
			this.RecacheNeeds();
			this.warnPlayerOnDesignateChopTree = false;
			this.warnPlayerOnDesignateMine = false;
			foreach (Precept precept2 in this.precepts)
			{
				if (precept2.def.warnPlayerOnDesignateChopTree)
				{
					this.warnPlayerOnDesignateChopTree = true;
				}
				if (precept2.def.warnPlayerOnDesignateMine)
				{
					this.warnPlayerOnDesignateMine = true;
				}
			}
			foreach (Precept precept3 in this.precepts)
			{
				precept3.Notify_RecachedPrecepts();
			}
			this.currentCacheId++;
		}

		// Token: 0x0600D45E RID: 54366 RVA: 0x003EB480 File Offset: 0x003E9680
		public void AddPrecept(Precept precept, bool init = false, FactionDef generatingFor = null, RitualPatternDef fillWith = null)
		{
			if (precept == null)
			{
				Log.Error("Tried to add a null PreceptDef.");
				return;
			}
			if (this.precepts.Contains(precept))
			{
				Log.Error("Tried to add the same PreceptDef twice.");
				return;
			}
			precept.ideo = this;
			this.precepts.Add(precept);
			this.precepts.SortBy((Precept x) => -this.GetPreceptImpact(x), (Precept x) => -x.def.displayOrderInImpact, (Precept x) => x.def.defName);
			if (init)
			{
				precept.Init(this, generatingFor);
				if (fillWith != null)
				{
					Precept_Ritual precept_Ritual = precept as Precept_Ritual;
					if (precept_Ritual != null)
					{
						fillWith.Fill(precept_Ritual);
						precept_Ritual.RegenerateName();
					}
				}
			}
			this.RecachePrecepts();
			if (precept.def.alsoAdds != null)
			{
				Precept precept2 = this.precepts.FirstOrDefault((Precept p) => p.def == precept.def.alsoAdds);
				if (precept2 != null)
				{
					precept2.RegenerateName();
					return;
				}
				Precept precept3 = PreceptMaker.MakePrecept(precept.def.alsoAdds);
				this.AddPrecept(precept3, true, generatingFor, (fillWith != null) ? precept3.def.ritualPatternBase : null);
			}
		}

		// Token: 0x0600D45F RID: 54367 RVA: 0x003EB5E0 File Offset: 0x003E97E0
		private float GetPreceptImpact(Precept precept)
		{
			if (!precept.SortByImpact)
			{
				return 0f;
			}
			return (float)precept.def.impact + 1f;
		}

		// Token: 0x0600D460 RID: 54368 RVA: 0x003EB602 File Offset: 0x003E9802
		public bool PreceptIsRequired(PreceptDef precept)
		{
			return this.GetMemeThatRequiresPrecept(precept) != null;
		}

		// Token: 0x0600D461 RID: 54369 RVA: 0x003EB610 File Offset: 0x003E9810
		public MemeDef GetMemeThatRequiresPrecept(PreceptDef precept)
		{
			Predicate<List<PreceptDef>> <>9__0;
			for (int i = 0; i < this.memes.Count; i++)
			{
				if (this.memes[i].requireOne != null)
				{
					List<List<PreceptDef>> requireOne = this.memes[i].requireOne;
					Predicate<List<PreceptDef>> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((List<PreceptDef> x) => x.Contains(precept)));
					}
					if (requireOne.Any(predicate))
					{
						return this.memes[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600D462 RID: 54370 RVA: 0x003EB698 File Offset: 0x003E9898
		public AcceptanceReport CanAddPreceptAllFactions(PreceptDef preceptDef)
		{
			AcceptanceReport result = this.foundation.CanAdd(preceptDef, false);
			if (!result.Accepted)
			{
				return result;
			}
			if (Find.World != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (faction.def.humanlikeFaction && faction.ideos != null && (faction.ideos.IsPrimary(this) || faction.ideos.IsMinor(this)) && !this.foundation.CanAddForFaction(preceptDef, faction.def, null, false, false, false, false))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600D463 RID: 54371 RVA: 0x003EB760 File Offset: 0x003E9960
		public void RemovePrecept(Precept precept, bool replacing = false)
		{
			this.precepts.Remove(precept);
			for (int i = this.precepts.Count - 1; i >= 0; i--)
			{
				if (this.precepts[i].def.takeNameFrom == precept.def)
				{
					this.RemovePrecept(this.precepts[i], false);
				}
			}
			Precept_Role precept_Role = precept as Precept_Role;
			if (precept_Role != null)
			{
				foreach (Pawn pawn in precept_Role.ChosenPawns())
				{
					Pawn_AbilityTracker abilities = pawn.abilities;
					if (abilities != null)
					{
						abilities.Notify_TemporaryAbilitiesChanged();
					}
				}
			}
			if (!replacing && precept.def.defaultSelectionWeight <= 0f && precept.def.issue.HasDefaultPrecept)
			{
				PreceptDef def;
				if ((from x in DefDatabase<PreceptDef>.AllDefs
				where x.issue == precept.def.issue && this.CanAddPreceptAllFactions(x).Accepted && x.defaultSelectionWeight > 0f
				select x).TryRandomElementByWeight((PreceptDef x) => x.defaultSelectionWeight, out def))
				{
					this.AddPrecept(PreceptMaker.MakePrecept(def), true, null, null);
					return;
				}
			}
			this.RecachePrecepts();
		}

		// Token: 0x0600D464 RID: 54372 RVA: 0x003EB8C4 File Offset: 0x003E9AC4
		public void SetIcon(IdeoIconDef iconDef, ColorDef colorDef, bool clearPrimaryFactionColor = false)
		{
			this.iconDef = iconDef;
			this.colorDef = colorDef;
			this.icon = null;
			if (clearPrimaryFactionColor)
			{
				this.primaryFactionColor = null;
			}
		}

		// Token: 0x0600D465 RID: 54373 RVA: 0x003EB8EA File Offset: 0x003E9AEA
		public void DrawIcon(Rect rect)
		{
			Color color = GUI.color;
			GUI.color = this.Color;
			GUI.DrawTexture(rect, this.Icon);
			GUI.color = color;
		}

		// Token: 0x0600D466 RID: 54374 RVA: 0x003EB910 File Offset: 0x003E9B10
		public List<Precept> GetAllPreceptsAllowingSituationalThought(ThoughtDef def)
		{
			Ideo.tmpThoughtPrecepts.Clear();
			if (!this.cachedPossibleSituationalThoughts.Contains(def))
			{
				return Ideo.tmpThoughtPrecepts;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				List<PreceptComp> comps = this.precepts[i].def.comps;
				for (int j = 0; j < comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought = comps[j] as PreceptComp_SituationalThought;
					if (preceptComp_SituationalThought != null && preceptComp_SituationalThought.thought == def)
					{
						Ideo.tmpThoughtPrecepts.Add(this.precepts[i]);
						break;
					}
				}
			}
			return Ideo.tmpThoughtPrecepts;
		}

		// Token: 0x0600D467 RID: 54375 RVA: 0x003EB9B0 File Offset: 0x003E9BB0
		public Precept GetFirstPreceptAllowingSituationalThought(ThoughtDef def)
		{
			if (!this.cachedPossibleSituationalThoughts.Contains(def))
			{
				return null;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				List<PreceptComp> comps = this.precepts[i].def.comps;
				for (int j = 0; j < comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought = comps[j] as PreceptComp_SituationalThought;
					if (preceptComp_SituationalThought != null && preceptComp_SituationalThought.thought == def)
					{
						return this.precepts[i];
					}
				}
			}
			return null;
		}

		// Token: 0x0600D468 RID: 54376 RVA: 0x003EBA34 File Offset: 0x003E9C34
		public bool HasPreceptForBuilding(ThingDef buildingDef)
		{
			return this.cachedPossibleBuildings.Any((Precept_Building b) => b.ThingDef == buildingDef);
		}

		// Token: 0x0600D469 RID: 54377 RVA: 0x003EBA68 File Offset: 0x003E9C68
		public bool MembersCanBuild(Thing thing)
		{
			if (this.classicMode)
			{
				return true;
			}
			BuildableDef buildableDef = thing.def.entityDefToBuild ?? thing.def;
			if (buildableDef.canGenerateDefaultDesignator)
			{
				return true;
			}
			Precept_ThingStyle styleSourcePrecept = thing.StyleSourcePrecept;
			if (((styleSourcePrecept != null) ? styleSourcePrecept.ideo : null) == this || this.cachedPossibleBuildables.Contains(buildableDef))
			{
				return true;
			}
			ThingDef thingDef = buildableDef as ThingDef;
			if (thingDef != null && this.HasPreceptForBuilding(thingDef))
			{
				return true;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def.willingToConstructOtherIdeoBuildables)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600D46A RID: 54378 RVA: 0x003EBB0A File Offset: 0x003E9D0A
		public bool IsVeneratedAnimal(Pawn pawn)
		{
			return this.IsVeneratedAnimal(pawn.def);
		}

		// Token: 0x0600D46B RID: 54379 RVA: 0x003EBB18 File Offset: 0x003E9D18
		public bool IsVeneratedAnimal(ThingDef thingDef)
		{
			return ModsConfig.IdeologyActive && this.VeneratedAnimals.Contains(thingDef);
		}

		// Token: 0x0600D46C RID: 54380 RVA: 0x003EBB30 File Offset: 0x003E9D30
		public IdeoWeaponDisposition GetDispositionForWeapon(ThingDef td)
		{
			if (!td.IsWeapon || td.weaponClasses.NullOrEmpty<WeaponClassDef>())
			{
				return IdeoWeaponDisposition.None;
			}
			foreach (Precept precept in this.precepts)
			{
				Precept_Weapon precept_Weapon = precept as Precept_Weapon;
				if (precept_Weapon != null)
				{
					IdeoWeaponDisposition dispositionForWeapon = precept_Weapon.GetDispositionForWeapon(td);
					if (dispositionForWeapon != IdeoWeaponDisposition.None)
					{
						return dispositionForWeapon;
					}
				}
			}
			return IdeoWeaponDisposition.None;
		}

		// Token: 0x0600D46D RID: 54381 RVA: 0x003EBBB0 File Offset: 0x003E9DB0
		public T GetFirstPreceptOfType<T>() where T : class
		{
			for (int i = 0; i < this.precepts.Count; i++)
			{
				T t = this.precepts[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x0600D46E RID: 54382 RVA: 0x003EBBFD File Offset: 0x003E9DFD
		public IEnumerable<T> GetAllPreceptsOfType<T>() where T : class
		{
			int num;
			for (int i = 0; i < this.precepts.Count; i = num + 1)
			{
				T t = this.precepts[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x0600D46F RID: 54383 RVA: 0x003EBC0D File Offset: 0x003E9E0D
		public bool HasPrecept(PreceptDef preceptDef)
		{
			return this.allPreceptDefs.Contains(preceptDef);
		}

		// Token: 0x0600D470 RID: 54384 RVA: 0x003EBC1C File Offset: 0x003E9E1C
		public int GetPreceptCountOfDef(PreceptDef preceptDef)
		{
			if (preceptDef == null || !this.allPreceptDefs.Contains(preceptDef))
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def == preceptDef)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600D471 RID: 54385 RVA: 0x003EBC70 File Offset: 0x003E9E70
		public Precept GetPrecept(PreceptDef preceptDef)
		{
			if (preceptDef == null || !this.allPreceptDefs.Contains(preceptDef))
			{
				return null;
			}
			for (int i = 0; i < this.precepts.Count; i++)
			{
				if (this.precepts[i].def == preceptDef)
				{
					return this.precepts[i];
				}
			}
			return null;
		}

		// Token: 0x0600D472 RID: 54386 RVA: 0x003EBCC8 File Offset: 0x003E9EC8
		public void SortMemesInDisplayOrder()
		{
			this.memes.SortBy((MemeDef x) => x.category == MemeCategory.Normal, (MemeDef x) => x.index);
		}

		// Token: 0x0600D473 RID: 54387 RVA: 0x003EBD20 File Offset: 0x003E9F20
		private void RecachePossibleSituationalThoughts()
		{
			this.cachedPossibleSituationalThoughts.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_SituationalThought preceptComp_SituationalThought = this.precepts[i].def.comps[j] as PreceptComp_SituationalThought;
					if (preceptComp_SituationalThought != null && !this.cachedPossibleSituationalThoughts.Contains(preceptComp_SituationalThought.thought))
					{
						this.cachedPossibleSituationalThoughts.Add(preceptComp_SituationalThought.thought);
					}
				}
			}
		}

		// Token: 0x0600D474 RID: 54388 RVA: 0x003EBDC4 File Offset: 0x003E9FC4
		private void RecachePossibleGoodwillSituations()
		{
			this.cachedPossibleGoodwillSituations.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_GoodwillSituation preceptComp_GoodwillSituation = this.precepts[i].def.comps[j] as PreceptComp_GoodwillSituation;
					if (preceptComp_GoodwillSituation != null && !this.cachedPossibleGoodwillSituations.Contains(preceptComp_GoodwillSituation.goodwillSituation))
					{
						this.cachedPossibleGoodwillSituations.Add(preceptComp_GoodwillSituation.goodwillSituation);
					}
				}
			}
		}

		// Token: 0x0600D475 RID: 54389 RVA: 0x003EBE64 File Offset: 0x003EA064
		public void RecachePossibleBuildings()
		{
			this.cachedPossibleBuildings.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Building precept_Building = this.precepts[i] as Precept_Building;
				if (precept_Building != null)
				{
					this.cachedPossibleBuildings.Add(precept_Building);
				}
			}
		}

		// Token: 0x0600D476 RID: 54390 RVA: 0x003EBEB4 File Offset: 0x003EA0B4
		public void RecachePossibleBuildables()
		{
			this.cachedPossibleBuildables.Clear();
			foreach (MemeDef memeDef in this.memes)
			{
				this.cachedPossibleBuildables.AddRange(memeDef.AllDesignatorBuildables);
			}
			foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
			{
				this.cachedPossibleBuildables.AddRange(thingStyleCategoryWithPriority.category.AllDesignatorBuildables);
			}
			foreach (Precept precept in this.precepts)
			{
				Precept_RitualSeat precept_RitualSeat = precept as Precept_RitualSeat;
				if (precept_RitualSeat != null)
				{
					this.cachedPossibleBuildables.Add(precept_RitualSeat.ThingDef);
				}
			}
		}

		// Token: 0x0600D477 RID: 54391 RVA: 0x003EBFC8 File Offset: 0x003EA1C8
		public void RecachePossibleMentalBreaks()
		{
			this.cachedPossibleMentalBreaks.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				for (int j = 0; j < this.precepts[i].def.comps.Count; j++)
				{
					PreceptComp_MentalBreak preceptComp_MentalBreak = this.precepts[i].def.comps[j] as PreceptComp_MentalBreak;
					if (preceptComp_MentalBreak != null)
					{
						this.cachedPossibleMentalBreaks.Add(preceptComp_MentalBreak.mentalBreakDef);
					}
				}
			}
		}

		// Token: 0x0600D478 RID: 54392 RVA: 0x003EC054 File Offset: 0x003EA254
		public void RecachePossibleRoles()
		{
			this.cachedPossibleRoles.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_Role precept_Role = this.precepts[i] as Precept_Role;
				if (precept_Role != null)
				{
					this.cachedPossibleRoles.Add(precept_Role);
				}
			}
		}

		// Token: 0x0600D479 RID: 54393 RVA: 0x003EC0A4 File Offset: 0x003EA2A4
		public void RecacheNeeds()
		{
			this.cachedEnabledNeeds.Clear();
			this.cachedDisabledNeeds.Clear();
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept precept = this.precepts[i];
				if (!precept.def.enablesNeeds.NullOrEmpty<NeedDef>())
				{
					this.cachedEnabledNeeds.AddRange(precept.def.enablesNeeds);
				}
				if (!precept.def.disablesNeeds.NullOrEmpty<NeedDef>())
				{
					this.cachedDisabledNeeds.AddRange(precept.def.disablesNeeds);
				}
			}
		}

		// Token: 0x0600D47A RID: 54394 RVA: 0x003EC13C File Offset: 0x003EA33C
		public void RegenerateAllPreceptNames()
		{
			foreach (Precept precept in this.precepts)
			{
				if (precept.UsesGeneratedName && !precept.nameLocked)
				{
					precept.RegenerateName();
				}
				precept.ClearTipCache();
			}
		}

		// Token: 0x0600D47B RID: 54395 RVA: 0x003EC1A4 File Offset: 0x003EA3A4
		public void RegenerateDescription(bool force = false)
		{
			if (this.description == null || !this.descriptionLocked)
			{
				IdeoDescriptionResult newDescription = this.GetNewDescription(force);
				this.description = newDescription.text;
				this.descriptionTemplate = newDescription.template;
			}
		}

		// Token: 0x0600D47C RID: 54396 RVA: 0x003EC1E4 File Offset: 0x003EA3E4
		public IdeoDescriptionResult GetNewDescription(bool force = false)
		{
			List<IdeoDescriptionMaker.PatternEntry> list = (from entry in this.memes.Where(delegate(MemeDef meme)
			{
				IdeoDescriptionMaker descriptionMaker = meme.descriptionMaker;
				return ((descriptionMaker != null) ? descriptionMaker.patterns : null) != null;
			}).SelectMany((MemeDef meme) => meme.descriptionMaker.patterns)
			group entry by entry.def into grp
			select grp.MaxBy((IdeoDescriptionMaker.PatternEntry entry) => entry.weight)).ToList<IdeoDescriptionMaker.PatternEntry>();
			if (!list.Any<IdeoDescriptionMaker.PatternEntry>())
			{
				if (ModsConfig.IdeologyActive && this.memes.Any<MemeDef>())
				{
					Log.Error("Memes provided no description patterns");
				}
				return new IdeoDescriptionResult
				{
					text = string.Empty
				};
			}
			IdeoStoryPatternDef def = list.RandomElementByWeight((IdeoDescriptionMaker.PatternEntry entry) => entry.weight).def;
			return IdeoDescriptionUtility.ResolveDescription(this, def, force);
		}

		// Token: 0x170020AB RID: 8363
		// (get) Token: 0x0600D47D RID: 54397 RVA: 0x003EC2F7 File Offset: 0x003EA4F7
		public int ColonistBelieverCountCached
		{
			get
			{
				if (this.colonistBelieverCountCached == -1)
				{
					return this.RecacheColonistBelieverCount();
				}
				return this.colonistBelieverCountCached;
			}
		}

		// Token: 0x0600D47E RID: 54398 RVA: 0x003EC310 File Offset: 0x003EA510
		public int RecacheColonistBelieverCount()
		{
			if (Current.ProgramState != ProgramState.Playing || Find.WindowStack.IsOpen<Dialog_ConfigureIdeo>())
			{
				return 0;
			}
			int num = this.colonistBelieverCountCached;
			int num2 = 0;
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep)
			{
				if (pawn.Ideo == this && !pawn.IsSlave && !pawn.IsQuestLodger())
				{
					num2++;
				}
			}
			this.colonistBelieverCountCached = num2;
			foreach (Precept precept in this.precepts)
			{
				Precept_Role precept_Role = precept as Precept_Role;
				if (precept_Role != null)
				{
					precept_Role.RecacheActivity();
				}
			}
			if (ModsConfig.IdeologyActive && Faction.OfPlayer.ideos.IsMinor(this))
			{
				if (this.colonistBelieverCountCached < Ideo.MinBelieversToEnableObligations && num != -1 && num >= Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleObligationsDeactivated".Translate(this.name), "LetterTitleObligationsDeactivatedTooFewBelievers".Translate(this.Named("IDEO"), Ideo.MinBelieversToEnableObligations), LetterDefOf.NeutralEvent, null, 0, true);
				}
				if (this.colonistBelieverCountCached >= Ideo.MinBelieversToEnableObligations && num != -1 && num < Ideo.MinBelieversToEnableObligations)
				{
					Find.LetterStack.ReceiveLetter("LetterTitleObligationsActivated".Translate(this.name), "LetterTitleObligationsActivatedEnoughBelievers".Translate(this.Named("IDEO"), Ideo.MinBelieversToEnableObligations), LetterDefOf.NeutralEvent, null, 0, true);
				}
			}
			return num2;
		}

		// Token: 0x0600D47F RID: 54399 RVA: 0x003EC4CC File Offset: 0x003EA6CC
		public Precept_Role GetRole(Pawn p)
		{
			foreach (Precept_Role precept_Role in this.RolesListForReading)
			{
				if (precept_Role.IsAssigned(p))
				{
					return precept_Role;
				}
			}
			return null;
		}

		// Token: 0x0600D480 RID: 54400 RVA: 0x003EC528 File Offset: 0x003EA728
		public StyleCategoryPair GetStyleAndCategoryFor(ThingDef thingDef)
		{
			Precept precept = null;
			for (int i = 0; i < this.precepts.Count; i++)
			{
				Precept_ThingDef precept_ThingDef = this.precepts[i] as Precept_ThingDef;
				if (precept_ThingDef != null && precept_ThingDef.ThingDef == thingDef)
				{
					precept = precept_ThingDef;
					break;
				}
			}
			return this.style.StyleForThingDef(thingDef, precept);
		}

		// Token: 0x0600D481 RID: 54401 RVA: 0x003EC57C File Offset: 0x003EA77C
		public ThingStyleDef GetStyleFor(ThingDef thingDef)
		{
			StyleCategoryPair styleAndCategoryFor = this.GetStyleAndCategoryFor(thingDef);
			if (styleAndCategoryFor == null)
			{
				return null;
			}
			return styleAndCategoryFor.styleDef;
		}

		// Token: 0x0600D482 RID: 54402 RVA: 0x003EC590 File Offset: 0x003EA790
		public StyleCategoryDef GetStyleCategoryFor(ThingDef thingDef)
		{
			StyleCategoryPair styleCategoryPair = this.style.StyleForThingDef(thingDef, null);
			if (styleCategoryPair == null)
			{
				return null;
			}
			return styleCategoryPair.category;
		}

		// Token: 0x0600D483 RID: 54403 RVA: 0x003EC5AC File Offset: 0x003EA7AC
		public void Notify_MemberChangedFaction(Pawn p, Faction oldFaction, Faction newFaction)
		{
			foreach (Precept precept in this.PreceptsListForReading)
			{
				precept.Notify_MemberChangedFaction(p, oldFaction, newFaction);
			}
			if (oldFaction == Faction.OfPlayer || newFaction == Faction.OfPlayer)
			{
				this.RecacheColonistBelieverCount();
			}
		}

		// Token: 0x0600D484 RID: 54404 RVA: 0x003EC618 File Offset: 0x003EA818
		public void Notify_NotPrimaryAnymore(Ideo newIdeo)
		{
			foreach (Precept precept in this.precepts)
			{
				precept.Notify_IdeoNotPrimaryAnymore(newIdeo);
			}
		}

		// Token: 0x0600D485 RID: 54405 RVA: 0x003EC66C File Offset: 0x003EA86C
		public bool HasMaxPreceptsForIssue(IssueDef issue)
		{
			if (!issue.allowMultiplePrecepts)
			{
				for (int i = 0; i < this.precepts.Count; i++)
				{
					if (this.precepts[i].def.issue == issue)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600D486 RID: 54406 RVA: 0x003EC6B3 File Offset: 0x003EA8B3
		public void MakeMemeberNamePluralDirty()
		{
			this.memberNamePluralCached = null;
		}

		// Token: 0x0600D487 RID: 54407 RVA: 0x003EC6BC File Offset: 0x003EA8BC
		public string GetUniqueLoadID()
		{
			return "Ideo_" + this.id.ToString();
		}

		// Token: 0x0600D488 RID: 54408 RVA: 0x003EC6D3 File Offset: 0x003EA8D3
		public override string ToString()
		{
			if (this.name != null)
			{
				return this.name;
			}
			return this.GetUniqueLoadID();
		}

		// Token: 0x0600D489 RID: 54409 RVA: 0x003EC6EC File Offset: 0x003EA8EC
		public void CopyTo(Ideo ideo)
		{
			ideo.foundation = IdeoGenerator.MakeFoundation(this.foundation.def);
			ideo.foundation.ideo = ideo;
			this.foundation.CopyTo(ideo.foundation);
			ideo.culture = this.culture;
			ideo.memes.Clear();
			ideo.memes.AddRange(this.memes);
			ideo.ClearPrecepts();
			foreach (Precept precept in this.precepts)
			{
				Precept precept2 = PreceptMaker.MakePrecept(precept.def);
				precept.CopyTo(precept2);
				precept2.ideo = ideo;
				ideo.precepts.Add(precept2);
			}
			ideo.RecachePrecepts();
			ideo.usedSymbols.Clear();
			ideo.usedSymbols.AddRange(ideo.usedSymbols);
			this.style.CopyTo(ideo.style);
			ideo.createdFromNoExpansionGame = this.createdFromNoExpansionGame;
			ideo.fluid = this.fluid;
			if (ideo.fluid)
			{
				ideo.development = new IdeoDevelopmentTracker(ideo);
				this.development.CopyTo(ideo.development);
			}
			ideo.initialPlayerIdeo = this.initialPlayerIdeo;
			ideo.name = this.name;
			ideo.adjective = this.adjective;
			ideo.memberName = this.memberName;
			ideo.iconDef = this.iconDef;
			ideo.colorDef = this.colorDef;
			ideo.primaryFactionColor = this.primaryFactionColor;
			ideo.leaderTitleMale = this.leaderTitleMale;
			ideo.leaderTitleFemale = this.leaderTitleFemale;
			ideo.usedSymbolPacks.Clear();
			ideo.usedSymbolPacks.AddRange(this.usedSymbolPacks);
			ideo.thingStyleCategories.Clear();
			ideo.thingStyleCategories.AddRange(this.thingStyleCategories);
			ideo.description = this.description;
			ideo.descriptionTemplate = this.descriptionTemplate;
			ideo.descriptionLocked = this.descriptionLocked;
			ideo.overriddenWorshipRoomLabel = this.overriddenWorshipRoomLabel;
			ideo.relicsCollected = this.relicsCollected;
		}

		// Token: 0x04009027 RID: 36903
		public static int MinBelieversToEnableObligations = 3;

		// Token: 0x04009028 RID: 36904
		public int id = -1;

		// Token: 0x04009029 RID: 36905
		public IdeoFoundation foundation;

		// Token: 0x0400902A RID: 36906
		public CultureDef culture;

		// Token: 0x0400902B RID: 36907
		public List<MemeDef> memes = new List<MemeDef>();

		// Token: 0x0400902C RID: 36908
		private List<Precept> precepts = new List<Precept>();

		// Token: 0x0400902D RID: 36909
		public List<string> usedSymbols = new List<string>();

		// Token: 0x0400902E RID: 36910
		public IdeoStyleTracker style;

		// Token: 0x0400902F RID: 36911
		public bool createdFromNoExpansionGame;

		// Token: 0x04009030 RID: 36912
		[LoadAlias("classic")]
		public bool classicExtraMode;

		// Token: 0x04009031 RID: 36913
		[LoadAlias("hiddenIdeoMode")]
		public bool classicMode;

		// Token: 0x04009032 RID: 36914
		public IdeoDevelopmentTracker development;

		// Token: 0x04009033 RID: 36915
		private bool fluid;

		// Token: 0x04009034 RID: 36916
		public bool hidden;

		// Token: 0x04009035 RID: 36917
		public bool solid;

		// Token: 0x04009036 RID: 36918
		public string fileName;

		// Token: 0x04009037 RID: 36919
		public bool initialPlayerIdeo;

		// Token: 0x04009038 RID: 36920
		public bool anyPreceptEdited;

		// Token: 0x04009039 RID: 36921
		public int currentCacheId = 1;

		// Token: 0x0400903A RID: 36922
		public string name;

		// Token: 0x0400903B RID: 36923
		public string adjective;

		// Token: 0x0400903C RID: 36924
		public string memberName;

		// Token: 0x0400903D RID: 36925
		public IdeoIconDef iconDef;

		// Token: 0x0400903E RID: 36926
		public ColorDef colorDef;

		// Token: 0x0400903F RID: 36927
		public Color? primaryFactionColor;

		// Token: 0x04009040 RID: 36928
		public string leaderTitleMale;

		// Token: 0x04009041 RID: 36929
		public string leaderTitleFemale;

		// Token: 0x04009042 RID: 36930
		public List<string> usedSymbolPacks = new List<string>();

		// Token: 0x04009043 RID: 36931
		public string description;

		// Token: 0x04009044 RID: 36932
		public string descriptionTemplate;

		// Token: 0x04009045 RID: 36933
		public bool descriptionLocked;

		// Token: 0x04009046 RID: 36934
		private string overriddenWorshipRoomLabel;

		// Token: 0x04009047 RID: 36935
		public bool relicsCollected;

		// Token: 0x04009048 RID: 36936
		private Texture2D icon;

		// Token: 0x04009049 RID: 36937
		public HashSet<ThoughtDef> cachedPossibleSituationalThoughts = new HashSet<ThoughtDef>();

		// Token: 0x0400904A RID: 36938
		public List<GoodwillSituationDef> cachedPossibleGoodwillSituations = new List<GoodwillSituationDef>();

		// Token: 0x0400904B RID: 36939
		public List<Precept_Role> cachedPossibleRoles = new List<Precept_Role>();

		// Token: 0x0400904C RID: 36940
		public HashSet<Precept_Building> cachedPossibleBuildings = new HashSet<Precept_Building>();

		// Token: 0x0400904D RID: 36941
		public HashSet<BuildableDef> cachedPossibleBuildables = new HashSet<BuildableDef>();

		// Token: 0x0400904E RID: 36942
		private HashSet<NeedDef> cachedEnabledNeeds = new HashSet<NeedDef>();

		// Token: 0x0400904F RID: 36943
		private HashSet<NeedDef> cachedDisabledNeeds = new HashSet<NeedDef>();

		// Token: 0x04009050 RID: 36944
		public List<ThingStyleCategoryWithPriority> thingStyleCategories = new List<ThingStyleCategoryWithPriority>();

		// Token: 0x04009051 RID: 36945
		private List<ThingDef> cachedVeneratedAnimals;

		// Token: 0x04009052 RID: 36946
		private List<XenotypeDef> cachedPreferredXenotypes;

		// Token: 0x04009053 RID: 36947
		private List<CustomXenotype> cachedPreferredCustomXenotypes;

		// Token: 0x04009054 RID: 36948
		private int requiredScarsCached;

		// Token: 0x04009055 RID: 36949
		private bool warnPlayerOnDesignateChopTree;

		// Token: 0x04009056 RID: 36950
		private bool warnPlayerOnDesignateMine;

		// Token: 0x04009057 RID: 36951
		private List<RitualObligationTargetFilter> cachedRitualTargetFilters;

		// Token: 0x04009058 RID: 36952
		private HashSet<PreceptDef> allPreceptDefs = new HashSet<PreceptDef>();

		// Token: 0x04009059 RID: 36953
		private string memberNamePluralCached;

		// Token: 0x0400905A RID: 36954
		public HashSet<MentalBreakDef> cachedPossibleMentalBreaks = new HashSet<MentalBreakDef>();

		// Token: 0x0400905B RID: 36955
		private static List<Pawn> tmpPlayerIdeoFollowers = new List<Pawn>();

		// Token: 0x0400905C RID: 36956
		private static List<Precept> tmpThoughtPrecepts = new List<Precept>();

		// Token: 0x0400905D RID: 36957
		private int colonistBelieverCountCached = -1;
	}
}
