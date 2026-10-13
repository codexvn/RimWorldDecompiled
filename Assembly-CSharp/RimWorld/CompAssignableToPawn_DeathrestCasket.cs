using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BF1 RID: 11249
	public class CompAssignableToPawn_DeathrestCasket : CompAssignableToPawn_Bed
	{
		// Token: 0x0600FBF7 RID: 64503 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x0600FBF8 RID: 64504 RVA: 0x00491D97 File Offset: 0x0048FF97
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedDeathrestCasket != null;
		}

		// Token: 0x0600FBF9 RID: 64505 RVA: 0x00491DA8 File Offset: 0x0048FFA8
		public override void TryAssignPawn(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			pawn.ownership.ClaimDeathrestCasket(building_Bed);
			building_Bed.NotifyRoomAssignedPawnsChanged();
		}

		// Token: 0x0600FBFA RID: 64506 RVA: 0x00491DD4 File Offset: 0x0048FFD4
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			Building_Bed ownedBed = pawn.ownership.OwnedBed;
			pawn.ownership.UnclaimDeathrestCasket();
			if (ownedBed != null)
			{
				ownedBed.NotifyRoomAssignedPawnsChanged();
			}
		}

		// Token: 0x0600FBFB RID: 64507 RVA: 0x00491E04 File Offset: 0x00490004
		public override AcceptanceReport CanAssignTo(Pawn pawn)
		{
			Building_Bed building_Bed = (Building_Bed)this.parent;
			if (pawn.BodySize > building_Bed.def.building.bed_maxBodySize)
			{
				return "TooLargeForBed".Translate();
			}
			if (building_Bed.ForSlaves && !pawn.IsSlave)
			{
				return "CannotAssignBedToColonist".Translate();
			}
			if (building_Bed.ForColonists && pawn.IsSlave)
			{
				return "CannotAssignBedToSlave".Translate();
			}
			CompDeathrestBindable compDeathrestBindable = this.parent.TryGetComp<CompDeathrestBindable>();
			if (compDeathrestBindable != null && compDeathrestBindable.BoundPawn != null && compDeathrestBindable.BoundPawn != pawn)
			{
				return "CannotAssignAlreadyBound".Translate(compDeathrestBindable.BoundPawn);
			}
			Pawn_GeneTracker genes = pawn.genes;
			Gene_Deathrest gene_Deathrest = (genes != null) ? genes.GetFirstGeneOfType<Gene_Deathrest>() : null;
			if (gene_Deathrest == null)
			{
				return "CannotAssignBedCannotDeathrest".Translate();
			}
			if (compDeathrestBindable != null && gene_Deathrest.BindingWillExceedStackLimit(compDeathrestBindable))
			{
				return "CannotAssignBedCannotBindToMoreBuildings".Translate(this.parent.def.Named("BUILDING"));
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x0600FBFC RID: 64508 RVA: 0x00491F1C File Offset: 0x0049011C
		protected override void PostPostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.PostLoadInit && ModsConfig.BiotechActive && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedDeathrestCasket != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned bed. Removing.");
			}
		}
	}
}
