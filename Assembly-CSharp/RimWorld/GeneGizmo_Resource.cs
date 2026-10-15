using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class GeneGizmo_Resource : Gizmo_Slider
{
	[CompilerGenerated]
	private sealed class _003CGetBarThresholds_003Ed__24 : IEnumerable<float>, IEnumerable, IEnumerator<float>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private float _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public GeneGizmo_Resource _003C_003E4__this;

		private int _003Ci_003E5__2;

		float IEnumerator<float>.Current
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
		public _003CGetBarThresholds_003Ed__24(int _003C_003E1__state)
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
			GeneGizmo_Resource geneGizmo_Resource = _003C_003E4__this;
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
			if (_003Ci_003E5__2 < geneGizmo_Resource.gene.def.resourceGizmoThresholds.Count)
			{
				_003C_003E2__current = geneGizmo_Resource.gene.def.resourceGizmoThresholds[_003Ci_003E5__2];
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
		IEnumerator<float> IEnumerable<float>.GetEnumerator()
		{
			_003CGetBarThresholds_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetBarThresholds_003Ed__24(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<float>)this).GetEnumerator();
		}
	}

	protected Gene_Resource gene;

	protected List<IGeneResourceDrain> drainGenes;

	protected override Color BarColor { get; }

	protected override Color BarHighlightColor { get; }

	protected override bool IsDraggable
	{
		get
		{
			if (!gene.pawn.IsColonistPlayerControlled)
			{
				return gene.pawn.IsPrisonerOfColony;
			}
			return true;
		}
	}

	protected override string BarLabel => $"{gene.ValueForDisplay} / {gene.MaxForDisplay}";

	protected override int Increments => gene.MaxForDisplay / 10;

	protected override float ValuePercent => gene.ValuePercent;

	protected override FloatRange DragRange => new FloatRange(0f, gene.Max);

	protected override float Target
	{
		get
		{
			return gene.targetValue / gene.Max;
		}
		set
		{
			gene.SetTargetValuePct(value);
		}
	}

	protected override string Title
	{
		get
		{
			string text = gene.ResourceLabel.CapitalizeFirst();
			if (Find.Selector.SelectedPawns.Count != 1)
			{
				text = text + " (" + gene.pawn.LabelShort + ")";
			}
			return text;
		}
	}

	public GeneGizmo_Resource(Gene_Resource gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barHighlightColor)
	{
		this.gene = gene;
		this.drainGenes = drainGenes;
		BarColor = barColor;
		BarHighlightColor = barHighlightColor;
	}

	[IteratorStateMachine(typeof(_003CGetBarThresholds_003Ed__24))]
	protected override IEnumerable<float> GetBarThresholds()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetBarThresholds_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}
}
