using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BD9 RID: 11225
	public class CompAnimalPenMarker : CompAutoCut, IRenameable
	{
		// Token: 0x1700275A RID: 10074
		// (get) Token: 0x0600FB16 RID: 64278 RVA: 0x0048F0F0 File Offset: 0x0048D2F0
		public CompProperties_AnimalPenMarker Props
		{
			get
			{
				return this.props as CompProperties_AnimalPenMarker;
			}
		}

		// Token: 0x1700275B RID: 10075
		// (get) Token: 0x0600FB17 RID: 64279 RVA: 0x0048F0FD File Offset: 0x0048D2FD
		public PenMarkerState PenState
		{
			get
			{
				ThingWithComps parent = this.parent;
				if (parent == null)
				{
					return null;
				}
				Map map = parent.Map;
				if (map == null)
				{
					return null;
				}
				return map.animalPenManager.GetPenMarkerState(this);
			}
		}

		// Token: 0x1700275C RID: 10076
		// (get) Token: 0x0600FB18 RID: 64280 RVA: 0x0048F121 File Offset: 0x0048D321
		public ThingFilter AnimalFilter
		{
			get
			{
				return this.animalFilter;
			}
		}

		// Token: 0x1700275D RID: 10077
		// (get) Token: 0x0600FB19 RID: 64281 RVA: 0x0048F129 File Offset: 0x0048D329
		public List<ThingDef> ForceDisplayedAnimalDefs
		{
			get
			{
				return this.forceDisplayedAnimalDefs;
			}
		}

		// Token: 0x1700275E RID: 10078
		// (get) Token: 0x0600FB1A RID: 64282 RVA: 0x0048F134 File Offset: 0x0048D334
		public override bool CanDesignatePlants
		{
			get
			{
				PenMarkerState penState = this.PenState;
				return penState != null && !penState.Unenclosed;
			}
		}

		// Token: 0x1700275F RID: 10079
		// (get) Token: 0x0600FB1B RID: 64283 RVA: 0x0048F156 File Offset: 0x0048D356
		// (set) Token: 0x0600FB1C RID: 64284 RVA: 0x0048F168 File Offset: 0x0048D368
		public string RenamableLabel
		{
			get
			{
				return this.label ?? this.BaseLabel;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17002760 RID: 10080
		// (get) Token: 0x0600FB1D RID: 64285 RVA: 0x0048F171 File Offset: 0x0048D371
		public string BaseLabel
		{
			get
			{
				return this.parent.def.label.CapitalizeFirst();
			}
		}

		// Token: 0x17002761 RID: 10081
		// (get) Token: 0x0600FB1E RID: 64286 RVA: 0x0048F188 File Offset: 0x0048D388
		public string InspectLabel
		{
			get
			{
				return this.RenamableLabel;
			}
		}

		// Token: 0x17002762 RID: 10082
		// (get) Token: 0x0600FB1F RID: 64287 RVA: 0x0048F190 File Offset: 0x0048D390
		public PenFoodCalculator PenFoodCalculator
		{
			get
			{
				if (this.cachedFoodCalculator == null)
				{
					this.cachedFoodCalculator = new PenFoodCalculator();
					this.cachedFoodCalculator_cachedAtTick = null;
				}
				if (this.cachedFoodCalculator_cachedAtTick != null)
				{
					int ticksGame = Find.TickManager.TicksGame;
					int? num = this.cachedFoodCalculator_cachedAtTick + 20;
					if (!(ticksGame > num.GetValueOrDefault() & num != null))
					{
						goto IL_95;
					}
				}
				this.cachedFoodCalculator.ResetAndProcessPen(this);
				this.cachedFoodCalculator_cachedAtTick = new int?(Find.TickManager.TicksGame);
				IL_95:
				return this.cachedFoodCalculator;
			}
		}

		// Token: 0x0600FB20 RID: 64288 RVA: 0x0048F238 File Offset: 0x0048D438
		public void AddForceDisplayedAnimal(ThingDef animalDef)
		{
			if (!this.forceDisplayedAnimalDefs.Contains(animalDef))
			{
				this.forceDisplayedAnimalDefs.Add(animalDef);
			}
		}

		// Token: 0x0600FB21 RID: 64289 RVA: 0x0048F254 File Offset: 0x0048D454
		public void RemoveForceDisplayedAnimal(ThingDef animalDef)
		{
			this.forceDisplayedAnimalDefs.Remove(animalDef);
		}

		// Token: 0x0600FB22 RID: 64290 RVA: 0x0048F263 File Offset: 0x0048D463
		public bool AcceptsToPen(Pawn animal)
		{
			return this.animalFilter.Allows(animal) && AnimalPenUtility.GetFixedAnimalFilter().Allows(animal);
		}

		// Token: 0x0600FB23 RID: 64291 RVA: 0x0048F280 File Offset: 0x0048D480
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship && this.label.NullOrEmpty())
			{
				this.label = (this.parent.Map.animalPenManager.MakeNewAnimalPenName() ?? "AnimalPenMarkerDefaultLabel".Translate(1));
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Cow);
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Goat);
				this.forceDisplayedAnimalDefs.Add(ThingDefOf.Chicken);
				base.AutoCutFilter.CopyAllowancesFrom(this.parent.Map.animalPenManager.GetDefaultAutoCutFilter());
				this.animalFilter.CopyAllowancesFrom(AnimalPenUtility.GetDefaultAnimalFilter());
			}
		}

		// Token: 0x0600FB24 RID: 64292 RVA: 0x0048F34E File Offset: 0x0048D54E
		public override IEnumerable<IntVec3> GetAutoCutCells()
		{
			foreach (Region region in this.PenState.DirectlyConnectedRegions)
			{
				foreach (IntVec3 intVec in region.Cells)
				{
					yield return intVec;
				}
				IEnumerator<IntVec3> enumerator2 = null;
			}
			List<Region>.Enumerator enumerator = default(List<Region>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600FB25 RID: 64293 RVA: 0x0048F35E File Offset: 0x0048D55E
		public override ThingFilter GetDefaultAutoCutFilter()
		{
			return this.parent.Map.animalPenManager.GetDefaultAutoCutFilter();
		}

		// Token: 0x0600FB26 RID: 64294 RVA: 0x0048F35E File Offset: 0x0048D55E
		public override ThingFilter GetFixedAutoCutFilter()
		{
			return this.parent.Map.animalPenManager.GetDefaultAutoCutFilter();
		}

		// Token: 0x0600FB27 RID: 64295 RVA: 0x0048F378 File Offset: 0x0048D578
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Deep.Look<ThingFilter>(ref this.animalFilter, "animalFilter", Array.Empty<object>());
			Scribe_Collections.Look<ThingDef>(ref this.forceDisplayedAnimalDefs, "forceDisplayedAnimalDefs", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.forceDisplayedAnimalDefs == null)
			{
				this.forceDisplayedAnimalDefs = new List<ThingDef>();
			}
		}

		// Token: 0x0600FB28 RID: 64296 RVA: 0x0048F3E4 File Offset: 0x0048D5E4
		public override string CompInspectStringExtra()
		{
			if (!this.parent.Spawned)
			{
				return string.Empty;
			}
			PenFoodCalculator penFoodCalculator = this.PenFoodCalculator;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.label).Append(": ").AppendLine(penFoodCalculator.PenSizeDescription());
			PenMarkerState penState = this.PenState;
			if (penState.Unenclosed)
			{
				stringBuilder.Append("PenUnenclosedLabel".Translate());
				if (penState.PassableDoors)
				{
					stringBuilder.Append(" (" + "PenOpenDoorLabel".Translate() + ")");
				}
				else
				{
					stringBuilder.Append(".");
				}
				stringBuilder.AppendLine();
			}
			else if (!penState.HasOutsideAccess)
			{
				stringBuilder.Append("NoExteriorAccess".Translate());
				stringBuilder.AppendLine();
			}
			bool flag = penFoodCalculator.SumNutritionConsumptionPerDay > penFoodCalculator.NutritionPerDayToday;
			string value = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.NutritionPerDayToday, false);
			string text = PenFoodCalculator.NutritionPerDayToString(penFoodCalculator.SumNutritionConsumptionPerDay, false);
			if (flag)
			{
				text = text.Colorize(ColorLibrary.RedReadable);
			}
			stringBuilder.Append("PenFoodTab_NaturalNutritionGrowthRate".Translate()).Append(": ").AppendLine(value);
			stringBuilder.Append("PenFoodTab_TotalNutritionConsumptionRate".Translate()).Append(": ").AppendLine(text);
			if (penFoodCalculator.sumStockpiledNutritionAvailableNow > 0f)
			{
				stringBuilder.Append("PenFoodTab_StockpileTotal".Translate()).Append(": ").AppendLine(PenFoodCalculator.NutritionToString(penFoodCalculator.sumStockpiledNutritionAvailableNow, false));
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x0400A4D5 RID: 42197
		private const int FoodCalculatorCacheTicks = 20;

		// Token: 0x0400A4D6 RID: 42198
		public string label = "";

		// Token: 0x0400A4D7 RID: 42199
		private ThingFilter animalFilter = new ThingFilter();

		// Token: 0x0400A4D8 RID: 42200
		private List<ThingDef> forceDisplayedAnimalDefs = new List<ThingDef>();

		// Token: 0x0400A4D9 RID: 42201
		private PenFoodCalculator cachedFoodCalculator;

		// Token: 0x0400A4DA RID: 42202
		private int? cachedFoodCalculator_cachedAtTick;
	}
}
