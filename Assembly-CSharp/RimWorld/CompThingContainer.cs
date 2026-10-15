using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompThingContainer : ThingComp, IThingHolder, ISearchableContents
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__27 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompThingContainer _003C_003E4__this;

		private IEnumerator<Thing> _003C_003E7__wrap1;

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
		public _003CCompGetGizmosExtra_003Ed__27(int _003C_003E1__state)
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
				CompThingContainer compThingContainer = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((IEnumerable<Thing>)compThingContainer.innerContainer).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					Gizmo gizmo = Building.SelectContainedItemGizmo(compThingContainer.parent, current);
					if (gizmo != null)
					{
						_003C_003E2__current = gizmo;
						_003C_003E1__state = 1;
						return true;
					}
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
			_003CCompGetGizmosExtra_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__27(0)
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

	[CompilerGenerated]
	private sealed class _003CSpecialDisplayStats_003Ed__23 : IEnumerable<StatDrawEntry>, IEnumerable, IEnumerator<StatDrawEntry>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private StatDrawEntry _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompThingContainer _003C_003E4__this;

		StatDrawEntry IEnumerator<StatDrawEntry>.Current
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
		public _003CSpecialDisplayStats_003Ed__23(int _003C_003E1__state)
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
			CompThingContainer compThingContainer = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!compThingContainer.Empty)
				{
					_003C_003E2__current = new StatDrawEntry(StatCategoryDefOf.Building, "Contents".Translate(), compThingContainer.LabelCapWithTotalCount, compThingContainer.LabelCapWithTotalCount, 1200, null, Gen.YieldSingle(new Dialog_InfoCard.Hyperlink(compThingContainer.ContainedThing)));
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
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
		IEnumerator<StatDrawEntry> IEnumerable<StatDrawEntry>.GetEnumerator()
		{
			_003CSpecialDisplayStats_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CSpecialDisplayStats_003Ed__23(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<StatDrawEntry>)this).GetEnumerator();
		}
	}

	public ThingOwner innerContainer;

	public CompProperties_ThingContainer Props => (CompProperties_ThingContainer)props;

	public Thing ContainedThing
	{
		get
		{
			if (!innerContainer.Any)
			{
				return null;
			}
			return innerContainer[0];
		}
	}

	public bool Empty => ContainedThing == null;

	public int TotalStackCount
	{
		get
		{
			if (!Empty)
			{
				return innerContainer.TotalStackCountOfDef(innerContainer[0].def);
			}
			return 0;
		}
	}

	public bool Full
	{
		get
		{
			if (!Empty)
			{
				return TotalStackCount >= Props.stackLimit;
			}
			return false;
		}
	}

	public string LabelCapWithTotalCount
	{
		get
		{
			if (!Empty)
			{
				return ContainedThing.LabelCapNoCount + " x" + TotalStackCount.ToStringCached();
			}
			return null;
		}
	}

	public ThingOwner SearchableContents => innerContainer;

	public CompThingContainer()
	{
		innerContainer = new ThingOwner<Thing>(this);
	}

	public virtual bool Accepts(ThingDef thingDef)
	{
		if (!Empty)
		{
			if (TotalStackCount < Props.stackLimit)
			{
				return thingDef == ContainedThing.def;
			}
			return false;
		}
		return true;
	}

	public virtual bool Accepts(Thing thing)
	{
		return Accepts(thing.def);
	}

	public override void CompTick()
	{
		innerContainer.DoTick();
	}

	public override void PostDraw()
	{
		if (!Empty && Props.drawContainedThing)
		{
			ContainedThing.DrawNowAt((parent.Position + Props.containedThingOffset.RotatedBy(parent.Rotation)).ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop));
		}
	}

	public override void DrawGUIOverlay()
	{
		if (parent.Spawned && Props.drawStackLabel && !Empty && Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
		{
			GenMapUI.DrawThingLabel(parent, TotalStackCount.ToStringCached());
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			innerContainer.TryDropAll(parent.Position, map, ThingPlaceMode.Near);
		}
	}

	public override string CompInspectStringExtra()
	{
		return "Contents".Translate() + ": " + (Empty ? ((string)"Nothing".Translate()) : LabelCapWithTotalCount);
	}

	[IteratorStateMachine(typeof(_003CSpecialDisplayStats_003Ed__23))]
	public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CSpecialDisplayStats_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__27))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}
}
