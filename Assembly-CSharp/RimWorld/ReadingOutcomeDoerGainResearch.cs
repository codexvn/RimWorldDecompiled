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

public class ReadingOutcomeDoerGainResearch : BookOutcomeDoer
{
	[CompilerGenerated]
	private sealed class _003CGetHyperlinks_003Ed__14 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ReadingOutcomeDoerGainResearch _003C_003E4__this;

		private Dictionary<ResearchProjectDef, float>.Enumerator _003C_003E7__wrap1;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003CGetHyperlinks_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(Dictionary<ResearchProjectDef, float>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ReadingOutcomeDoerGainResearch readingOutcomeDoerGainResearch = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = readingOutcomeDoerGainResearch.values.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					var (researchProjectDef2, _) = (KeyValuePair<ResearchProjectDef, float>)(ref _003C_003E7__wrap1.Current);
					if (readingOutcomeDoerGainResearch.IsProjectVisible(researchProjectDef2))
					{
						_003C_003E2__current = new Dialog_InfoCard.Hyperlink(researchProjectDef2);
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(Dictionary<ResearchProjectDef, float>.Enumerator);
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CGetHyperlinks_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetHyperlinks_003Ed__14(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	protected Dictionary<ResearchProjectDef, float> values = new Dictionary<ResearchProjectDef, float>();

	private const float MultipleProjectFactor = 1.25f;

	public new BookOutcomeProperties_GainResearch Props => (BookOutcomeProperties_GainResearch)props;

	public virtual int RoundTo { get; } = 10;

	private bool IsValid(ResearchProjectDef project)
	{
		if (project.generalRules == null)
		{
			return false;
		}
		if (Props.exclude.Count > 0 && Props.exclude.ContainsAny((BookOutcomeProperties_GainResearch.BookResearchItem i) => i.project == project))
		{
			return false;
		}
		if ((!Props.ignoreZeroBaseCost || project.baseCost != 0f) && (Props.tabs.NullOrEmpty() || Props.tabs.Exists((BookOutcomeProperties_GainResearch.BookTabItem x) => x.tab == project.tab)))
		{
			return project.TechprintCount == 0;
		}
		return false;
	}

	public override bool BenefitDetailsCanChange(Pawn reader = null)
	{
		foreach (var (researchProjectDef2, _) in values)
		{
			if (researchProjectDef2.IsHidden)
			{
				return true;
			}
			if (!researchProjectDef2.IsFinished)
			{
				return true;
			}
		}
		return false;
	}

	public override void OnBookGenerated(Pawn author = null)
	{
		List<ResearchProjectDef> allDefsListForReading = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
		List<ResearchProjectDef> list = new List<ResearchProjectDef>(allDefsListForReading.Count);
		if (Props.include.Count > 0)
		{
			for (int i = 0; i < Props.include.Count; i++)
			{
				ResearchProjectDef project = Props.include[i].project;
				if (!project.IsFinished && IsValid(project))
				{
					list.Add(project);
				}
			}
		}
		else
		{
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				if (allDefsListForReading[j].PrerequisitesCompleted && !allDefsListForReading[j].IsFinished && IsValid(allDefsListForReading[j]))
				{
					list.Add(allDefsListForReading[j]);
				}
			}
		}
		if (list.Count == 0)
		{
			for (int k = 0; k < allDefsListForReading.Count; k++)
			{
				if (IsValid(allDefsListForReading[k]))
				{
					list.Add(allDefsListForReading[k]);
				}
			}
		}
		if (list.Empty())
		{
			Log.ErrorOnce($"Research book had no valid projects and failed to find any backup projects.\nignoring zero base cost: {Props.ignoreZeroBaseCost}).\ntabs: {Props.tabs.Select((BookOutcomeProperties_GainResearch.BookTabItem x) => x.tab.generalTitle).ToCommaList()}.\nexcludes: {(Props.exclude.Empty() ? "(none)" : Props.exclude.Select((BookOutcomeProperties_GainResearch.BookResearchItem x) => x.project.label).ToCommaList())}", 15242436);
		}
		else
		{
			int count = ((!Rand.Chance(0.25f)) ? 1 : 2);
			List<ResearchProjectDef> list2 = list.TakeRandomDistinct(count);
			float num = GetBaseValue();
			if (list2.Count > 1)
			{
				num *= 1.25f;
			}
			float value = num / (float)list2.Count;
			for (int l = 0; l < list2.Count; l++)
			{
				values[list2[l]] = value;
			}
		}
	}

	public override void Reset()
	{
		values.Clear();
	}

	public override bool DoesProvidesOutcome(Pawn reader)
	{
		foreach (KeyValuePair<ResearchProjectDef, float> value2 in values)
		{
			value2.Deconstruct(out var key, out var _);
			if (!key.IsFinished)
			{
				return true;
			}
		}
		return false;
	}

	protected virtual float GetBaseValue()
	{
		return BookUtility.GetResearchExpForQuality(base.Quality);
	}

	public override void OnReadingTick(Pawn reader, float factor)
	{
		foreach (var (researchProjectDef2, num2) in values)
		{
			if (IsProjectVisible(researchProjectDef2) && !researchProjectDef2.IsFinished)
			{
				Find.ResearchManager.AddProgress(researchProjectDef2, num2 * factor);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetHyperlinks_003Ed__14))]
	public override IEnumerable<Dialog_InfoCard.Hyperlink> GetHyperlinks()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetHyperlinks_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetBenefitsString(Pawn reader = null)
	{
		if (values.Count == 0)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		foreach (KeyValuePair<ResearchProjectDef, float> value2 in values)
		{
			value2.Deconstruct(out var key, out var value);
			ResearchProjectDef researchProjectDef = key;
			float num = value * 2500f;
			if (RoundTo != 0)
			{
				num = Mathf.Round(num / (float)RoundTo) * (float)RoundTo;
			}
			string text = $"{researchProjectDef.LabelCap}: {"PerHour".Translate(num.ToStringDecimalIfSmall())}";
			if (researchProjectDef.IsFinished)
			{
				text += $" ({"AlreadyUnlocked".Translate()})";
				text = text.Colorize(Color.gray);
			}
			else if (!IsProjectVisible(researchProjectDef))
			{
				text += $" ({"WhenDiscovered".Translate()})";
				text = text.Colorize(Color.gray);
			}
			stringBuilder.AppendLine(" - " + text);
		}
		return stringBuilder.ToString().TrimEndNewlines();
	}

	private bool IsProjectVisible(ResearchProjectDef project)
	{
		if (Props.usesHiddenProjects)
		{
			return project.CanStartNow;
		}
		return true;
	}

	public override IEnumerable<RulePack> GetTopicRulePacks()
	{
		return values.Keys.Select((ResearchProjectDef x) => x.generalRules);
	}

	public override void PostExposeData()
	{
		Scribe_Collections.Look(ref values, "values");
	}
}
