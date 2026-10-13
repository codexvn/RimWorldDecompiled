using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020008E3 RID: 2275
	public static class ThingOwnerUtility
	{
		// Token: 0x06003A3B RID: 14907 RVA: 0x00135378 File Offset: 0x00133578
		public static bool ThisOrAnyCompIsThingHolder(this ThingDef thingDef)
		{
			if (typeof(IThingHolder).IsAssignableFrom(thingDef.thingClass))
			{
				return true;
			}
			for (int i = 0; i < thingDef.comps.Count; i++)
			{
				if (typeof(IThingHolder).IsAssignableFrom(thingDef.comps[i].compClass))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A3C RID: 14908 RVA: 0x001353DC File Offset: 0x001335DC
		public static ThingOwner TryGetInnerInteractableThingOwner(this Thing thing)
		{
			IThingHolder thingHolder = thing as IThingHolder;
			ThingWithComps thingWithComps = thing as ThingWithComps;
			if (thingHolder != null)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				if (directlyHeldThings != null)
				{
					return directlyHeldThings;
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps = thingWithComps.AllComps;
				for (int i = 0; i < allComps.Count; i++)
				{
					IThingHolder thingHolder2 = allComps[i] as IThingHolder;
					if (thingHolder2 != null)
					{
						ThingOwner directlyHeldThings2 = thingHolder2.GetDirectlyHeldThings();
						if (directlyHeldThings2 != null)
						{
							return directlyHeldThings2;
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			if (thingHolder != null)
			{
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
				{
					ThingOwner directlyHeldThings3 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
					if (directlyHeldThings3 != null)
					{
						ThingOwnerUtility.tmpHolders.Clear();
						return directlyHeldThings3;
					}
				}
			}
			if (thingWithComps != null)
			{
				List<ThingComp> allComps2 = thingWithComps.AllComps;
				for (int j = 0; j < allComps2.Count; j++)
				{
					IThingHolder thingHolder3 = allComps2[j] as IThingHolder;
					if (thingHolder3 != null)
					{
						thingHolder3.GetChildHolders(ThingOwnerUtility.tmpHolders);
						if (ThingOwnerUtility.tmpHolders.Any<IThingHolder>())
						{
							ThingOwner directlyHeldThings4 = ThingOwnerUtility.tmpHolders[0].GetDirectlyHeldThings();
							if (directlyHeldThings4 != null)
							{
								ThingOwnerUtility.tmpHolders.Clear();
								return directlyHeldThings4;
							}
						}
					}
				}
			}
			ThingOwnerUtility.tmpHolders.Clear();
			return null;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x0013550A File Offset: 0x0013370A
		public static bool SpawnedOrAnyParentSpawned(IThingHolder holder)
		{
			return ThingOwnerUtility.SpawnedParentOrMe(holder) != null;
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x00135518 File Offset: 0x00133718
		public static Thing SpawnedParentOrMe(IThingHolder holder)
		{
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Spawned)
				{
					return thing;
				}
				ThingComp thingComp = holder as ThingComp;
				if (thingComp != null && thingComp.parent.Spawned)
				{
					return thingComp.parent;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00135568 File Offset: 0x00133768
		public static IntVec3 GetRootPosition(IThingHolder holder)
		{
			IntVec3 result = IntVec3.Invalid;
			while (holder != null)
			{
				Thing thing = holder as Thing;
				if (thing != null && thing.Position.IsValid)
				{
					result = thing.Position;
				}
				else
				{
					ThingComp thingComp = holder as ThingComp;
					if (thingComp != null && thingComp.parent.Position.IsValid)
					{
						result = thingComp.parent.Position;
					}
				}
				holder = holder.ParentHolder;
			}
			return result;
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x001355D8 File Offset: 0x001337D8
		public static Map GetRootMap(IThingHolder holder)
		{
			while (holder != null)
			{
				Map map = holder as Map;
				if (map != null)
				{
					return map;
				}
				holder = holder.ParentHolder;
			}
			return null;
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x00135600 File Offset: 0x00133800
		public static PlanetTile GetRootTile(IThingHolder holder)
		{
			while (holder != null)
			{
				WorldObject worldObject = holder as WorldObject;
				if (worldObject != null && worldObject.Tile.Valid)
				{
					return worldObject.Tile;
				}
				holder = holder.ParentHolder;
			}
			return PlanetTile.Invalid;
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00135640 File Offset: 0x00133840
		public static bool ContentsSuspended(IThingHolder holder)
		{
			while (holder != null)
			{
				if (!(holder is Building_CryptosleepCasket))
				{
					ISuspendableThingHolder suspendableThingHolder = holder as ISuspendableThingHolder;
					if (suspendableThingHolder == null || !suspendableThingHolder.IsContentsSuspended)
					{
						holder = holder.ParentHolder;
						continue;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x06003A43 RID: 14915 RVA: 0x00135677 File Offset: 0x00133877
		public static bool ContentsInCryptosleep(IThingHolder holder)
		{
			while (holder != null)
			{
				if (holder is Building_CryptosleepCasket)
				{
					return true;
				}
				holder = holder.ParentHolder;
			}
			return false;
		}

		// Token: 0x06003A44 RID: 14916 RVA: 0x00135691 File Offset: 0x00133891
		public static bool IsEnclosingContainer(this IThingHolder holder)
		{
			return holder != null && !(holder is Pawn_CarryTracker) && !(holder is Corpse) && !(holder is Map) && !(holder is Caravan) && !(holder is Settlement_TraderTracker) && !(holder is TradeShip);
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x001356CC File Offset: 0x001338CC
		public static bool ShouldAutoRemoveDestroyedThings(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is Caravan);
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x001356E4 File Offset: 0x001338E4
		public static bool ShouldAutoExtinguishInnerThings(IThingHolder holder)
		{
			return !(holder is Map);
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x001356F2 File Offset: 0x001338F2
		public static bool ShouldRemoveDesignationsOnAddedThings(IThingHolder holder)
		{
			return holder.IsEnclosingContainer();
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x001356FC File Offset: 0x001338FC
		public static void AppendThingHoldersFromThings(List<IThingHolder> outThingsHolders, IList<Thing> container)
		{
			if (container == null)
			{
				return;
			}
			int i = 0;
			int count = container.Count;
			while (i < count)
			{
				IThingHolder thingHolder = container[i] as IThingHolder;
				if (thingHolder != null)
				{
					outThingsHolders.Add(thingHolder);
				}
				ThingWithComps thingWithComps = container[i] as ThingWithComps;
				if (thingWithComps != null)
				{
					List<ThingComp> allComps = thingWithComps.AllComps;
					for (int j = 0; j < allComps.Count; j++)
					{
						IThingHolder thingHolder2 = allComps[j] as IThingHolder;
						if (thingHolder2 != null)
						{
							outThingsHolders.Add(thingHolder2);
						}
					}
				}
				i++;
			}
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00135781 File Offset: 0x00133981
		public static bool AnyParentIs<T>(Thing thing) where T : class, IThingHolder
		{
			return ThingOwnerUtility.GetAnyParent<T>(thing) != null;
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00135794 File Offset: 0x00133994
		public static T GetAnyParent<T>(Thing thing) where T : class, IThingHolder
		{
			T t = thing as T;
			if (t != null)
			{
				return t;
			}
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				T t2 = parentHolder as T;
				if (t2 != null)
				{
					return t2;
				}
			}
			return default(T);
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x001357EC File Offset: 0x001339EC
		public static Thing GetFirstParentThing(Thing thing)
		{
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				Thing thing2 = parentHolder as Thing;
				if (thing2 != null)
				{
					return thing2;
				}
				ThingComp thingComp = parentHolder as ThingComp;
				if (thingComp != null)
				{
					return thingComp.parent;
				}
			}
			return null;
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x0013582C File Offset: 0x00133A2C
		public static Thing GetFirstSpawnedParentThing(Thing thing)
		{
			if (thing.Spawned)
			{
				return thing;
			}
			for (IThingHolder parentHolder = thing.ParentHolder; parentHolder != null; parentHolder = parentHolder.ParentHolder)
			{
				Thing thing2 = parentHolder as Thing;
				if (thing2 != null && thing2.Spawned)
				{
					return thing2;
				}
				ThingComp thingComp = parentHolder as ThingComp;
				if (thingComp != null && thingComp.parent.Spawned)
				{
					return thingComp.parent;
				}
			}
			return null;
		}

		// Token: 0x06003A4D RID: 14925 RVA: 0x0013588C File Offset: 0x00133A8C
		public static void GetAllThingsRecursively(IThingHolder holder, List<Thing> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null)
		{
			outThings.Clear();
			if (passCheck != null && !passCheck(holder))
			{
				return;
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpStack.Push(holder);
			while (ThingOwnerUtility.tmpStack.Count != 0)
			{
				IThingHolder thingHolder = ThingOwnerUtility.tmpStack.Pop();
				if (allowUnreal || ThingOwnerUtility.AreImmediateContentsReal(thingHolder))
				{
					ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
					if (directlyHeldThings != null)
					{
						outThings.AddRange(directlyHeldThings);
					}
				}
				ThingOwnerUtility.tmpHolders.Clear();
				thingHolder.GetChildHolders(ThingOwnerUtility.tmpHolders);
				for (int i = 0; i < ThingOwnerUtility.tmpHolders.Count; i++)
				{
					if (passCheck == null || passCheck(ThingOwnerUtility.tmpHolders[i]))
					{
						ThingOwnerUtility.tmpStack.Push(ThingOwnerUtility.tmpHolders[i]);
					}
				}
			}
			ThingOwnerUtility.tmpStack.Clear();
			ThingOwnerUtility.tmpHolders.Clear();
		}

		// Token: 0x06003A4E RID: 14926 RVA: 0x00135964 File Offset: 0x00133B64
		public static void GetAllThingsRecursively<T>(Map map, ThingRequest request, List<T> outThings, bool allowUnreal = true, Predicate<IThingHolder> passCheck = null, bool alsoGetSpawnedThings = true) where T : Thing
		{
			outThings.Clear();
			if (alsoGetSpawnedThings)
			{
				List<Thing> list = map.listerThings.ThingsMatching(request);
				for (int i = 0; i < list.Count; i++)
				{
					T t = list[i] as T;
					if (t != null)
					{
						outThings.Add(t);
					}
				}
			}
			ThingOwnerUtility.tmpMapChildHolders.Clear();
			map.GetChildHolders(ThingOwnerUtility.tmpMapChildHolders);
			for (int j = 0; j < ThingOwnerUtility.tmpMapChildHolders.Count; j++)
			{
				ThingOwnerUtility.tmpThings.Clear();
				ThingOwnerUtility.GetAllThingsRecursively(ThingOwnerUtility.tmpMapChildHolders[j], ThingOwnerUtility.tmpThings, allowUnreal, passCheck);
				for (int k = 0; k < ThingOwnerUtility.tmpThings.Count; k++)
				{
					T t2 = ThingOwnerUtility.tmpThings[k] as T;
					if (t2 != null && request.Accepts(t2))
					{
						outThings.Add(t2);
					}
				}
			}
			ThingOwnerUtility.tmpThings.Clear();
			ThingOwnerUtility.tmpMapChildHolders.Clear();
		}

		// Token: 0x06003A4F RID: 14927 RVA: 0x00135A70 File Offset: 0x00133C70
		public static List<Thing> GetAllThingsRecursively(IThingHolder holder, bool allowUnreal = true)
		{
			List<Thing> list = new List<Thing>();
			ThingOwnerUtility.GetAllThingsRecursively(holder, list, allowUnreal, null);
			return list;
		}

		// Token: 0x06003A50 RID: 14928 RVA: 0x00135A8D File Offset: 0x00133C8D
		public static bool AreImmediateContentsReal(IThingHolder holder)
		{
			return !(holder is Corpse) && !(holder is MinifiedThing);
		}

		// Token: 0x06003A51 RID: 14929 RVA: 0x00135AA8 File Offset: 0x00133CA8
		public static bool TryGetFixedTemperature(IThingHolder holder, Thing forThing, out float temperature)
		{
			if (holder is Pawn_InventoryTracker && forThing.TryGetComp<CompHatcher>() != null)
			{
				temperature = 14f;
				return true;
			}
			if (holder is CompLaunchable || holder is ActiveTransporterInfo || holder is TravellingTransporters)
			{
				temperature = 14f;
				return true;
			}
			if (holder is Settlement_TraderTracker || holder is TradeShip)
			{
				temperature = 14f;
				return true;
			}
			if (holder is CompTransporter)
			{
				temperature = 14f;
				return true;
			}
			temperature = 21f;
			return false;
		}

		// Token: 0x04002A87 RID: 10887
		private static readonly Stack<IThingHolder> tmpStack = new Stack<IThingHolder>();

		// Token: 0x04002A88 RID: 10888
		private static readonly List<IThingHolder> tmpHolders = new List<IThingHolder>();

		// Token: 0x04002A89 RID: 10889
		private static readonly List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04002A8A RID: 10890
		private static readonly List<IThingHolder> tmpMapChildHolders = new List<IThingHolder>();
	}
}
