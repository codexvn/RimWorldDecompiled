using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002722 RID: 10018
	public class RitualObligationTrigger_MemberDied : RitualObligationTrigger
	{
		// Token: 0x0600DBF2 RID: 56306 RVA: 0x0040CF18 File Offset: 0x0040B118
		public override void Notify_MemberDied(Pawn p)
		{
			if ((this.mustBePlayerIdeo && !Faction.OfPlayer.ideos.Has(this.ritual.ideo)) || (p.HomeFaction != Faction.OfPlayer && !p.IsSlave) || !p.IsFreeColonist || p.IsKidnapped())
			{
				return;
			}
			this.ritual.AddObligation(new RitualObligation(this.ritual, p, true)
			{
				sendLetter = !p.IsSlave
			});
		}
	}
}
