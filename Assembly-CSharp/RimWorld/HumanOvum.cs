using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

[StaticConstructorOnStartup]
public class HumanOvum : ThingWithComps
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public List<FloatMenuOption> maleOptions;

		internal void _003CGetGizmos_003Eb__0()
		{
			Find.WindowStack.Add(new FloatMenu(maleOptions));
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__6 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HumanOvum _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private IEnumerator<FloatMenuOption> _003C_003E7__wrap1;

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
		public _003CGetFloatMenuOptions_003Ed__6(int _003C_003E1__state)
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
				HumanOvum humanOvum = _003C_003E4__this;
				FloatMenuOption floatMenuOption;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)humanOvum).GetFloatMenuOptions(selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0077;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0077;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0077:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					floatMenuOption = humanOvum.CanFertilizeFloatOption(selPawn, pawnAsLabel: false);
					if (floatMenuOption != null)
					{
						_003C_003E2__current = floatMenuOption;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetFloatMenuOptions_003Ed__6 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CGetFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__3 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public HumanOvum _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__3(int _003C_003E1__state)
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
				HumanOvum CS_0024_003C_003E8__locals16 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((ThingWithComps)CS_0024_003C_003E8__locals16).GetGizmos().GetEnumerator();
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
					if (!ModsConfig.BiotechActive)
					{
						return false;
					}
					if (CS_0024_003C_003E8__locals16.MapHeld == null)
					{
						break;
					}
					if (CS_0024_003C_003E8__locals16.fertilizingMan?.CurJob?.def != JobDefOf.FertilizeOvum || CS_0024_003C_003E8__locals16.fertilizingMan.CurJob.GetTarget(TargetIndex.A).Thing != CS_0024_003C_003E8__locals16)
					{
						CS_0024_003C_003E8__locals16.fertilizingMan = null;
					}
					if (CS_0024_003C_003E8__locals16.fertilizingMan == null)
					{
						_003C_003Ec__DisplayClass3_0 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass3_0
						{
							maleOptions = new List<FloatMenuOption>()
						};
						foreach (Pawn item in CS_0024_003C_003E8__locals16.MapHeld.mapPawns.FreeColonistsSpawned)
						{
							FloatMenuOption floatMenuOption = CS_0024_003C_003E8__locals16.CanFertilizeFloatOption(item, pawnAsLabel: true);
							if (floatMenuOption != null)
							{
								CS_0024_003C_003E8__locals13.maleOptions.Add(floatMenuOption);
							}
						}
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = $"{"Fertilize".Translate()}...",
							defaultDesc = "FertilizeGizmoDescription".Translate(),
							icon = FertilizeIcon.Texture,
							action = delegate
							{
								Find.WindowStack.Add(new FloatMenu(CS_0024_003C_003E8__locals13.maleOptions));
							}
						};
						if (CS_0024_003C_003E8__locals13.maleOptions.Count == 0)
						{
							command_Action.Disable("FertilizeDisabledNoMales".Translate());
						}
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "FertilizeCancel".Translate(CS_0024_003C_003E8__locals16.fertilizingMan.LabelShort),
						defaultDesc = "FertilizeCancel".Translate(CS_0024_003C_003E8__locals16.fertilizingMan.LabelShort),
						icon = CancelFertilizeIcon,
						action = delegate
						{
							CS_0024_003C_003E8__locals16.fertilizingMan.jobs.EndCurrentJob(JobCondition.InterruptForced);
						},
						activateSound = SoundDefOf.Designate_Cancel
					};
					_003C_003E1__state = 3;
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
			_003CGetGizmos_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__3(0)
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

	public static readonly CachedTexture FertilizeIcon = new CachedTexture("UI/Gizmos/Fertilize");

	private Pawn fertilizingMan;

	private static readonly Texture2D CancelFertilizeIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__3))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public Thing ProduceEmbryo(Pawn father)
	{
		HumanEmbryo humanEmbryo = (HumanEmbryo)ThingMaker.MakeThing(ThingDefOf.HumanEmbryo);
		CompHasPawnSources comp = humanEmbryo.GetComp<CompHasPawnSources>();
		List<Pawn> pawnSources = GetComp<CompHasPawnSources>().pawnSources;
		if (!pawnSources.NullOrEmpty())
		{
			foreach (Pawn item in pawnSources)
			{
				comp.AddSource(item);
			}
		}
		comp.AddSource(father);
		if (!humanEmbryo.TryPopulateGenes())
		{
			humanEmbryo.Destroy();
			return null;
		}
		return humanEmbryo;
	}

	public override Thing SplitOff(int count)
	{
		return base.SplitOff(count) as HumanOvum;
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__6))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	private FloatMenuOption CanFertilizeFloatOption(Pawn pawn, bool pawnAsLabel)
	{
		AcceptanceReport acceptanceReport = CanFertilizeReport(pawn);
		if (acceptanceReport.Accepted)
		{
			return new FloatMenuOption(pawnAsLabel ? pawn.LabelShort : "FertilizeThing".Translate(this).ToString(), delegate
			{
				Pawn pawn2 = GetComp<CompHasPawnSources>().pawnSources.FirstOrDefault();
				PawnRelationDef relation;
				float num = PregnancyUtility.InbredChanceFromParents(pawn, pawn2, out relation);
				if (num > 0f)
				{
					Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("FertilizeOvumInbredChance".Translate(pawn2.Named("MOTHER"), pawn.Named("FATHER"), relation.label.Named("RELATION"), num.ToStringPercent().Named("CHANCE")), delegate
					{
						TakeJob();
					}));
				}
				else
				{
					TakeJob();
				}
			});
		}
		if (!acceptanceReport.Reason.NullOrEmpty())
		{
			return new FloatMenuOption(pawnAsLabel ? "DisabledOption".Translate(pawn.LabelShort, acceptanceReport.Reason).ToString() : ("FertilizeCannotFloatOption".Translate().ToString() + ": " + acceptanceReport.Reason), null);
		}
		return null;
		void TakeJob()
		{
			Job job = JobMaker.MakeJob(JobDefOf.FertilizeOvum, this);
			job.count = 1;
			if (pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc))
			{
				fertilizingMan = pawn;
			}
		}
	}

	private AcceptanceReport CanFertilizeReport(Pawn pawn)
	{
		if (pawn.gender != Gender.Male)
		{
			return false;
		}
		if (pawn.IsQuestLodger())
		{
			return false;
		}
		if (!pawn.CanReach(this, PathEndMode.OnCell, Danger.Deadly))
		{
			return "NoPath".Translate();
		}
		if ((float)pawn.ageTracker.AgeBiologicalYears < 14f)
		{
			return "CannotMustBeAge".Translate(14f).CapitalizeFirst();
		}
		if (pawn.Sterile())
		{
			return "CannotSterile".Translate();
		}
		if (pawn.Downed || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
		{
			return "Incapacitated".Translate().ToLower();
		}
		return true;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref fertilizingMan, "fertilizingMan");
	}
}
