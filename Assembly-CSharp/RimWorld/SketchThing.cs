using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002192 RID: 8594
	public class SketchThing : SketchBuildable
	{
		// Token: 0x17001C80 RID: 7296
		// (get) Token: 0x0600BBA8 RID: 48040 RVA: 0x00364911 File Offset: 0x00362B11
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x17001C81 RID: 7297
		// (get) Token: 0x0600BBA9 RID: 48041 RVA: 0x00364919 File Offset: 0x00362B19
		public override ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x17001C82 RID: 7298
		// (get) Token: 0x0600BBAA RID: 48042 RVA: 0x00364921 File Offset: 0x00362B21
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.def, this.stuff, this.stackCount);
			}
		}

		// Token: 0x17001C83 RID: 7299
		// (get) Token: 0x0600BBAB RID: 48043 RVA: 0x003644A8 File Offset: 0x003626A8
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x17001C84 RID: 7300
		// (get) Token: 0x0600BBAC RID: 48044 RVA: 0x0036493A File Offset: 0x00362B3A
		public override CellRect OccupiedRect
		{
			get
			{
				return GenAdj.OccupiedRect(this.pos, this.rot, this.def.size);
			}
		}

		// Token: 0x17001C85 RID: 7301
		// (get) Token: 0x0600BBAD RID: 48045 RVA: 0x00364958 File Offset: 0x00362B58
		public override float SpawnOrder
		{
			get
			{
				return this.spawnOrder;
			}
		}

		// Token: 0x17001C86 RID: 7302
		// (get) Token: 0x0600BBAE RID: 48046 RVA: 0x00364960 File Offset: 0x00362B60
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.def.GetStatValueAbstract(StatDefOf.MaxHitPoints, this.stuff ?? GenStuff.DefaultStuffFor(this.def)));
			}
		}

		// Token: 0x0600BBAF RID: 48047 RVA: 0x0036498C File Offset: 0x00362B8C
		public Thing Instantiate()
		{
			Thing thing = ThingMaker.MakeThing(this.def, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
			thing.stackCount = this.stackCount;
			if (this.quality != null)
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(this.quality.Value, new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
			}
			if (this.hitPoints != null)
			{
				thing.HitPoints = this.hitPoints.Value;
			}
			return thing;
		}

		// Token: 0x0600BBB0 RID: 48048 RVA: 0x00364A14 File Offset: 0x00362C14
		public override void DrawGhost(IntVec3 at, Color color)
		{
			GhostDrawer.DrawGhostThing(at, this.rot, this.def, this.def.graphic, color, AltitudeLayer.Blueprint, null, false, null);
		}

		// Token: 0x0600BBB1 RID: 48049 RVA: 0x00364A44 File Offset: 0x00362C44
		public Thing GetSameSpawned(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.IsSame(thingList[i]))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x0600BBB2 RID: 48050 RVA: 0x00364A90 File Offset: 0x00362C90
		public bool IsSame(Thing thing)
		{
			CellRect lhs = GenAdj.OccupiedRect(thing.Position, this.rot, thing.def.Size);
			CellRect lhs2 = GenAdj.OccupiedRect(thing.Position, this.rot.Opposite, thing.def.Size);
			CellRect rhs = thing.OccupiedRect();
			return thing.def == this.def && (this.stuff == null || thing.Stuff == this.stuff) && (thing.Rotation == this.rot || thing.Rotation == this.rot.Opposite || !this.def.rotatable) && (lhs == rhs || lhs2 == rhs);
		}

		// Token: 0x0600BBB3 RID: 48051 RVA: 0x00364B53 File Offset: 0x00362D53
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return this.GetSameSpawned(at, map) != null;
		}

		// Token: 0x0600BBB4 RID: 48052 RVA: 0x00364B60 File Offset: 0x00362D60
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CellRect lhs = GenAdj.OccupiedRect(at, this.rot, thingList[i].def.Size);
				CellRect lhs2 = GenAdj.OccupiedRect(at, this.rot.Opposite, thingList[i].def.Size);
				CellRect rhs = thingList[i].OccupiedRect();
				if ((lhs == rhs || lhs2 == rhs) && thingList[i].def.entityDefToBuild == this.def && (this.stuff == null || ((IConstructible)thingList[i]).EntityToBuildStuff() == this.stuff) && (thingList[i].Rotation == this.rot || thingList[i].Rotation == this.rot.Opposite || !this.def.rotatable))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x0600BBB5 RID: 48053 RVA: 0x00364C80 File Offset: 0x00362E80
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, this.rot, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, this.rot, map, wipeIfCollides, thingToIgnore, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def), false, false, false).Accepted;
		}

		// Token: 0x0600BBB6 RID: 48054 RVA: 0x00364696 File Offset: 0x00362896
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x0600BBB7 RID: 48055 RVA: 0x00364CFC File Offset: 0x00362EFC
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, this.rot, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
		}

		// Token: 0x0600BBB8 RID: 48056 RVA: 0x00364D28 File Offset: 0x00362F28
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, TerrainDef defaultAffordanceTerrain = null)
		{
			if (!at.RectAbout(this.def.size, this.rot).InBounds(map))
			{
				return false;
			}
			if (forceTerrainAffordance && !this.CanBuildOnTerrain(at, map))
			{
				base.ForceTerrainAffordance(at, this.rot, map, defaultAffordanceTerrain);
			}
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			switch (spawnMode)
			{
			case Sketch.SpawnMode.Blueprint:
				GenConstruct.PlaceBlueprintForBuild(this.def, at, map, this.rot, faction, this.stuff ?? GenStuff.DefaultStuffFor(this.def), null, null, false);
				break;
			case Sketch.SpawnMode.Normal:
			{
				Thing thing = this.Instantiate();
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing);
				}
				if (faction != null && thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(faction);
				}
				this.SetDormant(thing, dormant);
				GenSpawn.Spawn(thing, at, map, this.rot, WipeMode.VanishOrMoveAside, false, false);
				break;
			}
			case Sketch.SpawnMode.TransportPod:
			{
				Thing thing2 = this.Instantiate();
				thing2.Position = at;
				thing2.Rotation = this.rot;
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing2);
				}
				if (faction != null)
				{
					thing2.SetFactionDirect(faction);
				}
				this.SetDormant(thing2, dormant);
				ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
				activeTransporterInfo.innerContainer.TryAdd(thing2, 1, true);
				activeTransporterInfo.openDelay = 60;
				activeTransporterInfo.leaveSlag = false;
				activeTransporterInfo.despawnPodBeforeSpawningThing = true;
				activeTransporterInfo.spawnWipeMode = (wipeIfCollides ? new WipeMode?(WipeMode.VanishOrMoveAside) : null);
				activeTransporterInfo.moveItemsAsideBeforeSpawning = true;
				activeTransporterInfo.setRotation = new Rot4?(this.rot);
				DropPodUtility.MakeDropPodAt(at, map, activeTransporterInfo, faction);
				break;
			}
			default:
				throw new NotImplementedException("Spawn mode " + spawnMode.ToString() + " not implemented!");
			}
			return true;
		}

		// Token: 0x0600BBB9 RID: 48057 RVA: 0x00364EE0 File Offset: 0x003630E0
		private void SetDormant(Thing thing, bool dormant)
		{
			CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				if (dormant)
				{
					compCanBeDormant.ToSleep();
					return;
				}
				compCanBeDormant.WakeUp();
			}
		}

		// Token: 0x0600BBBA RID: 48058 RVA: 0x00364F08 File Offset: 0x00363108
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchThing sketchThing = other as SketchThing;
			if (sketchThing == null)
			{
				return false;
			}
			if (sketchThing == this)
			{
				return true;
			}
			if (this.def == sketchThing.def && this.stuff == sketchThing.stuff && this.stackCount == sketchThing.stackCount && this.pos == sketchThing.pos && this.rot == sketchThing.rot)
			{
				QualityCategory? qualityCategory = this.quality;
				QualityCategory? qualityCategory2 = sketchThing.quality;
				if (qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null))
				{
					int? num = this.hitPoints;
					int? num2 = sketchThing.hitPoints;
					if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
					{
						return this.spawnOrder == sketchThing.spawnOrder;
					}
				}
			}
			return false;
		}

		// Token: 0x0600BBBB RID: 48059 RVA: 0x00364FF8 File Offset: 0x003631F8
		public override SketchEntity DeepCopy()
		{
			SketchThing sketchThing = (SketchThing)base.DeepCopy();
			sketchThing.def = this.def;
			sketchThing.stuff = this.stuff;
			sketchThing.stackCount = this.stackCount;
			sketchThing.rot = this.rot;
			sketchThing.quality = this.quality;
			sketchThing.hitPoints = this.hitPoints;
			sketchThing.spawnOrder = this.spawnOrder;
			return sketchThing;
		}

		// Token: 0x0600BBBC RID: 48060 RVA: 0x00365064 File Offset: 0x00363264
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Values.Look<Rot4>(ref this.rot, "rot", default(Rot4), false);
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
			Scribe_Values.Look<int?>(ref this.hitPoints, "hitPoints", null, false);
			Scribe_Values.Look<float>(ref this.spawnOrder, "spawnOrder", 0f, false);
		}

		// Token: 0x0400813A RID: 33082
		public ThingDef def;

		// Token: 0x0400813B RID: 33083
		public ThingDef stuff;

		// Token: 0x0400813C RID: 33084
		public int stackCount;

		// Token: 0x0400813D RID: 33085
		public Rot4 rot;

		// Token: 0x0400813E RID: 33086
		public QualityCategory? quality;

		// Token: 0x0400813F RID: 33087
		public int? hitPoints;

		// Token: 0x04008140 RID: 33088
		public float spawnOrder = 2f;
	}
}
