using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class FleckDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__29 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public FleckDef _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__29(int _003C_003E1__state)
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
			FleckDef fleckDef = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (fleckDef.fleckSystemClass == null)
				{
					_003C_003E2__current = "FleckDef without system class type set!";
					_003C_003E1__state = 1;
					return true;
				}
				if (!typeof(FleckSystem).IsAssignableFrom(fleckDef.fleckSystemClass))
				{
					_003C_003E2__current = "FleckDef has system class type assigned which is not assignable to FleckSystemBase!";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_0092;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0092;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0092;
			case 3:
				_003C_003E1__state = -1;
				break;
			case 4:
				_003C_003E1__state = -1;
				break;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0092:
				if (fleckDef.graphicData == null && fleckDef.randomGraphics.NullOrEmpty())
				{
					_003C_003E2__current = "Fleck graphic data and random graphics are null!";
					_003C_003E1__state = 3;
					return true;
				}
				if (fleckDef.graphicData != null && !typeof(Graphic_Fleck).IsAssignableFrom(fleckDef.graphicData.graphicClass))
				{
					_003C_003E2__current = "Fleck graphic class is not derived from Graphic_Fleck!";
					_003C_003E1__state = 4;
					return true;
				}
				if (!fleckDef.randomGraphics.NullOrEmpty() && fleckDef.randomGraphics.Any((GraphicData g) => !typeof(Graphic_Fleck).IsAssignableFrom(g.graphicClass)))
				{
					_003C_003E2__current = "random fleck graphic class is not derived from Graphic_Fleck!";
					_003C_003E1__state = 5;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__29 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__29(0)
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

	public Type fleckSystemClass;

	public AltitudeLayer altitudeLayer;

	public float altitudeLayerIncOffset;

	public bool drawGUIOverlay;

	public GraphicData graphicData;

	public List<GraphicData> randomGraphics;

	public bool realTime;

	public bool attachedToHead;

	public bool useAttachLink;

	public float fadeInTime;

	public float solidTime = 1f;

	public float fadeOutTime;

	public Vector3 acceleration = Vector3.zero;

	public FloatRange speedPerTime = FloatRange.Zero;

	public float growthRate;

	public List<CurvedScaler> scalers;

	public bool collide;

	public SoundDef landSound;

	public Vector3 unattachedDrawOffset;

	public Vector3 attachedDrawOffset;

	public bool rotateTowardsMoveDirection;

	public float rotateTowardsMoveDirectionExtraAngle;

	public bool drawOffscreen;

	public FloatRange archHeight = FloatRange.Zero;

	public FloatRange archDuration;

	public SimpleCurve archCurve;

	public Vector3 scalingAnchor = Vector3.one * 0.5f;

	public float Lifetime => fadeInTime + solidTime + fadeOutTime;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__29))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__29(-2)
		{
			_003C_003E4__this = this
		};
	}

	public GraphicData GetGraphicData(int id)
	{
		if (graphicData != null)
		{
			return graphicData;
		}
		Rand.PushState(id);
		try
		{
			return randomGraphics.RandomElement();
		}
		finally
		{
			Rand.PopState();
		}
	}
}
