using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class PawnRenderNode_AnimalPart_Body : PawnRenderNode_AnimalPart
{
	[CompilerGenerated]
	private sealed class _003CStateGraphicsFor_003Ed__1 : IEnumerable<(GraphicStateDef state, Graphic graphic)>, IEnumerable, IEnumerator<(GraphicStateDef state, Graphic graphic)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (GraphicStateDef state, Graphic graphic) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public PawnRenderNode_AnimalPart_Body _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<(GraphicStateDef state, Graphic graphic)> _003C_003E7__wrap1;

		(GraphicStateDef state, Graphic graphic) IEnumerator<(GraphicStateDef, Graphic)>.Current
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
		public _003CStateGraphicsFor_003Ed__1(int _003C_003E1__state)
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
				PawnRenderNode_AnimalPart_Body pawnRenderNode_AnimalPart_Body = _003C_003E4__this;
				(GraphicStateDef, Graphic)? swimmingGraphic;
				(GraphicStateDef, Graphic)? stationaryGraphic;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((PawnRenderNode)pawnRenderNode_AnimalPart_Body).StateGraphicsFor(pawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00cd;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						(GraphicStateDef, Graphic) current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					swimmingGraphic = pawnRenderNode_AnimalPart_Body.GetSwimmingGraphic(pawn);
					if (swimmingGraphic.HasValue)
					{
						_003C_003E2__current = swimmingGraphic.Value;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_00cd;
					IL_00cd:
					stationaryGraphic = pawnRenderNode_AnimalPart_Body.GetStationaryGraphic(pawn);
					if (stationaryGraphic.HasValue)
					{
						_003C_003E2__current = stationaryGraphic.Value;
						_003C_003E1__state = 3;
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
		IEnumerator<(GraphicStateDef state, Graphic graphic)> IEnumerable<(GraphicStateDef, Graphic)>.GetEnumerator()
		{
			_003CStateGraphicsFor_003Ed__1 _003CStateGraphicsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CStateGraphicsFor_003Ed__ = this;
			}
			else
			{
				_003CStateGraphicsFor_003Ed__ = new _003CStateGraphicsFor_003Ed__1(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CStateGraphicsFor_003Ed__.pawn = _003C_003E3__pawn;
			return _003CStateGraphicsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(GraphicStateDef, Graphic)>)this).GetEnumerator();
		}
	}

	public PawnRenderNode_AnimalPart_Body(Pawn pawn, PawnRenderNodeProperties props, PawnRenderTree tree)
		: base(pawn, props, tree)
	{
	}

	[IteratorStateMachine(typeof(_003CStateGraphicsFor_003Ed__1))]
	protected override IEnumerable<(GraphicStateDef state, Graphic graphic)> StateGraphicsFor(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CStateGraphicsFor_003Ed__1(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}

	private (GraphicStateDef state, Graphic graphic)? GetSwimmingGraphic(Pawn pawn)
	{
		PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
		if (curKindLifeStage.swimmingGraphicData == null)
		{
			return null;
		}
		Graphic graphic = curKindLifeStage.swimmingGraphicData.Graphic;
		if (pawn.gender == Gender.Female && curKindLifeStage.femaleSwimmingGraphicData != null)
		{
			graphic = curKindLifeStage.femaleSwimmingGraphicData.Graphic;
		}
		if (pawn.TryGetAlternate(out var ag, out var _))
		{
			graphic = ag.GetSwimmingGraphic(graphic);
		}
		Color baseColor = graphic.Color;
		Color baseColor2 = graphic.ColorTwo;
		if (pawn.IsMutant)
		{
			baseColor = MutantUtility.GetMutantSkinColor(pawn, baseColor);
			baseColor2 = MutantUtility.GetMutantSkinColor(pawn, baseColor2);
		}
		baseColor = pawn.health.hediffSet.GetSkinColor(baseColor);
		baseColor2 = pawn.health.hediffSet.GetSkinColor(baseColor2);
		graphic = graphic.GetColoredVersion(graphic.Shader, baseColor, baseColor2);
		return (GraphicStateDefOf.Swimming, graphic);
	}

	private (GraphicStateDef state, Graphic graphic)? GetStationaryGraphic(Pawn pawn)
	{
		PawnKindLifeStage curKindLifeStage = pawn.ageTracker.CurKindLifeStage;
		if (curKindLifeStage.stationaryGraphicData == null)
		{
			return null;
		}
		Graphic graphic = curKindLifeStage.stationaryGraphicData.Graphic;
		if (pawn.gender == Gender.Female && curKindLifeStage.femaleStationaryGraphicData != null)
		{
			graphic = curKindLifeStage.femaleStationaryGraphicData.Graphic;
		}
		if (pawn.TryGetAlternate(out var ag, out var _))
		{
			graphic = ag.GetStationaryGraphic(graphic);
		}
		Color baseColor = graphic.Color;
		Color baseColor2 = graphic.ColorTwo;
		if (pawn.IsMutant)
		{
			baseColor = MutantUtility.GetMutantSkinColor(pawn, baseColor);
			baseColor2 = MutantUtility.GetMutantSkinColor(pawn, baseColor2);
		}
		baseColor = pawn.health.hediffSet.GetSkinColor(baseColor);
		baseColor2 = pawn.health.hediffSet.GetSkinColor(baseColor2);
		graphic = graphic.GetColoredVersion(graphic.Shader, baseColor, baseColor2);
		return (GraphicStateDefOf.Stationary, graphic);
	}
}
