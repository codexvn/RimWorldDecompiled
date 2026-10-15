using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public abstract class Gene_Resource : Gene
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__37 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Gene_Resource _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__37(int _003C_003E1__state)
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
			Gene_Resource gene_Resource = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!gene_Resource.Active)
				{
					return false;
				}
				if (gene_Resource.gizmo == null)
				{
					gene_Resource.gizmo = (GeneGizmo_Resource)Activator.CreateInstance(gene_Resource.def.resourceGizmoType, gene_Resource, gene_Resource.DrainGenes, gene_Resource.BarColor, gene_Resource.BarHighlightColor);
				}
				if ((Find.Selector.SelectedPawns.Count == 1 || gene_Resource.def.showGizmoOnMultiSelect) && (!gene_Resource.pawn.Drafted || gene_Resource.def.showGizmoWhenDrafted))
				{
					_003C_003E2__current = gene_Resource.gizmo;
					_003C_003E1__state = 1;
					return true;
				}
				break;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__37 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__37(0)
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

	public float targetValue = 0.5f;

	protected float cur;

	protected float max;

	[Unsaved(false)]
	protected GeneGizmo_Resource gizmo;

	[Unsaved(false)]
	private List<IGeneResourceDrain> tmpDrainGenes = new List<IGeneResourceDrain>();

	public virtual string ResourceLabel => def.resourceLabel;

	public abstract float InitialResourceMax { get; }

	protected abstract Color BarColor { get; }

	protected abstract Color BarHighlightColor { get; }

	public abstract float MinLevelForAlert { get; }

	public virtual int ValueForDisplay => PostProcessValue(cur);

	public virtual int MaxForDisplay => PostProcessValue(max);

	public virtual float MaxLevelOffset => 0f;

	public virtual float Max => max;

	public virtual float Value
	{
		get
		{
			return cur;
		}
		set
		{
			cur = Mathf.Clamp(value, 0f, max);
		}
	}

	public virtual float ValuePercent
	{
		get
		{
			return cur / max;
		}
		set
		{
			cur = max * value;
		}
	}

	private List<IGeneResourceDrain> DrainGenes
	{
		get
		{
			tmpDrainGenes.Clear();
			List<Gene> genesListForReading = pawn.genes.GenesListForReading;
			for (int i = 0; i < genesListForReading.Count; i++)
			{
				if (genesListForReading[i] is IGeneResourceDrain geneResourceDrain && geneResourceDrain.Resource == this)
				{
					tmpDrainGenes.Add(geneResourceDrain);
				}
			}
			return tmpDrainGenes;
		}
	}

	public override void PostAdd()
	{
		base.PostAdd();
		Reset();
	}

	public void SetMax(float newMax)
	{
		max = newMax;
		cur = Mathf.Clamp(cur, 0f, max);
		SetTargetValuePct(targetValue);
	}

	public void ResetMax()
	{
		max = InitialResourceMax;
		cur = Mathf.Clamp(cur, 0f, max);
		SetTargetValuePct(targetValue);
	}

	public override void Reset()
	{
		cur = (max = InitialResourceMax);
		targetValue = 0.5f;
	}

	public virtual void SetTargetValuePct(float val)
	{
		targetValue = val * Max;
	}

	public virtual int PostProcessValue(float value)
	{
		return Mathf.RoundToInt(value * 100f);
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__37))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__37(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref cur, "cur", 0f);
		Scribe_Values.Look(ref max, "max", 0f);
		Scribe_Values.Look(ref targetValue, "targetValue", 0.5f);
	}
}
