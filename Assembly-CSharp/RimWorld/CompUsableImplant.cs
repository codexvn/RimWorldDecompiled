using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EC2 RID: 11970
	public class CompUsableImplant : CompUsable
	{
		// Token: 0x06010E5C RID: 69212 RVA: 0x004DD858 File Offset: 0x004DBA58
		protected override string FloatMenuOptionLabel(Pawn pawn)
		{
			CompUseEffect_InstallImplant compUseEffect_InstallImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			if (compUseEffect_InstallImplant != null)
			{
				Hediff_Level hediff_Level = compUseEffect_InstallImplant.GetExistingImplant(pawn) as Hediff_Level;
				if (hediff_Level != null && compUseEffect_InstallImplant.Props.canUpgrade && (float)hediff_Level.level < hediff_Level.def.maxSeverity)
				{
					return "UpgradeImplant".Translate(hediff_Level.def.label, hediff_Level.level + 1);
				}
			}
			return base.FloatMenuOptionLabel(pawn);
		}

		// Token: 0x06010E5D RID: 69213 RVA: 0x004DD8DC File Offset: 0x004DBADC
		public override void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget, bool forced = false)
		{
			CompUseEffect_InstallImplant useEffectImplant = this.parent.TryGetComp<CompUseEffect_InstallImplant>();
			Hediff_Level hediff_Level = useEffectImplant.GetExistingImplant(pawn) as Hediff_Level;
			TaggedString text = CompRoyalImplant.CheckForViolations(pawn, useEffectImplant.Props.hediffDef, (hediff_Level != null && useEffectImplant.Props.canUpgrade) ? 1 : 0);
			if (!text.NullOrEmpty())
			{
				Find.WindowStack.Add(new Dialog_MessageBox(text, "Yes".Translate(), delegate()
				{
					this.UseJobInternal(pawn, extraTarget, useEffectImplant.Props.hediffDef);
				}, "No".Translate(), null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			this.UseJobInternal(pawn, extraTarget, useEffectImplant.Props.hediffDef);
		}

		// Token: 0x06010E5E RID: 69214 RVA: 0x004DD9D0 File Offset: 0x004DBBD0
		private void UseJobInternal(Pawn pawn, LocalTargetInfo extraTarget, HediffDef hediff)
		{
			base.TryStartUseJob(pawn, extraTarget, false);
			if (hediff == HediffDefOf.PsychicAmplifier && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) < 1E-45f)
			{
				Messages.Message("MessagePsylinkNoSensitivity".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.CautionInput, false);
			}
		}
	}
}
