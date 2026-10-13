using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002AD8 RID: 10968
	public class FilthGrayFleshNoticeable : Filth
	{
		// Token: 0x0600F46C RID: 62572 RVA: 0x00470588 File Offset: 0x0046E788
		protected override void Tick()
		{
			if (this.sentLetter)
			{
				return;
			}
			foreach (Pawn pawn in base.Map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.Position.InHorDistOf(base.Position, 5f) && PawnUtility.ShouldSendNotificationAbout(pawn) && !MetalhorrorUtility.IsInfected(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && pawn.health.capacities.CanBeAwake)
				{
					this.SendLetter(pawn);
					break;
				}
			}
		}

		// Token: 0x0600F46D RID: 62573 RVA: 0x00470644 File Offset: 0x0046E844
		private void SendLetter(Pawn pawn)
		{
			this.sentLetter = true;
			string text = "";
			if (!Find.Anomaly.hasSeenGrayFlesh)
			{
				text = "LetterInterrogationUnlocked".Translate() + " ";
			}
			text += "LetterGrayFleshMethodOne".Translate();
			string arg = string.Concat(new string[]
			{
				"- ",
				text,
				"\n\n",
				string.Format("- {0}\n\n", "LetterGrayFleshMethodTwo".Translate()),
				string.Format("- {0}", "LetterGrayFleshMethodThree".Translate())
			});
			TaggedString text2 = "LetterGrayFleshDiscovered".Translate(pawn.Named("PAWN"), arg.Named("METHODS"));
			Find.LetterStack.ReceiveLetter("LetterGrayFleshDiscoveredLabel".Translate(), text2, LetterDefOf.ThreatSmall, new LookTargets(base.Position, base.Map), null, null, null, null, 0, true);
			Thing thing = ThingMaker.MakeThing(ThingDefOf.GrayFleshSample, null);
			thing.TryGetComp<CompAnalyzableBiosignature>().biosignature = this.biosignature;
			GenSpawn.Spawn(thing, base.Position, base.Map, WipeMode.Vanish);
			Find.Anomaly.hasSeenGrayFlesh = true;
		}

		// Token: 0x0600F46E RID: 62574 RVA: 0x0047077E File Offset: 0x0046E97E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sentLetter, "sentLetter", false, false);
			Scribe_Values.Look<int>(ref this.biosignature, "biosignature", 0, false);
		}

		// Token: 0x0400A117 RID: 41239
		private bool sentLetter;

		// Token: 0x0400A118 RID: 41240
		public int biosignature;

		// Token: 0x0400A119 RID: 41241
		private const int DetectDist = 5;
	}
}
