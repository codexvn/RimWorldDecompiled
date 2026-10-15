using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;
using Verse;

public class Command_CallBossgroup : Command
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public BossgroupDef bg;

		public Command_CallBossgroup _003C_003E4__this;

		internal void _003Cget_FloatMenuOptions_003Eb__0()
		{
			CallBossgroupUtility.TryStartSummonBossgroupJob(bg, _003C_003E4__this.mechanitor.Pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003Cget_FloatMenuOptions_003Ed__3 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Command_CallBossgroup _003C_003E4__this;

		private IEnumerator<BossgroupDef> _003C_003E7__wrap1;

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
		public _003Cget_FloatMenuOptions_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 1) <= 1u)
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
				Command_CallBossgroup command_CallBossgroup = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					IEnumerable<BossgroupDef> allDefs = DefDatabase<BossgroupDef>.AllDefs;
					_003C_003E7__wrap1 = allDefs.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				case 2:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals5 = new _003C_003Ec__DisplayClass3_0
					{
						_003C_003E4__this = command_CallBossgroup,
						bg = _003C_003E7__wrap1.Current
					};
					AcceptanceReport acceptanceReport = CallBossgroupUtility.BossgroupEverCallable(command_CallBossgroup.mechanitor.Pawn, CS_0024_003C_003E8__locals5.bg);
					if (!acceptanceReport)
					{
						_003C_003E2__current = new FloatMenuOption("CannotSummon".Translate(CS_0024_003C_003E8__locals5.bg.boss.kindDef.label) + ": " + acceptanceReport.Reason, null);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E2__current = new FloatMenuOption("Summon".Translate(CS_0024_003C_003E8__locals5.bg.boss.kindDef.label), delegate
					{
						CallBossgroupUtility.TryStartSummonBossgroupJob(CS_0024_003C_003E8__locals5.bg, CS_0024_003C_003E8__locals5._003C_003E4__this.mechanitor.Pawn);
					});
					_003C_003E1__state = 2;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003Cget_FloatMenuOptions_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003Cget_FloatMenuOptions_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	private static readonly CachedTexture BossgroupTex = new CachedTexture("UI/Icons/SummonMechThreat");

	private Pawn_MechanitorTracker mechanitor;

	private IEnumerable<FloatMenuOption> FloatMenuOptions
	{
		[IteratorStateMachine(typeof(_003Cget_FloatMenuOptions_003Ed__3))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_FloatMenuOptions_003Ed__3(-2)
			{
				_003C_003E4__this = this
			};
		}
	}

	public override string DescPostfix
	{
		get
		{
			string text = "";
			Dictionary<BossgroupDef, AcceptanceReport> source = DefDatabase<BossgroupDef>.AllDefs.ToDictionary((BossgroupDef b) => b, (BossgroupDef b) => CallBossgroupUtility.BossgroupEverCallable(mechanitor.Pawn, b));
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> item in source.Where((KeyValuePair<BossgroupDef, AcceptanceReport> b) => b.Value))
			{
				text = text + "\n\n" + "ReadyToSummonThreat".Translate(item.Key.boss.kindDef.label).Colorize(ColorLibrary.Green).CapitalizeFirst();
			}
			foreach (KeyValuePair<BossgroupDef, AcceptanceReport> item2 in source.Where((KeyValuePair<BossgroupDef, AcceptanceReport> b) => !b.Value))
			{
				text = text + "\n\n" + ("CannotSummon".Translate(item2.Key.boss.kindDef.label) + ": " + item2.Value.Reason).Colorize(ColorLibrary.RedReadable);
			}
			return text;
		}
	}

	public Command_CallBossgroup(Pawn_MechanitorTracker mechanitor)
	{
		this.mechanitor = mechanitor;
		defaultLabel = "CommandCallBossgroup".Translate();
		defaultDesc = "CommandCallBossgroupDesc".Translate();
		icon = BossgroupTex.Texture;
	}

	public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
	{
		disabled = IsDisabled(out disabledReason);
		return base.GizmoOnGUI(topLeft, maxWidth, parms);
	}

	private bool IsDisabled(out string reason)
	{
		int lastBossgroupCalled = Find.BossgroupManager.lastBossgroupCalled;
		int num = Find.TickManager.TicksGame - lastBossgroupCalled;
		if (Faction.OfMechanoids == null || Faction.OfMechanoids.deactivated)
		{
			reason = "MechsDisabled".Translate();
			return true;
		}
		if (!mechanitor.Pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			reason = "Incapable".Translate().CapitalizeFirst();
			return true;
		}
		if (num < 120000)
		{
			reason = "BossgroupAvailableIn".Translate((120000 - num).ToStringTicksToPeriod()).CapitalizeFirst();
			return true;
		}
		PawnKindDef pendingBossgroup = CallBossgroupUtility.GetPendingBossgroup();
		if (pendingBossgroup != null)
		{
			reason = "BossgroupIncoming".Translate(pendingBossgroup.label).CapitalizeFirst();
			return true;
		}
		if (FloatMenuOptions.All((FloatMenuOption f) => f.action == null))
		{
			reason = null;
			return true;
		}
		reason = null;
		return false;
	}

	public override void ProcessInput(Event ev)
	{
		base.ProcessInput(ev);
		List<FloatMenuOption> list = new List<FloatMenuOption>();
		list.AddRange(FloatMenuOptions);
		Find.WindowStack.Add(new FloatMenu(list));
	}
}
