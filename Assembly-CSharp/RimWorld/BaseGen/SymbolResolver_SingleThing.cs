using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02003938 RID: 14648
	public class SymbolResolver_SingleThing : SymbolResolver
	{
		// Token: 0x060142BB RID: 82619 RVA: 0x006027A4 File Offset: 0x006009A4
		public override bool CanResolve(ResolveParams rp)
		{
			if (!base.CanResolve(rp))
			{
				return false;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				return true;
			}
			IntVec3 intVec;
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams rp2 = rp;
				rp2.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				if (!SymbolResolver_SinglePawn.TryFindSpawnCell(rp2, out intVec))
				{
					return false;
				}
			}
			return ((rp.singleThingDef == null || rp.singleThingDef.category != ThingCategory.Item) && (rp.singleThingToSpawn == null || rp.singleThingToSpawn.def.category != ThingCategory.Item)) || this.TryFindSpawnCellForItem(rp.rect, out intVec, false);
		}

		// Token: 0x060142BC RID: 82620 RVA: 0x00602848 File Offset: 0x00600A48
		public override void Resolve(ResolveParams rp)
		{
			if (rp.singleThingToSpawn is Pawn)
			{
				ResolveParams resolveParams = rp;
				resolveParams.singlePawnToSpawn = (Pawn)rp.singleThingToSpawn;
				BaseGen.symbolStack.Push("pawn", resolveParams, null);
				return;
			}
			if (rp.singleThingToSpawn != null && rp.singleThingToSpawn.Spawned)
			{
				return;
			}
			ThingDef thingDef2;
			if (rp.singleThingToSpawn == null)
			{
				ThingDef thingDef;
				if ((thingDef = rp.singleThingDef) == null)
				{
					thingDef = (from x in ThingSetMakerUtility.allGeneratableItems
					where x.IsWeapon || x.IsMedicine || x.IsDrug
					select x).RandomElement<ThingDef>();
				}
				thingDef2 = thingDef;
			}
			else
			{
				thingDef2 = rp.singleThingToSpawn.def;
			}
			Rot4? thingRot = rp.thingRot;
			IntVec3 intVec;
			if (thingDef2.category == ThingCategory.Item)
			{
				thingRot = new Rot4?(Rot4.North);
				if (!this.TryFindSpawnCellForItem(rp.rect, out intVec, rp.spawnOutside.GetValueOrDefault()))
				{
					if (rp.singleThingToSpawn != null)
					{
						rp.singleThingToSpawn.Destroy(DestroyMode.Vanish);
					}
					return;
				}
			}
			else
			{
				bool flag;
				bool flag2;
				intVec = this.FindBestSpawnCellForNonItem(rp.rect, thingDef2, ref thingRot, out flag, out flag2, rp.spawnOutside.GetValueOrDefault());
				if ((flag || flag2) && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit != null && rp.skipSingleThingIfHasToWipeBuildingOrDoesntFit.Value)
				{
					return;
				}
			}
			if (thingRot == null)
			{
				Log.Error("Could not resolve rotation. Bug.");
			}
			Thing thing;
			if (rp.singleThingToSpawn == null)
			{
				ThingDef stuff;
				if (rp.singleThingStuff != null && rp.singleThingStuff.stuffProps.CanMake(thingDef2))
				{
					stuff = rp.singleThingStuff;
				}
				else if (thingDef2.building != null)
				{
					stuff = BaseGenUtility.CheapStuffFor(thingDef2, rp.faction);
				}
				else
				{
					stuff = GenStuff.RandomStuffInexpensiveFor(thingDef2, rp.faction, null);
				}
				thing = ThingMaker.MakeThing(thingDef2, stuff);
				thing.stackCount = (rp.singleThingStackCount ?? 1);
				if (thing.stackCount <= 0)
				{
					thing.stackCount = 1;
				}
				if (thing.def.CanHaveFaction && thing.Faction != rp.faction)
				{
					thing.SetFaction(rp.faction, null);
					Thing thing2 = thing;
					FactionIdeosTracker ideos = rp.faction.ideos;
					ThingStyleDef styleDef;
					if (ideos == null)
					{
						styleDef = null;
					}
					else
					{
						Ideo primaryIdeo = ideos.PrimaryIdeo;
						styleDef = ((primaryIdeo != null) ? primaryIdeo.GetStyleFor(thing.def) : null);
					}
					thing2.SetStyleDef(styleDef);
				}
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(QualityUtility.GenerateQualityBaseGen(), new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
				if (rp.postThingGenerate != null)
				{
					rp.postThingGenerate(thing);
				}
			}
			else
			{
				thing = rp.singleThingToSpawn;
			}
			if (rp.spawnBridgeIfTerrainCantSupportThing == null || rp.spawnBridgeIfTerrainCantSupportThing.Value)
			{
				BaseGenUtility.CheckSpawnBridgeUnder(thing.def, intVec, thingRot.Value);
			}
			thing = GenSpawn.Spawn(thing, intVec, BaseGen.globalSettings.map, thingRot.Value, WipeMode.Vanish, false, false);
			if (thing != null && rp.singleThingInnerThings != null)
			{
				CompThingContainer compThingContainer = thing.TryGetComp<CompThingContainer>();
				if (compThingContainer != null)
				{
					using (List<Thing>.Enumerator enumerator = rp.singleThingInnerThings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Thing thing3 = enumerator.Current;
							if (compThingContainer.Accepts(thing3))
							{
								compThingContainer.innerContainer.TryAdd(thing3, true);
							}
						}
						goto IL_366;
					}
				}
				Building_Casket building_Casket = thing as Building_Casket;
				if (building_Casket != null)
				{
					ThingOwner directlyHeldThings = building_Casket.GetDirectlyHeldThings();
					foreach (Thing item in rp.singleThingInnerThings)
					{
						directlyHeldThings.TryAdd(item, true);
					}
				}
			}
			IL_366:
			if (thing != null && thing.def.category == ThingCategory.Item)
			{
				thing.SetForbidden(true, false);
			}
			if (rp.postThingSpawn != null)
			{
				rp.postThingSpawn(thing);
			}
		}

		// Token: 0x060142BD RID: 82621 RVA: 0x00602C04 File Offset: 0x00600E04
		private bool TryFindSpawnCellForItem(CellRect rect, out IntVec3 result, bool spawnOutside = false)
		{
			Map map = BaseGen.globalSettings.map;
			return CellFinder.TryFindRandomCellInsideWith(rect, delegate(IntVec3 c)
			{
				if (c.GetFirstItem(map) != null)
				{
					return false;
				}
				if (!c.Standable(map))
				{
					SurfaceType surfaceType = c.GetSurfaceType(map);
					if (surfaceType != SurfaceType.Item && surfaceType != SurfaceType.Eat)
					{
						return false;
					}
				}
				return !spawnOutside || !c.Roofed(map);
			}, out result);
		}

		// Token: 0x060142BE RID: 82622 RVA: 0x00602C44 File Offset: 0x00600E44
		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, ref Rot4? rot, out bool hasToWipeBuilding, out bool doesntFit, bool spawnOutside = false)
		{
			if (!thingDef.rotatable)
			{
				rot = new Rot4?(Rot4.North);
			}
			if (rot == null)
			{
				SymbolResolver_SingleThing.tmpRotations.Shuffle<Rot4>();
				for (int i = 0; i < SymbolResolver_SingleThing.tmpRotations.Length; i++)
				{
					IntVec3 result = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[i], out hasToWipeBuilding, out doesntFit, spawnOutside);
					if (!hasToWipeBuilding && !doesntFit)
					{
						rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[i]);
						return result;
					}
				}
				for (int j = 0; j < SymbolResolver_SingleThing.tmpRotations.Length; j++)
				{
					IntVec3 result2 = this.FindBestSpawnCellForNonItem(rect, thingDef, SymbolResolver_SingleThing.tmpRotations[j], out hasToWipeBuilding, out doesntFit, spawnOutside);
					if (!doesntFit)
					{
						rot = new Rot4?(SymbolResolver_SingleThing.tmpRotations[j]);
						return result2;
					}
				}
				rot = new Rot4?(Rot4.Random);
				return this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit, spawnOutside);
			}
			return this.FindBestSpawnCellForNonItem(rect, thingDef, rot.Value, out hasToWipeBuilding, out doesntFit, spawnOutside);
		}

		// Token: 0x060142BF RID: 82623 RVA: 0x00602D50 File Offset: 0x00600F50
		private IntVec3 FindBestSpawnCellForNonItem(CellRect rect, ThingDef thingDef, Rot4 rot, out bool hasToWipeBuilding, out bool doesntFit, bool spawnOutside = false)
		{
			Map map = BaseGen.globalSettings.map;
			if (thingDef.category == ThingCategory.Building)
			{
				foreach (IntVec3 intVec in rect.Cells.InRandomOrder(null))
				{
					CellRect rect2 = GenAdj.OccupiedRect(intVec, rot, thingDef.size);
					if (rect2.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect2, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect2) && GenConstruct.TerrainCanSupport(rect2, map, thingDef) && (!spawnOutside || !intVec.Roofed(map)) && !this.InteractionCellBlocked(thingDef, intVec, rot, map))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return intVec;
					}
				}
				foreach (IntVec3 intVec2 in rect.Cells.InRandomOrder(null))
				{
					CellRect rect3 = GenAdj.OccupiedRect(intVec2, rot, thingDef.size);
					if (rect3.FullyContainedWithin(rect) && !BaseGenUtility.AnyDoorAdjacentCardinalTo(rect3, map) && !this.AnyNonStandableCellOrAnyBuildingInside(rect3) && (!spawnOutside || !intVec2.Roofed(map)) && !this.InteractionCellBlocked(thingDef, intVec2, rot, map))
					{
						hasToWipeBuilding = false;
						doesntFit = false;
						return intVec2;
					}
				}
			}
			foreach (IntVec3 intVec3 in rect.Cells.InRandomOrder(null))
			{
				CellRect rect4 = GenAdj.OccupiedRect(intVec3, rot, thingDef.size);
				if (rect4.FullyContainedWithin(rect) && !this.AnyNonStandableCellOrAnyBuildingInside(rect4) && (!spawnOutside || !intVec3.Roofed(map)) && !this.InteractionCellBlocked(thingDef, intVec3, rot, map))
				{
					hasToWipeBuilding = false;
					doesntFit = false;
					return intVec3;
				}
			}
			foreach (IntVec3 intVec4 in rect.Cells.InRandomOrder(null))
			{
				if (GenAdj.OccupiedRect(intVec4, rot, thingDef.size).FullyContainedWithin(rect) && (!spawnOutside || !intVec4.Roofed(map)))
				{
					hasToWipeBuilding = true;
					doesntFit = false;
					return intVec4;
				}
			}
			IntVec3 centerCell = rect.CenterCell;
			CellRect cellRect = GenAdj.OccupiedRect(centerCell, rot, thingDef.size);
			if (cellRect.minX < 0)
			{
				centerCell.x += -cellRect.minX;
			}
			if (cellRect.minZ < 0)
			{
				centerCell.z += -cellRect.minZ;
			}
			if (cellRect.maxX >= map.Size.x)
			{
				centerCell.x -= cellRect.maxX - map.Size.x + 1;
			}
			if (cellRect.maxZ >= map.Size.z)
			{
				centerCell.z -= cellRect.maxZ - map.Size.z + 1;
			}
			hasToWipeBuilding = true;
			doesntFit = true;
			return centerCell;
		}

		// Token: 0x060142C0 RID: 82624 RVA: 0x00603070 File Offset: 0x00601270
		private bool AnyNonStandableCellOrAnyBuildingInside(CellRect rect)
		{
			Map map = BaseGen.globalSettings.map;
			foreach (IntVec3 c in rect)
			{
				if (!c.Standable(map) || c.GetEdifice(map) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060142C1 RID: 82625 RVA: 0x006030DC File Offset: 0x006012DC
		private bool InteractionCellBlocked(ThingDef thingDef, IntVec3 cell, Rot4 rot, Map map)
		{
			return thingDef.hasInteractionCell && !ThingUtility.InteractionCellWhenAt(thingDef, cell, rot, map).Standable(map);
		}

		// Token: 0x0400DF06 RID: 57094
		private static Rot4[] tmpRotations = new Rot4[]
		{
			Rot4.North,
			Rot4.South,
			Rot4.West,
			Rot4.East
		};
	}
}
