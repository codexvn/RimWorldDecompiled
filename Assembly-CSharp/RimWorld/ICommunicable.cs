using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029E6 RID: 10726
	public interface ICommunicable
	{
		// Token: 0x0600ECB4 RID: 60596
		string GetCallLabel();

		// Token: 0x0600ECB5 RID: 60597
		string GetInfoText();

		// Token: 0x0600ECB6 RID: 60598
		void TryOpenComms(Pawn negotiator);

		// Token: 0x0600ECB7 RID: 60599
		Faction GetFaction();

		// Token: 0x0600ECB8 RID: 60600
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
