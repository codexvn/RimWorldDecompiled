using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group;

public static class PsychicRitualGizmo
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_0
	{
		public Thing target;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass0_1
	{
		public PsychicRitualDef_InvocationCircle ritualDef;

		public _003C_003Ec__DisplayClass0_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetGizmos_003Eb__1()
		{
			InitializePsychicRitual(ritualDef, CS_0024_003C_003E8__locals1.target);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__0 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing target;

		public Thing _003C_003E3__target;

		private _003C_003Ec__DisplayClass0_0 _003C_003E8__1;

		private IEnumerator<PsychicRitualDef_InvocationCircle> _003C_003E7__wrap1;

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
		public _003CGetGizmos_003Ed__0(int _003C_003E1__state)
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
			_003C_003E8__1 = null;
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass0_0();
					_003C_003E8__1.target = target;
					IOrderedEnumerable<PsychicRitualDef_InvocationCircle> orderedEnumerable = from ritualDef in VisibleRituals()
						orderby ritualDef.label
						select ritualDef;
					_003C_003E7__wrap1 = orderedEnumerable.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass0_1 CS_0024_003C_003E8__locals8 = new _003C_003Ec__DisplayClass0_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						ritualDef = _003C_003E7__wrap1.Current
					};
					Command_PsychicRitual command_PsychicRitual = new Command_PsychicRitual
					{
						defaultLabel = CS_0024_003C_003E8__locals8.ritualDef.LabelCap,
						defaultDesc = CS_0024_003C_003E8__locals8.ritualDef.description + $"\n\n{CS_0024_003C_003E8__locals8.ritualDef.TimeAndOfferingLabel()}",
						action = delegate
						{
							InitializePsychicRitual(CS_0024_003C_003E8__locals8.ritualDef, CS_0024_003C_003E8__locals8.CS_0024_003C_003E8__locals1.target);
						},
						icon = CS_0024_003C_003E8__locals8.ritualDef.uiIcon
					};
					AcceptanceReport acceptanceReport = Find.PsychicRitualManager.CanInvoke(CS_0024_003C_003E8__locals8.ritualDef, CS_0024_003C_003E8__locals8.CS_0024_003C_003E8__locals1.target.Map);
					if ((bool)acceptanceReport)
					{
						command_PsychicRitual.Disabled = false;
						command_PsychicRitual.disabledReason = null;
					}
					else
					{
						command_PsychicRitual.Disable(acceptanceReport.Reason.CapitalizeFirst());
					}
					_003C_003E2__current = command_PsychicRitual;
					_003C_003E1__state = 1;
					return true;
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
			_003CGetGizmos_003Ed__0 _003CGetGizmos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetGizmos_003Ed__ = this;
			}
			else
			{
				_003CGetGizmos_003Ed__ = new _003CGetGizmos_003Ed__0(0);
			}
			_003CGetGizmos_003Ed__.target = _003C_003E3__target;
			return _003CGetGizmos_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	private static readonly List<PsychicRitualDef_InvocationCircle> tmpVisibleRituals = new List<PsychicRitualDef_InvocationCircle>(16);

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__0))]
	public static IEnumerable<Gizmo> GetGizmos(Thing target)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__0(-2)
		{
			_003C_003E3__target = target
		};
	}

	public static Gizmo CancelGizmo(PsychicRitual psychicRitual)
	{
		return new Command_Action
		{
			defaultLabel = "CommandCancelPsychicRitual".Translate(psychicRitual.def.label).CapitalizeFirst(),
			defaultDesc = "CommandCancelPsychicRitualDesc".Translate(psychicRitual.def.label).CapitalizeFirst().EndWithPeriod(),
			icon = ContentFinder<Texture2D>.Get("UI/Commands/CancelPsychicRitual_Gizmo"),
			action = delegate
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("CommandCancelPsychicRitualConfirm".Translate(psychicRitual.def.label), delegate
				{
					psychicRitual.CancelPsychicRitual(TaggedString.Empty);
				}));
				SoundDefOf.Tick_Low.PlayOneShotOnCamera();
			},
			hotKey = KeyBindingDefOf.Misc6
		};
	}

	public static Gizmo LeaveGizmo(PsychicRitual psychicRitual, Pawn pawn)
	{
		return new Command_Action
		{
			defaultLabel = "CommandLeavePsychicRitual".Translate(psychicRitual.def.label).CapitalizeFirst(),
			defaultDesc = "CommandLeavePsychicRitualDesc".Translate(psychicRitual.def.label).CapitalizeFirst().EndWithPeriod(),
			action = delegate
			{
				psychicRitual.LeavePsychicRitual(pawn, TaggedString.Empty);
				SoundDefOf.Tick_Low.PlayOneShotOnCamera();
			},
			hotKey = KeyBindingDefOf.Misc5
		};
	}

	private static List<PsychicRitualDef_InvocationCircle> VisibleRituals()
	{
		tmpVisibleRituals.Clear();
		foreach (PsychicRitualDef allDef in DefDatabase<PsychicRitualDef>.AllDefs)
		{
			if (allDef.Visible && allDef is PsychicRitualDef_InvocationCircle item)
			{
				tmpVisibleRituals.Add(item);
			}
		}
		return tmpVisibleRituals;
	}

	private static void InitializePsychicRitual(PsychicRitualDef_InvocationCircle psychicRitualDef, Thing target)
	{
		TargetInfo target2 = new TargetInfo(target);
		PsychicRitualRoleAssignments assignments = psychicRitualDef.BuildRoleAssignments(target2);
		PsychicRitualCandidatePool candidatePool = psychicRitualDef.FindCandidatePool();
		Map currentMap = Find.CurrentMap;
		psychicRitualDef.InitializeCast(currentMap);
		Find.WindowStack.Add(new Dialog_BeginPsychicRitual(psychicRitualDef, candidatePool, assignments, currentMap));
	}
}
