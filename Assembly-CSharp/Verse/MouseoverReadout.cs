using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000BCD RID: 3021
	public class MouseoverReadout
	{
		// Token: 0x17000BEA RID: 3050
		// (get) Token: 0x06004D55 RID: 19797 RVA: 0x001892E3 File Offset: 0x001874E3
		private bool ShouldShow
		{
			get
			{
				return Find.MainTabsRoot.OpenTab == null;
			}
		}

		// Token: 0x06004D56 RID: 19798 RVA: 0x001892F4 File Offset: 0x001874F4
		public void MouseoverReadoutOnGUI()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (!this.ShouldShow)
			{
				return;
			}
			GenUI.DrawTextWinterShadow(new Rect(256f, (float)(UI.screenHeight - 256), -256f, 256f));
			Text.Font = GameFont.Small;
			GUI.color = new Color(1f, 1f, 1f, 0.8f);
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(Find.CurrentMap))
			{
				return;
			}
			float num = 0f;
			if (intVec.Fogged(Find.CurrentMap))
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), "Undiscovered".Translate());
				GUI.color = Color.white;
				return;
			}
			Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), MouseoverUtility.GetGlowLabelByValue(Find.CurrentMap.glowGrid.GroundGlowAt(intVec, false, false)));
			num += 19f;
			if (Find.CurrentMap.Biome.inVacuum)
			{
				using (ProfilerBlock.Scope("vacuum"))
				{
					Rect rect = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
					float vacuum = intVec.GetVacuum(Find.CurrentMap);
					if (!Mathf.Approximately(vacuum, this.cachedVacuum))
					{
						this.cachedVacuumString = string.Format("{0} ({1})", "Vacuum".Translate().CapitalizeFirst(), vacuum.ToStringPercent("0"));
						this.cachedVacuum = vacuum;
					}
					Widgets.Label(rect, this.cachedVacuumString);
					num += 19f;
				}
			}
			Rect rect2 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
			TerrainDef terrain = intVec.GetTerrain(Find.CurrentMap);
			bool flag = intVec.IsPolluted(Find.CurrentMap);
			if (terrain != this.cachedTerrain || flag != this.cachedPolluted)
			{
				float fertility = intVec.GetFertility(Find.CurrentMap);
				string t = ((double)fertility > 0.0001) ? (" " + "FertShort".TranslateSimple() + " " + fertility.ToStringPercent()) : "";
				TaggedString t2 = flag ? "PollutedTerrain".Translate(terrain.label).CapitalizeFirst() : terrain.LabelCap;
				this.cachedTerrainString = t2 + ((terrain.passability != Traversability.Impassable) ? string.Format(" ({0})", "WalkSpeed".Translate(GenPath.SpeedPercentString((float)terrain.pathCost)) + t) : null);
				this.cachedTerrain = terrain;
				this.cachedPolluted = flag;
			}
			Widgets.Label(rect2, this.cachedTerrainString);
			num += 19f;
			Zone zone = intVec.GetZone(Find.CurrentMap);
			if (zone != null)
			{
				Rect rect3 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
				string label = zone.label;
				Widgets.Label(rect3, label);
				num += 19f;
			}
			float depth = Find.CurrentMap.snowGrid.GetDepth(intVec);
			if (depth > 0.03f)
			{
				Rect rect4 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
				WeatherBuildupCategory buildupCategory = WeatherBuildupUtility.GetBuildupCategory(depth);
				string label2 = "Snow".Translate() + " (" + WeatherBuildupUtility.GetSnowDescription(buildupCategory) + ")" + " (" + "WalkSpeed".Translate(GenPath.SpeedPercentString((float)WeatherBuildupUtility.MovementTicksAddOn(buildupCategory))) + ")";
				Widgets.Label(rect4, label2);
				num += 19f;
			}
			if (ModsConfig.OdysseyActive)
			{
				float depth2 = Find.CurrentMap.sandGrid.GetDepth(intVec);
				if (depth2 > 0.03f)
				{
					Rect rect5 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
					WeatherBuildupCategory buildupCategory2 = WeatherBuildupUtility.GetBuildupCategory(depth2);
					string label3 = "Sand".Translate() + " (" + WeatherBuildupUtility.GetSandDescription(buildupCategory2) + ")" + " (" + "WalkSpeed".Translate(GenPath.SpeedPercentString((float)WeatherBuildupUtility.MovementTicksAddOn(buildupCategory2))) + ")";
					Widgets.Label(rect5, label3);
					num += 19f;
				}
			}
			List<Thing> thingList = intVec.GetThingList(Find.CurrentMap);
			for (int i = 0; i < thingList.Count; i++)
			{
				Thing thing = thingList[i];
				CompSelectProxy compSelectProxy = thing.TryGetComp<CompSelectProxy>();
				if (compSelectProxy != null && compSelectProxy.thingToSelect != null)
				{
					thing = compSelectProxy.thingToSelect;
				}
				if (thing.def.category != ThingCategory.Mote)
				{
					Pawn pawn = thing as Pawn;
					if (pawn == null || !pawn.IsHiddenFromPlayer())
					{
						Rect rect6 = new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f);
						string labelMouseover = thing.LabelMouseover;
						Widgets.Label(rect6, labelMouseover);
						num += 19f;
					}
				}
			}
			RoofDef roof = intVec.GetRoof(Find.CurrentMap);
			if (roof != null)
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), roof.LabelCap);
				num += 19f;
			}
			if (Find.CurrentMap.gasGrid.AnyGasAt(intVec))
			{
				this.DrawGas(GasType.BlindSmoke, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.BlindSmoke), ref num);
				this.DrawGas(GasType.ToxGas, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.ToxGas), ref num);
				this.DrawGas(GasType.RotStink, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.RotStink), ref num);
				this.DrawGas(GasType.DeadlifeDust, Find.CurrentMap.gasGrid.DensityAt(intVec, GasType.DeadlifeDust), ref num);
			}
			WaterBody waterBody;
			if (ModsConfig.OdysseyActive && Find.CurrentMap.waterBodyTracker.TryGetWaterBodyAt(intVec, out waterBody) && waterBody.HasFish)
			{
				float population = waterBody.Population;
				float maxPopulation = waterBody.MaxPopulation;
				IEnumerable<ThingDef> commonFishIncludingExtras = waterBody.CommonFishIncludingExtras;
				IEnumerable<ThingDef> uncommonFish = waterBody.UncommonFish;
				IEnumerable<ThingDef> source = commonFishIncludingExtras.Concat(uncommonFish);
				TaggedString taggedString = "Fish".Translate().CapitalizeFirst();
				string text = (from x in source
				select x.label).ToCommaList(false, false).CapitalizeFirst();
				string text2 = string.Empty;
				GameCondition_GillRot activeCondition = Find.CurrentMap.gameConditionManager.GetActiveCondition<GameCondition_GillRot>();
				if (activeCondition != null && !activeCondition.HiddenByOtherCondition(Find.CurrentMap))
				{
					text2 = " (" + activeCondition.LabelCap + ")";
				}
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - num, 999f, 999f), string.Format("{0}: {1} ({2:F0}/{3:F0}){4}", new object[]
				{
					taggedString,
					text,
					population,
					maxPopulation,
					text2
				}));
				num += 19f;
			}
			GUI.color = Color.white;
		}

		// Token: 0x06004D57 RID: 19799 RVA: 0x00189B08 File Offset: 0x00187D08
		private void DrawGas(GasType gasType, byte density, ref float curYOffset)
		{
			if (density > 0)
			{
				Widgets.Label(new Rect(MouseoverReadout.BotLeft.x, (float)UI.screenHeight - MouseoverReadout.BotLeft.y - curYOffset, 999f, 999f), gasType.GetLabel().CapitalizeFirst() + " " + ((float)density / 255f).ToStringPercent("F0"));
				curYOffset += 19f;
			}
		}

		// Token: 0x040036F6 RID: 14070
		private TerrainDef cachedTerrain;

		// Token: 0x040036F7 RID: 14071
		private bool cachedPolluted;

		// Token: 0x040036F8 RID: 14072
		private float cachedVacuum;

		// Token: 0x040036F9 RID: 14073
		private string cachedTerrainString;

		// Token: 0x040036FA RID: 14074
		private string cachedVacuumString;

		// Token: 0x040036FB RID: 14075
		private const float YInterval = 19f;

		// Token: 0x040036FC RID: 14076
		private static readonly Vector2 BotLeft = new Vector2(15f, 65f);
	}
}
