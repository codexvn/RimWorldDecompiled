using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompBiosculpterPod : ThingComp, ISuspendableThingHolder, IThingHolder, IThingHolderWithDrawnPawn, IStoreSettingsParent, INotifyHauledTo, ISearchableContents
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass91_0
	{
		public CompBiosculpterPod_Cycle cycle;

		public CompBiosculpterPod _003C_003E4__this;

		internal void _003CCompGetGizmosExtra_003Eb__0()
		{
			_003C_003E4__this.SelectPawnsForCycleOptions(cycle, out var options);
			if (_003C_003E4__this.biotunedTo != null && options.Count > 0)
			{
				options[0].action();
				if (!(cycle is CompBiosculpterPod_HealingCycle))
				{
					Messages.Message("BiosculpterEnteringMessage".Translate(_003C_003E4__this.biotunedTo.Named("PAWN"), cycle.Props.label.Named("CYCLE")).CapitalizeFirst(), _003C_003E4__this.parent, MessageTypeDefOf.SilentInput, historical: false);
				}
			}
			else
			{
				Find.WindowStack.Add(new FloatMenu(options));
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass96_0
	{
		public CompBiosculpterPod _003C_003E4__this;

		public Pawn selPawn;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass96_1
	{
		public CompBiosculpterPod_Cycle cycle;

		public _003C_003Ec__DisplayClass96_0 CS_0024_003C_003E8__locals1;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.PrepareCycleJob(CS_0024_003C_003E8__locals1.selPawn, CS_0024_003C_003E8__locals1.selPawn, cycle, CS_0024_003C_003E8__locals1._003C_003E4__this.EnterBiosculpterJob());
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__96 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompBiosculpterPod _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass96_0 _003C_003E8__1;

		private string _003CcycleIndependentfailureReason_003E5__2;

		private List<CompBiosculpterPod_Cycle>.Enumerator _003C_003E7__wrap2;

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
		public _003CCompFloatMenuOptions_003Ed__96(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || (uint)(num - 2) <= 1u)
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
			_003CcycleIndependentfailureReason_003E5__2 = null;
			_003C_003E7__wrap2 = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompBiosculpterPod compBiosculpterPod = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass96_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					if (_003C_003E8__1.selPawn.IsQuestLodger())
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnter".Translate() + ": " + "CryptosleepCasketGuestsNotAllowed".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 1;
						return true;
					}
					_003CcycleIndependentfailureReason_003E5__2 = compBiosculpterPod.CannotUseNowReason() ?? compBiosculpterPod.CannotUseNowPawnReason(_003C_003E8__1.selPawn);
					_003C_003E7__wrap2 = compBiosculpterPod.AvailableCycles.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -1;
					return false;
				case 2:
					_003C_003E1__state = -3;
					break;
				case 3:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap2.MoveNext())
				{
					_003C_003Ec__DisplayClass96_1 CS_0024_003C_003E8__locals13 = new _003C_003Ec__DisplayClass96_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						cycle = _003C_003E7__wrap2.Current
					};
					string text = _003CcycleIndependentfailureReason_003E5__2 ?? compBiosculpterPod.CannotUseNowCycleReason(CS_0024_003C_003E8__locals13.cycle) ?? compBiosculpterPod.CannotUseNowPawnCycleReason(CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.selPawn, CS_0024_003C_003E8__locals13.cycle);
					if (text != null)
					{
						_003C_003E2__current = new FloatMenuOption(CannotStartText(CS_0024_003C_003E8__locals13.cycle, text), null);
						_003C_003E1__state = 2;
						return true;
					}
					string label = "EnterBiosculpterPod".Translate(CS_0024_003C_003E8__locals13.cycle.Props.label, ((int)(CS_0024_003C_003E8__locals13.cycle.Props.durationDays / compBiosculpterPod.GetCycleSpeedFactorForPawn(CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.selPawn) * 60000f)).ToStringTicksToDays());
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(label, delegate
					{
						CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1._003C_003E4__this.PrepareCycleJob(CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.selPawn, CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.selPawn, CS_0024_003C_003E8__locals13.cycle, CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1._003C_003E4__this.EnterBiosculpterJob());
					}), CS_0024_003C_003E8__locals13.CS_0024_003C_003E8__locals1.selPawn, compBiosculpterPod.parent);
					_003C_003E1__state = 3;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap2 = default(List<CompBiosculpterPod_Cycle>.Enumerator);
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
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__96 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__96(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__91 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompBiosculpterPod _003C_003E4__this;

		private BiosculpterPodState _003Cstate_003E5__2;

		private string _003CcycleIndependentCannotUseReason_003E5__3;

		private List<CompBiosculpterPod_Cycle>.Enumerator _003C_003E7__wrap3;

		private IEnumerator<Gizmo> _003C_003E7__wrap4;

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
		public _003CCompGetGizmosExtra_003Ed__91(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CcycleIndependentCannotUseReason_003E5__3 = null;
			_003C_003E7__wrap3 = default(List<CompBiosculpterPod_Cycle>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				CompBiosculpterPod CS_0024_003C_003E8__locals57 = _003C_003E4__this;
				Gizmo gizmo;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003Cstate_003E5__2 = CS_0024_003C_003E8__locals57.State;
					_003CcycleIndependentCannotUseReason_003E5__3 = CS_0024_003C_003E8__locals57.CannotUseNowReason();
					_003C_003E7__wrap3 = CS_0024_003C_003E8__locals57.AvailableCycles.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_01f0;
				case 1:
					_003C_003E1__state = -3;
					goto IL_01f0;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0296;
				case 3:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals57.biotunedTo?.Ideo?.HasPrecept(PreceptDefOf.AgeReversal_Demanded) == true)
					{
						Command_Toggle command_Toggle = new Command_Toggle
						{
							defaultLabel = "BiosculpterAutoAgeReversalLabel".Translate(CS_0024_003C_003E8__locals57.biotunedTo.Named("PAWN"))
						};
						TaggedString taggedString = ((CS_0024_003C_003E8__locals57.biotunedTo.ageTracker.AgeReversalDemandedDeadlineTicks > 0) ? "BiosculpterAutoAgeReversalDescriptionFuture".Translate(CS_0024_003C_003E8__locals57.biotunedTo.Named("PAWN"), ((int)CS_0024_003C_003E8__locals57.biotunedTo.ageTracker.AgeReversalDemandedDeadlineTicks).ToStringTicksToPeriodVague().Named("TIME")) : "BiosculpterAutoAgeReversalDescriptionNow".Translate(CS_0024_003C_003E8__locals57.biotunedTo.Named("PAWN")));
						command_Toggle.defaultDesc = "BiosculpterAutoAgeReversalDescription".Translate(CS_0024_003C_003E8__locals57.biotunedTo.Named("PAWN"), taggedString.Named("NEXTTREATMENT"));
						command_Toggle.icon = CS_0024_003C_003E8__locals57.AutoAgeReversalIcon;
						command_Toggle.isActive = () => CS_0024_003C_003E8__locals57.AutoAgeReversal;
						command_Toggle.toggleAction = delegate
						{
							CS_0024_003C_003E8__locals57.autoAgeReversal = !CS_0024_003C_003E8__locals57.autoAgeReversal;
						};
						if (!CanAgeReverse(CS_0024_003C_003E8__locals57.biotunedTo))
						{
							command_Toggle.Disable("UnderMinBiosculpterAgeReversalAge".Translate(CS_0024_003C_003E8__locals57.biotunedTo.ageTracker.AdultMinAge.Named("ADULTAGE")).CapitalizeFirst());
							CS_0024_003C_003E8__locals57.autoAgeReversal = false;
						}
						_003C_003E2__current = command_Toggle;
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_04e3;
				case 4:
					_003C_003E1__state = -1;
					goto IL_04e3;
				case 5:
					_003C_003E1__state = -4;
					goto IL_052e;
				case 6:
					_003C_003E1__state = -1;
					goto IL_0579;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: advance cycle +1 day",
						action = delegate
						{
							CS_0024_003C_003E8__locals57.currentCycleTicksRemaining -= 60000f;
						},
						Disabled = (CS_0024_003C_003E8__locals57.State != BiosculpterPodState.Occupied)
					};
					_003C_003E1__state = 8;
					return true;
				case 8:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: complete biotune timer",
						action = delegate
						{
							CS_0024_003C_003E8__locals57.biotunedCountdownTicks = 10;
						},
						Disabled = (CS_0024_003C_003E8__locals57.biotunedCountdownTicks <= 0)
					};
					_003C_003E1__state = 9;
					return true;
				case 9:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: fill nutrition and cycle ingredients",
						action = delegate
						{
							CS_0024_003C_003E8__locals57.liquifiedNutrition = 5f;
							CS_0024_003C_003E8__locals57.devFillPodLatch = true;
						},
						Disabled = (CS_0024_003C_003E8__locals57.State == BiosculpterPodState.Occupied || (CS_0024_003C_003E8__locals57.devFillPodLatch && CS_0024_003C_003E8__locals57.liquifiedNutrition == 5f))
					};
					_003C_003E1__state = 10;
					return true;
				case 10:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0296:
					_003C_003E2__current = new Command_Toggle
					{
						defaultLabel = "BiosculpterAutoLoadNutritionLabel".Translate(),
						defaultDesc = "BiosculpterAutoLoadNutritionDescription".Translate(),
						icon = (CS_0024_003C_003E8__locals57.autoLoadNutrition ? TexCommand.ForbidOff : TexCommand.ForbidOn),
						isActive = () => CS_0024_003C_003E8__locals57.autoLoadNutrition,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals57.autoLoadNutrition = !CS_0024_003C_003E8__locals57.autoLoadNutrition;
						}
					};
					_003C_003E1__state = 3;
					return true;
					IL_01f0:
					if (_003C_003E7__wrap3.MoveNext())
					{
						_003C_003Ec__DisplayClass91_0 CS_0024_003C_003E8__locals28 = new _003C_003Ec__DisplayClass91_0
						{
							_003C_003E4__this = CS_0024_003C_003E8__locals57,
							cycle = _003C_003E7__wrap3.Current
						};
						string text = _003CcycleIndependentCannotUseReason_003E5__3 ?? CS_0024_003C_003E8__locals57.CannotUseNowCycleReason(CS_0024_003C_003E8__locals28.cycle);
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "BiosculpterPodCycleCommand".Translate(CS_0024_003C_003E8__locals28.cycle.Props.label) + ((CS_0024_003C_003E8__locals57.biotunedTo != null) ? (" (" + CS_0024_003C_003E8__locals57.biotunedTo.LabelShort + ")") : ""),
							defaultDesc = CS_0024_003C_003E8__locals57.CycleDescription(CS_0024_003C_003E8__locals28.cycle),
							icon = CS_0024_003C_003E8__locals28.cycle.Props.Icon,
							action = delegate
							{
								CS_0024_003C_003E8__locals28._003C_003E4__this.SelectPawnsForCycleOptions(CS_0024_003C_003E8__locals28.cycle, out var options);
								if (CS_0024_003C_003E8__locals28._003C_003E4__this.biotunedTo != null && options.Count > 0)
								{
									options[0].action();
									if (!(CS_0024_003C_003E8__locals28.cycle is CompBiosculpterPod_HealingCycle))
									{
										Messages.Message("BiosculpterEnteringMessage".Translate(CS_0024_003C_003E8__locals28._003C_003E4__this.biotunedTo.Named("PAWN"), CS_0024_003C_003E8__locals28.cycle.Props.label.Named("CYCLE")).CapitalizeFirst(), CS_0024_003C_003E8__locals28._003C_003E4__this.parent, MessageTypeDefOf.SilentInput, historical: false);
									}
								}
								else
								{
									Find.WindowStack.Add(new FloatMenu(options));
								}
							},
							activateSound = SoundDefOf.Tick_Tiny,
							Disabled = (text != null)
						};
						List<FloatMenuOption> options2;
						if (text != null)
						{
							command_Action.Disable(text);
						}
						else if (!CS_0024_003C_003E8__locals57.SelectPawnsForCycleOptions(CS_0024_003C_003E8__locals28.cycle, out options2, shortCircuit: true))
						{
							command_Action.Disable((CS_0024_003C_003E8__locals57.biotunedTo != null) ? "BiosculpterNoEligiblePawnsBiotuned".Translate(CS_0024_003C_003E8__locals57.biotunedTo.Named("PAWN")) : "BiosculpterNoEligiblePawns".Translate());
						}
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = default(List<CompBiosculpterPod_Cycle>.Enumerator);
					if (_003Cstate_003E5__2 == BiosculpterPodState.Occupied)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "BiosculpterInteruptCycle".Translate(),
							defaultDesc = "BiosculpterInteruptCycleDesc".Translate(),
							icon = InterruptCycleIcon,
							action = delegate
							{
								CS_0024_003C_003E8__locals57.EjectContents(interrupted: true, playSounds: true);
							},
							activateSound = SoundDefOf.Designate_Cancel
						};
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0296;
					IL_0579:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: complete cycle",
							action = delegate
							{
								CS_0024_003C_003E8__locals57.currentCycleTicksRemaining = 10f;
							},
							Disabled = (CS_0024_003C_003E8__locals57.State != BiosculpterPodState.Occupied)
						};
						_003C_003E1__state = 7;
						return true;
					}
					break;
					IL_04e3:
					_003C_003E7__wrap4 = StorageSettingsClipboard.CopyPasteGizmosFor(CS_0024_003C_003E8__locals57.allowedNutritionSettings).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_052e;
					IL_052e:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap4 = null;
					gizmo = Building.SelectContainedItemGizmo(CS_0024_003C_003E8__locals57.parent, CS_0024_003C_003E8__locals57.Occupant);
					if (gizmo != null)
					{
						_003C_003E2__current = gizmo;
						_003C_003E1__state = 6;
						return true;
					}
					goto IL_0579;
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
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
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
			_003CCompGetGizmosExtra_003Ed__91 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__91(0)
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

	private const int NoPowerEjectCumulativeTicks = 60000;

	private const int BiotunedDuration = 4800000;

	private const float NutritionRequired = 5f;

	private const float CacheForSecs = 2f;

	private static readonly Texture2D InterruptCycleIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	private static readonly Material BackgroundMat = SolidColorMaterials.NewSolidColorMaterial(new Color(0.082f, 0.078f, 0.063f), ShaderDatabase.SolidColorBehind);

	private const float BackgroundRect_YOff = 0.07317074f;

	private const float Pawn_YOff = 0.03658537f;

	private string currentCycleKey;

	private float currentCycleTicksRemaining;

	private int currentCyclePowerCutTicks;

	private ThingOwner innerContainer;

	private Pawn biotunedTo;

	private int biotunedCountdownTicks;

	private StorageSettings allowedNutritionSettings;

	private float liquifiedNutrition;

	public bool autoLoadNutrition = true;

	public bool devFillPodLatch;

	private bool autoAgeReversal;

	private int tickEntered = -99999;

	public Job queuedEnterJob;

	public Pawn queuedPawn;

	private List<ThingCount> chosenExtraItems = new List<ThingCount>();

	private List<FloatMenuOption> cycleEligiblePawnOptions = new List<FloatMenuOption>();

	private Pawn pawnEnteringBiosculpter;

	private Dictionary<CompBiosculpterPod_Cycle, List<IngredientCount>> cachedExtraIngredients = new Dictionary<CompBiosculpterPod_Cycle, List<IngredientCount>>();

	private Dictionary<CompBiosculpterPod_Cycle, CacheAnyPawnEligibleCycle> cachedAnyPawnEligible = new Dictionary<CompBiosculpterPod_Cycle, CacheAnyPawnEligibleCycle>();

	private static Dictionary<Pawn, List<CompBiosculpterPod>> cachedBiotunedPods = new Dictionary<Pawn, List<CompBiosculpterPod>>();

	private Pawn cacheReachIngredientsPawn;

	private CompBiosculpterPod_Cycle cacheReachIngredientsCycle;

	private float cacheReachIngredientsTime = float.MinValue;

	private bool cacheReachIngredientsResult;

	private Effecter progressBarEffecter;

	private Effecter operatingEffecter;

	private Effecter readyEffecter;

	private Texture2D cachedAutoAgeReverseIcon;

	private List<CompBiosculpterPod_Cycle> cachedAvailableCycles;

	private Dictionary<string, CompBiosculpterPod_Cycle> cycleLookup;

	private static string cachedAgeReversalCycleKey = null;

	private List<string> tmpIngredientsStrings = new List<string>();

	private static readonly List<Thing> tmpItems = new List<Thing>();

	private CompPowerTrader powerTraderComp;

	private CompPower powerComp;

	private static List<ThingDef> cachedPodDefs;

	public CompProperties_BiosculpterPod Props => props as CompProperties_BiosculpterPod;

	public ThingOwner SearchableContents => innerContainer;

	public bool IsContentsSuspended => true;

	public float RequiredNutritionRemaining => Mathf.Max(5f - liquifiedNutrition, 0f);

	public bool NutritionLoaded => RequiredNutritionRemaining <= 0f;

	public bool AutoAgeReversal => autoAgeReversal;

	private Texture2D AutoAgeReversalIcon
	{
		get
		{
			if (cachedAutoAgeReverseIcon == null)
			{
				cachedAutoAgeReverseIcon = ContentFinder<Texture2D>.Get("UI/Gizmos/BiosculpterAutoAgeReversal");
			}
			return cachedAutoAgeReverseIcon;
		}
	}

	public BiosculpterPodState State
	{
		get
		{
			if (Occupant != null)
			{
				return BiosculpterPodState.Occupied;
			}
			if (NutritionLoaded)
			{
				return BiosculpterPodState.SelectingCycle;
			}
			return BiosculpterPodState.LoadingNutrition;
		}
	}

	public Pawn Occupant
	{
		get
		{
			if (pawnEnteringBiosculpter != null)
			{
				return pawnEnteringBiosculpter;
			}
			if (currentCycleKey == null)
			{
				return null;
			}
			if (innerContainer.Count != 1)
			{
				return null;
			}
			return innerContainer[0] as Pawn;
		}
	}

	public CompBiosculpterPod_Cycle CurrentCycle
	{
		get
		{
			if (currentCycleKey == null)
			{
				return null;
			}
			foreach (CompBiosculpterPod_Cycle availableCycle in AvailableCycles)
			{
				if (availableCycle.Props.key == currentCycleKey)
				{
					return availableCycle;
				}
			}
			return null;
		}
	}

	public List<CompBiosculpterPod_Cycle> AvailableCycles
	{
		get
		{
			if (cachedAvailableCycles == null)
			{
				SetupCycleCaches();
			}
			return cachedAvailableCycles;
		}
	}

	public string AgeReversalCycleKey
	{
		get
		{
			if (cachedAgeReversalCycleKey == null)
			{
				SetupCycleCaches();
			}
			return cachedAgeReversalCycleKey;
		}
	}

	private float CycleSpeedFactorNoPawn => CleanlinessSpeedFactor * BiotunedSpeedFactor;

	public float CycleSpeedFactor
	{
		get
		{
			if (Occupant == null)
			{
				return Mathf.Max(0.1f, CycleSpeedFactorNoPawn);
			}
			return GetCycleSpeedFactorForPawn(Occupant);
		}
	}

	private float CleanlinessSpeedFactor => parent.GetStatValue(StatDefOf.BiosculpterPodSpeedFactor);

	private float BiotunedSpeedFactor
	{
		get
		{
			if (biotunedTo == null)
			{
				return 1f;
			}
			return Props.biotunedCycleSpeedFactor;
		}
	}

	public bool PowerOn => parent.TryGetComp<CompPowerTrader>().PowerOn;

	public float HeldPawnDrawPos_Y => parent.DrawPos.y - 0.03658537f;

	public float HeldPawnBodyAngle => parent.Rotation.Opposite.AsAngle;

	public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;

	public bool StorageTabVisible => true;

	public CompBiosculpterPod()
	{
		innerContainer = new ThingOwner<Thing>(this);
	}

	public override void Initialize(CompProperties props)
	{
		base.Initialize(props);
		allowedNutritionSettings = new StorageSettings(this);
		if (parent.def.building.defaultStorageSettings != null)
		{
			allowedNutritionSettings.CopyFrom(parent.def.building.defaultStorageSettings);
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (ModLister.CheckIdeology("Biosculpter pod comp"))
		{
			base.PostSpawnSetup(respawningAfterLoad);
		}
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
		Scribe_Values.Look(ref currentCycleKey, "currentCycleKey");
		Scribe_Values.Look(ref currentCycleTicksRemaining, "currentCycleTicksRemaining", 0f);
		Scribe_Values.Look(ref currentCyclePowerCutTicks, "currentCyclePowerCutTicks", 0);
		Scribe_References.Look(ref biotunedTo, "biotunedTo");
		Scribe_Values.Look(ref biotunedCountdownTicks, "biotunedCountdownTicks", 0);
		Scribe_Deep.Look(ref allowedNutritionSettings, "allowedNutritionSettings");
		Scribe_Values.Look(ref liquifiedNutrition, "liquifiedNutrition", 0f);
		Scribe_Values.Look(ref autoLoadNutrition, "autoLoadNutrition", defaultValue: false);
		Scribe_Values.Look(ref devFillPodLatch, "devFillPodLatch", defaultValue: false);
		Scribe_Values.Look(ref autoAgeReversal, "autoAgeReversal", defaultValue: false);
		Scribe_Values.Look(ref tickEntered, "tickEntered", 0);
		Scribe_References.Look(ref queuedEnterJob, "queuedEnterJob");
		Scribe_References.Look(ref queuedPawn, "queuedPawn");
		if (allowedNutritionSettings == null)
		{
			allowedNutritionSettings = new StorageSettings(this);
			if (parent.def.building.defaultStorageSettings != null)
			{
				allowedNutritionSettings.CopyFrom(parent.def.building.defaultStorageSettings);
			}
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			if (currentCycleKey == "healing")
			{
				currentCycleKey = "medic";
			}
			if (biotunedTo != null)
			{
				SetBiotuned(biotunedTo);
			}
			LiquifyNutrition();
		}
	}

	public CompBiosculpterPod_Cycle GetCycle(string key)
	{
		if (cycleLookup == null)
		{
			SetupCycleCaches();
		}
		return cycleLookup[key];
	}

	public float GetCycleSpeedFactorForPawn(Pawn p)
	{
		return Mathf.Max(0.1f, CycleSpeedFactorNoPawn * p.GetStatValue(StatDefOf.BiosculpterOccupantSpeed));
	}

	private void SetupCycleCaches()
	{
		cachedAvailableCycles = new List<CompBiosculpterPod_Cycle>();
		cachedAvailableCycles.AddRange(parent.AllComps.OfType<CompBiosculpterPod_Cycle>());
		cycleLookup = new Dictionary<string, CompBiosculpterPod_Cycle>();
		foreach (CompBiosculpterPod_Cycle cachedAvailableCycle in cachedAvailableCycles)
		{
			if (cachedAvailableCycle is CompBiosculpterPod_AgeReversalCycle compBiosculpterPod_AgeReversalCycle)
			{
				cachedAgeReversalCycleKey = compBiosculpterPod_AgeReversalCycle.Props.key;
			}
			cycleLookup[cachedAvailableCycle.Props.key] = cachedAvailableCycle;
		}
	}

	public void SetBiotuned(Pawn newBiotunedTo)
	{
		if (newBiotunedTo != biotunedTo)
		{
			autoAgeReversal = false;
		}
		if (biotunedTo != null && cachedBiotunedPods.ContainsKey(biotunedTo))
		{
			cachedBiotunedPods[biotunedTo].Remove(this);
		}
		if (newBiotunedTo != null && !cachedBiotunedPods.ContainsKey(newBiotunedTo))
		{
			cachedBiotunedPods[newBiotunedTo] = new List<CompBiosculpterPod>();
		}
		if (newBiotunedTo != null && !cachedBiotunedPods[newBiotunedTo].Contains(this))
		{
			cachedBiotunedPods[newBiotunedTo].Add(this);
		}
		if (newBiotunedTo != null && newBiotunedTo != biotunedTo)
		{
			biotunedCountdownTicks = 4800000;
		}
		biotunedTo = newBiotunedTo;
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		SetBiotuned(null);
		if (mode == DestroyMode.Deconstruct || mode == DestroyMode.KillFinalize)
		{
			EjectContents(interrupted: true, playSounds: false, previousMap);
		}
		innerContainer.ClearAndDestroyContents();
		base.PostDestroy(mode, previousMap);
	}

	public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			EjectContents(interrupted: true, playSounds: false, map);
			currentCycleKey = null;
		}
		progressBarEffecter?.Cleanup();
		progressBarEffecter = null;
		operatingEffecter?.Cleanup();
		operatingEffecter = null;
		readyEffecter?.Cleanup();
		readyEffecter = null;
	}

	public override void DrawGUIOverlay()
	{
		base.DrawGUIOverlay();
		if (!Find.ScreenshotModeHandler.Active && (biotunedTo != null || Occupant != null))
		{
			GenMapUI.DrawThingLabel(parent, biotunedTo?.LabelShort ?? Occupant.LabelShort, GenMapUI.DefaultThingLabelColor);
		}
	}

	public override string CompInspectStringExtra()
	{
		StringBuilder stringBuilder = new StringBuilder();
		BiosculpterPodState state = State;
		if (parent.Spawned)
		{
			CompBiosculpterPod_Cycle currentCycle = CurrentCycle;
			if (currentCycle != null)
			{
				stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterPodCycleLabel".Translate()).Append(": ")
					.Append(currentCycle.Props.LabelCap);
				if (biotunedTo == null)
				{
					stringBuilder.Append(" " + "BiosculpterPodCycleWillBiotune".Translate());
				}
			}
			else if (state == BiosculpterPodState.SelectingCycle)
			{
				if (PowerOn)
				{
					if (queuedEnterJob != null && !queuedEnterJob.biosculpterCycleKey.NullOrEmpty())
					{
						stringBuilder.Append("BiosculpterPodCycleStandby".Translate(GetCycle(queuedEnterJob.biosculpterCycleKey).Props.label.Named("CYCLE"), queuedPawn.Named("PAWN")));
					}
					else
					{
						stringBuilder.Append("BiosculpterPodCycleSelection".Translate().CapitalizeFirst());
					}
				}
				else
				{
					stringBuilder.Append("BiosculpterPodCycleSelectionNoPower".Translate().CapitalizeFirst());
				}
			}
			if (state == BiosculpterPodState.LoadingNutrition)
			{
				stringBuilder.Append("BiosculpterPodCycleLabelLoading".Translate().CapitalizeFirst());
				stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ")
					.Append(liquifiedNutrition.ToStringByStyle(ToStringStyle.FloatMaxOne))
					.Append(" / ")
					.Append(5f);
			}
			if (state == BiosculpterPodState.Occupied)
			{
				float num = currentCycleTicksRemaining / CycleSpeedFactor;
				stringBuilder.AppendLineIfNotEmpty().Append("Contains".Translate()).Append(": ")
					.Append(Occupant.NameShortColored.Resolve());
				if (!PowerOn)
				{
					stringBuilder.AppendLine().Append("BiosculpterCycleNoPowerInterrupt".Translate((60000 - currentCyclePowerCutTicks).ToStringTicksToPeriod().Named("TIME")).Colorize(ColorLibrary.RedReadable));
				}
				stringBuilder.AppendLine().Append("BiosculpterCycleTimeRemaining".Translate()).Append(": ")
					.Append(((int)num).ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor));
				Ideo ideo = Occupant.Ideo;
				if (ideo != null && ideo.HasPrecept(PreceptDefOf.Biosculpting_Accelerated))
				{
					stringBuilder.Append(" (" + "BiosculpterCycleAccelerated".Translate() + ")");
				}
				if (biotunedTo != null)
				{
					stringBuilder.AppendLine().Append("BiosculpterBiotunedSpeedFactor".Translate()).Append(": ")
						.Append(BiotunedSpeedFactor.ToStringPercent());
				}
				stringBuilder.AppendLine().Append("BiosculpterCleanlinessSpeedFactor".Translate()).Append(": ")
					.Append(CleanlinessSpeedFactor.ToStringPercent());
			}
		}
		if (biotunedTo != null && state != BiosculpterPodState.Occupied)
		{
			stringBuilder.AppendLineIfNotEmpty().Append("BiosculpterBiotunedTo".Translate()).Append(": ")
				.Append(biotunedTo.LabelShort)
				.Append(" (")
				.Append(biotunedCountdownTicks.ToStringTicksToPeriod())
				.Append(")");
		}
		if (stringBuilder.Length <= 0)
		{
			return null;
		}
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__91))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__91(-2)
		{
			_003C_003E4__this = this
		};
	}

	private string IngredientsDescription(CompBiosculpterPod_Cycle cycle)
	{
		tmpIngredientsStrings.Clear();
		if (!cycle.Props.extraRequiredIngredients.NullOrEmpty() && !devFillPodLatch)
		{
			for (int i = 0; i < cycle.Props.extraRequiredIngredients.Count; i++)
			{
				tmpIngredientsStrings.Add(cycle.Props.extraRequiredIngredients[i].Summary);
			}
		}
		return tmpIngredientsStrings.ToCommaList(useAnd: true);
	}

	private string CycleDescription(CompBiosculpterPod_Cycle cycle)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(cycle.Description(biotunedTo));
		float num = cycle.Props.durationDays / CycleSpeedFactor;
		float num2 = num / PreceptDefOf.Biosculpting_Accelerated.biosculpterPodCycleSpeedFactor;
		stringBuilder.AppendLine("\n\n" + "BiosculpterPodCycleDuration".Translate() + ": " + ((int)(num * 60000f)).ToStringTicksToDays());
		if (!Find.IdeoManager.classicMode)
		{
			stringBuilder.Append("BiosculpterPodCycleDurationTranshumanists".Translate() + ": " + ((int)(num2 * 60000f)).ToStringTicksToDays());
		}
		return stringBuilder.ToString();
	}

	public bool PawnCanUseNow(Pawn pawn, CompBiosculpterPod_Cycle cycle)
	{
		return (CannotUseNowReason() ?? CannotUseNowPawnReason(pawn) ?? CannotUseNowCycleReason(cycle) ?? CannotUseNowPawnCycleReason(pawn, cycle)) == null;
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__96))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__96(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public static bool CanAgeReverse(Pawn biosculptee)
	{
		return biosculptee.ageTracker.Adult;
	}

	public static List<CompBiosculpterPod> BiotunedPods(Pawn pawn)
	{
		return cachedBiotunedPods.TryGetValue(pawn);
	}

	public static bool HasBiotunedAutoAgeReversePod(Pawn pawn)
	{
		List<CompBiosculpterPod> list = cachedBiotunedPods.TryGetValue(pawn);
		if (list == null)
		{
			return false;
		}
		foreach (CompBiosculpterPod item in list)
		{
			if (item.AutoAgeReversal)
			{
				return true;
			}
		}
		return false;
	}

	public static string CannotStartText(CompBiosculpterPod_Cycle cycle, string translatedReason)
	{
		return "BiosculpterCannotStartCycle".Translate(cycle.Props.label) + ": " + translatedReason.CapitalizeFirst();
	}

	public string CannotUseNowCycleReason(CompBiosculpterPod_Cycle cycle)
	{
		List<string> list = cycle.MissingResearchLabels();
		if (list.Any())
		{
			return "MissingRequiredResearch".Translate() + " " + list.ToCommaList();
		}
		return null;
	}

	public string CannotUseNowPawnCycleReason(Pawn p, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
	{
		return CannotUseNowPawnCycleReason(p, p, cycle, checkIngredients);
	}

	private bool CanReachOrHasIngredients(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool useCache = false)
	{
		if (!PawnCarryingExtraCycleIngredients(biosculptee, cycle) && (biosculptee == hauler || !PawnCarryingExtraCycleIngredients(hauler, cycle)))
		{
			return CanReachRequiredIngredients(hauler, cycle, useCache);
		}
		return true;
	}

	public string CannotUseNowPawnCycleReason(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, bool checkIngredients = true)
	{
		if (AgeReversalCycleKey != null && cycle.Props.key == AgeReversalCycleKey && !CanAgeReverse(biosculptee))
		{
			return "UnderMinBiosculpterAgeReversalAge".Translate(biosculptee.ageTracker.AdultMinAge.Named("ADULTAGE")).CapitalizeFirst();
		}
		if (checkIngredients && !CanReachOrHasIngredients(hauler, biosculptee, cycle, useCache: true))
		{
			return "BiosculpterMissingIngredients".Translate(IngredientsDescription(cycle).Named("INGREDIENTS")).CapitalizeFirst();
		}
		return null;
	}

	public string CannotUseNowPawnReason(Pawn p)
	{
		if (biotunedTo != null && biotunedTo != p)
		{
			return "BiosculpterBiotunedToAnother".Translate().CapitalizeFirst();
		}
		if (!p.CanReach(parent, PathEndMode.InteractionCell, Danger.Deadly))
		{
			return "NoPath".Translate().CapitalizeFirst();
		}
		return null;
	}

	public string CannotUseNowReason()
	{
		if (!PowerOn)
		{
			return "NoPower".Translate().CapitalizeFirst();
		}
		if (State == BiosculpterPodState.LoadingNutrition)
		{
			return "BiosculpterNutritionNotLoaded".Translate().CapitalizeFirst();
		}
		if (State == BiosculpterPodState.Occupied)
		{
			return "BiosculpterOccupied".Translate().CapitalizeFirst();
		}
		return null;
	}

	private List<IngredientCount> RequiredIngredients(CompBiosculpterPod_Cycle cycle)
	{
		List<ThingDefCountClass> extraRequiredIngredients = cycle.Props.extraRequiredIngredients;
		if (extraRequiredIngredients == null || devFillPodLatch)
		{
			return null;
		}
		if (!cachedExtraIngredients.ContainsKey(cycle))
		{
			cachedExtraIngredients[cycle] = extraRequiredIngredients.Select((ThingDefCountClass tc) => tc.ToIngredientCount()).ToList();
		}
		return cachedExtraIngredients[cycle];
	}

	private bool CanReachRequiredIngredients(Pawn pawn, CompBiosculpterPod_Cycle cycle, bool useCache = false)
	{
		chosenExtraItems.Clear();
		if (cycle.Props.extraRequiredIngredients == null || devFillPodLatch)
		{
			return true;
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (useCache && cacheReachIngredientsPawn == pawn && cacheReachIngredientsCycle == cycle && realtimeSinceStartup < cacheReachIngredientsTime + 2f)
		{
			return cacheReachIngredientsResult;
		}
		cacheReachIngredientsPawn = pawn;
		cacheReachIngredientsCycle = cycle;
		cacheReachIngredientsTime = realtimeSinceStartup;
		cacheReachIngredientsResult = WorkGiver_DoBill.TryFindBestFixedIngredients(RequiredIngredients(cycle), pawn, parent, chosenExtraItems);
		return cacheReachIngredientsResult;
	}

	private bool SelectPawnCycleOption(Pawn pawn, CompBiosculpterPod_Cycle cycle, out FloatMenuOption option)
	{
		string text = CannotUseNowPawnReason(pawn) ?? CannotUseNowPawnCycleReason(pawn, cycle, checkIngredients: false);
		string label = pawn.Label + ((text == null) ? "" : (": " + text));
		Action action = null;
		if (text == null)
		{
			action = delegate
			{
				PrepareCycleJob(pawn, pawn, cycle, EnterBiosculpterJob());
			};
		}
		option = new FloatMenuOption(label, action);
		return text == null;
	}

	private bool SelectPawnsForCycleOptions(CompBiosculpterPod_Cycle cycle, out List<FloatMenuOption> options, bool shortCircuit = false)
	{
		cycleEligiblePawnOptions.Clear();
		options = cycleEligiblePawnOptions;
		if (!cachedAnyPawnEligible.ContainsKey(cycle))
		{
			cachedAnyPawnEligible[cycle] = new CacheAnyPawnEligibleCycle
			{
				gameTime = float.MinValue
			};
		}
		int ticksGame = Find.TickManager.TicksGame;
		if (shortCircuit && (float)ticksGame < cachedAnyPawnEligible[cycle].gameTime + 2f)
		{
			return cachedAnyPawnEligible[cycle].anyEligible;
		}
		cachedAnyPawnEligible[cycle].gameTime = ticksGame;
		if (biotunedTo != null)
		{
			if (biotunedTo.Dead || !biotunedTo.Spawned || biotunedTo.Map != parent.Map)
			{
				cachedAnyPawnEligible[cycle].anyEligible = false;
				return cachedAnyPawnEligible[cycle].anyEligible;
			}
			if (SelectPawnCycleOption(biotunedTo, cycle, out var option) && shortCircuit)
			{
				cachedAnyPawnEligible[cycle].anyEligible = true;
				return cachedAnyPawnEligible[cycle].anyEligible;
			}
			cycleEligiblePawnOptions.Add(option);
		}
		else
		{
			foreach (Pawn item in parent.Map.mapPawns.FreeColonistsSpawned)
			{
				if (!item.IsQuestLodger())
				{
					if (SelectPawnCycleOption(item, cycle, out var option2) && shortCircuit)
					{
						cachedAnyPawnEligible[cycle].anyEligible = true;
						return cachedAnyPawnEligible[cycle].anyEligible;
					}
					cycleEligiblePawnOptions.Add(option2);
				}
			}
		}
		cachedAnyPawnEligible[cycle].anyEligible = cycleEligiblePawnOptions.Count > 0;
		return cachedAnyPawnEligible[cycle].anyEligible;
	}

	public Job EnterBiosculpterJob()
	{
		return JobMaker.MakeJob(JobDefOf.EnterBiosculpterPod, parent);
	}

	public Job MakeCarryToBiosculpterJob(Pawn willBeCarried)
	{
		return JobMaker.MakeJob(JobDefOf.CarryToBiosculpterPod, willBeCarried, LocalTargetInfo.Invalid, parent);
	}

	public void ConfigureJobForCycle(Job job, CompBiosculpterPod_Cycle cycle, List<ThingCount> extraIngredients)
	{
		if (!extraIngredients.NullOrEmpty())
		{
			job.targetQueueB = new List<LocalTargetInfo>(extraIngredients.Count);
			job.countQueue = new List<int>(extraIngredients.Count);
			foreach (ThingCount extraIngredient in extraIngredients)
			{
				job.targetQueueB.Add(extraIngredient.Thing);
				job.countQueue.Add(extraIngredient.Count);
			}
		}
		job.haulMode = HaulMode.ToCellNonStorage;
		job.biosculpterCycleKey = cycle.Props.key;
	}

	public void PrepareCycleJob(Pawn hauler, Pawn biosculptee, CompBiosculpterPod_Cycle cycle, Job job)
	{
		OrderToPod(cycle, biosculptee, delegate
		{
			chosenExtraItems.Clear();
			if (!CanReachOrHasIngredients(hauler, biosculptee, cycle))
			{
				Messages.Message("BiosculpterMissingIngredients".Translate(IngredientsDescription(cycle).Named("INGREDIENTS")).CapitalizeFirst(), parent, MessageTypeDefOf.NegativeEvent, historical: false);
			}
			else
			{
				ConfigureJobForCycle(job, cycle, chosenExtraItems);
				if (cycle.Props.extraRequiredIngredients != null && !devFillPodLatch)
				{
					if (job.def == JobDefOf.CarryToBiosculpterPod)
					{
						Messages.Message("BiosculpterCarryStartedMessage".Translate(hauler.Named("PAWN"), IngredientsDescription(cycle).Named("INGREDIENTS"), biosculptee.Named("DOWNED"), cycle.Props.label.Named("CYCLE")), parent, MessageTypeDefOf.SilentInput, historical: false);
					}
					else
					{
						Messages.Message("BiosculpterLoadingStartedMessage".Translate(hauler.Named("PAWN"), IngredientsDescription(cycle).Named("INGREDIENTS"), cycle.Props.label.Named("CYCLE")), parent, MessageTypeDefOf.SilentInput, historical: false);
					}
				}
				if (hauler.jobs.TryTakeOrderedJob(job, JobTag.Misc))
				{
					SetQueuedInformation(job, biosculptee);
				}
			}
		});
	}

	public void ClearQueuedInformation()
	{
		SetQueuedInformation(null, null);
	}

	public void SetQueuedInformation(Job job, Pawn biosculptee)
	{
		queuedEnterJob = job;
		queuedPawn = biosculptee;
	}

	public bool CanAcceptNutrition(Thing thing)
	{
		return allowedNutritionSettings.AllowedToAccept(thing);
	}

	public bool CanAcceptOnceCycleChosen(Pawn pawn)
	{
		if (State != BiosculpterPodState.SelectingCycle || !PowerOn)
		{
			return false;
		}
		if (biotunedTo != null && biotunedTo != pawn)
		{
			return false;
		}
		return true;
	}

	public bool PawnCarryingExtraCycleIngredients(Pawn pawn, string cycleKey, bool remove = false)
	{
		return PawnCarryingExtraCycleIngredients(pawn, GetCycle(cycleKey), remove);
	}

	public bool PawnCarryingExtraCycleIngredients(Pawn pawn, CompBiosculpterPod_Cycle cycle, bool remove = false)
	{
		if (cycle.Props.extraRequiredIngredients.NullOrEmpty() || devFillPodLatch)
		{
			return true;
		}
		foreach (ThingDefCountClass extraRequiredIngredient in cycle.Props.extraRequiredIngredients)
		{
			if (pawn.inventory.Count(extraRequiredIngredient.thingDef) < extraRequiredIngredient.count)
			{
				return false;
			}
		}
		if (remove)
		{
			foreach (ThingDefCountClass extraRequiredIngredient2 in cycle.Props.extraRequiredIngredients)
			{
				pawn.inventory.RemoveCount(extraRequiredIngredient2.thingDef, extraRequiredIngredient2.count);
			}
		}
		return true;
	}

	public bool TryAcceptPawn(Pawn pawn, string cycleKey)
	{
		return TryAcceptPawn(pawn, GetCycle(cycleKey));
	}

	public bool TryAcceptPawn(Pawn pawn, CompBiosculpterPod_Cycle cycle)
	{
		if (!CanAcceptOnceCycleChosen(pawn))
		{
			return false;
		}
		if (!PawnCarryingExtraCycleIngredients(pawn, cycle, remove: true))
		{
			return false;
		}
		currentCycleKey = cycle.Props.key;
		innerContainer.ClearAndDestroyContents();
		pawnEnteringBiosculpter = pawn;
		bool num = pawn.DeSpawnOrDeselect();
		if (pawn.holdingOwner != null)
		{
			pawn.holdingOwner.TryTransferToContainer(pawn, innerContainer);
		}
		else
		{
			innerContainer.TryAdd(pawn);
		}
		if (num)
		{
			Find.Selector.Select(pawn, playSound: false, forceDesignatorDeselect: false);
		}
		pawnEnteringBiosculpter = null;
		currentCycleTicksRemaining = cycle.Props.durationDays * 60000f;
		liquifiedNutrition = 0f;
		devFillPodLatch = false;
		ClearQueuedInformation();
		tickEntered = Find.TickManager.TicksGame;
		return true;
	}

	public void EjectContents(bool interrupted, bool playSounds, Map destMap = null)
	{
		if (destMap == null)
		{
			destMap = parent.Map;
		}
		Pawn occupant = Occupant;
		currentCycleKey = null;
		currentCycleTicksRemaining = 0f;
		currentCyclePowerCutTicks = 0;
		liquifiedNutrition = 0f;
		devFillPodLatch = false;
		innerContainer.TryDropAll(parent.InteractionCell, destMap, ThingPlaceMode.Near);
		if (occupant != null)
		{
			FilthMaker.TryMakeFilth(parent.InteractionCell, destMap, ThingDefOf.Filth_PodSlime, new IntRange(3, 6).RandomInRange);
			if (interrupted)
			{
				occupant.needs?.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SoakingWet);
				occupant.health?.AddHediff(HediffDefOf.BiosculptingSickness, null, null);
			}
		}
		if (playSounds)
		{
			Props.exitSound?.PlayOneShot(SoundInfo.InMap(new TargetInfo(parent.Position, parent.Map)));
		}
	}

	private void CycleCompleted()
	{
		Pawn occupant = Occupant;
		CompBiosculpterPod_Cycle currentCycle = CurrentCycle;
		SetBiotuned(occupant);
		currentCycle.CycleCompleted(occupant);
		EjectContents(interrupted: false, playSounds: true);
		if (occupant != null)
		{
			Need_Food need_Food = occupant.needs?.food;
			if (need_Food != null)
			{
				need_Food.CurLevelPercentage = 1f;
			}
			Need_Rest need_Rest = occupant.needs?.rest;
			if (need_Rest != null)
			{
				need_Rest.CurLevelPercentage = 1f;
			}
			if (currentCycle.Props.gainThoughtOnCompletion != null)
			{
				occupant.needs?.mood?.thoughts.memories.TryGainMemory(ThoughtDefOf.AgeReversalReceived);
			}
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedBiosculpterPod, occupant.Named(HistoryEventArgsNames.Doer)));
		}
		if (tickEntered > 0)
		{
			occupant.drugs.Notify_LeftSuspension(Find.TickManager.TicksGame - tickEntered);
		}
	}

	private void LiquifyNutrition()
	{
		tmpItems.AddRange(innerContainer);
		foreach (Thing tmpItem in tmpItems)
		{
			float num = tmpItem.GetStatValue(StatDefOf.Nutrition) * (float)tmpItem.stackCount;
			if (!(num <= 0f) && !(tmpItem is Pawn))
			{
				liquifiedNutrition = Mathf.Min(5f, liquifiedNutrition + num);
				tmpItem.Destroy();
			}
		}
		tmpItems.Clear();
	}

	public override void CompTick()
	{
		if (!ModLister.CheckIdeology("Biosculpting"))
		{
			return;
		}
		base.CompTick();
		if (State != BiosculpterPodState.SelectingCycle || !PowerOn)
		{
			readyEffecter?.Cleanup();
			readyEffecter = null;
		}
		else if (Props.readyEffecter != null)
		{
			if (readyEffecter == null)
			{
				readyEffecter = Props.readyEffecter.Spawn();
				ColorizeEffecter(readyEffecter, Props.selectCycleColor);
				readyEffecter.Trigger(parent, new TargetInfo(parent.InteractionCell, parent.Map));
			}
			readyEffecter.EffectTick(parent, new TargetInfo(parent.InteractionCell, parent.Map));
		}
		if (State != BiosculpterPodState.Occupied)
		{
			progressBarEffecter?.Cleanup();
			progressBarEffecter = null;
			operatingEffecter?.Cleanup();
			operatingEffecter = null;
		}
		else
		{
			Pawn occupant = Occupant;
			biotunedCountdownTicks = 4800000;
			if (PowerOn)
			{
				int num = 1;
				currentCycleTicksRemaining -= (float)num * CycleSpeedFactor;
				if (currentCycleTicksRemaining <= 0f)
				{
					CycleCompleted();
				}
			}
			else
			{
				currentCyclePowerCutTicks++;
				if (currentCyclePowerCutTicks >= 60000)
				{
					EjectContents(interrupted: true, playSounds: true);
					Messages.Message("BiosculpterNoPowerEjectedMessage".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeEvent, historical: false);
				}
			}
			if (currentCycleTicksRemaining > 0f)
			{
				if (progressBarEffecter == null)
				{
					progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
				}
				progressBarEffecter.EffectTick(parent, TargetInfo.Invalid);
				MoteProgressBar moteProgressBar = (progressBarEffecter.children[0] as SubEffecter_ProgressBar)?.mote;
				if (moteProgressBar != null)
				{
					float num2 = CurrentCycle.Props.durationDays * 60000f;
					moteProgressBar.progress = 1f - Mathf.Clamp01(currentCycleTicksRemaining / num2);
					int num3 = (parent.RotatedSize.z - 1) / 2;
					moteProgressBar.offsetZ = 0f - ((float)num3 + 0.5f);
				}
				if (Props.operatingEffecter != null)
				{
					if (!PowerOn)
					{
						operatingEffecter?.Cleanup();
						operatingEffecter = null;
					}
					else
					{
						if (operatingEffecter == null)
						{
							operatingEffecter = Props.operatingEffecter.Spawn();
							ColorizeEffecter(operatingEffecter, CurrentCycle.Props.operatingColor);
							operatingEffecter.Trigger(parent, new TargetInfo(parent.InteractionCell, parent.Map));
						}
						operatingEffecter.EffectTick(parent, new TargetInfo(parent.InteractionCell, parent.Map));
					}
				}
			}
		}
		if (PowerOn && biotunedCountdownTicks > 0)
		{
			biotunedCountdownTicks--;
		}
		if (biotunedCountdownTicks <= 0)
		{
			SetBiotuned(null);
		}
		SetPower();
		if (biotunedTo?.Ideo != null && !biotunedTo.Ideo.HasPrecept(PreceptDefOf.AgeReversal_Demanded))
		{
			autoAgeReversal = false;
		}
	}

	private void SetPower()
	{
		if (powerTraderComp == null)
		{
			powerTraderComp = parent.TryGetComp<CompPowerTrader>();
		}
		if (powerComp == null)
		{
			powerComp = parent.TryGetComp<CompPower>();
		}
		if (State == BiosculpterPodState.Occupied)
		{
			powerTraderComp.PowerOutput = 0f - powerComp.Props.PowerConsumption;
		}
		else
		{
			powerTraderComp.PowerOutput = 0f - powerComp.Props.idlePowerDraw;
		}
	}

	private void ColorizeEffecter(Effecter effecter, Color color)
	{
		foreach (SubEffecter child in effecter.children)
		{
			if (child is SubEffecter_Sprayer subEffecter_Sprayer)
			{
				subEffecter_Sprayer.colorOverride = color * child.def.color;
			}
		}
	}

	public override void PostDraw()
	{
		base.PostDraw();
		Rot4 rotation = parent.Rotation;
		Vector3 s = new Vector3(parent.def.graphicData.drawSize.x * 0.8f, 1f, parent.def.graphicData.drawSize.y * 0.8f);
		Vector3 drawPos = parent.DrawPos;
		drawPos.y -= 0.07317074f;
		Graphics.DrawMesh(MeshPool.plane10, Matrix4x4.TRS(drawPos, rotation.AsQuat, s), BackgroundMat, 0);
		if (State == BiosculpterPodState.Occupied)
		{
			Pawn occupant = Occupant;
			Vector3 drawLoc = parent.DrawPos + FloatingOffset(currentCycleTicksRemaining + (float)currentCyclePowerCutTicks);
			Rot4 rotation2 = parent.Rotation;
			if (rotation2 == Rot4.East || rotation2 == Rot4.West)
			{
				drawLoc.z += 0.2f;
			}
			occupant.Drawer.renderer.RenderPawnAt(drawLoc, null, neverAimWeapon: true);
		}
	}

	public static Vector3 FloatingOffset(float tickOffset)
	{
		float num = tickOffset % 500f / 500f;
		float num2 = Mathf.Sin((float)Math.PI * num);
		float z = num2 * num2 * 0.04f;
		return new Vector3(0f, 0f, z);
	}

	public void GetChildHolders(List<IThingHolder> outChildren)
	{
		ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
	}

	public ThingOwner GetDirectlyHeldThings()
	{
		return innerContainer;
	}

	public StorageSettings GetStoreSettings()
	{
		return allowedNutritionSettings;
	}

	public StorageSettings GetParentStoreSettings()
	{
		return parent.def.building.fixedStorageSettings;
	}

	public void Notify_SettingsChanged()
	{
	}

	private static void OrderToPod(CompBiosculpterPod_Cycle cycle, Pawn pawn, Action giveJobAct)
	{
		if (cycle is CompBiosculpterPod_HealingCycle compBiosculpterPod_HealingCycle)
		{
			string healingDescriptionForPawn = compBiosculpterPod_HealingCycle.GetHealingDescriptionForPawn(pawn);
			string text = (healingDescriptionForPawn.NullOrEmpty() ? "BiosculpterNoCoditionsToHeal".Translate(pawn.Named("PAWN"), compBiosculpterPod_HealingCycle.Props.label.Named("CYCLE")).Resolve() : ("OnCompletionOfCycle".Translate(compBiosculpterPod_HealingCycle.Props.label.Named("CYCLE")).Resolve() + ":\n\n" + healingDescriptionForPawn));
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(text, giveJobAct, healingDescriptionForPawn.NullOrEmpty()));
		}
		else
		{
			giveJobAct();
		}
	}

	public static Thing FindPodFor(Pawn pawn, Pawn traveller, bool biotuned)
	{
		if (cachedPodDefs.NullOrEmpty())
		{
			cachedPodDefs = DefDatabase<ThingDef>.AllDefs.Where((ThingDef def) => def.GetCompProperties<CompProperties_BiosculpterPod>() != null).ToList();
		}
		foreach (ThingDef cachedPodDef in cachedPodDefs)
		{
			Thing thing = GenClosest.ClosestThingReachable(traveller.Position, pawn.Map, ThingRequest.ForDef(cachedPodDef), PathEndMode.InteractionCell, TraverseParms.For(pawn), 9999f, Validator);
			if (thing != null)
			{
				return thing;
			}
		}
		return null;
		bool Validator(Thing t)
		{
			CompBiosculpterPod compBiosculpterPod = t.TryGetComp<CompBiosculpterPod>();
			if (biotuned && compBiosculpterPod.biotunedTo != traveller)
			{
				return false;
			}
			return compBiosculpterPod.CanAcceptOnceCycleChosen(traveller);
		}
	}

	public static bool WasLoadingCanceled(Thing thing)
	{
		CompBiosculpterPod compBiosculpterPod = thing.TryGetComp<CompBiosculpterPod>();
		if (compBiosculpterPod != null && compBiosculpterPod.State != 0)
		{
			return true;
		}
		return false;
	}

	public void ClearCycle()
	{
		currentCycleKey = null;
	}

	public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
	{
		LiquifyNutrition();
		SoundDefOf.Standard_Drop.PlayOneShot(parent);
	}
}
