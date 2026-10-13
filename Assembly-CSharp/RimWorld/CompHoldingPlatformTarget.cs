using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002CE6 RID: 11494
	[StaticConstructorOnStartup]
	public class CompHoldingPlatformTarget : ThingComp
	{
		// Token: 0x170028E0 RID: 10464
		// (get) Token: 0x0601024B RID: 66123 RVA: 0x004ABA8B File Offset: 0x004A9C8B
		public CompProperties_HoldingPlatformTarget Props
		{
			get
			{
				return (CompProperties_HoldingPlatformTarget)this.props;
			}
		}

		// Token: 0x170028E1 RID: 10465
		// (get) Token: 0x0601024C RID: 66124 RVA: 0x004ABA98 File Offset: 0x004A9C98
		public CompStudiable CompStudiable
		{
			get
			{
				CompStudiable result;
				if ((result = this.compStudiable) == null)
				{
					result = (this.compStudiable = this.parent.GetComp<CompStudiable>());
				}
				return result;
			}
		}

		// Token: 0x170028E2 RID: 10466
		// (get) Token: 0x0601024D RID: 66125 RVA: 0x004ABAC4 File Offset: 0x004A9CC4
		public CompActivity CompActivity
		{
			get
			{
				if (this.didCheckForActivityComp)
				{
					return this.compActivity;
				}
				if (this.compActivity == null)
				{
					Pawn pawn = this.parent as Pawn;
					this.compActivity = (((pawn != null) ? pawn.activity : null) ?? this.parent.GetComp<CompActivity>());
				}
				this.didCheckForActivityComp = true;
				return this.compActivity;
			}
		}

		// Token: 0x170028E3 RID: 10467
		// (get) Token: 0x0601024E RID: 66126 RVA: 0x004ABB21 File Offset: 0x004A9D21
		public CompEntityHolder EntityHolder
		{
			get
			{
				return this.targetHolder.TryGetComp<CompEntityHolder>();
			}
		}

		// Token: 0x170028E4 RID: 10468
		// (get) Token: 0x0601024F RID: 66127 RVA: 0x004ABB30 File Offset: 0x004A9D30
		public bool StudiedAtHoldingPlatform
		{
			get
			{
				if (!this.EverStudiable)
				{
					return false;
				}
				Pawn pawn = this.parent as Pawn;
				return pawn == null || ((!pawn.IsAnimal || pawn.RaceProps.IsAnomalyEntity) && ((pawn.IsMutant && pawn.mutant.Def.canBeCapturedToHoldingPlatform) || (!pawn.RaceProps.Humanlike && !pawn.Inhumanized() && !pawn.kindDef.studiableAsPrisoner)));
			}
		}

		// Token: 0x170028E5 RID: 10469
		// (get) Token: 0x06010250 RID: 66128 RVA: 0x004ABBB0 File Offset: 0x004A9DB0
		public bool CanStudy
		{
			get
			{
				return this.containmentMode == EntityContainmentMode.Study && this.EverStudiable;
			}
		}

		// Token: 0x170028E6 RID: 10470
		// (get) Token: 0x06010251 RID: 66129 RVA: 0x004ABBC3 File Offset: 0x004A9DC3
		private bool EverStudiable
		{
			get
			{
				return !this.parent.Destroyed && (!(this.parent is Pawn) || (this.CompStudiable != null && this.CompStudiable.AnomalyKnowledge > 0f));
			}
		}

		// Token: 0x170028E7 RID: 10471
		// (get) Token: 0x06010252 RID: 66130 RVA: 0x004ABBFF File Offset: 0x004A9DFF
		public Building_HoldingPlatform HeldPlatform
		{
			get
			{
				return this.parent.ParentHolder as Building_HoldingPlatform;
			}
		}

		// Token: 0x170028E8 RID: 10472
		// (get) Token: 0x06010253 RID: 66131 RVA: 0x004ABC11 File Offset: 0x004A9E11
		public bool CurrentlyHeldOnPlatform
		{
			get
			{
				return this.HeldPlatform != null && this.parent.SpawnedOrAnyParentSpawned;
			}
		}

		// Token: 0x170028E9 RID: 10473
		// (get) Token: 0x06010254 RID: 66132 RVA: 0x004ABC28 File Offset: 0x004A9E28
		public bool CanBeCaptured
		{
			get
			{
				Pawn pawn = this.parent as Pawn;
				if (pawn == null)
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
				if (this.CompStudiable != null && Find.Anomaly.HighestLevelReached < this.CompStudiable.Props.minMonolithLevelForStudy && Find.Anomaly.GenerateMonolith)
				{
					return false;
				}
				if (!pawn.Downed && !(pawn.ParentHolder is Pawn_CarryTracker))
				{
					CompActivity compActivity = this.CompActivity;
					return compActivity != null && compActivity.IsDormant;
				}
				return true;
			}
		}

		// Token: 0x06010255 RID: 66133 RVA: 0x004ABCCB File Offset: 0x004A9ECB
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (this.targetHolder != null && this.parent.Map != this.targetHolder.Map)
			{
				this.targetHolder = null;
			}
		}

		// Token: 0x06010256 RID: 66134 RVA: 0x004ABCF4 File Offset: 0x004A9EF4
		public override void CompTick()
		{
			base.CompTick();
			Pawn pawn = this.parent as Pawn;
			if (pawn != null)
			{
				if (!pawn.SpawnedOrAnyParentSpawned)
				{
					return;
				}
				if (this.CurrentlyHeldOnPlatform)
				{
					this.CaptivityTick(pawn);
				}
				else if (this.targetHolder != null && (this.targetHolder.Destroyed || this.EntityHolder.HeldPawn != null))
				{
					this.targetHolder = null;
				}
				if (this.isEscaping && pawn.mindState.enemyTarget == null)
				{
					this.isEscaping = false;
				}
			}
			Building_HoldingPlatform heldPlatform = this.HeldPlatform;
			if (heldPlatform != null && heldPlatform.HasAttachedBioferriteHarvester)
			{
				this.extractBioferrite = false;
			}
			if (this.CanBeCaptured)
			{
				LessonAutoActivator.TeachOpportunity(ConceptDefOf.CapturingEntities, OpportunityType.Important);
			}
		}

		// Token: 0x06010257 RID: 66135 RVA: 0x004ABDA4 File Offset: 0x004A9FA4
		private void CaptivityTick(Pawn pawn)
		{
			pawn.mindState.entityTicksInCaptivity++;
			Building_HoldingPlatform building_HoldingPlatform = this.targetHolder as Building_HoldingPlatform;
			if (building_HoldingPlatform != null && building_HoldingPlatform != this.HeldPlatform && building_HoldingPlatform.Occupied)
			{
				this.targetHolder = null;
			}
			if (this.parent.IsHashIntervalTick(2500))
			{
				float num = ContainmentUtility.InitiateEscapeMtbDays(pawn, null);
				if (num >= 0f && Rand.MTBEventOccurs(num, 60000f, 2500f))
				{
					this.Escape(true);
				}
			}
		}

		// Token: 0x06010258 RID: 66136 RVA: 0x004ABE28 File Offset: 0x004AA028
		public void Notify_HeldOnPlatform(ThingOwner newOwner)
		{
			this.targetHolder = null;
			Pawn pawn = null;
			Pawn pawn2 = this.parent as Pawn;
			if (pawn2 != null)
			{
				pawn2.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn2, false);
				pawn = pawn2;
			}
			if (newOwner != null)
			{
				if (this.Props.heldPawnKind != null)
				{
					PawnKindDef heldPawnKind = this.Props.heldPawnKind;
					Faction ofEntities = Faction.OfEntities;
					PawnGenerationContext context = PawnGenerationContext.NonPlayer;
					float? fixedBiologicalAge = new float?(0f);
					Pawn pawn3 = PawnGenerator.GeneratePawn(new PawnGenerationRequest(heldPawnKind, ofEntities, context, null, true, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedBiologicalAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
					newOwner.TryAdd(pawn3, true);
					CompHoldingPlatformTarget compHoldingPlatformTarget = pawn3.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget != null)
					{
						compHoldingPlatformTarget.Notify_HeldOnPlatform(newOwner);
					}
					pawn = pawn3;
					if (this.Props.heldPawnKind == PawnKindDefOf.Revenant)
					{
						CompBiosignatureOwner compBiosignatureOwner = this.parent.TryGetComp<CompBiosignatureOwner>();
						if (compBiosignatureOwner != null)
						{
							pawn3.TryGetComp<CompRevenant>().biosignature = compBiosignatureOwner.biosignature;
						}
						CompStudiable compStudiable;
						if (pawn3.TryGetComp(out compStudiable))
						{
							compStudiable.lastStudiedTick = Find.TickManager.TicksGame;
						}
					}
					Find.HiddenItemsManager.SetDiscovered(pawn3.def);
					this.parent.Destroy(DestroyMode.Vanish);
				}
				this.containmentMode = EntityContainmentMode.Study;
			}
			if (pawn == null || this.HeldPlatform == null)
			{
				return;
			}
			Lord lord = pawn.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(pawn, PawnLostCondition.MadePrisoner, null);
			}
			CompActivity compActivity = pawn.TryGetComp<CompActivity>();
			if (compActivity != null)
			{
				compActivity.Notify_HeldOnPlatform();
			}
			Find.StudyManager.UpdateStudiableCache(this.HeldPlatform, this.HeldPlatform.Map);
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.CapturingEntities, KnowledgeAmount.Total);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ContainingEntities, OpportunityType.Important);
		}

		// Token: 0x06010259 RID: 66137 RVA: 0x004AC022 File Offset: 0x004AA222
		public void Notify_ReleasedFromPlatform()
		{
			Find.StudyManager.UpdateStudiableCache(this.HeldPlatform, this.HeldPlatform.Map);
		}

		// Token: 0x0601025A RID: 66138 RVA: 0x004AC040 File Offset: 0x004AA240
		public void Escape(bool initiator)
		{
			List<Pawn> list = new List<Pawn>();
			List<Building> list2 = new List<Building>
			{
				this.HeldPlatform
			};
			this.HeldPlatform.EjectContents();
			Pawn pawn = this.parent as Pawn;
			if (pawn == null)
			{
				return;
			}
			pawn.health.overrideDeathOnDownedChance = 0f;
			list.Add(pawn);
			this.isEscaping = true;
			if (this.Props.lookForTargetOnEscape && !pawn.Downed)
			{
				Pawn enemyTarget = (Pawn)AttackTargetFinder.BestAttackTarget(pawn, TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable, (Thing x) => x is Pawn && x.def.race.intelligence >= Intelligence.ToolUser, 0f, 9999f, default(IntVec3), float.MaxValue, true, true, true, false);
				pawn.mindState.enemyTarget = enemyTarget;
			}
			CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
			if (compRevenant != null)
			{
				compRevenant.revenantState = RevenantState.Escape;
			}
			HediffComp_Invisibility invisibilityComp = pawn.GetInvisibilityComp();
			if (invisibilityComp != null)
			{
				invisibilityComp.BecomeVisible(true);
			}
			if (!initiator)
			{
				return;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null)
			{
				return;
			}
			foreach (Thing thing in (from x in room.ContainedAndAdjacentThings
			where x is Building_HoldingPlatform
			select x).ToList<Thing>())
			{
				Pawn heldPawn = ((Building_HoldingPlatform)thing).HeldPawn;
				if (heldPawn != null && heldPawn != pawn)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = heldPawn.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
					{
						float num = ContainmentUtility.InitiateEscapeMtbDays(heldPawn, null);
						if (num > 0f && Rand.Chance(CompHoldingPlatformTarget.JoinEscapeChanceFromEscapeIntervalCurve.Evaluate(num)))
						{
							list.Add(heldPawn);
							list2.Add(compHoldingPlatformTarget.HeldPlatform);
							compHoldingPlatformTarget.Escape(false);
						}
					}
				}
			}
			Find.LetterStack.ReceiveLetter(LetterMaker.MakeLetter("LetterLabelEscapingFromHoldingPlatform".Translate(), "LetterEscapingFromHoldingPlatform".Translate((from p in list
			select p.LabelCap).ToLineList("  - ", false)), LetterDefOf.ThreatBig, list2, null, null, null), null, 0, true);
		}

		// Token: 0x0601025B RID: 66139 RVA: 0x004AC288 File Offset: 0x004AA488
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.StudiedAtHoldingPlatform && !this.CurrentlyHeldOnPlatform && this.CanBeCaptured)
			{
				if (this.targetHolder != null)
				{
					yield return new Command_Action
					{
						defaultLabel = "CancelCapture".Translate(),
						defaultDesc = "CancelCaptureDesc".Translate(this.parent).Resolve(),
						icon = CompHoldingPlatformTarget.CancelTex,
						action = delegate()
						{
							this.targetHolder = null;
						}
					};
				}
				else if (this.parent.Spawned)
				{
					yield return new Command_Action
					{
						defaultLabel = "CaptureEntity".Translate() + "...",
						defaultDesc = "CaptureEntityDesc".Translate(this.parent).Resolve(),
						icon = CompHoldingPlatformTarget.CaptureIcon.Texture,
						action = delegate()
						{
							StudyUtility.TargetHoldingPlatformForEntity(null, this.parent, false, null);
						},
						activateSound = SoundDefOf.Click,
						Disabled = !StudyUtility.HoldingPlatformAvailableOnCurrentMap(),
						disabledReason = "NoHoldingPlatformsAvailable".Translate()
					};
				}
			}
			if (this.CurrentlyHeldOnPlatform)
			{
				if (this.targetHolder != null && this.targetHolder != this.HeldPlatform)
				{
					yield return new Command_Action
					{
						defaultLabel = "CancelTransfer".Translate(),
						defaultDesc = "CancelTransferDesc".Translate(),
						icon = CompHoldingPlatformTarget.CancelTex,
						action = delegate()
						{
							this.targetHolder = null;
						}
					};
				}
				else
				{
					yield return new Command_Action
					{
						defaultLabel = "TransferEntity".Translate(this.parent) + "...",
						defaultDesc = "TransferEntityDesc".Translate(this.parent).Resolve(),
						icon = CompHoldingPlatformTarget.TransferIcon.Texture,
						action = delegate()
						{
							StudyUtility.TargetHoldingPlatformForEntity(null, this.parent, true, this.HeldPlatform);
						},
						activateSound = SoundDefOf.Click
					};
				}
			}
			if (DebugSettings.ShowDevGizmos && this.CurrentlyHeldOnPlatform)
			{
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = "DEV: Escape",
					action = delegate()
					{
						this.Escape(true);
					}
				};
				yield return command_Action;
				yield return new Command_Action
				{
					defaultLabel = "DEV: Kill",
					action = delegate()
					{
						this.parent.Kill(null, null);
					}
				};
			}
			yield break;
		}

		// Token: 0x0601025C RID: 66140 RVA: 0x004AC298 File Offset: 0x004AA498
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.CanBeCaptured && ContainmentUtility.ShowContainmentStats(this.parent))
			{
				float num = this.parent.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1);
				if (this.Props.heldPawnKind != null)
				{
					num = this.Props.heldPawnKind.race.GetStatValueAbstract(StatDefOf.MinimumContainmentStrength, null);
				}
				if (!text.NullOrEmpty())
				{
					text += "\n";
				}
				text += "Capturable".Translate() + ". " + StatDefOf.MinimumContainmentStrength.LabelCap + ": " + num.ToString("F1");
			}
			return text;
		}

		// Token: 0x0601025D RID: 66141 RVA: 0x004AC364 File Offset: 0x004AA564
		public override void PostExposeData()
		{
			Scribe_References.Look<Thing>(ref this.targetHolder, "targetHolder", false);
			Scribe_Values.Look<bool>(ref this.isEscaping, "isEscaping", false, false);
			Scribe_Values.Look<EntityContainmentMode>(ref this.containmentMode, "containmentMode", EntityContainmentMode.MaintainOnly, false);
			Scribe_Values.Look<bool>(ref this.extractBioferrite, "extractBioferrite", false, false);
		}

		// Token: 0x0400A8A9 RID: 43177
		private const int CheckInitiateEscapeIntervalTicks = 2500;

		// Token: 0x0400A8AA RID: 43178
		private static readonly SimpleCurve JoinEscapeChanceFromEscapeIntervalCurve = new SimpleCurve
		{
			{
				new CurvePoint(120f, 0.33f),
				true
			},
			{
				new CurvePoint(60f, 0.5f),
				true
			},
			{
				new CurvePoint(10f, 0.9f),
				true
			}
		};

		// Token: 0x0400A8AB RID: 43179
		private static readonly CachedTexture CaptureIcon = new CachedTexture("UI/Commands/CaptureEntity");

		// Token: 0x0400A8AC RID: 43180
		private static readonly CachedTexture TransferIcon = new CachedTexture("UI/Commands/TransferEntity");

		// Token: 0x0400A8AD RID: 43181
		private static readonly Texture2D CancelTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x0400A8AE RID: 43182
		public Thing targetHolder;

		// Token: 0x0400A8AF RID: 43183
		public bool isEscaping;

		// Token: 0x0400A8B0 RID: 43184
		public EntityContainmentMode containmentMode;

		// Token: 0x0400A8B1 RID: 43185
		public bool extractBioferrite;

		// Token: 0x0400A8B2 RID: 43186
		[Unsaved(false)]
		private CompStudiable compStudiable;

		// Token: 0x0400A8B3 RID: 43187
		[Unsaved(false)]
		private CompActivity compActivity;

		// Token: 0x0400A8B4 RID: 43188
		[Unsaved(false)]
		private bool didCheckForActivityComp;
	}
}
