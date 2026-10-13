using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EF6 RID: 12022
	public abstract class CompGrayStatue : ThingComp
	{
		// Token: 0x17002BD5 RID: 11221
		// (get) Token: 0x06010F93 RID: 69523 RVA: 0x004E290C File Offset: 0x004E0B0C
		private CompProperties_GrayStatue Props
		{
			get
			{
				return (CompProperties_GrayStatue)this.props;
			}
		}

		// Token: 0x06010F94 RID: 69524 RVA: 0x004E2919 File Offset: 0x004E0B19
		public override void PostPostMake()
		{
			this.triggerRadius = this.Props.triggerRadiusRange.RandomInRange;
		}

		// Token: 0x06010F95 RID: 69525 RVA: 0x004E2934 File Offset: 0x004E0B34
		public override string CompInspectStringExtra()
		{
			return (this.triggered ? "Activated" : "ActivatedByProximity").Translate().CapitalizeFirst();
		}

		// Token: 0x06010F96 RID: 69526 RVA: 0x004E2967 File Offset: 0x004E0B67
		public override void CompTick()
		{
			if (this.triggered)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckTrigger();
			}
		}

		// Token: 0x06010F97 RID: 69527 RVA: 0x004E298C File Offset: 0x004E0B8C
		private void CheckTrigger()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.triggerRadius, false))
			{
				if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map))
				{
					foreach (Thing thing in intVec.GetThingList(this.parent.Map))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							this.triggered = true;
							this.Trigger(pawn);
							TaggedString label = this.Props.letterLabel.Formatted(pawn.Named("PAWN"));
							TaggedString text = this.Props.letterText.Formatted(pawn.Named("PAWN"));
							Find.LetterStack.ReceiveLetter(label, text, this.Props.letterDef ?? LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
							return;
						}
					}
				}
			}
		}

		// Token: 0x06010F98 RID: 69528
		protected abstract void Trigger(Pawn target);

		// Token: 0x06010F99 RID: 69529 RVA: 0x004E2B20 File Offset: 0x004E0D20
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.triggerRadius, "triggerRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.triggered, "triggered", false, false);
		}

		// Token: 0x0400B08D RID: 45197
		private float triggerRadius;

		// Token: 0x0400B08E RID: 45198
		private bool triggered;
	}
}
