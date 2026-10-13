using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C7A RID: 7290
	public class QuestPart_RequiredShuttleThings : QuestPart
	{
		// Token: 0x0600A27B RID: 41595 RVA: 0x002EB0F4 File Offset: 0x002E92F4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				if (compShuttle != null)
				{
					compShuttle.requireAllColonistsOnMap = this.requireAllColonistsOnMap;
					compShuttle.requiredColonistCount = this.requiredColonistCount;
				}
			}
		}

		// Token: 0x0600A27C RID: 41596 RVA: 0x002EB144 File Offset: 0x002E9344
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.requireAllColonistsOnMap, "requireAllColonistsOnMap", false, false);
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", -1, false);
		}

		// Token: 0x04007044 RID: 28740
		public Thing shuttle;

		// Token: 0x04007045 RID: 28741
		public MapParent mapParent;

		// Token: 0x04007046 RID: 28742
		public bool requireAllColonistsOnMap;

		// Token: 0x04007047 RID: 28743
		public int requiredColonistCount = -1;

		// Token: 0x04007048 RID: 28744
		public string inSignal;
	}
}
