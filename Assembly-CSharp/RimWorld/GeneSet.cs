using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Grammar;

namespace RimWorld;

public class GeneSet : IExposable
{
	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__27 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneSet _003C_003E4__this;

		private Dialog_InfoCard.Hyperlink? inspectGenesHyperlink;

		public Dialog_InfoCard.Hyperlink? _003C_003E3__inspectGenesHyperlink;

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
		public _003CSpecialDisplayStats_003Ed__27(int _003C_003E1__state)
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
			GeneSet geneSet = _003C_003E4__this;
			int metabolismTotal;
			int num2;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				IEnumerable<Dialog_InfoCard.Hyperlink> enumerable = Dialog_InfoCard.DefsToHyperlinks(geneSet.GeneDefHyperlinks);
				if (inspectGenesHyperlink.HasValue)
				{
					enumerable = enumerable.Concat(inspectGenesHyperlink.Value);
				}
				_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "Genes".Translate().CapitalizeFirst(), geneSet.genes.Select((GeneDef x) => x.label).ToCommaList().CapitalizeFirst(), "ContainedGenesDesc".Translate() + ":", 999, null, enumerable);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				int complexityTotal = geneSet.ComplexityTotal;
				if (complexityTotal != 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "ComplexityTotal".Translate(), complexityTotal.ToStringWithSign(), "ComplexityTotalDesc".Translate(), 998);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0143;
			}
			case 2:
				_003C_003E1__state = -1;
				goto IL_0143;
			case 3:
				_003C_003E1__state = -1;
				goto IL_019d;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0143:
				metabolismTotal = geneSet.MetabolismTotal;
				if (metabolismTotal != 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "MetabolismTotal".Translate(), metabolismTotal.ToStringWithSign(), "MetabolismTotalDesc".Translate(), 997);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_019d;
				IL_019d:
				num2 = geneSet.genes.Sum((GeneDef x) => x.biostatArc);
				if (num2 != 0)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Genetics, "ArchitesRequired".Translate(), num2.ToString(), "ArchitesRequiredDesc".Translate(), 995);
					_003C_003E1__state = 4;
					return true;
				}
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
			_003CSpecialDisplayStats_003Ed__27 _003CSpecialDisplayStats_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CSpecialDisplayStats_003Ed__ = this;
			}
			else
			{
				_003CSpecialDisplayStats_003Ed__ = new _003CSpecialDisplayStats_003Ed__27(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CSpecialDisplayStats_003Ed__.inspectGenesHyperlink = _003C_003E3__inspectGenesHyperlink;
			return _003CSpecialDisplayStats_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_GeneDefHyperlinks_003Ed__19 : IEnumerable<DefHyperlink>, IEnumerable, IEnumerator<DefHyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private DefHyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneSet _003C_003E4__this;

		private int _003Ci_003E5__2;

		DefHyperlink IEnumerator<DefHyperlink>.Current
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
		public _003Cget_GeneDefHyperlinks_003Ed__19(int _003C_003E1__state)
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
			GeneSet geneSet = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003Ci_003E5__2 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				_003Ci_003E5__2++;
				break;
			}
			if (_003Ci_003E5__2 < geneSet.genes.Count)
			{
				_003C_003E2__current = new DefHyperlink(geneSet.genes[_003Ci_003E5__2]);
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<DefHyperlink> IEnumerable<DefHyperlink>.GetEnumerator()
		{
			_003Cget_GeneDefHyperlinks_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_GeneDefHyperlinks_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<DefHyperlink>)this).GetEnumerator();
		}
	}

	private List<GeneDef> genes = new List<GeneDef>();

	private string name;

	[Unsaved(false)]
	private List<GeneDef> cachedNonOverriddenGenes;

	public const string ErrorSlugline = "ERR";

	private List<GeneDefWithType> tmpGeneDefWithTypes = new List<GeneDefWithType>();

	public string Label
	{
		get
		{
			if (name.NullOrEmpty())
			{
				GenerateName();
			}
			return name;
		}
	}

	private string LabelShortAdj
	{
		get
		{
			if (!genes.NullOrEmpty())
			{
				return genes[0].LabelShortAdj;
			}
			return "ERR";
		}
	}

	public List<GeneDef> GenesListForReading => genes;

	public bool Empty => genes.Count == 0;

	public int ComplexityTotal
	{
		get
		{
			RecacheOverridesIfNeeded();
			int num = 0;
			for (int i = 0; i < cachedNonOverriddenGenes.Count; i++)
			{
				num += cachedNonOverriddenGenes[i].biostatCpx;
			}
			return num;
		}
	}

	public int MetabolismTotal
	{
		get
		{
			RecacheOverridesIfNeeded();
			int num = 0;
			for (int i = 0; i < cachedNonOverriddenGenes.Count; i++)
			{
				num += cachedNonOverriddenGenes[i].biostatMet;
			}
			return num;
		}
	}

	public int ArchitesTotal
	{
		get
		{
			RecacheOverridesIfNeeded();
			int num = 0;
			for (int i = 0; i < cachedNonOverriddenGenes.Count; i++)
			{
				num += cachedNonOverriddenGenes[i].biostatArc;
			}
			return num;
		}
	}

	private IEnumerable<DefHyperlink> GeneDefHyperlinks
	{
		[IteratorStateMachine(typeof(_003Cget_GeneDefHyperlinks_003Ed__19))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_GeneDefHyperlinks_003Ed__19(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public void AddGene(GeneDef gene)
	{
		if (!genes.Contains(gene))
		{
			genes.Add(gene);
			DirtyCache();
		}
	}

	public void SetNameDirect(string name)
	{
		this.name = name;
	}

	public bool IsOverridden(GeneDef geneDef)
	{
		if (!ModsConfig.BiotechActive)
		{
			return false;
		}
		if (!genes.Contains(geneDef))
		{
			return false;
		}
		RecacheOverridesIfNeeded();
		return !cachedNonOverriddenGenes.Contains(geneDef);
	}

	public void GenerateName()
	{
		if (ModsConfig.BiotechActive && genes.Any())
		{
			GrammarRequest request = default(GrammarRequest);
			request.Includes.Add(RulePackDefOf.NamerGenepack);
			request.Rules.Add(new Rule_String("geneWord", LabelShortAdj));
			request.Rules.Add(new Rule_String("geneCountMinusOne", (genes.Count - 1).ToString()));
			request.Constants.Add("geneCount", genes.Count.ToString());
			name = GrammarResolver.Resolve("r_name", request, null, forceLog: false, null, null, null, capitalizeFirstSentence: false);
		}
		else
		{
			name = "ERR";
		}
	}

	public bool CanAddGeneDuringGeneration(GeneDef gene)
	{
		if (!ModsConfig.BiotechActive)
		{
			return false;
		}
		if (genes.Contains(gene))
		{
			return false;
		}
		if (genes.Count > 0 && !GeneTuning.BiostatRange.Includes(gene.biostatMet + MetabolismTotal))
		{
			return false;
		}
		if (!gene.canGenerateInGeneSet)
		{
			return false;
		}
		if (gene.prerequisite != null && !genes.Contains(gene.prerequisite))
		{
			return false;
		}
		for (int i = 0; i < genes.Count; i++)
		{
			if (gene.ConflictsWith(genes[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void SortGenes()
	{
		genes.SortBy((GeneDef x) => 0f - x.displayCategory.displayPriorityInGenepack);
	}

	public override string ToString()
	{
		return "GeneSet: " + (genes.NullOrEmpty() ? "None" : genes.Select((GeneDef x) => x.LabelCap.ToString()).ToCommaList());
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__27))]
	public IEnumerable<StatDrawEntry> SpecialDisplayStats(Dialog_InfoCard.Hyperlink? inspectGenesHyperlink = null)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__27(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__inspectGenesHyperlink = inspectGenesHyperlink
		};
	}

	public void Debug_RemoveGene(GeneDef gene)
	{
		if (genes.Remove(gene))
		{
			DirtyCache();
		}
	}

	private void DirtyCache()
	{
		name = null;
		cachedNonOverriddenGenes = null;
	}

	private void RecacheOverridesIfNeeded()
	{
		if (cachedNonOverriddenGenes != null)
		{
			return;
		}
		cachedNonOverriddenGenes = new List<GeneDef>();
		foreach (GeneDef gene in genes)
		{
			tmpGeneDefWithTypes.Add(new GeneDefWithType(gene, xenogene: true));
		}
		cachedNonOverriddenGenes.AddRange(tmpGeneDefWithTypes.NonOverriddenGenes());
		tmpGeneDefWithTypes.Clear();
	}

	public GeneSet Copy()
	{
		GeneSet geneSet = new GeneSet();
		geneSet.name = name;
		geneSet.genes.AddRange(genes);
		geneSet.DirtyCache();
		return geneSet;
	}

	public bool Matches(GeneSet other)
	{
		if (genes.Count != other.genes.Count)
		{
			return false;
		}
		for (int i = 0; i < genes.Count; i++)
		{
			if (!other.genes.Contains(genes[i]))
			{
				return false;
			}
		}
		return true;
	}

	public void ExposeData()
	{
		Scribe_Collections.Look(ref genes, "genes", LookMode.Def);
		Scribe_Values.Look(ref name, "name");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			genes.RemoveAll((GeneDef x) => x == null);
		}
	}
}
