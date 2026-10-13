using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020003AB RID: 939
	public class AnimalPenManager
	{
		// Token: 0x06001811 RID: 6161 RVA: 0x00079F74 File Offset: 0x00078174
		public AnimalPenManager(Map map)
		{
			this.map = map;
			map.events.BuildingSpawned += this.Notify_BuildingSpawned;
			map.events.BuildingDespawned += this.Notify_BuildingDespawned;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x00079FE4 File Offset: 0x000781E4
		public void RebuildAllPens()
		{
			this.ForceRebuildPens();
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x00079FEC File Offset: 0x000781EC
		public PenMarkerState GetPenMarkerState(CompAnimalPenMarker marker)
		{
			this.RebuildIfDirty();
			return this.penMarkers[marker];
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0007A000 File Offset: 0x00078200
		public CompAnimalPenMarker GetPenNamed(string name)
		{
			return this.penMarkers.Keys.FirstOrDefault((CompAnimalPenMarker marker) => marker.label == name);
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0007A038 File Offset: 0x00078238
		public ThingFilter GetFixedAutoCutFilter()
		{
			if (this.cachedFixedAutoCutFilter == null)
			{
				this.cachedFixedAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.map.wildPlantSpawner.AllWildPlants)
				{
					if (thingDef.plant.allowAutoCut)
					{
						this.cachedFixedAutoCutFilter.SetAllow(thingDef, true);
					}
				}
				this.cachedFixedAutoCutFilter.SetAllow(ThingCategoryDefOf.Stumps, true, null, null);
			}
			return this.cachedFixedAutoCutFilter;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0007A0D4 File Offset: 0x000782D4
		public ThingFilter GetDefaultAutoCutFilter()
		{
			if (this.cachedDefaultAutoCutFilter == null)
			{
				this.cachedDefaultAutoCutFilter = new ThingFilter();
				ThingDef plant_Grass = ThingDefOf.Plant_Grass;
				float num = plant_Grass.ingestible.CachedNutrition / plant_Grass.plant.growDays * 0.5f;
				foreach (ThingDef thingDef in this.GetFixedAutoCutFilter().AllowedThingDefs)
				{
					if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thingDef) || (thingDef.ingestible.CachedNutrition / thingDef.plant.growDays < num && (thingDef.plant.harvestedThingDef == null || !thingDef.plant.harvestedThingDef.IsIngestible)))
					{
						this.cachedDefaultAutoCutFilter.SetAllow(thingDef, true);
					}
				}
			}
			return this.cachedDefaultAutoCutFilter;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0007A1B0 File Offset: 0x000783B0
		private void Notify_BuildingSpawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0007A1B0 File Offset: 0x000783B0
		private void Notify_BuildingDespawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0007A1B9 File Offset: 0x000783B9
		private void RebuildIfDirty()
		{
			if (this.dirty)
			{
				this.ForceRebuildPens();
			}
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0007A1CC File Offset: 0x000783CC
		private void ForceRebuildPens()
		{
			this.dirty = false;
			this.penMarkers.Clear();
			foreach (Building thing in this.map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker = thing.TryGetComp<CompAnimalPenMarker>();
				this.penMarkers.Add(compAnimalPenMarker, new PenMarkerState(compAnimalPenMarker));
			}
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0007A24C File Offset: 0x0007844C
		public string MakeNewAnimalPenName()
		{
			this.existingPenNames.Clear();
			this.existingPenNames.AddRange(from marker in this.penMarkers.Keys
			select marker.label);
			int num = 1;
			string text;
			for (;;)
			{
				text = "AnimalPenMarkerDefaultLabel".Translate(num);
				if (!this.existingPenNames.Contains(text))
				{
					break;
				}
				num++;
			}
			this.existingPenNames.Clear();
			return text;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0007A2D8 File Offset: 0x000784D8
		public void DrawPlacingMouseAttachments(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (((thingDef != null) ? thingDef.CompDefFor<CompAnimalPenMarker>() : null) == null)
			{
				return;
			}
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(this.map))
			{
				return;
			}
			if (this.cached_placingPenFoodCalculator_forPosition == null || intVec != this.cached_placingPenFoodCalculator_forPosition)
			{
				this.cached_placingPenFoodCalculator.ResetAndProcessPen(intVec, this.map, true);
				this.cached_placingPenFoodCalculator_forPosition = new IntVec3?(intVec);
			}
			AnimalPenGUI.DrawPlacingMouseAttachments(intVec, this.map, this.cached_placingPenFoodCalculator);
		}

		// Token: 0x04001587 RID: 5511
		private readonly Map map;

		// Token: 0x04001588 RID: 5512
		private readonly Dictionary<CompAnimalPenMarker, PenMarkerState> penMarkers = new Dictionary<CompAnimalPenMarker, PenMarkerState>();

		// Token: 0x04001589 RID: 5513
		private bool dirty = true;

		// Token: 0x0400158A RID: 5514
		private ThingFilter cachedDefaultAutoCutFilter;

		// Token: 0x0400158B RID: 5515
		private ThingFilter cachedFixedAutoCutFilter;

		// Token: 0x0400158C RID: 5516
		private HashSet<string> existingPenNames = new HashSet<string>();

		// Token: 0x0400158D RID: 5517
		private readonly PenFoodCalculator cached_placingPenFoodCalculator = new PenFoodCalculator();

		// Token: 0x0400158E RID: 5518
		private IntVec3? cached_placingPenFoodCalculator_forPosition;
	}
}
