using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001776 RID: 6006
	public abstract class ShipJob : IExposable, ILoadReferenceable
	{
		// Token: 0x170015A1 RID: 5537
		// (get) Token: 0x06008CF1 RID: 36081
		protected abstract bool ShouldEnd { get; }

		// Token: 0x170015A2 RID: 5538
		// (get) Token: 0x06008CF2 RID: 36082 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool HasDestination
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170015A3 RID: 5539
		// (get) Token: 0x06008CF3 RID: 36083 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Interruptible
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170015A4 RID: 5540
		// (get) Token: 0x06008CF4 RID: 36084 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool ShowGizmos
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008CF5 RID: 36085 RVA: 0x0029EE82 File Offset: 0x0029D082
		public ShipJob()
		{
		}

		// Token: 0x06008CF6 RID: 36086 RVA: 0x0029EE91 File Offset: 0x0029D091
		public ShipJob(TransportShip transportShip)
		{
			this.loadID = Find.UniqueIDsManager.GetNextShipJobID();
			this.transportShip = transportShip;
		}

		// Token: 0x06008CF7 RID: 36087 RVA: 0x0029EEB7 File Offset: 0x0029D0B7
		public virtual bool TryStart()
		{
			if (this.transportShip == null)
			{
				Log.Error("Trying to start a ship job with a null ship object.");
				return false;
			}
			if (this.jobState == ShipJobState.Ended)
			{
				Log.Error("Trying to start an already ended ship job.");
				return false;
			}
			this.jobState = ShipJobState.Working;
			return true;
		}

		// Token: 0x06008CF8 RID: 36088 RVA: 0x0029EEEA File Offset: 0x0029D0EA
		public virtual void End()
		{
			this.jobState = ShipJobState.Ended;
		}

		// Token: 0x06008CF9 RID: 36089 RVA: 0x0029EEF3 File Offset: 0x0029D0F3
		public virtual void TickInterval(int delta)
		{
			if (this.jobState != ShipJobState.Working)
			{
				Log.Error("Trying to tick " + this.jobState.ToString() + " job.");
			}
			if (this.ShouldEnd)
			{
				this.End();
			}
		}

		// Token: 0x06008CFA RID: 36090 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual IEnumerable<Gizmo> GetJobGizmos()
		{
			return null;
		}

		// Token: 0x06008CFB RID: 36091 RVA: 0x0029EF31 File Offset: 0x0029D131
		public virtual string GetJobInfo()
		{
			return base.GetType().Name;
		}

		// Token: 0x06008CFC RID: 36092 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ShipThingExtraInspectString()
		{
			return null;
		}

		// Token: 0x06008CFD RID: 36093 RVA: 0x0029EF40 File Offset: 0x0029D140
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<ShipJobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<ShipJobState>(ref this.jobState, "jobState", ShipJobState.Uninitialized, false);
			Scribe_References.Look<TransportShip>(ref this.transportShip, "transportShip", false);
		}

		// Token: 0x06008CFE RID: 36094 RVA: 0x0029EF92 File Offset: 0x0029D192
		public string GetUniqueLoadID()
		{
			return "ShipJob_" + this.loadID.ToString();
		}

		// Token: 0x04005B9B RID: 23451
		public ShipJobDef def;

		// Token: 0x04005B9C RID: 23452
		public int loadID = -1;

		// Token: 0x04005B9D RID: 23453
		public ShipJobState jobState;

		// Token: 0x04005B9E RID: 23454
		public TransportShip transportShip;
	}
}
