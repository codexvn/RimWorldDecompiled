using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Trait : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetPermaThoughts_003Ed__27 : IEnumerable<ThoughtDef>, IEnumerable, IEnumerator<ThoughtDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThoughtDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Trait _003C_003E4__this;

		private TraitDegreeData _003Cdegree_003E5__2;

		private List<ThoughtDef> _003CallThoughts_003E5__3;

		private int _003Ci_003E5__4;

		ThoughtDef IEnumerator<ThoughtDef>.Current
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
		public _003CGetPermaThoughts_003Ed__27(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cdegree_003E5__2 = null;
			_003CallThoughts_003E5__3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			Trait trait = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_011f;
			}
			_003C_003E1__state = -1;
			_003Cdegree_003E5__2 = trait.CurrentData;
			_003CallThoughts_003E5__3 = DefDatabase<ThoughtDef>.AllDefsListForReading;
			_003Ci_003E5__4 = 0;
			goto IL_012f;
			IL_011f:
			_003Ci_003E5__4++;
			goto IL_012f;
			IL_012f:
			if (_003Ci_003E5__4 < _003CallThoughts_003E5__3.Count)
			{
				if (_003CallThoughts_003E5__3[_003Ci_003E5__4].IsSituational && _003CallThoughts_003E5__3[_003Ci_003E5__4].Worker is ThoughtWorker_AlwaysActive && _003CallThoughts_003E5__3[_003Ci_003E5__4].requiredTraits != null && _003CallThoughts_003E5__3[_003Ci_003E5__4].requiredTraits.Contains(trait.def) && (!_003CallThoughts_003E5__3[_003Ci_003E5__4].RequiresSpecificTraitsDegree || _003CallThoughts_003E5__3[_003Ci_003E5__4].requiredTraitsDegree == _003Cdegree_003E5__2.degree))
				{
					_003C_003E2__current = _003CallThoughts_003E5__3[_003Ci_003E5__4];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_011f;
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
		IEnumerator<ThoughtDef> IEnumerable<ThoughtDef>.GetEnumerator()
		{
			_003CGetPermaThoughts_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetPermaThoughts_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThoughtDef>)this).GetEnumerator();
		}
	}

	public Pawn pawn;

	public Gene sourceGene;

	public Gene suppressedByGene;

	public TraitDef def;

	private int degree;

	private bool scenForced;

	[LoadAlias("suppressed")]
	public bool suppressedByTrait;

	private List<string> suppressedReasons = new List<string>();

	private static List<WorkTypeDef> tmpDisabledWorktypes = new List<WorkTypeDef>();

	public int Degree => degree;

	public TraitDegreeData CurrentData => def.DataAtDegree(degree);

	public string Label => CurrentData.GetLabelFor(pawn);

	public string LabelCap => CurrentData.GetLabelCapFor(pawn);

	public bool ScenForced => scenForced;

	public bool Suppressed
	{
		get
		{
			if (ModsConfig.BiotechActive)
			{
				if (suppressedByTrait || suppressedByGene != null)
				{
					return true;
				}
				if (sourceGene != null && sourceGene.Overridden && sourceGene.overriddenByGene.def != sourceGene.def)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Trait()
	{
	}

	public Trait(TraitDef def, int degree = 0, bool forced = false)
	{
		this.def = def;
		this.degree = degree;
		scenForced = forced;
	}

	public void ExposeData()
	{
		Scribe_Defs.Look(ref def, "def");
		Scribe_References.Look(ref sourceGene, "sourceGene");
		Scribe_Values.Look(ref degree, "degree", 0);
		Scribe_Values.Look(ref scenForced, "scenForced", defaultValue: false);
		Scribe_Values.Look(ref suppressedByTrait, "suppressedByTrait", defaultValue: false);
		Scribe_References.Look(ref suppressedByGene, "suppressedBy");
		if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && def == null)
		{
			def = DefDatabase<TraitDef>.GetRandom();
			degree = PawnGenerator.RandomTraitDegree(def);
		}
	}

	public float OffsetOfStat(StatDef stat)
	{
		if (Suppressed)
		{
			return 0f;
		}
		float num = 0f;
		TraitDegreeData currentData = CurrentData;
		if (currentData.statOffsets != null)
		{
			for (int i = 0; i < currentData.statOffsets.Count; i++)
			{
				if (currentData.statOffsets[i].stat == stat)
				{
					num += currentData.statOffsets[i].value;
				}
			}
		}
		return num;
	}

	public float MultiplierOfStat(StatDef stat)
	{
		if (Suppressed)
		{
			return 1f;
		}
		float num = 1f;
		TraitDegreeData currentData = CurrentData;
		if (currentData.statFactors != null)
		{
			for (int i = 0; i < currentData.statFactors.Count; i++)
			{
				if (currentData.statFactors[i].stat == stat)
				{
					num *= currentData.statFactors[i].value;
				}
			}
		}
		return num;
	}

	public string TipString(Pawn pawn)
	{
		StringBuilder stringBuilder = new StringBuilder();
		suppressedReasons.Clear();
		TraitDegreeData currentData = CurrentData;
		stringBuilder.AppendLine(currentData.description.Formatted(pawn.Named("PAWN")).AdjustedFor(pawn).Resolve());
		bool num = CurrentData.skillGains.Count > 0;
		bool flag = GetPermaThoughts().Any();
		bool flag2 = currentData.statOffsets != null;
		bool flag3 = currentData.statFactors != null;
		if (num || flag || flag2 || flag3 || !Mathf.Approximately(currentData.painFactor, 1f) || currentData.painOffset != 0f || !currentData.aptitudes.NullOrEmpty())
		{
			stringBuilder.AppendLine();
		}
		if (num)
		{
			foreach (SkillGain skillGain in CurrentData.skillGains)
			{
				if (skillGain.amount != 0)
				{
					stringBuilder.AppendLine("    " + skillGain.skill.skillLabel.CapitalizeFirst() + ":   " + skillGain.amount.ToString("+##;-##"));
				}
			}
		}
		if (flag)
		{
			foreach (ThoughtDef permaThought in GetPermaThoughts())
			{
				stringBuilder.AppendLine("    " + "PermanentMoodEffect".Translate() + " " + permaThought.stages[0].baseMoodEffect.ToStringByStyle(ToStringStyle.Integer, ToStringNumberSense.Offset));
			}
		}
		if (flag2)
		{
			for (int i = 0; i < currentData.statOffsets.Count; i++)
			{
				StatModifier statModifier = currentData.statOffsets[i];
				string valueToStringAsOffset = statModifier.ValueToStringAsOffset;
				string value = "    " + statModifier.stat.LabelCap + " " + valueToStringAsOffset;
				stringBuilder.AppendLine(value);
			}
		}
		if (currentData.painOffset != 0f)
		{
			stringBuilder.AppendLine("    " + "Pain".Translate() + " " + currentData.painOffset.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Offset));
		}
		if (flag3)
		{
			for (int j = 0; j < currentData.statFactors.Count; j++)
			{
				StatModifier statModifier2 = currentData.statFactors[j];
				string toStringAsFactor = statModifier2.ToStringAsFactor;
				string value2 = "    " + statModifier2.stat.LabelCap + " " + toStringAsFactor;
				stringBuilder.AppendLine(value2);
			}
		}
		if (!Mathf.Approximately(currentData.hungerRateFactor, 1f))
		{
			string text = currentData.hungerRateFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor);
			string value3 = "    " + "HungerRate".Translate() + " " + text;
			stringBuilder.AppendLine(value3);
		}
		if (!Mathf.Approximately(currentData.painFactor, 1f))
		{
			stringBuilder.AppendLine("    " + "Pain".Translate() + " " + currentData.painFactor.ToStringByStyle(ToStringStyle.PercentOne, ToStringNumberSense.Factor));
		}
		if (!currentData.aptitudes.NullOrEmpty())
		{
			stringBuilder.AppendLine().AppendLineTagged(("Aptitudes".Translate().CapitalizeFirst() + ":").AsTipTitle());
			stringBuilder.AppendLine(currentData.aptitudes.Select((Aptitude x) => x.skill.LabelCap.ToString() + " " + x.level.ToStringWithSign()).ToLineList("  - ", capitalizeItems: true));
		}
		if (!currentData.enablesNeeds.NullOrEmpty())
		{
			stringBuilder.AppendLine().AppendLineTagged(("AddsNeeds".Translate().CapitalizeFirst() + ":").AsTipTitle());
			stringBuilder.AppendLine(currentData.enablesNeeds.Select((NeedDef x) => x.LabelCap.ToString()).ToLineList("  - ", capitalizeItems: true));
		}
		if (!currentData.disablesNeeds.NullOrEmpty())
		{
			stringBuilder.AppendLine().AppendLineTagged(("DisablesNeeds".Translate().CapitalizeFirst() + ":").AsTipTitle());
			stringBuilder.AppendLine(currentData.disablesNeeds.Select((NeedDef x) => x.LabelCap.ToString()).ToLineList("  - ", capitalizeItems: true));
		}
		if (ModsConfig.RoyaltyActive)
		{
			List<MeditationFocusDef> allowedMeditationFocusTypes = CurrentData.allowedMeditationFocusTypes;
			if (!allowedMeditationFocusTypes.NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("EnablesMeditationFocusType".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + allowedMeditationFocusTypes.Select((MeditationFocusDef f) => f.LabelCap.Resolve()).ToLineList("  - "));
			}
		}
		if (ModsConfig.IdeologyActive)
		{
			List<IssueDef> affectedIssues = CurrentData.GetAffectedIssues(def);
			if (affectedIssues.Count != 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("OverridesSomePrecepts".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + affectedIssues.Select((IssueDef x) => x.LabelCap.Resolve()).ToLineList("  - "));
			}
			List<MemeDef> affectedMemes = CurrentData.GetAffectedMemes(def, agreeable: true);
			if (affectedMemes.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("AgreeableMemes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + affectedMemes.Select((MemeDef x) => x.LabelCap.Resolve()).ToLineList("  - "));
			}
			List<MemeDef> affectedMemes2 = CurrentData.GetAffectedMemes(def, agreeable: false);
			if (affectedMemes2.Count > 0)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("DisagreeableMemes".Translate().Colorize(ColoredText.TipSectionTitleColor) + ":\n" + affectedMemes2.Select((MemeDef x) => x.LabelCap.Resolve()).ToLineList("  - "));
			}
		}
		if (ModsConfig.BiotechActive)
		{
			if (sourceGene != null)
			{
				stringBuilder.AppendLine().AppendLine(("AddedByGene".Translate() + ": " + sourceGene.LabelCap).Colorize(ColoredText.GeneColor));
			}
			if (Suppressed)
			{
				foreach (Trait allTrait in pawn.story.traits.allTraits)
				{
					if (allTrait != this && allTrait.def.CanSuppress(this))
					{
						suppressedReasons.Add($"{allTrait.Label} ({"Trait".Translate()})");
					}
				}
				if (sourceGene != null && sourceGene.Overridden && sourceGene.overriddenByGene.def != sourceGene.def)
				{
					suppressedReasons.Add($"{sourceGene.overriddenByGene.Label} ({"Gene".Translate()})");
				}
				if (suppressedByGene != null)
				{
					suppressedReasons.Add($"{suppressedByGene.Label} ({"Gene".Translate()})");
				}
				if (suppressedReasons.Any())
				{
					stringBuilder.AppendLine().AppendLine(("Suppressed".Translate().CapitalizeFirst() + ": " + "SuppressedDesc".Translate() + ":\n" + suppressedReasons.ToLineList("  - ", capitalizeItems: true)).Colorize(ColoredText.SubtleGrayColor));
				}
			}
		}
		if (stringBuilder.Length > 0)
		{
			if (stringBuilder[stringBuilder.Length - 1] == '\n')
			{
				if (stringBuilder.Length > 1)
				{
					if (stringBuilder[stringBuilder.Length - 2] == '\r')
					{
						stringBuilder.Remove(stringBuilder.Length - 2, 2);
						goto IL_0991;
					}
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
			}
		}
		goto IL_0991;
		IL_0991:
		return stringBuilder.ToString();
	}

	public override string ToString()
	{
		if (def == null)
		{
			return "Trait(null)";
		}
		return "Trait(" + def.ToString() + "-" + degree + ")";
	}

	[IteratorStateMachine(typeof(_003CGetPermaThoughts_003Ed__27))]
	private IEnumerable<ThoughtDef> GetPermaThoughts()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetPermaThoughts_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}

	private bool AllowsWorkType(WorkTypeDef workDef)
	{
		return (def.disabledWorkTags & workDef.workTags) == 0;
	}

	public void Notify_MentalStateEndedOn(Pawn pawn, bool causedByMood)
	{
		if (causedByMood)
		{
			Notify_MentalStateEndedOn(pawn);
		}
	}

	public void Notify_MentalStateEndedOn(Pawn pawn)
	{
		TraitDegreeData currentData = CurrentData;
		if (!currentData.mentalBreakInspirationGainSet.NullOrEmpty() && !(Rand.Value > currentData.mentalBreakInspirationGainChance))
		{
			pawn.mindState.inspirationHandler.TryStartInspiration(currentData.mentalBreakInspirationGainSet.RandomElement(), currentData.mentalBreakInspirationGainReasonText);
		}
	}

	public List<WorkTypeDef> GetDisabledWorkTypes()
	{
		tmpDisabledWorktypes.Clear();
		for (int i = 0; i < def.disabledWorkTypes.Count; i++)
		{
			tmpDisabledWorktypes.Add(def.disabledWorkTypes[i]);
		}
		List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
		for (int j = 0; j < allDefsListForReading.Count; j++)
		{
			WorkTypeDef workTypeDef = allDefsListForReading[j];
			if (!AllowsWorkType(workTypeDef))
			{
				tmpDisabledWorktypes.Add(workTypeDef);
			}
		}
		return tmpDisabledWorktypes;
	}
}
