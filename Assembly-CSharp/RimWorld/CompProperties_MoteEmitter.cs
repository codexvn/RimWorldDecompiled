using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompProperties_MoteEmitter : CompProperties
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__20 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_MoteEmitter _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__20(int _003C_003E1__state)
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
			CompProperties_MoteEmitter compProperties_MoteEmitter = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (compProperties_MoteEmitter.mote == null && compProperties_MoteEmitter.perRotationMotes.NullOrEmpty())
				{
					_003C_003E2__current = "CompMoteEmitter must have a mote assigned.";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0059;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0059;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0059:
				if (!compProperties_MoteEmitter.perRotationMotes.NullOrEmpty() && compProperties_MoteEmitter.perRotationMotes.Count != 4)
				{
					_003C_003E2__current = "perRotationMotes must contain 4 elements for North, East, South, West";
					_003C_003E1__state = 2;
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
			_003CConfigErrors_003Ed__20 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__20(0)
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

	public ThingDef mote;

	public List<ThingDef> perRotationMotes;

	public Vector3 offset;

	public Vector3 offsetMin = Vector3.zero;

	public Vector3 offsetMax = Vector3.zero;

	public Vector3 offsetNorth = Vector3.zero;

	public Vector3 offsetSouth = Vector3.zero;

	public Vector3 offsetEast = Vector3.zero;

	public Vector3 offsetWest = Vector3.zero;

	public bool useParentRotation;

	public SoundDef soundOnEmission;

	public int emissionInterval = -1;

	public int ticksSinceLastEmittedMaxOffset;

	public bool maintain;

	[NoTranslate]
	public string saveKeysPrefix;

	public Vector3 EmissionOffset => new Vector3(Rand.Range(offsetMin.x, offsetMax.x), Rand.Range(offsetMin.y, offsetMax.y), Rand.Range(offsetMin.z, offsetMax.z));

	public CompProperties_MoteEmitter()
	{
		compClass = typeof(CompMoteEmitter);
	}

	public Vector3 RotationOffset(Rot4 rot)
	{
		return rot.AsInt switch
		{
			0 => offsetNorth, 
			1 => offsetEast, 
			2 => offsetSouth, 
			3 => offsetWest, 
			_ => Vector3.zero, 
		};
	}

	public ThingDef RotationMote(Rot4 rot)
	{
		return perRotationMotes?[rot.AsInt];
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__20))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__20(-2)
		{
			_003C_003E4__this = this
		};
	}
}
