using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002B07 RID: 11015
	public class InsectLairEntrance : MapPortal
	{
		// Token: 0x0600F5F4 RID: 62964 RVA: 0x00476C9D File Offset: 0x00474E9D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.spawnGravcore, "spawnGravcore", false, false);
		}

		// Token: 0x0600F5F5 RID: 62965 RVA: 0x00476CB8 File Offset: 0x00474EB8
		public override void OnEntered(Pawn pawn)
		{
			if (!this.beenEntered)
			{
				TaggedString taggedString = "EnteredMegahiveText".Translate(pawn.Named("PAWN"));
				if (this.spawnGravcore)
				{
					taggedString += "\n\n" + "EnteredMegahiveGravcoreExtra".Translate();
				}
				Find.LetterStack.ReceiveLetter("EnteredMegahiveLabel".Translate(), taggedString, LetterDefOf.ThreatBig, this.exit, null, null, null, null, 0, true);
			}
			base.OnEntered(pawn);
		}

		// Token: 0x0400A1DF RID: 41439
		public bool spawnGravcore;
	}
}
