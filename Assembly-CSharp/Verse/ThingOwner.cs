using System;
using System.Collections;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008D6 RID: 2262
	public class ThingOwner<T> : ThingOwner, IList<T>, ICollection<T>, IEnumerable<!0>, IEnumerable where T : Thing
	{
		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x00133B6C File Offset: 0x00131D6C
		public List<T> InnerListForReading
		{
			get
			{
				return this.innerList;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		public T this[int index]
		{
			get
			{
				return this.innerList[index];
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x00133B82 File Offset: 0x00131D82
		public override int Count
		{
			get
			{
				return this.innerList.Count;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		T IList<!0>.this[int index]
		{
			get
			{
				return this.innerList[index];
			}
			set
			{
				throw new InvalidOperationException("ThingOwner doesn't allow setting individual elements.");
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060039C3 RID: 14787 RVA: 0x000028E7 File Offset: 0x00000AE7
		bool ICollection<!0>.IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x00133B9B File Offset: 0x00131D9B
		public ThingOwner()
		{
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x00133BAE File Offset: 0x00131DAE
		public ThingOwner(IThingHolder owner) : base(owner)
		{
		}

		// Token: 0x060039C6 RID: 14790 RVA: 0x00133BAE File Offset: 0x00131DAE
		public ThingOwner(IThingHolder owner, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true) : base(owner)
		{
		}

		// Token: 0x060039C7 RID: 14791 RVA: 0x00133BC2 File Offset: 0x00131DC2
		public ThingOwner(IThingHolder owner, bool oneStackOnly, LookMode contentsLookMode = LookMode.Deep, bool removeContentsIfDestroyed = true) : base(owner, oneStackOnly, contentsLookMode, removeContentsIfDestroyed)
		{
		}

		// Token: 0x060039C8 RID: 14792 RVA: 0x00133BDC File Offset: 0x00131DDC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<T>(ref this.innerList, "innerList", true, this.contentsLookMode, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				int num = this.innerList.RemoveAll(delegate(T x)
				{
					if (x != null)
					{
						MinifiedThing minifiedThing = x as MinifiedThing;
						return minifiedThing != null && minifiedThing.InnerThing == null;
					}
					return true;
				});
				if (num > 0)
				{
					Log.Warning(string.Format("ThingOwner removed {0} invalid entries during PostLoadInit.", num));
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					if (this.innerList[i] != null)
					{
						this.innerList[i].holdingOwner = this;
					}
				}
			}
		}

		// Token: 0x060039C9 RID: 14793 RVA: 0x00133CA7 File Offset: 0x00131EA7
		public List<T>.Enumerator GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060039CA RID: 14794 RVA: 0x00133CB4 File Offset: 0x00131EB4
		public override int GetCountCanAccept(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (!(item is T))
			{
				return 0;
			}
			return base.GetCountCanAccept(item, canMergeWithExistingStacks);
		}

		// Token: 0x060039CB RID: 14795 RVA: 0x00133CC8 File Offset: 0x00131EC8
		public override int TryAdd(Thing item, int count, bool canMergeWithExistingStacks = true)
		{
			if (count <= 0)
			{
				return 0;
			}
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return 0;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + ((item != null) ? item.ToString() : null) + " to ThingOwner but this item is already here.");
				return 0;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					count.ToString(),
					" of ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}));
				return 0;
			}
			if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return 0;
			}
			int stackCount = item.stackCount;
			int num = Mathf.Min(stackCount, count);
			Thing thing = item.SplitOff(num);
			if (this.TryAdd((T)((object)thing), canMergeWithExistingStacks))
			{
				CompPushable compPushable = item.TryGetComp<CompPushable>();
				if (compPushable != null)
				{
					Pawn pawn = this.owner as Pawn;
					if (pawn != null)
					{
						compPushable.OnStartedCarrying(pawn);
					}
				}
				return num;
			}
			if (thing != item)
			{
				int result = stackCount - item.stackCount - thing.stackCount;
				item.TryAbsorbStack(thing, false);
				return result;
			}
			return stackCount - item.stackCount;
		}

		// Token: 0x060039CC RID: 14796 RVA: 0x00133E10 File Offset: 0x00132010
		public override bool TryAdd(Thing item, bool canMergeWithExistingStacks = true)
		{
			if (item == null)
			{
				Log.Warning("Tried to add null item to ThingOwner.");
				return false;
			}
			T t = item as T;
			if (t == null)
			{
				return false;
			}
			if (base.Contains(item))
			{
				Log.Warning("Tried to add " + item.ToStringSafe<Thing>() + " to ThingOwner but this item is already here.");
				return false;
			}
			if (item.holdingOwner != null)
			{
				Log.Warning(string.Concat(new string[]
				{
					"Tried to add ",
					item.ToStringSafe<Thing>(),
					" to ThingOwner but this thing is already in another container. owner=",
					this.owner.ToStringSafe<IThingHolder>(),
					", current container owner=",
					item.holdingOwner.Owner.ToStringSafe<IThingHolder>(),
					". Use TryAddOrTransfer, TryTransferToContainer, or remove the item before adding it."
				}));
				return false;
			}
			if (!base.CanAcceptAnyOf(item, canMergeWithExistingStacks))
			{
				return false;
			}
			if (canMergeWithExistingStacks)
			{
				for (int i = 0; i < this.innerList.Count; i++)
				{
					T t2 = this.innerList[i];
					if (t2.CanStackWith(item))
					{
						int num = Mathf.Min(item.stackCount, t2.def.stackLimit - t2.stackCount);
						if (num > 0)
						{
							Thing other = item.SplitOff(num);
							int stackCount = t2.stackCount;
							t2.TryAbsorbStack(other, true);
							if (t2.stackCount > stackCount)
							{
								base.NotifyAddedAndMergedWith(t2, t2.stackCount - stackCount);
							}
							if (item.Destroyed || item.stackCount == 0)
							{
								return true;
							}
						}
					}
				}
			}
			if (this.Count >= this.maxStacks)
			{
				return false;
			}
			item.holdingOwner = this;
			this.innerList.Add(t);
			this.NotifyAdded(t);
			return true;
		}

		// Token: 0x060039CD RID: 14797 RVA: 0x00133FD4 File Offset: 0x001321D4
		protected override void NotifyAdded(Thing item)
		{
			IThingHolderEvents<T> thingHolderEvents = this.owner as IThingHolderEvents<T>;
			if (thingHolderEvents != null)
			{
				thingHolderEvents.Notify_ItemAdded(item as T);
			}
			base.NotifyAdded(item);
		}

		// Token: 0x060039CE RID: 14798 RVA: 0x00134008 File Offset: 0x00132208
		protected override void NotifyRemoved(Thing item)
		{
			IThingHolderEvents<T> thingHolderEvents = this.owner as IThingHolderEvents<T>;
			if (thingHolderEvents != null)
			{
				thingHolderEvents.Notify_ItemRemoved(item as T);
			}
			base.NotifyRemoved(item);
		}

		// Token: 0x060039CF RID: 14799 RVA: 0x0013403C File Offset: 0x0013223C
		public void TryAddRangeOrTransfer(IEnumerable<T> things, bool canMergeWithExistingStacks = true, bool destroyLeftover = false)
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
				IList<T> list = things as IList<!0>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (!base.TryAddOrTransfer(list[i], canMergeWithExistingStacks) && destroyLeftover)
						{
							list[i].Destroy(DestroyMode.Vanish);
						}
					}
					return;
				}
				foreach (T t in things)
				{
					if (!base.TryAddOrTransfer(t, canMergeWithExistingStacks) && destroyLeftover)
					{
						t.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x060039D0 RID: 14800 RVA: 0x00134110 File Offset: 0x00132310
		public override int IndexOf(Thing item)
		{
			T t = item as T;
			if (t == null)
			{
				return -1;
			}
			return this.innerList.IndexOf(t);
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x00134140 File Offset: 0x00132340
		public override bool Remove(Thing item)
		{
			if (!base.Contains(item))
			{
				return false;
			}
			if (item.holdingOwner == this)
			{
				item.holdingOwner = null;
			}
			int index = this.innerList.LastIndexOf((T)((object)item));
			this.innerList.RemoveAt(index);
			this.NotifyRemoved(item);
			return true;
		}

		// Token: 0x060039D2 RID: 14802 RVA: 0x00134190 File Offset: 0x00132390
		public int RemoveAll(Predicate<T> predicate)
		{
			int num = 0;
			for (int i = this.innerList.Count - 1; i >= 0; i--)
			{
				if (predicate(this.innerList[i]))
				{
					this.Remove(this.innerList[i]);
					num++;
				}
			}
			return num;
		}

		// Token: 0x060039D3 RID: 14803 RVA: 0x001341E8 File Offset: 0x001323E8
		protected override Thing GetAt(int index)
		{
			return this.innerList[index];
		}

		// Token: 0x060039D4 RID: 14804 RVA: 0x001341FC File Offset: 0x001323FC
		public void GetThingsOfType<J>(List<J> list) where J : Thing
		{
			for (int i = 0; i < this.innerList.Count; i++)
			{
				J j = this.innerList[i] as J;
				if (j != null)
				{
					list.Add(j);
				}
			}
		}

		// Token: 0x060039D5 RID: 14805 RVA: 0x0013424C File Offset: 0x0013244C
		public int TryTransferToContainer(Thing item, ThingOwner otherContainer, int stackCount, out T resultingTransferredItem, bool canMergeWithExistingStacks = true)
		{
			Thing thing;
			int result = base.TryTransferToContainer(item, otherContainer, stackCount, out thing, canMergeWithExistingStacks);
			resultingTransferredItem = (T)((object)thing);
			return result;
		}

		// Token: 0x060039D6 RID: 14806 RVA: 0x00134273 File Offset: 0x00132473
		public new T Take(Thing thing, int count)
		{
			return (T)((object)base.Take(thing, count));
		}

		// Token: 0x060039D7 RID: 14807 RVA: 0x00134282 File Offset: 0x00132482
		public new T Take(Thing thing)
		{
			return (T)((object)base.Take(thing));
		}

		// Token: 0x060039D8 RID: 14808 RVA: 0x00134290 File Offset: 0x00132490
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, int count, out T resultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, count, out thing2, placedAction2, nearPlaceValidator);
			resultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x060039D9 RID: 14809 RVA: 0x001342E4 File Offset: 0x001324E4
		public bool TryDrop(Thing thing, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, mode, out thing2, placedAction2, nearPlaceValidator);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x060039DA RID: 14810 RVA: 0x00134330 File Offset: 0x00132530
		public bool TryDrop(Thing thing, IntVec3 dropLoc, Map map, ThingPlaceMode mode, out T lastResultingThing, Action<T, int> placedAction = null, Predicate<IntVec3> nearPlaceValidator = null)
		{
			Action<Thing, int> placedAction2 = null;
			if (placedAction != null)
			{
				placedAction2 = delegate(Thing t, int c)
				{
					placedAction((T)((object)t), c);
				};
			}
			Thing thing2;
			bool result = base.TryDrop(thing, dropLoc, map, mode, out thing2, placedAction2, nearPlaceValidator, true);
			lastResultingThing = (T)((object)thing2);
			return result;
		}

		// Token: 0x060039DB RID: 14811 RVA: 0x00134380 File Offset: 0x00132580
		int IList<!0>.IndexOf(T item)
		{
			return this.innerList.IndexOf(item);
		}

		// Token: 0x060039DC RID: 14812 RVA: 0x0013438E File Offset: 0x0013258E
		void IList<!0>.Insert(int index, T item)
		{
			throw new InvalidOperationException("ThingOwner doesn't allow inserting individual elements at any position.");
		}

		// Token: 0x060039DD RID: 14813 RVA: 0x0013439A File Offset: 0x0013259A
		void ICollection<!0>.Add(T item)
		{
			this.TryAdd(item, true);
		}

		// Token: 0x060039DE RID: 14814 RVA: 0x001343AA File Offset: 0x001325AA
		void ICollection<!0>.CopyTo(T[] array, int arrayIndex)
		{
			this.innerList.CopyTo(array, arrayIndex);
		}

		// Token: 0x060039DF RID: 14815 RVA: 0x001343B9 File Offset: 0x001325B9
		bool ICollection<!0>.Contains(T item)
		{
			return this.innerList.Contains(item);
		}

		// Token: 0x060039E0 RID: 14816 RVA: 0x001343C7 File Offset: 0x001325C7
		bool ICollection<!0>.Remove(T item)
		{
			return this.Remove(item);
		}

		// Token: 0x060039E1 RID: 14817 RVA: 0x001343D5 File Offset: 0x001325D5
		IEnumerator<T> IEnumerable<!0>.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x060039E2 RID: 14818 RVA: 0x001343D5 File Offset: 0x001325D5
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.innerList.GetEnumerator();
		}

		// Token: 0x04002A72 RID: 10866
		private List<T> innerList = new List<T>();
	}
}
