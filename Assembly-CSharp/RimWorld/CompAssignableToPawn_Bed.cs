using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BEF RID: 11247
	public class CompAssignableToPawn_Bed : CompAssignableToPawn
	{
		// Token: 0x17002790 RID: 10128
		// (get) Token: 0x0600FBE3 RID: 64483 RVA: 0x00491960 File Offset: 0x0048FB60
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				if (!this.parent.def.building.bed_humanlike)
				{
					return from x in this.parent.Map.mapPawns.PawnsInFaction(Faction.OfPlayer)
					where x.IsAnimal && !x.RaceProps.Dryad
					select x into p
					orderby p.kindDef.label, p.Label
					select p;
				}
				return this.parent.Map.mapPawns.FreeColonists.OrderByDescending(delegate(Pawn p)
				{
					if (!this.CanAssignTo(p).Accepted)
					{
						return 0;
					}
					if (this.IdeoligionForbids(p))
					{
						return 0;
					}
					return 1;
				}).ThenBy((Pawn p) => p.LabelShort);
			}
		}

		// Token: 0x0600FBE4 RID: 64484 RVA: 0x00491A6C File Offset: 0x0048FC6C
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandBedSetOwnerDesc".Translate(this.parent.def.building.bed_humanlike ? FactionDefOf.PlayerColony.pawnSingular : "Animal".Translate().ToString());
		}

		// Token: 0x0600FBE5 RID: 64485 RVA: 0x00491AC8 File Offset: 0x0048FCC8
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.OwnedBed != null;
		}

		// Token: 0x0600FBE6 RID: 64486 RVA: 0x00491AD8 File Offset: 0x0048FCD8
		public override void TryAssignPawn(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			pawn.ownership.ClaimBedIfNonMedical(building_Bed);
			building_Bed.NotifyRoomAssignedPawnsChanged();
			this.uninstalledAssignedPawns.Remove(pawn);
		}

		// Token: 0x0600FBE7 RID: 64487 RVA: 0x00491B14 File Offset: 0x0048FD14
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			pawn.ownership.UnclaimBed();
			if (ownedBed != null)
			{
				ownedBed.NotifyRoomAssignedPawnsChanged();
			}
			if (uninstall && !this.uninstalledAssignedPawns.Contains(pawn))
			{
				this.uninstalledAssignedPawns.Add(pawn);
			}
		}

		// Token: 0x0600FBE8 RID: 64488 RVA: 0x00491B60 File Offset: 0x0048FD60
		protected override bool ShouldShowAssignmentGizmo()
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			return building_Bed.Faction == Faction.OfPlayer && !building_Bed.ForPrisoners && !building_Bed.Medical;
		}

		// Token: 0x0600FBE9 RID: 64489 RVA: 0x00491B9C File Offset: 0x0048FD9C
		public override AcceptanceReport CanAssignTo(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			if (pawn.BodySize > building_Bed.def.building.bed_maxBodySize)
			{
				return "TooLargeForBed".Translate();
			}
			if (!pawn.DevelopmentalStage.Baby())
			{
				if (building_Bed.ForSlaves && !pawn.IsSlave)
				{
					return "CannotAssignBedToColonist".Translate();
				}
				if (building_Bed.ForColonists && pawn.IsSlave)
				{
					return "CannotAssignBedToSlave".Translate();
				}
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600FBEA RID: 64490 RVA: 0x00491C2F File Offset: 0x0048FE2F
		protected override bool CanSetUninstallAssignedPawn(Pawn pawn)
		{
			return pawn != null && !this.AssignedAnything(pawn) && this.CanAssignTo(pawn) && (pawn.IsPrisonerOfColony || pawn.IsColonist);
		}

		// Token: 0x0600FBEB RID: 64491 RVA: 0x00491C60 File Offset: 0x0048FE60
		public override bool IdeoligionForbids(Pawn pawn)
		{
			if (!ModsConfig.IdeologyActive || base.Props.maxAssignedPawnsCount == 1)
			{
				return base.IdeoligionForbids(pawn);
			}
			foreach (Pawn pawn2 in base.AssignedPawns)
			{
				if (!BedUtility.WillingToShareBed(pawn, pawn2))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600FBEC RID: 64492 RVA: 0x00491CD4 File Offset: 0x0048FED4
		protected virtual void PostPostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.OwnedBed != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.");
			}
		}

		// Token: 0x0600FBED RID: 64493 RVA: 0x00491D12 File Offset: 0x0048FF12
		public override void PostExposeData()
		{
			base.PostExposeData();
			this.PostPostExposeData();
		}
	}
}
