using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003431 RID: 13361
	public class PawnColumnWorker_ReleaseAnimalToWild : PawnColumnWorker_Designator
	{
		// Token: 0x17003138 RID: 12600
		// (get) Token: 0x06012CCC RID: 77004 RVA: 0x005949E3 File Offset: 0x00592BE3
		protected override DesignationDef DesignationType
		{
			get
			{
				return DesignationDefOf.ReleaseAnimalToWild;
			}
		}

		// Token: 0x06012CCD RID: 77005 RVA: 0x005949EA File Offset: 0x00592BEA
		protected override string GetTip(Pawn pawn)
		{
			return "DesignatorReleaseAnimalToWildDesc".Translate();
		}

		// Token: 0x06012CCE RID: 77006 RVA: 0x005949FB File Offset: 0x00592BFB
		protected override bool HasCheckbox(Pawn pawn)
		{
			return pawn.RaceProps.Animal && pawn.RaceProps.IsFlesh && pawn.Faction == Faction.OfPlayer && pawn.SpawnedOrAnyParentSpawned;
		}

		// Token: 0x06012CCF RID: 77007 RVA: 0x00594A2C File Offset: 0x00592C2C
		protected override void Notify_DesignationAdded(Pawn pawn)
		{
			if (pawn.Faction != Faction.OfPlayer)
			{
				TaggedString taggedString = "AnimalReleaseConfirm".Translate(pawn.Named("PAWN"), pawn.Faction.Named("FACTION"));
				Find.WindowStack.Add(new Dialog_Confirm(taggedString, delegate()
				{
					ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal(pawn);
				}));
				return;
			}
			ReleaseAnimalToWildUtility.CheckWarnAboutBondedAnimal(pawn);
		}

		// Token: 0x06012CD0 RID: 77008 RVA: 0x00594AB8 File Offset: 0x00592CB8
		protected override bool ShouldConfirmDesignation(Pawn pawn, out string title)
		{
			if (pawn.HomeFaction == Faction.OfPlayer || pawn.HomeFaction == null)
			{
				return base.ShouldConfirmDesignation(pawn, out title);
			}
			title = "AnimalReleaseConfirm".Translate(pawn.Named("PAWN"), pawn.HomeFaction.Named("FACTION"));
			return true;
		}
	}
}
