using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B6 RID: 14262
	public class RoomRequirement_ForbidAltars : RoomRequirement
	{
		// Token: 0x06013C44 RID: 80964 RVA: 0x005DDDAC File Offset: 0x005DBFAC
		public override bool Met(Room r, Pawn p = null)
		{
			using (List<Thing>.Enumerator enumerator = r.ContainedAndAdjacentThings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.def.isAltar)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
