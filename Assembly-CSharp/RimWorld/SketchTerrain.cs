using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002191 RID: 8593
	public class SketchTerrain : SketchBuildable
	{
		// Token: 0x17001C7A RID: 7290
		// (get) Token: 0x0600BB95 RID: 48021 RVA: 0x0036440F File Offset: 0x0036260F
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001C7B RID: 7291
		// (get) Token: 0x0600BB96 RID: 48022 RVA: 0x00364417 File Offset: 0x00362617
		public override ThingDef Stuff
		{
			get
			{
				return this.stuffForComparingSimilar;
			}
		}

		// Token: 0x17001C7C RID: 7292
		// (get) Token: 0x0600BB97 RID: 48023 RVA: 0x0004FC72 File Offset: 0x0004DE72
		public override float SpawnOrder
		{
			get
			{
				return 1f;
			}
		}

		// Token: 0x17001C7D RID: 7293
		// (get) Token: 0x0600BB98 RID: 48024 RVA: 0x0036441F File Offset: 0x0036261F
		public override CellRect OccupiedRect
		{
			get
			{
				if (this.cachedOccupiedRectPos != this.pos)
				{
					this.cachedOccupiedRect = CellRect.SingleCell(this.pos);
					this.cachedOccupiedRectPos = this.pos;
				}
				return this.cachedOccupiedRect;
			}
		}

		// Token: 0x17001C7E RID: 7294
		// (get) Token: 0x0600BB99 RID: 48025 RVA: 0x00364458 File Offset: 0x00362658
		public override string Label
		{
			get
			{
				if (this.def.designatorDropdown == null || this.def.designatorDropdown.label.NullOrEmpty() || !this.treatSimilarAsSame)
				{
					return base.Label;
				}
				return this.def.designatorDropdown.label;
			}
		}

		// Token: 0x17001C7F RID: 7295
		// (get) Token: 0x0600BB9A RID: 48026 RVA: 0x003644A8 File Offset: 0x003626A8
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x0600BB9B RID: 48027 RVA: 0x003644B8 File Offset: 0x003626B8
		public override void DrawGhost(IntVec3 at, Color color)
		{
			ThingDef blueprintDef = this.def.blueprintDef;
			GraphicDatabase.Get(blueprintDef.graphic.GetType(), blueprintDef.graphic.path, blueprintDef.graphic.Shader, blueprintDef.graphic.drawSize, color, Color.white, blueprintDef.graphicData, null, null).DrawFromDef(at.ToVector3ShiftedWithAltitude(AltitudeLayer.Blueprint), Rot4.North, this.def.blueprintDef, 0f);
		}

		// Token: 0x0600BB9C RID: 48028 RVA: 0x00364533 File Offset: 0x00362733
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return at.InBounds(map) && this.IsSameOrSimilar(at.GetTerrain(map));
		}

		// Token: 0x0600BB9D RID: 48029 RVA: 0x00364550 File Offset: 0x00362750
		public bool IsSameOrSimilar(BuildableDef other)
		{
			if (other == null)
			{
				return false;
			}
			if (!this.treatSimilarAsSame)
			{
				return other == this.def;
			}
			if (this.def.designatorDropdown == null && other.designatorDropdown == null && other.BuildableByPlayer)
			{
				return true;
			}
			if (this.def.designatorDropdown == null || other.designatorDropdown == null)
			{
				return other == this.def;
			}
			return other.designatorDropdown == this.def.designatorDropdown;
		}

		// Token: 0x0600BB9E RID: 48030 RVA: 0x003645C8 File Offset: 0x003627C8
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].Position == at && this.IsSameOrSimilar(thingList[i].def.entityDefToBuild))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x0600BB9F RID: 48031 RVA: 0x00364630 File Offset: 0x00362830
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, Rot4.North, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, Rot4.North, map, wipeIfCollides, thingToIgnore, null, null, false, false, false).Accepted;
		}

		// Token: 0x0600BBA0 RID: 48032 RVA: 0x00364696 File Offset: 0x00362896
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x0600BBA1 RID: 48033 RVA: 0x003646BC File Offset: 0x003628BC
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, Rot4.North, null, null);
		}

		// Token: 0x0600BBA2 RID: 48034 RVA: 0x003646D4 File Offset: 0x003628D4
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, TerrainDef defaultAffordanceTerrain = null)
		{
			if (!at.InBounds(map))
			{
				return false;
			}
			if (forceTerrainAffordance && !this.CanBuildOnTerrain(at, map))
			{
				base.ForceTerrainAffordance(at, Rot4.North, map, defaultAffordanceTerrain);
			}
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			if (spawnMode != Sketch.SpawnMode.Blueprint)
			{
				if (spawnMode != Sketch.SpawnMode.Normal)
				{
					throw new NotImplementedException("Spawn mode " + spawnMode.ToString() + " not implemented!");
				}
				if (this.def.isFoundation)
				{
					map.terrainGrid.SetFoundation(at, this.GetDefFromStuff());
				}
				else if (this.def.temporary)
				{
					map.terrainGrid.SetTempTerrain(at, this.GetDefFromStuff());
				}
				else
				{
					map.terrainGrid.SetTerrain(at, this.GetDefFromStuff());
				}
			}
			else
			{
				GenConstruct.PlaceBlueprintForBuild(this.GetDefFromStuff(), at, map, Rot4.North, faction, null, null, null, false);
			}
			return true;
		}

		// Token: 0x0600BBA3 RID: 48035 RVA: 0x003647B8 File Offset: 0x003629B8
		private TerrainDef GetDefFromStuff()
		{
			if (this.stuffForComparingSimilar == null)
			{
				return this.def;
			}
			foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
			{
				if (this.IsSameOrSimilar(terrainDef) && !terrainDef.CostList.NullOrEmpty<ThingDefCountClass>() && terrainDef.CostList[0].thingDef == this.stuffForComparingSimilar)
				{
					return terrainDef;
				}
			}
			return this.def;
		}

		// Token: 0x0600BBA4 RID: 48036 RVA: 0x00364848 File Offset: 0x00362A48
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchTerrain sketchTerrain = other as SketchTerrain;
			return sketchTerrain != null && (sketchTerrain == this || (this.IsSameOrSimilar(sketchTerrain.Buildable) && this.pos == sketchTerrain.pos));
		}

		// Token: 0x0600BBA5 RID: 48037 RVA: 0x00364888 File Offset: 0x00362A88
		public override SketchEntity DeepCopy()
		{
			SketchTerrain sketchTerrain = (SketchTerrain)base.DeepCopy();
			sketchTerrain.def = this.def;
			sketchTerrain.stuffForComparingSimilar = this.stuffForComparingSimilar;
			sketchTerrain.treatSimilarAsSame = this.treatSimilarAsSame;
			return sketchTerrain;
		}

		// Token: 0x0600BBA6 RID: 48038 RVA: 0x003648B9 File Offset: 0x00362AB9
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<TerrainDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuffForComparingSimilar, "stuff");
			Scribe_Values.Look<bool>(ref this.treatSimilarAsSame, "treatSimilarAsSame", false, false);
		}

		// Token: 0x04008135 RID: 33077
		public TerrainDef def;

		// Token: 0x04008136 RID: 33078
		public ThingDef stuffForComparingSimilar;

		// Token: 0x04008137 RID: 33079
		public bool treatSimilarAsSame;

		// Token: 0x04008138 RID: 33080
		private CellRect cachedOccupiedRect = CellRect.Empty;

		// Token: 0x04008139 RID: 33081
		private IntVec3 cachedOccupiedRectPos = IntVec3.Invalid;
	}
}
