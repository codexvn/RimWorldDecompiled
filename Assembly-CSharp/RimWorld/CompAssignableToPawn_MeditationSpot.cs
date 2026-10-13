using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BEC RID: 11244
	public class CompAssignableToPawn_MeditationSpot : CompAssignableToPawn
	{
		// Token: 0x0600FBCB RID: 64459 RVA: 0x0049158B File Offset: 0x0048F78B
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandMeditationSpotSetOwnerDesc".Translate();
		}

		// Token: 0x1700278E RID: 10126
		// (get) Token: 0x0600FBCC RID: 64460 RVA: 0x0049159C File Offset: 0x0048F79C
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				return from p in this.parent.Map.mapPawns.FreeColonists
				orderby this.CanAssignTo(p).Accepted descending
				select p;
			}
		}

		// Token: 0x0600FBCD RID: 64461 RVA: 0x004915D8 File Offset: 0x0048F7D8
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

		// Token: 0x0600FBCE RID: 64462 RVA: 0x0049165E File Offset: 0x0048F85E
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedMeditationSpot != null;
		}

		// Token: 0x0600FBCF RID: 64463 RVA: 0x0049166E File Offset: 0x0048F86E
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimMeditationSpot((Building)this.parent);
		}

		// Token: 0x0600FBD0 RID: 64464 RVA: 0x00491687 File Offset: 0x0048F887
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			pawn.ownership.UnclaimMeditationSpot();
		}

		// Token: 0x0600FBD1 RID: 64465 RVA: 0x00491698 File Offset: 0x0048F898
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedMeditationSpot != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned meditation spot. Removing.");
			}
		}
	}
}
