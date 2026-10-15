using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld;

[StaticConstructorOnStartup]
public class AncientHatch : MapPortal
{
	[CompilerGenerated]
	private sealed class _003CGetExtraGenSteps_003Ed__10 : IEnumerable<GenStepWithParams>, IEnumerable, IEnumerator<GenStepWithParams>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private GenStepWithParams _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AncientHatch _003C_003E4__this;

		GenStepWithParams IEnumerator<GenStepWithParams>.Current
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
		public _003CGetExtraGenSteps_003Ed__10(int _003C_003E1__state)
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
			AncientHatch ancientHatch = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (ancientHatch.layout != null)
				{
					_003C_003E2__current = new GenStepWithParams(GenStepDefOf.AncientStockpile, new GenStepParams
					{
						layout = ancientHatch.layout
					});
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = new GenStepWithParams(GenStepDefOf.AncientStockpile, default(GenStepParams));
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
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
		IEnumerator<GenStepWithParams> IEnumerable<GenStepWithParams>.GetEnumerator()
		{
			_003CGetExtraGenSteps_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetExtraGenSteps_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<GenStepWithParams>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__13 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AncientHatch _003C_003E4__this;

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
				AncientHatch ancientHatch = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MapPortal)ancientHatch).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Gizmo current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
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

	public TileMutatorWorker_Stockpile.StockpileType stockpileType;

	public LayoutDef layout;

	private CompHackable hackableInt;

	private GraphicData openGraphicData;

	private const string OpenTexturePath = "Things/Building/AncientHatch/AncientHatch_Open";

	private CompHackable Hackable => hackableInt ?? (hackableInt = GetComp<CompHackable>());

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref stockpileType, "stockpileType", TileMutatorWorker_Stockpile.StockpileType.Medicine);
		Scribe_Defs.Look(ref layout, "layout");
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		openGraphicData = new GraphicData();
		openGraphicData.CopyFrom(def.graphicData);
		openGraphicData.texPath = "Things/Building/AncientHatch/AncientHatch_Open";
	}

	public override void Print(SectionLayer layer)
	{
		if (IsEnterable(out var _))
		{
			openGraphicData.Graphic.Print(layer, this, 0f);
		}
		else
		{
			Graphic.Print(layer, this, 0f);
		}
	}

	[IteratorStateMachine(typeof(_003CGetExtraGenSteps_003Ed__10))]
	protected override IEnumerable<GenStepWithParams> GetExtraGenSteps()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetExtraGenSteps_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override bool IsEnterable(out string reason)
	{
		if (!Hackable.IsHacked)
		{
			reason = "Locked".Translate();
			return false;
		}
		return base.IsEnterable(out reason);
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder(base.GetInspectString());
		if (Hackable.IsHacked)
		{
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("HatchUnlocked".Translate());
		}
		return stringBuilder.ToString();
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
}
