using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x0200044B RID: 1099
	public sealed class ListerThings
	{
		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x0008E7B5 File Offset: 0x0008C9B5
		public List<Thing> AllThings
		{
			get
			{
				return this.listsByGroup[2];
			}
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x0008E7C0 File Offset: 0x0008C9C0
		public ListerThings(ListerThingsUse use, ThingListChangedCallbacks thingListChangedCallbacks = null)
		{
			this.use = use;
			this.thingListChangedCallbacks = thingListChangedCallbacks;
			this.listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
			this.stateHashByGroup = new int[ThingListGroupHelper.AllGroups.Length];
			this.haulSources = new List<IHaulSource>();
			this.listsByGroup[2] = new List<Thing>();
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x0008E82D File Offset: 0x0008CA2D
		public List<Thing> ThingsInGroup(ThingRequestGroup group)
		{
			return this.ThingsMatching(ThingRequest.ForGroup(group));
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x0008E83C File Offset: 0x0008CA3C
		public int StateHashOfGroup(ThingRequestGroup group)
		{
			if (this.use == ListerThingsUse.Region && !group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get state hash of group " + group.ToString() + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968738832);
				return -1;
			}
			return Gen.HashCombineInt(85693994, this.stateHashByGroup[(int)group]);
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x0008E894 File Offset: 0x0008CA94
		public List<Thing> ThingsOfDef(ThingDef def)
		{
			if (def == ThingDefOf.MinifiedThing)
			{
				Log.ErrorOnce("Tried to get ThingsOfDef of MinifiedThing, use ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)) instead", 1927834012);
			}
			return this.ThingsMatching(ThingRequest.ForDef(def));
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0008E8B9 File Offset: 0x0008CAB9
		public bool AnyThingWithDef(ThingDef def)
		{
			return this.listsByDef.ContainsKey(def) && this.listsByDef[def].Count > 0;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x0008E8E0 File Offset: 0x0008CAE0
		public List<Thing> ThingsMatching(ThingRequest req)
		{
			if (req.singleDef != null)
			{
				return this.listsByDef.GetValueOrDefault(req.singleDef, ListerThings.EmptyList);
			}
			if (req.group == ThingRequestGroup.Undefined)
			{
				string str = "Invalid ThingRequest ";
				ThingRequest thingRequest = req;
				throw new InvalidOperationException(str + thingRequest.ToString());
			}
			if (this.use == ListerThingsUse.Region && !req.group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get things in group " + req.group.ToString() + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132);
				return ListerThings.EmptyList;
			}
			return this.listsByGroup[(int)req.group] ?? ListerThings.EmptyList;
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x0008E990 File Offset: 0x0008CB90
		public List<Thing> ThingsMatchingFilter(ThingFilter filter)
		{
			ListerThings.tmpThingsMatchingFilter.Clear();
			foreach (ThingDef def in filter.AllowedThingDefs)
			{
				ListerThings.tmpThingsMatchingFilter.AddRange(this.ThingsOfDef(def));
			}
			return ListerThings.tmpThingsMatchingFilter;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0008E9F8 File Offset: 0x0008CBF8
		public void GetThingsOfType<T>(List<T> list) where T : Thing
		{
			if (typeof(T) == typeof(Thing))
			{
				Log.Error("Do not call this method with type 'Thing' directly, as it will return all things currently registered.");
				return;
			}
			List<Thing> allThings = this.AllThings;
			for (int i = 0; i < this.AllThings.Count; i++)
			{
				T t = allThings[i] as T;
				if (t != null)
				{
					list.Add(t);
				}
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x0008EA69 File Offset: 0x0008CC69
		public IEnumerable<T> GetThingsOfType<T>() where T : Thing
		{
			if (typeof(T) == typeof(Thing))
			{
				Log.Error("Do not call this method with type 'Thing' directly, as it will return all things currently registered.");
				yield break;
			}
			List<Thing> things = this.AllThings;
			int num;
			for (int i = 0; i < this.AllThings.Count; i = num + 1)
			{
				T t = things[i] as T;
				if (t != null)
				{
					yield return t;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06001C73 RID: 7283 RVA: 0x0008EA79 File Offset: 0x0008CC79
		public IEnumerable<Thing> GetAllThings(Predicate<Thing> validator = null, bool lookInHaulSources = false)
		{
			foreach (Thing thing in this.AllThings)
			{
				if (validator == null || validator(thing))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			if (!lookInHaulSources)
			{
				yield break;
			}
			foreach (IHaulSource haulSource in this.haulSources)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					if (validator == null || validator(thing2))
					{
						yield return thing2;
					}
				}
				IEnumerator<Thing> enumerator3 = null;
			}
			List<IHaulSource>.Enumerator enumerator2 = default(List<IHaulSource>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06001C74 RID: 7284 RVA: 0x0008EA98 File Offset: 0x0008CC98
		public void GetAllThings(in List<Thing> list, Predicate<Thing> validator = null, bool lookInHaulSources = false)
		{
			if (validator != null)
			{
				using (List<Thing>.Enumerator enumerator = this.AllThings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing = enumerator.Current;
						if (validator(thing))
						{
							list.Add(thing);
						}
					}
					goto IL_50;
				}
			}
			list.AddRange(this.AllThings);
			IL_50:
			if (!lookInHaulSources)
			{
				return;
			}
			foreach (IHaulSource haulSource in this.haulSources)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					if (validator == null || validator(thing2))
					{
						list.Add(thing2);
					}
				}
			}
		}

		// Token: 0x06001C75 RID: 7285 RVA: 0x0008EB90 File Offset: 0x0008CD90
		public void GetAllThings(in List<Thing> list, ThingRequestGroup group, Predicate<Thing> validator = null, bool lookInHaulSources = false)
		{
			if (validator != null)
			{
				using (List<Thing>.Enumerator enumerator = this.ThingsInGroup(group).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Thing thing = enumerator.Current;
						if (validator(thing))
						{
							list.Add(thing);
						}
					}
					goto IL_52;
				}
			}
			list.AddRange(this.ThingsInGroup(group));
			IL_52:
			if (!lookInHaulSources)
			{
				return;
			}
			foreach (IHaulSource haulSource in this.haulSources)
			{
				foreach (Thing thing2 in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					if (this.GroupIncludes(thing2, group) && (validator == null || validator(thing2)))
					{
						list.Add(thing2);
					}
				}
			}
		}

		// Token: 0x06001C76 RID: 7286 RVA: 0x0008EC94 File Offset: 0x0008CE94
		public bool Contains(Thing t)
		{
			return this.AllThings.Contains(t);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0008ECA4 File Offset: 0x0008CEA4
		public void Add(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (!this.listsByDef.TryGetValue(t.def, out list))
			{
				list = new List<Thing>();
				this.listsByDef.Add(t.def, list);
			}
			list.Add(t);
			IHaulSource haulSource = t as IHaulSource;
			if (haulSource != null)
			{
				this.haulSources.Add(haulSource);
			}
			foreach (ThingRequestGroup thingRequestGroup in ThingListGroupHelper.AllGroups)
			{
				if (this.GroupIncludes(t, thingRequestGroup))
				{
					List<Thing> list2 = this.listsByGroup[(int)thingRequestGroup];
					if (list2 == null)
					{
						list2 = new List<Thing>();
						this.listsByGroup[(int)thingRequestGroup] = list2;
						this.stateHashByGroup[(int)thingRequestGroup] = 0;
					}
					list2.Add(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
			ThingListChangedCallbacks thingListChangedCallbacks = this.thingListChangedCallbacks;
			if (thingListChangedCallbacks == null)
			{
				return;
			}
			Action<Thing> onThingAdded = thingListChangedCallbacks.onThingAdded;
			if (onThingAdded == null)
			{
				return;
			}
			onThingAdded(t);
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0008ED93 File Offset: 0x0008CF93
		private bool GroupIncludes(Thing thing, ThingRequestGroup group)
		{
			return (this.use != ListerThingsUse.Region || group.StoreInRegion()) && group.Includes(thing.def);
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0008EDB4 File Offset: 0x0008CFB4
		public void Remove(Thing t)
		{
			if (!ListerThings.EverListable(t.def, this.use))
			{
				return;
			}
			List<Thing> list;
			if (this.listsByDef.TryGetValue(t.def, out list))
			{
				list.Remove(t);
			}
			IHaulSource haulSource = t as IHaulSource;
			if (haulSource != null)
			{
				this.haulSources.Remove(haulSource);
			}
			ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
			for (int i = 0; i < allGroups.Length; i++)
			{
				ThingRequestGroup thingRequestGroup = allGroups[i];
				if ((this.use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
				{
					this.listsByGroup[i].Remove(t);
					this.stateHashByGroup[(int)thingRequestGroup]++;
				}
			}
			ThingListChangedCallbacks thingListChangedCallbacks = this.thingListChangedCallbacks;
			if (thingListChangedCallbacks == null)
			{
				return;
			}
			Action<Thing> onThingRemoved = thingListChangedCallbacks.onThingRemoved;
			if (onThingRemoved == null)
			{
				return;
			}
			onThingRemoved(t);
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x0008EE7F File Offset: 0x0008D07F
		public static bool EverListable(ThingDef def, ListerThingsUse use)
		{
			return (def.category != ThingCategory.Mote || (def.drawGUIOverlay && use != ListerThingsUse.Region)) && (def.category != ThingCategory.Projectile || use != ListerThingsUse.Region);
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x0008EEAC File Offset: 0x0008D0AC
		public void Clear()
		{
			this.listsByDef.Clear();
			for (int i = 0; i < this.listsByGroup.Length; i++)
			{
				if (this.listsByGroup[i] != null)
				{
					this.listsByGroup[i].Clear();
				}
				this.stateHashByGroup[i] = 0;
			}
		}

		// Token: 0x04001857 RID: 6231
		private readonly Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

		// Token: 0x04001858 RID: 6232
		private readonly List<Thing>[] listsByGroup;

		// Token: 0x04001859 RID: 6233
		private readonly List<IHaulSource> haulSources;

		// Token: 0x0400185A RID: 6234
		private readonly int[] stateHashByGroup;

		// Token: 0x0400185B RID: 6235
		public ListerThingsUse use;

		// Token: 0x0400185C RID: 6236
		public ThingListChangedCallbacks thingListChangedCallbacks;

		// Token: 0x0400185D RID: 6237
		private static readonly List<Thing> EmptyList = new List<Thing>();

		// Token: 0x0400185E RID: 6238
		private static readonly List<Thing> tmpThingsMatchingFilter = new List<Thing>(1024);
	}
}
