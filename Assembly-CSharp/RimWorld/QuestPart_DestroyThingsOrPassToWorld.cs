using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C24 RID: 7204
	public class QuestPart_DestroyThingsOrPassToWorld : QuestPart
	{
		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x0600A070 RID: 41072 RVA: 0x002E3ADF File Offset: 0x002E1CDF
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				if (this.questLookTargets)
				{
					int num;
					for (int i = 0; i < this.things.Count; i = num + 1)
					{
						yield return this.things[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x0600A071 RID: 41073 RVA: 0x002E3AEF File Offset: 0x002E1CEF
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.things);
			}
		}

		// Token: 0x0600A072 RID: 41074 RVA: 0x002E3B18 File Offset: 0x002E1D18
		public static void Destroy(List<Thing> things)
		{
			for (int i = things.Count - 1; i >= 0; i--)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(things[i]);
			}
		}

		// Token: 0x0600A073 RID: 41075 RVA: 0x002E3B44 File Offset: 0x002E1D44
		public static void Destroy(Thing thing)
		{
			Thing thing2;
			if (thing.ParentHolder is MinifiedThing)
			{
				thing2 = (Thing)thing.ParentHolder;
			}
			else
			{
				thing2 = thing;
			}
			if (!thing2.Destroyed)
			{
				thing2.DestroyOrPassToWorld(DestroyMode.QuestLogic);
			}
		}

		// Token: 0x0600A074 RID: 41076 RVA: 0x002E3B80 File Offset: 0x002E1D80
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.questLookTargets, "questLookTargets", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.things.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0600A075 RID: 41077 RVA: 0x002E3C00 File Offset: 0x002E1E00
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				List<Thing> source = Find.RandomPlayerHomeMap.listerThings.ThingsInGroup(ThingRequestGroup.Plant);
				this.things.Clear();
				this.things.Add(source.FirstOrDefault<Thing>());
			}
		}

		// Token: 0x04006EEF RID: 28399
		public string inSignal;

		// Token: 0x04006EF0 RID: 28400
		public List<Thing> things = new List<Thing>();

		// Token: 0x04006EF1 RID: 28401
		public bool questLookTargets = true;
	}
}
