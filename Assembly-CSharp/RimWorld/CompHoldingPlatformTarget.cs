using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

[StaticConstructorOnStartup]
public class CompHoldingPlatformTarget : ThingComp
{
	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__38 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompHoldingPlatformTarget _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__38(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			CompHoldingPlatformTarget CS_0024_003C_003E8__locals21 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals21.StudiedAtHoldingPlatform && !CS_0024_003C_003E8__locals21.CurrentlyHeldOnPlatform && CS_0024_003C_003E8__locals21.CanBeCaptured)
				{
					if (CS_0024_003C_003E8__locals21.targetHolder != null)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CancelCapture".Translate(),
							defaultDesc = "CancelCaptureDesc".Translate(CS_0024_003C_003E8__locals21.parent).Resolve(),
							icon = CancelTex,
							action = delegate
							{
								CS_0024_003C_003E8__locals21.targetHolder = null;
							}
						};
						_003C_003E1__state = 1;
						return true;
					}
					if (CS_0024_003C_003E8__locals21.parent.Spawned)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CaptureEntity".Translate() + "...",
							defaultDesc = "CaptureEntityDesc".Translate(CS_0024_003C_003E8__locals21.parent).Resolve(),
							icon = CaptureIcon.Texture,
							action = delegate
							{
								StudyUtility.TargetHoldingPlatformForEntity(null, CS_0024_003C_003E8__locals21.parent);
							},
							activateSound = SoundDefOf.Click,
							Disabled = !StudyUtility.HoldingPlatformAvailableOnCurrentMap(),
							disabledReason = "NoHoldingPlatformsAvailable".Translate()
						};
						_003C_003E1__state = 2;
						return true;
					}
				}
				goto IL_0194;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0194;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0194;
			case 3:
				_003C_003E1__state = -1;
				goto IL_02b1;
			case 4:
				_003C_003E1__state = -1;
				goto IL_02b1;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = new Command_Action
				{
					defaultLabel = "DEV: Kill",
					action = delegate
					{
						CS_0024_003C_003E8__locals21.parent.Kill(null);
					}
				};
				_003C_003E1__state = 6;
				return true;
			case 6:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0194:
				if (CS_0024_003C_003E8__locals21.CurrentlyHeldOnPlatform)
				{
					if (CS_0024_003C_003E8__locals21.targetHolder != null && CS_0024_003C_003E8__locals21.targetHolder != CS_0024_003C_003E8__locals21.HeldPlatform)
					{
						_003C_003E2__current = new Command_Action
						{
							defaultLabel = "CancelTransfer".Translate(),
							defaultDesc = "CancelTransferDesc".Translate(),
							icon = CancelTex,
							action = delegate
							{
								CS_0024_003C_003E8__locals21.targetHolder = null;
							}
						};
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "TransferEntity".Translate(CS_0024_003C_003E8__locals21.parent) + "...",
						defaultDesc = "TransferEntityDesc".Translate(CS_0024_003C_003E8__locals21.parent).Resolve(),
						icon = TransferIcon.Texture,
						action = delegate
						{
							StudyUtility.TargetHoldingPlatformForEntity(null, CS_0024_003C_003E8__locals21.parent, transferBetweenPlatforms: true, CS_0024_003C_003E8__locals21.HeldPlatform);
						},
						activateSound = SoundDefOf.Click
					};
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_02b1;
				IL_02b1:
				if (DebugSettings.ShowDevGizmos && CS_0024_003C_003E8__locals21.CurrentlyHeldOnPlatform)
				{
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = "DEV: Escape",
						action = delegate
						{
							CS_0024_003C_003E8__locals21.Escape(initiator: true);
						}
					};
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 5;
					return true;
				}
				break;
			}
			return false;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CCompGetGizmosExtra_003Ed__38 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__38(0)
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

	private const int CheckInitiateEscapeIntervalTicks = 2500;

	private static readonly SimpleCurve JoinEscapeChanceFromEscapeIntervalCurve = new SimpleCurve
	{
		new CurvePoint(120f, 0.33f),
		new CurvePoint(60f, 0.5f),
		new CurvePoint(10f, 0.9f)
	};

	private static readonly CachedTexture CaptureIcon = new CachedTexture("UI/Commands/CaptureEntity");

	private static readonly CachedTexture TransferIcon = new CachedTexture("UI/Commands/TransferEntity");

	private static readonly Texture2D CancelTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel");

	public Thing targetHolder;

	public bool isEscaping;

	public EntityContainmentMode containmentMode;

	public bool extractBioferrite;

	[Unsaved(false)]
	private CompStudiable compStudiable;

	[Unsaved(false)]
	private CompActivity compActivity;

	[Unsaved(false)]
	private bool didCheckForActivityComp;

	public CompProperties_HoldingPlatformTarget Props => (CompProperties_HoldingPlatformTarget)props;

	public CompStudiable CompStudiable => compStudiable ?? (compStudiable = parent.GetComp<CompStudiable>());

	public CompActivity CompActivity
	{
		get
		{
			if (didCheckForActivityComp)
			{
				return compActivity;
			}
			if (compActivity == null)
			{
				compActivity = (parent as Pawn)?.activity ?? parent.GetComp<CompActivity>();
			}
			didCheckForActivityComp = true;
			return compActivity;
		}
	}

	public CompEntityHolder EntityHolder => targetHolder.TryGetComp<CompEntityHolder>();

	public bool StudiedAtHoldingPlatform
	{
		get
		{
			if (!EverStudiable)
			{
				return false;
			}
			if (parent is Pawn pawn)
			{
				if (pawn.IsAnimal && !pawn.RaceProps.IsAnomalyEntity)
				{
					return false;
				}
				if (pawn.IsMutant && pawn.mutant.Def.canBeCapturedToHoldingPlatform)
				{
					return true;
				}
				if (!pawn.RaceProps.Humanlike && !pawn.Inhumanized())
				{
					return !pawn.kindDef.studiableAsPrisoner;
				}
				return false;
			}
			return true;
		}
	}

	public bool CanStudy
	{
		get
		{
			if (containmentMode == EntityContainmentMode.Study)
			{
				return EverStudiable;
			}
			return false;
		}
	}

	private bool EverStudiable
	{
		get
		{
			if (parent.Destroyed)
			{
				return false;
			}
			if (parent is Pawn)
			{
				if (CompStudiable != null)
				{
					return CompStudiable.AnomalyKnowledge > 0f;
				}
				return false;
			}
			return true;
		}
	}

	public Building_HoldingPlatform HeldPlatform => parent.ParentHolder as Building_HoldingPlatform;

	public bool CurrentlyHeldOnPlatform
	{
		get
		{
			if (HeldPlatform != null)
			{
				return parent.SpawnedOrAnyParentSpawned;
			}
			return false;
		}
	}

	public bool CanBeCaptured
	{
		get
		{
			if (!(parent is Pawn pawn))
			{
				return true;
			}
			if (pawn.Faction == Faction.OfPlayer)
			{
				return false;
			}
			if (pawn.IsMutant && !pawn.mutant.Def.canBeCapturedToHoldingPlatform)
			{
				return false;
			}
			if (CompStudiable != null && Find.Anomaly.HighestLevelReached < CompStudiable.Props.minMonolithLevelForStudy && Find.Anomaly.GenerateMonolith)
			{
				return false;
			}
			if (!pawn.Downed && !(pawn.ParentHolder is Pawn_CarryTracker))
			{
				return CompActivity?.IsDormant ?? false;
			}
			return true;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		if (targetHolder != null && parent.Map != targetHolder.Map)
		{
			targetHolder = null;
		}
	}

	public override void CompTick()
	{
		base.CompTick();
		if (parent is Pawn pawn)
		{
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			if (CurrentlyHeldOnPlatform)
			{
				CaptivityTick(pawn);
			}
			else if (targetHolder != null && (targetHolder.Destroyed || EntityHolder.HeldPawn != null))
			{
				targetHolder = null;
			}
			if (isEscaping && pawn.mindState.enemyTarget == null)
			{
				isEscaping = false;
			}
		}
		Building_HoldingPlatform heldPlatform = HeldPlatform;
		if (heldPlatform != null && heldPlatform.HasAttachedBioferriteHarvester)
		{
			extractBioferrite = false;
		}
		if (CanBeCaptured)
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.CapturingEntities, OpportunityType.Important);
		}
	}

	private void CaptivityTick(Pawn pawn)
	{
		pawn.mindState.entityTicksInCaptivity++;
		if (targetHolder is Building_HoldingPlatform building_HoldingPlatform && building_HoldingPlatform != HeldPlatform && building_HoldingPlatform.Occupied)
		{
			targetHolder = null;
		}
		if (parent.IsHashIntervalTick(2500))
		{
			float num = ContainmentUtility.InitiateEscapeMtbDays(pawn);
			if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
			{
				Escape(initiator: true);
			}
		}
	}

	public void Notify_HeldOnPlatform(ThingOwner newOwner)
	{
		targetHolder = null;
		Pawn pawn = null;
		if (parent is Pawn pawn2)
		{
			pawn2.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2);
			pawn = pawn2;
		}
		if (newOwner != null)
		{
			if (Props.heldPawnKind != null)
			{
				PawnKindDef heldPawnKind = Props.heldPawnKind;
				Faction ofEntities = Faction.OfEntities;
				float? fixedBiologicalAge = 0f;
				Pawn pawn3 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(heldPawnKind, ofEntities, PawnGenerationContext.NonPlayer, null, forceGenerateNewPawn: true, allowDead: false, allowDowned: false, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null));
				newOwner.TryAdd(pawn3);
				pawn3.TryGetComp<CompHoldingPlatformTarget>()?.Notify_HeldOnPlatform(newOwner);
				pawn = pawn3;
				if (Props.heldPawnKind == PawnKindDefOf.Revenant)
				{
					CompBiosignatureOwner compBiosignatureOwner = parent.TryGetComp<CompBiosignatureOwner>();
					if (compBiosignatureOwner != null)
					{
						pawn3.TryGetComp<CompRevenant>().biosignature = compBiosignatureOwner.biosignature;
					}
					if (pawn3.TryGetComp<CompStudiable>(out var comp))
					{
						comp.lastStudiedTick = Find.TickManager.TicksGame;
					}
				}
				Find.HiddenItemsManager.SetDiscovered(pawn3.def);
				parent.Destroy();
			}
			containmentMode = EntityContainmentMode.Study;
		}
		if (pawn != null && HeldPlatform != null)
		{
			pawn.GetLord()?.Notify_PawnLost(pawn, PawnLostCondition.MadePrisoner, null);
			pawn.TryGetComp<CompActivity>()?.Notify_HeldOnPlatform();
			Find.StudyManager.UpdateStudiableCache(HeldPlatform, HeldPlatform.Map);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CapturingEntities, KnowledgeAmount.Total);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ContainingEntities, OpportunityType.Important);
		}
	}

	public void Notify_ReleasedFromPlatform()
	{
		Find.StudyManager.UpdateStudiableCache(HeldPlatform, HeldPlatform.Map);
	}

	public void Escape(bool initiator)
	{
		List<Pawn> list = new List<Pawn>();
		List<Building> list2 = new List<Building> { HeldPlatform };
		HeldPlatform.EjectContents();
		if (!(parent is Pawn pawn))
		{
			return;
		}
		pawn.health.overrideDeathOnDownedChance = 0f;
		list.Add(pawn);
		isEscaping = true;
		if (Props.lookForTargetOnEscape && !pawn.Downed)
		{
			Pawn enemyTarget = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && (int)x.def.race.intelligence >= 1, 0f, 9999f, default(IntVec3), float.MaxValue, canBashDoors: true, canTakeTargetsCloserThanEffectiveMinRange: true, canBashFences: true);
			pawn.mindState.enemyTarget = enemyTarget;
		}
		CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
		if (compRevenant != null)
		{
			compRevenant.revenantState = RevenantState.Escape;
		}
		pawn.GetInvisibilityComp()?.BecomeVisible(instant: true);
		if (!initiator)
		{
			return;
		}
		Room room = pawn.GetRoom();
		if (room == null)
		{
			return;
		}
		foreach (Building_HoldingPlatform item in room.ContainedAndAdjacentThings.Where((Thing x) => x is Building_HoldingPlatform).ToList())
		{
			Pawn heldPawn = item.HeldPawn;
			if (heldPawn == null || heldPawn == pawn)
			{
				continue;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = heldPawn.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
			{
				float num = ContainmentUtility.InitiateEscapeMtbDays(heldPawn);
				if (!(num <= 0f) && Rand.Chance(JoinEscapeChanceFromEscapeIntervalCurve.Evaluate(num)))
				{
					list.Add(heldPawn);
					list2.Add(compHoldingPlatformTarget.HeldPlatform);
					compHoldingPlatformTarget.Escape(initiator: false);
				}
			}
		}
		Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("LetterLabelEscapingFromHoldingPlatform".Translate(), "LetterEscapingFromHoldingPlatform".Translate(list.Select((Pawn p) => p.LabelCap).ToLineList("  - ")), LetterDefOf.ThreatBig, list2));
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__38))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__38(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		string text = base.CompInspectStringExtra();
		if (CanBeCaptured && ContainmentUtility.ShowContainmentStats(parent))
		{
			float num = parent.GetStatValue(StatDefOf.MinimumContainmentStrength);
			if (Props.heldPawnKind != null)
			{
				num = Props.heldPawnKind.race.GetStatValueAbstract(StatDefOf.MinimumContainmentStrength);
			}
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			text += "Capturable".Translate() + ". " + StatDefOf.MinimumContainmentStrength.LabelCap + ": " + num.ToString("F1");
		}
		return text;
	}

	public override void PostExposeData()
	{
		Scribe_References.Look(ref targetHolder, "targetHolder");
		Scribe_Values.Look(ref isEscaping, "isEscaping", defaultValue: false);
		Scribe_Values.Look(ref containmentMode, "containmentMode", EntityContainmentMode.MaintainOnly);
		Scribe_Values.Look(ref extractBioferrite, "extractBioferrite", defaultValue: false);
	}
}
