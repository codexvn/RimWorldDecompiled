using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002ADB RID: 10971
	public class FilthRevenantSmear : Filth
	{
		// Token: 0x0600F47C RID: 62588 RVA: 0x00470B90 File Offset: 0x0046ED90
		private void SendLetter(Pawn triggerer)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelRevenantSmearDiscovered".Translate(), "LetterRevenantSmearDiscovered".Translate(triggerer.Named("PAWN")), LetterDefOf.ThreatSmall, this, null, null, null, null, 0, true);
			this.revenant.TryGetComp<CompRevenant>().revenantSmearNotified = true;
		}

		// Token: 0x0600F47D RID: 62589 RVA: 0x00470BE8 File Offset: 0x0046EDE8
		protected override void Tick()
		{
			Pawn pawn = this.revenant;
			if (((pawn != null) ? pawn.mindState : null) == null)
			{
				return;
			}
			if (this.revenant.TryGetComp<CompRevenant>().revenantSmearNotified)
			{
				return;
			}
			if (this.revenant.mindState.lastBecameVisibleTick > 0 || this.revenant.mindState.lastForcedVisibleTick > 0)
			{
				return;
			}
			if (this.IsHashIntervalTick(60))
			{
				int num = GenRadial.NumCellsInRadius(4f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = base.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Pawn pawn2 = thingList[j] as Pawn;
							if (pawn2 != null && pawn2.IsColonistPlayerControlled)
							{
								this.SendLetter(pawn2);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x0400A11C RID: 41244
		private const int Radius = 4;

		// Token: 0x0400A11D RID: 41245
		public Pawn revenant;
	}
}
