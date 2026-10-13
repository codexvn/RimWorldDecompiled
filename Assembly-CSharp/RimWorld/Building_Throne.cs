using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002A56 RID: 10838
	public class Building_Throne : Building
	{
		// Token: 0x1700257B RID: 9595
		// (get) Token: 0x0600F0BE RID: 61630 RVA: 0x0045D5A4 File Offset: 0x0045B7A4
		public static IEnumerable<RoyalTitleDef> AllTitlesForThroneStature
		{
			get
			{
				return from title in DefDatabase<RoyalTitleDef>.AllDefsListForReading
				where !title.throneRoomRequirements.NullOrEmpty<RoomRequirement>()
				orderby title.seniority
				select title;
			}
		}

		// Token: 0x1700257C RID: 9596
		// (get) Token: 0x0600F0BF RID: 61631 RVA: 0x0045D5FE File Offset: 0x0045B7FE
		public Pawn AssignedPawn
		{
			get
			{
				if (!ModLister.CheckRoyalty("Throne"))
				{
					return null;
				}
				if (this.CompAssignableToPawn == null || !this.CompAssignableToPawn.AssignedPawnsForReading.Any<Pawn>())
				{
					return null;
				}
				return this.CompAssignableToPawn.AssignedPawnsForReading[0];
			}
		}

		// Token: 0x1700257D RID: 9597
		// (get) Token: 0x0600F0C0 RID: 61632 RVA: 0x0045D63B File Offset: 0x0045B83B
		public CompAssignableToPawn_Throne CompAssignableToPawn
		{
			get
			{
				return base.GetComp<CompAssignableToPawn_Throne>();
			}
		}

		// Token: 0x1700257E RID: 9598
		// (get) Token: 0x0600F0C1 RID: 61633 RVA: 0x0045D644 File Offset: 0x0045B844
		public RoyalTitleDef TitleStature
		{
			get
			{
				Room room = this.GetRoom(RegionType.Set_All);
				if (room == null || room.OutdoorsForWork)
				{
					return null;
				}
				RoyalTitleDef result = null;
				foreach (RoyalTitleDef royalTitleDef in Building_Throne.AllTitlesForThroneStature)
				{
					bool flag = true;
					for (int i = 0; i < royalTitleDef.throneRoomRequirements.Count; i++)
					{
						if (!(royalTitleDef.throneRoomRequirements[i] is RoomRequirement_HasAssignedThroneAnyOf) && !royalTitleDef.throneRoomRequirements[i].MetOrDisabled(room, null))
						{
							flag = false;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					result = royalTitleDef;
				}
				return result;
			}
		}

		// Token: 0x0600F0C2 RID: 61634 RVA: 0x0045D6F4 File Offset: 0x0045B8F4
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			Room room = this.GetRoom(RegionType.Set_All);
			Pawn p = (this.CompAssignableToPawn.AssignedPawnsForReading.Count == 1) ? this.CompAssignableToPawn.AssignedPawnsForReading[0] : null;
			RoyalTitleDef titleStature = this.TitleStature;
			text += "\n" + "ThroneMaxSatisfiedTitle".Translate() + ": " + ((titleStature == null) ? "None".Translate() : titleStature.GetLabelCapFor(p));
			string text2 = RoomRoleWorker_ThroneRoom.Validate(room);
			if (text2 != null)
			{
				return text + "\n" + text2;
			}
			return text;
		}

		// Token: 0x0600F0C3 RID: 61635 RVA: 0x0045D7A0 File Offset: 0x0045B9A0
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.CheckRoyalty("Throne"))
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}
	}
}
