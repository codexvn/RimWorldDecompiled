using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200071C RID: 1820
	[StaticConstructorOnStartup]
	public class HediffComp_Immunizable : HediffComp_SeverityModifierBase
	{
		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x06002C5E RID: 11358 RVA: 0x000E627C File Offset: 0x000E447C
		public HediffCompProperties_Immunizable Props
		{
			get
			{
				return (HediffCompProperties_Immunizable)this.props;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x06002C5F RID: 11359 RVA: 0x000E6289 File Offset: 0x000E4489
		public override string CompLabelInBracketsExtra
		{
			get
			{
				if (!this.Hidden && this.FullyImmune)
				{
					return "DevelopedImmunityLower".Translate();
				}
				return null;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x06002C60 RID: 11360 RVA: 0x000E62AC File Offset: 0x000E44AC
		public override string CompTipStringExtra
		{
			get
			{
				if (!this.Hidden && base.Def.PossibleToDevelopImmunityNaturally() && !this.FullyImmune)
				{
					return "Immunity".Translate() + ": " + this.NaturalImmunity.ToStringPercent("0.#");
				}
				return null;
			}
		}

		// Token: 0x170007E7 RID: 2023
		// (get) Token: 0x06002C61 RID: 11361 RVA: 0x000E6306 File Offset: 0x000E4506
		public float NaturalImmunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, true);
			}
		}

		// Token: 0x170007E8 RID: 2024
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x000E6324 File Offset: 0x000E4524
		public float Immunity
		{
			get
			{
				return base.Pawn.health.immunity.GetImmunity(base.Def, false);
			}
		}

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x06002C63 RID: 11363 RVA: 0x000E6342 File Offset: 0x000E4542
		public bool FullyImmune
		{
			get
			{
				return this.Immunity >= 1f;
			}
		}

		// Token: 0x170007EA RID: 2026
		// (get) Token: 0x06002C64 RID: 11364 RVA: 0x000E6354 File Offset: 0x000E4554
		public override TextureAndColor CompStateIcon
		{
			get
			{
				if (this.FullyImmune)
				{
					return HediffComp_Immunizable.IconImmune;
				}
				return TextureAndColor.None;
			}
		}

		// Token: 0x170007EB RID: 2027
		// (get) Token: 0x06002C65 RID: 11365 RVA: 0x000E636E File Offset: 0x000E456E
		private bool Hidden
		{
			get
			{
				return (!Prefs.DevMode || !DebugSettings.godMode) && this.Props.hidden;
			}
		}

		// Token: 0x170007EC RID: 2028
		// (get) Token: 0x06002C66 RID: 11366 RVA: 0x000E638C File Offset: 0x000E458C
		private float SeverityFactorFromHediffs
		{
			get
			{
				float num = 1f;
				if (!this.Props.severityFactorsFromHediffs.NullOrEmpty<HediffDefFactor>())
				{
					for (int i = 0; i < this.Props.severityFactorsFromHediffs.Count; i++)
					{
						if (base.Pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.severityFactorsFromHediffs[i].HediffDef, false) != null)
						{
							num *= this.Props.severityFactorsFromHediffs[i].Factor;
						}
					}
				}
				return num;
			}
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x000E6414 File Offset: 0x000E4614
		public override void CopyFrom(HediffComp other)
		{
			HediffComp_Immunizable hediffComp_Immunizable = other as HediffComp_Immunizable;
			if (hediffComp_Immunizable != null)
			{
				Pawn pawn = this.parent.pawn;
				pawn.health.immunity.TryAddImmunityRecord(this.parent.def, this.parent.def);
				ImmunityRecord immunityRecord = pawn.health.immunity.GetImmunityRecord(this.parent.def);
				if (immunityRecord != null)
				{
					immunityRecord.immunity = hediffComp_Immunizable.Immunity;
				}
			}
		}

		// Token: 0x06002C68 RID: 11368 RVA: 0x000E6486 File Offset: 0x000E4686
		public override void CompPostPostAdd(DamageInfo? dinfo)
		{
			base.CompPostPostAdd(dinfo);
			this.severityPerDayNotImmuneRandomFactor = this.Props.severityPerDayNotImmuneRandomFactor.RandomInRange;
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x000E64A5 File Offset: 0x000E46A5
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Values.Look<float>(ref this.severityPerDayNotImmuneRandomFactor, "severityPerDayNotImmuneRandomFactor", 1f, false);
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x000E64C3 File Offset: 0x000E46C3
		public override float SeverityChangePerDay()
		{
			return (this.FullyImmune ? this.Props.severityPerDayImmune : (this.Props.severityPerDayNotImmune * this.severityPerDayNotImmuneRandomFactor)) * this.SeverityFactorFromHediffs;
		}

		// Token: 0x06002C6B RID: 11371 RVA: 0x000E64F4 File Offset: 0x000E46F4
		public override string CompDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.CompDebugString());
			if (this.severityPerDayNotImmuneRandomFactor != 1f)
			{
				stringBuilder.AppendLine("severityPerDayNotImmuneRandomFactor: " + this.severityPerDayNotImmuneRandomFactor.ToString("0.##"));
			}
			if (!base.Pawn.Dead)
			{
				ImmunityRecord immunityRecord = base.Pawn.health.immunity.GetImmunityRecord(base.Def);
				if (immunityRecord != null)
				{
					stringBuilder.AppendLine("immunity change per day: " + (immunityRecord.ImmunityChangePerTick(base.Pawn, true, this.parent) * 60000f).ToString("F3"));
					stringBuilder.AppendLine("pawn immunity gain speed: " + StatDefOf.ImmunityGainSpeed.ValueToString(base.Pawn.GetStatValue(StatDefOf.ImmunityGainSpeed, true, -1), ToStringNumberSense.Absolute, true));
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002398 RID: 9112
		private float severityPerDayNotImmuneRandomFactor = 1f;

		// Token: 0x04002399 RID: 9113
		private static readonly Texture2D IconImmune = ContentFinder<Texture2D>.Get("UI/Icons/Medical/IconImmune", true);
	}
}
