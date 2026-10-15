using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompFleshmassSpitter : CompFleshmassHeartChild, IAttackTargetSearcher, IVerbOwner
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__39 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompFleshmassSpitter _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__39(int _003C_003E1__state)
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
				CompFleshmassSpitter CS_0024_003C_003E8__locals3 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingComp)CS_0024_003C_003E8__locals3).CompGetGizmosExtra().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0072;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0072;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0072:
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
							defaultDesc = "Dev: Cooldown Spit",
							defaultLabel = "Dev: Cooldown Spit",
							action = delegate
							{
								CS_0024_003C_003E8__locals3.lastSpitTick = -999999;
								CS_0024_003C_003E8__locals3.nextSpitDelay = -999999;
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
			_003CCompGetGizmosExtra_003Ed__39 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__39(0)
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

	private static readonly IntRange SpitIntervalRangeTicks = new IntRange(5000, 7500);

	private const int SpitCheckIntervalTicks = 180;

	private const int ReadySpriteIndex = 0;

	private const int CooldownSpriteIndex = 1;

	private int lastSpitTick = -99999;

	private int nextSpitDelay = -99999;

	private LocalTargetInfo lastAttackedTarget;

	private VerbTracker verbTracker;

	private Effecter progressBarEffecter;

	private Verb AttackVerb => AllVerbs[0];

	private int TicksToNextSpit => lastSpitTick + nextSpitDelay - Find.TickManager.TicksGame;

	private bool OnCooldown => TicksToNextSpit > 0;

	public Thing Thing => parent;

	public Verb CurrentEffectiveVerb => AttackVerb;

	public LocalTargetInfo LastAttackedTarget => lastAttackedTarget;

	public int LastAttackTargetTick => lastSpitTick;

	public VerbTracker VerbTracker => verbTracker ?? (verbTracker = new VerbTracker(this));

	public List<VerbProperties> VerbProperties => parent.def.Verbs;

	public List<Tool> Tools => parent.def.tools;

	public ImplementOwnerTypeDef ImplementOwnerTypeDef => ImplementOwnerTypeDefOf.NativeVerb;

	public Thing ConstantCaster => parent;

	public List<Verb> AllVerbs => VerbTracker.AllVerbs;

	public string UniqueVerbOwnerID()
	{
		return "CompFleshmassSpitter_" + parent.ThingID;
	}

	public bool VerbsStillUsableBy(Pawn p)
	{
		return false;
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		SetupVerbs();
		if (!respawningAfterLoad)
		{
			lastSpitTick = Find.TickManager.TicksGame;
			nextSpitDelay = SpitIntervalRangeTicks.RandomInRange;
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref nextSpitDelay, "nextSpitDelay", 0);
		Scribe_Values.Look(ref lastSpitTick, "lastSpitTick", 0);
		Scribe_TargetInfo.Look(ref lastAttackedTarget, "lastAttackedTarget");
		Scribe_Deep.Look(ref verbTracker, "verbTracker", this);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__39))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__39(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void SetupVerbs()
	{
		foreach (Verb allVerb in AllVerbs)
		{
			allVerb.caster = parent;
		}
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		progressBarEffecter?.ForceEnd();
		progressBarEffecter = null;
	}

	public override void CompTick()
	{
		base.CompTick();
		if (!parent.Spawned)
		{
			return;
		}
		if (OnCooldown)
		{
			if (progressBarEffecter == null)
			{
				progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
			}
			progressBarEffecter.EffectTick(parent, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
			mote.progress = 1f - (float)Mathf.Max(TicksToNextSpit, 0) / (float)nextSpitDelay;
			mote.offsetZ = -0.8f;
			parent.overrideGraphicIndex = 1;
		}
		else
		{
			parent.overrideGraphicIndex = 0;
		}
		if (TicksToNextSpit <= 0 && parent.IsHashIntervalTick(180))
		{
			Thing thing = (Thing)AttackTargetFinder.BestShootTargetFromCurrentPosition(this, TargetScanFlags.NeedThreat, (Thing t) => !t.Position.Roofed(t.Map));
			if (thing != null)
			{
				AttackVerb.TryStartCastOn(thing);
				lastSpitTick = Find.TickManager.TicksGame;
				nextSpitDelay = SpitIntervalRangeTicks.RandomInRange;
				Messages.Message("SpitterAttacking".Translate(), parent, MessageTypeDefOf.NegativeEvent);
			}
		}
	}

	public override void PostDrawExtraSelectionOverlays()
	{
		AttackVerb.DrawHighlight(LocalTargetInfo.Invalid);
	}

	public override string CompInspectStringExtra()
	{
		if (TicksToNextSpit > 0)
		{
			return "SpitterGatheringSpit".Translate() + ": " + TicksToNextSpit.ToStringTicksToPeriod();
		}
		return "SpitterReadyToSpit".Translate();
	}
}
