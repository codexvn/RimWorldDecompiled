using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C63 RID: 7267
	public class QuestPart_MechCluster : QuestPart
	{
		// Token: 0x170018E6 RID: 6374
		// (get) Token: 0x0600A1ED RID: 41453 RVA: 0x002E87F8 File Offset: 0x002E69F8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in base.QuestLookTargets)
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.spawnedClusterPos.IsValid && this.mapParent != null && this.mapParent.HasMap)
				{
					yield return new GlobalTargetInfo(this.spawnedClusterPos, this.mapParent.Map, false);
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A1EE RID: 41454 RVA: 0x002E8808 File Offset: 0x002E6A08
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				Map anyPlayerHomeMap = Find.AnyPlayerHomeMap;
				this.mapParent = ((anyPlayerHomeMap != null) ? anyPlayerHomeMap.Parent : null);
			}
			if (signal.tag == this.inSignal && this.mapParent != null && this.mapParent.HasMap)
			{
				this.spawnedClusterPos = this.dropSpot;
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					this.spawnedClusterPos = MechClusterUtility.FindClusterPosition(this.mapParent.Map, this.sketch, 100, 0.5f);
				}
				if (this.spawnedClusterPos == IntVec3.Invalid)
				{
					return;
				}
				MechClusterUtility.SpawnCluster(this.spawnedClusterPos, this.mapParent.Map, this.sketch, true, false, this.tag);
				Find.LetterStack.ReceiveLetter("LetterLabelMechClusterArrived".Translate(), "LetterMechClusterArrived".Translate(), LetterDefOf.ThreatBig, new TargetInfo(this.spawnedClusterPos, this.mapParent.Map, false), null, this.quest, null, null, 0, true);
			}
		}

		// Token: 0x0600A1EF RID: 41455 RVA: 0x002E893C File Offset: 0x002E6B3C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<string>(ref this.tag, "tag", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Deep.Look<MechClusterSketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<IntVec3>(ref this.dropSpot, "dropSpot", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.spawnedClusterPos, "spawnedClusterPos", default(IntVec3), false);
		}

		// Token: 0x04006FE0 RID: 28640
		public MechClusterSketch sketch;

		// Token: 0x04006FE1 RID: 28641
		public string inSignal;

		// Token: 0x04006FE2 RID: 28642
		public string tag;

		// Token: 0x04006FE3 RID: 28643
		public MapParent mapParent;

		// Token: 0x04006FE4 RID: 28644
		public IntVec3 dropSpot = IntVec3.Invalid;

		// Token: 0x04006FE5 RID: 28645
		private IntVec3 spawnedClusterPos = IntVec3.Invalid;
	}
}
