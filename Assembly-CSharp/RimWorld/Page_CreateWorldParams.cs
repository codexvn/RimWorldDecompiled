using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Profile;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003289 RID: 12937
	public class Page_CreateWorldParams : Page
	{
		// Token: 0x17002EFF RID: 12031
		// (get) Token: 0x0601232E RID: 74542 RVA: 0x005579A7 File Offset: 0x00555BA7
		public override string PageTitle
		{
			get
			{
				return "CreateWorld".Translate();
			}
		}

		// Token: 0x0601232F RID: 74543 RVA: 0x005579B8 File Offset: 0x00555BB8
		public override void PreOpen()
		{
			base.PreOpen();
			if (!this.initialized)
			{
				this.Reset();
				this.initialized = true;
			}
		}

		// Token: 0x06012330 RID: 74544 RVA: 0x005579D5 File Offset: 0x00555BD5
		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-CreateWorldParams");
		}

		// Token: 0x06012331 RID: 74545 RVA: 0x005579EC File Offset: 0x00555BEC
		public void Reset()
		{
			this.seedString = GenText.RandomSeedString();
			this.planetCoverage = ((!Prefs.DevMode || !UnityData.isEditor) ? (ModsConfig.OdysseyActive ? 0.5f : 0.3f) : 0.05f);
			this.rainfall = OverallRainfall.Normal;
			this.temperature = OverallTemperature.Normal;
			this.population = OverallPopulation.Normal;
			this.landmarkDensity = LandmarkDensity.Normal;
			this.pollution = (ModsConfig.BiotechActive ? 0.05f : 0f);
			this.ResetFactionCounts();
		}

		// Token: 0x06012332 RID: 74546 RVA: 0x00557A70 File Offset: 0x00555C70
		private void ResetFactionCounts()
		{
			this.factions = new List<FactionDef>();
			foreach (FactionDef factionDef in FactionGenerator.ConfigurableFactions)
			{
				if (factionDef.startingCountAtWorldCreation > 0)
				{
					for (int i = 0; i < factionDef.startingCountAtWorldCreation; i++)
					{
						this.factions.Add(factionDef);
					}
				}
			}
			using (IEnumerator<FactionDef> enumerator = FactionGenerator.ConfigurableFactions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FactionDef faction = enumerator.Current;
					if (faction.replacesFaction != null)
					{
						this.factions.RemoveAll((FactionDef x) => x == faction.replacesFaction);
					}
				}
			}
			this.initialFactions = new List<FactionDef>();
			this.initialFactions.AddRange(this.factions);
		}

		// Token: 0x06012333 RID: 74547 RVA: 0x00557B68 File Offset: 0x00555D68
		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 0f, false);
			float num = (mainRect.width - this.Margin) * 0.5f;
			Rect rect2 = new Rect(mainRect.x, mainRect.y, num, mainRect.height);
			Widgets.BeginGroup(rect2);
			Text.Font = GameFont.Small;
			float num2 = 0f;
			float width = rect2.width - 200f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "WorldSeed".Translate());
			Rect rect3 = new Rect(200f, num2, width, 30f);
			this.seedString = Widgets.TextField(rect3, this.seedString);
			num2 += 40f;
			if (Widgets.ButtonText(new Rect(200f, num2, width, 30f), "RandomizeSeed".Translate(), true, true, true, null))
			{
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
				this.seedString = GenText.RandomSeedString();
			}
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetCoverage".Translate());
			Rect rect4 = new Rect(200f, num2, width, 30f);
			if (Widgets.ButtonText(rect4, this.planetCoverage.ToStringPercent(), true, true, true, null))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				float[] array = Prefs.DevMode ? Page_CreateWorldParams.PlanetCoveragesDev : Page_CreateWorldParams.PlanetCoverages;
				for (int i = 0; i < array.Length; i++)
				{
					float coverage = array[i];
					string text = coverage.ToStringPercent();
					if (coverage <= 0.1f)
					{
						text += " (dev)";
					}
					FloatMenuOption item = new FloatMenuOption(text, delegate()
					{
						if (this.planetCoverage != coverage)
						{
							this.planetCoverage = coverage;
							if (this.planetCoverage == 1f)
							{
								Messages.Message("MessageMaxPlanetCoveragePerformanceWarning".Translate(), MessageTypeDefOf.CautionInput, false);
							}
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			TooltipHandler.TipRegionByKey(new Rect(0f, num2, rect4.xMax, rect4.height), "PlanetCoverageTip");
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetRainfall".Translate());
			Rect rect5 = new Rect(200f, num2, width, 30f);
			this.rainfall = (OverallRainfall)Mathf.RoundToInt(Widgets.HorizontalSlider(rect5, (float)this.rainfall, 0f, (float)(OverallRainfallUtility.EnumValuesCount - 1), true, "PlanetRainfall_Normal".Translate(), "PlanetRainfall_Low".Translate(), "PlanetRainfall_High".Translate(), 1f));
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetTemperature".Translate());
			Rect rect6 = new Rect(200f, num2, width, 30f);
			this.temperature = (OverallTemperature)Mathf.RoundToInt(Widgets.HorizontalSlider(rect6, (float)this.temperature, 0f, (float)(OverallTemperatureUtility.EnumValuesCount - 1), true, "PlanetTemperature_Normal".Translate(), "PlanetTemperature_Low".Translate(), "PlanetTemperature_High".Translate(), 1f));
			num2 += 40f;
			Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetPopulation".Translate());
			Rect rect7 = new Rect(200f, num2, width, 30f);
			this.population = (OverallPopulation)Mathf.RoundToInt(Widgets.HorizontalSlider(rect7, (float)this.population, 0f, (float)(OverallPopulationUtility.EnumValuesCount - 1), true, "PlanetPopulation_Normal".Translate(), "PlanetPopulation_Low".Translate(), "PlanetPopulation_High".Translate(), 1f));
			if (ModsConfig.OdysseyActive)
			{
				num2 += 40f;
				Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetLandmarkDensity".Translate());
				Rect rect8 = new Rect(200f, num2, width, 30f);
				this.landmarkDensity = (LandmarkDensity)Mathf.RoundToInt(Widgets.HorizontalSlider(rect8, (float)this.landmarkDensity, 0f, (float)(LandmarkDensityUtility.EnumValuesCount - 1), true, "PlanetLandmarkDensity_Normal".Translate(), "PlanetLandmarkDensity_Low".Translate(), "PlanetLandmarkDensity_High".Translate(), 1f));
			}
			if (ModsConfig.BiotechActive)
			{
				num2 += 40f;
				Widgets.Label(new Rect(0f, num2, 200f, 30f), "PlanetPollution".Translate());
				Rect rect9 = new Rect(200f, num2, width, 30f);
				this.pollution = Widgets.HorizontalSlider(rect9, this.pollution, 0f, 1f, true, this.pollution.ToStringPercent(), null, null, 0.05f);
			}
			if (!TutorSystem.TutorialMode)
			{
				num2 += 40f;
				Widgets.Label(new Rect(0f, num2, 200f, 30f), "AdvancedSettings".Translate());
				if (Widgets.ButtonText(new Rect(200f, num2, width, 30f), "Edit".Translate() + "...", true, true, true, null))
				{
					Find.WindowStack.Add(new Dialog_AdvancedGameConfig());
				}
			}
			Widgets.EndGroup();
			Rect rect10 = new Rect(mainRect.x + mainRect.xMax - num, mainRect.y, num, mainRect.height);
			bool isDefaultFactionCounts = this.factions.SetsEqual(this.initialFactions);
			WorldFactionsUIUtility.DoWindowContents(rect10, this.factions, isDefaultFactionCounts);
			float y = rect.yMax - 38f;
			float x = mainRect.center.x;
			Rect rect11 = new Rect(x - Page.BottomButSize.x - 8.5f, y, Page.BottomButSize.x, Page.BottomButSize.y);
			if (Widgets.ButtonText(rect11, "ResetAll".Translate(), true, true, true, null))
			{
				this.Reset();
			}
			rect11.x = x + 8.5f;
			if (Widgets.ButtonText(rect11, "ResetFactions".Translate(), true, true, true, null))
			{
				this.ResetFactionCounts();
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			base.DoBottomButtons(rect, "WorldGenerate".Translate(), null, null, true, true);
		}

		// Token: 0x06012334 RID: 74548 RVA: 0x0055825A File Offset: 0x0055645A
		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			LongEventHandler.QueueLongEvent(delegate()
			{
				Find.GameInitData.ResetWorldRelatedMapInitData();
				Current.Game.World = WorldGenerator.GenerateWorld(this.planetCoverage, this.seedString, this.rainfall, this.temperature, this.population, this.landmarkDensity, this.factions, this.pollution);
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					if (this.next != null)
					{
						Find.WindowStack.Add(this.next);
					}
					MemoryUtility.UnloadUnusedUnityAssets();
					Find.World.renderer.RegenerateAllLayersNow();
					this.Close(true);
				});
			}, "GeneratingWorld", true, null, true, false, null);
			return false;
		}

		// Token: 0x0400BD45 RID: 48453
		private bool initialized;

		// Token: 0x0400BD46 RID: 48454
		private string seedString;

		// Token: 0x0400BD47 RID: 48455
		private float planetCoverage;

		// Token: 0x0400BD48 RID: 48456
		private OverallRainfall rainfall;

		// Token: 0x0400BD49 RID: 48457
		private OverallTemperature temperature;

		// Token: 0x0400BD4A RID: 48458
		private OverallPopulation population;

		// Token: 0x0400BD4B RID: 48459
		private LandmarkDensity landmarkDensity;

		// Token: 0x0400BD4C RID: 48460
		public float pollution;

		// Token: 0x0400BD4D RID: 48461
		private List<FactionDef> factions;

		// Token: 0x0400BD4E RID: 48462
		private List<FactionDef> initialFactions;

		// Token: 0x0400BD4F RID: 48463
		private static readonly float[] PlanetCoverages = new float[]
		{
			0.3f,
			0.5f,
			1f
		};

		// Token: 0x0400BD50 RID: 48464
		private static readonly float[] PlanetCoveragesDev = new float[]
		{
			0.3f,
			0.5f,
			1f,
			0.05f
		};

		// Token: 0x0400BD51 RID: 48465
		private const float LabelWidth = 200f;
	}
}
