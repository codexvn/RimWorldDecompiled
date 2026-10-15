using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class CompUniqueWeapon : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__26 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompUniqueWeapon _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CSpecialDisplayStats_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			CompUniqueWeapon compUniqueWeapon = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (compUniqueWeapon.traits.NullOrEmpty())
				{
					break;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Stat_ThingUniqueWeaponTrait_Desc".Translate());
				stringBuilder.AppendLine();
				for (int i = 0; i < compUniqueWeapon.traits.Count; i++)
				{
					WeaponTraitDef weaponTraitDef = compUniqueWeapon.traits[i];
					stringBuilder.AppendLine(weaponTraitDef.LabelCap.Colorize(ColorLibrary.Yellow));
					stringBuilder.AppendLine(weaponTraitDef.description);
					if (!weaponTraitDef.statOffsets.NullOrEmpty())
					{
						stringBuilder.Append(weaponTraitDef.statOffsets.Select((StatModifier x) => $"{x.stat.LabelCap} {x.stat.Worker.ValueToString(x.value, finalized: false, ToStringNumberSense.Offset)}").ToLineList(" - "));
						stringBuilder.AppendLine();
					}
					if (!weaponTraitDef.statFactors.NullOrEmpty())
					{
						stringBuilder.Append(weaponTraitDef.statFactors.Select((StatModifier x) => $"{x.stat.LabelCap} {x.stat.Worker.ValueToString(x.value, finalized: false, ToStringNumberSense.Factor)}").ToLineList(" - "));
						stringBuilder.AppendLine();
					}
					if (!Mathf.Approximately(weaponTraitDef.burstShotCountMultiplier, 1f))
					{
						stringBuilder.AppendLine($" - {"StatsReport_BurstShotCountMultiplier".Translate()} {weaponTraitDef.burstShotCountMultiplier.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor)}");
					}
					if (!Mathf.Approximately(weaponTraitDef.burstShotSpeedMultiplier, 1f))
					{
						stringBuilder.AppendLine($" - {"StatsReport_BurstShotSpeedMultiplier".Translate()} {weaponTraitDef.burstShotSpeedMultiplier.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor)}");
					}
					if (!Mathf.Approximately(weaponTraitDef.additionalStoppingPower, 0f))
					{
						stringBuilder.AppendLine($" - {"StatsReport_AdditionalStoppingPower".Translate()} {weaponTraitDef.additionalStoppingPower.ToStringByStyle(ToStringStyle.FloatOne, ToStringNumberSense.Offset)}");
					}
					if (i < compUniqueWeapon.traits.Count - 1)
					{
						stringBuilder.AppendLine();
					}
				}
				_003C_003E2__current = new StatDrawEntry(compUniqueWeapon.parent.def.IsMeleeWeapon ? StatCategoryDefOf.Weapon_Melee : StatCategoryDefOf.Weapon_Ranged, "Stat_ThingUniqueWeaponTrait_Label".Translate(), compUniqueWeapon.traits.Select((WeaponTraitDef x) => x.label).ToCommaList().CapitalizeFirst(), stringBuilder.ToString(), 1104);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__26 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__26(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	private List<WeaponTraitDef> traits = new List<WeaponTraitDef>();

	private ColorDef color;

	private string name;

	private CompStyleable styleable;

	private static readonly IntRange NumTraitsRange = new IntRange(1, 3);

	private bool? ignoreAccuracyMaluses;

	public CompProperties_UniqueWeapon Props => (CompProperties_UniqueWeapon)props;

	public List<WeaponTraitDef> TraitsListForReading => traits;

	public bool IgnoreAccuracyMaluses
	{
		get
		{
			if (!ignoreAccuracyMaluses.HasValue)
			{
				ignoreAccuracyMaluses = false;
				foreach (WeaponTraitDef item in TraitsListForReading)
				{
					if (item.ignoresAccuracyMaluses)
					{
						ignoreAccuracyMaluses = true;
						break;
					}
				}
			}
			return ignoreAccuracyMaluses.Value;
		}
	}

	public override void PostExposeData()
	{
		Scribe_Collections.Look(ref traits, "traits", LookMode.Def);
		Scribe_Defs.Look(ref color, "color");
		Scribe_Values.Look(ref name, "name");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (traits == null)
			{
				traits = new List<WeaponTraitDef>();
			}
			Setup(fromSave: true);
		}
	}

	public override string TransformLabel(string label)
	{
		if (parent.compStyleable?.SourcePrecept?.Label != null)
		{
			return label;
		}
		if (!name.NullOrEmpty())
		{
			return name;
		}
		return label;
	}

	public override void PostPostMake()
	{
		if (!ModLister.CheckOdyssey("Unique Weapons"))
		{
			return;
		}
		InitializeTraits();
		if (parent.TryGetComp<CompQuality>(out var comp))
		{
			comp.SetQuality(QualityUtility.GenerateQuality(QualityGenerator.Super), ArtGenerationContext.Outsider);
		}
		List<string> list = new List<string>();
		color = DefDatabase<ColorDef>.AllDefs.Where((ColorDef c) => c.colorType == ColorType.Weapon && c.randomlyPickable).RandomElement();
		foreach (WeaponTraitDef item in TraitsListForReading)
		{
			if (item.forcedColor != null)
			{
				color = item.forcedColor;
			}
			if (item.traitAdjectives != null)
			{
				list.AddRange(item.traitAdjectives);
			}
		}
		GrammarRequest request = default(GrammarRequest);
		request.Rules.Add(new Rule_String("weapon_type", Props.namerLabels.RandomElement()));
		request.Rules.Add(new Rule_String("color", color.label));
		request.Rules.Add(new Rule_String("trait_adjective", list.RandomElement()));
		foreach (Rule rule in TaleData_Pawn.GenerateRandom(humanLike: true).GetRules("ANYPAWN", request.Constants))
		{
			request.Rules.Add(rule);
		}
		request.Includes.Add(RulePackDefOf.NamerUniqueWeapon);
		name = NameGenerator.GenerateName(request, null, appendNumberIfNameUsed: false, "r_weapon_name").StripTags();
		if (parent.TryGetComp<CompArt>(out var comp2))
		{
			comp2.Title = name;
		}
	}

	private void InitializeTraits()
	{
		IEnumerable<WeaponTraitDef> allDefs = DefDatabase<WeaponTraitDef>.AllDefs;
		if (traits == null)
		{
			traits = new List<WeaponTraitDef>();
		}
		using (new RandBlock(MapGenerator.mapBeingGenerated?.NextGenSeed ?? parent.HashOffset()))
		{
			int randomInRange = NumTraitsRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				IEnumerable<WeaponTraitDef> source = allDefs.Where(CanAddTrait);
				if (source.Any())
				{
					AddTrait(source.RandomElementByWeight((WeaponTraitDef x) => x.commonality));
				}
			}
			Setup(fromSave: false);
		}
	}

	public bool CanAddTrait(WeaponTraitDef trait)
	{
		if (!ModLister.CheckOdyssey("Unique Weapons"))
		{
			return false;
		}
		if (!Props.weaponCategories.Contains(trait.weaponCategory))
		{
			return false;
		}
		if (TraitsListForReading.Empty() && !trait.canGenerateAlone)
		{
			return false;
		}
		if (!traits.NullOrEmpty())
		{
			foreach (WeaponTraitDef trait2 in traits)
			{
				if (trait.Overlaps(trait2))
				{
					return false;
				}
			}
		}
		return true;
	}

	public void AddTrait(WeaponTraitDef traitDef)
	{
		if (ModLister.CheckOdyssey("Unique Weapons"))
		{
			traits.Add(traitDef);
		}
	}

	public void Setup(bool fromSave)
	{
		foreach (WeaponTraitDef trait in traits)
		{
			if (trait.abilityProps != null)
			{
				CompEquippableAbilityReloadable compEquippableAbilityReloadable = parent.TryGetComp<CompEquippableAbilityReloadable>();
				compEquippableAbilityReloadable.props = trait.abilityProps;
				if (!fromSave)
				{
					compEquippableAbilityReloadable.Notify_PropsChanged();
				}
			}
		}
	}

	public override void Notify_Equipped(Pawn pawn)
	{
		if (!ModLister.CheckOdyssey("Unique weapon"))
		{
			return;
		}
		foreach (WeaponTraitDef trait in traits)
		{
			trait.Worker.Notify_Equipped(pawn);
		}
	}

	public void Notify_EquipmentLost(Pawn pawn)
	{
		foreach (WeaponTraitDef trait in traits)
		{
			trait.Worker.Notify_EquipmentLost(pawn);
		}
	}

	public override float GetStatOffset(StatDef stat)
	{
		float num = 0f;
		foreach (WeaponTraitDef item in TraitsListForReading)
		{
			num += item.statOffsets.GetStatOffsetFromList(stat);
		}
		return num;
	}

	public override float GetStatFactor(StatDef stat)
	{
		float num = 1f;
		foreach (WeaponTraitDef item in TraitsListForReading)
		{
			num *= item.statFactors.GetStatFactorFromList(stat);
		}
		return num;
	}

	public override void GetStatsExplanation(StatDef stat, StringBuilder sb, string whitespace = "")
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (WeaponTraitDef item in TraitsListForReading)
		{
			float statOffsetFromList = item.statOffsets.GetStatOffsetFromList(stat);
			if (!Mathf.Approximately(statOffsetFromList, 0f))
			{
				stringBuilder.AppendLine(whitespace + "    " + item.LabelCap + ": " + stat.Worker.ValueToString(statOffsetFromList, finalized: false, ToStringNumberSense.Offset));
			}
			float statFactorFromList = item.statFactors.GetStatFactorFromList(stat);
			if (!Mathf.Approximately(statFactorFromList, 1f))
			{
				stringBuilder.AppendLine(whitespace + "    " + item.LabelCap + ": " + stat.Worker.ValueToString(statFactorFromList, finalized: false, ToStringNumberSense.Factor));
			}
		}
		if (stringBuilder.Length != 0)
		{
			sb.AppendLine(whitespace + "StatsReport_WeaponTraits".Translate() + ":");
			sb.Append(stringBuilder.ToString());
		}
	}

	public override Color? ForceColor()
	{
		return color?.color;
	}

	public override string CompInspectStringExtra()
	{
		if (traits.NullOrEmpty())
		{
			return null;
		}
		return "WeaponTraits".Translate() + ": " + traits.Select((WeaponTraitDef x) => x.label).ToCommaList().CapitalizeFirst();
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__26))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__26(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompTipStringExtra()
	{
		return "WeaponTraits".Translate() + ": " + TraitsListForReading.Select((WeaponTraitDef x) => x.label).ToCommaList().CapitalizeFirst();
	}
}
