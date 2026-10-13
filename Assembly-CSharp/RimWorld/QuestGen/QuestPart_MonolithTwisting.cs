using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BBA RID: 15290
	public class QuestPart_MonolithTwisting : QuestPart_Alert_StructuresArriving
	{
		// Token: 0x06014C3D RID: 85053 RVA: 0x0063877B File Offset: 0x0063697B
		public QuestPart_MonolithTwisting()
		{
		}

		// Token: 0x06014C3E RID: 85054 RVA: 0x00638783 File Offset: 0x00636983
		public QuestPart_MonolithTwisting(string label, string explanation, string inSignalEnable, string inSignalDisable, int delay, Building_VoidMonolith monolith, int intensifiedDelay) : base(label, explanation, inSignalEnable, inSignalDisable, delay, monolith)
		{
			this.monolith = monolith;
			this.intensifiedDelay = intensifiedDelay;
		}

		// Token: 0x06014C3F RID: 85055 RVA: 0x006387A9 File Offset: 0x006369A9
		protected override void Enable(SignalArgs receivedArgs)
		{
			this.intensifyTick = Find.TickManager.TicksGame + this.intensifiedDelay;
			base.Enable(receivedArgs);
		}

		// Token: 0x06014C40 RID: 85056 RVA: 0x006387CC File Offset: 0x006369CC
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.effecter == null)
			{
				this.effecter = EffecterDefOf.MonolithTwisting.Spawn();
			}
			if (Find.TickManager.TicksGame >= this.intensifyTick && this.intenseEffecter == null)
			{
				this.intenseEffecter = EffecterDefOf.MonolithTwistingIntense.SpawnMaintained(this.monolith, this.monolith.MapHeld, 1f);
			}
			this.effecter.EffectTick(this.monolith, this.monolith);
		}

		// Token: 0x06014C41 RID: 85057 RVA: 0x00638858 File Offset: 0x00636A58
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_VoidMonolith>(ref this.monolith, "monolith", false);
			Scribe_Values.Look<int>(ref this.intensifyTick, "intensifyTick", 0, false);
		}

		// Token: 0x0400E727 RID: 59175
		private Building_VoidMonolith monolith;

		// Token: 0x0400E728 RID: 59176
		private int intensifyTick;

		// Token: 0x0400E729 RID: 59177
		private Effecter effecter;

		// Token: 0x0400E72A RID: 59178
		private Effecter intenseEffecter;

		// Token: 0x0400E72B RID: 59179
		private int intensifiedDelay;
	}
}
