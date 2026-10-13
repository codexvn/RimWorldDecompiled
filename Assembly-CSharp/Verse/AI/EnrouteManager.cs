using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000E72 RID: 3698
	public class EnrouteManager : IExposable
	{
		// Token: 0x06005D27 RID: 23847 RVA: 0x001DACDC File Offset: 0x001D8EDC
		public EnrouteManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x06005D28 RID: 23848 RVA: 0x001DAD01 File Offset: 0x001D8F01
		public void AddEnroute(IHaulEnroute container, Pawn pawn, ThingDef stuff, int count)
		{
			this.GetOrAddTracker(container).Add(pawn, stuff, count);
			pawn.MapHeld.events.Notify_HaulEnrouteAdded((Thing)container, pawn, stuff, count);
		}

		// Token: 0x06005D29 RID: 23849 RVA: 0x001DAD30 File Offset: 0x001D8F30
		public int GetEnroute(IHaulEnroute thing, ThingDef stuff, Pawn excludeEnrouteFor = null)
		{
			ThingCountTracker thingCountTracker;
			if (this.lookup.TryGetValue(thing, out thingCountTracker))
			{
				return thingCountTracker.Get(stuff, excludeEnrouteFor);
			}
			return 0;
		}

		// Token: 0x06005D2A RID: 23850 RVA: 0x001DAD58 File Offset: 0x001D8F58
		public bool AnyEnrouteTo(Thing thing)
		{
			IHaulEnroute haulEnroute = thing as IHaulEnroute;
			return haulEnroute != null && this.lookup.ContainsKey(haulEnroute);
		}

		// Token: 0x06005D2B RID: 23851 RVA: 0x001DAD80 File Offset: 0x001D8F80
		public void Notify_ContainerDespawned(IHaulEnroute thing)
		{
			ThingCountTracker item;
			if (!this.lookup.TryGetValue(thing, out item))
			{
				return;
			}
			this.enroute.Remove(item);
			this.lookup.Remove(thing);
		}

		// Token: 0x06005D2C RID: 23852 RVA: 0x001DADB8 File Offset: 0x001D8FB8
		private ThingCountTracker GetOrAddTracker(IHaulEnroute container)
		{
			ThingCountTracker thingCountTracker;
			if (this.lookup.TryGetValue(container, out thingCountTracker))
			{
				return thingCountTracker;
			}
			thingCountTracker = new ThingCountTracker(container);
			this.enroute.Add(thingCountTracker);
			this.lookup[container] = thingCountTracker;
			return thingCountTracker;
		}

		// Token: 0x06005D2D RID: 23853 RVA: 0x001DADF8 File Offset: 0x001D8FF8
		public void SendReservations(IHaulEnroute from, IHaulEnroute to)
		{
			ThingCountTracker other;
			if (!this.lookup.TryGetValue(from, out other))
			{
				return;
			}
			this.GetOrAddTracker(to).CopyReservations(other);
		}

		// Token: 0x06005D2E RID: 23854 RVA: 0x001DAE24 File Offset: 0x001D9024
		public void ReleaseFor(IHaulEnroute container, Pawn pawn)
		{
			ThingCountTracker thingCountTracker;
			if (!this.lookup.TryGetValue(container, out thingCountTracker))
			{
				return;
			}
			thingCountTracker.ReleaseFor(pawn);
			if (thingCountTracker.CanCleanup())
			{
				this.lookup.Remove(container);
				this.enroute.Remove(thingCountTracker);
			}
			this.map.events.Notify_HaulEnrouteReleased((Thing)container, pawn);
		}

		// Token: 0x06005D2F RID: 23855 RVA: 0x001DAE84 File Offset: 0x001D9084
		public void ReleaseAllClaimedBy(Pawn pawn)
		{
			for (int i = this.enroute.Count - 1; i >= 0; i--)
			{
				ThingCountTracker thingCountTracker = this.enroute[i];
				thingCountTracker.ReleaseFor(pawn);
				if (thingCountTracker.CanCleanup())
				{
					this.lookup.Remove(thingCountTracker.parent);
					this.enroute.RemoveAt(i);
				}
				this.map.events.Notify_HaulEnrouteReleased(thingCountTracker.ParentThing, pawn);
			}
		}

		// Token: 0x06005D30 RID: 23856 RVA: 0x001DAEFC File Offset: 0x001D90FC
		public void InterruptEnroutePawns(IHaulEnroute container, Pawn exclude)
		{
			ThingCountTracker thingCountTracker;
			if (!this.lookup.TryGetValue(container, out thingCountTracker))
			{
				return;
			}
			thingCountTracker.InterruptEnroutePawns(exclude);
			if (thingCountTracker.CanCleanup())
			{
				this.lookup.Remove(thingCountTracker.parent);
				this.enroute.Remove(thingCountTracker);
			}
		}

		// Token: 0x06005D31 RID: 23857 RVA: 0x001DAF48 File Offset: 0x001D9148
		public void ExposeData()
		{
			Scribe_Collections.Look<ThingCountTracker>(ref this.enroute, "enroute", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int i = this.enroute.Count - 1; i >= 0; i--)
				{
					if (this.enroute[i].parent == null)
					{
						this.enroute.RemoveAt(i);
					}
				}
				foreach (ThingCountTracker thingCountTracker in this.enroute)
				{
					this.lookup.Add(thingCountTracker.parent, thingCountTracker);
				}
			}
		}

		// Token: 0x06005D32 RID: 23858 RVA: 0x001DAFFC File Offset: 0x001D91FC
		public void LogEnroute()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Enroute =======");
			stringBuilder.AppendLine("Count: " + this.enroute.Count.ToString());
			for (int i = 0; i < this.enroute.Count; i++)
			{
				ThingCountTracker thingCountTracker = this.enroute[i];
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = stringBuilder;
				string format = "[{0}]: {1}";
				object arg = i;
				Thing thing = thingCountTracker.parent as Thing;
				stringBuilder2.AppendLine(string.Format(format, arg, (thing != null) ? thing.LabelShort : "NON-THING"));
				if (thingCountTracker.ReadOnlyPairs.Count == 0)
				{
					stringBuilder.AppendLine("   NO ELEMENTS - SHOULD HAVE BEEN RELEASED");
				}
				else
				{
					foreach (KeyValuePair<ThingDef, List<ThingCountTracker.PawnCount>> keyValuePair in thingCountTracker.ReadOnlyPairs)
					{
						ThingDef thingDef;
						List<ThingCountTracker.PawnCount> list;
						keyValuePair.Deconstruct(out thingDef, out list);
						ThingDef thingDef2 = thingDef;
						List<ThingCountTracker.PawnCount> list2 = list;
						stringBuilder.AppendLine(string.Format("   {0}, {1} enroute:", thingDef2.label, thingCountTracker.Get(thingDef2, null)));
						for (int j = 0; j < list2.Count; j++)
						{
							ThingCountTracker.PawnCount pawnCount = list2[j];
							stringBuilder.AppendLine(string.Format("      [{0}]: {1,-12} x {2}", j, pawnCount.pawn.LabelShort, pawnCount.count));
						}
					}
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04004340 RID: 17216
		private List<ThingCountTracker> enroute = new List<ThingCountTracker>();

		// Token: 0x04004341 RID: 17217
		private Map map;

		// Token: 0x04004342 RID: 17218
		private readonly Dictionary<IHaulEnroute, ThingCountTracker> lookup = new Dictionary<IHaulEnroute, ThingCountTracker>();
	}
}
