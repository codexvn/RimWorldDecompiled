using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002446 RID: 9286
	[StaticConstructorOnStartup]
	public static class GuestUtility
	{
		// Token: 0x17001E58 RID: 7768
		// (get) Token: 0x0600C905 RID: 51461 RVA: 0x003ABC46 File Offset: 0x003A9E46
		public static Texture2D SlaveIcon
		{
			get
			{
				if (GuestUtility.slaveIcon == null)
				{
					GuestUtility.slaveIcon = ContentFinder<Texture2D>.Get("UI/Icons/Slavery", true);
				}
				return GuestUtility.slaveIcon;
			}
		}

		// Token: 0x0600C906 RID: 51462 RVA: 0x003ABC6A File Offset: 0x003A9E6A
		public static Texture2D GetGuestIcon(GuestStatus guestStatus)
		{
			if (guestStatus == GuestStatus.Slave)
			{
				return GuestUtility.SlaveIcon;
			}
			return null;
		}

		// Token: 0x0600C907 RID: 51463 RVA: 0x003ABC78 File Offset: 0x003A9E78
		public static List<WorkTypeDef> GetDisabledWorkTypes(this Pawn_GuestTracker guest)
		{
			GuestUtility.tmpDisabledWorkTypes.Clear();
			if (guest.IsSlave)
			{
				foreach (WorkTypeDef workTypeDef in DefDatabase<WorkTypeDef>.AllDefs)
				{
					if (workTypeDef.disabledForSlaves)
					{
						GuestUtility.tmpDisabledWorkTypes.Add(workTypeDef);
					}
				}
			}
			return GuestUtility.tmpDisabledWorkTypes;
		}

		// Token: 0x0600C908 RID: 51464 RVA: 0x003ABCE8 File Offset: 0x003A9EE8
		public static bool IsSellingToSlavery(Pawn slave, Faction slaveOwner)
		{
			return (slave.IsSlave || slave.IsPrisoner) && slave.HomeFaction != slaveOwner;
		}

		// Token: 0x0600C909 RID: 51465 RVA: 0x003ABD08 File Offset: 0x003A9F08
		public static void GetExtraFactionsFromGuestStatus(Pawn pawn, List<ExtraFaction> outExtraFactions)
		{
			if (pawn.SlaveFaction != null)
			{
				outExtraFactions.Add(new ExtraFaction(pawn.SlaveFaction, ExtraFactionType.HomeFaction));
			}
		}

		// Token: 0x0600C90A RID: 51466 RVA: 0x003ABD24 File Offset: 0x003A9F24
		public static void Notify_PrisonerEscaped(Pawn prisoner)
		{
			ThoughtToAddToAll thoughtToAddToAll = new ThoughtToAddToAll(ThoughtDefOf.ColonyPrisonerEscaped, prisoner);
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_Colonists)
			{
				if (pawn != prisoner)
				{
					thoughtToAddToAll.Add(pawn);
				}
			}
		}

		// Token: 0x040088C0 RID: 35008
		public static Texture2D SlaveSuppressionFillTex = SolidColorMaterials.NewSolidColorTexture(new Color32(245, 209, 66, byte.MaxValue));

		// Token: 0x040088C1 RID: 35009
		public static readonly Texture2D RansomIcon = ContentFinder<Texture2D>.Get("UI/Icons/Ransom", true);

		// Token: 0x040088C2 RID: 35010
		private static Texture2D slaveIcon;

		// Token: 0x040088C3 RID: 35011
		private static List<WorkTypeDef> tmpDisabledWorkTypes = new List<WorkTypeDef>();
	}
}
