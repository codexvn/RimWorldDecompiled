using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020027C1 RID: 10177
	public abstract class RitualSpectatorFilter
	{
		// Token: 0x0600DEEB RID: 57067
		public abstract bool Allowed(Pawn p);

		// Token: 0x04009622 RID: 38434
		[MustTranslate]
		public string description;
	}
}
