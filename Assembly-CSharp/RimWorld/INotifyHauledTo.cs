using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029AD RID: 10669
	public interface INotifyHauledTo
	{
		// Token: 0x0600EB1D RID: 60189
		void Notify_HauledTo(Pawn hauler, Thing thing, int count);
	}
}
