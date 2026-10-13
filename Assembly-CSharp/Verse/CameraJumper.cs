using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020001D0 RID: 464
	public static class CameraJumper
	{
		// Token: 0x06000CB2 RID: 3250 RVA: 0x0003B82B File Offset: 0x00039A2B
		public static void TryJumpAndSelect(GlobalTargetInfo target, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			if (!target.IsValid)
			{
				return;
			}
			CameraJumper.TryJump(target, mode);
			CameraJumper.TrySelect(target, false);
		}

		// Token: 0x06000CB3 RID: 3251 RVA: 0x0003B848 File Offset: 0x00039A48
		public static void TrySelect(GlobalTargetInfo target, bool skipTargetAdjustment = false)
		{
			if (!target.IsValid)
			{
				return;
			}
			if (!skipTargetAdjustment)
			{
				target = CameraJumper.GetAdjustedTarget(target);
			}
			if (target.HasThing)
			{
				CameraJumper.TrySelectInternal(target.Thing);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TrySelectInternal(target.WorldObject);
			}
		}

		// Token: 0x06000CB4 RID: 3252 RVA: 0x0003B898 File Offset: 0x00039A98
		private static void TrySelectInternal(Thing thing)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (thing.Spawned && thing.def.selectable)
			{
				bool flag = CameraJumper.TryHideWorld();
				bool flag2 = false;
				if (thing.Map != Find.CurrentMap)
				{
					Current.Game.CurrentMap = thing.Map;
					flag2 = true;
					if (!flag)
					{
						SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
					}
				}
				if (flag || flag2)
				{
					Find.CameraDriver.JumpToCurrentMapLoc(thing.Position);
				}
				Find.Selector.ClearSelection();
				Find.Selector.Select(thing, true, true);
			}
		}

		// Token: 0x06000CB5 RID: 3253 RVA: 0x0003B924 File Offset: 0x00039B24
		private static void TrySelectInternal(WorldObject worldObject)
		{
			if (Find.World == null)
			{
				return;
			}
			if (worldObject.Spawned && worldObject.SelectableNow)
			{
				CameraJumper.TryShowWorld();
				Find.WorldSelector.ClearSelection();
				Find.WorldSelector.Select(worldObject, true);
			}
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x0003B95C File Offset: 0x00039B5C
		public static void TryJump(GlobalTargetInfo target, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			if (!target.IsValid)
			{
				return;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				CameraJumper.TryJumpInternal(target.Thing, mode);
				return;
			}
			if (target.HasWorldObject)
			{
				CameraJumper.TryJumpInternal(target.WorldObject);
				return;
			}
			if (target.Cell.IsValid)
			{
				CameraJumper.TryJumpInternal(target.Cell, target.Map, mode);
				return;
			}
			CameraJumper.TryJumpInternal(target.Tile);
		}

		// Token: 0x06000CB7 RID: 3255 RVA: 0x0003B9DA File Offset: 0x00039BDA
		public static void TryJump(IntVec3 cell, Map map, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(cell, map, false), mode);
		}

		// Token: 0x06000CB8 RID: 3256 RVA: 0x0003B9EA File Offset: 0x00039BEA
		public static void TryJump(PlanetTile tile, CameraJumper.MovementMode mode = CameraJumper.MovementMode.Pan)
		{
			CameraJumper.TryJump(new GlobalTargetInfo(tile), mode);
		}

		// Token: 0x06000CB9 RID: 3257 RVA: 0x0003B9F8 File Offset: 0x00039BF8
		private static void TryJumpInternal(Thing thing, CameraJumper.MovementMode mode)
		{
			CameraJumper.TryJumpInternal(thing.PositionHeld, thing.MapHeld, mode);
		}

		// Token: 0x06000CBA RID: 3258 RVA: 0x0003BA0C File Offset: 0x00039C0C
		private static void TryJumpInternal(IntVec3 cell, Map map, CameraJumper.MovementMode mode)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (!cell.IsValid)
			{
				return;
			}
			if (map == null || !Find.Maps.Contains(map))
			{
				return;
			}
			if (!cell.InBounds(map))
			{
				return;
			}
			bool flag = CameraJumper.TryHideWorld();
			bool flag2 = false;
			if (Find.CurrentMap != map)
			{
				Current.Game.CurrentMap = map;
				flag2 = true;
				if (!flag)
				{
					SoundDefOf.MapSelected.PlayOneShotOnCamera(null);
				}
			}
			CameraJumper.JumpLocalInternal(cell, (!Prefs.SmoothCameraJumps || flag || flag2) ? CameraJumper.MovementMode.Cut : mode);
		}

		// Token: 0x06000CBB RID: 3259 RVA: 0x0003BA8A File Offset: 0x00039C8A
		private static void TryJumpInternal(WorldObject worldObject)
		{
			CameraJumper.TryJumpInternal(worldObject.Tile);
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x0003BA97 File Offset: 0x00039C97
		private static void TryJumpInternal(PlanetTile tile)
		{
			if (Find.World == null)
			{
				return;
			}
			if (!tile.Valid)
			{
				return;
			}
			CameraJumper.TryShowWorld();
			Find.WorldCameraDriver.JumpTo(tile);
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0003BABC File Offset: 0x00039CBC
		private static void JumpLocalInternal(IntVec3 localCell, CameraJumper.MovementMode mode)
		{
			if (mode != CameraJumper.MovementMode.Pan)
			{
				if (mode != CameraJumper.MovementMode.Cut)
				{
				}
				Find.CameraDriver.JumpToCurrentMapLoc(localCell);
				return;
			}
			Find.CameraDriver.PanToMapLoc(localCell);
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x0003BAE0 File Offset: 0x00039CE0
		public static bool CanJump(GlobalTargetInfo target)
		{
			if (!target.IsValid)
			{
				return false;
			}
			target = CameraJumper.GetAdjustedTarget(target);
			if (target.HasThing)
			{
				return target.Thing.MapHeld != null && Find.Maps.Contains(target.Thing.MapHeld) && target.Thing.PositionHeld.IsValid && target.Thing.PositionHeld.InBounds(target.Thing.MapHeld);
			}
			if (target.HasWorldObject)
			{
				return target.WorldObject.Spawned;
			}
			if (target.Cell.IsValid)
			{
				return target.Map != null && Find.Maps.Contains(target.Map) && target.Cell.IsValid && target.Cell.InBounds(target.Map);
			}
			return target.Tile.Valid;
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x0003BBE0 File Offset: 0x00039DE0
		public static GlobalTargetInfo GetAdjustedTarget(GlobalTargetInfo target)
		{
			if (target.HasThing)
			{
				Thing thing = target.Thing;
				if (thing.Spawned)
				{
					return thing;
				}
				GlobalTargetInfo result = GlobalTargetInfo.Invalid;
				for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
				{
					Thing thing2 = parentHolder as Thing;
					if (thing2 != null && thing2.Spawned)
					{
						result = thing2;
						break;
					}
					ThingComp thingComp = parentHolder as ThingComp;
					if (thingComp != null && thingComp.parent.Spawned)
					{
						result = thingComp.parent;
						break;
					}
					WorldObject worldObject = parentHolder as WorldObject;
					if (worldObject != null && worldObject.Spawned)
					{
						result = worldObject;
						break;
					}
				}
				if (result.IsValid)
				{
					return result;
				}
				if (target.Thing.TryGetComp<CompCauseGameCondition>() != null)
				{
					List<Site> sites = Find.WorldObjects.Sites;
					for (int i = 0; i < sites.Count; i++)
					{
						for (int j = 0; j < sites[i].parts.Count; j++)
						{
							if (sites[i].parts[j].conditionCauser == target.Thing)
							{
								return sites[i];
							}
						}
					}
				}
				if (thing.Tile.Valid)
				{
					return new GlobalTargetInfo(thing.Tile);
				}
			}
			else if (target.Cell.IsValid && target.Tile.Valid && target.Map != null && !Find.Maps.Contains(target.Map))
			{
				MapParent parent = target.Map.Parent;
				if (parent != null && parent.Spawned)
				{
					return parent;
				}
				return GlobalTargetInfo.Invalid;
			}
			return target;
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x0003BDA4 File Offset: 0x00039FA4
		public static GlobalTargetInfo GetWorldTarget(GlobalTargetInfo target)
		{
			GlobalTargetInfo adjustedTarget = CameraJumper.GetAdjustedTarget(target);
			if (!adjustedTarget.IsValid)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (adjustedTarget.IsWorldTarget)
			{
				return adjustedTarget;
			}
			return CameraJumper.GetWorldTargetOfMap(adjustedTarget.Map);
		}

		// Token: 0x06000CC1 RID: 3265 RVA: 0x0003BDDE File Offset: 0x00039FDE
		public static GlobalTargetInfo GetWorldTargetOfMap(Map map)
		{
			if (map == null)
			{
				return GlobalTargetInfo.Invalid;
			}
			if (map.Parent != null && map.Parent.Spawned)
			{
				return map.Parent;
			}
			return GlobalTargetInfo.Invalid;
		}

		// Token: 0x06000CC2 RID: 3266 RVA: 0x0003BE10 File Offset: 0x0003A010
		public static bool TryHideWorld()
		{
			if (!WorldRendererUtility.WorldSelected)
			{
				return false;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode != WorldRenderMode.None)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.None;
				SoundDefOf.TabClose.PlayOneShotOnCamera(null);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				return true;
			}
			return false;
		}

		// Token: 0x06000CC3 RID: 3267 RVA: 0x0003BE6C File Offset: 0x0003A06C
		public static bool TryShowWorld()
		{
			if (WorldRendererUtility.WorldSelected)
			{
				return true;
			}
			if (Current.ProgramState != ProgramState.Playing)
			{
				return false;
			}
			if (Find.World.renderer.wantedMode == WorldRenderMode.None)
			{
				AmbientSoundManager.EnsureWorldAmbientSoundCreated();
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				SoundDefOf.TabOpen.PlayOneShotOnCamera(null);
				Find.MainTabsRoot.EscapeCurrentTab(false);
				return true;
			}
			return false;
		}

		// Token: 0x020001D1 RID: 465
		public enum MovementMode
		{
			// Token: 0x04000743 RID: 1859
			Pan,
			// Token: 0x04000744 RID: 1860
			Cut
		}
	}
}
