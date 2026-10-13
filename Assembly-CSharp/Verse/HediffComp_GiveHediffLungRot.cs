using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x020006BF RID: 1727
	public class HediffComp_GiveHediffLungRot : HediffComp
	{
		// Token: 0x1700078D RID: 1933
		// (get) Token: 0x06002B16 RID: 11030 RVA: 0x000E1A2D File Offset: 0x000DFC2D
		private HediffCompProperties_GiveHediffLungRot Props
		{
			get
			{
				return (HediffCompProperties_GiveHediffLungRot)this.props;
			}
		}

		// Token: 0x06002B17 RID: 11031 RVA: 0x000026EA File Offset: 0x000008EA
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
		}

		// Token: 0x06002B18 RID: 11032 RVA: 0x000E1A3C File Offset: 0x000DFC3C
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			Pawn pawn = this.parent.pawn;
			if (pawn.Spawned && this.parent.Severity >= this.Props.minSeverity && pawn.Position.AnyGas(pawn.Map, GasType.RotStink) && !pawn.health.hediffSet.HasHediff(HediffDefOf.LungRot, false) && pawn.IsHashIntervalTick(this.Props.mtbCheckDuration, delta) && !pawn.health.immunity.AnyGeneMakesFullyImmuneTo(HediffDefOf.LungRot) && Rand.MTBEventOccurs(this.Props.mtbOverRotGasExposureCurve.Evaluate(this.parent.Severity), 2500f, (float)this.Props.mtbCheckDuration))
			{
				IEnumerable<BodyPartRecord> lungRotAffectedBodyParts = GasUtility.GetLungRotAffectedBodyParts(pawn);
				if (lungRotAffectedBodyParts.Any<BodyPartRecord>())
				{
					foreach (BodyPartRecord part in lungRotAffectedBodyParts)
					{
						pawn.health.AddHediff(HediffDefOf.LungRot, part, null, null);
					}
					if (PawnUtility.ShouldSendNotificationAbout(pawn))
					{
						TaggedString label = "LetterLabelNewDisease".Translate() + ": " + HediffDefOf.LungRot.LabelCap;
						TaggedString text = "LetterTextLungRot".Translate(pawn.Named("PAWN"), HediffDefOf.LungRot.label, pawn.LabelDefinite()).AdjustedFor(pawn, "PAWN", true).CapitalizeFirst();
						Find.LetterStack.ReceiveLetter(label, text, LetterDefOf.NegativeEvent, pawn, null, null, null, null, 0, true);
					}
				}
			}
		}
	}
}
