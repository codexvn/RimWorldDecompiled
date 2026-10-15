using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompMannable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass10_0
	{
		public CompMannable _003C_003E4__this;

		public Pawn pawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			Job job = JobMaker.MakeJob(JobDefOf.ManTurret, _003C_003E4__this.parent);
			pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__10 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompMannable _003C_003E4__this;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompFloatMenuOptions_003Ed__10(int _003C_003E1__state)
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
			CompMannable compMannable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass10_0 CS_0024_003C_003E8__locals9 = new _003C_003Ec__DisplayClass10_0
				{
					_003C_003E4__this = _003C_003E4__this,
					pawn = pawn
				};
				if (!CS_0024_003C_003E8__locals9.pawn.RaceProps.ToolUser)
				{
					return false;
				}
				if (!CS_0024_003C_003E8__locals9.pawn.CanReserveAndReach(compMannable.parent, PathEndMode.InteractionCell, Danger.Deadly))
				{
					return false;
				}
				if (compMannable.Props.manWorkType != 0 && CS_0024_003C_003E8__locals9.pawn.WorkTagIsDisabled(compMannable.Props.manWorkType))
				{
					if (compMannable.Props.manWorkType == WorkTags.Violent)
					{
						_003C_003E2__current = new FloatMenuOption("CannotManThing".Translate(compMannable.parent.LabelShort, compMannable.parent) + " (" + "IsIncapableOfViolenceLower".Translate(CS_0024_003C_003E8__locals9.pawn.LabelShort, CS_0024_003C_003E8__locals9.pawn) + ")", null);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_014b;
				}
				if (!compMannable.Props.planetLayerWhitelist.NullOrEmpty() && !compMannable.Props.planetLayerWhitelist.Contains(CS_0024_003C_003E8__locals9.pawn.Map.Tile.LayerDef))
				{
					_003C_003E2__current = new FloatMenuOption("CannotManThing".Translate(compMannable.parent.LabelShort, compMannable.parent) + " (" + "CannotFunctionOnLayer".Translate(CS_0024_003C_003E8__locals9.pawn.Map.Tile.LayerDef.label) + ")", null);
					_003C_003E1__state = 2;
					return true;
				}
				FloatMenuOption floatMenuOption = new FloatMenuOption("OrderManThing".Translate(compMannable.parent.LabelShort, compMannable.parent), delegate
				{
					Job job = JobMaker.MakeJob(JobDefOf.ManTurret, CS_0024_003C_003E8__locals9._003C_003E4__this.parent);
					CS_0024_003C_003E8__locals9.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
				});
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 3;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_014b;
			case 2:
				_003C_003E1__state = -1;
				return false;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_014b:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__10 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__10(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.pawn = _003C_003E3__pawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private int lastManTick = -1;

	private Pawn lastManPawn;

	public bool MannedNow
	{
		get
		{
			if (Find.TickManager.TicksGame - lastManTick <= 1 && lastManPawn != null)
			{
				return lastManPawn.Spawned;
			}
			return false;
		}
	}

	public Pawn ManningPawn
	{
		get
		{
			if (!MannedNow)
			{
				return null;
			}
			return lastManPawn;
		}
	}

	public CompProperties_Mannable Props => (CompProperties_Mannable)props;

	public void ManForATick(Pawn pawn)
	{
		lastManTick = Find.TickManager.TicksGame;
		lastManPawn = pawn;
		pawn.mindState.lastMannedThing = parent;
	}

	public override string CompInspectStringExtra()
	{
		if (parent.Spawned && !Props.planetLayerWhitelist.NullOrEmpty() && !Props.planetLayerWhitelist.Contains(parent.Map.Tile.LayerDef))
		{
			return "CannotFunctionOnLayer".Translate(parent.Map.Tile.LayerDef.label).CapitalizeFirst().Colorize(ColoredText.WarningColor);
		}
		return null;
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__10))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__10(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__pawn = pawn
		};
	}
}
