using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

[StaticConstructorOnStartup]
public class Building_GeneExtractor : Building_Enterable, IThingHolderWithDrawnPawn, IThingHolder
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass37_0
	{
		public Building_GeneExtractor _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CGetFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.SelectPawn(selPawn);
		}

		internal void _003CGetFloatMenuOptions_003Eb__1()
		{
			selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, _003C_003E4__this), JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass38_0
	{
		public Pawn pawn;

		public Building_GeneExtractor _003C_003E4__this;

		internal void _003CGetGizmos_003Eb__6()
		{
			_003C_003E4__this.SelectPawn(pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetFloatMenuOptions_003Ed__37 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GeneExtractor _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		private _003C_003Ec__DisplayClass37_0 _003C_003E8__1;

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
		public _003CGetFloatMenuOptions_003Ed__37(int _003C_003E1__state)
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
				Building_GeneExtractor building_GeneExtractor = _003C_003E4__this;
				AcceptanceReport acceptanceReport;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass37_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003E8__1.selPawn = selPawn;
					_003C_003E7__wrap1 = ((ThingWithComps)building_GeneExtractor).GetFloatMenuOptions(_003C_003E8__1.selPawn).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00b6;
				case 2:
					_003C_003E1__state = -1;
					break;
				case 3:
					_003C_003E1__state = -1;
					break;
				case 4:
					_003C_003E1__state = -1;
					break;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_00b6:
					if (_003C_003E7__wrap1.MoveNext())
					{
						FloatMenuOption current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (!_003C_003E8__1.selPawn.CanReach(building_GeneExtractor, PathEndMode.InteractionCell, Danger.Deadly))
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnterBuilding".Translate(building_GeneExtractor) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
						_003C_003E1__state = 2;
						return true;
					}
					acceptanceReport = building_GeneExtractor.CanAcceptPawn(_003C_003E8__1.selPawn);
					if (acceptanceReport.Accepted)
					{
						_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(building_GeneExtractor), delegate
						{
							_003C_003E8__1._003C_003E4__this.SelectPawn(_003C_003E8__1.selPawn);
						}), _003C_003E8__1.selPawn, building_GeneExtractor);
						_003C_003E1__state = 3;
						return true;
					}
					if (building_GeneExtractor.SelectedPawn == _003C_003E8__1.selPawn && !_003C_003E8__1.selPawn.IsPrisonerOfColony)
					{
						_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("EnterBuilding".Translate(building_GeneExtractor), delegate
						{
							_003C_003E8__1.selPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.EnterBuilding, _003C_003E8__1._003C_003E4__this), JobTag.Misc);
						}), _003C_003E8__1.selPawn, building_GeneExtractor);
						_003C_003E1__state = 4;
						return true;
					}
					if (!acceptanceReport.Reason.NullOrEmpty())
					{
						_003C_003E2__current = new FloatMenuOption("CannotEnterBuilding".Translate(building_GeneExtractor) + ": " + acceptanceReport.Reason.CapitalizeFirst(), null);
						_003C_003E1__state = 5;
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
			_003CGetFloatMenuOptions_003Ed__37 _003CGetFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CGetFloatMenuOptions_003Ed__ = new _003CGetFloatMenuOptions_003Ed__37(0)
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
	private sealed class _003CGetGizmos_003Ed__38 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Building_GeneExtractor _003C_003E4__this;

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
		public _003CGetGizmos_003Ed__38(int _003C_003E1__state)
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
				Building_GeneExtractor CS_0024_003C_003E8__locals28 = _003C_003E4__this;
				Command_Action command_Action;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Building_Enterable)CS_0024_003C_003E8__locals28).GetGizmos().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					_003C_003E1__state = -1;
					if (DebugSettings.ShowDevGizmos)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "DEV: Finish extraction",
							action = delegate
							{
								CS_0024_003C_003E8__locals28.Finish();
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					break;
				case 3:
					_003C_003E1__state = -1;
					break;
				case 4:
					_003C_003E1__state = -1;
					break;
				case 5:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Gizmo current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					if (CS_0024_003C_003E8__locals28.Working)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandCancelExtraction".Translate(),
							defaultDesc = "CommandCancelExtractionDesc".Translate(),
							icon = CancelIcon,
							action = delegate
							{
								CS_0024_003C_003E8__locals28.Cancel();
							},
							activateSound = SoundDefOf.Designate_Cancel
						};
						_003C_003E1__state = 2;
						return true;
					}
					if (CS_0024_003C_003E8__locals28.selectedPawn != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CommandCancelLoad".Translate(),
							defaultDesc = "CommandCancelLoadDesc".Translate(),
							icon = CancelIcon,
							activateSound = SoundDefOf.Designate_Cancel,
							action = delegate
							{
								CS_0024_003C_003E8__locals28.innerContainer.TryDropAll(CS_0024_003C_003E8__locals28.Position, CS_0024_003C_003E8__locals28.Map, ThingPlaceMode.Near);
								if (CS_0024_003C_003E8__locals28.selectedPawn.CurJobDef == JobDefOf.EnterBuilding)
								{
									CS_0024_003C_003E8__locals28.selectedPawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
								}
								CS_0024_003C_003E8__locals28.selectedPawn = null;
								CS_0024_003C_003E8__locals28.startTick = -1;
								CS_0024_003C_003E8__locals28.sustainerWorking = null;
							}
						};
						_003C_003E1__state = 4;
						return true;
					}
					command_Action = new Command_Action
					{
						defaultLabel = "InsertPerson".Translate() + "...",
						defaultDesc = "InsertPersonGeneExtractorDesc".Translate(),
						icon = CS_0024_003C_003E8__locals28.InsertPawnTex,
						action = delegate
						{
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							foreach (Pawn item in CS_0024_003C_003E8__locals28.Map.mapPawns.AllPawnsSpawned.OrderBy(delegate(Pawn p)
							{
								if (p.IsColonist)
								{
									return 0;
								}
								return (p.IsPrisonerOfColony || p.IsSlaveOfColony) ? 1 : 2;
							}).ThenBy((Pawn p) => p.Label))
							{
								_003C_003Ec__DisplayClass38_0 CS_0024_003C_003E8__locals27 = new _003C_003Ec__DisplayClass38_0
								{
									_003C_003E4__this = CS_0024_003C_003E8__locals28,
									pawn = item
								};
								if (CS_0024_003C_003E8__locals27.pawn.genes != null)
								{
									AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals28.CanAcceptPawn(CS_0024_003C_003E8__locals27.pawn);
									string text = CS_0024_003C_003E8__locals27.pawn.LabelShortCap + ", " + CS_0024_003C_003E8__locals27.pawn.genes.XenotypeLabelCap;
									if (!acceptanceReport.Accepted)
									{
										if (!acceptanceReport.Reason.NullOrEmpty())
										{
											list.Add(new FloatMenuOption(text + ": " + acceptanceReport.Reason, null, CS_0024_003C_003E8__locals27.pawn, Color.white));
										}
									}
									else
									{
										Hediff firstHediffOfDef = CS_0024_003C_003E8__locals27.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.XenogermReplicating);
										if (firstHediffOfDef != null)
										{
											text += $" ({firstHediffOfDef.LabelBase}, {firstHediffOfDef.TryGetComp<HediffComp_Disappears>().ticksToDisappear.ToStringTicksToPeriod(allowSeconds: true, shortForm: true).Colorize(ColoredText.SubtleGrayColor)})";
										}
										list.Add(new FloatMenuOption(text, delegate
										{
											CS_0024_003C_003E8__locals27._003C_003E4__this.SelectPawn(CS_0024_003C_003E8__locals27.pawn);
										}, CS_0024_003C_003E8__locals27.pawn, Color.white));
									}
								}
							}
							if (!list.Any())
							{
								list.Add(new FloatMenuOption("NoExtractablePawns".Translate(), null));
							}
							Find.WindowStack.Add(new FloatMenu(list));
						}
					};
					if (!CS_0024_003C_003E8__locals28.PowerOn)
					{
						command_Action.Disable("NoPower".Translate().CapitalizeFirst());
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 5;
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
			_003CGetGizmos_003Ed__38 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__38(0)
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

	private int ticksRemaining;

	private int powerCutTicks;

	[Unsaved(false)]
	private CompPowerTrader cachedPowerComp;

	[Unsaved(false)]
	private Texture2D cachedInsertPawnTex;

	[Unsaved(false)]
	private Sustainer sustainerWorking;

	[Unsaved(false)]
	private Effecter progressBar;

	private const int TicksToExtract = 30000;

	private const int NoPowerEjectCumulativeTicks = 60000;

	private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	private static readonly SimpleCurve GeneCountChanceCurve = new SimpleCurve
	{
		new CurvePoint(1f, 0.7f),
		new CurvePoint(2f, 0.2f),
		new CurvePoint(3f, 0.08f),
		new CurvePoint(4f, 0.02f)
	};

	private Pawn ContainedPawn => innerContainer.FirstOrDefault() as Pawn;

	public bool PowerOn => PowerTraderComp.PowerOn;

	public override bool IsContentsSuspended => false;

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

	public Texture2D InsertPawnTex
	{
		get
		{
			if (cachedInsertPawnTex == null)
			{
				cachedInsertPawnTex = ContentFinder<Texture2D>.Get("UI/Gizmos/InsertPawn");
			}
			return cachedInsertPawnTex;
		}
	}

	public float HeldPawnDrawPos_Y => DrawPos.y + 0.03658537f;

	public float HeldPawnBodyAngle => base.Rotation.Opposite.AsAngle;

	public PawnPosture HeldPawnPosture => PawnPosture.LayingOnGroundFaceUp;

	public override Vector3 PawnDrawOffset => IntVec3.West.RotatedBy(base.Rotation).ToVector3() / def.size.x;

	public override void PostPostMake()
	{
		if (!ModLister.CheckBiotech("gene extractor"))
		{
			Destroy();
		}
		else
		{
			base.PostPostMake();
		}
	}

	public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
	{
		sustainerWorking = null;
		if (progressBar != null)
		{
			progressBar.Cleanup();
			progressBar = null;
		}
		base.DeSpawn(mode);
	}

	protected override void Tick()
	{
		base.Tick();
		if (this.IsHashIntervalTick(250))
		{
			PowerTraderComp.PowerOutput = (base.Working ? (0f - base.PowerComp.Props.PowerConsumption) : (0f - base.PowerComp.Props.idlePowerDraw));
		}
		if (base.Working)
		{
			if (ContainedPawn == null)
			{
				Cancel();
				return;
			}
			if (PowerTraderComp.PowerOn)
			{
				TickEffects();
				if (PowerOn)
				{
					ticksRemaining--;
				}
				if (ticksRemaining <= 0)
				{
					Finish();
				}
				return;
			}
			powerCutTicks++;
			if (powerCutTicks >= 60000)
			{
				Pawn containedPawn = ContainedPawn;
				if (containedPawn != null)
				{
					Messages.Message("GeneExtractorNoPowerEjectedMessage".Translate(containedPawn.Named("PAWN")), containedPawn, MessageTypeDefOf.NegativeEvent, historical: false);
				}
				Cancel();
			}
		}
		else
		{
			if (selectedPawn != null && selectedPawn.Dead)
			{
				Cancel();
			}
			if (progressBar != null)
			{
				progressBar.Cleanup();
				progressBar = null;
			}
		}
	}

	private void TickEffects()
	{
		if (sustainerWorking == null || sustainerWorking.Ended)
		{
			sustainerWorking = SoundDefOf.GeneExtractor_Working.TrySpawnSustainer(SoundInfo.InMap(this, MaintenanceType.PerTick));
		}
		else
		{
			sustainerWorking.Maintain();
		}
		if (progressBar == null)
		{
			progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn();
		}
		progressBar.EffectTick(new TargetInfo(base.Position + IntVec3.North.RotatedBy(base.Rotation), base.Map), TargetInfo.Invalid);
		MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBar.children[0]).mote;
		if (mote != null)
		{
			mote.progress = 1f - Mathf.Clamp01((float)ticksRemaining / 30000f);
			mote.offsetZ = ((base.Rotation == Rot4.North) ? 0.5f : (-0.5f));
		}
	}

	public override AcceptanceReport CanAcceptPawn(Pawn pawn)
	{
		if (!pawn.IsColonist && !pawn.IsSlaveOfColony && !pawn.IsPrisonerOfColony && (!pawn.IsColonySubhuman || !pawn.IsGhoul))
		{
			return false;
		}
		if (selectedPawn != null && selectedPawn != pawn)
		{
			return false;
		}
		if (!pawn.RaceProps.Humanlike || pawn.IsQuestLodger())
		{
			return false;
		}
		if (!PowerOn)
		{
			return "NoPower".Translate().CapitalizeFirst();
		}
		if (innerContainer.Count > 0)
		{
			return "Occupied".Translate();
		}
		if (pawn.genes == null || !pawn.genes.GenesListForReading.Any((Gene x) => x.def.passOnDirectly))
		{
			return "PawnHasNoGenes".Translate(pawn.Named("PAWN"));
		}
		if (!pawn.genes.GenesListForReading.Any((Gene x) => x.def.biostatArc == 0))
		{
			return "PawnHasNoNonArchiteGenes".Translate(pawn.Named("PAWN"));
		}
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.XenogerminationComa))
		{
			return "InXenogerminationComa".Translate();
		}
		return true;
	}

	private void Cancel()
	{
		startTick = -1;
		selectedPawn = null;
		sustainerWorking = null;
		powerCutTicks = 0;
		innerContainer.TryDropAll(def.hasInteractionCell ? InteractionCell : base.Position, base.Map, ThingPlaceMode.Near);
	}

	private void Finish()
	{
		selectedPawn = null;
		sustainerWorking = null;
		powerCutTicks = 0;
		List<GeneDef> genesToAdd;
		if (ContainedPawn != null)
		{
			Pawn containedPawn = ContainedPawn;
			Rand.PushState(containedPawn.thingIDNumber ^ startTick);
			genesToAdd = new List<GeneDef>();
			Genepack genepack = (Genepack)ThingMaker.MakeThing(ThingDefOf.Genepack);
			int num = Mathf.Min((int)GeneCountChanceCurve.RandomElementByWeight((CurvePoint p) => p.y).x, containedPawn.genes.GenesListForReading.Count((Gene x) => x.def.biostatArc == 0));
			for (int i = 0; i < num; i++)
			{
				if (!containedPawn.genes.GenesListForReading.TryRandomElementByWeight(SelectionWeight, out var result))
				{
					break;
				}
				genesToAdd.Add(result.def);
			}
			if (genesToAdd.Any())
			{
				genepack.Initialize(genesToAdd);
			}
			GeneUtility.ExtractXenogerm(containedPawn, Mathf.RoundToInt(60000f * GeneTuning.GeneExtractorRegrowingDurationDaysRange.RandomInRange));
			IntVec3 intVec = (def.hasInteractionCell ? InteractionCell : base.Position);
			innerContainer.TryDropAll(intVec, base.Map, ThingPlaceMode.Near);
			if (!containedPawn.Dead && (containedPawn.IsPrisonerOfColony || containedPawn.IsSlaveOfColony))
			{
				containedPawn.needs?.mood?.thoughts?.memories?.TryGainMemory(ThoughtDefOf.XenogermHarvested_Prisoner);
			}
			if (genesToAdd.Any())
			{
				GenPlace.TryPlaceThing(genepack, intVec, base.Map, ThingPlaceMode.Near, null, null, null);
			}
			Messages.Message("GeneExtractionComplete".Translate(containedPawn.Named("PAWN")) + ": " + genesToAdd.Select((GeneDef x) => x.label).ToCommaList().CapitalizeFirst(), new LookTargets(containedPawn, genepack), MessageTypeDefOf.PositiveEvent);
			Rand.PopState();
		}
		startTick = -1;
		float SelectionWeight(Gene g)
		{
			if (genesToAdd.Contains(g.def))
			{
				return 0f;
			}
			if (g.def.biostatArc > 0)
			{
				return 0f;
			}
			if (g.def.endogeneCategory == EndogeneCategory.Melanin)
			{
				return 0f;
			}
			if (!GeneTuning.BiostatRange.Includes(g.def.biostatMet + genesToAdd.Sum((GeneDef x) => x.biostatMet)))
			{
				return 0f;
			}
			if (g.def.biostatCpx > 0)
			{
				return 3f;
			}
			return 1f;
		}
	}

	public override void TryAcceptPawn(Pawn pawn)
	{
		if ((bool)CanAcceptPawn(pawn))
		{
			selectedPawn = pawn;
			bool num = pawn.DeSpawnOrDeselect();
			if (innerContainer.TryAddOrTransfer(pawn))
			{
				startTick = Find.TickManager.TicksGame;
				ticksRemaining = 30000;
			}
			if (num)
			{
				Find.Selector.Select(pawn, playSound: false, forceDesignatorDeselect: false);
			}
		}
	}

	protected override void SelectPawn(Pawn pawn)
	{
		if (pawn.health.hediffSet.HasHediff(HediffDefOf.XenogermReplicating))
		{
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmExtractXenogermWillKill".Translate(pawn.Named("PAWN")), delegate
			{
				base.SelectPawn(pawn);
			}));
		}
		else
		{
			base.SelectPawn(pawn);
		}
	}

	[IteratorStateMachine(typeof(_003CGetFloatMenuOptions_003Ed__37))]
	public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetFloatMenuOptions_003Ed__37(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__38))]
	public override IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__38(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
	{
		base.DynamicDrawPhaseAt(phase, drawLoc, flip);
		if (base.Working && ContainedPawn != null)
		{
			ContainedPawn.Drawer.renderer.DynamicDrawPhaseAt(phase, drawLoc + PawnDrawOffset, null, neverAimWeapon: true);
		}
	}

	public override string GetInspectString()
	{
		string text = base.GetInspectString();
		if (selectedPawn != null && innerContainer.Count == 0)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "WaitingForPawn".Translate(selectedPawn.Named("PAWN")).Resolve();
		}
		else if (base.Working && ContainedPawn != null)
		{
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text = text + "ExtractingXenogermFrom".Translate(ContainedPawn.Named("PAWN")).Resolve() + "\n";
			text = ((!PowerOn) ? (text + "ExtractionPausedNoPower".Translate((60000 - powerCutTicks).ToStringTicksToPeriod().Named("TIME")).Colorize(ColorLibrary.RedReadable)) : (text + "DurationLeft".Translate(ticksRemaining.ToStringTicksToPeriod()).Resolve()));
		}
		return text;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref ticksRemaining, "ticksRemaining", 0);
		Scribe_Values.Look(ref powerCutTicks, "powerCutTicks", 0);
	}
}
