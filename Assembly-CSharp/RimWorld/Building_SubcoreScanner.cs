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
public class Building_SubcoreScanner : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass41_0
	{
		public Building_SubcoreScanner _003C_003E4__this;

		public Pawn selPawn;

		public Action _003C_003E9__1;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			if (_003C_003E4__this.DestroyOccupantBrain)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRipscanPawn".Translate(selPawn.Named("PAWN")), delegate
				{
					_003C_003E4__this.SelectPawn(selPawn);
				}, destructive: true));
			}
			else
			{
				_003C_003E4__this.SelectPawn(selPawn);
			}
		}

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			_003C_003E4__this.SelectPawn(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass45_0
	{
		public Pawn pawn;

		public Building_SubcoreScanner _003C_003E4__this;

		public Action _003C_003E9__4;

		internal void _003CGetGizmos_003Eb__3()
		{
			if (_003C_003E4__this.def.building.destroyBrain)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRipscanPawn".Translate(pawn.Named("PAWN")), delegate
				{
					_003C_003E4__this.SelectPawn(pawn);
				}, destructive: true));
			}
			else
			{
				_003C_003E4__this.SelectPawn(pawn);
			}
		}

		internal void _003CGetGizmos_003Eb__4()
		{
			_003C_003E4__this.SelectPawn(pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__41 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_SubcoreScanner _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass41_0 _003C_003E8__1;

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
		public _003CGetFloatMenuOptions_003Ed__41(int _003C_003E1__state)
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
				Building_SubcoreScanner building_SubcoreScanner = _003C_003E4__this;
				AcceptanceReport acceptanceReport;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass41_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E7__wrap1 = ((ThingWithComps)building_SubcoreScanner).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
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
					if (!_003C_003E8__1.selPawn.CanReach(building_SubcoreScanner, PathEndMode.InteractionCell, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnterBuilding".Translate(building_SubcoreScanner) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 2;
						return true;
					}
					acceptanceReport = building_SubcoreScanner.CanAcceptPawn(_003C_003E8__1.selPawn);
					if (acceptanceReport.Accepted)
					{
						_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(building_SubcoreScanner), delegate
						{
							if (_003C_003E8__1._003C_003E4__this.DestroyOccupantBrain)
							{
								Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRipscanPawn".Translate(_003C_003E8__1.selPawn.Named("PAWN")), delegate
								{
									_003C_003E8__1._003C_003E4__this.SelectPawn(_003C_003E8__1.selPawn);
								}, destructive: true));
							}
							else
							{
								_003C_003E8__1._003C_003E4__this.SelectPawn(_003C_003E8__1.selPawn);
							}
						}), _003C_003E8__1.selPawn, building_SubcoreScanner);
						_003C_003E1__state = 3;
						return true;
					}
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnterBuilding".Translate(building_SubcoreScanner) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
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
			_003CGetFloatMenuOptions_003Ed__41 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__41(0)
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
	private sealed class _003CGetGizmos_003Ed__45 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_SubcoreScanner _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__45(int _003C_003E1__state)
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
				Building_SubcoreScanner CS_0024_003C_003E8__locals41 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building_Enterable)CS_0024_003C_003E8__locals41).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0082;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0082;
				case 2:
					_003C_003E1__state = -1;
					goto IL_0340;
				case 3:
					_003C_003E1__state = -1;
					goto IL_0340;
				case 4:
					_003C_003E1__state = -1;
					goto IL_03f0;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0443;
				case 6:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_0082:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!CS_0024_003C_003E8__locals41.initScanner)
					{
						Command_Action command_Action = new Command_Action
						{
							defaultLabel = "SubcoreScannerStart".Translate()
						};
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.Append("SubcoreScannerProduces".Translate() + " " + CS_0024_003C_003E8__locals41.def.building.subcoreScannerOutputDef.label + ".");
						stringBuilder.Append("\n\n");
						stringBuilder.Append("DurationHours".Translate() + ": " + CS_0024_003C_003E8__locals41.def.building.subcoreScannerTicks.ToStringTicksToPeriod());
						stringBuilder.Append("\n\n");
						string text = CS_0024_003C_003E8__locals41.def.building.subcoreScannerFixedIngredients.Select((IngredientCount i) => i.Summary).ToCommaList(useAnd: true);
						stringBuilder.Append("SubcoreScannerStartDesc".Translate(CS_0024_003C_003E8__locals41.def.label, text));
						command_Action.defaultDesc = stringBuilder.ToString();
						command_Action.icon = CS_0024_003C_003E8__locals41.InitScannerIcon.Texture;
						command_Action.action = delegate
						{
							CS_0024_003C_003E8__locals41.initScanner = true;
						};
						command_Action.activateSound = SoundDefOf.Tick_Tiny;
						_003C_003E2__current = command_Action;
						_003C_003E1__state = 2;
						return true;
					}
					if (CS_0024_003C_003E8__locals41.SelectedPawn == null)
					{
						Command_Action command_Action2 = new Command_Action
						{
							defaultLabel = "InsertPerson".Translate() + "...",
							defaultDesc = "InsertPersonSubcoreScannerDesc".Translate(CS_0024_003C_003E8__locals41.def.label),
							icon = InsertPersonIcon.Texture,
							action = delegate
							{
								List<FloatMenuOption> list = new List<FloatMenuOption>();
								IReadOnlyList<Pawn> allPawnsSpawned = CS_0024_003C_003E8__locals41.Map.mapPawns.AllPawnsSpawned;
								for (int j = 0; j < allPawnsSpawned.Count; j++)
								{
									_003C_003Ec__DisplayClass45_0 CS_0024_003C_003E8__locals25 = new _003C_003Ec__DisplayClass45_0
									{
										_003C_003E4__this = CS_0024_003C_003E8__locals41,
										pawn = allPawnsSpawned[j]
									};
									AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals41.CanAcceptPawn(CS_0024_003C_003E8__locals25.pawn);
									if (!acceptanceReport.Accepted)
									{
										if (!acceptanceReport.Reason.NullOrEmpty())
										{
											list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals25.pawn.LabelShortCap + ": " + acceptanceReport.Reason, null, CS_0024_003C_003E8__locals25.pawn, Color.white));
										}
									}
									else
									{
										list.Add(new FloatMenuOption(CS_0024_003C_003E8__locals25.pawn.LabelShortCap, delegate
										{
											if (CS_0024_003C_003E8__locals25._003C_003E4__this.def.building.destroyBrain)
											{
												Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRipscanPawn".Translate(CS_0024_003C_003E8__locals25.pawn.Named("PAWN")), delegate
												{
													CS_0024_003C_003E8__locals25._003C_003E4__this.SelectPawn(CS_0024_003C_003E8__locals25.pawn);
												}, destructive: true));
											}
											else
											{
												CS_0024_003C_003E8__locals25._003C_003E4__this.SelectPawn(CS_0024_003C_003E8__locals25.pawn);
											}
										}, CS_0024_003C_003E8__locals25.pawn, Color.white));
									}
								}
								if (!list.Any())
								{
									list.Add(new FloatMenuOption("NoExtractablePawns".Translate(), null));
								}
								Find.WindowStack.Add(new FloatMenu(list));
							}
						};
						if (!CS_0024_003C_003E8__locals41.PowerOn)
						{
							command_Action2.Disable("NoPower".Translate().CapitalizeFirst());
						}
						else if (CS_0024_003C_003E8__locals41.State == SubcoreScannerState.WaitingForIngredients)
						{
							StringBuilder stringBuilder2 = new StringBuilder("SubcoreScannerWaitingForIngredientsDesc".Translate().CapitalizeFirst() + ":\n");
							CS_0024_003C_003E8__locals41.AppendIngredientsList(stringBuilder2);
							command_Action2.Disable(stringBuilder2.ToString());
						}
						_003C_003E2__current = command_Action2;
						_003C_003E1__state = 3;
						return true;
					}
					goto IL_0340;
					IL_03f0:
					if (!DebugSettings.ShowDevGizmos)
					{
						break;
					}
					if (CS_0024_003C_003E8__locals41.State == SubcoreScannerState.Occupied)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Complete",
							action = delegate
							{
								CS_0024_003C_003E8__locals41.fabricationTicksLeft = 0;
							}
						};
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0443;
					IL_0340:
					if (CS_0024_003C_003E8__locals41.initScanner)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = ((CS_0024_003C_003E8__locals41.State == SubcoreScannerState.Occupied) ? "CommandCancelSubcoreScan".Translate() : "CommandCancelLoad".Translate()),
							defaultDesc = ((CS_0024_003C_003E8__locals41.State == SubcoreScannerState.Occupied) ? "CommandCancelSubcoreScanDesc".Translate() : "CommandCancelLoadDesc".Translate()),
							icon = CancelLoadingIcon,
							action = delegate
							{
								if (CS_0024_003C_003E8__locals41.State == SubcoreScannerState.Occupied && CS_0024_003C_003E8__locals41.DestroyOccupantBrain)
								{
									Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmCancelRipscan".Translate(CS_0024_003C_003E8__locals41.Occupant.Named("PAWN")), CS_0024_003C_003E8__locals41.EjectContents, destructive: true));
								}
								else
								{
									CS_0024_003C_003E8__locals41.EjectContents();
								}
							},
							activateSound = SoundDefOf.Designate_Cancel
						};
						_003C_003E1__state = 4;
						return true;
					}
					goto IL_03f0;
					IL_0443:
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = (CS_0024_003C_003E8__locals41.debugDisableNeedForIngredients ? "DEV: Enable Ingredients" : "DEV: Disable Ingredients"),
						action = delegate
						{
							CS_0024_003C_003E8__locals41.debugDisableNeedForIngredients = !CS_0024_003C_003E8__locals41.debugDisableNeedForIngredients;
						}
					};
					_003C_003E1__state = 6;
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
			_003CGetGizmos_003Ed__45 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__45(0)
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

	private bool initScanner;

	private int fabricationTicksLeft;

	private Effecter effectStart;

	private Effecter effectHusk;

	private bool debugDisableNeedForIngredients;

	private Mote workingMote;

	private Sustainer sustainerWorking;

	private Effecter progressBarEffecter;

	public static readonly Texture2D CancelLoadingIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	public static readonly CachedTexture InsertPersonIcon = new CachedTexture("UI/Icons/InsertPersonSubcoreScanner");

	private static Dictionary<Rot4, ThingDef> MotePerRotation;

	private static Dictionary<Rot4, ThingDef> MotePerRotationRip;

	private static readonly Dictionary<Rot4, Vector3> HuskEffectOffsets = new Dictionary<Rot4, Vector3>
	{
		{
			Rot4.North,
			new Vector3(0f, 0f, 0.47f)
		},
		{
			Rot4.South,
			new Vector3(0f, 0f, -0.3f)
		},
		{
			Rot4.East,
			new Vector3(0.4f, 0f, -0.025f)
		},
		{
			Rot4.West,
			new Vector3(-0.4f, 0f, -0.025f)
		}
	};

	private const float ProgressBarOffsetZ = -0.8f;

	public CachedTexture InitScannerIcon = new CachedTexture("UI/Icons/SubcoreScannerStart");

	public float HeldPawnDrawPos_Y => DrawPos.y + 0.03658537f;

	public float HeldPawnBodyAngle => base.Rotation.AsAngle;

	public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;

	public bool PowerOn => this.TryGetComp<CompPowerTrader>().PowerOn;

	public override Vector3 PawnDrawOffset => Vector3.zero;

	public Pawn Occupant
	{
		get
		{
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (innerContainer[i] is Pawn result)
				{
					return result;
				}
			}
			return null;
		}
	}

	public bool AllRequiredIngredientsLoaded
	{
		get
		{
			if (!debugDisableNeedForIngredients)
			{
				for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
				{
					if (GetRequiredCountOf(def.building.subcoreScannerFixedIngredients[i].FixedIngredient) > 0)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public SubcoreScannerState State
	{
		get
		{
			if (!initScanner || !PowerOn)
			{
				return SubcoreScannerState.Inactive;
			}
			if (!AllRequiredIngredientsLoaded)
			{
				return SubcoreScannerState.WaitingForIngredients;
			}
			if (Occupant == null)
			{
				return SubcoreScannerState.WaitingForOccupant;
			}
			return SubcoreScannerState.Occupied;
		}
	}

	public bool DestroyOccupantBrain => def.building.destroyBrain;

	public override void SpawnSetup(Map map, bool respawningAfterLoad)
	{
		base.SpawnSetup(map, respawningAfterLoad);
		for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
		{
			def.building.subcoreScannerFixedIngredients[i].ResolveReferences();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		progressBarEffecter?.Cleanup();
		progressBarEffecter = null;
		effectHusk?.Cleanup();
		effectHusk = null;
		effectStart?.Cleanup();
		effectStart = null;
		if (!base.BeingTransportedOnGravship && DestroyOccupantBrain && Occupant != null)
		{
			KillOccupant();
		}
		base.DeSpawn(mode);
	}

	public int GetRequiredCountOf(ThingDef thingDef)
	{
		for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
		{
			if (def.building.subcoreScannerFixedIngredients[i].FixedIngredient == thingDef)
			{
				int num = innerContainer.TotalStackCountOfDef(def.building.subcoreScannerFixedIngredients[i].FixedIngredient);
				return (int)def.building.subcoreScannerFixedIngredients[i].GetBaseCount() - num;
			}
		}
		return 0;
	}

	public override AcceptanceReport CanAcceptPawn(Pawn selPawn)
	{
		if (!selPawn.IsColonist && !selPawn.IsSlaveOfColony && !selPawn.IsPrisonerOfColony)
		{
			return false;
		}
		if (selectedPawn != null && selectedPawn != selPawn)
		{
			return false;
		}
		if (!PowerOn)
		{
			return "CannotUseNoPower".Translate();
		}
		if (State != SubcoreScannerState.WaitingForOccupant)
		{
			switch (State)
			{
			case SubcoreScannerState.Inactive:
				return "SubcoreScannerNotInit".Translate();
			case SubcoreScannerState.WaitingForIngredients:
			{
				StringBuilder stringBuilder = new StringBuilder("SubcoreScannerRequiresIngredients".Translate() + ": ");
				bool flag = false;
				for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
				{
					IngredientCount ingredientCount = def.building.subcoreScannerFixedIngredients[i];
					int num = innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
					int num2 = (int)ingredientCount.GetBaseCount();
					if (num < num2)
					{
						if (flag)
						{
							stringBuilder.Append(", ");
						}
						stringBuilder.Append($"{ingredientCount.FixedIngredient.LabelCap} x{num2 - num}");
						flag = true;
					}
				}
				return stringBuilder.ToString();
			}
			case SubcoreScannerState.Occupied:
				return "SubcoreScannerOccupied".Translate();
			}
		}
		else
		{
			if (selPawn.IsQuestLodger())
			{
				return "CryptosleepCasketGuestsNotAllowed".Translate();
			}
			if (selPawn.health.hediffSet.HasHediff(HediffDefOf.ScanningSickness))
			{
				return "SubcoreScannerPawnHasSickness".Translate(HediffDefOf.ScanningSickness.label);
			}
			if (selPawn.DevelopmentalStage.Baby())
			{
				return "SubcoreScannerBabyNotAllowed".Translate();
			}
		}
		return true;
	}

	public override void TryAcceptPawn(Pawn pawn)
	{
		if ((bool)CanAcceptPawn(pawn))
		{
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
			fabricationTicksLeft = def.building.subcoreScannerTicks;
		}
	}

	public bool CanAcceptIngredient(Thing thing)
	{
		return GetRequiredCountOf(thing.def) > 0;
	}

	public void EjectContents()
	{
		Pawn occupant = Occupant;
		if (occupant == null)
		{
			innerContainer.TryDropAll(InteractionCell, base.Map, ThingPlaceMode.Near);
		}
		else
		{
			if (def.building.subcoreScannerHediff != null)
			{
				occupant.health?.AddHediff(def.building.subcoreScannerHediff, null, null);
			}
			if (DestroyOccupantBrain)
			{
				KillOccupant();
			}
			for (int num = innerContainer.Count - 1; num >= 0; num--)
			{
				if (innerContainer[num] is Pawn || innerContainer[num] is Corpse)
				{
					innerContainer.TryDrop(innerContainer[num], InteractionCell, base.Map, ThingPlaceMode.Near, 1, out var _);
				}
			}
			innerContainer.ClearAndDestroyContents();
		}
		selectedPawn = null;
		initScanner = false;
	}

	private void KillOccupant()
	{
		Pawn occupant = Occupant;
		DamageInfo dinfo = new DamageInfo(DamageDefOf.ExecutionCut, 9999f, 999f, -1f, null, occupant.health.hediffSet.GetBrain());
		dinfo.SetIgnoreInstantKillProtection(ignore: true);
		dinfo.SetAllowDamagePropagation(val: false);
		occupant.forceNoDeathNotification = true;
		occupant.TakeDamage(dinfo);
		occupant.forceNoDeathNotification = false;
		ThoughtUtility.GiveThoughtsForPawnExecuted(occupant, null, PawnExecutionKind.Ripscanned);
		Messages.Message("MessagePawnKilledRipscanner".Translate(occupant.Named("PAWN")), occupant, MessageTypeDefOf.NegativeHealthEvent);
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__41))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__41(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public static bool WasLoadingCancelled(Thing thing)
	{
		if (thing is Building_SubcoreScanner { initScanner: false })
		{
			return true;
		}
		return false;
	}

	public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
	{
		base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		Occupant?.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc, null, neverAimWeapon: true);
	}

	protected override void Tick()
	{
		base.Tick();
		if (MotePerRotation == null)
		{
			MotePerRotation = new Dictionary<Rot4, ThingDef>
			{
				{
					Rot4.South,
					ThingDefOf.SoftScannerGlow_South
				},
				{
					Rot4.East,
					ThingDefOf.SoftScannerGlow_East
				},
				{
					Rot4.West,
					ThingDefOf.SoftScannerGlow_West
				},
				{
					Rot4.North,
					ThingDefOf.SoftScannerGlow_North
				}
			};
			MotePerRotationRip = new Dictionary<Rot4, ThingDef>
			{
				{
					Rot4.South,
					ThingDefOf.RipScannerGlow_South
				},
				{
					Rot4.East,
					ThingDefOf.RipScannerGlow_East
				},
				{
					Rot4.West,
					ThingDefOf.RipScannerGlow_West
				},
				{
					Rot4.North,
					ThingDefOf.RipScannerGlow_North
				}
			};
		}
		SubcoreScannerState state = State;
		if (state == SubcoreScannerState.Occupied)
		{
			fabricationTicksLeft--;
			if (fabricationTicksLeft <= 0)
			{
				if (!DestroyOccupantBrain)
				{
					Messages.Message("MessageSubcoreSoftscannerCompleted".Translate(Occupant.Named("PAWN")), Occupant, MessageTypeDefOf.PositiveEvent);
				}
				EjectContents();
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(def.building.subcoreScannerOutputDef), InteractionCell, base.Map, ThingPlaceMode.Near, null, null, null);
				if (def.building.subcoreScannerComplete != null)
				{
					def.building.subcoreScannerComplete.PlayOneShot(this);
				}
			}
			if (workingMote == null || workingMote.Destroyed)
			{
				workingMote = MoteMaker.MakeAttachedOverlay(this, DestroyOccupantBrain ? MotePerRotationRip[base.Rotation] : MotePerRotation[base.Rotation], Vector3.zero);
			}
			workingMote?.Maintain();
			if (DestroyOccupantBrain)
			{
				if (effectHusk == null)
				{
					effectHusk = EffecterDefOf.RipScannerHeadGlow.Spawn(this, base.MapHeld, HuskEffectOffsets[base.Rotation]);
				}
				effectHusk.EffectTick(this, this);
			}
			if (progressBarEffecter == null)
			{
				progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
			}
			progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
			MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
			mote.progress = 1f - (float)fabricationTicksLeft / (float)def.building.subcoreScannerTicks;
			mote.offsetZ = -0.8f;
			if (def.building.subcoreScannerWorking != null)
			{
				if (sustainerWorking == null || sustainerWorking.Ended)
				{
					sustainerWorking = def.building.subcoreScannerWorking.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
				}
				else
				{
					sustainerWorking.Maintain();
				}
			}
		}
		else
		{
			effectHusk?.Cleanup();
			effectHusk = null;
			progressBarEffecter?.Cleanup();
			progressBarEffecter = null;
		}
		if (state == SubcoreScannerState.Occupied)
		{
			if (def.building.subcoreScannerStartEffect != null)
			{
				if (effectStart == null)
				{
					effectStart = def.building.subcoreScannerStartEffect.Spawn();
					effectStart.Trigger(this, new TargetInfo(InteractionCell, base.Map));
				}
				effectStart.EffectTick(this, new TargetInfo(InteractionCell, base.Map));
			}
		}
		else
		{
			effectStart?.Cleanup();
			effectStart = null;
		}
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__45))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__45(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		switch (State)
		{
		case SubcoreScannerState.WaitingForIngredients:
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("SubcoreScannerWaitingForIngredients".Translate());
			AppendIngredientsList(stringBuilder);
			break;
		case SubcoreScannerState.WaitingForOccupant:
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("SubcoreScannerWaitingForOccupant".Translate());
			break;
		case SubcoreScannerState.Occupied:
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("SubcoreScannerCompletesIn".Translate() + ": " + fabricationTicksLeft.ToStringTicksToPeriod());
			break;
		}
		return stringBuilder.ToString();
	}

	private void AppendIngredientsList(StringBuilder sb)
	{
		for (int i = 0; i < def.building.subcoreScannerFixedIngredients.Count; i++)
		{
			IngredientCount ingredientCount = def.building.subcoreScannerFixedIngredients[i];
			int value = innerContainer.TotalStackCountOfDef(ingredientCount.FixedIngredient);
			int value2 = (int)ingredientCount.GetBaseCount();
			sb.AppendInNewLine($" - {ingredientCount.FixedIngredient.LabelCap} {value} / {value2}");
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref initScanner, "initScanner", defaultValue: false);
		Scribe_Values.Look(ref fabricationTicksLeft, "fabricationTicksLeft", 0);
	}
}
