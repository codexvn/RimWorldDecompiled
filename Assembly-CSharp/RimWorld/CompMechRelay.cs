using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class CompMechRelay : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__12 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMechRelay _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__12(int _003C_003E1__state)
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
				CompMechRelay CS_0024_003C_003E8__locals6 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals6).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0076;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0076;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0076:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!DebugSettings.ShowDevGizmos || CS_0024_003C_003E8__locals6.inert)
					{
						break;
					}
					if (CS_0024_003C_003E8__locals6.active)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Deactivate",
							action = CS_0024_003C_003E8__locals6.Deactivate
						};
						_003C_003E1__state = 2;
						return true;
					}
					if (CS_0024_003C_003E8__locals6.destabilizationTick > 0)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Destabilize now",
							action = delegate
							{
								CS_0024_003C_003E8__locals6.destabilizationTick = Find.TickManager.TicksGame;
							}
						};
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__12(0)
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

	public const int DestabilizationTicks = 7500;

	private const int CrashFlecksPerCell = 1;

	private bool active = true;

	public bool inert;

	private int destabilizationTick = -1;

	public CompProperties_MechRelay Props => (CompProperties_MechRelay)props;

	public int DestabilizationTick => destabilizationTick;

	public void Deactivate()
	{
		if (active && !inert)
		{
			active = false;
			if (!Props.destabilizationMessage.NullOrEmpty())
			{
				Messages.Message(Props.destabilizationMessage, parent, MessageTypeDefOf.PositiveEvent);
			}
			destabilizationTick = Find.TickManager.TicksGame + 7500;
		}
	}

	public override void CompTick()
	{
		if (inert || destabilizationTick <= 0 || Find.TickManager.TicksGame < destabilizationTick)
		{
			return;
		}
		destabilizationTick = -1;
		Map map = parent.Map;
		IntVec3 position = parent.Position;
		parent.Destroy();
		if (Props.crashedThingDef != null)
		{
			Thing thing = ThingMaker.MakeThing(Props.crashedThingDef);
			GenPlace.TryPlaceThing(thing, position, map, ThingPlaceMode.Direct, null, null, null);
			CellRect cellRect = parent.OccupiedRect();
			for (int i = 0; i < cellRect.Area; i++)
			{
				FleckMaker.ThrowDustPuff(cellRect.RandomVector3.WithY(AltitudeLayer.MoteLow.AltitudeFor()), map, 2f);
			}
			if (thing.Faction != parent.Faction)
			{
				thing.SetFaction(parent.Faction);
			}
			if (!Props.crashedLetterLabel.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(Props.crashedLetterLabel, Props.crashedLetterText, LetterDefOf.NeutralEvent, thing);
			}
		}
	}

	public override string CompInspectStringExtra()
	{
		if (inert)
		{
			return null;
		}
		if (active && !Props.activeInspectString.NullOrEmpty())
		{
			return Props.activeInspectString;
		}
		if (destabilizationTick > 0 && !Props.destabilizationInspectString.NullOrEmpty())
		{
			return Props.destabilizationInspectString + ": " + (destabilizationTick - Find.TickManager.TicksGame).ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor);
		}
		return base.CompInspectStringExtra();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__12))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref active, "active", defaultValue: true);
		Scribe_Values.Look(ref inert, "inert", defaultValue: false);
		Scribe_Values.Look(ref destabilizationTick, "destabilizationTick", -1);
	}
}
