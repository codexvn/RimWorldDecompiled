using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse;

public class AnimationDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__8 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AnimationDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		private Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator _003C_003E7__wrap2;

		string IEnumerator<string>.Current
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
		public _003CConfigErrors_003Ed__8(int _003C_003E1__state)
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
			_003C_003E7__wrap2 = default(Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				AnimationDef animationDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)animationDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0072:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (animationDef.keyframeParts == null || animationDef.curveParts == null)
					{
						return false;
					}
					_003C_003E7__wrap2 = animationDef.keyframeParts.GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				while (_003C_003E7__wrap2.MoveNext())
				{
					var (pawnRenderNodeTagDef2, _) = (KeyValuePair<PawnRenderNodeTagDef, KeyframeAnimationPart>)(ref _003C_003E7__wrap2.Current);
					if (animationDef.curveParts.ContainsKey(pawnRenderNodeTagDef2))
					{
						_003C_003E2__current = $"Animation def has a render node tag {pawnRenderNodeTagDef2} which is driven by both keyframes and curve parts. You can only use one or the other for a given node tag";
						_003C_003E1__state = 2;
						return true;
					}
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap2 = default(Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGraphicStates_003Ed__9 : IEnumerable<(GraphicStateDef state, Graphic graphic)>, IEnumerable, IEnumerator<(GraphicStateDef state, Graphic graphic)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (GraphicStateDef state, Graphic graphic) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public AnimationDef _003C_003E4__this;

		private PawnRenderNode node;

		public PawnRenderNode _003C_003E3__node;

		private Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator _003C_003E7__wrap1;

		private List<Keyframe>.Enumerator _003C_003E7__wrap2;

		private Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart>.Enumerator _003C_003E7__wrap3;

		private List<CurveKey>.Enumerator _003C_003E7__wrap4;

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
		public _003CGetGraphicStates_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -4:
			case -3:
			case 1:
				try
				{
					if (num == -4 || num == 1)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally2();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -6:
			case -5:
			case 2:
				try
				{
					if (num == -6 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally4();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = default(Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator);
			_003C_003E7__wrap2 = default(List<Keyframe>.Enumerator);
			_003C_003E7__wrap3 = default(Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart>.Enumerator);
			_003C_003E7__wrap4 = default(List<CurveKey>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				AnimationDef animationDef = _003C_003E4__this;
				PawnRenderNodeTagDef key;
				CurveKeyAnimationPart curveKeyAnimationPart;
				KeyframeAnimationPart keyframeAnimationPart;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (animationDef.keyframeParts != null)
					{
						_003C_003E7__wrap1 = animationDef.keyframeParts.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_01c1;
					}
					goto IL_01e3;
				case 1:
					_003C_003E1__state = -4;
					goto IL_019f;
				case 2:
					{
						_003C_003E1__state = -6;
						goto IL_02b9;
					}
					IL_02d8:
					while (_003C_003E7__wrap3.MoveNext())
					{
						_003C_003E7__wrap3.Current.Deconstruct(out key, out CurveKeyAnimationPart value);
						PawnRenderNodeTagDef pawnRenderNodeTagDef = key;
						curveKeyAnimationPart = value;
						if (pawnRenderNodeTagDef != node.Props.tagDef || curveKeyAnimationPart.keyframes.NullOrEmpty())
						{
							continue;
						}
						goto IL_0250;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = default(Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart>.Enumerator);
					break;
					IL_01c1:
					while (_003C_003E7__wrap1.MoveNext())
					{
						_003C_003E7__wrap1.Current.Deconstruct(out key, out KeyframeAnimationPart value2);
						PawnRenderNodeTagDef pawnRenderNodeTagDef2 = key;
						keyframeAnimationPart = value2;
						if (pawnRenderNodeTagDef2 != node.Props.tagDef || keyframeAnimationPart.keyframes.NullOrEmpty())
						{
							continue;
						}
						goto IL_0099;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart>.Enumerator);
					goto IL_01e3;
					IL_0099:
					_003C_003E7__wrap2 = keyframeAnimationPart.keyframes.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_019f;
					IL_019f:
					while (_003C_003E7__wrap2.MoveNext())
					{
						Keyframe current = _003C_003E7__wrap2.Current;
						if (current.graphicState == null || !current.graphicState.TryGetDefaultGraphic(out var graphic))
						{
							continue;
						}
						if (current.graphicState.applySkinColorTint)
						{
							Color baseColor = graphic.Color;
							Color baseColor2 = graphic.ColorTwo;
							Pawn pawn = node.tree.pawn;
							if (pawn.IsMutant)
							{
								baseColor = MutantUtility.GetMutantSkinColor(pawn, baseColor);
								baseColor2 = MutantUtility.GetMutantSkinColor(pawn, baseColor2);
							}
							baseColor = pawn.health.hediffSet.GetSkinColor(baseColor);
							baseColor2 = pawn.health.hediffSet.GetSkinColor(baseColor2);
							graphic = graphic.GetColoredVersion(graphic.Shader, baseColor, baseColor2);
						}
						_003C_003E2__current = (state: current.graphicState, graphic: graphic);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<Keyframe>.Enumerator);
					goto IL_01c1;
					IL_0250:
					_003C_003E7__wrap4 = curveKeyAnimationPart.keyframes.GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_02b9;
					IL_01e3:
					if (animationDef.curveParts == null)
					{
						break;
					}
					_003C_003E7__wrap3 = animationDef.curveParts.GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_02d8;
					IL_02b9:
					while (_003C_003E7__wrap4.MoveNext())
					{
						CurveKey current2 = _003C_003E7__wrap4.Current;
						if (current2.graphicState != null && current2.graphicState.TryGetDefaultGraphic(out var graphic2))
						{
							_003C_003E2__current = (state: current2.graphicState, graphic: graphic2);
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap4 = default(List<CurveKey>.Enumerator);
					goto IL_02d8;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -3;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -5;
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(GraphicStateDef state, Graphic graphic)> IEnumerable<(GraphicStateDef, Graphic)>.GetEnumerator()
		{
			_003CGetGraphicStates_003Ed__9 _003CGetGraphicStates_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetGraphicStates_003Ed__ = this;
			}
			else
			{
				_003CGetGraphicStates_003Ed__ = new _003CGetGraphicStates_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetGraphicStates_003Ed__.node = _003C_003E3__node;
			return _003CGetGraphicStates_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(GraphicStateDef, Graphic)>)this).GetEnumerator();
		}
	}

	public int durationTicks;

	public bool startOnRandomTick;

	public bool playWhenDowned;

	public float posRotScale = 1f;

	public LoopMode loopMode;

	public Dictionary<PawnRenderNodeTagDef, KeyframeAnimationPart> keyframeParts;

	public Dictionary<PawnRenderNodeTagDef, CurveKeyAnimationPart> curveParts;

	public bool TryGetPartForNodeTag(PawnRenderNodeTagDef nodeTag, out AnimationPart part)
	{
		if (keyframeParts != null && keyframeParts.TryGetValue(nodeTag, out var value))
		{
			part = value;
			return true;
		}
		if (curveParts != null && curveParts.TryGetValue(nodeTag, out var value2))
		{
			part = value2;
			return true;
		}
		part = null;
		return false;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__8))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetGraphicStates_003Ed__9))]
	public virtual IEnumerable<(GraphicStateDef state, Graphic graphic)> GetGraphicStates(PawnRenderNode node)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGraphicStates_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__node = node
		};
	}
}
