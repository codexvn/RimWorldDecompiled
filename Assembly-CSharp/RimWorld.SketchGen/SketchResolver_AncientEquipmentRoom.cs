using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.SketchGen;

public class SketchResolver_AncientEquipmentRoom : SketchResolver
{
	[CompilerGenerated]
	private sealed class _003Cget_CentralThings_003Ed__1 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003Cget_CentralThings_003Ed__1(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientMachine;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientStorageCylinder;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_CentralThings_003Ed__1(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_EdgeThings_003Ed__3 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003Cget_EdgeThings_003Ed__3(int _003C_003E1__state)
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
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientSystemRack;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientEquipmentBlocks;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				return false;
			}
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
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_EdgeThings_003Ed__3(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private static IEnumerable<ThingDef> CentralThings
	{
		[IteratorStateMachine(typeof(_003Cget_CentralThings_003Ed__1))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_CentralThings_003Ed__1(-2);
		}
	}

	private static IEnumerable<ThingDef> EdgeThings
	{
		[IteratorStateMachine(typeof(_003Cget_EdgeThings_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_EdgeThings_003Ed__3(-2);
		}
	}

	protected override bool CanResolveInt(SketchResolveParams parms)
	{
		if (parms.rect.HasValue)
		{
			return parms.sketch != null;
		}
		return false;
	}

	protected override void ResolveInt(SketchResolveParams parms)
	{
		if (!ModLister.CheckIdeology("Ancient equipment room"))
		{
			return;
		}
		SketchResolveParams parms2 = parms;
		parms2.cornerThing = ThingDefOf.AncientLamp;
		parms2.requireFloor = true;
		SketchResolverDefOf.AddCornerThings.Resolve(parms2);
		foreach (ThingDef centralThing in CentralThings)
		{
			SketchResolveParams parms3 = parms;
			parms3.thingCentral = centralThing;
			parms3.requireFloor = true;
			SketchResolverDefOf.AddThingsCentral.Resolve(parms3);
		}
		foreach (ThingDef edgeThing in EdgeThings)
		{
			SketchResolveParams parms4 = parms;
			parms4.wallEdgeThing = edgeThing;
			parms4.requireFloor = true;
			SketchResolverDefOf.AddWallEdgeThings.Resolve(parms4);
		}
	}
}
