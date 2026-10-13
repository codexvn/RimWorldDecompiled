using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200034C RID: 844
	public class TickList
	{
		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001595 RID: 5525 RVA: 0x00065CD0 File Offset: 0x00063ED0
		private int TickInterval
		{
			get
			{
				switch (this.tickType)
				{
				case TickerType.Normal:
					return 1;
				case TickerType.Rare:
					return 250;
				case TickerType.Long:
					return 2000;
				default:
					return -1;
				}
			}
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00065D0C File Offset: 0x00063F0C
		public TickList(TickerType tickType)
		{
			this.tickType = tickType;
			for (int i = 0; i < this.TickInterval; i++)
			{
				this.thingLists.Add(new List<Thing>());
			}
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00065D68 File Offset: 0x00063F68
		public void Reset()
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].Clear();
			}
			this.thingsToRegister.Clear();
			this.thingsToDeregister.Clear();
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00065DB4 File Offset: 0x00063FB4
		public void RemoveWhere(Predicate<Thing> predicate)
		{
			for (int i = 0; i < this.thingLists.Count; i++)
			{
				this.thingLists[i].RemoveAll(predicate);
			}
			this.thingsToRegister.RemoveAll(predicate);
			this.thingsToDeregister.RemoveAll(predicate);
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x00065E04 File Offset: 0x00064004
		public void RegisterThing(Thing t)
		{
			this.thingsToRegister.Add(t);
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x00065E12 File Offset: 0x00064012
		public void DeregisterThing(Thing t)
		{
			this.thingsToDeregister.Add(t);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x00065E20 File Offset: 0x00064020
		public void Tick()
		{
			for (int i = 0; i < this.thingsToRegister.Count; i++)
			{
				this.BucketOf(this.thingsToRegister[i]).Add(this.thingsToRegister[i]);
			}
			this.thingsToRegister.Clear();
			for (int j = 0; j < this.thingsToDeregister.Count; j++)
			{
				this.BucketOf(this.thingsToDeregister[j]).Remove(this.thingsToDeregister[j]);
			}
			this.thingsToDeregister.Clear();
			if (DebugSettings.fastEcology)
			{
				Find.World.tileTemperatures.ClearCaches();
				for (int k = 0; k < this.thingLists.Count; k++)
				{
					List<Thing> list = this.thingLists[k];
					for (int l = 0; l < list.Count; l++)
					{
						if (list[l].def.category == ThingCategory.Plant)
						{
							list[l].TickLong();
						}
					}
				}
			}
			List<Thing> list2 = this.thingLists[Find.TickManager.TicksGame % this.TickInterval];
			for (int m = 0; m < list2.Count; m++)
			{
				Thing thing = list2[m];
				if (!thing.Destroyed)
				{
					try
					{
						thing.DoTick();
					}
					catch (Exception arg)
					{
						string arg2 = thing.Spawned ? string.Format(" (at {0})", thing.Position) : "";
						if (Prefs.DevMode)
						{
							Log.Error(string.Format("Exception ticking {0}{1}: {2}", thing.ToStringSafe<Thing>(), arg2, arg));
						}
						else
						{
							Log.ErrorOnce(string.Format("Exception ticking {0}{1}. Suppressing further errors. Exception: {2}", thing.ToStringSafe<Thing>(), arg2, arg), thing.thingIDNumber ^ 576876901);
						}
					}
				}
			}
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x00066008 File Offset: 0x00064208
		private List<Thing> BucketOf(Thing t)
		{
			int num = t.GetHashCode();
			if (num < 0)
			{
				num *= -1;
			}
			int index = num % this.TickInterval;
			return this.thingLists[index];
		}

		// Token: 0x04001402 RID: 5122
		private TickerType tickType;

		// Token: 0x04001403 RID: 5123
		private List<List<Thing>> thingLists = new List<List<Thing>>();

		// Token: 0x04001404 RID: 5124
		private List<Thing> thingsToRegister = new List<Thing>();

		// Token: 0x04001405 RID: 5125
		private List<Thing> thingsToDeregister = new List<Thing>();
	}
}
