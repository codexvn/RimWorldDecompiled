using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse;

public class Hediff_Labor : HediffWithParents
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__12 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Hediff_Labor _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__12(int _003C_003E1__state)
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
				Hediff_Labor CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				Lord lord;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((HediffWithParents)CS_0024_003C_003E8__locals9).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007c;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0166;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Force progress to labor pushing",
						action = delegate
						{
							CS_0024_003C_003E8__locals9.pawn.health.RemoveHediff(CS_0024_003C_003E8__locals9);
						}
					};
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007c:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					lord = CS_0024_003C_003E8__locals9.pawn.GetLord();
					if (lord == null || lord.LordJob == null)
					{
						Precept_Ritual precept_Ritual = (Precept_Ritual)CS_0024_003C_003E8__locals9.pawn.Ideo.GetPrecept(PreceptDefOf.ChildBirth);
						TargetInfo targetInfo = BestBed(CS_0024_003C_003E8__locals9.pawn);
						Command_Ritual command_Ritual = new Command_Ritual(precept_Ritual, targetInfo, null, new Dictionary<string, Pawn> { { "mother", CS_0024_003C_003E8__locals9.pawn } });
						if (!targetInfo.IsValid)
						{
							command_Ritual.Disabled = true;
							command_Ritual.disabledReason = "NoAppropriateBedChildBirth".Translate();
						}
						else if (!CS_0024_003C_003E8__locals9.FoundBirthDoctor(precept_Ritual))
						{
							command_Ritual.Disabled = true;
							command_Ritual.disabledReason = "NoDoctorChildBirth".Translate();
						}
						_003C_003E2__current = command_Ritual;
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0166;
					IL_0166:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = PregnancyUtility.BirthQualityGizmo(CS_0024_003C_003E8__locals9.pawn);
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
			_003CGetGizmos_003Ed__12 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__12(0)
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

	private RitualRole testForDoctor;

	private int laborPushingDuration;

	private Effecter progressBar;

	public override string LabelBase => "Labor".Translate();

	public override string LabelInBrackets
	{
		get
		{
			string labelInBrackets = base.LabelInBrackets;
			if (!labelInBrackets.NullOrEmpty())
			{
				return "LaborDilation".Translate() + ", " + labelInBrackets;
			}
			return "LaborDilation".Translate();
		}
	}

	public float Progress
	{
		get
		{
			HediffComp_Disappears hediffComp_Disappears = this.TryGetComp<HediffComp_Disappears>();
			return 1f - (float)(hediffComp_Disappears.ticksToDisappear + laborPushingDuration) / (float)Mathf.Max(1, hediffComp_Disappears.disappearsAfterTicks + laborPushingDuration);
		}
	}

	private static int GetRandomLaborPushingDuration()
	{
		return HediffDefOf.PregnancyLaborPushing.CompProps<HediffCompProperties_Disappears>().disappearsAfterTicks.RandomInRange;
	}

	public override void PostAdd(DamageInfo? dinfo)
	{
		base.PostAdd(dinfo);
		Severity = def.stages.RandomElement().minSeverity;
		laborPushingDuration = GetRandomLaborPushingDuration();
	}

	private static TargetInfo BestBed(Pawn mother)
	{
		return mother.CurrentBed() ?? RestUtility.FindPatientBedFor(mother);
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__12))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__12(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void PreRemoved()
	{
		base.PreRemoved();
		Hediff_LaborPushing obj = (Hediff_LaborPushing)pawn.health.AddHediff(HediffDefOf.PregnancyLaborPushing, null, null);
		if (PawnUtility.ShouldSendNotificationAbout(pawn))
		{
			Messages.Message("MessageColonistInFinalStagesOfLabor".Translate(pawn.Named("PAWN")), pawn, MessageTypeDefOf.PositiveEvent);
		}
		obj.SetParents(base.Mother, base.Father, geneSet);
		HediffComp_Disappears hediffComp_Disappears = obj.TryGetComp<HediffComp_Disappears>();
		hediffComp_Disappears.disappearsAfterTicks = laborPushingDuration;
		hediffComp_Disappears.ticksToDisappear = laborPushingDuration;
		obj.laborDuration = this.TryGetComp<HediffComp_Disappears>().disappearsAfterTicks;
	}

	private bool FoundBirthDoctor(Precept_Ritual birthRitual)
	{
		if (testForDoctor == null)
		{
			testForDoctor = birthRitual.behavior.def.roles.First((RitualRole r) => r.id == "doctor");
		}
		foreach (Pawn item in pawn.MapHeld.mapPawns.FreeColonistsSpawned)
		{
			if (item != pawn && RitualRoleAssignments.PawnNotAssignableReason(item, testForDoctor, birthRitual, null, null, out var _) == null)
			{
				return true;
			}
		}
		return false;
	}

	public override void TickInterval(int delta)
	{
		if (pawn.SpawnedOrAnyParentSpawned)
		{
			if (progressBar == null)
			{
				progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
			}
			progressBar.EffectTick(pawn, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBar.children[0]).mote;
			if (mote != null)
			{
				mote.progress = Progress;
			}
		}
		else
		{
			progressBar?.Cleanup();
			progressBar = null;
		}
	}

	public override void PostRemoved()
	{
		progressBar?.Cleanup();
		progressBar = null;
		base.PostRemoved();
	}

	public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
	{
		base.Notify_PawnDied(dinfo, culprit);
		progressBar?.Cleanup();
		progressBar = null;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref laborPushingDuration, "laborPushingDuration", -1);
		if (Scribe.mode == LoadSaveMode.LoadingVars && laborPushingDuration == -1)
		{
			laborPushingDuration = GetRandomLaborPushingDuration();
		}
	}
}
