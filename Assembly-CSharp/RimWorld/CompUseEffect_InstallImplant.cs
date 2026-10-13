using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002F63 RID: 12131
	public class CompUseEffect_InstallImplant : CompUseEffect
	{
		// Token: 0x17002C35 RID: 11317
		// (get) Token: 0x06011184 RID: 70020 RVA: 0x004E8951 File Offset: 0x004E6B51
		public CompProperties_UseEffectInstallImplant Props
		{
			get
			{
				return (CompProperties_UseEffectInstallImplant)this.props;
			}
		}

		// Token: 0x06011185 RID: 70021 RVA: 0x004E8960 File Offset: 0x004E6B60
		public override void DoEffect(Pawn user)
		{
			BodyPartRecord bodyPartRecord = user.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null);
			if (bodyPartRecord == null)
			{
				return;
			}
			Hediff firstHediffOfDef = user.health.hediffSet.GetFirstHediffOfDef(this.Props.hediffDef, false);
			if (firstHediffOfDef == null && !this.Props.requiresExistingHediff)
			{
				user.health.AddHediff(this.Props.hediffDef, bodyPartRecord, null, null);
				return;
			}
			if (this.Props.canUpgrade)
			{
				((Hediff_Level)firstHediffOfDef).ChangeLevel(1);
			}
		}

		// Token: 0x06011186 RID: 70022 RVA: 0x004E89FC File Offset: 0x004E6BFC
		public override AcceptanceReport CanBeUsedBy(Pawn p)
		{
			if ((!p.IsFreeColonist || p.HasExtraHomeFaction(null)) && !this.Props.allowNonColonists)
			{
				return "InstallImplantNotAllowedForNonColonists".Translate();
			}
			if (p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null) == null)
			{
				return "InstallImplantNoBodyPart".Translate() + ": " + this.Props.bodyPart.LabelShort;
			}
			if (this.Props.requiresPsychicallySensitive && p.psychicEntropy != null && !p.psychicEntropy.IsPsychicallySensitive)
			{
				return "InstallImplantPsychicallyDeaf".Translate();
			}
			Hediff existingImplant = this.GetExistingImplant(p);
			if (this.Props.requiresExistingHediff && existingImplant == null)
			{
				return "InstallImplantHediffRequired".Translate(this.Props.hediffDef.label);
			}
			if (existingImplant != null)
			{
				if (!this.Props.canUpgrade)
				{
					return "InstallImplantAlreadyInstalled".Translate();
				}
				Hediff_Level hediff_Level = (Hediff_Level)existingImplant;
				if ((float)hediff_Level.level >= hediff_Level.def.maxSeverity)
				{
					return "InstallImplantAlreadyMaxLevel".Translate();
				}
				if (this.Props.maxSeverity <= (float)hediff_Level.level)
				{
					return "InstallImplantAlreadyMaxLevel".Translate() + " " + this.Props.maxSeverity.ToString();
				}
				if (this.Props.minSeverity > (float)hediff_Level.level)
				{
					return "InstallImplantMinLevel".Translate(this.Props.minSeverity);
				}
			}
			return true;
		}

		// Token: 0x06011187 RID: 70023 RVA: 0x004E8BCC File Offset: 0x004E6DCC
		public Hediff GetExistingImplant(Pawn p)
		{
			for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
			{
				Hediff hediff = p.health.hediffSet.hediffs[i];
				if (hediff.def == this.Props.hediffDef && hediff.Part == p.RaceProps.body.GetPartsWithDef(this.Props.bodyPart).FirstOrFallback(null))
				{
					return hediff;
				}
			}
			return null;
		}
	}
}
