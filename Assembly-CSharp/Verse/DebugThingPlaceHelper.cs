using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B31 RID: 2865
	public static class DebugThingPlaceHelper
	{
		// Token: 0x06004A51 RID: 19025 RVA: 0x0017A614 File Offset: 0x00178814
		public static bool IsDebugSpawnable(ThingDef def, bool allowPlayerBuildable = false)
		{
			return def.forceDebugSpawnable || (!typeof(Corpse).IsAssignableFrom(def.thingClass) && !def.IsBlueprint && !def.IsFrame && def != ThingDefOf.ActiveDropPod && !(def.thingClass == typeof(MinifiedThing)) && !(def.thingClass == typeof(MinifiedTree)) && !(def.thingClass == typeof(UnfinishedThing)) && !def.thingClass.IsSubclassOf(typeof(SignalAction)) && !def.destroyOnDrop && (def.category == ThingCategory.Filth || def.category == ThingCategory.Item || def.category == ThingCategory.Plant || def.category == ThingCategory.Ethereal || (def.category == ThingCategory.Building && def.building.isNaturalRock) || (def.category == ThingCategory.Building && !def.BuildableByPlayer) || (def.category == ThingCategory.Building && def.BuildableByPlayer && allowPlayerBuildable)));
		}

		// Token: 0x06004A52 RID: 19026 RVA: 0x0017A72C File Offset: 0x0017892C
		public static void DebugSpawn(ThingDef def, IntVec3 c, int stackCount = -1, bool direct = false, ThingStyleDef thingStyleDef = null, bool canBeMinified = true, WipeMode? wipeMode = null)
		{
			if (def == ThingDefOf.Fire)
			{
				Fire firstThing = c.GetFirstThing(Find.CurrentMap);
				if (firstThing != null)
				{
					firstThing.fireSize = Mathf.Min(firstThing.fireSize + 0.1f, 1.75f);
					return;
				}
			}
			if (stackCount <= 0)
			{
				stackCount = def.stackLimit;
			}
			ThingDef stuff = GenStuff.RandomStuffFor(def);
			Thing thing = ThingMaker.MakeThing(def, stuff);
			if (thingStyleDef != null)
			{
				thing.StyleDef = thingStyleDef;
			}
			CompQuality compQuality = thing.TryGetComp<CompQuality>();
			if (compQuality != null)
			{
				compQuality.SetQuality(QualityUtility.GenerateQualityRandomEqualChance(), new ArtGenerationContext?(ArtGenerationContext.Colony));
			}
			if (thing.def.Minifiable && canBeMinified)
			{
				thing = thing.MakeMinified(DestroyMode.Vanish);
			}
			if (thing.def.CanHaveFaction)
			{
				if (thing.def.building != null && thing.def.building.isInsectCocoon)
				{
					thing.SetFaction(Faction.OfInsects, null);
				}
				else
				{
					thing.SetFaction(Faction.OfPlayerSilentFail, null);
				}
			}
			thing.stackCount = stackCount;
			if (wipeMode != null)
			{
				GenSpawn.Spawn(def, c, Find.CurrentMap, wipeMode.Value);
			}
			else
			{
				GenPlace.TryPlaceThing(thing, c, Find.CurrentMap, direct ? ThingPlaceMode.Direct : ThingPlaceMode.Near, null, null, null, 1);
			}
			thing.Notify_DebugSpawned();
		}

		// Token: 0x06004A53 RID: 19027 RVA: 0x0017A85C File Offset: 0x00178A5C
		public static List<DebugActionNode> TryPlaceOptionsForStackCount(int stackCount, bool direct)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef3 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && (stackCount < 0 || def.stackLimit >= stackCount)
			select def)
			{
				ThingDef localDef = localDef3;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null, false, null);
				}, null));
			}
			if (stackCount == 1)
			{
				foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
				where def.Minifiable
				select def)
				{
					ThingDef localDef = localDef2;
					list.Add(new DebugActionNode(localDef.defName + " (minified)", DebugActionType.ToolMap, delegate()
					{
						DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null, true, null);
					}, null));
				}
			}
			return list;
		}

		// Token: 0x06004A54 RID: 19028 RVA: 0x0017A9B8 File Offset: 0x00178BB8
		public static List<DebugActionNode> TryPlaceOptionsUnminified()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where def.Minifiable
			select def)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), 1, false, null, false, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06004A55 RID: 19029 RVA: 0x0017AA5C File Offset: 0x00178C5C
		public static List<DebugActionNode> TryPlaceOptionsForBaseMarketValue(float marketValue, bool direct)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false) && def.stackLimit > 1
			select def)
			{
				ThingDef localDef = localDef2;
				int stackCount = (int)(marketValue / localDef.BaseMarketValue);
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), stackCount, direct, null, true, null);
				}, null));
			}
			return list;
		}

		// Token: 0x06004A56 RID: 19030 RVA: 0x0017AB2C File Offset: 0x00178D2C
		public static List<DebugActionNode> SpawnOptions(WipeMode wipeMode)
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, true)
			select def)
			{
				ThingDef localDef = localDef2;
				list.Add(new DebugActionNode(localDef.defName, DebugActionType.ToolMap, delegate()
				{
					DebugThingPlaceHelper.DebugSpawn(localDef, UI.MouseCell(), 1, true, null, true, new WipeMode?(wipeMode));
				}, null));
			}
			return list;
		}

		// Token: 0x06004A57 RID: 19031 RVA: 0x0017ABE8 File Offset: 0x00178DE8
		public static List<DebugActionNode> TryAbandonOptionsForStackCount()
		{
			List<DebugActionNode> list = new List<DebugActionNode>();
			foreach (ThingDef localDef2 in from def in DefDatabase<ThingDef>.AllDefs
			where DebugThingPlaceHelper.IsDebugSpawnable(def, false)
			select def)
			{
				ThingDef localDef = localDef2;
				DebugActionNode debugActionNode = new DebugActionNode();
				debugActionNode.label = localDef.defName;
				debugActionNode.actionType = DebugActionType.Action;
				debugActionNode.AddChild(new DebugActionNode(localDef.defName + " x1", DebugActionType.ToolWorld, delegate()
				{
					DebugThingPlaceHelper.<TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(localDef, GenWorld.MouseTile(false), 1);
				}, null));
				debugActionNode.AddChild(new DebugActionNode(localDef.defName + " full stack", DebugActionType.ToolWorld, delegate()
				{
					DebugThingPlaceHelper.<TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(localDef, GenWorld.MouseTile(false), localDef.stackLimit);
				}, null));
				for (int i = 50; i <= 1000; i += 50)
				{
					int localCount = i;
					debugActionNode.AddChild(new DebugActionNode(localDef.defName + " x" + i.ToString(), DebugActionType.ToolWorld, delegate()
					{
						DebugThingPlaceHelper.<TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(localDef, GenWorld.MouseTile(false), localCount);
					}, null));
				}
				list.Add(debugActionNode);
			}
			return list;
		}

		// Token: 0x06004A58 RID: 19032 RVA: 0x0017AD6C File Offset: 0x00178F6C
		[CompilerGenerated]
		internal static void <TryAbandonOptionsForStackCount>g__DebugAbandon|6_0(ThingDef def, PlanetTile tile, int count)
		{
			while (count > 0)
			{
				Thing thing = ThingMaker.MakeThing(def, null);
				thing.stackCount = Mathf.Max(1, Mathf.Min(count, def.stackLimit));
				count -= thing.stackCount;
				thing.Notify_AbandonedAtTile(tile);
				thing.Destroy(DestroyMode.Vanish);
			}
		}
	}
}
