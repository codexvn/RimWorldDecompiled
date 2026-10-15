using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.Sound;

namespace RimWorld;

public class CompToxifier : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass11_0
	{
		public int ticks;

		public CompToxifier _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__3()
		{
			_003C_003E4__this.pollutingProgressTicks = _003C_003E4__this.Props.pollutionIntervalTicks - ticks;
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__11 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompToxifier _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__11(int _003C_003E1__state)
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
			CompToxifier CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (DebugSettings.ShowDevGizmos)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Pollute",
						action = delegate
						{
							CS_0024_003C_003E8__locals12.PolluteNextCell();
						},
						Disabled = !CS_0024_003C_003E8__locals12.CanPolluteNow
					};
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Pollute all",
					action = delegate
					{
						while (CS_0024_003C_003E8__locals12.CanPolluteNow)
						{
							CS_0024_003C_003E8__locals12.PolluteNextCell(silent: true);
						}
					},
					Disabled = !CS_0024_003C_003E8__locals12.CanPolluteNow
				};
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Set next pollution time",
					action = delegate
					{
						List<FloatMenuOption> list = new List<FloatMenuOption>();
						int[] array = new int[11]
						{
							60, 120, 180, 240, 300, 600, 900, 1200, 1500, 1800,
							3600
						};
						for (int i = 0; i < array.Length; i++)
						{
							_003C_003Ec__DisplayClass11_0 CS_0024_003C_003E8__locals6 = new _003C_003Ec__DisplayClass11_0
							{
								_003C_003E4__this = CS_0024_003C_003E8__locals12,
								ticks = array[i]
							};
							list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals6.ticks.ToStringSecondsFromTicks("F0"), delegate
							{
								CS_0024_003C_003E8__locals6._003C_003E4__this.pollutingProgressTicks = CS_0024_003C_003E8__locals6._003C_003E4__this.Props.pollutionIntervalTicks - CS_0024_003C_003E8__locals6.ticks;
							}));
						}
						Find.WindowStack.Add(new FloatMenu(list));
					},
					Disabled = !CS_0024_003C_003E8__locals12.CanPolluteNow
				};
				_003C_003E1__state = 3;
				return true;
			case 3:
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__11 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__11(0)
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

	private IntVec3 nextPollutionCell = IntVec3.Invalid;

	private int pollutingProgressTicks;

	private CompProperties_Toxifier Props => (CompProperties_Toxifier)props;

	public bool CanPolluteNow
	{
		get
		{
			if (!parent.Spawned)
			{
				return false;
			}
			if (!nextPollutionCell.CanPollute(parent.Map))
			{
				UpdateNextPolluteCell();
			}
			return nextPollutionCell.CanPollute(parent.Map);
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (!ModLister.CheckBiotech("Toxifier"))
		{
			parent.Destroy();
			return;
		}
		base.PostSpawnSetup(respawningAfterLoad);
		if (!respawningAfterLoad && !parent.BeingTransportedOnGravship)
		{
			pollutingProgressTicks = Props.pollutionIntervalTicks;
		}
	}

	public override string CompInspectStringExtra()
	{
		TaggedString taggedString = base.CompInspectStringExtra();
		if (CanPolluteNow)
		{
			if (!taggedString.NullOrEmpty())
			{
				taggedString += "\n";
			}
			taggedString += string.Concat("PollutingTerrainProgress".Translate() + ": " + (Props.pollutionIntervalTicks - pollutingProgressTicks).ToStringTicksToPeriod() + " (", Props.cellsToPollute.ToString(), " ") + "TilesLower".Translate() + ")";
		}
		return taggedString.Resolve();
	}

	private void UpdateNextPolluteCell()
	{
		if (nextPollutionCell.CanPollute(parent.Map))
		{
			return;
		}
		nextPollutionCell = IntVec3.Invalid;
		int num = GenRadial.NumCellsInRadius(Props.radius);
		for (int i = 0; i < num; i++)
		{
			IntVec3 c2 = parent.Position + GenRadial.RadialPattern[i];
			if (NextPolluteCellValidator(c2))
			{
				nextPollutionCell = c2;
				break;
			}
		}
		bool NextPolluteCellValidator(IntVec3 c)
		{
			if (!c.InBounds(parent.Map))
			{
				return false;
			}
			if (!c.CanPollute(parent.Map))
			{
				return false;
			}
			return true;
		}
	}

	private void PolluteNextCell(bool silent = false)
	{
		if (!CanPolluteNow)
		{
			return;
		}
		int num = GenRadial.NumCellsInRadius(Props.radius);
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			IntVec3 c = parent.Position + GenRadial.RadialPattern[i];
			if (c.InBounds(parent.Map) && c.CanPollute(parent.Map))
			{
				c.Pollute(parent.Map, silent);
				num2++;
				if (num2 >= Props.cellsToPollute)
				{
					break;
				}
			}
		}
		if (!silent)
		{
			DoEffects();
		}
	}

	private void DoEffects()
	{
		FleckMaker.Static(parent.TrueCenter(), parent.Map, FleckDefOf.Fleck_ToxifierPollutionSource);
		SoundDefOf.Toxifier_Pollute.PlayOneShot(parent);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__11))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__11(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void CompTickInterval(int delta)
	{
		if (CanPolluteNow)
		{
			pollutingProgressTicks += delta;
			if (pollutingProgressTicks >= Props.pollutionIntervalTicks)
			{
				pollutingProgressTicks = 0;
				PolluteNextCell();
			}
		}
	}

	public override void PostExposeData()
	{
		Scribe_Values.Look(ref nextPollutionCell, "nextPollutionCell", IntVec3.Invalid);
		Scribe_Values.Look(ref pollutingProgressTicks, "pollutingProgressTicks", 0);
	}
}
