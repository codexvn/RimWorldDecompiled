using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021D0 RID: 8656
	public class Zone_Growing : Zone, IPlantToGrowSettable
	{
		// Token: 0x17001CCD RID: 7373
		// (get) Token: 0x0600BD18 RID: 48408 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsMultiselectable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001CCE RID: 7374
		// (get) Token: 0x0600BD19 RID: 48409 RVA: 0x0036DD9F File Offset: 0x0036BF9F
		protected override Color NextZoneColor
		{
			get
			{
				return ZoneColorUtility.NextGrowingZoneColor();
			}
		}

		// Token: 0x17001CCF RID: 7375
		// (get) Token: 0x0600BD1A RID: 48410 RVA: 0x0036DDA6 File Offset: 0x0036BFA6
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return base.Cells;
			}
		}

		// Token: 0x17001CD0 RID: 7376
		// (get) Token: 0x0600BD1B RID: 48411 RVA: 0x0036DDAE File Offset: 0x0036BFAE
		public ThingDef PlantDefToGrow
		{
			get
			{
				if (this.plantDefToGrow != null)
				{
					return this.plantDefToGrow;
				}
				if (PollutionUtility.SettableEntirelyPolluted(this))
				{
					this.plantDefToGrow = ThingDefOf.Plant_Toxipotato;
				}
				else
				{
					this.plantDefToGrow = ThingDefOf.Plant_Potato;
				}
				return this.plantDefToGrow;
			}
		}

		// Token: 0x0600BD1C RID: 48412 RVA: 0x0036DDE5 File Offset: 0x0036BFE5
		public Zone_Growing()
		{
		}

		// Token: 0x0600BD1D RID: 48413 RVA: 0x0036DDFB File Offset: 0x0036BFFB
		public Zone_Growing(ZoneManager zoneManager) : base("GrowingZone".Translate(), zoneManager)
		{
		}

		// Token: 0x0600BD1E RID: 48414 RVA: 0x0036DE21 File Offset: 0x0036C021
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
			Scribe_Values.Look<bool>(ref this.allowSow, "allowSow", true, false);
			Scribe_Values.Look<bool>(ref this.allowCut, "allowCut", true, false);
		}

		// Token: 0x0600BD1F RID: 48415 RVA: 0x0036DE60 File Offset: 0x0036C060
		public void ContentsStatistics(out int totalPlants, out float averagePlantAgeTicks, out int oldestPlantAgeTicks, out float averagePlantGrowth, out float maxPlantGrowth)
		{
			averagePlantAgeTicks = 0f;
			totalPlants = 0;
			oldestPlantAgeTicks = 0;
			averagePlantGrowth = 0f;
			maxPlantGrowth = 0f;
			foreach (IntVec3 c in base.Cells)
			{
				foreach (Thing thing in c.GetThingList(base.Map))
				{
					if (thing.def == this.plantDefToGrow)
					{
						Plant plant = thing as Plant;
						if (plant != null)
						{
							totalPlants++;
							averagePlantAgeTicks += (float)plant.Age;
							oldestPlantAgeTicks = Mathf.Max(oldestPlantAgeTicks, plant.Age);
							averagePlantGrowth += plant.Growth;
							maxPlantGrowth = Mathf.Max(maxPlantGrowth, plant.Growth);
						}
					}
				}
			}
			averagePlantGrowth /= (float)totalPlants;
			averagePlantAgeTicks /= (float)totalPlants;
		}

		// Token: 0x0600BD20 RID: 48416 RVA: 0x0036DF78 File Offset: 0x0036C178
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
			stringBuilder.AppendLine();
			if (!base.Cells.NullOrEmpty<IntVec3>())
			{
				int num;
				float f;
				int numTicks;
				float f2;
				float f3;
				this.ContentsStatistics(out num, out f, out numTicks, out f2, out f3);
				if (num > 0)
				{
					string arg = Mathf.RoundToInt(f).ToStringTicksToPeriodVerbose(true, true);
					string arg2 = numTicks.ToStringTicksToPeriodVerbose(true, true);
					stringBuilder.AppendLine(string.Format("{0}: {1} {2}", "Contains".Translate().CapitalizeFirst(), num, Find.ActiveLanguageWorker.Pluralize(this.plantDefToGrow.label, num)));
					stringBuilder.AppendLine(string.Format("{0}: {1} ({2})", "AveragePlantAge".Translate().CapitalizeFirst(), arg, "PercentGrowth".Translate(f2.ToStringPercent())));
					stringBuilder.AppendLine(string.Format("{0}: {1} ({2})", "OldestPlantAge".Translate().CapitalizeFirst(), arg2, "PercentGrowth".Translate(f3.ToStringPercent())));
				}
				IntVec3 c = base.Cells.First<IntVec3>();
				if (c.UsesOutdoorTemperature(base.Map))
				{
					stringBuilder.AppendLine(string.Format("{0}: {1}", "OutdoorGrowingPeriod".Translate(), Zone_Growing.GrowingQuadrumsDescription(base.Map.Tile, this.plantDefToGrow)));
				}
				if (PlantUtility.GrowthSeasonNow(c, base.Map, this.plantDefToGrow))
				{
					stringBuilder.Append("GrowSeasonHereNow".Translate());
				}
				else
				{
					stringBuilder.Append("CannotGrowBadSeasonTemperature".Translate());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600BD21 RID: 48417 RVA: 0x0036E140 File Offset: 0x0036C340
		public static string GrowingQuadrumsDescription(PlanetTile tile, ThingDef plantDef = null)
		{
			float minTemp = (plantDef != null) ? plantDef.plant.minOptimalGrowthTemperature : 6f;
			float maxTemp = (plantDef != null) ? plantDef.plant.maxOptimalGrowthTemperature : 42f;
			List<Twelfth> list = GenTemperature.TwelfthsInAverageTemperatureRange(tile, minTemp, maxTemp);
			if (list.NullOrEmpty<Twelfth>())
			{
				return "NoGrowingPeriod".Translate();
			}
			if (list.Count == 12)
			{
				return "GrowYearRound".Translate();
			}
			return "PeriodDays".Translate(string.Format("{0}/{1}", list.Count * 5, 60)) + (" (" + QuadrumUtility.QuadrumsRangeLabel(list) + ")");
		}

		// Token: 0x0600BD22 RID: 48418 RVA: 0x0036E200 File Offset: 0x0036C400
		public override void AddCell(IntVec3 c)
		{
			base.AddCell(c);
			foreach (Thing thing in base.Map.thingGrid.ThingsListAt(c))
			{
				CompPlantPreventCutting compPlantPreventCutting;
				if (!thing.TryGetComp(out compPlantPreventCutting) || !compPlantPreventCutting.PreventCutting)
				{
					Designator_PlantsHarvestWood.PossiblyWarnPlayerImportantPlantDesignateCut(thing);
				}
			}
		}

		// Token: 0x0600BD23 RID: 48419 RVA: 0x0036E278 File Offset: 0x0036C478
		public override IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Command_Hide_ZoneGrow(this);
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowSow".Translate(),
				defaultDesc = "CommandAllowSowDesc".Translate(),
				hotKey = KeyBindingDefOf.Command_ItemForbid,
				icon = TexCommand.ForbidOff,
				isActive = (() => this.allowSow),
				toggleAction = delegate()
				{
					this.allowSow = !this.allowSow;
				}
			};
			yield return new Command_Toggle
			{
				defaultLabel = "CommandAllowCut".Translate(),
				defaultDesc = "CommandAllowCutDesc".Translate(),
				icon = Designator_PlantsCut.IconTex,
				isActive = (() => this.allowCut),
				toggleAction = delegate()
				{
					this.allowCut = !this.allowCut;
				}
			};
			yield break;
			yield break;
		}

		// Token: 0x0600BD24 RID: 48420 RVA: 0x0036E288 File Offset: 0x0036C488
		public override IEnumerable<Gizmo> GetZoneAddGizmos()
		{
			yield return DesignatorUtility.FindAllowedDesignator<Designator_ZoneAdd_Growing_Expand>();
			yield break;
		}

		// Token: 0x0600BD25 RID: 48421 RVA: 0x0036E291 File Offset: 0x0036C491
		public ThingDef GetPlantDefToGrow()
		{
			return this.PlantDefToGrow;
		}

		// Token: 0x0600BD26 RID: 48422 RVA: 0x0036E299 File Offset: 0x0036C499
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x0600BD27 RID: 48423 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CanAcceptSowNow()
		{
			return true;
		}

		// Token: 0x04008248 RID: 33352
		private ThingDef plantDefToGrow;

		// Token: 0x04008249 RID: 33353
		public bool allowSow = true;

		// Token: 0x0400824A RID: 33354
		public bool allowCut = true;
	}
}
