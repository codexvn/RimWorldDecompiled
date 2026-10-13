using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002190 RID: 8592
	public abstract class SketchEntity : IExposable
	{
		// Token: 0x17001C75 RID: 7285
		// (get) Token: 0x0600BB84 RID: 48004
		public abstract string Label { get; }

		// Token: 0x17001C76 RID: 7286
		// (get) Token: 0x0600BB85 RID: 48005
		public abstract string LabelCap { get; }

		// Token: 0x17001C77 RID: 7287
		// (get) Token: 0x0600BB86 RID: 48006
		public abstract CellRect OccupiedRect { get; }

		// Token: 0x17001C78 RID: 7288
		// (get) Token: 0x0600BB87 RID: 48007
		public abstract float SpawnOrder { get; }

		// Token: 0x17001C79 RID: 7289
		// (get) Token: 0x0600BB88 RID: 48008 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool LostImportantReferences
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600BB89 RID: 48009
		public abstract void DrawGhost(IntVec3 at, Color color);

		// Token: 0x0600BB8A RID: 48010
		public abstract bool IsSameSpawned(IntVec3 at, Map map);

		// Token: 0x0600BB8B RID: 48011
		public abstract bool IsSameSpawnedOrBlueprintOrFrame(IntVec3 at, Map map);

		// Token: 0x0600BB8C RID: 48012
		public abstract bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x0600BB8D RID: 48013
		public abstract bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false);

		// Token: 0x0600BB8E RID: 48014
		public abstract bool CanBuildOnTerrain(IntVec3 at, Map map);

		// Token: 0x0600BB8F RID: 48015
		public abstract bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, TerrainDef defaultAffordanceTerrain = null);

		// Token: 0x0600BB90 RID: 48016
		public abstract bool SameForSubtracting(SketchEntity other);

		// Token: 0x0600BB91 RID: 48017 RVA: 0x00364364 File Offset: 0x00362564
		public bool SpawnNear(IntVec3 near, Map map, float radius, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, Func<SketchEntity, IntVec3, bool> validator = null, TerrainDef affordanceTerrain = null)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = near + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map) && (validator == null || validator(this, intVec)) && this.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, forceTerrainAffordance, spawnedThings, dormant, null))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB92 RID: 48018 RVA: 0x003643C8 File Offset: 0x003625C8
		public virtual SketchEntity DeepCopy()
		{
			SketchEntity sketchEntity = (SketchEntity)Activator.CreateInstance(base.GetType());
			sketchEntity.pos = this.pos;
			return sketchEntity;
		}

		// Token: 0x0600BB93 RID: 48019 RVA: 0x003643E8 File Offset: 0x003625E8
		public virtual void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.pos, "pos", default(IntVec3), false);
		}

		// Token: 0x04008134 RID: 33076
		public IntVec3 pos;
	}
}
