using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000211 RID: 529
	public class BodyPartDef : Def
	{
		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000E14 RID: 3604 RVA: 0x00042A27 File Offset: 0x00040C27
		public bool IsSolidInDefinition_Debug
		{
			get
			{
				return this.solid;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000E15 RID: 3605 RVA: 0x00042A2F File Offset: 0x00040C2F
		public bool IsSkinCoveredInDefinition_Debug
		{
			get
			{
				return this.skinCovered;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000E16 RID: 3606 RVA: 0x00042A37 File Offset: 0x00040C37
		public bool IsMirroredPart
		{
			get
			{
				return this.tags.Contains(BodyPartTagDefOf.Mirrored);
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000E17 RID: 3607 RVA: 0x00042A49 File Offset: 0x00040C49
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000E18 RID: 3608 RVA: 0x00042A65 File Offset: 0x00040C65
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00042A9F File Offset: 0x00040C9F
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.frostbiteVulnerability > 10f)
			{
				yield return "frostbitePriority > max 10: " + this.frostbiteVulnerability.ToString();
			}
			if (this.solid && this.bleedRate > 0f)
			{
				yield return "solid but bleedRate is not zero";
			}
			yield break;
			yield break;
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00042AB0 File Offset: 0x00040CB0
		public bool IsSolid(BodyPartRecord part, List<Hediff> hediffs)
		{
			for (BodyPartRecord bodyPartRecord = part; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
			{
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i].Part == bodyPartRecord && hediffs[i] is Hediff_AddedPart)
					{
						return hediffs[i].def.addedPartProps.solid && !hediffs[i].def.organicAddedBodypart;
					}
				}
			}
			return this.solid;
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00042B2E File Offset: 0x00040D2E
		public bool IsSkinCovered(BodyPartRecord part, HediffSet body)
		{
			return !body.PartOrAnyAncestorHasDirectlyAddedParts(part) && this.skinCovered;
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00042B41 File Offset: 0x00040D41
		public float GetMaxHealth(Pawn pawn)
		{
			return (float)Mathf.CeilToInt((float)this.hitPoints * pawn.HealthScale);
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00042B58 File Offset: 0x00040D58
		public float GetHitChanceFactorFor(DamageDef damage)
		{
			if (this.conceptual)
			{
				return 0f;
			}
			if (this.hitChanceFactors == null)
			{
				return 1f;
			}
			float result;
			if (this.hitChanceFactors.TryGetValue(damage, out result))
			{
				return result;
			}
			return 1f;
		}

		// Token: 0x04000861 RID: 2145
		[MustTranslate]
		public string labelShort;

		// Token: 0x04000862 RID: 2146
		public List<BodyPartTagDef> tags = new List<BodyPartTagDef>();

		// Token: 0x04000863 RID: 2147
		public int hitPoints = 10;

		// Token: 0x04000864 RID: 2148
		public float permanentInjuryChanceFactor = 1f;

		// Token: 0x04000865 RID: 2149
		public float bleedRate = 1f;

		// Token: 0x04000866 RID: 2150
		public float frostbiteVulnerability;

		// Token: 0x04000867 RID: 2151
		private bool skinCovered;

		// Token: 0x04000868 RID: 2152
		private bool solid;

		// Token: 0x04000869 RID: 2153
		public bool alive = true;

		// Token: 0x0400086A RID: 2154
		public bool delicate;

		// Token: 0x0400086B RID: 2155
		public bool canScarify;

		// Token: 0x0400086C RID: 2156
		public bool beautyRelated;

		// Token: 0x0400086D RID: 2157
		public bool conceptual;

		// Token: 0x0400086E RID: 2158
		public bool socketed;

		// Token: 0x0400086F RID: 2159
		public ThingDef spawnThingOnRemoved;

		// Token: 0x04000870 RID: 2160
		public bool pawnGeneratorCanAmputate;

		// Token: 0x04000871 RID: 2161
		public bool canSuggestAmputation = true;

		// Token: 0x04000872 RID: 2162
		public bool forceAlwaysRemovable;

		// Token: 0x04000873 RID: 2163
		public Dictionary<DamageDef, float> hitChanceFactors;

		// Token: 0x04000874 RID: 2164
		public bool destroyableByDamage = true;

		// Token: 0x04000875 RID: 2165
		public bool canBeVacuumBurnt = true;

		// Token: 0x04000876 RID: 2166
		[MustTranslate]
		public string removeRecipeLabelOverride;

		// Token: 0x04000877 RID: 2167
		public float executionPartPriority;

		// Token: 0x04000878 RID: 2168
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
