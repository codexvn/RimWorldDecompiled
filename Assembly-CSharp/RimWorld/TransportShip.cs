using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002830 RID: 10288
	public class TransportShip : IExposable, ILoadReferenceable
	{
		// Token: 0x1700224E RID: 8782
		// (get) Token: 0x0600E0E1 RID: 57569 RVA: 0x0041FC9F File Offset: 0x0041DE9F
		public bool ShipExistsAndIsSpawned
		{
			get
			{
				return this.shipThing != null && this.shipThing.Spawned;
			}
		}

		// Token: 0x1700224F RID: 8783
		// (get) Token: 0x0600E0E2 RID: 57570 RVA: 0x0041FCB6 File Offset: 0x0041DEB6
		public bool Disposed
		{
			get
			{
				return this.disposed;
			}
		}

		// Token: 0x17002250 RID: 8784
		// (get) Token: 0x0600E0E3 RID: 57571 RVA: 0x0041FCBE File Offset: 0x0041DEBE
		public bool Waiting
		{
			get
			{
				return this.ShipExistsAndIsSpawned && (this.curJob == null || typeof(ShipJob_Wait).IsAssignableFrom(this.curJob.GetType()));
			}
		}

		// Token: 0x17002251 RID: 8785
		// (get) Token: 0x0600E0E4 RID: 57572 RVA: 0x0041FCEE File Offset: 0x0041DEEE
		public bool ShowGizmos
		{
			get
			{
				return this.curJob != null && this.curJob.ShowGizmos;
			}
		}

		// Token: 0x17002252 RID: 8786
		// (get) Token: 0x0600E0E5 RID: 57573 RVA: 0x0041FD08 File Offset: 0x0041DF08
		private bool CanDispose
		{
			get
			{
				if (this.started && this.curJob == null)
				{
					Thing thing = this.shipThing;
					if (thing == null || !thing.Spawned)
					{
						thing = this.shipThing;
						if (thing == null || !(thing.ParentHolder is ActiveTransporterInfo))
						{
							Thing thing2 = this.shipThing;
							if (thing2 == null || !thing2.IsInCaravan())
							{
								if (!this.shipJobs.Any((ShipJob x) => x.def.blocksDisposalIfQueuedUnspawned) && !Find.World.worldObjects.TravellingTransporters.Any(delegate(TravellingTransporters x)
								{
									TransportersArrivalAction_TransportShip transportersArrivalAction_TransportShip = x.arrivalAction as TransportersArrivalAction_TransportShip;
									return transportersArrivalAction_TransportShip != null && transportersArrivalAction_TransportShip.transportShip == this;
								}) && !Find.QuestManager.IsReservedByAnyQuest(this))
								{
									return true;
								}
							}
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17002253 RID: 8787
		// (get) Token: 0x0600E0E6 RID: 57574 RVA: 0x0041FDC9 File Offset: 0x0041DFC9
		public CompTransporter TransporterComp
		{
			get
			{
				if (this.cachedCompTransporter == null)
				{
					this.cachedCompTransporter = this.shipThing.TryGetComp<CompTransporter>();
				}
				return this.cachedCompTransporter;
			}
		}

		// Token: 0x17002254 RID: 8788
		// (get) Token: 0x0600E0E7 RID: 57575 RVA: 0x0041FDEA File Offset: 0x0041DFEA
		public CompShuttle ShuttleComp
		{
			get
			{
				if (this.cachedCompShuttle == null)
				{
					this.cachedCompShuttle = this.shipThing.TryGetComp<CompShuttle>();
				}
				return this.cachedCompShuttle;
			}
		}

		// Token: 0x17002255 RID: 8789
		// (get) Token: 0x0600E0E8 RID: 57576 RVA: 0x0041FE0C File Offset: 0x0041E00C
		public bool LeavingSoonAutomatically
		{
			get
			{
				if (this.curJob != null && this.curJob.def == ShipJobDefOf.FlyAway)
				{
					return true;
				}
				foreach (ShipJob shipJob in this.shipJobs)
				{
					if (shipJob.Interruptible)
					{
						return false;
					}
					if (shipJob.def == ShipJobDefOf.FlyAway)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17002256 RID: 8790
		// (get) Token: 0x0600E0E9 RID: 57577 RVA: 0x0041FE94 File Offset: 0x0041E094
		public bool HasPredeterminedDestination
		{
			get
			{
				if (this.curJob != null && this.curJob.HasDestination)
				{
					return true;
				}
				using (List<ShipJob>.Enumerator enumerator = this.shipJobs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HasDestination)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x0600E0EA RID: 57578 RVA: 0x0041FF04 File Offset: 0x0041E104
		public TransportShip()
		{
		}

		// Token: 0x0600E0EB RID: 57579 RVA: 0x0041FF2C File Offset: 0x0041E12C
		public TransportShip(TransportShipDef def)
		{
			this.def = def;
			this.loadID = Find.UniqueIDsManager.GetNextTransportShipID();
			Find.TransportShipManager.RegisterShipObject(this);
		}

		// Token: 0x0600E0EC RID: 57580 RVA: 0x0041FF7E File Offset: 0x0041E17E
		public void Start()
		{
			if (this.started)
			{
				return;
			}
			this.started = true;
			this.TryGetNextJob();
		}

		// Token: 0x0600E0ED RID: 57581 RVA: 0x0041FF98 File Offset: 0x0041E198
		public void Tick()
		{
			if (!this.started)
			{
				return;
			}
			if (this.curJob == null || this.curJob.jobState == ShipJobState.Ended)
			{
				this.TryGetNextJob();
			}
			if (this.curJob != null)
			{
				this.curJob.TickInterval(1);
				return;
			}
			if (this.CanDispose)
			{
				this.Dispose();
			}
		}

		// Token: 0x0600E0EE RID: 57582 RVA: 0x0041FFED File Offset: 0x0041E1ED
		public void ForceJob(ShipJob shipJob)
		{
			if (this.curJob != null && this.curJob.Interruptible)
			{
				this.EndCurrentJob();
			}
			this.shipJobs.Clear();
			this.AddJob(shipJob);
			this.TryGetNextJob();
		}

		// Token: 0x0600E0EF RID: 57583 RVA: 0x00420022 File Offset: 0x0041E222
		public void ForceJob(ShipJobDef def)
		{
			this.ForceJob(ShipJobMaker.MakeShipJob(def));
		}

		// Token: 0x0600E0F0 RID: 57584 RVA: 0x00420030 File Offset: 0x0041E230
		public void SetNextJob(ShipJob shipJob)
		{
			if (this.Disposed)
			{
				Log.Error("Trying to add a job to a disposed transport ship. id=" + this.GetUniqueLoadID());
				return;
			}
			shipJob.transportShip = this;
			this.shipJobs.Insert(0, shipJob);
		}

		// Token: 0x0600E0F1 RID: 57585 RVA: 0x00420064 File Offset: 0x0041E264
		public void ForceJob_DelayCurrent(ShipJob shipJob)
		{
			if (this.curJob != null)
			{
				this.shipJobs.Insert(0, this.curJob);
				this.curJob = null;
			}
			if (!this.started)
			{
				this.Start();
			}
			this.SetNextJob(shipJob);
			this.TryGetNextJob();
		}

		// Token: 0x0600E0F2 RID: 57586 RVA: 0x004200A2 File Offset: 0x0041E2A2
		public void AddJob(ShipJob shipJob)
		{
			if (this.Disposed)
			{
				Log.Error("Trying to add a job to a disposed transport ship. id=" + this.GetUniqueLoadID());
				return;
			}
			shipJob.transportShip = this;
			this.shipJobs.Add(shipJob);
		}

		// Token: 0x0600E0F3 RID: 57587 RVA: 0x004200D5 File Offset: 0x0041E2D5
		public void AddJob(ShipJobDef def)
		{
			this.AddJob(ShipJobMaker.MakeShipJob(def));
		}

		// Token: 0x0600E0F4 RID: 57588 RVA: 0x004200E4 File Offset: 0x0041E2E4
		public void AddJobs(params ShipJobDef[] defs)
		{
			if (defs == null)
			{
				return;
			}
			for (int i = 0; i < defs.Length; i++)
			{
				this.AddJob(defs[i]);
			}
		}

		// Token: 0x0600E0F5 RID: 57589 RVA: 0x0042010C File Offset: 0x0041E30C
		public void EndCurrentJob()
		{
			if (this.curJob != null)
			{
				this.curJob.End();
			}
			this.curJob = null;
		}

		// Token: 0x0600E0F6 RID: 57590 RVA: 0x00420128 File Offset: 0x0041E328
		public void TryGetNextJob()
		{
			this.EndCurrentJob();
			if (this.shipJobs.NullOrEmpty<ShipJob>())
			{
				return;
			}
			ShipJob shipJob = this.shipJobs.First<ShipJob>();
			if (shipJob.TryStart())
			{
				this.curJob = shipJob;
				this.shipJobs.Remove(shipJob);
			}
		}

		// Token: 0x0600E0F7 RID: 57591 RVA: 0x00420174 File Offset: 0x0041E374
		public void ArriveAt(IntVec3 cell, MapParent mapParent)
		{
			if (this.curJob != null && this.curJob.Interruptible)
			{
				this.EndCurrentJob();
			}
			ShipJob_Arrive shipJob_Arrive = (ShipJob_Arrive)ShipJobMaker.MakeShipJob(ShipJobDefOf.Arrive);
			shipJob_Arrive.cell = cell;
			shipJob_Arrive.mapParent = mapParent;
			this.SetNextJob(shipJob_Arrive);
			this.Start();
			if (this.curJob == null || this.curJob.Interruptible)
			{
				this.TryGetNextJob();
			}
		}

		// Token: 0x0600E0F8 RID: 57592 RVA: 0x004201E4 File Offset: 0x0041E3E4
		public void Dispose()
		{
			if (this.def.playerShuttle)
			{
				this.shipThing.TryGetComp<CompShuttle>().shipParent = null;
			}
			else
			{
				Thing thing = this.shipThing;
				if (thing != null && !thing.Destroyed)
				{
					this.shipThing.Destroy(DestroyMode.QuestLogic);
				}
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "Disposed", this.Named("SUBJECT"));
			Find.TransportShipManager.DeregisterShipObject(this);
		}

		// Token: 0x0600E0F9 RID: 57593 RVA: 0x00420258 File Offset: 0x0041E458
		public void LogJobs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Jobs for " + this.GetUniqueLoadID() + ":");
			if (this.curJob == null)
			{
				stringBuilder.AppendLine("  - CurJob: null");
			}
			else
			{
				stringBuilder.AppendLine("  - CurJob: " + this.curJob.GetJobInfo());
			}
			foreach (ShipJob shipJob in this.shipJobs)
			{
				stringBuilder.AppendLine("  - " + shipJob.GetJobInfo());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0600E0FA RID: 57594 RVA: 0x0042031C File Offset: 0x0041E51C
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<bool>(ref this.disposed, "disposed", false, false);
			Scribe_Values.Look<bool>(ref this.started, "started", false, false);
			Scribe_Defs.Look<TransportShipDef>(ref this.def, "def");
			Scribe_Deep.Look<ShipJob>(ref this.curJob, "curJob", Array.Empty<object>());
			Scribe_Collections.Look<ShipJob>(ref this.shipJobs, "shipJobs", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.questTags, "questTags", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (this.shipThing != null && this.shipThing.Destroyed)
				{
					this.shipThing = null;
				}
				this.shipThingSpawnedOrHasOwner = (this.shipThing != null && (this.shipThing.SpawnedOrAnyParentSpawned || this.shipThing.ParentHolder != null));
			}
			Scribe_Values.Look<bool>(ref this.shipThingSpawnedOrHasOwner, "shipThingSpawned", false, false);
			if (this.shipThingSpawnedOrHasOwner)
			{
				Scribe_References.Look<Thing>(ref this.shipThing, "shipThing", false);
				return;
			}
			Scribe_Deep.Look<Thing>(ref this.shipThing, "shipThing", Array.Empty<object>());
		}

		// Token: 0x0600E0FB RID: 57595 RVA: 0x00420444 File Offset: 0x0041E644
		public string GetUniqueLoadID()
		{
			return "TransportShip_" + this.loadID.ToString();
		}

		// Token: 0x04009730 RID: 38704
		public int loadID = -1;

		// Token: 0x04009731 RID: 38705
		public TransportShipDef def;

		// Token: 0x04009732 RID: 38706
		public ShipJob curJob;

		// Token: 0x04009733 RID: 38707
		public Thing shipThing;

		// Token: 0x04009734 RID: 38708
		private List<ShipJob> shipJobs = new List<ShipJob>();

		// Token: 0x04009735 RID: 38709
		public List<string> questTags = new List<string>();

		// Token: 0x04009736 RID: 38710
		public bool started;

		// Token: 0x04009737 RID: 38711
		private bool disposed;

		// Token: 0x04009738 RID: 38712
		private bool shipThingSpawnedOrHasOwner;

		// Token: 0x04009739 RID: 38713
		[Unsaved(false)]
		private CompShuttle cachedCompShuttle;

		// Token: 0x0400973A RID: 38714
		[Unsaved(false)]
		private CompTransporter cachedCompTransporter;
	}
}
