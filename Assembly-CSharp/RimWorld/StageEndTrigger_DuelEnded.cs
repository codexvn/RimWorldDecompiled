using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020026A6 RID: 9894
	public class StageEndTrigger_DuelEnded : StageEndTrigger_AnyPawnDead
	{
		// Token: 0x0600D91D RID: 55581 RVA: 0x00404F48 File Offset: 0x00403148
		protected override bool Trigger(LordJob_Ritual ritual)
		{
			if (base.Trigger(ritual))
			{
				return true;
			}
			foreach (string roleId in this.roleIds)
			{
				using (IEnumerator<Pawn> enumerator2 = ritual.assignments.AssignedPawns(roleId).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Downed)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
