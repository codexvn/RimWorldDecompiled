using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008E0 RID: 2272
	public abstract class ThingOwner : IExposable, IList<Thing>, ICollection<Thing>, IEnumerable<Thing>, IEnumerable
	{
		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x00134461 File Offset: 0x00132661
		public IThingHolder Owner
		{
			get
			{
				return this.owner;
			}
		}

		// Token: 0x17000AD1 RID: 2769
		// (get) Token: 0x060039F7 RID: 14839
		public abstract int Count { get; }

		// Token: 0x17000AD2 RID: 2770
		public Thing this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
		}

		// Token: 0x17000AD3 RID: 2771
		// (get) Token: 0x060039F9 RID: 14841 RVA: 0x00134472 File Offset: 0x00132672
		public bool Any
		{
			get
			{
				return this.Count > 0;
			}
		}

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060039FA RID: 14842 RVA: 0x00134480 File Offset: 0x00132680
		// (remove) Token: 0x060039FB RID: 14843 RVA: 0x001344B8 File Offset: 0x001326B8
		public event Action OnContentsChanged;

		// Token: 0x17000AD4 RID: 2772
		// (get) Token: 0x060039FC RID: 14844 RVA: 0x001344F0 File Offset: 0x001326F0
		public int TotalStackCount
		{
			get
			{
				int num = 0;
				int count = this.Count;
				for (int i = 0; i < count; i++)
				{
					num += this.GetAt(i).stackCount;
				}
				return num;
			}
		}

		// Token: 0x17000AD5 RID: 2773
		// (get) Token: 0x060039FD RID: 14845 RVA: 0x00134522 File Offset: 0x00132722
		public string ContentsString
		{
			get
			{
				if (this.Any)
				{
					return GenThing.ThingsToCommaList(this, false, true, -1);
				}
				return "NothingLower".Translate();
			}
		}

		// Token: 0x17000AD6 RID: 2774
		Thing IList<Thing>.this[int index]
		{
			get
			{
				return this.GetAt(index);
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		// Token: 0x17000AD7 RID: 2775
		// (get) Token: 0x06003A00 RID: 14848 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool ICollection<Thing>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00134545 File Offset: 0x00132745
		public ThingOwner()
		{
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00134566 File Offset: 0x00132766
		public ThingOwner(IThingHolder owner)
		{
			this.owner = owner;
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x0013458E File Offset: 0x0013278E
		public ThingOwner(IThingHolder owner, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true)
		{
			this.owner = owner;
			this.contentsLookMode = contentsLookMode;
			this.removeContentsIfDestroyed = removeContentsIfDestroyed;
		}

		// Token: 0x06003A04 RID: 14852 RVA: 0x001345C4 File Offset: 0x001327C4
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true) : this(owner)
		{
			this.maxStacks = (oneStackOnly ? 1 : 999999);
			this.contentsLookMode = contentsLookMode;
			this.removeContentsIfDestroyed = removeContentsIfDestroyed;
		}

		// Token: 0x06003A05 RID: 14853 RVA: 0x001345F0 File Offset: 0x001327F0
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.maxStacks, "maxStacks", 999999, false);
			Scribe_Values.Look<LookMode>(ref this.contentsLookMode, "contentsLookMode", LookMode.Deep, false);
			Scribe_Values.Look<bool>(ref this.removeContentsIfDestroyed, "removeContentsIfDestroyed", true, false);
			Scribe_Values.Look<bool>(ref this.dontTickContents, "dontTickContents", false, false);
		}

		// Token: 0x06003A06 RID: 14854 RVA: 0x0013464C File Offset: 0x0013284C
		public void DoTick()
		{
			if (this.dontTickContents)
			{
				return;
			}
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing at = this.GetAt(i);
				at.DoTick();
				if (at.Destroyed && this.removeContentsIfDestroyed)
				{
					this.Remove(at);
				}
			}
		}

		// Token: 0x06003A07 RID: 14855 RVA: 0x0013469C File Offset: 0x0013289C
		public void Clear()
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.Remove(this.GetAt(i));
			}
		}

		// Token: 0x06003A08 RID: 14856 RVA: 0x001346CC File Offset: 0x001328CC
		public void ClearAndDestroyContents(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int i = this.Count - 1; i >= 0; i--)
				{
					Thing at = this.GetAt(i);
					at.Destroy(mode);
					this.Remove(at);
				}
			}
		}

		// Token: 0x06003A09 RID: 14857 RVA: 0x00134710 File Offset: 0x00132910
		public void ClearAndDestroyContentsOrPassToWorld(DestroyMode mode = DestroyMode.Vanish)
		{
			while (this.Any)
			{
				for (int i = this.Count - 1; i >= 0; i--)
				{
					Thing at = this.GetAt(i);
					at.DestroyOrPassToWorld(mode);
					this.Remove(at);
				}
			}
		}

		// Token: 0x06003A0A RID: 14858 RVA: 0x00134751 File Offset: 0x00132951
		public bool CanAcceptAnyOf(Thing item, bool canMergeWithExistingStacks = true)
		{
			return this.GetCountCanAccept(item, canMergeWithExistingStacks) > 0;
		}

		// Token: 0x06003A0B RID: 14859 RVA: 0x00134760 File Offset: 0x00132960
		public virtual int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null || item.stackCount <= 0)
			{
				return 0;
			}
			if (this.maxStacks == 999999)
			{
				return item.stackCount;
			}
			int num = 0;
			if (this.Count < this.maxStacks)
			{
				num += (this.maxStacks - this.Count) * item.def.stackLimit;
			}
			if (num >= item.stackCount)
			{
				return Mathf.Min(num, item.stackCount);
			}
			if (canMergeWithExistingStacks)
			{
				int i = 0;
				int count = this.Count;
				while (i < count)
				{
					Thing at = this.GetAt(i);
					if (at.stackCount < at.def.stackLimit && at.CanStackWith(item))
					{
						num += at.def.stackLimit - at.stackCount;
						if (num >= item.stackCount)
						{
							return Mathf.Min(num, item.stackCount);
						}
					}
					i++;
				}
			}
			return Mathf.Min(num, item.stackCount);
		}

		// Token: 0x06003A0C RID: 14860
		public abstract int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true);

		// Token: 0x06003A0D RID: 14861
		public abstract bool TryAdd(Thing item, bool canMergeWithExistingStacks = true);

		// Token: 0x06003A0E RID: 14862
		public abstract int IndexOf(Thing item);

		// Token: 0x06003A0F RID: 14863
		public abstract bool Remove(Thing item);

		// Token: 0x06003A10 RID: 14864
		protected abstract Thing GetAt(int index);

		// Token: 0x06003A11 RID: 14865 RVA: 0x00134842 File Offset: 0x00132A42
		public bool Contains(Thing item)
		{
			return item != null && item.holdingOwner == this;
		}

		// Token: 0x06003A12 RID: 14866 RVA: 0x00134852 File Offset: 0x00132A52
		public void RemoveAt(int index)
		{
			if (index < 0 || index >= this.Count)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			this.Remove(this.GetAt(index));
		}

		// Token: 0x06003A13 RID: 14867 RVA: 0x0013487A File Offset: 0x00132A7A
		public int TryAddOrTransfer(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				return 0;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, count, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, count, canMergeWithExistingStacks);
		}

		// Token: 0x06003A14 RID: 14868 RVA: 0x001348AC File Offset: 0x00132AAC
		public bool TryAddOrTransfer(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add or transfer null item to ThingOwner.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				return item.holdingOwner.TryTransferToContainer(item, this, canMergeWithExistingStacks);
			}
			return this.TryAdd(item, canMergeWithExistingStacks);
		}

		// Token: 0x06003A15 RID: 14869 RVA: 0x001348DC File Offset: 0x00132ADC
		public void TryAddRangeOrTransfer(IEnumerable<Thing> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
		{
			if (things == this)
			{
				return;
			}
			ThingOwner thingOwner = things as ThingOwner;
			if (thingOwner != null)
			{
				thingOwner.TryTransferAllToContainer(this, canMergeWithExistingStacks);
				if (destroyLeftover)
				{
					thingOwner.ClearAndDestroyContents(DestroyMode.Vanish);
					return;
				}
			}
			else
			{
				IList<Thing> list = things as IList<Thing>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!this.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					return;
				}
				foreach (Thing thing in things)
				{
					if (!this.TryAddOrTransfer(thing, canMergeWithExistingStacks) && destroyLeftover)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06003A16 RID: 14870 RVA: 0x0013499C File Offset: 0x00132B9C
		public int RemoveAll(Predicate<Thing> predicate)
		{
			int num = 0;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				if (predicate(this.GetAt(i)))
				{
					this.Remove(this.GetAt(i));
					num++;
				}
			}
			return num;
		}

		// Token: 0x06003A17 RID: 14871 RVA: 0x001349E0 File Offset: 0x00132BE0
		public bool TryTransferToContainer(Thing item, ThingOwner otherContainer, bool canMergeWithExistingStacks = true)
		{
			return this.TryTransferToContainer(item, otherContainer, item.stackCount, canMergeWithExistingStacks) == item.stackCount;
		}

		// Token: 0x06003A18 RID: 14872 RVA: 0x001349FC File Offset: 0x00132BFC
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			return this.TryTransferToContainer(item, otherContainer, count, out thing, canMergeWithExistingStacks);
		}

		// Token: 0x06003A19 RID: 14873 RVA: 0x00134A18 File Offset: 0x00132C18
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int count, out Thing resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			if (!this.Contains(item))
			{
				Log.Error("Can't transfer item " + ((item != null) ? item.ToString() : null) + " because it's not here. owner=" + this.owner.ToStringSafe<IThingHolder>());
				resultingTransferredItem = null;
				return 0;
			}
			if (otherContainer == this && count > 0)
			{
				resultingTransferredItem = item;
				return item.stackCount;
			}
			if (!otherContainer.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				resultingTransferredItem = null;
				return 0;
			}
			if (count <= 0)
			{
				resultingTransferredItem = null;
				return 0;
			}
			if (this.owner is Map || otherContainer.owner is Map)
			{
				Log.Warning("Can't transfer items to or from Maps directly. They must be spawned or despawned manually. Use TryAdd(item.SplitOff(count))");
				resultingTransferredItem = null;
				return 0;
			}
			int num = Mathf.Min(item.stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.Contains(thing))
			{
				this.Remove(thing);
			}
			if (otherContainer.TryAdd(thing, canMergeWithExistingStacks))
			{
				resultingTransferredItem = thing;
				Map mapHeld = item.MapHeld;
				if (mapHeld != null)
				{
					ResourceCounter resourceCounter = mapHeld.resourceCounter;
					if (resourceCounter != null)
					{
						resourceCounter.CheckUpdateResource(thing);
					}
				}
				return thing.stackCount;
			}
			resultingTransferredItem = null;
			if (otherContainer.Contains(thing) || thing.stackCount <= 0 || thing.Destroyed)
			{
				return thing.stackCount;
			}
			int result = num - thing.stackCount;
			if (item != thing)
			{
				item.TryAbsorbStack(thing, false);
			}
			else
			{
				this.TryAdd(thing, false);
			}
			Map mapHeld2 = item.MapHeld;
			if (mapHeld2 == null)
			{
				return result;
			}
			ResourceCounter resourceCounter2 = mapHeld2.resourceCounter;
			if (resourceCounter2 == null)
			{
				return result;
			}
			resourceCounter2.CheckUpdateResource(thing);
			return result;
		}

		// Token: 0x06003A1A RID: 14874 RVA: 0x00134B74 File Offset: 0x00132D74
		public void TryTransferAllToContainer(ThingOwner other, bool canMergeWithExistingStacks = true)
		{
			for (int i = this.Count - 1; i >= 0; i--)
			{
				this.TryTransferToContainer(this.GetAt(i), other, canMergeWithExistingStacks);
			}
		}

		// Token: 0x06003A1B RID: 14875 RVA: 0x00134BA4 File Offset: 0x00132DA4
		public Thing Take(Thing thing, int count)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to take " + thing.ToStringSafe<Thing>() + " but it's not here.");
				return null;
			}
			if (count > thing.stackCount)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to get ",
					count.ToString(),
					" of ",
					thing.ToStringSafe<Thing>(),
					" while only having ",
					thing.stackCount.ToString()
				}));
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				this.Remove(thing);
				return thing;
			}
			Thing thing2 = thing.SplitOff(count);
			thing2.holdingOwner = null;
			return thing2;
		}

		// Token: 0x06003A1C RID: 14876 RVA: 0x00134C51 File Offset: 0x00132E51
		public Thing Take(Thing thing)
		{
			return this.Take(thing, thing.stackCount);
		}

		// Token: 0x06003A1D RID: 14877 RVA: 0x00134C60 File Offset: 0x00132E60
		public bool TryDrop(Thing thing, ThingPlaceMode mode, int count, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + ((thing != null) ? thing.ToString() : null) + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, count, out lastResultingThing, placedAction, nearPlaceValidator);
		}

		// Token: 0x06003A1E RID: 14878 RVA: 0x00134CCC File Offset: 0x00132ECC
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out Thing resultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			if (!this.Contains(thing))
			{
				Log.Error("Tried to drop " + thing.ToStringSafe<Thing>() + " but it's not here.");
				resultingThing = null;
				return false;
			}
			if (thing.stackCount < count)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to drop ",
					count.ToString(),
					" of ",
					(thing != null) ? thing.ToString() : null,
					" while only having ",
					thing.stackCount.ToString()
				}));
				count = thing.stackCount;
			}
			if (count == thing.stackCount)
			{
				if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					this.Remove(thing);
					return true;
				}
				return false;
			}
			else
			{
				Thing thing2 = thing.SplitOff(count);
				if (GenDrop.TryDropSpawn(thing2, dropLoc, map, mode, out resultingThing, placedAction, nearPlaceValidator, true))
				{
					return true;
				}
				thing.TryAbsorbStack(thing2, false);
				return false;
			}
		}

		// Token: 0x06003A1F RID: 14879 RVA: 0x00134DB8 File Offset: 0x00132FB8
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Map rootMap = ThingOwnerUtility.GetRootMap(this.owner);
			IntVec3 rootPosition = ThingOwnerUtility.GetRootPosition(this.owner);
			if (rootMap == null || !rootPosition.IsValid)
			{
				Log.Error("Cannot drop " + ((thing != null) ? thing.ToString() : null) + " without a dropLoc and with an owner whose map is null.");
				lastResultingThing = null;
				return false;
			}
			return this.TryDrop(thing, rootPosition, rootMap, mode, out lastResultingThing, placedAction, nearPlaceValidator, true);
		}

		// Token: 0x06003A20 RID: 14880 RVA: 0x00134E20 File Offset: 0x00133020
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out Thing lastResultingThing, Action<Thing, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			if (!this.Contains(thing))
			{
				Log.Error(this.owner.ToStringSafe<IThingHolder>() + " container tried to drop  " + thing.ToStringSafe<Thing>() + " which it didn't contain.");
				lastResultingThing = null;
				return false;
			}
			if (GenDrop.TryDropSpawn(thing, dropLoc, map, mode, out lastResultingThing, placedAction, nearPlaceValidator, playDropSound))
			{
				this.Remove(thing);
				return true;
			}
			return false;
		}

		// Token: 0x06003A21 RID: 14881 RVA: 0x00134E80 File Offset: 0x00133080
		public bool TryDropAll(IntVec3 dropLoc, Map map, ThingPlaceMode mode, Action<Thing, int> placeAction = null, Predicate<IntVec3> nearPlaceValidator = null, bool playDropSound = true)
		{
			bool result = true;
			for (int i = this.Count - 1; i >= 0; i--)
			{
				Thing thing;
				if (!this.TryDrop(this.GetAt(i), dropLoc, map, mode, out thing, placeAction, nearPlaceValidator, playDropSound))
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x00134EBF File Offset: 0x001330BF
		public bool Contains(ThingDef def)
		{
			return this.Contains(def, 1);
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x00134ECC File Offset: 0x001330CC
		public bool Contains(ThingDef def, int minCount)
		{
			if (minCount <= 0)
			{
				return true;
			}
			int num = 0;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.GetAt(i).def == def)
				{
					num += this.GetAt(i).stackCount;
				}
				if (num >= minCount)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003A24 RID: 14884 RVA: 0x00134F1C File Offset: 0x0013311C
		public int TotalStackCountOfDef(ThingDef def)
		{
			int num = 0;
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				if (this.GetAt(i).def == def)
				{
					num += this.GetAt(i).stackCount;
				}
			}
			return num;
		}

		// Token: 0x06003A25 RID: 14885 RVA: 0x00134F5D File Offset: 0x0013315D
		public void Notify_ContainedItemDestroyed(Thing t)
		{
			if (ThingOwnerUtility.ShouldAutoRemoveDestroyedThings(this.owner))
			{
				this.Remove(t);
			}
		}

		// Token: 0x06003A26 RID: 14886 RVA: 0x00134F74 File Offset: 0x00133174
		protected virtual void NotifyAdded(Thing item)
		{
			if (ThingOwnerUtility.ShouldAutoExtinguishInnerThings(this.owner) && item.HasAttachment(ThingDefOf.Fire))
			{
				item.GetAttachment(ThingDefOf.Fire).Destroy(DestroyMode.Vanish);
			}
			if (ThingOwnerUtility.ShouldRemoveDesignationsOnAddedThings(this.owner))
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					maps[i].designationManager.RemoveAllDesignationsOn(item, false);
				}
			}
			Thing thing = this.owner as Thing;
			if (thing == null || !thing.Faction.IsPlayerSafe())
			{
				WorldObject worldObject = this.owner as WorldObject;
				if (worldObject == null || !worldObject.Faction.IsPlayerSafe())
				{
					Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
					if (pawn_InventoryTracker == null || !pawn_InventoryTracker.pawn.Faction.IsPlayerSafe())
					{
						goto IL_CA;
					}
				}
			}
			item.EverSeenByPlayer = true;
			IL_CA:
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAdded(item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnAdded((Pawn)item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelAdded((Apparel)item);
				if (pawn_ApparelTracker.pawn.Faction.IsPlayerSafe())
				{
					item.EverSeenByPlayer = true;
				}
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentAdded((ThingWithComps)item);
				if (pawn_EquipmentTracker.pawn.Faction.IsPlayerSafe())
				{
					item.EverSeenByPlayer = true;
				}
			}
			this.NotifyColonistBarIfColonistCorpse(item);
			Action onContentsChanged = this.OnContentsChanged;
			if (onContentsChanged == null)
			{
				return;
			}
			onContentsChanged();
		}

		// Token: 0x06003A27 RID: 14887 RVA: 0x00135108 File Offset: 0x00133308
		protected void NotifyAddedAndMergedWith(Thing item, int mergedCount)
		{
			CompTransporter compTransporter = this.owner as CompTransporter;
			if (compTransporter != null)
			{
				compTransporter.Notify_ThingAddedAndMergedWith(item, mergedCount);
			}
		}

		// Token: 0x06003A28 RID: 14888 RVA: 0x0013512C File Offset: 0x0013332C
		protected virtual void NotifyRemoved(Thing item)
		{
			Pawn_InventoryTracker pawn_InventoryTracker = this.owner as Pawn_InventoryTracker;
			if (pawn_InventoryTracker != null)
			{
				pawn_InventoryTracker.Notify_ItemRemoved(item);
			}
			Pawn_ApparelTracker pawn_ApparelTracker = this.owner as Pawn_ApparelTracker;
			if (pawn_ApparelTracker != null)
			{
				pawn_ApparelTracker.Notify_ApparelRemoved((Apparel)item);
			}
			Pawn_EquipmentTracker pawn_EquipmentTracker = this.owner as Pawn_EquipmentTracker;
			if (pawn_EquipmentTracker != null)
			{
				pawn_EquipmentTracker.Notify_EquipmentRemoved((ThingWithComps)item);
			}
			Caravan caravan = this.owner as Caravan;
			if (caravan != null)
			{
				caravan.Notify_PawnRemoved((Pawn)item);
			}
			this.NotifyColonistBarIfColonistCorpse(item);
			Action onContentsChanged = this.OnContentsChanged;
			if (onContentsChanged == null)
			{
				return;
			}
			onContentsChanged();
		}

		// Token: 0x06003A29 RID: 14889 RVA: 0x001351B8 File Offset: 0x001333B8
		private void NotifyColonistBarIfColonistCorpse(Thing thing)
		{
			Corpse corpse = thing as Corpse;
			if (corpse != null && !corpse.Bugged && corpse.InnerPawn.Faction != null && corpse.InnerPawn.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x06003A2A RID: 14890 RVA: 0x0013438E File Offset: 0x0013258E
		void IList<Thing>.Insert(int index, Thing item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x06003A2B RID: 14891 RVA: 0x00135208 File Offset: 0x00133408
		void ICollection<Thing>.Add(Thing item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x06003A2C RID: 14892 RVA: 0x00135214 File Offset: 0x00133414
		void ICollection<Thing>.CopyTo(Thing[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this.GetAt(i);
			}
		}

		// Token: 0x06003A2D RID: 14893 RVA: 0x0013523E File Offset: 0x0013343E
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.Count; i = num + 1)
			{
				yield return this.GetAt(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x06003A2E RID: 14894 RVA: 0x0013524D File Offset: 0x0013344D
		IEnumerator IEnumerable.GetEnumerator()
		{
			int num;
			for (int i = 0; i < this.Count; i = num + 1)
			{
				yield return this.GetAt(i);
				num = i;
			}
			yield break;
		}

		// Token: 0x04002A78 RID: 10872
		protected IThingHolder owner;

		// Token: 0x04002A79 RID: 10873
		protected int maxStacks = 999999;

		// Token: 0x04002A7A RID: 10874
		public LookMode contentsLookMode = LookMode.Deep;

		// Token: 0x04002A7B RID: 10875
		public bool removeContentsIfDestroyed = true;

		// Token: 0x04002A7C RID: 10876
		public bool dontTickContents;

		// Token: 0x04002A7D RID: 10877
		private const int InfMaxStacks = 999999;
	}
}
