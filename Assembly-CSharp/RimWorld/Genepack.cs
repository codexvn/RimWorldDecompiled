using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class Genepack : GeneSetHolderBase
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__18 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Genepack _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__18(int _003C_003E1__state)
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
				Genepack CS_0024_003C_003E8__locals4 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((GeneSetHolderBase)CS_0024_003C_003E8__locals4).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
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
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "GenepackAutoLoad".Translate(),
						defaultDesc = "GenepackAutoLoadDesc".Translate(),
						icon = IconTex.Texture,
						isActive = () => CS_0024_003C_003E8__locals4.autoLoad,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals4.autoLoad = !CS_0024_003C_003E8__locals4.autoLoad;
						}
					};
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CGetGizmos_003Ed__18 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__18(0)
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

	public Thing targetContainer;

	private float hpRecoveryPct;

	private float deteriorationPct;

	private bool autoLoad = true;

	private const int HitPointRecoveryPerDayInGeneBankPerDay = 1;

	private static readonly CachedTexture IconTex = new CachedTexture("UI/Gizmos/AutoLoadGenepack");

	public override string LabelNoCount
	{
		get
		{
			if (geneSet != null && geneSet.Label != "ERR")
			{
				return geneSet.Label;
			}
			return base.LabelNoCount;
		}
	}

	private CompGenepackContainer ParentContainer
	{
		get
		{
			IThingHolder parentHolder = base.ParentHolder;
			if (parentHolder == null || !(parentHolder is CompGenepackContainer result))
			{
				return null;
			}
			return result;
		}
	}

	public bool Deteriorating
	{
		get
		{
			CompGenepackContainer parentContainer = ParentContainer;
			if (parentContainer == null || !parentContainer.PowerOn)
			{
				return true;
			}
			return false;
		}
	}

	public bool AutoLoad => autoLoad;

	public void Initialize(List<GeneDef> genes)
	{
		geneSet = new GeneSet();
		foreach (GeneDef gene in genes)
		{
			geneSet.AddGene(gene);
		}
	}

	public override void PostMake()
	{
		if (!ModLister.CheckBiotech("genepack"))
		{
			Destroy();
			return;
		}
		base.PostMake();
		geneSet = GeneUtility.GenerateGeneSet(null);
	}

	public override void TickRare()
	{
		base.TickRare();
		if (ParentContainer != null)
		{
			if (Deteriorating)
			{
				float statValue = this.GetStatValue(StatDefOf.DeteriorationRate);
				if (statValue > 0.001f && base.ParentHolder != null)
				{
					deteriorationPct += statValue * 250f / 60000f;
					if (deteriorationPct >= 1f)
					{
						deteriorationPct -= 1f;
						SteadyEnvironmentEffects.DoDeteriorationDamage(this, base.PositionHeld, base.MapHeld, sendMessage: true);
					}
				}
			}
			else
			{
				if (HitPoints >= base.MaxHitPoints)
				{
					return;
				}
				hpRecoveryPct += 0.004166667f;
				if (hpRecoveryPct >= 1f)
				{
					hpRecoveryPct -= 1f;
					HitPoints++;
					if (HitPoints == base.MaxHitPoints)
					{
						hpRecoveryPct = 0f;
					}
				}
			}
		}
		else if (base.Spawned && !autoLoad && targetContainer != null)
		{
			targetContainer = null;
		}
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (targetContainer != null && targetContainer.Map == base.Map)
		{
			GenDraw.DrawLineBetween(DrawPos, targetContainer.DrawPos);
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__18))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__18(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref targetContainer, "targetContainer");
		Scribe_Values.Look(ref hpRecoveryPct, "hpRecoveryPct", 0f);
		Scribe_Values.Look(ref deteriorationPct, "deteriorationPct", 0f);
		Scribe_Values.Look(ref autoLoad, "autoLoad", defaultValue: true);
	}
}
