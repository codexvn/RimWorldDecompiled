using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002EB9 RID: 11961
	public class CompUniqueWeapon : ThingComp
	{
		// Token: 0x17002B7E RID: 11134
		// (get) Token: 0x06010E02 RID: 69122 RVA: 0x004DBFAD File Offset: 0x004DA1AD
		public CompProperties_UniqueWeapon Props
		{
			get
			{
				return (CompProperties_UniqueWeapon)this.props;
			}
		}

		// Token: 0x17002B7F RID: 11135
		// (get) Token: 0x06010E03 RID: 69123 RVA: 0x004DBFBA File Offset: 0x004DA1BA
		public List<WeaponTraitDef> TraitsListForReading
		{
			get
			{
				return this.traits;
			}
		}

		// Token: 0x17002B80 RID: 11136
		// (get) Token: 0x06010E04 RID: 69124 RVA: 0x004DBFC4 File Offset: 0x004DA1C4
		public bool IgnoreAccuracyMaluses
		{
			get
			{
				if (this.ignoreAccuracyMaluses == null)
				{
					this.ignoreAccuracyMaluses = new bool?(false);
					using (List<WeaponTraitDef>.Enumerator enumerator = this.TraitsListForReading.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.ignoresAccuracyMaluses)
							{
								this.ignoreAccuracyMaluses = new bool?(true);
								break;
							}
						}
					}
				}
				return this.ignoreAccuracyMaluses.Value;
			}
		}

		// Token: 0x06010E05 RID: 69125 RVA: 0x004DC048 File Offset: 0x004DA248
		public override void PostExposeData()
		{
			Scribe_Collections.Look<WeaponTraitDef>(ref this.traits, "traits", LookMode.Def, Array.Empty<object>());
			Scribe_Defs.Look<ColorDef>(ref this.color, "color");
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.traits == null)
				{
					this.traits = new List<WeaponTraitDef>();
				}
				this.Setup(true);
			}
		}

		// Token: 0x06010E06 RID: 69126 RVA: 0x004DC0AF File Offset: 0x004DA2AF
		public override string TransformLabel(string label)
		{
			CompStyleable compStyleable = this.parent.compStyleable;
			bool flag;
			if (compStyleable == null)
			{
				flag = (null != null);
			}
			else
			{
				Precept_ThingStyle sourcePrecept = compStyleable.SourcePrecept;
				flag = (((sourcePrecept != null) ? sourcePrecept.Label : null) != null);
			}
			if (flag)
			{
				return label;
			}
			if (!this.name.NullOrEmpty())
			{
				return this.name;
			}
			return label;
		}

		// Token: 0x06010E07 RID: 69127 RVA: 0x004DC0F0 File Offset: 0x004DA2F0
		public override void PostPostMake()
		{
			if (!ModLister.CheckOdyssey("Unique Weapons"))
			{
				return;
			}
			this.InitializeTraits();
			CompQuality compQuality;
			if (this.parent.TryGetComp(out compQuality))
			{
				compQuality.SetQuality(QualityUtility.GenerateQuality(QualityGenerator.Super), new ArtGenerationContext?(ArtGenerationContext.Outsider));
			}
			List<string> list = new List<string>();
			this.color = (from c in DefDatabase<ColorDef>.AllDefs
			where c.colorType == ColorType.Weapon && c.randomlyPickable
			select c).RandomElement<ColorDef>();
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				if (weaponTraitDef.forcedColor != null)
				{
					this.color = weaponTraitDef.forcedColor;
				}
				if (weaponTraitDef.traitAdjectives != null)
				{
					list.AddRange(weaponTraitDef.traitAdjectives);
				}
			}
			GrammarRequest request = default(GrammarRequest);
			request.Rules.Add(new Rule_String("weapon_type", this.Props.namerLabels.RandomElement<string>()));
			request.Rules.Add(new Rule_String("color", this.color.label));
			request.Rules.Add(new Rule_String("trait_adjective", list.RandomElement<string>()));
			foreach (Rule item in TaleData_Pawn.GenerateRandom(true).GetRules("ANYPAWN", request.Constants))
			{
				request.Rules.Add(item);
			}
			request.Includes.Add(RulePackDefOf.NamerUniqueWeapon);
			this.name = NameGenerator.GenerateName(request, null, false, "r_weapon_name", null).StripTags();
			CompArt compArt;
			if (this.parent.TryGetComp(out compArt))
			{
				compArt.Title = this.name;
			}
		}

		// Token: 0x06010E08 RID: 69128 RVA: 0x004DC2E4 File Offset: 0x004DA4E4
		private void InitializeTraits()
		{
			IEnumerable<WeaponTraitDef> allDefs = DefDatabase<WeaponTraitDef>.AllDefs;
			if (this.traits == null)
			{
				this.traits = new List<WeaponTraitDef>();
			}
			Map mapBeingGenerated = MapGenerator.mapBeingGenerated;
			using (new RandBlock((mapBeingGenerated != null) ? mapBeingGenerated.NextGenSeed : this.parent.HashOffset()))
			{
				int randomInRange = CompUniqueWeapon.NumTraitsRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					IEnumerable<WeaponTraitDef> source = allDefs.Where(new Func<WeaponTraitDef, bool>(this.CanAddTrait));
					if (source.Any<WeaponTraitDef>())
					{
						this.AddTrait(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
					}
				}
				this.Setup(false);
			}
		}

		// Token: 0x06010E09 RID: 69129 RVA: 0x004DC3BC File Offset: 0x004DA5BC
		public bool CanAddTrait(WeaponTraitDef trait)
		{
			if (!ModLister.CheckOdyssey("Unique Weapons"))
			{
				return false;
			}
			if (!this.Props.weaponCategories.Contains(trait.weaponCategory))
			{
				return false;
			}
			if (this.TraitsListForReading.Empty<WeaponTraitDef>() && !trait.canGenerateAlone)
			{
				return false;
			}
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				foreach (WeaponTraitDef other in this.traits)
				{
					if (trait.Overlaps(other))
					{
						return false;
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06010E0A RID: 69130 RVA: 0x004DC464 File Offset: 0x004DA664
		public void AddTrait(WeaponTraitDef traitDef)
		{
			if (!ModLister.CheckOdyssey("Unique Weapons"))
			{
				return;
			}
			this.traits.Add(traitDef);
		}

		// Token: 0x06010E0B RID: 69131 RVA: 0x004DC480 File Offset: 0x004DA680
		public void Setup(bool fromSave)
		{
			foreach (WeaponTraitDef weaponTraitDef in this.traits)
			{
				if (weaponTraitDef.abilityProps != null)
				{
					CompEquippableAbilityReloadable compEquippableAbilityReloadable = this.parent.TryGetComp<CompEquippableAbilityReloadable>();
					compEquippableAbilityReloadable.props = weaponTraitDef.abilityProps;
					if (!fromSave)
					{
						compEquippableAbilityReloadable.Notify_PropsChanged();
					}
				}
			}
		}

		// Token: 0x06010E0C RID: 69132 RVA: 0x004DC4F8 File Offset: 0x004DA6F8
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.CheckOdyssey("Unique weapon"))
			{
				return;
			}
			foreach (WeaponTraitDef weaponTraitDef in this.traits)
			{
				weaponTraitDef.Worker.Notify_Equipped(pawn);
			}
		}

		// Token: 0x06010E0D RID: 69133 RVA: 0x004DC55C File Offset: 0x004DA75C
		public void Notify_EquipmentLost(Pawn pawn)
		{
			foreach (WeaponTraitDef weaponTraitDef in this.traits)
			{
				weaponTraitDef.Worker.Notify_EquipmentLost(pawn);
			}
		}

		// Token: 0x06010E0E RID: 69134 RVA: 0x004DC5B4 File Offset: 0x004DA7B4
		public override float GetStatOffset(StatDef stat)
		{
			float num = 0f;
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				num += weaponTraitDef.statOffsets.GetStatOffsetFromList(stat);
			}
			return num;
		}

		// Token: 0x06010E0F RID: 69135 RVA: 0x004DC618 File Offset: 0x004DA818
		public override float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				num *= weaponTraitDef.statFactors.GetStatFactorFromList(stat);
			}
			return num;
		}

		// Token: 0x06010E10 RID: 69136 RVA: 0x004DC67C File Offset: 0x004DA87C
		public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (WeaponTraitDef weaponTraitDef in this.TraitsListForReading)
			{
				float statOffsetFromList = weaponTraitDef.statOffsets.GetStatOffsetFromList(stat);
				if (!Mathf.Approximately(statOffsetFromList, 0f))
				{
					stringBuilder.AppendLine(whitespace + "    " + weaponTraitDef.LabelCap + ": " + stat.Worker.ValueToString(statOffsetFromList, false, ToStringNumberSense.Offset));
				}
				float statFactorFromList = weaponTraitDef.statFactors.GetStatFactorFromList(stat);
				if (!Mathf.Approximately(statFactorFromList, 1f))
				{
					stringBuilder.AppendLine(whitespace + "    " + weaponTraitDef.LabelCap + ": " + stat.Worker.ValueToString(statFactorFromList, false, ToStringNumberSense.Factor));
				}
			}
			if (stringBuilder.Length == 0)
			{
				return;
			}
			sb.AppendLine(whitespace + "StatsReport_WeaponTraits".Translate() + ":");
			sb.Append(stringBuilder.ToString());
		}

		// Token: 0x06010E11 RID: 69137 RVA: 0x004DC7C8 File Offset: 0x004DA9C8
		public override Color? ForceColor()
		{
			ColorDef colorDef = this.color;
			if (colorDef == null)
			{
				return null;
			}
			return new Color?(colorDef.color);
		}

		// Token: 0x06010E12 RID: 69138 RVA: 0x004DC7F4 File Offset: 0x004DA9F4
		public override string CompInspectStringExtra()
		{
			if (this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				return null;
			}
			return "WeaponTraits".Translate() + ": " + (from x in this.traits
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x06010E13 RID: 69139 RVA: 0x004DC864 File Offset: 0x004DAA64
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (!this.traits.NullOrEmpty<WeaponTraitDef>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_ThingUniqueWeaponTrait_Desc".Translate());
				stringBuilder.AppendLine();
				for (int i = 0; i < this.traits.Count; i++)
				{
					WeaponTraitDef weaponTraitDef = this.traits[i];
					stringBuilder.AppendLine(weaponTraitDef.LabelCap.Colorize(ColorLibrary.Yellow));
					stringBuilder.AppendLine(weaponTraitDef.description);
					if (!weaponTraitDef.statOffsets.NullOrEmpty<StatModifier>())
					{
						stringBuilder.Append((from x in weaponTraitDef.statOffsets
						select string.Format("{0} {1}", x.stat.LabelCap, x.stat.Worker.ValueToString(x.value, false, ToStringNumberSense.Offset))).ToLineList(" - ", false));
						stringBuilder.AppendLine();
					}
					if (!weaponTraitDef.statFactors.NullOrEmpty<StatModifier>())
					{
						stringBuilder.Append((from x in weaponTraitDef.statFactors
						select string.Format("{0} {1}", x.stat.LabelCap, x.stat.Worker.ValueToString(x.value, false, ToStringNumberSense.Factor))).ToLineList(" - ", false));
						stringBuilder.AppendLine();
					}
					if (!Mathf.Approximately(weaponTraitDef.burstShotCountMultiplier, 1f))
					{
						stringBuilder.AppendLine(string.Format(" - {0} {1}", "StatsReport_BurstShotCountMultiplier".Translate(), weaponTraitDef.burstShotCountMultiplier.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor)));
					}
					if (!Mathf.Approximately(weaponTraitDef.burstShotSpeedMultiplier, 1f))
					{
						stringBuilder.AppendLine(string.Format(" - {0} {1}", "StatsReport_BurstShotSpeedMultiplier".Translate(), weaponTraitDef.burstShotSpeedMultiplier.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor)));
					}
					if (!Mathf.Approximately(weaponTraitDef.additionalStoppingPower, 0f))
					{
						stringBuilder.AppendLine(string.Format(" - {0} {1}", "StatsReport_AdditionalStoppingPower".Translate(), weaponTraitDef.additionalStoppingPower.ToStringByStyle(ToStringStyle.FloatOne, ToStringNumberSense.Offset)));
					}
					if (i < this.traits.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				yield return new StatDrawEntry(this.parent.def.IsMeleeWeapon ? StatCategoryDefOf.Weapon_Melee : StatCategoryDefOf.Weapon_Ranged, "Stat_ThingUniqueWeaponTrait_Label".Translate(), (from x in this.traits
				select x.label).ToCommaList(false, false).CapitalizeFirst(), stringBuilder.ToString(), 1104, null, null, false, false);
			}
			yield break;
		}

		// Token: 0x06010E14 RID: 69140 RVA: 0x004DC874 File Offset: 0x004DAA74
		public override string CompTipStringExtra()
		{
			return "WeaponTraits".Translate() + ": " + (from x in this.TraitsListForReading
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0400AFA9 RID: 44969
		private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

		// Token: 0x0400AFAA RID: 44970
		private ColorDef color;

		// Token: 0x0400AFAB RID: 44971
		private string name;

		// Token: 0x0400AFAC RID: 44972
		private CompStyleable styleable;

		// Token: 0x0400AFAD RID: 44973
		private static readonly IntRange NumTraitsRange = new IntRange(1, 3);

		// Token: 0x0400AFAE RID: 44974
		private bool? ignoreAccuracyMaluses;
	}
}
