using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class GeneSetHolderBase : ThingWithComps
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__13 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneSetHolderBase _003C_003E4__this;

		private IEnumerator<Gizmo> _003C_003E7__wrap1;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__13(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GeneSetHolderBase geneSetHolderBase = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)geneSetHolderBase).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (geneSetHolderBase.geneSet != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "InspectGenes".Translate() + "...",
							defaultDesc = geneSetHolderBase.InspectGeneDescription,
							icon = GeneticInfoTex.Texture,
							action = delegate
							{
								InspectPaneUtility.OpenTab(typeof(ITab_Genes));
							}
						};
						_003C_003E1__state = 2;
						return true;
					}
					break;
				}
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__10 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneSetHolderBase _003C_003E4__this;

		private IEnumerator<StatDrawEntry> _003C_003E7__wrap1;

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
		public _003CSpecialDisplayStats_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GeneSetHolderBase geneSetHolderBase = _003C_003E4__this;
				Dialog_InfoCard.Hyperlink? inspectGenesHyperlink;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)geneSetHolderBase).SpecialDisplayStats().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0100;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (geneSetHolderBase.geneSet == null)
					{
						break;
					}
					inspectGenesHyperlink = null;
					if (ThingSelectionUtility.SelectableByMapClick(geneSetHolderBase))
					{
						inspectGenesHyperlink = new Dialog_InfoCard.Hyperlink(geneSetHolderBase, -1, thingIsGeneOwner: true);
					}
					_003C_003E7__wrap1 = geneSetHolderBase.geneSet.SpecialDisplayStats(inspectGenesHyperlink).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0100;
					IL_0100:
					if (_003C_003E7__wrap1.MoveNext())
					{
						StatDrawEntry current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					break;
				}
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__10(0)
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

	protected GeneSet geneSet;

	private const int MaxGeneLabels = 5;

	public static readonly CachedTexture GeneticInfoTex = new CachedTexture("UI/Gizmos/ViewGenes");

	private List<string> tmpGeneLabelsDesc = new List<string>();

	private List<string> tmpGeneLabels = new List<string>();

	public GeneSet GeneSet => geneSet;

	protected virtual string InspectGeneDescription => "InspectGenesDesc".Translate(this);

	public override string DescriptionDetailed
	{
		get
		{
			tmpGeneLabelsDesc.Clear();
			string text = base.DescriptionDetailed;
			if (geneSet == null || !geneSet.GenesListForReading.Any())
			{
				return text;
			}
			if (!text.NullOrEmpty())
			{
				text += "\n\n";
			}
			for (int i = 0; i < geneSet.GenesListForReading.Count; i++)
			{
				tmpGeneLabelsDesc.Add(geneSet.GenesListForReading[i].label);
			}
			return text + ("Genes".Translate().CapitalizeFirst() + ":\n" + tmpGeneLabelsDesc.ToLineList("  - ", capitalizeItems: true));
		}
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__10))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		tmpGeneLabels.Clear();
		if (geneSet != null && geneSet.GenesListForReading.Any())
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			List<GeneDef> genesListForReading = geneSet.GenesListForReading;
			int num = Mathf.Min(5, genesListForReading.Count);
			for (int i = 0; i < num; i++)
			{
				string text2 = genesListForReading[i].label;
				if (geneSet.IsOverridden(genesListForReading[i]))
				{
					text2 += " (" + "Overridden".Translate() + ")";
				}
				tmpGeneLabels.Add(text2);
			}
			if (genesListForReading.Count > num)
			{
				tmpGeneLabels.Add("Etc".Translate() + "...");
			}
			text += "Genes".Translate().CapitalizeFirst() + ":\n" + tmpGeneLabels.ToLineList("  - ", capitalizeItems: true);
		}
		return text;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__13))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Deep.Look(ref geneSet, "geneSet");
	}
}
