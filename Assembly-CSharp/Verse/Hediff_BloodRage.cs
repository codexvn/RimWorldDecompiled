using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000738 RID: 1848
	public class Hediff_BloodRage : Hediff
	{
		// Token: 0x06002D05 RID: 11525 RVA: 0x000E8778 File Offset: 0x000E6978
		public override void PostAdd(DamageInfo? dinfo)
		{
			if (!ModLister.CheckAnomaly("Blood rage"))
			{
				this.pawn.health.RemoveHediff(this);
				return;
			}
			base.PostAdd(dinfo);
			if (this.pawn.RaceProps.Animal)
			{
				this.adjustedSeverityRaisePerTick = 8.8E-05f * Hediff_BloodRage.SeverityRaiseRangeAnimal.RandomInRange;
			}
			else if (this.pawn.RaceProps.Humanlike)
			{
				this.adjustedSeverityRaisePerTick = 8.8E-05f * Hediff_BloodRage.SeverityRaiseRange.RandomInRange;
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Wimp))
				{
					this.adjustedSeverityRaisePerTick *= 0.5f;
				}
				if (this.pawn.story.traits.HasTrait(TraitDefOf.Kind))
				{
					this.adjustedSeverityRaisePerTick *= 0.5f;
				}
			}
			this.adjustedSeverityRaisePerTick *= this.pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000E8880 File Offset: 0x000E6A80
		public override void TickInterval(int delta)
		{
			base.TickInterval(delta);
			if (this.Severity >= 1f)
			{
				if (this.pawn.RaceProps.Humanlike)
				{
					BloodRainUtility.TryTriggerBerserkShort(this.pawn);
				}
				if (this.pawn.RaceProps.Animal && this.pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterBloodRain, null, false, false, false, null, false, false, false) && MessagesRepeatAvoider.MessageShowAllowed("MessageManhunterBloodrain-" + this.pawn.LabelShort, 240f))
				{
					Messages.Message("Manhunter_BloodRain".Translate(this.pawn), this.pawn, MessageTypeDefOf.NeutralEvent, false);
				}
			}
			if (BloodRainUtility.ExposedToBloodRain(this.pawn))
			{
				this.Severity += this.adjustedSeverityRaisePerTick;
				return;
			}
			this.Severity -= 0.000132f;
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000E897C File Offset: 0x000E6B7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.adjustedSeverityRaisePerTick, "adjustedSeverityRaisePerTick", 0f, false);
		}

		// Token: 0x040023CE RID: 9166
		private float adjustedSeverityRaisePerTick;

		// Token: 0x040023CF RID: 9167
		private const float SeverityRaisePerTick = 8.8E-05f;

		// Token: 0x040023D0 RID: 9168
		private const float SeverityRaisePerTickAnimals = 8.8E-05f;

		// Token: 0x040023D1 RID: 9169
		private const float SeverityFallPerTick = 0.000132f;

		// Token: 0x040023D2 RID: 9170
		private static readonly FloatRange SeverityRaiseRange = new FloatRange(0.75f, 1.25f);

		// Token: 0x040023D3 RID: 9171
		private static readonly FloatRange SeverityRaiseRangeAnimal = new FloatRange(0.1f, 0.8f);
	}
}
