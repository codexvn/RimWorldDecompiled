using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld;

public class MinifiedTree : MinifiedThing, IThingHolderTickable, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__16 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MinifiedTree _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__16(int _003C_003E1__state)
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
				MinifiedTree CS_0024_003C_003E8__locals4 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((MinifiedThing)CS_0024_003C_003E8__locals4).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007a;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Die in 1 hour",
						action = delegate
						{
							CS_0024_003C_003E8__locals4.ticksTillDeath = 2500;
						}
					};
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Die in 1 day",
						action = delegate
						{
							CS_0024_003C_003E8__locals4.ticksTillDeath = 60000;
						}
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Destroy",
							action = delegate
							{
								CS_0024_003C_003E8__locals4.Destroy();
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__16(0)
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

	private int ticksTillDeath;

	public const int InitialTicksTillDeath = 420000;

	public const float DyingYieldPercentage = 0.5f;

	public bool ShouldTickContents => false;

	public int TicksTillDeath => ticksTillDeath;

	public Plant InnerTree => (Plant)base.InnerThing;

	public override Graphic Graphic
	{
		get
		{
			if (cachedGraphic == null)
			{
				cachedGraphic = base.InnerThing.Graphic.ExtractInnerGraphicFor(base.InnerThing, null);
				Vector2 minifiedDrawSize = GetMinifiedDrawSize(base.InnerThing.def.size.ToVector2(), 1.1f);
				Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)base.InnerThing.def.size.x * cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)base.InnerThing.def.size.z * cachedGraphic.drawSize.y);
				cachedGraphic = cachedGraphic.GetCopy(newDrawSize, ShaderTypeDefOf.Cutout.Shader);
			}
			return cachedGraphic;
		}
	}

	protected override Graphic LoadCrateFrontGraphic()
	{
		return GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/BurlapBag", ShaderDatabase.Cutout, GetMinifiedDrawSize(base.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
	}

	private void RecordTreeDeath(LookTargets lookTargets)
	{
		if (base.InnerThing.def.plant.treeLoversCareIfChopped)
		{
			foreach (Ideo allIdeo in Faction.OfPlayer.ideos.AllIdeos)
			{
				if (allIdeo.WarnPlayerOnDesignateChopTree)
				{
					Messages.Message("MessageMinifiedTreeDied".Translate(), lookTargets, MessageTypeDefOf.NegativeEvent);
					break;
				}
			}
		}
		Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.MinifiedTreeDied, new SignalArgs(base.InnerThing.Named(HistoryEventArgsNames.Victim))));
	}

	public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
	{
		if (base.InnerThing == null)
		{
			base.Destroy(mode);
			return;
		}
		Caravan anyParent = ThingOwnerUtility.GetAnyParent<Caravan>(this);
		ActiveTransporterInfo activeTransporterInfo = base.ParentHolder as ActiveTransporterInfo;
		RecordTreeDeath((anyParent == null) ? new LookTargets(base.PositionHeld, base.MapHeld) : ((LookTargets)anyParent));
		ThingDef harvestedThingDef = base.InnerThing.def.plant.harvestedThingDef;
		int num = (int)((float)((Plant)base.InnerThing).YieldNow() * 0.5f);
		List<Thing> list = new List<Thing>();
		while (num > 0)
		{
			int num2 = Mathf.Min(num, harvestedThingDef.stackLimit);
			Thing thing = ThingMaker.MakeThing(harvestedThingDef);
			thing.stackCount = num2;
			list.Add(thing);
			num -= num2;
		}
		IntVec3 center = ((anyParent == null) ? base.PositionHeld : IntVec3.Invalid);
		Map map = ((anyParent == null) ? base.MapHeld : null);
		if (base.ParentHolder != null)
		{
			base.ParentHolder.GetDirectlyHeldThings().Remove(this);
		}
		base.Destroy(mode);
		if (anyParent != null)
		{
			foreach (Thing item in list)
			{
				anyParent.AddPawnOrItem(item, addCarriedPawnToWorldPawnsIfAny: true);
			}
			return;
		}
		if (activeTransporterInfo != null)
		{
			foreach (Thing item2 in list)
			{
				activeTransporterInfo.innerContainer.TryAdd(item2);
			}
			return;
		}
		if (map == null)
		{
			return;
		}
		foreach (Thing item3 in list)
		{
			GenPlace.TryPlaceThing(item3, center, map, ThingPlaceMode.Near, null, null, null);
		}
	}

	public override void PostMake()
	{
		base.PostMake();
		ticksTillDeath = 420000;
	}

	protected override void Tick()
	{
		base.Tick();
		ticksTillDeath--;
		if (ticksTillDeath <= 0)
		{
			Destroy();
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__16))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_MyMapRemoved()
	{
		base.Notify_MyMapRemoved();
		RecordTreeDeath(null);
	}

	public override string GetInspectString()
	{
		return "MinifiedTreeWillDieIn".Translate(ticksTillDeath.ToStringTicksToPeriod().Named("time"));
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksTillDeath, "ticksTillDeath", 0);
	}
}
