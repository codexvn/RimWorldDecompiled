using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DC4 RID: 11716
	public class CompProximityLetter : ThingComp
	{
		// Token: 0x17002A0C RID: 10764
		// (get) Token: 0x060107A2 RID: 67490 RVA: 0x004C0D86 File Offset: 0x004BEF86
		public CompProperties_ProximityLetter Props
		{
			get
			{
				return (CompProperties_ProximityLetter)this.props;
			}
		}

		// Token: 0x060107A3 RID: 67491 RVA: 0x004C0D94 File Offset: 0x004BEF94
		public void SendLetter(Pawn triggerer)
		{
			Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(triggerer.Named("PAWN")), this.Props.letterText.Formatted(triggerer.Named("PAWN")), this.Props.letterDef, this.parent, null, null, null, null, 0, true);
			this.letterSent = true;
		}

		// Token: 0x060107A4 RID: 67492 RVA: 0x004C0E04 File Offset: 0x004BF004
		public override void CompTick()
		{
			base.CompTick();
			if (this.letterSent || Find.Anomaly.Level > 0)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = this.parent.Map;
			int num = GenRadial.NumCellsInRadius((float)this.Props.radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn = thingList[j] as Pawn;
						if (pawn != null && pawn.IsColonistPlayerControlled && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && pawn.Awake() && GenSight.LineOfSightToThing(pawn.Position, this.parent, this.parent.Map, false, null))
						{
							this.SendLetter(pawn);
							return;
						}
					}
				}
			}
		}

		// Token: 0x060107A5 RID: 67493 RVA: 0x004C0F1C File Offset: 0x004BF11C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.letterSent, "letterSent", false, false);
		}

		// Token: 0x0400ABE0 RID: 44000
		public bool letterSent;
	}
}
