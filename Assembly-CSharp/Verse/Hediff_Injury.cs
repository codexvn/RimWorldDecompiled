using System;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000749 RID: 1865
	public class Hediff_Injury : HediffWithComps
	{
		// Token: 0x1700082B RID: 2091
		// (get) Token: 0x06002D7D RID: 11645 RVA: 0x000EA618 File Offset: 0x000E8818
		public override int UIGroupKey
		{
			get
			{
				int num = base.UIGroupKey;
				if (this.IsTended())
				{
					num = Gen.HashCombineInt(num, 152235495);
				}
				return num;
			}
		}

		// Token: 0x1700082C RID: 2092
		// (get) Token: 0x06002D7E RID: 11646 RVA: 0x000EA644 File Offset: 0x000E8844
		public override string LabelBase
		{
			get
			{
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					if (base.Part != null && base.Part.def.delicate && !hediffComp_GetsPermanent.Props.instantlyPermanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.instantlyPermanentLabel;
					}
					if (!hediffComp_GetsPermanent.Props.permanentLabel.NullOrEmpty())
					{
						return hediffComp_GetsPermanent.Props.permanentLabel;
					}
				}
				return base.LabelBase;
			}
		}

		// Token: 0x1700082D RID: 2093
		// (get) Token: 0x06002D7F RID: 11647 RVA: 0x000EA6C0 File Offset: 0x000E88C0
		public override string LabelInBrackets
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.LabelInBrackets);
				if (this.sourceHediffDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.sourceHediffDef.label);
				}
				else if (this.sourceDef != null)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					if (!this.sourceToolLabel.NullOrEmpty())
					{
						stringBuilder.Append("SourceToolLabel".Translate(this.sourceLabel, this.sourceToolLabel));
					}
					else if (this.sourceBodyPartGroup != null)
					{
						stringBuilder.Append("SourceToolLabel".Translate(this.sourceLabel, this.sourceBodyPartGroup.LabelShort));
					}
					else
					{
						stringBuilder.Append(this.sourceLabel);
					}
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent && hediffComp_GetsPermanent.PainCategory != PainCategory.Painless)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(("PainCategory_" + hediffComp_GetsPermanent.PainCategory.ToString()).Translate());
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x1700082E RID: 2094
		// (get) Token: 0x06002D80 RID: 11648 RVA: 0x000EA818 File Offset: 0x000E8A18
		public override Color LabelColor
		{
			get
			{
				if (this.IsPermanent())
				{
					return Hediff_Injury.PermanentInjuryColor;
				}
				return Color.white;
			}
		}

		// Token: 0x1700082F RID: 2095
		// (get) Token: 0x06002D81 RID: 11649 RVA: 0x000EA830 File Offset: 0x000E8A30
		public override string SeverityLabel
		{
			get
			{
				if (this.Severity == 0f)
				{
					return null;
				}
				return this.Severity.ToString("F1");
			}
		}

		// Token: 0x17000830 RID: 2096
		// (get) Token: 0x06002D82 RID: 11650 RVA: 0x000EA85F File Offset: 0x000E8A5F
		public override float SummaryHealthPercentImpact
		{
			get
			{
				if (this.IsPermanent() || !this.Visible)
				{
					return 0f;
				}
				return this.Severity / (75f * this.pawn.HealthScale);
			}
		}

		// Token: 0x17000831 RID: 2097
		// (get) Token: 0x06002D83 RID: 11651 RVA: 0x000EA890 File Offset: 0x000E8A90
		public override float PainOffset
		{
			get
			{
				if (this.pawn.Dead || (base.Part != null && this.pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(base.Part)) || this.causesNoPain)
				{
					return 0f;
				}
				HediffComp_GetsPermanent hediffComp_GetsPermanent = this.TryGetComp<HediffComp_GetsPermanent>();
				float num;
				if (hediffComp_GetsPermanent != null && hediffComp_GetsPermanent.IsPermanent)
				{
					num = this.Severity * this.def.injuryProps.averagePainPerSeverityPermanent * hediffComp_GetsPermanent.PainFactor;
				}
				else
				{
					num = this.Severity * this.def.injuryProps.painPerSeverity;
				}
				return num / this.pawn.HealthScale;
			}
		}

		// Token: 0x17000832 RID: 2098
		// (get) Token: 0x06002D84 RID: 11652 RVA: 0x000EA938 File Offset: 0x000E8B38
		public override float BleedRate
		{
			get
			{
				if (this.pawn.Dead)
				{
					return 0f;
				}
				if (this.BleedingStoppedDueToAge)
				{
					return 0f;
				}
				if (!this.pawn.health.CanBleed)
				{
					return 0f;
				}
				if ((base.Part != null && base.Part.def.IsSolid(base.Part, this.pawn.health.hediffSet.hediffs)) || this.IsTended() || this.IsPermanent())
				{
					return 0f;
				}
				if (base.Part != null)
				{
					Hediff directlyAddedPartFor = this.pawn.health.hediffSet.GetDirectlyAddedPartFor(base.Part);
					if (directlyAddedPartFor != null && !directlyAddedPartFor.def.organicAddedBodypart)
					{
						return 0f;
					}
				}
				float num = this.Severity * this.def.injuryProps.bleedRate * this.pawn.RaceProps.bleedRateFactor;
				if (base.Part != null)
				{
					num *= base.Part.def.bleedRate;
				}
				return num;
			}
		}

		// Token: 0x17000833 RID: 2099
		// (get) Token: 0x06002D85 RID: 11653 RVA: 0x000EAA48 File Offset: 0x000E8C48
		private int AgeTicksToStopBleeding
		{
			get
			{
				int num = 90000;
				float t = Mathf.Clamp(Mathf.InverseLerp(1f, 30f, this.Severity), 0f, 1f);
				return num + Mathf.RoundToInt(Mathf.Lerp(0f, 90000f, t));
			}
		}

		// Token: 0x17000834 RID: 2100
		// (get) Token: 0x06002D86 RID: 11654 RVA: 0x000EAA95 File Offset: 0x000E8C95
		private bool BleedingStoppedDueToAge
		{
			get
			{
				return this.ageTicks >= this.AgeTicksToStopBleeding;
			}
		}

		// Token: 0x06002D87 RID: 11655 RVA: 0x000EAAA8 File Offset: 0x000E8CA8
		public override void TickInterval(int delta)
		{
			bool bleedingStoppedDueToAge = this.BleedingStoppedDueToAge;
			base.TickInterval(delta);
			bool bleedingStoppedDueToAge2 = this.BleedingStoppedDueToAge;
			if (bleedingStoppedDueToAge != bleedingStoppedDueToAge2)
			{
				this.pawn.health.Notify_HediffChanged(this);
			}
		}

		// Token: 0x06002D88 RID: 11656 RVA: 0x000EAAE0 File Offset: 0x000E8CE0
		public override void Heal(float amount)
		{
			this.Severity -= amount;
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPostInjuryHeal(amount);
				}
			}
			this.pawn.health.Notify_HediffChanged(this);
		}

		// Token: 0x06002D89 RID: 11657 RVA: 0x000EAB3C File Offset: 0x000E8D3C
		public override bool TryMergeWith(Hediff other)
		{
			Hediff_Injury hediff_Injury = other as Hediff_Injury;
			return hediff_Injury != null && hediff_Injury.def == this.def && hediff_Injury.Part == base.Part && !hediff_Injury.IsTended() && !hediff_Injury.IsPermanent() && !this.IsTended() && !this.IsPermanent() && this.def.injuryProps.canMerge && base.TryMergeWith(other);
		}

		// Token: 0x06002D8A RID: 11658 RVA: 0x000EABAC File Offset: 0x000E8DAC
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part != null && base.Part.coverageAbs <= 0f && (dinfo == null || dinfo.Value.Def != DamageDefOf.SurgicalCut))
			{
				string[] array = new string[6];
				array[0] = "Added injury to ";
				int num = 1;
				BodyPartDef def = base.Part.def;
				array[num] = ((def != null) ? def.ToString() : null);
				array[2] = " but it should be impossible to hit it. pawn=";
				array[3] = this.pawn.ToStringSafe<Pawn>();
				array[4] = " dinfo=";
				array[5] = dinfo.ToStringSafe<DamageInfo?>();
				Log.Error(string.Concat(array));
			}
		}

		// Token: 0x06002D8B RID: 11659 RVA: 0x000EAC55 File Offset: 0x000E8E55
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Drawer.renderer.WoundOverlays.ClearCache();
			PortraitsCache.SetDirty(this.pawn);
			GlobalTextureAtlasManager.TryMarkPawnFrameSetDirty(this.pawn);
		}

		// Token: 0x06002D8C RID: 11660 RVA: 0x000EAC90 File Offset: 0x000E8E90
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.destroysBodyParts, "destroysBodyParts", true, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error("Hediff_Injury has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Token: 0x04002408 RID: 9224
		public bool destroysBodyParts = true;

		// Token: 0x04002409 RID: 9225
		private static readonly Color PermanentInjuryColor = new Color(0.72f, 0.72f, 0.72f);
	}
}
