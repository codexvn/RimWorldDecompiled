using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class Gene_Hemogen : Gene_Resource, IGeneResourceDrain
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__26 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Gene_Hemogen _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__26(int _003C_003E1__state)
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
				Gene_Hemogen gene_Hemogen = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!gene_Hemogen.Active)
					{
						return false;
					}
					_003C_003E7__wrap1 = ((Gene_Resource)gene_Hemogen).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0081;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0081;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0081:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = GeneResourceDrainUtility.GetResourceDrainGizmos(gene_Hemogen).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__26 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__26(0)
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

	public bool hemogenPacksAllowed = true;

	public Gene_Resource Resource => this;

	public Pawn Pawn => pawn;

	public bool CanOffset
	{
		get
		{
			if (Active)
			{
				return !pawn.Deathresting;
			}
			return false;
		}
	}

	public string DisplayLabel => Label + " (" + "Gene".Translate() + ")";

	public float ResourceLossPerDay => def.resourceLossPerDay;

	public override float InitialResourceMax => 1f;

	public override float MinLevelForAlert => 0.15f;

	public override float MaxLevelOffset => 0.1f;

	protected override Color BarColor => new ColorInt(138, 3, 3).ToColor;

	protected override Color BarHighlightColor => new ColorInt(145, 42, 42).ToColor;

	public override void PostAdd()
	{
		if (ModLister.CheckBiotech("Hemogen"))
		{
			base.PostAdd();
			Reset();
		}
	}

	public override void Notify_IngestedThing(Thing thing, int numTaken)
	{
		if (thing.def.IsMeat)
		{
			IngestibleProperties ingestible = thing.def.ingestible;
			if (ingestible != null && ingestible.sourceDef?.race?.Humanlike == true)
			{
				GeneUtility.OffsetHemogen(pawn, 0.0375f * thing.GetStatValue(StatDefOf.Nutrition) * (float)numTaken);
			}
		}
	}

	public override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		GeneResourceDrainUtility.TickResourceDrainInterval(this, delta);
	}

	public override void SetTargetValuePct(float val)
	{
		targetValue = Mathf.Clamp(val * Max, 0f, Max - MaxLevelOffset);
	}

	public bool ShouldConsumeHemogenNow()
	{
		return Value < targetValue;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__26))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__26(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref hemogenPacksAllowed, "hemogenPacksAllowed", defaultValue: true);
	}
}
