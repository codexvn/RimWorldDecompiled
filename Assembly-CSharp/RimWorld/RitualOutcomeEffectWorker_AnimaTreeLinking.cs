using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002772 RID: 10098
	public class RitualOutcomeEffectWorker_AnimaTreeLinking : RitualOutcomeEffectWorker_FromQuality
	{
		// Token: 0x170021E5 RID: 8677
		// (get) Token: 0x0600DD7D RID: 56701 RVA: 0x00002501 File Offset: 0x00000701
		public override bool SupportsAttachableOutcomeEffect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DD7E RID: 56702 RVA: 0x00411501 File Offset: 0x0040F701
		public RitualOutcomeEffectWorker_AnimaTreeLinking()
		{
		}

		// Token: 0x0600DD7F RID: 56703 RVA: 0x00411509 File Offset: 0x0040F709
		public RitualOutcomeEffectWorker_AnimaTreeLinking(RitualOutcomeEffectDef def) : base(def)
		{
		}

		// Token: 0x0600DD80 RID: 56704 RVA: 0x00413400 File Offset: 0x00411600
		public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
		{
			float quality = base.GetQuality(jobRitual, progress);
			Pawn pawn = jobRitual.PawnWithRole("organizer");
			Thing thing = jobRitual.selectedTarget.Thing;
			object obj = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			int num = (int)RitualOutcomeEffectWorker_AnimaTreeLinking.RestoredGrassFromQuality.Evaluate(quality);
			object obj2 = obj;
			if (obj2 != null)
			{
				obj2.FinishLinkingRitual(pawn, num);
			}
			string text = "LetterTextLinkingRitualCompleted".Translate(pawn.Named("PAWN"), jobRitual.selectedTarget.Thing.Named("LINKABLE"));
			if (num > 0)
			{
				text += " " + "LetterTextLinkingRitualCompletedAnimaGrass".Translate(num);
			}
			text = text + "\n\n" + this.OutcomeQualityBreakdownDesc(quality, progress, jobRitual);
			Find.LetterStack.ReceiveLetter("LetterLabelLinkingRitualCompleted".Translate(), text, LetterDefOf.RitualOutcomePositive, new LookTargets(new TargetInfo[]
			{
				pawn,
				jobRitual.selectedTarget.Thing
			}), null, null, null, null, 0, true);
		}

		// Token: 0x0400954A RID: 38218
		public static readonly SimpleCurve RestoredGrassFromQuality = new SimpleCurve
		{
			{
				new CurvePoint(0.2f, 0f),
				true
			},
			{
				new CurvePoint(0.4f, 1f),
				true
			},
			{
				new CurvePoint(0.6f, 3f),
				true
			},
			{
				new CurvePoint(0.8f, 5f),
				true
			},
			{
				new CurvePoint(1f, 8f),
				true
			}
		};
	}
}
