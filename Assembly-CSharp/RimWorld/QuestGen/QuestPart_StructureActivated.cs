using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BAF RID: 15279
	public class QuestPart_StructureActivated : QuestPart
	{
		// Token: 0x06014BF7 RID: 84983 RVA: 0x002D92DC File Offset: 0x002D74DC
		public QuestPart_StructureActivated()
		{
		}

		// Token: 0x06014BF8 RID: 84984 RVA: 0x00637466 File Offset: 0x00635666
		public QuestPart_StructureActivated(int structureCount, string activatedSignal, string destroyedSignal, string outSignal, int stage)
		{
			this.structureCount = structureCount;
			this.activatedSignal = activatedSignal;
			this.destroyedSignal = destroyedSignal;
			this.outSignal = outSignal;
			this.stage = stage;
		}

		// Token: 0x06014BF9 RID: 84985 RVA: 0x00637494 File Offset: 0x00635694
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			Thing thing;
			if ((signal.tag == this.activatedSignal || signal.tag == this.destroyedSignal) && signal.args.TryGetArg<Thing>("SUBJECT", out thing))
			{
				Pawn arg;
				signal.args.TryGetArg<Pawn>("ACTIVATOR", out arg);
				this.activatedStructureCount++;
				Messages.Message("StructureActivatedMessage".Translate(arg.Named("PAWN"), thing.Named("STRUCTURE")) + ((this.structureCount > 1) ? string.Format(" ({0}/{1}).", this.activatedStructureCount, this.structureCount) : "."), thing, MessageTypeDefOf.PositiveEvent, true);
				if (this.activatedStructureCount == this.structureCount)
				{
					Signal signal2 = new Signal(this.outSignal, false);
					signal2.args.Add(arg.Named("ACTIVATOR"));
					Find.SignalManager.SendSignal(signal2);
					switch (this.stage)
					{
					case 1:
						Find.LetterStack.ReceiveLetter("VoidAwakeningStageOneStructuresActivatedLabel".Translate(), "VoidAwakeningStageOneStructuresActivatedText".Translate(), LetterDefOf.NegativeEvent, null, 0, true);
						return;
					case 2:
						Find.LetterStack.ReceiveLetter("VoidAwakeningStageTwoStructuresActivatedLabel".Translate(), "VoidAwakeningStageTwoStructuresActivatedText".Translate(), LetterDefOf.NegativeEvent, null, 0, true);
						break;
					case 3:
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x06014BFA RID: 84986 RVA: 0x0063761C File Offset: 0x0063581C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.activatedSignal, "studiedSignal", null, false);
			Scribe_Values.Look<string>(ref this.destroyedSignal, "destroyedSignal", null, false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
			Scribe_Values.Look<int>(ref this.structureCount, "structureCount", 0, false);
			Scribe_Values.Look<int>(ref this.activatedStructureCount, "studiedStructureCount", 0, false);
			Scribe_Values.Look<int>(ref this.stage, "wave", 0, false);
		}

		// Token: 0x0400E6FB RID: 59131
		private string activatedSignal;

		// Token: 0x0400E6FC RID: 59132
		private string destroyedSignal;

		// Token: 0x0400E6FD RID: 59133
		private string outSignal;

		// Token: 0x0400E6FE RID: 59134
		private int structureCount;

		// Token: 0x0400E6FF RID: 59135
		private int activatedStructureCount;

		// Token: 0x0400E700 RID: 59136
		private int stage;
	}
}
