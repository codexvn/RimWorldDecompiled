using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BED RID: 11245
	public class CompAssignableToPawn_Throne : CompAssignableToPawn
	{
		// Token: 0x0600FBD5 RID: 64469 RVA: 0x00491724 File Offset: 0x0048F924
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandThroneSetOwnerDesc".Translate();
		}

		// Token: 0x0600FBD6 RID: 64470 RVA: 0x00491735 File Offset: 0x0048F935
		protected override bool CanSetUninstallAssignedPawn(Pawn pawn)
		{
			return pawn != null && !this.AssignedAnything(pawn) && this.CanAssignTo(pawn) && pawn.IsColonist;
		}

		// Token: 0x1700278F RID: 10127
		// (get) Token: 0x0600FBD7 RID: 64471 RVA: 0x0049175C File Offset: 0x0048F95C
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists.Where(delegate(Pawn p)
				{
					Faction faction;
					return p.royalty != null && (p.royalty.AllTitlesForReading.Any<RoyalTitle>() || p.royalty.CanUpdateTitleOfAnyFaction(out faction));
				})
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x0600FBD8 RID: 64472 RVA: 0x004917B8 File Offset: 0x0048F9B8
		public override string CompInspectStringExtra()
		{
			if (base.AssignedPawnsForReading.Count == 0)
			{
				return "Owner".Translate() + ": " + "Nobody".Translate();
			}
			if (base.AssignedPawnsForReading.Count == 1)
			{
				return "Owner".Translate() + ": " + base.AssignedPawnsForReading[0].Label;
			}
			return "";
		}

		// Token: 0x0600FBD9 RID: 64473 RVA: 0x0049183E File Offset: 0x0048FA3E
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedThrone != null;
		}

		// Token: 0x0600FBDA RID: 64474 RVA: 0x0049184E File Offset: 0x0048FA4E
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimThrone((Building_Throne)this.parent);
		}

		// Token: 0x0600FBDB RID: 64475 RVA: 0x00491867 File Offset: 0x0048FA67
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			pawn.ownership.UnclaimThrone();
			if (uninstall && pawn != null && !this.uninstalledAssignedPawns.Contains(pawn))
			{
				this.uninstalledAssignedPawns.Add(pawn);
			}
		}

		// Token: 0x0600FBDC RID: 64476 RVA: 0x00491898 File Offset: 0x0048FA98
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedThrone != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned throne. Removing.");
			}
		}
	}
}
