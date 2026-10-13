using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200185F RID: 6239
	public class ThoughtWorker_ImprisonedByFaction : ThoughtWorker
	{
		// Token: 0x06008FA4 RID: 36772 RVA: 0x002A696B File Offset: 0x002A4B6B
		protected override ThoughtState CurrentSocialStateInternal(Pawn p, Pawn other)
		{
			return p.IsPrisoner && p.guest.HostFaction == other.Faction;
		}
	}
}
