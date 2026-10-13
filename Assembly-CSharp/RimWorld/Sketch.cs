using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200218A RID: 8586
	public class Sketch : IExposable
	{
		// Token: 0x17001C67 RID: 7271
		// (get) Token: 0x0600BB2F RID: 47919 RVA: 0x00361E04 File Offset: 0x00360004
		public List<SketchEntity> Entities
		{
			get
			{
				return this.entities;
			}
		}

		// Token: 0x17001C68 RID: 7272
		// (get) Token: 0x0600BB30 RID: 47920 RVA: 0x00361E0C File Offset: 0x0036000C
		public List<SketchThing> Things
		{
			get
			{
				return this.cachedThings;
			}
		}

		// Token: 0x17001C69 RID: 7273
		// (get) Token: 0x0600BB31 RID: 47921 RVA: 0x00361E14 File Offset: 0x00360014
		public List<SketchTerrain> Terrain
		{
			get
			{
				return this.cachedTerrain;
			}
		}

		// Token: 0x17001C6A RID: 7274
		// (get) Token: 0x0600BB32 RID: 47922 RVA: 0x00361E1C File Offset: 0x0036001C
		public List<SketchBuildable> Buildables
		{
			get
			{
				return this.cachedBuildables;
			}
		}

		// Token: 0x17001C6B RID: 7275
		// (get) Token: 0x0600BB33 RID: 47923 RVA: 0x00361E24 File Offset: 0x00360024
		public CellRect OccupiedRect
		{
			get
			{
				if (this.occupiedRectDirty)
				{
					this.cachedOccupiedRect = CellRect.Empty;
					bool flag = false;
					for (int i = 0; i < this.entities.Count; i++)
					{
						if (!flag)
						{
							this.cachedOccupiedRect = this.entities[i].OccupiedRect;
							flag = true;
						}
						else
						{
							CellRect occupiedRect = this.entities[i].OccupiedRect;
							this.cachedOccupiedRect = CellRect.FromLimits(Mathf.Min(this.cachedOccupiedRect.minX, occupiedRect.minX), Mathf.Min(this.cachedOccupiedRect.minZ, occupiedRect.minZ), Mathf.Max(this.cachedOccupiedRect.maxX, occupiedRect.maxX), Mathf.Max(this.cachedOccupiedRect.maxZ, occupiedRect.maxZ));
						}
					}
					this.occupiedRectDirty = false;
				}
				return this.cachedOccupiedRect;
			}
		}

		// Token: 0x17001C6C RID: 7276
		// (get) Token: 0x0600BB34 RID: 47924 RVA: 0x00361F08 File Offset: 0x00360108
		public IntVec2 OccupiedSize
		{
			get
			{
				return new IntVec2(this.OccupiedRect.Width, this.OccupiedRect.Height);
			}
		}

		// Token: 0x17001C6D RID: 7277
		// (get) Token: 0x0600BB35 RID: 47925 RVA: 0x00361F38 File Offset: 0x00360138
		public IntVec3 OccupiedCenter
		{
			get
			{
				return this.OccupiedRect.CenterCell;
			}
		}

		// Token: 0x17001C6E RID: 7278
		// (get) Token: 0x0600BB36 RID: 47926 RVA: 0x00361F53 File Offset: 0x00360153
		public bool Empty
		{
			get
			{
				return !this.Entities.Any<SketchEntity>();
			}
		}

		// Token: 0x0600BB37 RID: 47927 RVA: 0x00361F64 File Offset: 0x00360164
		public bool Add(SketchEntity entity, bool wipeIfCollides = true)
		{
			if (entity == null)
			{
				throw new ArgumentNullException("entity");
			}
			if (this.entities.Contains(entity))
			{
				return true;
			}
			if (wipeIfCollides)
			{
				this.WipeColliding(entity);
			}
			else if (this.WouldCollide(entity))
			{
				return false;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			SketchTerrain entity2;
			if (sketchTerrain != null && this.terrainAt.TryGetValue(sketchTerrain.pos, out entity2))
			{
				this.Remove(entity2);
			}
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				for (int i = this.cachedBuildables.Count - 1; i >= 0; i--)
				{
					if (sketchBuildable.OccupiedRect.Overlaps(this.cachedBuildables[i].OccupiedRect) && GenSpawn.SpawningWipes(sketchBuildable.Buildable, this.cachedBuildables[i].Buildable, false))
					{
						this.Remove(this.cachedBuildables[i]);
					}
				}
			}
			this.entities.Add(entity);
			this.AddToCache(entity);
			return true;
		}

		// Token: 0x0600BB38 RID: 47928 RVA: 0x00362058 File Offset: 0x00360258
		public bool AddThing(ThingDef def, IntVec3 pos, Rot4 rot, ThingDef stuff = null, int stackCount = 1, QualityCategory? quality = null, int? hitPoints = null, bool wipeIfCollides = true, float spawnOrder = 2f)
		{
			SketchThing entity = new SketchThing
			{
				def = def,
				stuff = stuff,
				pos = pos,
				rot = rot,
				stackCount = stackCount,
				quality = quality,
				hitPoints = hitPoints,
				spawnOrder = spawnOrder
			};
			return this.Add(entity, wipeIfCollides);
		}

		// Token: 0x0600BB39 RID: 47929 RVA: 0x003620B4 File Offset: 0x003602B4
		public bool AddTerrain(TerrainDef def, IntVec3 pos, bool wipeIfCollides = true)
		{
			return this.Add(new SketchTerrain
			{
				def = def,
				pos = pos
			}, wipeIfCollides);
		}

		// Token: 0x0600BB3A RID: 47930 RVA: 0x003620DD File Offset: 0x003602DD
		public bool Remove(SketchEntity entity)
		{
			if (entity == null)
			{
				return false;
			}
			if (!this.entities.Contains(entity))
			{
				return false;
			}
			this.entities.Remove(entity);
			this.RemoveFromCache(entity);
			return true;
		}

		// Token: 0x0600BB3B RID: 47931 RVA: 0x0036210C File Offset: 0x0036030C
		public bool RemoveTerrain(IntVec3 cell)
		{
			SketchTerrain entity;
			return this.terrainAt.TryGetValue(cell, out entity) && this.Remove(entity);
		}

		// Token: 0x0600BB3C RID: 47932 RVA: 0x00362132 File Offset: 0x00360332
		public void Clear()
		{
			this.entities.Clear();
			this.RecacheAll();
		}

		// Token: 0x0600BB3D RID: 47933 RVA: 0x00362148 File Offset: 0x00360348
		public TerrainDef TerrainAt(IntVec3 pos)
		{
			SketchTerrain sketchTerrain = this.SketchTerrainAt(pos);
			if (sketchTerrain == null)
			{
				return null;
			}
			return sketchTerrain.def;
		}

		// Token: 0x0600BB3E RID: 47934 RVA: 0x00362168 File Offset: 0x00360368
		public SketchTerrain SketchTerrainAt(IntVec3 pos)
		{
			SketchTerrain result;
			if (!this.terrainAt.TryGetValue(pos, out result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x0600BB3F RID: 47935 RVA: 0x00362188 File Offset: 0x00360388
		public bool AnyTerrainAt(int x, int z)
		{
			return this.AnyTerrainAt(new IntVec3(x, 0, z));
		}

		// Token: 0x0600BB40 RID: 47936 RVA: 0x00362198 File Offset: 0x00360398
		public bool AnyTerrainAt(IntVec3 pos)
		{
			return this.TerrainAt(pos) != null;
		}

		// Token: 0x0600BB41 RID: 47937 RVA: 0x003621A4 File Offset: 0x003603A4
		public IEnumerable<SketchThing> ThingsAt(IntVec3 pos)
		{
			SketchThing val;
			if (this.thingsAt_single.TryGetValue(pos, out val))
			{
				return Gen.YieldSingle<SketchThing>(val);
			}
			List<SketchThing> result;
			if (this.thingsAt_multiple.TryGetValue(pos, out result))
			{
				return result;
			}
			return Sketch.EmptySketchThingList;
		}

		// Token: 0x0600BB42 RID: 47938 RVA: 0x003621E0 File Offset: 0x003603E0
		public bool AnyThingAt(IntVec3 pos)
		{
			SketchThing sketchThing;
			SketchThing sketchThing2;
			List<SketchThing> list;
			return (this.edificeAt.TryGetValue(pos, out sketchThing) && sketchThing != null) || this.thingsAt_single.TryGetValue(pos, out sketchThing2) || this.thingsAt_multiple.TryGetValue(pos, out list);
		}

		// Token: 0x0600BB43 RID: 47939 RVA: 0x00362224 File Offset: 0x00360424
		public void ThingsAt(IntVec3 pos, out SketchThing singleResult, out List<SketchThing> multipleResults)
		{
			SketchThing sketchThing;
			if (this.thingsAt_single.TryGetValue(pos, out sketchThing))
			{
				singleResult = sketchThing;
				multipleResults = null;
				return;
			}
			List<SketchThing> list;
			if (this.thingsAt_multiple.TryGetValue(pos, out list))
			{
				singleResult = null;
				multipleResults = list;
				return;
			}
			singleResult = null;
			multipleResults = null;
		}

		// Token: 0x0600BB44 RID: 47940 RVA: 0x00362268 File Offset: 0x00360468
		public SketchThing EdificeAt(IntVec3 pos)
		{
			SketchThing result;
			if (this.edificeAt.TryGetValue(pos, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x0600BB45 RID: 47941 RVA: 0x00362288 File Offset: 0x00360488
		public bool WouldCollide(SketchEntity entity)
		{
			if (this.entities.Contains(entity))
			{
				return false;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				return this.WouldCollide(sketchThing.def, sketchThing.pos, sketchThing.rot);
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			return sketchTerrain != null && this.WouldCollide(sketchTerrain.def, sketchTerrain.pos);
		}

		// Token: 0x0600BB46 RID: 47942 RVA: 0x003622E8 File Offset: 0x003604E8
		public bool WouldCollide(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 pos2 in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(pos2);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						return true;
					}
				}
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect))
				{
					if (def.race != null)
					{
						if (this.cachedThings[i].def.passability == Traversability.Impassable)
						{
							return true;
						}
					}
					else if (!GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def, null, null))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600BB47 RID: 47943 RVA: 0x003623E8 File Offset: 0x003605E8
		public bool WouldCollide(TerrainDef def, IntVec3 pos)
		{
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				return true;
			}
			for (int i = 0; i < this.cachedThings.Count; i++)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB48 RID: 47944 RVA: 0x00362478 File Offset: 0x00360678
		public void WipeColliding(SketchEntity entity)
		{
			if (!this.WouldCollide(entity))
			{
				return;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				this.WipeColliding(sketchThing.def, sketchThing.pos, sketchThing.rot);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.WipeColliding(sketchTerrain.def, sketchTerrain.pos);
			}
		}

		// Token: 0x0600BB49 RID: 47945 RVA: 0x003624D0 File Offset: 0x003606D0
		public void WipeColliding(ThingDef def, IntVec3 pos, Rot4 rot)
		{
			if (!this.WouldCollide(def, pos, rot))
			{
				return;
			}
			CellRect cellRect = GenAdj.OccupiedRect(pos, rot, def.size);
			if (def.terrainAffordanceNeeded != null)
			{
				foreach (IntVec3 intVec in cellRect)
				{
					TerrainDef terrainDef = this.TerrainAt(intVec);
					if (terrainDef != null && !terrainDef.affordances.Contains(def.terrainAffordanceNeeded))
					{
						this.RemoveTerrain(intVec);
					}
				}
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (cellRect.Overlaps(this.cachedThings[i].OccupiedRect) && !GenConstruct.CanPlaceBlueprintOver(def, this.cachedThings[i].def, null, null))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x0600BB4A RID: 47946 RVA: 0x003625C8 File Offset: 0x003607C8
		public void WipeColliding(TerrainDef def, IntVec3 pos)
		{
			if (!this.WouldCollide(def, pos))
			{
				return;
			}
			if (!def.layerable && this.TerrainAt(pos) != null)
			{
				this.RemoveTerrain(pos);
			}
			for (int i = this.cachedThings.Count - 1; i >= 0; i--)
			{
				if (this.cachedThings[i].OccupiedRect.Contains(pos) && this.cachedThings[i].def.terrainAffordanceNeeded != null && !def.affordances.Contains(this.cachedThings[i].def.terrainAffordanceNeeded))
				{
					this.Remove(this.cachedThings[i]);
				}
			}
		}

		// Token: 0x0600BB4B RID: 47947 RVA: 0x0036267C File Offset: 0x0036087C
		public bool IsSpawningBlocked(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				if (this.entities[i].IsSpawningBlocked(this.entities[i].pos + offset, map, null, false))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600BB4C RID: 47948 RVA: 0x003626DC File Offset: 0x003608DC
		public bool AnyThingOutOfBounds(Map map, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			for (int i = 0; i < this.entities.Count; i++)
			{
				SketchThing sketchThing = this.entities[i] as SketchThing;
				if (sketchThing != null)
				{
					if (sketchThing.def.size == IntVec2.One)
					{
						if (!(this.entities[i].pos + offset).InBounds(map))
						{
							return true;
						}
					}
					else
					{
						using (CellRect.Enumerator enumerator = sketchThing.OccupiedRect.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (!(enumerator.Current + offset).InBounds(map))
								{
									return true;
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600BB4D RID: 47949 RVA: 0x003627B4 File Offset: 0x003609B4
		public void Spawn(Map map, IntVec3 pos, Faction faction, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, bool clearEdificeWhereFloor = false, List<Thing> spawnedThings = null, bool dormant = false, bool buildRoofsInstantly = false, Func<SketchEntity, IntVec3, bool> canSpawnThing = null, Action<IntVec3, SketchEntity> onFailedToSpawnThing = null, TerrainDef defaultAffordanceTerrain = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			if (clearEdificeWhereFloor)
			{
				for (int i = 0; i < this.cachedTerrain.Count; i++)
				{
					if (this.cachedTerrain[i].def.layerable)
					{
						Building edifice = (this.cachedTerrain[i].pos + offset).GetEdifice(map);
						if (edifice != null)
						{
							edifice.Destroy(DestroyMode.Vanish);
						}
					}
				}
			}
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				IntVec3 intVec = sketchEntity.pos + offset;
				if (((canSpawnThing != null && !canSpawnThing(sketchEntity, intVec)) || !sketchEntity.Spawn(intVec, map, faction, spawnMode, wipeIfCollides, forceTerrainAffordance, spawnedThings, dormant, defaultAffordanceTerrain)) && onFailedToSpawnThing != null)
				{
					onFailedToSpawnThing(intVec, sketchEntity);
				}
			}
			if (spawnedThings != null && spawnMode == Sketch.SpawnMode.TransportPod && !wipeIfCollides)
			{
				bool flag = false;
				for (int j = 0; j < spawnedThings.Count; j++)
				{
					for (int k = j + 1; k < spawnedThings.Count; k++)
					{
						CellRect cellRect = GenAdj.OccupiedRect(spawnedThings[j].Position, spawnedThings[j].Rotation, spawnedThings[j].def.size);
						CellRect other = GenAdj.OccupiedRect(spawnedThings[k].Position, spawnedThings[k].Rotation, spawnedThings[k].def.size);
						if (cellRect.Overlaps(other) && (GenSpawn.SpawningWipes(spawnedThings[j].def, spawnedThings[k].def, false) || GenSpawn.SpawningWipes(spawnedThings[k].def, spawnedThings[j].def, false)))
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					for (int l = 0; l < spawnedThings.Count; l++)
					{
						ActiveTransporterInfo activeTransporterInfo = spawnedThings[l].ParentHolder as ActiveTransporterInfo;
						if (activeTransporterInfo != null)
						{
							activeTransporterInfo.spawnWipeMode = null;
						}
					}
				}
			}
			if (buildRoofsInstantly && spawnMode == Sketch.SpawnMode.Normal)
			{
				foreach (IntVec3 a in this.GetSuggestedRoofCells())
				{
					IntVec3 c = a + offset;
					if (c.InBounds(map) && !c.Roofed(map))
					{
						map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
					}
				}
			}
		}

		// Token: 0x0600BB4E RID: 47950 RVA: 0x00362A98 File Offset: 0x00360C98
		public void Merge(Sketch other, bool wipeIfCollides = true)
		{
			foreach (SketchEntity sketchEntity in from x in other.entities
			orderby x.SpawnOrder
			select x)
			{
				this.Add(sketchEntity.DeepCopy(), wipeIfCollides);
			}
		}

		// Token: 0x0600BB4F RID: 47951 RVA: 0x00362B10 File Offset: 0x00360D10
		public void MergeAt(Sketch other, IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool wipeIfCollides = true)
		{
			Sketch sketch = other.DeepCopy();
			sketch.MoveBy(sketch.GetOffset(pos, posType));
			this.Merge(sketch, wipeIfCollides);
		}

		// Token: 0x0600BB50 RID: 47952 RVA: 0x00362B3C File Offset: 0x00360D3C
		public void Subtract(Sketch other)
		{
			for (int i = 0; i < this.entities.Count; i++)
			{
				for (int j = 0; j < other.entities.Count; j++)
				{
					if (this.entities[i].SameForSubtracting(other.entities[j]))
					{
						this.Remove(this.entities[i]);
						i--;
						break;
					}
				}
			}
		}

		// Token: 0x0600BB51 RID: 47953 RVA: 0x00362BAD File Offset: 0x00360DAD
		public void MoveBy(IntVec2 by)
		{
			this.MoveBy(by.ToIntVec3);
		}

		// Token: 0x0600BB52 RID: 47954 RVA: 0x00362BBC File Offset: 0x00360DBC
		public void MoveBy(IntVec3 by)
		{
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos += by;
			}
			this.RecacheAll();
		}

		// Token: 0x0600BB53 RID: 47955 RVA: 0x00362C20 File Offset: 0x00360E20
		public void MoveOccupiedCenterToZero()
		{
			this.MoveBy(new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z));
		}

		// Token: 0x0600BB54 RID: 47956 RVA: 0x00362C48 File Offset: 0x00360E48
		public Sketch DeepCopy()
		{
			Sketch sketch = new Sketch();
			foreach (SketchEntity sketchEntity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				sketch.Add(sketchEntity.DeepCopy(), true);
			}
			return sketch;
		}

		// Token: 0x0600BB55 RID: 47957 RVA: 0x00362CC8 File Offset: 0x00360EC8
		private void AddToCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Add(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						this.edificeAt[key] = sketchThing;
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing item;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Add(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out item))
					{
						this.thingsAt_single.Remove(key2);
						List<SketchThing> list2 = new List<SketchThing>();
						list2.Add(item);
						list2.Add(sketchThing);
						this.thingsAt_multiple.Add(key2, list2);
					}
					else
					{
						this.thingsAt_single.Add(key2, sketchThing);
					}
				}
				this.cachedThings.Add(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				this.terrainAt[sketchTerrain.pos] = sketchTerrain;
				this.cachedTerrain.Add(sketchTerrain);
			}
		}

		// Token: 0x0600BB56 RID: 47958 RVA: 0x00362E5C File Offset: 0x0036105C
		private void RemoveFromCache(SketchEntity entity)
		{
			this.occupiedRectDirty = true;
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable != null)
			{
				this.cachedBuildables.Remove(sketchBuildable);
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null)
			{
				if (sketchThing.def.building != null && sketchThing.def.building.isEdifice)
				{
					foreach (IntVec3 key in sketchThing.OccupiedRect)
					{
						SketchThing sketchThing2;
						if (this.edificeAt.TryGetValue(key, out sketchThing2) && sketchThing2 == sketchThing)
						{
							this.edificeAt.Remove(key);
						}
					}
				}
				foreach (IntVec3 key2 in sketchThing.OccupiedRect)
				{
					List<SketchThing> list;
					SketchThing sketchThing3;
					if (this.thingsAt_multiple.TryGetValue(key2, out list))
					{
						list.Remove(sketchThing);
					}
					else if (this.thingsAt_single.TryGetValue(key2, out sketchThing3) && sketchThing3 == sketchThing)
					{
						this.thingsAt_single.Remove(key2);
					}
				}
				this.cachedThings.Remove(sketchThing);
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null)
			{
				SketchTerrain sketchTerrain2;
				if (this.terrainAt.TryGetValue(sketchTerrain.pos, out sketchTerrain2) && sketchTerrain2 == sketchTerrain)
				{
					this.terrainAt.Remove(sketchTerrain.pos);
				}
				this.cachedTerrain.Remove(sketchTerrain);
			}
		}

		// Token: 0x0600BB57 RID: 47959 RVA: 0x00362FF0 File Offset: 0x003611F0
		private void Recache(SketchEntity entity)
		{
			this.RemoveFromCache(entity);
			this.AddToCache(entity);
		}

		// Token: 0x0600BB58 RID: 47960 RVA: 0x00363000 File Offset: 0x00361200
		public void RecacheAll()
		{
			this.terrainAt.Clear();
			this.edificeAt.Clear();
			this.thingsAt_single.Clear();
			this.cachedThings.Clear();
			this.cachedTerrain.Clear();
			this.cachedBuildables.Clear();
			this.occupiedRectDirty = true;
			foreach (KeyValuePair<IntVec3, List<SketchThing>> keyValuePair in this.thingsAt_multiple)
			{
				keyValuePair.Value.Clear();
			}
			foreach (SketchEntity entity in from x in this.entities
			orderby x.SpawnOrder
			select x)
			{
				this.AddToCache(entity);
			}
		}

		// Token: 0x0600BB59 RID: 47961 RVA: 0x00363104 File Offset: 0x00361304
		public bool LineOfSight(IntVec3 start, IntVec3 end, bool skipFirstCell = false, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (!skipFirstCell || !(intVec == start))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BB5A RID: 47962 RVA: 0x0036317C File Offset: 0x0036137C
		public bool LineOfSight(IntVec3 start, IntVec3 end, CellRect startRect, CellRect endRect, Func<IntVec3, bool> validator = null)
		{
			foreach (IntVec3 intVec in GenSight.PointsOnLineOfSight(start, end))
			{
				if (endRect.Contains(intVec))
				{
					return true;
				}
				if (!startRect.Contains(intVec))
				{
					if (!this.CanBeSeenOver(intVec))
					{
						return false;
					}
					if (validator != null && !validator(intVec))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BB5B RID: 47963 RVA: 0x00363200 File Offset: 0x00361400
		public bool CanBeSeenOver(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing == null || sketchThing.def.Fillage != FillCategory.Full;
		}

		// Token: 0x0600BB5C RID: 47964 RVA: 0x0036322B File Offset: 0x0036142B
		public bool Passable(int x, int z)
		{
			return this.Passable(new IntVec3(x, 0, z));
		}

		// Token: 0x0600BB5D RID: 47965 RVA: 0x0036323C File Offset: 0x0036143C
		public bool Passable(IntVec3 pos)
		{
			TerrainDef terrainDef = this.TerrainAt(pos);
			if (terrainDef != null && terrainDef.passability == Traversability.Impassable)
			{
				return false;
			}
			using (IEnumerator<SketchThing> enumerator = this.ThingsAt(pos).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.passability == Traversability.Impassable)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600BB5E RID: 47966 RVA: 0x003632AC File Offset: 0x003614AC
		public void DrawGhost(IntVec3 pos, Sketch.SpawnPosType posType = Sketch.SpawnPosType.Unchanged, bool placingMode = false, Thing thingToIgnore = null, Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null)
		{
			IntVec3 offset = this.GetOffset(pos, posType);
			Map currentMap = Find.CurrentMap;
			bool flag = false;
			using (List<SketchEntity>.Enumerator enumerator = this.Entities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.OccupiedRect.MovedBy(offset).InBounds(currentMap))
					{
						flag = true;
						break;
					}
				}
			}
			foreach (SketchBuildable sketchBuildable in this.Buildables)
			{
				Thing spawnedBlueprintOrFrame = sketchBuildable.GetSpawnedBlueprintOrFrame(sketchBuildable.pos + offset, currentMap);
				if (spawnedBlueprintOrFrame != null)
				{
					Sketch.tmpSketchThings.Add(spawnedBlueprintOrFrame);
				}
				else
				{
					SketchThing sketchThing = sketchBuildable as SketchThing;
					if (sketchThing != null)
					{
						Thing sameSpawned = sketchThing.GetSameSpawned(sketchThing.pos + offset, currentMap);
						if (sameSpawned != null)
						{
							Sketch.tmpSketchThings.Add(sameSpawned);
						}
					}
				}
			}
			CellRect cellRect = Find.CameraDriver.CurrentViewRect.ExpandedBy(1).ClipInsideMap(Find.CurrentMap);
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				if ((placingMode || !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) && sketchEntity.OccupiedRect.MovedBy(offset).InBounds(currentMap))
				{
					Color color = (flag || (sketchEntity.IsSpawningBlocked(sketchEntity.pos + offset, currentMap, thingToIgnore, false) && !sketchEntity.IsSameSpawnedOrBlueprintOrFrame(sketchEntity.pos + offset, currentMap)) || (validator != null && !validator(sketchEntity, offset, Sketch.tmpSketchThings, Find.CurrentMap))) ? Sketch.BlockedColor : Sketch.GhostColor;
					if (cellRect.Contains(sketchEntity.pos + offset))
					{
						sketchEntity.DrawGhost(sketchEntity.pos + offset, color);
					}
				}
			}
			Sketch.tmpSketchThings.Clear();
		}

		// Token: 0x0600BB5F RID: 47967 RVA: 0x003634F8 File Offset: 0x003616F8
		public void FloodFill(IntVec3 root, Predicate<IntVec3> passCheck, Func<IntVec3, int, bool> processor, int maxCellsToProcess = 2147483647, CellRect? bounds = null, IEnumerable<IntVec3> extraRoots = null)
		{
			if (this.floodFillWorking)
			{
				Log.Error("Nested FloodFill calls are not allowed. This will cause bugs.");
			}
			this.floodFillWorking = true;
			if (this.floodFillOpenSet == null)
			{
				this.floodFillOpenSet = new Queue<IntVec3>();
			}
			if (this.floodFillTraversalDistance == null)
			{
				this.floodFillTraversalDistance = new Dictionary<IntVec3, int>();
			}
			this.floodFillTraversalDistance.Clear();
			this.floodFillOpenSet.Clear();
			if (root.IsValid && extraRoots == null && !passCheck(root))
			{
				this.floodFillWorking = false;
				return;
			}
			if (bounds == null)
			{
				bounds = new CellRect?(this.OccupiedRect);
			}
			int area = bounds.Value.Area;
			IntVec3[] cardinalDirectionsAround = GenAdj.CardinalDirectionsAround;
			int num = cardinalDirectionsAround.Length;
			int num2 = 0;
			if (root.IsValid)
			{
				this.floodFillTraversalDistance.Add(root, 0);
				this.floodFillOpenSet.Enqueue(root);
			}
			if (extraRoots == null)
			{
				goto IL_23D;
			}
			IList<IntVec3> list = extraRoots as IList<IntVec3>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.floodFillTraversalDistance.SetOrAdd(list[i], 0);
					this.floodFillOpenSet.Enqueue(list[i]);
				}
				goto IL_23D;
			}
			using (IEnumerator<IntVec3> enumerator = extraRoots.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IntVec3 intVec = enumerator.Current;
					this.floodFillTraversalDistance.SetOrAdd(intVec, 0);
					this.floodFillOpenSet.Enqueue(intVec);
				}
				goto IL_23D;
			}
			IL_169:
			IntVec3 intVec2 = this.floodFillOpenSet.Dequeue();
			int num3 = this.floodFillTraversalDistance[intVec2];
			if (processor(intVec2, num3))
			{
				goto IL_24E;
			}
			num2++;
			if (num2 == maxCellsToProcess)
			{
				goto IL_24E;
			}
			for (int j = 0; j < num; j++)
			{
				IntVec3 intVec3 = intVec2 + cardinalDirectionsAround[j];
				if (bounds.Value.Contains(intVec3) && !this.floodFillTraversalDistance.ContainsKey(intVec3) && passCheck(intVec3))
				{
					this.floodFillOpenSet.Enqueue(intVec3);
					this.floodFillTraversalDistance.Add(intVec3, num3 + 1);
				}
			}
			if (this.floodFillOpenSet.Count > area)
			{
				Log.Error("Overflow on flood fill (>" + area.ToString() + " cells). Make sure we're not flooding over the same area after we check it.");
				this.floodFillWorking = false;
				return;
			}
			IL_23D:
			if (this.floodFillOpenSet.Count > 0)
			{
				goto IL_169;
			}
			IL_24E:
			this.floodFillWorking = false;
		}

		// Token: 0x0600BB60 RID: 47968 RVA: 0x0036376C File Offset: 0x0036196C
		public IEnumerable<IntVec3> GetSuggestedRoofCells()
		{
			if (this.Empty)
			{
				yield break;
			}
			CellRect occupiedRect = this.OccupiedRect;
			Sketch.tmpSuggestedRoofCellsVisited.Clear();
			Sketch.tmpYieldedSuggestedRoofCells.Clear();
			foreach (IntVec3 intVec in this.OccupiedRect)
			{
				if (!Sketch.tmpSuggestedRoofCellsVisited.Contains(intVec) && !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|83_0(intVec))
				{
					Sketch.tmpSuggestedRoofCells.Clear();
					this.FloodFill(intVec, (IntVec3 c) => !this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|83_0(c), delegate(IntVec3 c, int dist)
					{
						Sketch.tmpSuggestedRoofCellsVisited.Add(c);
						Sketch.tmpSuggestedRoofCells.Add(c);
						return false;
					}, int.MaxValue, null, null);
					bool flag = false;
					for (int k = 0; k < Sketch.tmpSuggestedRoofCells.Count; k++)
					{
						if (occupiedRect.IsOnEdge(Sketch.tmpSuggestedRoofCells[k]))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num;
						for (int i = 0; i < Sketch.tmpSuggestedRoofCells.Count; i = num + 1)
						{
							for (int j = 0; j < 9; j = num + 1)
							{
								IntVec3 intVec2 = Sketch.tmpSuggestedRoofCells[i] + GenAdj.AdjacentCellsAndInside[j];
								if (!Sketch.tmpYieldedSuggestedRoofCells.Contains(intVec2) && occupiedRect.Contains(intVec2) && (j == 8 || this.<GetSuggestedRoofCells>g__AnyRoofHolderAt|83_0(intVec2)))
								{
									Sketch.tmpYieldedSuggestedRoofCells.Add(intVec2);
									yield return intVec2;
								}
								num = j;
							}
							num = i;
						}
					}
				}
			}
			Sketch.tmpSuggestedRoofCellsVisited.Clear();
			Sketch.tmpYieldedSuggestedRoofCells.Clear();
			yield break;
			yield break;
		}

		// Token: 0x0600BB61 RID: 47969 RVA: 0x0036377C File Offset: 0x0036197C
		private IntVec3 GetOffset(IntVec3 pos, Sketch.SpawnPosType posType)
		{
			IntVec3 intVec;
			switch (posType)
			{
			case Sketch.SpawnPosType.Unchanged:
				intVec = IntVec3.Zero;
				break;
			case Sketch.SpawnPosType.OccupiedCenter:
				intVec = new IntVec3(-this.OccupiedCenter.x, 0, -this.OccupiedCenter.z);
				break;
			case Sketch.SpawnPosType.OccupiedBotLeft:
				intVec = new IntVec3(-this.OccupiedRect.minX, 0, -this.OccupiedRect.minZ);
				break;
			default:
				intVec = default(IntVec3);
				break;
			}
			intVec += pos;
			return intVec;
		}

		// Token: 0x0600BB62 RID: 47970 RVA: 0x003637FC File Offset: 0x003619FC
		public void Rotate(Rot4 rot)
		{
			if (rot == this.rotation)
			{
				return;
			}
			RotationDirection relativeRotation = Rot4.GetRelativeRotation(this.rotation, rot);
			this.rotation = rot;
			foreach (SketchEntity sketchEntity in this.Entities)
			{
				sketchEntity.pos = sketchEntity.pos.RotatedBy(relativeRotation);
				SketchThing sketchThing = sketchEntity as SketchThing;
				if (sketchThing != null)
				{
					if (sketchThing.def.rotatable)
					{
						sketchThing.rot.Rotate(relativeRotation);
					}
					else if (relativeRotation == RotationDirection.Clockwise && sketchThing.def.size.z % 2 == 0)
					{
						SketchEntity sketchEntity2 = sketchEntity;
						sketchEntity2.pos.z = sketchEntity2.pos.z - 1;
					}
					else if (relativeRotation == RotationDirection.Opposite)
					{
						if (sketchThing.def.size.x % 2 == 0)
						{
							SketchEntity sketchEntity3 = sketchEntity;
							sketchEntity3.pos.x = sketchEntity3.pos.x - 1;
						}
						if (sketchThing.def.size.z % 2 == 0)
						{
							SketchEntity sketchEntity4 = sketchEntity;
							sketchEntity4.pos.z = sketchEntity4.pos.z - 1;
						}
					}
					else if (relativeRotation == RotationDirection.Counterclockwise && sketchThing.def.size.x % 2 == 0)
					{
						SketchEntity sketchEntity5 = sketchEntity;
						sketchEntity5.pos.x = sketchEntity5.pos.x - 1;
					}
				}
			}
			this.RecacheAll();
		}

		// Token: 0x0600BB63 RID: 47971 RVA: 0x00363954 File Offset: 0x00361B54
		public virtual void ExposeData()
		{
			Scribe_Collections.Look<SketchEntity>(ref this.entities, "entities", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<Rot4>(ref this.rotation, "rotation", Rot4.North, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.entities.RemoveAll((SketchEntity x) => x == null) != 0)
				{
					Log.Error("Some sketch entities were null after loading.");
				}
				if (this.entities.RemoveAll((SketchEntity x) => x.LostImportantReferences) != 0)
				{
					Log.Error("Some sketch entities had null defs after loading.");
				}
				this.RecacheAll();
				Sketch.tmpToRemove.Clear();
				for (int i = 0; i < this.cachedThings.Count; i++)
				{
					if (this.cachedThings[i].def.IsDoor)
					{
						for (int j = 0; j < this.cachedThings.Count; j++)
						{
							if (this.cachedThings[j].def == ThingDefOf.Wall && this.cachedThings[j].pos == this.cachedThings[i].pos)
							{
								Sketch.tmpToRemove.Add(this.cachedThings[j]);
							}
						}
					}
				}
				for (int k = 0; k < Sketch.tmpToRemove.Count; k++)
				{
					Log.Error("Sketch has a wall and a door in the same cell. Fixing.");
					this.Remove(Sketch.tmpToRemove[k]);
				}
				Sketch.tmpToRemove.Clear();
			}
		}

		// Token: 0x0600BB64 RID: 47972 RVA: 0x00363AF0 File Offset: 0x00361CF0
		public void AddPrefab(PrefabDef prefab, IntVec3 pos, Rot4 rot)
		{
			rot = PrefabUtility.ValidateRotation(prefab, rot);
			IntVec3 root = PrefabUtility.GetRoot(prefab, pos, rot);
			foreach (ValueTuple<PrefabTerrainData, IntVec3> valueTuple in prefab.GetTerrain())
			{
				PrefabTerrainData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				if (Rand.Chance(item.chance))
				{
					IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(item2, rot);
					this.AddTerrain(item.def, root + adjustedLocalPosition, true);
				}
			}
			foreach (ValueTuple<PrefabThingData, IntVec3> valueTuple2 in prefab.GetThings())
			{
				PrefabThingData item3 = valueTuple2.Item1;
				IntVec3 item4 = valueTuple2.Item2;
				if (Rand.Chance(item3.chance))
				{
					IntVec3 adjustedThingLocalPosition = PrefabUtility.GetAdjustedThingLocalPosition(item3, rot, item4);
					this.AddThing(item3.def, root + adjustedThingLocalPosition, rot.Rotated(item3.relativeRotation), item3.stuff, item3.stackCountRange.RandomInRange, null, null, true, 2f);
				}
			}
			foreach (ValueTuple<SubPrefabData, IntVec3> valueTuple3 in prefab.GetPrefabs())
			{
				SubPrefabData item5 = valueTuple3.Item1;
				IntVec3 item6 = valueTuple3.Item2;
				if (Rand.Chance(item5.chance))
				{
					IntVec3 adjustedPrefabLocalPosition = PrefabUtility.GetAdjustedPrefabLocalPosition(item5, rot, item6);
					this.AddPrefab(item5.def, root + adjustedPrefabLocalPosition, rot.Rotated(item5.relativeRotation));
				}
			}
		}

		// Token: 0x0600BB67 RID: 47975 RVA: 0x00363DC8 File Offset: 0x00361FC8
		[CompilerGenerated]
		private bool <GetSuggestedRoofCells>g__AnyRoofHolderAt|83_0(IntVec3 c)
		{
			SketchThing sketchThing = this.EdificeAt(c);
			return sketchThing != null && sketchThing.def.holdsRoof;
		}

		// Token: 0x04008103 RID: 33027
		private List<SketchEntity> entities = new List<SketchEntity>();

		// Token: 0x04008104 RID: 33028
		private readonly List<SketchThing> cachedThings = new List<SketchThing>();

		// Token: 0x04008105 RID: 33029
		private readonly List<SketchTerrain> cachedTerrain = new List<SketchTerrain>();

		// Token: 0x04008106 RID: 33030
		private readonly List<SketchBuildable> cachedBuildables = new List<SketchBuildable>();

		// Token: 0x04008107 RID: 33031
		private readonly Dictionary<IntVec3, SketchTerrain> terrainAt = new Dictionary<IntVec3, SketchTerrain>();

		// Token: 0x04008108 RID: 33032
		private readonly Dictionary<IntVec3, SketchThing> edificeAt = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x04008109 RID: 33033
		private readonly Dictionary<IntVec3, SketchThing> thingsAt_single = new Dictionary<IntVec3, SketchThing>();

		// Token: 0x0400810A RID: 33034
		private readonly Dictionary<IntVec3, List<SketchThing>> thingsAt_multiple = new Dictionary<IntVec3, List<SketchThing>>();

		// Token: 0x0400810B RID: 33035
		private bool occupiedRectDirty = true;

		// Token: 0x0400810C RID: 33036
		private CellRect cachedOccupiedRect;

		// Token: 0x0400810D RID: 33037
		private Rot4 rotation = Rot4.North;

		// Token: 0x0400810E RID: 33038
		private bool floodFillWorking;

		// Token: 0x0400810F RID: 33039
		private Queue<IntVec3> floodFillOpenSet;

		// Token: 0x04008110 RID: 33040
		private Dictionary<IntVec3, int> floodFillTraversalDistance;

		// Token: 0x04008111 RID: 33041
		public const float SpawnOrder_BeforeTerrain = 0.5f;

		// Token: 0x04008112 RID: 33042
		public const float SpawnOrder_Terrain = 1f;

		// Token: 0x04008113 RID: 33043
		public const float SpawnOrder_Thing = 2f;

		// Token: 0x04008114 RID: 33044
		private static readonly List<SketchThing> EmptySketchThingList = new List<SketchThing>();

		// Token: 0x04008115 RID: 33045
		private static readonly Color GhostColor = new Color(0.7f, 0.7f, 0.7f, 0.35f);

		// Token: 0x04008116 RID: 33046
		private static readonly Color BlockedColor = new Color(0.8f, 0.2f, 0.2f, 0.35f);

		// Token: 0x04008117 RID: 33047
		private static readonly List<Thing> tmpSketchThings = new List<Thing>();

		// Token: 0x04008118 RID: 33048
		private static readonly HashSet<IntVec3> tmpSuggestedRoofCellsVisited = new HashSet<IntVec3>();

		// Token: 0x04008119 RID: 33049
		private static readonly List<IntVec3> tmpSuggestedRoofCells = new List<IntVec3>();

		// Token: 0x0400811A RID: 33050
		private static readonly HashSet<IntVec3> tmpYieldedSuggestedRoofCells = new HashSet<IntVec3>();

		// Token: 0x0400811B RID: 33051
		private static readonly List<SketchThing> tmpToRemove = new List<SketchThing>();

		// Token: 0x0200218B RID: 8587
		public enum SpawnPosType
		{
			// Token: 0x0400811D RID: 33053
			Unchanged,
			// Token: 0x0400811E RID: 33054
			OccupiedCenter,
			// Token: 0x0400811F RID: 33055
			OccupiedBotLeft
		}

		// Token: 0x0200218C RID: 8588
		public enum SpawnMode
		{
			// Token: 0x04008121 RID: 33057
			Blueprint,
			// Token: 0x04008122 RID: 33058
			Normal,
			// Token: 0x04008123 RID: 33059
			TransportPod
		}
	}
}
