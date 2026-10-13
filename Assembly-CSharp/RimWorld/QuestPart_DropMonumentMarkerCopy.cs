using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C3C RID: 7228
	public class QuestPart_DropMonumentMarkerCopy : QuestPart
	{
		// Token: 0x170018BE RID: 6334
		// (get) Token: 0x0600A0EB RID: 41195 RVA: 0x002E4FA7 File Offset: 0x002E31A7
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				if (this.copy != null)
				{
					yield return this.copy;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A0EC RID: 41196 RVA: 0x002E4FB8 File Offset: 0x002E31B8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				this.copy = null;
				MonumentMarker arg = signal.args.GetArg<MonumentMarker>("SUBJECT");
				if (this.mapParent == null || !this.mapParent.HasMap)
				{
					Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
					this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
				}
				if (arg != null && this.mapParent != null && this.mapParent.HasMap)
				{
					Map map = this.mapParent.Map;
					IntVec3 dropCenter = DropCellFinder.RandomDropSpot(map, true);
					this.copy = (MonumentMarker)ThingMaker.MakeThing(ThingDefOf.MonumentMarker, null);
					this.copy.sketch = arg.sketch.DeepCopy();
					if (!arg.questTags.NullOrEmpty<string>())
					{
						this.copy.questTags = new List<string>();
						this.copy.questTags.AddRange(arg.questTags);
					}
					DropPodUtility.DropThingsNear(dropCenter, map, Gen.YieldSingle<Thing>(this.copy.MakeMinified(DestroyMode.Vanish)), 110, false, false, true, false, true, null);
				}
				if (!this.outSignalResult.NullOrEmpty())
				{
					if (this.copy != null)
					{
						Find.SignalManager.SendSignal(new Signal(this.outSignalResult, this.copy.Named("SUBJECT")));
						return;
					}
					Find.SignalManager.SendSignal(new Signal(this.outSignalResult, false));
				}
			}
		}

		// Token: 0x0600A0ED RID: 41197 RVA: 0x002E512A File Offset: 0x002E332A
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyOrPassToWorldOnCleanup && this.copy != null)
			{
				QuestPart_DestroyThingsOrPassToWorld.Destroy(this.copy);
			}
		}

		// Token: 0x0600A0EE RID: 41198 RVA: 0x002E5150 File Offset: 0x002E3350
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignalResult, "outSignalResult", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_References.Look<MonumentMarker>(ref this.copy, "copy", false);
			Scribe_Values.Look<bool>(ref this.destroyOrPassToWorldOnCleanup, "destroyOrPassToWorldOnCleanup", false, false);
		}

		// Token: 0x0600A0EF RID: 41199 RVA: 0x002E51BC File Offset: 0x002E33BC
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			if (Find.AnyPlayerHomeMap != null)
			{
				this.mapParent = Find.RandomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x04006F3C RID: 28476
		public MapParent mapParent;

		// Token: 0x04006F3D RID: 28477
		public string inSignal;

		// Token: 0x04006F3E RID: 28478
		public string outSignalResult;

		// Token: 0x04006F3F RID: 28479
		public bool destroyOrPassToWorldOnCleanup;

		// Token: 0x04006F40 RID: 28480
		private MonumentMarker copy;
	}
}
