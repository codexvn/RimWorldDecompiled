using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200072A RID: 1834
	public class HediffComp_SurgeryInspectableMetalhorror : HediffComp_SurgeryInspectable
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002C99 RID: 11417 RVA: 0x000E6E6C File Offset: 0x000E506C
		public new HediffCompProperties_SurgeryInspectableMetalHorror Props
		{
			get
			{
				return (HediffCompProperties_SurgeryInspectableMetalHorror)this.props;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002C9A RID: 11418 RVA: 0x000E6E79 File Offset: 0x000E5079
		public Hediff_MetalhorrorImplant Implant
		{
			get
			{
				return (Hediff_MetalhorrorImplant)this.parent;
			}
		}

		// Token: 0x06002C9B RID: 11419 RVA: 0x000E6E88 File Offset: 0x000E5088
		public override SurgicalInspectionOutcome DoSurgicalInspection(Pawn surgeon)
		{
			AnalysisDetails analysisDetails;
			if (!Find.AnalysisManager.TryGetAnalysisProgress(this.Implant.Biosignature, out analysisDetails) || !analysisDetails.Satisfied)
			{
				return SurgicalInspectionOutcome.Nothing;
			}
			Hediff_MetalhorrorImplant hediff_MetalhorrorImplant;
			if (MetalhorrorUtility.IsInfected(surgeon, out hediff_MetalhorrorImplant))
			{
				hediff_MetalhorrorImplant.LiedAboutInspection = true;
				return SurgicalInspectionOutcome.Nothing;
			}
			this.Emerge(surgeon);
			return SurgicalInspectionOutcome.DetectedNoLetter;
		}

		// Token: 0x06002C9C RID: 11420 RVA: 0x000E6ED3 File Offset: 0x000E50D3
		public override void DoSurgicalInspectionVisible(Pawn surgeon)
		{
			this.Emerge(surgeon);
		}

		// Token: 0x06002C9D RID: 11421 RVA: 0x000E6EDC File Offset: 0x000E50DC
		private void Emerge(Pawn surgeon)
		{
			this.Implant.Emerge("MetalhorrorReasonSurgeryInspected".Translate(base.Pawn.Named("INFECTED"), surgeon.Named("PAWN")), false);
		}
	}
}
