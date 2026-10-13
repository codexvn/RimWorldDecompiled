using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002945 RID: 10565
	public static class MinifyUtility
	{
		// Token: 0x0600E767 RID: 59239 RVA: 0x00436E24 File Offset: 0x00435024
		public static MinifiedThing MakeMinified(this Thing thing, DestroyMode destroyMode = DestroyMode.Vanish)
		{
			if (!thing.def.Minifiable)
			{
				Log.Warning("Tried to minify " + ((thing != null) ? thing.ToString() : null) + " which is not minifiable.");
				return null;
			}
			thing.DeSpawnOrDeselect(destroyMode);
			if (thing.holdingOwner != null)
			{
				string str = "Can't minify thing which is in a ThingOwner because we don't know how to handle it. Remove it from the container first. holder=";
				IThingHolder parentHolder = thing.ParentHolder;
				Log.Warning(str + ((parentHolder != null) ? parentHolder.ToString() : null));
				return null;
			}
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(thing);
			MinifiedThing minifiedThing = (MinifiedThing)ThingMaker.MakeThing(thing.def.minifiedDef, null);
			minifiedThing.InnerThing = thing;
			if (blueprint_Install != null)
			{
				blueprint_Install.SetThingToInstallFromMinified(minifiedThing);
			}
			if (minifiedThing.InnerThing.stackCount > 1)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to minify ",
					thing.LabelCap,
					" with stack count ",
					minifiedThing.InnerThing.stackCount.ToString(),
					". Clamped stack count to 1."
				}));
				minifiedThing.InnerThing.stackCount = 1;
			}
			return minifiedThing;
		}

		// Token: 0x0600E768 RID: 59240 RVA: 0x00436F22 File Offset: 0x00435122
		public static Thing TryMakeMinified(this Thing thing)
		{
			if (thing.def.Minifiable)
			{
				return thing.MakeMinified(DestroyMode.Vanish);
			}
			return thing;
		}

		// Token: 0x0600E769 RID: 59241 RVA: 0x00436F3C File Offset: 0x0043513C
		public static Thing GetInnerIfMinified(this Thing outerThing)
		{
			MinifiedThing minifiedThing = outerThing as MinifiedThing;
			if (minifiedThing != null)
			{
				return minifiedThing.InnerThing;
			}
			return outerThing;
		}

		// Token: 0x0600E76A RID: 59242 RVA: 0x00436F5C File Offset: 0x0043515C
		public static MinifiedThing Uninstall(this Thing th)
		{
			if (!th.Spawned)
			{
				Log.Warning("Can't uninstall unspawned thing " + ((th != null) ? th.ToString() : null));
				return null;
			}
			bool flag = Find.Selector.IsSelected(th);
			Map map = th.Map;
			MinifiedThing minifiedThing = th.MakeMinified(DestroyMode.Vanish);
			GenPlace.TryPlaceThing(minifiedThing, th.Position, map, ThingPlaceMode.Near, null, null, null, 1);
			SoundDefOf.ThingUninstalled.PlayOneShot(new TargetInfo(th.Position, map, false));
			if (flag)
			{
				Find.Selector.Select(minifiedThing, false, false);
			}
			return minifiedThing;
		}
	}
}
