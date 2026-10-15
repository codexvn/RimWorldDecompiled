using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class PawnDrawUtility
{
	[CompilerGenerated]
	private sealed class _003CFindAnchors_003Ed__3 : IEnumerable<BodyTypeDef.WoundAnchor>, IEnumerable, IEnumerator<BodyTypeDef.WoundAnchor>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private BodyTypeDef.WoundAnchor _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private BodyPartRecord curPart;

		public BodyPartRecord _003C_003E3__curPart;

		private int _003Citerations_003E5__2;

		private int _003Cfound_003E5__3;

		private List<BodyTypeDef.WoundAnchor>.Enumerator _003C_003E7__wrap3;

		BodyTypeDef.WoundAnchor IEnumerator<BodyTypeDef.WoundAnchor>.Current
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
		public _003CFindAnchors_003Ed__3(int _003C_003E1__state)
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
			_003C_003E7__wrap3 = default(List<BodyTypeDef.WoundAnchor>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (pawn.story == null || pawn.story.bodyType == null || pawn.story.bodyType.woundAnchors.NullOrEmpty())
					{
						return false;
					}
					_003Citerations_003E5__2 = 0;
					_003Cfound_003E5__3 = 0;
					goto IL_01e4;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0107;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_01a4;
					}
					IL_01a4:
					while (_003C_003E7__wrap3.MoveNext())
					{
						BodyTypeDef.WoundAnchor current = _003C_003E7__wrap3.Current;
						if (curPart.IsInGroup(current.group))
						{
							_003Cfound_003E5__3++;
							_003C_003E2__current = current;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<BodyTypeDef.WoundAnchor>.Enumerator);
					goto IL_01c3;
					IL_0107:
					while (_003C_003E7__wrap3.MoveNext())
					{
						BodyTypeDef.WoundAnchor current2 = _003C_003E7__wrap3.Current;
						if (current2.tag == curPart.woundAnchorTag)
						{
							_003Cfound_003E5__3++;
							_003C_003E2__current = current2;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = default(List<BodyTypeDef.WoundAnchor>.Enumerator);
					goto IL_01c3;
					IL_01e4:
					if (_003Cfound_003E5__3 != 0 || curPart == null || _003Citerations_003E5__2 >= 100)
					{
						break;
					}
					if (curPart.woundAnchorTag != null)
					{
						_003C_003E7__wrap3 = pawn.story.bodyType.woundAnchors.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0107;
					}
					_003C_003E7__wrap3 = pawn.story.bodyType.woundAnchors.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01a4;
					IL_01c3:
					curPart = curPart.parent;
					_003Citerations_003E5__2++;
					goto IL_01e4;
				}
				if (_003Citerations_003E5__2 == 100)
				{
					Log.Error("PawnWoundDrawer.RenderOverBody.FindAnchors while() loop ran into iteration limit! This is never supposed to happen! Is there a cyclic body part parent reference?");
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
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<BodyTypeDef.WoundAnchor> IEnumerable<BodyTypeDef.WoundAnchor>.GetEnumerator()
		{
			_003CFindAnchors_003Ed__3 _003CFindAnchors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFindAnchors_003Ed__ = this;
			}
			else
			{
				_003CFindAnchors_003Ed__ = new _003CFindAnchors_003Ed__3(0);
			}
			_003CFindAnchors_003Ed__.pawn = _003C_003E3__pawn;
			_003CFindAnchors_003Ed__.curPart = _003C_003E3__curPart;
			return _003CFindAnchors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<BodyTypeDef.WoundAnchor>)this).GetEnumerator();
		}
	}

	public const string AnchorTagEyeLeft = "LeftEye";

	public const string AnchorTagEyeRight = "RightEye";

	public static bool AnchorUsable(Pawn pawn, BodyTypeDef.WoundAnchor anchor, Rot4 pawnRot)
	{
		if ((!anchor.rotation.HasValue || anchor.rotation.Value == pawnRot || (anchor.canMirror && anchor.rotation.Value == pawnRot.Opposite)) && (!anchor.narrowCrown.HasValue || (pawn.story != null && pawn.story.headType.narrow == anchor.narrowCrown.Value)))
		{
			if (!pawn.health.hediffSet.HasHead)
			{
				return anchor.layer != PawnOverlayDrawer.OverlayLayer.Head;
			}
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CFindAnchors_003Ed__3))]
	public static IEnumerable<BodyTypeDef.WoundAnchor> FindAnchors(Pawn pawn, BodyPartRecord curPart)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFindAnchors_003Ed__3(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__curPart = curPart
		};
	}

	public static void CalcAnchorData(Pawn pawn, BodyTypeDef.WoundAnchor anchor, Rot4 pawnRot, out Vector3 anchorOffset, out float range)
	{
		anchorOffset = anchor.offset;
		if (anchor.rotation == pawnRot.Opposite)
		{
			anchorOffset.x *= -1f;
		}
		if ((anchor.tag == "LeftEye" || anchor.tag == "RightEye") && pawnRot.IsHorizontal && (!ModsConfig.BiotechActive || !pawn.DevelopmentalStage.Juvenile()))
		{
			Vector3? eyeOffsetEastWest = pawn.story.headType.eyeOffsetEastWest;
			if (eyeOffsetEastWest.HasValue)
			{
				if (pawnRot == Rot4.East)
				{
					anchorOffset = eyeOffsetEastWest.Value;
				}
				else
				{
					anchorOffset = eyeOffsetEastWest.Value.ScaledBy(new Vector3(-1f, 1f, 1f));
				}
			}
		}
		range = anchor.range;
	}
}
