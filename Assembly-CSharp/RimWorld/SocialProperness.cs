using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001787 RID: 6023
	public static class SocialProperness
	{
		// Token: 0x06008D56 RID: 36182 RVA: 0x002A08F1 File Offset: 0x0029EAF1
		public static bool IsSociallyProper(this Thing t, Pawn p)
		{
			return t.IsSociallyProper(p, p.IsPrisonerOfColony, false);
		}

		// Token: 0x06008D57 RID: 36183 RVA: 0x002A0904 File Offset: 0x0029EB04
		public static bool IsSociallyProper(this Thing t, Pawn p, bool forPrisoner, bool animalsCare = false)
		{
			if (!animalsCare && p != null && !p.RaceProps.Humanlike)
			{
				return true;
			}
			if (!t.def.socialPropernessMatters)
			{
				return true;
			}
			if (!t.Spawned)
			{
				return true;
			}
			IntVec3 intVec = t.def.hasInteractionCell ? t.InteractionCell : t.Position;
			if (forPrisoner)
			{
				return p == null || intVec.GetRoom(t.Map) == p.GetRoom(RegionType.Set_All);
			}
			if (ModsConfig.BiotechActive && t.def == ThingDefOf.HemogenPack)
			{
				return !SocialProperness.BloodfeedingPrisonerInRoom(t.GetRoom(RegionType.Set_All));
			}
			return !intVec.IsInPrisonCell(t.Map);
		}

		// Token: 0x06008D58 RID: 36184 RVA: 0x002A09B0 File Offset: 0x0029EBB0
		public static bool BloodfeedingPrisonerInRoom(Room r)
		{
			if (r == null || !r.IsPrisonCell)
			{
				return false;
			}
			using (IEnumerator<Pawn> enumerator = r.Owners.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IsBloodfeeder())
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
