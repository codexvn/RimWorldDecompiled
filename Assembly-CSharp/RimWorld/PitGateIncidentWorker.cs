using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB1 RID: 6833
	public abstract class PitGateIncidentWorker : IExposable, ILoadReferenceable
	{
		// Token: 0x0600990A RID: 39178 RVA: 0x002C993B File Offset: 0x002C7B3B
		protected PitGateIncidentWorker()
		{
			this.loadID = Find.UniqueIDsManager.GetNextPitGateIncidentID();
		}

		// Token: 0x0600990B RID: 39179 RVA: 0x002C9965 File Offset: 0x002C7B65
		public virtual void Setup(float points)
		{
			this.fireTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600990C RID: 39180 RVA: 0x002C9977 File Offset: 0x002C7B77
		public virtual bool CanFireNow()
		{
			return this.pitGate != null && !this.pitGate.Destroyed;
		}

		// Token: 0x0600990D RID: 39181 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick()
		{
		}

		// Token: 0x0600990E RID: 39182 RVA: 0x002C9991 File Offset: 0x002C7B91
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Defs.Look<PitGateIncidentDef>(ref this.def, "def");
			Scribe_References.Look<Building>(ref this.pitGate, "pitGate", false);
		}

		// Token: 0x0600990F RID: 39183 RVA: 0x002C99C6 File Offset: 0x002C7BC6
		public string GetUniqueLoadID()
		{
			return "PitGateIncident_" + this.def.defName + "_" + this.loadID.ToString();
		}

		// Token: 0x040067ED RID: 26605
		private int loadID = -1;

		// Token: 0x040067EE RID: 26606
		public PitGateIncidentDef def;

		// Token: 0x040067EF RID: 26607
		public Building pitGate;

		// Token: 0x040067F0 RID: 26608
		public int fireTick = -99999;
	}
}
