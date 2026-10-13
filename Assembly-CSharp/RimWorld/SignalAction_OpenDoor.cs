using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AC2 RID: 10946
	public class SignalAction_OpenDoor : SignalAction
	{
		// Token: 0x0600F3E0 RID: 62432 RVA: 0x0046DAC8 File Offset: 0x0046BCC8
		protected override void DoAction(SignalArgs args)
		{
			if (this.door != null)
			{
				Pawn opener;
				if (args.TryGetArg<Pawn>("SUBJECT", out opener))
				{
					this.door.StartManualOpenBy(opener);
					return;
				}
				this.door.StartManualOpenBy(null);
			}
		}

		// Token: 0x0600F3E1 RID: 62433 RVA: 0x0046DB06 File Offset: 0x0046BD06
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_Door>(ref this.door, "door", false);
		}

		// Token: 0x0400A0B1 RID: 41137
		public Building_Door door;
	}
}
