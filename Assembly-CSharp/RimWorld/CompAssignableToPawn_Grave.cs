using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002BF2 RID: 11250
	public class CompAssignableToPawn_Grave : CompAssignableToPawn
	{
		// Token: 0x17002791 RID: 10129
		// (get) Token: 0x0600FBFF RID: 64511 RVA: 0x00491F8C File Offset: 0x0049018C
		public override IEnumerable<Pawn> AssigningCandidates
		{
			get
			{
				if (!this.parent.Spawned)
				{
					return Enumerable.Empty<Pawn>();
				}
				IEnumerable<Pawn> second = from Corpse x in this.parent.Map.listerThings.ThingsInGroup(ThingRequestGroup.Corpse)
				where x.InnerPawn.IsColonist
				select x.InnerPawn;
				return this.parent.Map.mapPawns.FreeColonistsSpawned.Concat(second);
			}
		}

		// Token: 0x0600FC00 RID: 64512 RVA: 0x0049202B File Offset: 0x0049022B
		public override bool AssignedAnything(Pawn pawn)
		{
			return pawn.ownership.AssignedGrave != null;
		}

		// Token: 0x0600FC01 RID: 64513 RVA: 0x0049203B File Offset: 0x0049023B
		public override void TryAssignPawn(Pawn pawn)
		{
			pawn.ownership.ClaimGrave((Building_Grave)this.parent);
		}

		// Token: 0x0600FC02 RID: 64514 RVA: 0x00492054 File Offset: 0x00490254
		public override void TryUnassignPawn(Pawn pawn, bool sort = true, bool uninstall = false)
		{
			pawn.ownership.UnclaimGrave();
		}

		// Token: 0x0600FC03 RID: 64515 RVA: 0x00492062 File Offset: 0x00490262
		protected override bool ShouldShowAssignmentGizmo()
		{
			return !((Building_Grave)this.parent).HasCorpse;
		}

		// Token: 0x0600FC04 RID: 64516 RVA: 0x00492077 File Offset: 0x00490277
		protected override string GetAssignmentGizmoLabel()
		{
			return "CommandGraveAssignColonistLabel".Translate();
		}

		// Token: 0x0600FC05 RID: 64517 RVA: 0x00492088 File Offset: 0x00490288
		protected override string GetAssignmentGizmoDesc()
		{
			return "CommandGraveAssignColonistDesc".Translate();
		}

		// Token: 0x0600FC06 RID: 64518 RVA: 0x0049209C File Offset: 0x0049029C
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.assignedPawns.RemoveAll((Pawn x) => x.ownership.AssignedGrave != this.parent) > 0)
			{
				Log.Warning(this.parent.ToStringSafe<ThingWithComps>() + " had pawns assigned that don't have it as an assigned grave. Removing.");
			}
		}
	}
}
