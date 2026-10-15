using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_GrowthVat : Building_Enterable, IStoreSettingsParent, IThingHolderWithDrawnPawn, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass86_0
	{
		public Pawn pawn;

		public Building_GrowthVat _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__8()
		{
			_003C_003E4__this.SelectPawn(pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass86_1
	{
		public List<FloatMenuOption> opts;

		public Building_GrowthVat _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__9()
		{
			Find.WindowStack.Add(new FloatMenu(opts));
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass86_2
	{
		public Thing embryo;

		public _003C_003Ec__DisplayClass86_1 CS_0024_003C_003E8__locals1;

		internal void _003CGetGizmos_003Eb__10()
		{
			CS_0024_003C_003E8__locals1._003C_003E4__this.SelectEmbryo(embryo as HumanEmbryo);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass93_0
	{
		public Building_GrowthVat _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.SelectPawn(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__93 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GrowthVat _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass93_0 _003C_003E8__1;

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
		public _003CGetFloatMenuOptions_003Ed__93(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				Building_GrowthVat building_GrowthVat = _003C_003E4__this;
				AcceptanceReport acceptanceReport;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass93_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E7__wrap1 = ((ThingWithComps)building_GrowthVat).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b2;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b2;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					_003C_003E1__state = -1;
					break;
				case 4:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00b2:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!_003C_003E8__1.selPawn.CanReach(building_GrowthVat, PathEndMode.InteractionCell, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnterBuilding".Translate(building_GrowthVat) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 2;
						return true;
					}
					acceptanceReport = building_GrowthVat.CanAcceptPawn(_003C_003E8__1.selPawn);
					if (acceptanceReport.Accepted)
					{
						_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(building_GrowthVat), delegate
						{
							_003C_003E8__1._003C_003E4__this.SelectPawn(_003C_003E8__1.selPawn);
						}), _003C_003E8__1.selPawn, building_GrowthVat);
						_003C_003E1__state = 3;
						return true;
					}
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnterBuilding".Translate(building_GrowthVat) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
						_003C_003E1__state = 4;
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
			_003CGetFloatMenuOptions_003Ed__93 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__93(0)
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
	private sealed class _003CGetGizmos_003Ed__86 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GrowthVat _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__86(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				Building_GrowthVat CS_0024_003C_003E8__locals67 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building_Enterable)CS_0024_003C_003E8__locals67).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_009e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_009e;
				case 2:
					_003C_003E1__state = -4;
					goto IL_0103;
				case 3:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals67.selectedEmbryo != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "InspectGenes".Translate() + "...",
							defaultDesc = "InspectGenesEmbryoDesc".Translate(),
							icon = GeneSetHolderBase.GeneticInfoTex.Texture,
							action = delegate
							{
								InspectPaneUtility.OpenTab(typeof(ITab_Genes));
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_0237;
				case 4:
					_003C_003E1__state = -1;
					goto IL_0237;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Learn",
						action = ((Hediff_VatLearning)CS_0024_003C_003E8__locals67.VatLearning).Learn
					};
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					goto IL_02e6;
				case 7:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Embryo birth now",
						action = delegate
						{
							CS_0024_003C_003E8__locals67.startTick = Find.TickManager.TicksGame;
							CS_0024_003C_003E8__locals67.Finish();
						}
					};
					_003C_003E1__state = 8;
					return true;
				case 8:
					_003C_003E1__state = -1;
					goto IL_0723;
				case 9:
					_003C_003E1__state = -1;
					goto IL_0412;
				case 10:
					_003C_003E1__state = -1;
					goto IL_0520;
				case 11:
					_003C_003E1__state = -1;
					goto IL_0723;
				case 12:
					_003C_003E1__state = -1;
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Empty nutrition",
						action = delegate
						{
							CS_0024_003C_003E8__locals67.containedNutrition = 0f;
						}
					};
					_003C_003E1__state = 13;
					return true;
				case 13:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_009e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = StorageSettingsClipboard.CopyPasteGizmosFor(CS_0024_003C_003E8__locals67.allowedNutritionSettings).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0103;
					IL_0237:
					if (DebugSettings.ShowDevGizmos)
					{
						if (CS_0024_003C_003E8__locals67.selectedPawn != null && CS_0024_003C_003E8__locals67.innerContainer.Contains(CS_0024_003C_003E8__locals67.selectedPawn))
						{
							_003C_003E2__current = new Command_Action
							{
								defaultLabel = "DEV: Advance 1 year",
								action = delegate
								{
									CS_0024_003C_003E8__locals67.selectedPawn.ageTracker.Notify_TickedInGrowthVat(3600000);
								}
							};
							_003C_003E1__state = 5;
							return true;
						}
						goto IL_02e6;
					}
					goto IL_0723;
					IL_0103:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals67.Working)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandCancelGrowth".Translate(),
							defaultDesc = "CommandCancelGrowthDesc".Translate(),
							icon = CancelLoadingIcon,
							activateSound = SoundDefOf.Designate_Cancel,
							action = delegate
							{
								Action action = delegate
								{
									CS_0024_003C_003E8__locals67.Finish();
									CS_0024_003C_003E8__locals67.innerContainer.TryDropAll(CS_0024_003C_003E8__locals67.InteractionCell, CS_0024_003C_003E8__locals67.Map, ThingPlaceMode.Near);
								};
								if (CS_0024_003C_003E8__locals67.startTick > Find.TickManager.TicksGame && CS_0024_003C_003E8__locals67.selectedEmbryo != null && CS_0024_003C_003E8__locals67.innerContainer.Contains(CS_0024_003C_003E8__locals67.selectedEmbryo))
								{
									Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation("ImplantEmbryoCancelVat".Translate(CS_0024_003C_003E8__locals67.selectedEmbryo.Label), action, destructive: true);
									Find.WindowStack.Add(window);
								}
								else
								{
									action();
								}
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					if (CS_0024_003C_003E8__locals67.selectedPawn != null || CS_0024_003C_003E8__locals67.selectedEmbryo != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandCancelLoad".Translate(),
							defaultDesc = "CommandCancelLoadDesc".Translate(),
							icon = CancelLoadingIcon,
							activateSound = SoundDefOf.Designate_Cancel,
							action = delegate
							{
								CS_0024_003C_003E8__locals67.DestroyEmbryo();
								CS_0024_003C_003E8__locals67.innerContainer.TryDropAll(CS_0024_003C_003E8__locals67.InteractionCell, CS_0024_003C_003E8__locals67.Map, ThingPlaceMode.Near);
								if (CS_0024_003C_003E8__locals67.innerContainer.Contains(CS_0024_003C_003E8__locals67.selectedPawn))
								{
									CS_0024_003C_003E8__locals67.Notify_PawnRemoved();
								}
								if (CS_0024_003C_003E8__locals67.selectedPawn?.CurJobDef == JobDefOf.EnterBuilding)
								{
									CS_0024_003C_003E8__locals67.selectedPawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
								}
								CS_0024_003C_003E8__locals67.OnStop();
							}
						};
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_0412;
					IL_0520:
					if (CS_0024_003C_003E8__locals67.selectedEmbryo == null && Find.Storyteller.difficulty.ChildrenAllowed)
					{
						_003C_003Ec__DisplayClass86_1 CS_0024_003C_003E8__locals48 = new _003C_003Ec__DisplayClass86_1
						{
							_003C_003E4__this = CS_0024_003C_003E8__locals67
						};
						List<Thing> list = CS_0024_003C_003E8__locals67.Map.listerThings.ThingsOfDef(ThingDefOf.HumanEmbryo);
						CS_0024_003C_003E8__locals48.opts = new List<FloatMenuOption>();
						using (List<Thing>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								_003C_003Ec__DisplayClass86_2 CS_0024_003C_003E8__locals46 = new _003C_003Ec__DisplayClass86_2
								{
									CS_0024_003C_003E8__locals1 = CS_0024_003C_003E8__locals48,
									embryo = enumerator.Current
								};
								if (!CS_0024_003C_003E8__locals46.embryo.IsForbidden(Faction.OfPlayer))
								{
									CS_0024_003C_003E8__locals46.CS_0024_003C_003E8__locals1.opts.Add(new FloatMenuOption(CS_0024_003C_003E8__locals46.embryo.LabelCap, delegate
									{
										CS_0024_003C_003E8__locals46.CS_0024_003C_003E8__locals1._003C_003E4__this.SelectEmbryo(CS_0024_003C_003E8__locals46.embryo as HumanEmbryo);
									}, CS_0024_003C_003E8__locals46.embryo, Color.white));
								}
							}
						}
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "ImplantEmbryo".Translate() + "...",
							defaultDesc = "InsertEmbryoGrowthVatDesc".Translate(540000.ToStringTicksToPeriod()).Resolve(),
							icon = InsertEmbryoIcon.Texture,
							action = delegate
							{
								Find.WindowStack.Add(new FloatMenu(CS_0024_003C_003E8__locals48.opts));
							}
						};
						if (CS_0024_003C_003E8__locals48.opts.NullOrEmpty())
						{
							command_Action.Disable("ImplantNoEmbryos".Translate().CapitalizeFirst());
						}
						else if (CS_0024_003C_003E8__locals67.selectedPawn != null)
						{
							command_Action.Disable("PersonSelected".Translate().CapitalizeFirst());
						}
						else if (!CS_0024_003C_003E8__locals67.PowerOn)
						{
							command_Action.Disable("NoPower".Translate().CapitalizeFirst());
						}
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 11;
						return true;
					}
					goto IL_0723;
					IL_02e6:
					if (CS_0024_003C_003E8__locals67.selectedEmbryo != null && CS_0024_003C_003E8__locals67.innerContainer.Contains(CS_0024_003C_003E8__locals67.selectedEmbryo))
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Advance gestation 1 day",
							action = delegate
							{
								CS_0024_003C_003E8__locals67.startTick = Mathf.Max(0, CS_0024_003C_003E8__locals67.startTick - 60000);
							}
						};
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0723;
					IL_0723:
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Fill nutrition",
							action = delegate
							{
								CS_0024_003C_003E8__locals67.containedNutrition = 10f;
							}
						};
						_003C_003E1__state = 12;
						return true;
					}
					break;
					IL_0412:
					if (CS_0024_003C_003E8__locals67.selectedPawn == null)
					{
						Command_Action command_Action2 = new Command_Action
						{
							defaultLabel = "InsertPerson".Translate() + "...",
							defaultDesc = "InsertPersonGrowthVatDesc".Translate(),
							icon = InsertPawnIcon.Texture,
							action = delegate
							{
								List<FloatMenuOption> list2 = new List<FloatMenuOption>();
								foreach (Pawn item in CS_0024_003C_003E8__locals67.Map.mapPawns.AllPawnsSpawned)
								{
									_003C_003Ec__DisplayClass86_0 CS_0024_003C_003E8__locals64 = new _003C_003Ec__DisplayClass86_0
									{
										_003C_003E4__this = CS_0024_003C_003E8__locals67,
										pawn = item
									};
									if ((bool)CS_0024_003C_003E8__locals67.CanAcceptPawn(item))
									{
										list2.Add(new FloatMenuOption(item.LabelCap, delegate
										{
											CS_0024_003C_003E8__locals64._003C_003E4__this.SelectPawn(CS_0024_003C_003E8__locals64.pawn);
										}, CS_0024_003C_003E8__locals64.pawn, Color.white));
									}
								}
								if (!list2.Any())
								{
									list2.Add(new FloatMenuOption("NoViablePawns".Translate(), null));
								}
								Find.WindowStack.Add(new FloatMenu(list2));
							}
						};
						if (CS_0024_003C_003E8__locals67.selectedEmbryo != null)
						{
							command_Action2.Disable("EmbryoSelected".Translate().CapitalizeFirst());
						}
						else if (!CS_0024_003C_003E8__locals67.PowerOn)
						{
							command_Action2.Disable("NoPower".Translate().CapitalizeFirst());
						}
						else if (!CS_0024_003C_003E8__locals67.AnyAcceptablePawns)
						{
							command_Action2.Disable("NoPawnsCanEnterGrowthVat".Translate(18f).ToString());
						}
						_003C_003E2__current = command_Action2;
						_003C_003E1__state = 10;
						return true;
					}
					goto IL_0520;
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

		private void _003C_003Em__Finally2()
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
			_003CGetGizmos_003Ed__86 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__86(0)
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

	public HumanEmbryo selectedEmbryo;

	private float embryoStarvation;

	private float containedNutrition;

	private StorageSettings allowedNutritionSettings;

	[Unsaved(false)]
	private CompPowerTrader cachedPowerComp;

	[Unsaved(false)]
	private Graphic cachedTopGraphic;

	[Unsaved(false)]
	private Graphic fetusEarlyStageGraphic;

	[Unsaved(false)]
	private Graphic fetusLateStageGraphic;

	[Unsaved(false)]
	private Sustainer sustainerWorking;

	[Unsaved(false)]
	private Hediff cachedVatLearning;

	[Unsaved(false)]
	private Effecter bubbleEffecter;

	private static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	public static readonly CachedTexture InsertPawnIcon = new CachedTexture("UI/Gizmos/InsertPawn");

	public static readonly CachedTexture InsertEmbryoIcon = new CachedTexture("UI/Gizmos/InsertEmbryo");

	private const float BiostarvationGainPerDayNoFoodOrPower = 0.5f;

	private const float BiostarvationFallPerDayPoweredAndFed = 0.1f;

	private const float BasePawnConsumedNutritionPerDay = 3f;

	private const float BaseEmbryoConsumedNutritionPerDay = 6f;

	private const float AgeToEject = 18f;

	public const float NutritionBuffer = 10f;

	public const int AgeTicksPerTickInGrowthVat = 20;

	private const float EmbryoBirthQuality = 0.7f;

	public const int EmbryoGestationTicks = 540000;

	private const int EmbryoLateStageGraphicTicksRemaining = 540000;

	private const float FetusMinSize = 0.4f;

	private const float FetusMaxSize = 0.95f;

	private const int GlowIntervalTicks = 132;

	private static Dictionary<Rot4, ThingDef> GlowMotePerRotation;

	private static Dictionary<Rot4, EffecterDef> BubbleEffecterPerRotation;

	public bool StorageTabVisible => true;

	public float HeldPawnDrawPos_Y => DrawPos.y + 0.03658537f;

	public float HeldPawnBodyAngle => base.Rotation.AsAngle;

	public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;

	public bool PowerOn => PowerTraderComp.PowerOn;

	public override Vector3 PawnDrawOffset => CompBiosculpterPod.FloatingOffset(Find.TickManager.TicksGame);

	private CompPowerTrader PowerTraderComp
	{
		get
		{
			if (cachedPowerComp == null)
			{
				cachedPowerComp = this.TryGetComp<CompPowerTrader>();
			}
			return cachedPowerComp;
		}
	}

	public float BiostarvationDailyOffset
	{
		get
		{
			if (!base.Working)
			{
				return 0f;
			}
			if (!PowerOn || containedNutrition <= 0f)
			{
				return 0.5f;
			}
			return -0.1f;
		}
	}

	private float BiostarvationSeverityPercent
	{
		get
		{
			if (selectedEmbryo != null)
			{
				return embryoStarvation;
			}
			if (selectedPawn != null)
			{
				Hediff firstHediffOfDef = selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation);
				if (firstHediffOfDef != null)
				{
					return firstHediffOfDef.Severity / HediffDefOf.BioStarvation.maxSeverity;
				}
			}
			return 0f;
		}
	}

	public float NutritionConsumedPerDay
	{
		get
		{
			float num = ((selectedEmbryo != null) ? 6f : 3f);
			if (BiostarvationSeverityPercent > 0f)
			{
				float num2 = 1.1f;
				num *= num2;
			}
			return num;
		}
	}

	public float NutritionStored
	{
		get
		{
			float num = containedNutrition;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				Thing thing = innerContainer[i];
				num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition);
			}
			return num;
		}
	}

	public float NutritionNeeded
	{
		get
		{
			if (selectedPawn == null && selectedEmbryo == null)
			{
				return 0f;
			}
			return 10f - NutritionStored;
		}
	}

	public int EmbryoGestationTicksRemaining => startTick - Find.TickManager.TicksGame;

	public float EmbryoGestationPct => 1f - Mathf.Clamp01((float)EmbryoGestationTicksRemaining / 540000f);

	private Graphic TopGraphic
	{
		get
		{
			if (cachedTopGraphic == null)
			{
				cachedTopGraphic = GraphicDatabase.Get<Graphic_Multi>("Things/Building/Misc/GrowthVat/GrowthVatTop", ShaderDatabase.Transparent, def.graphicData.drawSize, Color.white);
			}
			return cachedTopGraphic;
		}
	}

	private Graphic FetusEarlyStage
	{
		get
		{
			if (fetusEarlyStageGraphic == null)
			{
				fetusEarlyStageGraphic = GraphicDatabase.Get<Graphic_Single>("Other/VatGrownFetus_EarlyStage", ShaderDatabase.Cutout, Vector2.one, Color.white);
			}
			return fetusEarlyStageGraphic;
		}
	}

	private Graphic FetusLateStage
	{
		get
		{
			if (fetusLateStageGraphic == null)
			{
				fetusLateStageGraphic = GraphicDatabase.Get<Graphic_Single>("Other/VatGrownFetus_LateStage", ShaderDatabase.Cutout, Vector2.one, Color.white);
			}
			return fetusLateStageGraphic;
		}
	}

	private Hediff VatLearning
	{
		get
		{
			if (cachedVatLearning == null)
			{
				cachedVatLearning = selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.VatLearning) ?? selectedPawn.health.AddHediff(HediffDefOf.VatLearning, null, null);
			}
			return cachedVatLearning;
		}
	}

	public override void PostMake()
	{
		base.PostMake();
		allowedNutritionSettings = new StorageSettings(this);
		if (def.building.defaultStorageSettings != null)
		{
			allowedNutritionSettings.CopyFrom(def.building.defaultStorageSettings);
		}
	}

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		if (respawningAfterLoad && selectedEmbryo != null && innerContainer.Contains(selectedEmbryo))
		{
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Color color = EmbryoColor();
				fetusEarlyStageGraphic = FetusEarlyStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
				fetusLateStageGraphic = FetusLateStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
			});
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		if (mode != DestroyMode.WillReplace)
		{
			if (selectedPawn != null && innerContainer.Contains(selectedPawn))
			{
				Notify_PawnRemoved();
			}
			DestroyEmbryo();
		}
		sustainerWorking = null;
		cachedVatLearning = null;
		base.DeSpawn(mode);
	}

	protected override void TickInterval(int delta)
	{
		base.TickInterval(delta);
		if (base.Working && selectedPawn != null && innerContainer.Contains(selectedPawn))
		{
			VatLearning?.TickInterval(delta);
			VatLearning?.PostTickInterval(delta);
		}
	}

	protected override void Tick()
	{
		base.Tick();
		if (this.IsHashIntervalTick(250))
		{
			PowerTraderComp.PowerOutput = (base.Working ? (0f - base.PowerComp.Props.PowerConsumption) : (0f - base.PowerComp.Props.idlePowerDraw));
		}
		Pawn pawn = selectedPawn;
		if (pawn == null || !pawn.Destroyed)
		{
			HumanEmbryo humanEmbryo = selectedEmbryo;
			if (humanEmbryo == null || !humanEmbryo.Destroyed)
			{
				goto IL_0078;
			}
		}
		OnStop();
		goto IL_0078;
		IL_0078:
		foreach (Thing item in (IEnumerable<Thing>)innerContainer)
		{
			if (item is HumanEmbryo humanEmbryo2 && humanEmbryo2 != selectedEmbryo)
			{
				innerContainer.TryDrop(item, InteractionCell, base.Map, ThingPlaceMode.Near, 1, out var _);
			}
		}
		if (base.Working)
		{
			if (selectedPawn != null)
			{
				if (selectedPawn.ageTracker.AgeBiologicalYearsFloat >= 18f)
				{
					Messages.Message("OccupantEjectedFromGrowthVat".Translate(selectedPawn.Named("PAWN")) + ": " + "PawnIsTooOld".Translate(selectedPawn.Named("PAWN")), selectedPawn, MessageTypeDefOf.NeutralEvent);
					Finish();
					return;
				}
				if (innerContainer.Contains(selectedPawn))
				{
					int ticks = Mathf.RoundToInt(20f * selectedPawn.GetStatValue(StatDefOf.GrowthVatOccupantSpeed));
					selectedPawn.ageTracker.Notify_TickedInGrowthVat(ticks);
					VatLearning?.Tick();
					VatLearning?.PostTick();
				}
				float num = BiostarvationDailyOffset / 60000f * HediffDefOf.BioStarvation.maxSeverity;
				Hediff firstHediffOfDef = selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity += num;
					if (firstHediffOfDef.ShouldRemove)
					{
						selectedPawn.health.RemoveHediff(firstHediffOfDef);
					}
				}
				else if (num > 0f)
				{
					Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, selectedPawn);
					hediff.Severity = num;
					selectedPawn.health.AddHediff(hediff, null, null);
				}
			}
			else if (selectedEmbryo != null)
			{
				if (EmbryoGestationTicksRemaining <= 0)
				{
					Finish();
					return;
				}
				embryoStarvation = Mathf.Clamp01(embryoStarvation + BiostarvationDailyOffset / 60000f);
			}
			if (BiostarvationSeverityPercent >= 1f)
			{
				Fail();
				return;
			}
			if (sustainerWorking == null || sustainerWorking.Ended)
			{
				sustainerWorking = SoundDefOf.GrowthVat_Working.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
			}
			else
			{
				sustainerWorking.Maintain();
			}
			containedNutrition = Mathf.Clamp(containedNutrition - NutritionConsumedPerDay / 60000f, 0f, 2.1474836E+09f);
			if (containedNutrition <= 0f)
			{
				TryAbsorbNutritiousThing();
			}
			if (GlowMotePerRotation == null)
			{
				GlowMotePerRotation = new Dictionary<Rot4, ThingDef>
				{
					{
						Rot4.South,
						ThingDefOf.Mote_VatGlowVertical
					},
					{
						Rot4.East,
						ThingDefOf.Mote_VatGlowHorizontal
					},
					{
						Rot4.West,
						ThingDefOf.Mote_VatGlowHorizontal
					},
					{
						Rot4.North,
						ThingDefOf.Mote_VatGlowVertical
					}
				};
				BubbleEffecterPerRotation = new Dictionary<Rot4, EffecterDef>
				{
					{
						Rot4.South,
						EffecterDefOf.Vat_Bubbles_South
					},
					{
						Rot4.East,
						EffecterDefOf.Vat_Bubbles_East
					},
					{
						Rot4.West,
						EffecterDefOf.Vat_Bubbles_West
					},
					{
						Rot4.North,
						EffecterDefOf.Vat_Bubbles_North
					}
				};
			}
			if (this.IsHashIntervalTick(132))
			{
				MoteMaker.MakeStaticMote(DrawPos, base.MapHeld, GlowMotePerRotation[base.Rotation]);
			}
			if (bubbleEffecter == null)
			{
				bubbleEffecter = BubbleEffecterPerRotation[base.Rotation].SpawnAttached(this, base.MapHeld);
			}
			bubbleEffecter.EffectTick(this, this);
		}
		else
		{
			TryGrowEmbryo();
			bubbleEffecter?.Cleanup();
			bubbleEffecter = null;
		}
	}

	public override AcceptanceReport CanAcceptPawn(Pawn pawn)
	{
		if (base.Working)
		{
			return "Occupied".Translate();
		}
		if (!PowerOn)
		{
			return "NoPower".Translate().CapitalizeFirst();
		}
		if (selectedEmbryo != null)
		{
			return "EmbryoSelected".Translate();
		}
		if (pawn.ageTracker.AgeBiologicalYearsFloat >= 18f)
		{
			return "TooOld".Translate(pawn.Named("PAWN"), 18f.Named("AGEYEARS"));
		}
		if (selectedPawn != null && selectedPawn != pawn)
		{
			return "WaitingForPawn".Translate(selectedPawn.Named("PAWN"));
		}
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioStarvation))
		{
			return "PawnBiostarving".Translate(pawn.Named("PAWN"));
		}
		return pawn.IsColonist && !pawn.IsQuestLodger();
	}

	public override void TryAcceptPawn(Pawn pawn)
	{
		if (selectedPawn == null || !CanAcceptPawn(pawn))
		{
			return;
		}
		selectedPawn = pawn;
		bool num = pawn.DeSpawnOrDeselect();
		if (innerContainer.TryAddOrTransfer(pawn))
		{
			SoundDefOf.GrowthVat_Close.PlayOneShot(SoundInfo.InMap(this));
			startTick = Find.TickManager.TicksGame;
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.VatLearning))
			{
				pawn.health.AddHediff(HediffDefOf.VatLearning, null, null);
			}
			if (!pawn.health.hediffSet.HasHediff(HediffDefOf.VatGrowing))
			{
				pawn.health.AddHediff(HediffDefOf.VatGrowing, null, null);
			}
		}
		if (num)
		{
			Find.Selector.Select(pawn, playSound: false, forceDesignatorDeselect: false);
		}
	}

	private void TryGrowEmbryo()
	{
		if (!base.Working && PowerOn && selectedEmbryo != null && innerContainer.Contains(selectedEmbryo))
		{
			SoundDefOf.GrowthVat_Close.PlayOneShot(SoundInfo.InMap(this));
			startTick = Find.TickManager.TicksGame + 540000;
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Color color = EmbryoColor();
				fetusEarlyStageGraphic = FetusEarlyStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
				fetusLateStageGraphic = FetusLateStage.GetColoredVersion(ShaderDatabase.Cutout, color, color);
			});
			if (selectedPawn != null)
			{
				Log.Error("Growing embryo while pawn was somehow marked as selected");
				selectedPawn = null;
			}
		}
	}

	private void TryAbsorbNutritiousThing()
	{
		for (int i = 0; i < innerContainer.Count; i++)
		{
			if (innerContainer[i] != selectedPawn && innerContainer[i].def != ThingDefOf.Xenogerm && innerContainer[i].def != ThingDefOf.HumanEmbryo)
			{
				float statValue = innerContainer[i].GetStatValue(StatDefOf.Nutrition);
				if (statValue > 0f)
				{
					containedNutrition += statValue;
					innerContainer[i].SplitOff(1).Destroy();
					break;
				}
			}
		}
	}

	private void Finish()
	{
		if (selectedPawn != null)
		{
			FinishPawn();
		}
		else if (selectedEmbryo != null)
		{
			FinishEmbryo();
		}
	}

	private void FinishEmbryo()
	{
		EmbryoBirth();
		DestroyEmbryo();
		OnStop();
	}

	private void FinishPawn()
	{
		if (selectedPawn != null && innerContainer.Contains(selectedPawn))
		{
			Notify_PawnRemoved();
			innerContainer.TryDrop(selectedPawn, InteractionCell, base.Map, ThingPlaceMode.Near, 1, out var _);
			OnStop();
		}
	}

	private void Fail()
	{
		if (innerContainer.Contains(selectedPawn))
		{
			Notify_PawnRemoved();
			innerContainer.TryDrop(selectedPawn, InteractionCell, base.Map, ThingPlaceMode.Near, 1, out var _);
			Hediff firstHediffOfDef = selectedPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BioStarvation);
			selectedPawn.Kill(null, firstHediffOfDef);
		}
		DestroyEmbryo(biostarvation: true);
		OnStop();
	}

	private void OnStop()
	{
		selectedPawn = null;
		selectedEmbryo = null;
		startTick = -1;
		embryoStarvation = 0f;
		sustainerWorking = null;
		cachedVatLearning = null;
	}

	private void DestroyEmbryo(bool biostarvation = false)
	{
		if (startTick < 0 || selectedEmbryo == null || !innerContainer.Contains(selectedEmbryo))
		{
			return;
		}
		if (startTick > Find.TickManager.TicksGame)
		{
			if (biostarvation)
			{
				Messages.Message("EmbryoEjectedFromGrowthVatBiostarvation".Translate(selectedEmbryo.Label), this, MessageTypeDefOf.NegativeEvent);
			}
			else
			{
				Messages.Message("EmbryoEjectedFromGrowthVat".Translate(selectedEmbryo.Label), this, MessageTypeDefOf.NegativeEvent);
			}
		}
		innerContainer.Remove(selectedEmbryo);
		selectedEmbryo.Destroy();
		selectedEmbryo = null;
	}

	private void EmbryoBirth()
	{
		if (selectedEmbryo != null && innerContainer.Contains(selectedEmbryo) && startTick <= Find.TickManager.TicksGame)
		{
			Precept_Ritual ritual = Faction.OfPlayer.ideos.PrimaryIdeo.GetPrecept(PreceptDefOf.ChildBirth) as Precept_Ritual;
			Thing thing = PregnancyUtility.ApplyBirthOutcome(((RitualOutcomeEffectWorker_ChildBirth)RitualOutcomeEffectDefOf.ChildBirth.GetInstance()).GetOutcome(0.7f, null), 0.7f, ritual, selectedEmbryo?.GeneSet?.GenesListForReading, selectedEmbryo.Mother, this, selectedEmbryo.Father);
			if (thing != null && embryoStarvation > 0f)
			{
				Pawn pawn = ((thing is Corpse corpse) ? corpse.InnerPawn : ((Pawn)thing));
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BioStarvation, pawn);
				hediff.Severity = Mathf.Lerp(0f, HediffDefOf.BioStarvation.maxSeverity, embryoStarvation);
				pawn.health.AddHediff(hediff, null, null);
			}
		}
	}

	private void Notify_PawnRemoved()
	{
		SoundDefOf.GrowthVat_Open.PlayOneShot(SoundInfo.InMap(this));
	}

	public bool CanAcceptNutrition(Thing thing)
	{
		return allowedNutritionSettings.AllowedToAccept(thing);
	}

	public StorageSettings GetStoreSettings()
	{
		return allowedNutritionSettings;
	}

	public StorageSettings GetParentStoreSettings()
	{
		return def.building.fixedStorageSettings;
	}

	public void Notify_SettingsChanged()
	{
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__86))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__86(-2)
		{
			_003C_003E4__this = this
		};
	}

	public void SelectEmbryo(HumanEmbryo embryo)
	{
		selectedEmbryo = embryo;
		embryo.implantTarget = this;
	}

	public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
	{
		if (base.Working && selectedPawn != null && innerContainer.Contains(selectedPawn))
		{
			selectedPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc + PawnDrawOffset, null, neverAimWeapon: true);
		}
		base.DynamicDrawPhaseAt(phase, drawLoc, flip);
	}

	protected override void DrawAt(Vector3 drawLoc, bool flip = false)
	{
		base.DrawAt(drawLoc, flip);
		if (base.Working && selectedPawn == null && selectedEmbryo != null && innerContainer.Contains(selectedEmbryo))
		{
			Vector2 drawSize = Vector2.one * Mathf.Lerp(0.4f, 0.95f, EmbryoGestationPct);
			if (EmbryoGestationTicksRemaining > 540000)
			{
				FetusEarlyStage.drawSize = drawSize;
				FetusEarlyStage.DrawFromDef(DrawPos + PawnDrawOffset + Altitudes.AltIncVect * 0.25f, base.Rotation, null);
			}
			else
			{
				FetusLateStage.drawSize = drawSize;
				FetusLateStage.DrawFromDef(DrawPos + PawnDrawOffset + Altitudes.AltIncVect * 0.25f, base.Rotation, null);
			}
		}
		TopGraphic.Draw(DrawPos + Altitudes.AltIncVect * 2f, base.Rotation, this);
	}

	private Color EmbryoColor()
	{
		Color result = PawnSkinColors.GetSkinColor(0.5f);
		if (selectedEmbryo?.GeneSet != null)
		{
			foreach (GeneDef item in selectedEmbryo.GeneSet.GenesListForReading)
			{
				if (item.skinColorOverride.HasValue)
				{
					return item.skinColorOverride.Value;
				}
				if (item.skinColorBase.HasValue)
				{
					result = item.skinColorBase.Value;
				}
			}
		}
		return result;
	}

	public override void DrawExtraSelectionOverlays()
	{
		base.DrawExtraSelectionOverlays();
		if (selectedEmbryo != null && selectedEmbryo.Map == base.Map)
		{
			GenDraw.DrawLineBetween(this.TrueCenter(), selectedEmbryo.TrueCenter());
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		if (base.Working)
		{
			if (selectedPawn != null && innerContainer.Contains(selectedPawn))
			{
				stringBuilder.AppendLineIfNotEmpty().Append($"{"CasketContains".Translate().ToString()}: {selectedPawn.NameShortColored.Resolve()}, {selectedPawn.ageTracker.AgeBiologicalYears}");
			}
			if (selectedEmbryo != null && innerContainer.Contains(selectedEmbryo))
			{
				stringBuilder.AppendLineIfNotEmpty().AppendLine("Gestating".Translate() + ": " + selectedEmbryo.Label.CapitalizeFirst());
				stringBuilder.AppendLineTagged("EmbryoTimeUntilBirth".Translate() + ": " + EmbryoGestationTicksRemaining.ToStringTicksToDays().Colorize(ColoredText.DateTimeColor));
				stringBuilder.Append("EmbryoBirthQuality".Translate() + ": " + 0.7f.ToStringPercent());
			}
			float biostarvationSeverityPercent = BiostarvationSeverityPercent;
			if (biostarvationSeverityPercent > 0f)
			{
				string text = ((BiostarvationDailyOffset >= 0f) ? "+" : string.Empty);
				stringBuilder.AppendLineIfNotEmpty().Append($"{"Biostarvation".Translate()}: {biostarvationSeverityPercent.ToStringPercent()} ({"PerDay".Translate(text + BiostarvationDailyOffset.ToStringPercent())})");
			}
		}
		else if (selectedPawn != null)
		{
			stringBuilder.AppendLineIfNotEmpty().Append("WaitingForPawn".Translate(selectedPawn.Named("PAWN")).Resolve());
		}
		stringBuilder.AppendLineIfNotEmpty().Append("Nutrition".Translate()).Append(": ")
			.Append(NutritionStored.ToStringByStyle(ToStringStyle.FloatMaxOne));
		if (base.Working)
		{
			stringBuilder.Append(" (-").Append("PerDay".Translate(NutritionConsumedPerDay.ToString("F1"))).Append(")");
		}
		return stringBuilder.ToString();
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__93))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__93(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_References.Look(ref selectedEmbryo, "selectedEmbryo");
		Scribe_Values.Look(ref embryoStarvation, "embryoStarvation", 0f);
		Scribe_Values.Look(ref containedNutrition, "containedNutrition", 0f);
		Scribe_Deep.Look(ref allowedNutritionSettings, "allowedNutritionSettings", this);
		if (allowedNutritionSettings == null)
		{
			allowedNutritionSettings = new StorageSettings(this);
			if (def.building.defaultStorageSettings != null)
			{
				allowedNutritionSettings.CopyFrom(def.building.defaultStorageSettings);
			}
		}
	}
}
