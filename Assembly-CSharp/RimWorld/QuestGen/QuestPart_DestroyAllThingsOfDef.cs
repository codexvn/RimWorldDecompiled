using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BA7 RID: 15271
	public class QuestPart_DestroyAllThingsOfDef : QuestPart
	{
		// Token: 0x06014BCE RID: 84942 RVA: 0x00636375 File Offset: 0x00634575
		public QuestPart_DestroyAllThingsOfDef()
		{
		}

		// Token: 0x06014BCF RID: 84943 RVA: 0x00636388 File Offset: 0x00634588
		public QuestPart_DestroyAllThingsOfDef(string inSignal, MapParent mapParent, List<ThingDef> defsToDestroy)
		{
			this.inSignal = inSignal;
			this.mapParent = mapParent;
			this.defsToDestroy = defsToDestroy;
		}

		// Token: 0x06014BD0 RID: 84944 RVA: 0x006363B0 File Offset: 0x006345B0
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				this.DestroyThings();
			}
		}

		// Token: 0x06014BD1 RID: 84945 RVA: 0x006363CC File Offset: 0x006345CC
		private void DestroyThingsOfDef(ThingDef def)
		{
			QuestPart_DestroyAllThingsOfDef.tmpToDestroy.AddRange(this.mapParent.Map.listerThings.ThingsOfDef(def));
			for (int i = 0; i < QuestPart_DestroyAllThingsOfDef.tmpToDestroy.Count; i++)
			{
				if (!QuestPart_DestroyAllThingsOfDef.tmpToDestroy[i].Destroyed)
				{
					QuestPart_DestroyAllThingsOfDef.tmpToDestroy[i].Destroy(DestroyMode.QuestLogic);
				}
			}
			QuestPart_DestroyAllThingsOfDef.tmpToDestroy.Clear();
		}

		// Token: 0x06014BD2 RID: 84946 RVA: 0x0063643C File Offset: 0x0063463C
		private void DestroyThings()
		{
			MapParent mapParent = this.mapParent;
			if (((mapParent != null) ? mapParent.Map : null) == null)
			{
				return;
			}
			foreach (ThingDef def in this.defsToDestroy)
			{
				this.DestroyThingsOfDef(def);
			}
		}

		// Token: 0x06014BD3 RID: 84947 RVA: 0x006364A4 File Offset: 0x006346A4
		public override void Cleanup()
		{
			base.Cleanup();
			this.DestroyThings();
			this.mapParent = null;
		}

		// Token: 0x06014BD4 RID: 84948 RVA: 0x006364BC File Offset: 0x006346BC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Collections.Look<ThingDef>(ref this.defsToDestroy, "defsToDestroy", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x0400E6D4 RID: 59092
		public string inSignal;

		// Token: 0x0400E6D5 RID: 59093
		public MapParent mapParent;

		// Token: 0x0400E6D6 RID: 59094
		public List<ThingDef> defsToDestroy = new List<ThingDef>();

		// Token: 0x0400E6D7 RID: 59095
		private static List<Thing> tmpToDestroy = new List<Thing>();
	}
}
