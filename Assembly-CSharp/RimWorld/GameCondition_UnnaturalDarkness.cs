using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4F RID: 7503
	public class GameCondition_UnnaturalDarkness : GameCondition_ForceWeather
	{
		// Token: 0x170019FB RID: 6651
		// (get) Token: 0x0600A820 RID: 43040 RVA: 0x00222DEB File Offset: 0x00220FEB
		public override int TransitionTicks
		{
			get
			{
				return 300;
			}
		}

		// Token: 0x0600A821 RID: 43041 RVA: 0x003021C1 File Offset: 0x003003C1
		public override void Init()
		{
			if (!ModLister.CheckAnomaly("Unnatural darkness"))
			{
				this.End();
				return;
			}
			base.Init();
		}

		// Token: 0x0600A822 RID: 43042 RVA: 0x003000A3 File Offset: 0x002FE2A3
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x0600A823 RID: 43043 RVA: 0x003016FF File Offset: 0x002FF8FF
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0f, GameCondition_NoSunlight.EclipseSkyColors, 1f, 0f));
		}

		// Token: 0x0600A824 RID: 43044 RVA: 0x003021DC File Offset: 0x003003DC
		public override WeatherDef ForcedWeather()
		{
			Map map = base.AffectedMaps.FirstOrDefault<Map>();
			if (map == null)
			{
				return null;
			}
			if (map.gameConditionManager.MapBrightness > 0.99f)
			{
				return WeatherDefOf.UnnaturalDarkness_Stage1;
			}
			return WeatherDefOf.UnnaturalDarkness_Stage2;
		}

		// Token: 0x0600A825 RID: 43045 RVA: 0x00302218 File Offset: 0x00300418
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			List<Map> affectedMaps = base.AffectedMaps;
			for (int i = 0; i < affectedMaps.Count; i++)
			{
				for (int j = 0; j < this.overlays.Count; j++)
				{
					this.overlays[j].TickOverlay(affectedMaps[i], 1f);
				}
				if (GenTicks.IsTickInterval(60))
				{
					foreach (Pawn pawn in affectedMaps[i].mapPawns.AllHumanlikeSpawned)
					{
						if (GameCondition_UnnaturalDarkness.AffectedByDarkness(pawn) && pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DarknessExposure, false) == null)
						{
							pawn.health.AddHediff(HediffDefOf.DarknessExposure, null, null, null);
						}
					}
				}
			}
		}

		// Token: 0x0600A826 RID: 43046 RVA: 0x00302310 File Offset: 0x00300510
		public static bool AffectedByDarkness(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && (pawn.Spawned && pawn.RaceProps.Humanlike && !pawn.Downed) && (pawn.IsColonistPlayerControlled || pawn.IsColonySubhumanPlayerControlled);
		}

		// Token: 0x0600A827 RID: 43047 RVA: 0x0030234C File Offset: 0x0030054C
		public override void End()
		{
			base.End();
			foreach (Map map in base.AffectedMaps)
			{
				map.weatherDecider.StartNextWeather();
			}
		}

		// Token: 0x0600A828 RID: 43048 RVA: 0x003023A8 File Offset: 0x003005A8
		public override void GameConditionDraw(Map map)
		{
			if (map.GameConditionManager.MapBrightness > 0.5f)
			{
				return;
			}
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x0600A829 RID: 43049 RVA: 0x003023F0 File Offset: 0x003005F0
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x0600A82A RID: 43050 RVA: 0x003023F8 File Offset: 0x003005F8
		public static bool InUnnaturalDarkness(Pawn p)
		{
			return p.SpawnedOrAnyParentSpawned && GameCondition_UnnaturalDarkness.UnnaturalDarknessAt(p.PositionHeld, p.MapHeld);
		}

		// Token: 0x0600A82B RID: 43051 RVA: 0x00302415 File Offset: 0x00300615
		public static bool UnnaturalDarknessOnMap(Map map)
		{
			return ModsConfig.AnomalyActive && map != null && map.gameConditionManager.MapBrightness <= 0.01f && map.gameConditionManager.ConditionIsActive(GameConditionDefOf.UnnaturalDarkness);
		}

		// Token: 0x0600A82C RID: 43052 RVA: 0x00302450 File Offset: 0x00300650
		public static bool UnnaturalDarknessAt(IntVec3 cell, Map map)
		{
			if (!GameCondition_UnnaturalDarkness.UnnaturalDarknessOnMap(map))
			{
				return false;
			}
			Building_Door door = cell.GetDoor(map);
			if (door != null)
			{
				float num = 0f;
				int num2 = 0;
				foreach (IntVec3 c in door.OccupiedRect().ExpandedBy(1).EdgeCellsNoCorners)
				{
					if (c.InBounds(map) && !c.Filled(map))
					{
						num += map.glowGrid.GroundGlowAt(c, false, false);
						num2++;
					}
				}
				if (num2 > 0)
				{
					return num / (float)num2 <= 0f;
				}
			}
			return map.glowGrid.GroundGlowAt(cell, false, false) <= 0f;
		}

		// Token: 0x0600A82D RID: 43053 RVA: 0x00302520 File Offset: 0x00300720
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.anyColonistAttacked, "anyColonistAttacked", false, false);
		}

		// Token: 0x04007455 RID: 29781
		public bool anyColonistAttacked;

		// Token: 0x04007456 RID: 29782
		private List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_UnnaturalDarkness()
		};
	}
}
