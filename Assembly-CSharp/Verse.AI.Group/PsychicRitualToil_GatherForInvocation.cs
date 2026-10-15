using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI.Group;

public class PsychicRitualToil_GatherForInvocation : PsychicRitualToil_Multiplex
{
	[CompilerGenerated]
	private sealed class _003CInvokerGatherPhaseToils_003Ed__11 : IEnumerable<PsychicRitualToil>, IEnumerable, IEnumerator<PsychicRitualToil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private PsychicRitualToil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private PsychicRitualDef_InvocationCircle def;

		public PsychicRitualDef_InvocationCircle _003C_003E3__def;

		private IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions;

		public IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> _003C_003E3__rolePositions;

		PsychicRitualToil IEnumerator<PsychicRitualToil>.Current
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
		public _003CInvokerGatherPhaseToils_003Ed__11(int _003C_003E1__state)
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
				if (def.RequiredOffering != null)
				{
					_003C_003E2__current = new PsychicRitualToil_GatherOfferings(def.InvokerRole, def.RequiredOffering);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0064;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0064;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a8;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00a8:
				_003C_003E2__current = new PsychicRitualToil_Goto(rolePositions.Slice(def.InvokerRole));
				_003C_003E1__state = 3;
				return true;
				IL_0064:
				if (def.TargetRole != null)
				{
					_003C_003E2__current = new PsychicRitualToil_CarryAndGoto(def.InvokerRole, def.TargetRole, rolePositions);
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00a8;
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
		IEnumerator<PsychicRitualToil> IEnumerable<PsychicRitualToil>.GetEnumerator()
		{
			_003CInvokerGatherPhaseToils_003Ed__11 _003CInvokerGatherPhaseToils_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CInvokerGatherPhaseToils_003Ed__ = this;
			}
			else
			{
				_003CInvokerGatherPhaseToils_003Ed__ = new _003CInvokerGatherPhaseToils_003Ed__11(0);
			}
			_003CInvokerGatherPhaseToils_003Ed__.def = _003C_003E3__def;
			_003CInvokerGatherPhaseToils_003Ed__.rolePositions = _003C_003E3__rolePositions;
			return _003CInvokerGatherPhaseToils_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<PsychicRitualToil>)this).GetEnumerator();
		}
	}

	protected PsychicRitualToil_Goto fallbackToil;

	protected PsychicRitualGraph invokerToil;

	protected PsychicRitualToil_Goto invokerFinalToil;

	private static List<Pawn> blockingPawns = new List<Pawn>(16);

	protected PsychicRitualToil_GatherForInvocation()
	{
	}

	protected PsychicRitualToil_GatherForInvocation(PsychicRitualDef_InvocationCircle def, PsychicRitualToil_Goto fallbackToil, PsychicRitualGraph invokerToil)
		: base(new Dictionary<PsychicRitualRoleDef, PsychicRitualToil> { { def.InvokerRole, invokerToil } }, fallbackToil)
	{
		this.fallbackToil = fallbackToil;
		this.invokerToil = invokerToil;
		invokerFinalToil = (PsychicRitualToil_Goto)invokerToil.GetToil(invokerToil.ToilCount - 1);
	}

	public PsychicRitualToil_GatherForInvocation(PsychicRitual psychicRitual, PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
		: this(def, FallbackToil(psychicRitual, def, rolePositions), InvokerToil(def, rolePositions))
	{
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref fallbackToil, "fallbackToil");
		Scribe_References.Look(ref invokerToil, "invokerToil");
		Scribe_References.Look(ref invokerFinalToil, "invokerFinalToil");
	}

	public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
	{
		blockingPawns.Clear();
		blockingPawns.AddRange(fallbackToil.BlockingPawns);
		if (invokerToil.CurrentToil == invokerFinalToil)
		{
			blockingPawns.AddRange(invokerFinalToil.BlockingPawns);
		}
		else
		{
			blockingPawns.AddRange(invokerFinalToil.ControlledPawns(psychicRitual));
		}
		string text = "PsychicRitualToil_GatherForInvocation_Report".Translate();
		string text2 = blockingPawns.Select((Pawn pawn) => pawn.LabelShortCap).ToCommaList();
		return text + ": " + text2;
	}

	public static PsychicRitualToil_Goto FallbackToil(PsychicRitual psychicRitual, PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
	{
		return new PsychicRitualToil_Goto(rolePositions.Slice(rolePositions.Keys.Except(def.InvokerRole)));
	}

	public static PsychicRitualGraph InvokerToil(PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
	{
		return new PsychicRitualGraph(InvokerGatherPhaseToils(def, rolePositions))
		{
			willAdvancePastLastToil = false
		};
	}

	[IteratorStateMachine(typeof(_003CInvokerGatherPhaseToils_003Ed__11))]
	public static IEnumerable<PsychicRitualToil> InvokerGatherPhaseToils(PsychicRitualDef_InvocationCircle def, IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> rolePositions)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CInvokerGatherPhaseToils_003Ed__11(-2)
		{
			_003C_003E3__def = def,
			_003C_003E3__rolePositions = rolePositions
		};
	}
}
