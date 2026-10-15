using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class CompProperties_GravshipThruster : CompProperties_GravshipFacility
{
	public class ExhaustSettings
	{
		public bool enabled = true;

		private FleckDef exhaustFleckDef;

		public float emissionsPerSecond = 10f;

		public Vector3 spawnOffset = new Vector3(0f, 0f, 0f);

		public FloatRange spawnRadiusRange = new FloatRange(0f);

		public bool inheritGravshipRotation = true;

		public bool inheritThrusterRotation = true;

		public Vector3 velocity = new Vector3(0f, 0f, -1f);

		public FloatRange velocityRotationRange = new FloatRange(0f);

		public FloatRange velocityMultiplierRange = new FloatRange(1f);

		public FloatRange rotationOverTimeRange = new FloatRange(0f);

		public FloatRange scaleRange = new FloatRange(1f);

		public FleckDef ExhaustFleckDef => exhaustFleckDef ?? FleckDefOf.GravshipThrusterExhaust;
	}

	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__13 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompProperties_GravshipThruster _003C_003E4__this;

		private ThingDef parentDef;

		public ThingDef _003C_003E3__parentDef;

		private IEnumerator<string> _003C_003E7__wrap1;

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
		public _003CConfigErrors_003Ed__13(int _003C_003E1__state)
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
				CompProperties_GravshipThruster compProperties_GravshipThruster = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((CompProperties)compProperties_GravshipThruster).ConfigErrors(parentDef).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0078;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0078;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0078:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (compProperties_GravshipThruster.exhaustSettings != null && compProperties_GravshipThruster.exhaustSettings.ExhaustFleckDef != null && compProperties_GravshipThruster.exhaustSettings.ExhaustFleckDef.fleckSystemClass != null && compProperties_GravshipThruster.exhaustSettings.ExhaustFleckDef.fleckSystemClass != typeof(FleckSystemThrown))
					{
						_003C_003E2__current = "exhaust fleck '" + compProperties_GravshipThruster.exhaustSettings.ExhaustFleckDef.defName + "' uses a fleckSystemClass other than FleckSystemThrown";
						_003C_003E1__state = 2;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__13 _003CConfigErrors_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CConfigErrors_003Ed__ = this;
			}
			else
			{
				_003CConfigErrors_003Ed__ = new _003CConfigErrors_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CConfigErrors_003Ed__.parentDef = _003C_003E3__parentDef;
			return _003CConfigErrors_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	public IntVec3 exclusionAreaSize;

	public IntVec3 exclusionAreaOffset;

	public int directionInfluence;

	public List<Vector3> flameOffsetsPerDirection = new List<Vector3>
	{
		Vector3.zero,
		Vector3.zero,
		Vector3.zero,
		Vector3.zero
	};

	public float flameSize;

	private ShaderTypeDef flameShaderType;

	public List<ShaderParameter> flameShaderParameters = new List<ShaderParameter>();

	public ExhaustSettings exhaustSettings = new ExhaustSettings();

	public ShaderTypeDef FlameShaderType => flameShaderType ?? ShaderTypeDefOf.MoteGlow;

	public CompProperties_GravshipThruster()
	{
		compClass = typeof(CompGravshipThruster);
	}

	public void GetExclusionZone(IntVec3 pos, Rot4 rot, ref List<IntVec3> exclusionCells)
	{
		if (exclusionCells == null)
		{
			exclusionCells = new List<IntVec3>();
		}
		else
		{
			exclusionCells.Clear();
		}
		for (int i = 0; i < exclusionAreaSize.x; i++)
		{
			for (int j = 0; j < exclusionAreaSize.z; j++)
			{
				exclusionCells.Add(pos + new IntVec3(i, 0, j).RotatedBy(rot) + exclusionAreaOffset.RotatedBy(rot));
			}
		}
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__13))]
	public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__13(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__parentDef = parentDef
		};
	}
}
