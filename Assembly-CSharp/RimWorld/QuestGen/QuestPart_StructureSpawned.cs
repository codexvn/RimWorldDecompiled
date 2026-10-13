using System;
using RimWorld.Utility;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BAE RID: 15278
	public class QuestPart_StructureSpawned : QuestPart
	{
		// Token: 0x06014BF2 RID: 84978 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_StructureSpawned()
		{
		}

		// Token: 0x06014BF3 RID: 84979 RVA: 0x006373C6 File Offset: 0x006355C6
		public QuestPart_StructureSpawned(string spawnedSignal, Thing structure)
		{
			this.spawnedSignal = spawnedSignal;
			this.structure = structure;
		}

		// Token: 0x06014BF4 RID: 84980 RVA: 0x006373DC File Offset: 0x006355DC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.spawnedSignal, "spawnedSignal", null, false);
			Scribe_References.Look<Thing>(ref this.structure, "structure", false);
		}

		// Token: 0x06014BF5 RID: 84981 RVA: 0x00637408 File Offset: 0x00635608
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag != this.spawnedSignal)
			{
				return;
			}
			VoidAwakeningUtility.SpawnVoidMetalAround(this.structure, 50, QuestPart_StructureSpawned.NumMassesRange.RandomInRange, true, null);
			VoidAwakeningUtility.SpawnMetalhorrorDefenders(this.structure);
		}

		// Token: 0x0400E6F7 RID: 59127
		private const int VoidMetalPatchSize = 50;

		// Token: 0x0400E6F8 RID: 59128
		private static readonly IntRange NumMassesRange = new IntRange(3, 4);

		// Token: 0x0400E6F9 RID: 59129
		private string spawnedSignal;

		// Token: 0x0400E6FA RID: 59130
		private Thing structure;
	}
}
