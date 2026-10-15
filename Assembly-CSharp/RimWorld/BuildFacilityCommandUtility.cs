using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class BuildFacilityCommandUtility
{
	[CompilerGenerated]
	private sealed class _003CBuildFacilityCommands_003Ed__0 : IEnumerable<Command>, IEnumerable, IEnumerator<Command>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Command _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private BuildableDef building;

		public BuildableDef _003C_003E3__building;

		private CompProperties_AffectedByFacilities _003CaffectedByFacilities_003E5__2;

		private int _003Ci_003E5__3;

		Command IEnumerator<Command>.Current
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
		public _003CBuildFacilityCommands_003Ed__0(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CaffectedByFacilities_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_007e;
			}
			_003C_003E1__state = -1;
			if (!(building is ThingDef thingDef))
			{
				return false;
			}
			_003CaffectedByFacilities_003E5__2 = thingDef.GetCompProperties<CompProperties_AffectedByFacilities>();
			if (_003CaffectedByFacilities_003E5__2 == null)
			{
				return false;
			}
			_003Ci_003E5__3 = 0;
			goto IL_008e;
			IL_008e:
			if (_003Ci_003E5__3 < _003CaffectedByFacilities_003E5__2.linkableFacilities.Count)
			{
				Designator_Build designator_Build = BuildCopyCommandUtility.FindAllowedDesignator(_003CaffectedByFacilities_003E5__2.linkableFacilities[_003Ci_003E5__3]);
				if (designator_Build != null)
				{
					_003C_003E2__current = designator_Build;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_007e;
			}
			return false;
			IL_007e:
			_003Ci_003E5__3++;
			goto IL_008e;
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
		IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
		{
			_003CBuildFacilityCommands_003Ed__0 _003CBuildFacilityCommands_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CBuildFacilityCommands_003Ed__ = this;
			}
			else
			{
				_003CBuildFacilityCommands_003Ed__ = new _003CBuildFacilityCommands_003Ed__0(0);
			}
			_003CBuildFacilityCommands_003Ed__.building = _003C_003E3__building;
			return _003CBuildFacilityCommands_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Command>)this).GetEnumerator();
		}
	}

	[IteratorStateMachine(typeof(_003CBuildFacilityCommands_003Ed__0))]
	public static IEnumerable<Command> BuildFacilityCommands(BuildableDef building)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CBuildFacilityCommands_003Ed__0(-2)
		{
			_003C_003E3__building = building
		};
	}
}
