using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI;
using Verse.Sound;

namespace Verse;

public class Hediff_DeathRefusal : HediffWithComps
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass27_0
	{
		public Command_ActionWithLimitedUseCount cmdSelfResurrect;

		public Hediff_DeathRefusal _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__2()
		{
			if (_003C_003E4__this.resurrectTimer.Finished)
			{
				_003C_003E4__this.Use();
				cmdSelfResurrect.UpdateUsesLeft();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__27 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_DeathRefusal _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__27(int _003C_003E1__state)
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
				Hediff_DeathRefusal CS_0024_003C_003E8__locals13 = _003C_003E4__this;
				_003C_003Ec__DisplayClass27_0 CS_0024_003C_003E8__locals20;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffWithComps)CS_0024_003C_003E8__locals13).GetGizmos().GetEnumerator();
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
					if (!CS_0024_003C_003E8__locals13.pawn.Dead || !CS_0024_003C_003E8__locals13.PlayerControlled)
					{
						break;
					}
					CS_0024_003C_003E8__locals20 = new _003C_003Ec__DisplayClass27_0();
					CS_0024_003C_003E8__locals20._003C_003E4__this = CS_0024_003C_003E8__locals13;
					CS_0024_003C_003E8__locals20.cmdSelfResurrect = new Command_ActionWithLimitedUseCount();
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.defaultLabel = "CommandSelfResurrect".Translate();
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.defaultDesc = "CommandSelfResurrectDesc".Translate();
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.usesLeftGetter = () => CS_0024_003C_003E8__locals13.usesLeft;
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.maxUsesGetter = () => CS_0024_003C_003E8__locals13.MaxUses;
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.UpdateUsesLeft();
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.icon = Icon.Texture;
					CS_0024_003C_003E8__locals20.cmdSelfResurrect.action = delegate
					{
						if (CS_0024_003C_003E8__locals20._003C_003E4__this.resurrectTimer.Finished)
						{
							CS_0024_003C_003E8__locals20._003C_003E4__this.Use();
							CS_0024_003C_003E8__locals20.cmdSelfResurrect.UpdateUsesLeft();
						}
					};
					_003C_003E2__current = CS_0024_003C_003E8__locals20.cmdSelfResurrect;
					_003C_003E1__state = 2;
					return true;
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
			_003CGetGizmos_003Ed__27 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__27(0)
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

	protected int usesLeft;

	private TickTimer resurrectTimer = new TickTimer();

	private TickTimer warmupTimer = new TickTimer();

	private Sustainer resurrectSustainer;

	private bool resurrecting;

	private bool aiEnabled = true;

	private Effecter effecter;

	private Effecter resurrectAvailableEffecter;

	private Effecter resurrectUsedEffecter;

	private static readonly CachedTexture Icon = new CachedTexture("UI/Abilities/SelfResurrect");

	private const float ScarredChance = 0.1f;

	private static readonly float ResurrectDurationSeconds = 3f;

	private static readonly FloatRange AIWarmupSeconds = new FloatRange(1f, 2f);

	public bool PlayerControlled
	{
		get
		{
			if (pawn.IsColonist)
			{
				if (pawn.HostFaction != null)
				{
					return pawn.IsSlave;
				}
				return true;
			}
			return false;
		}
	}

	public bool InProgress => !resurrectTimer.Finished;

	public int UsesLeft => usesLeft;

	public bool AIEnabled
	{
		get
		{
			return aiEnabled;
		}
		set
		{
			aiEnabled = value;
		}
	}

	public virtual int MaxUses => 2;

	public override string LabelInBrackets => UsesLeft + " " + ((UsesLeft > 1) ? "DeathRefusalUsePlural".Translate() : "DeathRefusalUseSingular".Translate()).ToString();

	public override void PostAdd(DamageInfo? dinfo)
	{
		if (!ModLister.CheckAnomaly("Death refusal"))
		{
			pawn.health.RemoveHediff(this);
			return;
		}
		base.PostAdd(dinfo);
		usesLeft = 1;
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__27))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__27(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref usesLeft, "usesLeft", 0);
		Scribe_Values.Look(ref resurrecting, "resurrecting", defaultValue: false);
		Scribe_Values.Look(ref aiEnabled, "aiEnabled", defaultValue: false);
		Scribe_Deep.Look(ref resurrectTimer, "resurrectTimer");
		Scribe_Deep.Look(ref warmupTimer, "warmupTimer");
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			resurrectTimer.OnFinish = Resurrect;
			warmupTimer.OnFinish = Use;
			if (!resurrecting && pawn.Dead)
			{
				TryTriggerAIWarmupResurrection();
			}
		}
	}

	public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		base.Notify_PawnDied(dinfo, culprit);
		if (PlayerControlled && PawnUtility.ShouldSendNotificationAbout(pawn))
		{
			Messages.Message("SelfResurrectText".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
		}
		TryTriggerAIWarmupResurrection();
		TryTriggerReadyEffect();
	}

	private void TryTriggerAIWarmupResurrection()
	{
		if (!PlayerControlled && !resurrecting && AIEnabled && usesLeft > 0)
		{
			resurrecting = true;
			warmupTimer.Start(GenTicks.TicksGame, AIWarmupSeconds.RandomInRange.SecondsToTicks(), Use);
		}
	}

	private void TryTriggerReadyEffect()
	{
		if (!resurrecting && usesLeft > 0 && resurrectAvailableEffecter == null && pawn.ParentHolder is Corpse corpse)
		{
			resurrectAvailableEffecter = EffecterDefOf.DeathRefusalAvailable.Spawn(corpse, corpse.MapHeld, Vector3.zero);
			pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(resurrectAvailableEffecter, corpse, 250);
		}
	}

	public override void TickInterval(int delta)
	{
		if (!resurrectTimer.Finished && pawn.ParentHolder is Corpse corpse)
		{
			if (effecter == null)
			{
				effecter = EffecterDefOf.CellPollution.Spawn(corpse, pawn.MapHeld, Vector3.zero);
				pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(effecter, corpse, 250);
			}
			if (resurrectSustainer == null)
			{
				SoundInfo info = SoundInfo.InMap(corpse, MaintenanceType.PerTickRare);
				resurrectSustainer = SoundDefOf.Pawn_SelfResurrection.TrySpawnSustainer(info);
			}
		}
	}

	public void TickRare()
	{
		if (!warmupTimer.Finished)
		{
			warmupTimer.TickIntervalDelta();
		}
		TryTriggerReadyEffect();
		if (!resurrectTimer.Finished)
		{
			resurrectTimer.TickIntervalDelta();
			if (resurrectSustainer != null && !resurrectSustainer.Ended)
			{
				resurrectSustainer?.Maintain();
			}
		}
		if (effecter != null)
		{
			effecter.ticksLeft = ((!resurrectTimer.Finished) ? (effecter.ticksLeft + 250) : 0);
		}
		if (resurrectAvailableEffecter != null)
		{
			resurrectAvailableEffecter.ticksLeft += 250;
		}
	}

	private void Resurrect()
	{
		resurrectSustainer?.End();
		resurrecting = false;
		pawn.Drawer.renderer.SetAnimation(null);
		ResurrectionUtility.TryResurrect(pawn, new ResurrectionParams
		{
			gettingScarsChance = 0.1f,
			canKidnap = false,
			canTimeoutOrFlee = false,
			useAvoidGridSmart = true,
			canSteal = false,
			invisibleStun = true
		});
		if (usesLeft == 0)
		{
			Severity = 0f;
		}
		if (pawn.Faction != Faction.OfPlayer && !pawn.Downed)
		{
			Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Weapon), PathEndMode.OnCell, TraverseParms.For(pawn), 5f);
			if (thing != null)
			{
				Job job = JobGiver_PickupDroppedWeapon.PickupWeaponJob(pawn, thing, ignoreForbidden: true);
				if (job != null)
				{
					pawn.jobs.StartJob(job, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
				}
			}
		}
		pawn.health.AddHediff(HediffDefOf.DeathRefusalSickness, null, null);
	}

	public void SetUseAmountDirect(int amount, bool ignoreLimit = false)
	{
		if (ignoreLimit)
		{
			usesLeft = amount;
		}
		else
		{
			usesLeft = Mathf.Clamp(amount, 0, MaxUses);
		}
	}

	private void Use()
	{
		Messages.Message("MessageUsingSelfResurrection".Translate(pawn), pawn, MessageTypeDefOf.NeutralEvent);
		resurrecting = true;
		usesLeft = Mathf.Max(usesLeft - 1, 0);
		resurrectTimer.Start(GenTicks.TicksGame, ResurrectDurationSeconds.SecondsToTicks(), Resurrect);
		resurrectAvailableEffecter?.ForceEnd();
		resurrectAvailableEffecter = null;
		if (pawn.ParentHolder is Corpse corpse)
		{
			resurrectUsedEffecter = EffecterDefOf.DeathRefusalUse.Spawn(corpse, corpse.MapHeld, Vector3.zero);
			pawn.MapHeld.effecterMaintainer.AddEffecterToMaintain(resurrectUsedEffecter, corpse, 1000);
			pawn.Drawer.renderer.SetAnimation(AnimationDefOf.DeathRefusalTwitches);
		}
	}

	public override void Notify_PawnCorpseDestroyed()
	{
		resurrectSustainer?.End();
	}
}
