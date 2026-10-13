using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020024CB RID: 9419
	public class Pawn_Ownership : IExposable
	{
		// Token: 0x17001F5D RID: 8029
		// (get) Token: 0x0600CD23 RID: 52515 RVA: 0x003C25FB File Offset: 0x003C07FB
		// (set) Token: 0x0600CD24 RID: 52516 RVA: 0x003C2603 File Offset: 0x003C0803
		public Building_Bed OwnedBed
		{
			get
			{
				return this.intOwnedBed;
			}
			private set
			{
				if (this.intOwnedBed != value)
				{
					this.intOwnedBed = value;
					ThoughtUtility.RemovePositiveBedroomThoughts(this.pawn);
				}
			}
		}

		// Token: 0x17001F5E RID: 8030
		// (get) Token: 0x0600CD25 RID: 52517 RVA: 0x003C2620 File Offset: 0x003C0820
		// (set) Token: 0x0600CD26 RID: 52518 RVA: 0x003C2628 File Offset: 0x003C0828
		public Building_Grave AssignedGrave { get; private set; }

		// Token: 0x17001F5F RID: 8031
		// (get) Token: 0x0600CD27 RID: 52519 RVA: 0x003C2631 File Offset: 0x003C0831
		// (set) Token: 0x0600CD28 RID: 52520 RVA: 0x003C2639 File Offset: 0x003C0839
		public Building_Throne AssignedThrone { get; private set; }

		// Token: 0x17001F60 RID: 8032
		// (get) Token: 0x0600CD29 RID: 52521 RVA: 0x003C2642 File Offset: 0x003C0842
		// (set) Token: 0x0600CD2A RID: 52522 RVA: 0x003C264A File Offset: 0x003C084A
		public Building AssignedMeditationSpot { get; private set; }

		// Token: 0x17001F61 RID: 8033
		// (get) Token: 0x0600CD2B RID: 52523 RVA: 0x003C2653 File Offset: 0x003C0853
		// (set) Token: 0x0600CD2C RID: 52524 RVA: 0x003C265B File Offset: 0x003C085B
		public Building_Bed AssignedDeathrestCasket { get; private set; }

		// Token: 0x17001F62 RID: 8034
		// (get) Token: 0x0600CD2D RID: 52525 RVA: 0x003C2664 File Offset: 0x003C0864
		public Room OwnedRoom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				Room room = this.OwnedBed.GetRoom(RegionType.Set_All);
				if (room == null)
				{
					return null;
				}
				if (room.Owners.Contains(this.pawn))
				{
					return room;
				}
				return null;
			}
		}

		// Token: 0x17001F63 RID: 8035
		// (get) Token: 0x0600CD2E RID: 52526 RVA: 0x003C26A4 File Offset: 0x003C08A4
		public Room Bedroom
		{
			get
			{
				if (this.OwnedBed == null)
				{
					return null;
				}
				return this.OwnedBed.GetRoom(RegionType.Set_All);
			}
		}

		// Token: 0x0600CD2F RID: 52527 RVA: 0x003C26BD File Offset: 0x003C08BD
		public Pawn_Ownership(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600CD30 RID: 52528 RVA: 0x003C26CC File Offset: 0x003C08CC
		public void ExposeData()
		{
			Building_Grave assignedGrave = this.AssignedGrave;
			Building_Throne assignedThrone = this.AssignedThrone;
			Building assignedMeditationSpot = this.AssignedMeditationSpot;
			Building_Bed assignedDeathrestCasket = this.AssignedDeathrestCasket;
			Scribe_References.Look<Building_Bed>(ref this.intOwnedBed, "ownedBed", false);
			Scribe_References.Look<Building>(ref assignedMeditationSpot, "assignedMeditationSpot", false);
			Scribe_References.Look<Building_Grave>(ref assignedGrave, "assignedGrave", false);
			Scribe_References.Look<Building_Throne>(ref assignedThrone, "assignedThrone", false);
			Scribe_References.Look<Building_Bed>(ref assignedDeathrestCasket, "assignedDeathrestCasket", false);
			this.AssignedGrave = assignedGrave;
			this.AssignedThrone = assignedThrone;
			this.AssignedMeditationSpot = assignedMeditationSpot;
			this.AssignedDeathrestCasket = assignedDeathrestCasket;
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.intOwnedBed != null)
				{
					CompAssignableToPawn compAssignableToPawn = this.intOwnedBed.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn != null && !compAssignableToPawn.AssignedPawns.Contains(this.pawn))
					{
						Building_Bed newBed = this.intOwnedBed;
						this.UnclaimBed();
						this.ClaimBedIfNonMedical(newBed);
					}
				}
				if (this.AssignedGrave != null)
				{
					CompAssignableToPawn compAssignableToPawn2 = this.AssignedGrave.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn2 != null && !compAssignableToPawn2.AssignedPawns.Contains(this.pawn))
					{
						Building_Grave assignedGrave2 = this.AssignedGrave;
						this.UnclaimGrave();
						this.ClaimGrave(assignedGrave2);
					}
				}
				if (this.AssignedThrone != null)
				{
					CompAssignableToPawn compAssignableToPawn3 = this.AssignedThrone.TryGetComp<CompAssignableToPawn>();
					if (compAssignableToPawn3 != null && !compAssignableToPawn3.AssignedPawns.Contains(this.pawn))
					{
						Building_Throne assignedThrone2 = this.AssignedThrone;
						this.UnclaimThrone();
						this.ClaimThrone(assignedThrone2);
					}
				}
			}
		}

		// Token: 0x0600CD31 RID: 52529 RVA: 0x003C2830 File Offset: 0x003C0A30
		public bool ClaimBedIfNonMedical(Building_Bed newBed)
		{
			if (newBed.IsOwner(this.pawn) || newBed.Medical)
			{
				return false;
			}
			if (ModsConfig.BiotechActive && newBed.def == ThingDefOf.DeathrestCasket)
			{
				this.UnclaimDeathrestCasket();
				newBed.CompAssignableToPawn.ForceAddPawn(this.pawn);
				this.AssignedDeathrestCasket = newBed;
				return true;
			}
			this.UnclaimBed();
			if (newBed.OwnersForReading.Count == newBed.SleepingSlotsCount)
			{
				newBed.OwnersForReading[newBed.OwnersForReading.Count - 1].ownership.UnclaimBed();
			}
			newBed.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.OwnedBed = newBed;
			if (this.pawn.IsFreeman && newBed.CompAssignableToPawn.IdeoligionForbids(this.pawn))
			{
				Log.Error("Assigned " + this.pawn.GetUniqueLoadID() + " to a bed against their or occupants' ideo.");
			}
			if (newBed.Medical)
			{
				Log.Warning(this.pawn.LabelCap + " claimed medical bed.");
				this.UnclaimBed();
			}
			return true;
		}

		// Token: 0x0600CD32 RID: 52530 RVA: 0x003C2948 File Offset: 0x003C0B48
		public bool UnclaimBed()
		{
			if (this.OwnedBed == null)
			{
				return false;
			}
			this.OwnedBed.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.OwnedBed = null;
			return true;
		}

		// Token: 0x0600CD33 RID: 52531 RVA: 0x003C2974 File Offset: 0x003C0B74
		public bool ClaimGrave(Building_Grave newGrave)
		{
			if (newGrave.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimGrave();
			if (newGrave.AssignedPawn != null)
			{
				newGrave.AssignedPawn.ownership.UnclaimGrave();
			}
			newGrave.CompAssignableToPawn.ForceAddPawn(this.pawn);
			newGrave.GetStoreSettings().Priority = StoragePriority.Critical;
			this.AssignedGrave = newGrave;
			return true;
		}

		// Token: 0x0600CD34 RID: 52532 RVA: 0x003C29D6 File Offset: 0x003C0BD6
		public bool UnclaimGrave()
		{
			if (this.AssignedGrave == null)
			{
				return false;
			}
			this.AssignedGrave.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedGrave.GetStoreSettings().Priority = StoragePriority.Important;
			this.AssignedGrave = null;
			return true;
		}

		// Token: 0x0600CD35 RID: 52533 RVA: 0x003C2A14 File Offset: 0x003C0C14
		public bool ClaimThrone(Building_Throne newThrone)
		{
			if (newThrone.AssignedPawn == this.pawn)
			{
				return false;
			}
			this.UnclaimThrone();
			if (newThrone.AssignedPawn != null)
			{
				newThrone.AssignedPawn.ownership.UnclaimThrone();
			}
			newThrone.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.AssignedThrone = newThrone;
			return true;
		}

		// Token: 0x0600CD36 RID: 52534 RVA: 0x003C2A6A File Offset: 0x003C0C6A
		public bool UnclaimThrone()
		{
			if (this.AssignedThrone == null)
			{
				return false;
			}
			this.AssignedThrone.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedThrone = null;
			return true;
		}

		// Token: 0x0600CD37 RID: 52535 RVA: 0x003C2A94 File Offset: 0x003C0C94
		public bool ClaimMeditationSpot(Building newSpot)
		{
			if (newSpot.GetAssignedPawn() == this.pawn)
			{
				return false;
			}
			this.UnclaimMeditationSpot();
			if (newSpot.GetAssignedPawn() != null)
			{
				newSpot.GetAssignedPawn().ownership.UnclaimMeditationSpot();
			}
			newSpot.TryGetComp<CompAssignableToPawn>().ForceAddPawn(this.pawn);
			this.AssignedMeditationSpot = newSpot;
			return true;
		}

		// Token: 0x0600CD38 RID: 52536 RVA: 0x003C2AEA File Offset: 0x003C0CEA
		public bool UnclaimMeditationSpot()
		{
			if (this.AssignedMeditationSpot == null)
			{
				return false;
			}
			this.AssignedMeditationSpot.TryGetComp<CompAssignableToPawn>().ForceRemovePawn(this.pawn);
			this.AssignedMeditationSpot = null;
			return true;
		}

		// Token: 0x0600CD39 RID: 52537 RVA: 0x003C2B14 File Offset: 0x003C0D14
		public bool ClaimDeathrestCasket(Building_Bed deathrestCasket)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (deathrestCasket.CompAssignableToPawn.AssignedPawns.Contains(this.pawn))
			{
				return false;
			}
			this.UnclaimDeathrestCasket();
			if (deathrestCasket.GetAssignedPawn() != null)
			{
				deathrestCasket.GetAssignedPawn().ownership.UnclaimDeathrestCasket();
			}
			deathrestCasket.CompAssignableToPawn.ForceAddPawn(this.pawn);
			this.AssignedDeathrestCasket = deathrestCasket;
			return true;
		}

		// Token: 0x0600CD3A RID: 52538 RVA: 0x003C2B7D File Offset: 0x003C0D7D
		public bool UnclaimDeathrestCasket()
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			if (this.AssignedDeathrestCasket == null)
			{
				return false;
			}
			this.AssignedDeathrestCasket.CompAssignableToPawn.ForceRemovePawn(this.pawn);
			this.AssignedDeathrestCasket = null;
			return true;
		}

		// Token: 0x0600CD3B RID: 52539 RVA: 0x003C2BB0 File Offset: 0x003C0DB0
		public void UnclaimAll()
		{
			this.UnclaimBed();
			this.UnclaimGrave();
			this.UnclaimThrone();
			this.UnclaimDeathrestCasket();
		}

		// Token: 0x0600CD3C RID: 52540 RVA: 0x003C2BD0 File Offset: 0x003C0DD0
		public void Notify_ChangedGuestStatus()
		{
			if (this.OwnedBed != null && ((this.OwnedBed.ForPrisoners && !this.pawn.IsPrisoner && !PawnUtility.IsBeingArrested(this.pawn)) || (!this.OwnedBed.ForPrisoners && this.pawn.IsPrisoner) || (this.OwnedBed.ForColonists && this.pawn.HostFaction == null)))
			{
				this.UnclaimBed();
			}
			if (this.AssignedDeathrestCasket != null && ((this.AssignedDeathrestCasket.ForPrisoners && !this.pawn.IsPrisoner && !PawnUtility.IsBeingArrested(this.pawn)) || (!this.AssignedDeathrestCasket.ForPrisoners && this.pawn.IsPrisoner) || (this.AssignedDeathrestCasket.ForColonists && this.pawn.HostFaction == null)))
			{
				this.UnclaimDeathrestCasket();
			}
		}

		// Token: 0x04008BAE RID: 35758
		private Pawn pawn;

		// Token: 0x04008BAF RID: 35759
		private Building_Bed intOwnedBed;
	}
}
