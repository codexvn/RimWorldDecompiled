using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x02000872 RID: 2162
	public class Corpse : ThingWithComps, IThingHolder, IStrippable, IBillGiver, IObservedThoughtGiver
	{
		// Token: 0x170009FE RID: 2558
		// (get) Token: 0x0600369F RID: 13983 RVA: 0x001254CA File Offset: 0x001236CA
		// (set) Token: 0x060036A0 RID: 13984 RVA: 0x001254E8 File Offset: 0x001236E8
		public Pawn InnerPawn
		{
			get
			{
				if (this.innerContainer.Count <= 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
			set
			{
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count > 0)
				{
					Log.Error("Setting InnerPawn in corpse that already has one.");
					this.innerContainer.Clear();
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x170009FF RID: 2559
		// (get) Token: 0x060036A1 RID: 13985 RVA: 0x00125535 File Offset: 0x00123735
		// (set) Token: 0x060036A2 RID: 13986 RVA: 0x00125548 File Offset: 0x00123748
		public int Age
		{
			get
			{
				return Find.TickManager.TicksGame - this.timeOfDeath;
			}
			set
			{
				this.timeOfDeath = Find.TickManager.TicksGame - value;
			}
		}

		// Token: 0x17000A00 RID: 2560
		// (get) Token: 0x060036A3 RID: 13987 RVA: 0x0012555C File Offset: 0x0012375C
		public override string LabelNoCount
		{
			get
			{
				if (this.cachedLabel == null)
				{
					if (this.Bugged)
					{
						Log.Error("LabelNoCount on Corpse while Bugged.");
						return string.Empty;
					}
					this.cachedLabel = "DeadLabel".Translate(this.InnerPawn.Label, this.InnerPawn);
				}
				return this.cachedLabel;
			}
		}

		// Token: 0x17000A01 RID: 2561
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x001255BF File Offset: 0x001237BF
		public override bool IngestibleNow
		{
			get
			{
				if (this.Bugged)
				{
					Log.Error("IngestibleNow on Corpse while Bugged.");
					return false;
				}
				return base.IngestibleNow && this.InnerPawn.RaceProps.IsFlesh && this.GetRotStage() == RotStage.Fresh;
			}
		}

		// Token: 0x17000A02 RID: 2562
		// (get) Token: 0x060036A5 RID: 13989 RVA: 0x00125600 File Offset: 0x00123800
		public RotDrawMode CurRotDrawMode
		{
			get
			{
				CompRottable comp = base.GetComp<CompRottable>();
				if (comp != null)
				{
					switch (comp.Stage)
					{
					case RotStage.Rotting:
						return RotDrawMode.Rotting;
					case RotStage.Dessicated:
						return RotDrawMode.Dessicated;
					}
					return RotDrawMode.Fresh;
				}
				return RotDrawMode.Fresh;
			}
		}

		// Token: 0x17000A03 RID: 2563
		// (get) Token: 0x060036A6 RID: 13990 RVA: 0x0012563C File Offset: 0x0012383C
		private bool ShouldVanish
		{
			get
			{
				return this.InnerPawn.RaceProps.Animal && this.vanishAfterTimestamp > 0 && this.Age >= this.vanishAfterTimestamp && base.Spawned && this.GetRoom(RegionType.Set_All) != null && this.GetRoom(RegionType.Set_All).TouchesMapEdge && !base.Map.roofGrid.Roofed(base.Position);
			}
		}

		// Token: 0x17000A04 RID: 2564
		// (get) Token: 0x060036A7 RID: 13991 RVA: 0x001256AE File Offset: 0x001238AE
		public BillStack BillStack
		{
			get
			{
				return this.operationsBillStack;
			}
		}

		// Token: 0x17000A05 RID: 2565
		// (get) Token: 0x060036A8 RID: 13992 RVA: 0x001256B6 File Offset: 0x001238B6
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		// Token: 0x17000A06 RID: 2566
		// (get) Token: 0x060036A9 RID: 13993 RVA: 0x001256C6 File Offset: 0x001238C6
		public bool Bugged
		{
			get
			{
				if (this.innerContainer.Count != 0)
				{
					Pawn pawn = this.innerContainer[0];
					if (((pawn != null) ? pawn.def : null) != null)
					{
						return this.innerContainer[0].kindDef == null;
					}
				}
				return true;
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x00125705 File Offset: 0x00123905
		public Corpse()
		{
			this.operationsBillStack = new BillStack(this);
			this.innerContainer = new ThingOwner<Pawn>(this, true, LookMode.Reference, false);
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x00125738 File Offset: 0x00123938
		public bool CurrentlyUsableForBills()
		{
			return this.InteractionCell.IsValid;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x00125753 File Offset: 0x00123953
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x0012575B File Offset: 0x0012395B
		public bool AnythingToStrip()
		{
			return this.InnerPawn.AnythingToStrip();
		}

		// Token: 0x060036AE RID: 13998 RVA: 0x00125768 File Offset: 0x00123968
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x060036AF RID: 13999 RVA: 0x00125770 File Offset: 0x00123970
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x060036B0 RID: 14000 RVA: 0x0012577E File Offset: 0x0012397E
		public override void PostMake()
		{
			base.PostMake();
			this.timeOfDeath = Find.TickManager.TicksGame;
		}

		// Token: 0x060036B1 RID: 14001 RVA: 0x00125798 File Offset: 0x00123998
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Bugged)
			{
				Log.Error(((this != null) ? this.ToString() : null) + " spawned in bugged state.");
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			this.InnerPawn.Rotation = Rot4.South;
			List<Hediff> hediffs = this.InnerPawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				hediffs[i].Notify_PawnCorpseSpawned();
			}
			this.NotifyColonistBar();
		}

		// Token: 0x060036B2 RID: 14002 RVA: 0x0012581C File Offset: 0x00123A1C
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (dinfo != null && dinfo.Value.Def == DamageDefOf.Bomb)
			{
				CompRottable comp = base.GetComp<CompRottable>();
				ThingDef thingDef;
				if (comp != null && comp.Stage == RotStage.Rotting)
				{
					thingDef = ThingDefOf.Filth_CorpseBile;
				}
				else
				{
					thingDef = this.InnerPawn.RaceProps.BloodDef;
				}
				if (thingDef != null)
				{
					int randomInRange = Corpse.ExplodeFilthCountRange.RandomInRange;
					for (int i = 0; i < randomInRange; i++)
					{
						FilthMaker.TryMakeFilth(base.PositionHeld, base.MapHeld, thingDef, 1, FilthSourceFlags.None, true);
					}
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x060036B3 RID: 14003 RVA: 0x001258B4 File Offset: 0x00123AB4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.DeSpawn(mode);
			if (!this.Bugged)
			{
				this.NotifyColonistBar();
			}
		}

		// Token: 0x060036B4 RID: 14004 RVA: 0x001258CC File Offset: 0x00123ACC
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Pawn pawn = null;
			if (!this.Bugged)
			{
				pawn = this.InnerPawn;
				this.NotifyColonistBar();
				this.innerContainer.Clear();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_CorpseLost(this, null);
			}
			base.Destroy(mode);
			if (pawn != null)
			{
				Corpse.PostCorpseDestroy(pawn, false);
			}
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x00125928 File Offset: 0x00123B28
		public static void PostCorpseDestroy(Pawn pawn, bool discarded = false)
		{
			Pawn_Ownership ownership = pawn.ownership;
			if (ownership != null)
			{
				ownership.UnclaimAll();
			}
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (equipment != null)
			{
				equipment.DestroyAllEquipment(DestroyMode.Vanish);
			}
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			pawn.health.Notify_PawnCorpseDestroyed();
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel != null)
			{
				apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (!PawnGenerator.IsBeingGenerated(pawn) && !discarded)
			{
				Ideo ideo = pawn.Ideo;
				if (ideo == null)
				{
					return;
				}
				ideo.Notify_MemberCorpseDestroyed(pawn);
			}
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x001259A0 File Offset: 0x00123BA0
		public override void TickRare()
		{
			this.TickRareInt();
			if (base.Destroyed)
			{
				return;
			}
			if (this.Bugged)
			{
				Log.Error(((this != null) ? this.ToString() : null) + " has null innerPawn. Destroying.");
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			Hediff_DeathRefusal firstHediff = this.InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null)
			{
				firstHediff.TickRare();
				if (base.Destroyed)
				{
					return;
				}
			}
			if (ModsConfig.AnomalyActive && this.InnerPawn.kindDef.IsFleshBeast() && this.GetRotStage() == RotStage.Dessicated)
			{
				FilthMaker.TryMakeFilth(base.PositionHeld, base.MapHeld, ThingDefOf.Filth_TwistedFlesh, 1, FilthSourceFlags.None, true);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			if (this.ShouldVanish)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x00125A64 File Offset: 0x00123C64
		protected void TickRareInt()
		{
			if (base.AllComps != null)
			{
				int i = 0;
				int count = base.AllComps.Count;
				while (i < count)
				{
					base.AllComps[i].CompTickRare();
					i++;
				}
			}
			if (base.Destroyed)
			{
				return;
			}
			this.InnerPawn.TickRare();
			GasUtility.CorpseGasEffectsTickRare(this);
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x00125ABC File Offset: 0x00123CBC
		protected override void IngestedCalculateAmounts(Pawn ingester, float nutritionWanted, out int numTaken, out float nutritionIngested)
		{
			BodyPartRecord bodyPartRecord = this.GetBestBodyPartToEat(nutritionWanted);
			if (bodyPartRecord == null)
			{
				Log.Error(((ingester != null) ? ingester.ToString() : null) + " ate " + ((this != null) ? this.ToString() : null) + " but no body part was found. Replacing with core part.");
				bodyPartRecord = this.InnerPawn.RaceProps.body.corePart;
			}
			float bodyPartNutrition = FoodUtility.GetBodyPartNutrition(this, bodyPartRecord);
			if (bodyPartRecord == this.InnerPawn.RaceProps.body.corePart)
			{
				if (ingester != null && PawnUtility.ShouldSendNotificationAbout(this.InnerPawn) && this.InnerPawn.RaceProps.Humanlike)
				{
					Messages.Message("MessageEatenByPredator".Translate(this.InnerPawn.LabelShort, ingester.Named("PREDATOR"), this.InnerPawn.Named("EATEN")).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeEvent, true);
				}
				numTaken = 1;
			}
			else
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.InnerPawn, bodyPartRecord);
				if (ingester != null)
				{
					hediff_MissingPart.lastInjury = HediffDefOf.Bite;
				}
				hediff_MissingPart.IsFresh = true;
				this.InnerPawn.health.AddHediff(hediff_MissingPart, null, null, null);
				numTaken = 0;
			}
			nutritionIngested = bodyPartNutrition;
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x00125C02 File Offset: 0x00123E02
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			foreach (Thing thing in this.InnerPawn.ButcherProducts(butcher, efficiency))
			{
				yield return thing;
			}
			IEnumerator<Thing> enumerator = null;
			if (this.InnerPawn.RaceProps.BloodDef != null)
			{
				FilthMaker.TryMakeFilth(butcher.Position, butcher.Map, this.InnerPawn.RaceProps.BloodDef, this.InnerPawn.LabelIndefinite(), 1, FilthSourceFlags.None);
			}
			if (this.InnerPawn.RaceProps.Humanlike)
			{
				Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.ButcheredHuman, new SignalArgs(butcher.Named(HistoryEventArgsNames.Doer), this.InnerPawn.Named(HistoryEventArgsNames.Victim))), true);
				TaleRecorder.RecordTale(TaleDefOf.ButcheredHumanlikeCorpse, new object[]
				{
					butcher
				});
			}
			yield break;
			yield break;
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x00125C20 File Offset: 0x00123E20
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.timeOfDeath, "timeOfDeath", 0, false);
			Scribe_Values.Look<int>(ref this.vanishAfterTimestamp, "vanishAfterTimestamp", 0, false);
			Scribe_Values.Look<bool>(ref this.everBuriedInSarcophagus, "everBuriedInSarcophagus", false, false);
			Scribe_Deep.Look<BillStack>(ref this.operationsBillStack, "operationsBillStack", new object[]
			{
				this
			});
			Scribe_Deep.Look<ThingOwner<Pawn>>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.innerContainer.removeContentsIfDestroyed)
			{
				this.innerContainer.removeContentsIfDestroyed = false;
			}
		}

		// Token: 0x060036BB RID: 14011 RVA: 0x00125CBE File Offset: 0x00123EBE
		public void Strip(bool notifyFaction = true)
		{
			this.InnerPawn.Strip(notifyFaction);
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x00125CCC File Offset: 0x00123ECC
		public override void DynamicDrawPhaseAt(DrawPhase phase, Vector3 drawLoc, bool flip = false)
		{
			this.InnerPawn.DynamicDrawPhaseAt(phase, drawLoc.WithYOffset(this.InnerPawn.Drawer.SeededYOffset), false);
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x00002C42 File Offset: 0x00000E42
		public Thought_Memory GiveObservedThought(Pawn observer)
		{
			return null;
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x00125CF4 File Offset: 0x00123EF4
		public HistoryEventDef GiveObservedHistoryEvent(Pawn observer)
		{
			if (!this.InnerPawn.RaceProps.Humanlike)
			{
				return null;
			}
			if (this.InnerPawn.health.killedByRitual && Find.TickManager.TicksGame - this.timeOfDeath < 60000)
			{
				return null;
			}
			if (this.StoringThing() != null)
			{
				return null;
			}
			if (this.IsNotFresh())
			{
				return HistoryEventDefOf.ObservedLayingRottingCorpse;
			}
			return HistoryEventDefOf.ObservedLayingCorpse;
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x00125D60 File Offset: 0x00123F60
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.InnerPawn.Faction != null && !this.InnerPawn.Faction.Hidden)
			{
				stringBuilder.AppendLineTagged("Faction".Translate() + ": " + this.InnerPawn.Faction.NameColored);
			}
			stringBuilder.AppendLine("DeadTime".Translate(this.Age.ToStringTicksToPeriodVague(true, false)));
			float num = 1f - this.InnerPawn.health.hediffSet.GetCoverageOfNotMissingNaturalParts(this.InnerPawn.RaceProps.body.corePart);
			if (num >= 0.01f)
			{
				stringBuilder.AppendLine("CorpsePercentMissing".Translate() + ": " + num.ToStringPercent());
			}
			Hediff_DeathRefusal firstHediff = this.InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			if (firstHediff != null && firstHediff.InProgress)
			{
				stringBuilder.AppendLine("SelfResurrecting".Translate());
			}
			stringBuilder.AppendLine(base.GetInspectString());
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x00125E9D File Offset: 0x0012409D
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BodySize".Translate(), this.InnerPawn.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 4195, null, null, false, false);
			if (this.GetRotStage() == RotStage.Fresh)
			{
				StatDef meatAmount = StatDefOf.MeatAmount;
				yield return new StatDrawEntry(meatAmount.category, meatAmount, this.InnerPawn.GetStatValue(meatAmount, true, -1), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
				StatDef leatherAmount = StatDefOf.LeatherAmount;
				yield return new StatDrawEntry(leatherAmount.category, leatherAmount, this.InnerPawn.GetStatValue(leatherAmount, true, -1), StatRequest.For(this.InnerPawn), ToStringNumberSense.Undefined, null, false);
			}
			yield break;
			yield break;
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00125EAD File Offset: 0x001240AD
		public void RotStageChanged()
		{
			this.InnerPawn.Drawer.renderer.SetAllGraphicsDirty();
			this.InnerPawn.Drawer.renderer.WoundOverlays.ClearCache();
			this.NotifyColonistBar();
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00125EE4 File Offset: 0x001240E4
		public BodyPartRecord GetBestBodyPartToEat(float nutritionWanted)
		{
			IEnumerable<BodyPartRecord> source = from x in this.InnerPawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where x.depth == BodyPartDepth.Outside && FoodUtility.GetBodyPartNutrition(this, x) > 0.001f
			select x;
			if (!source.Any<BodyPartRecord>())
			{
				return null;
			}
			return source.MinBy((BodyPartRecord x) => Mathf.Abs(FoodUtility.GetBodyPartNutrition(this, x) - nutritionWanted));
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x00125F4C File Offset: 0x0012414C
		private void NotifyColonistBar()
		{
			if (this.InnerPawn.Faction == Faction.OfPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x060036C4 RID: 14020 RVA: 0x000026EA File Offset: 0x000008EA
		public void Notify_BillDeleted(Bill bill)
		{
		}

		// Token: 0x060036C5 RID: 14021 RVA: 0x00125F72 File Offset: 0x00124172
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.InnerPawn.HasShowGizmosOnCorpseHediff)
			{
				foreach (Gizmo gizmo2 in this.InnerPawn.GetGizmos())
				{
					yield return gizmo2;
				}
				enumerator = null;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Resurrect",
					action = delegate()
					{
						ResurrectionUtility.TryResurrect(this.InnerPawn, null);
					}
				};
				if (ModsConfig.AnomalyActive)
				{
					yield return new Command_Action
					{
						defaultLabel = "DEV: Resurrect as shambler",
						action = delegate()
						{
							MutantUtility.ResurrectAsShambler(this.InnerPawn, -1, null);
						}
					};
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x04002902 RID: 10498
		protected ThingOwner<Pawn> innerContainer;

		// Token: 0x04002903 RID: 10499
		public int timeOfDeath = -1;

		// Token: 0x04002904 RID: 10500
		private int vanishAfterTimestamp = -1;

		// Token: 0x04002905 RID: 10501
		private BillStack operationsBillStack;

		// Token: 0x04002906 RID: 10502
		public bool everBuriedInSarcophagus;

		// Token: 0x04002907 RID: 10503
		[Unsaved(false)]
		private string cachedLabel;

		// Token: 0x04002908 RID: 10504
		private const int DontCauseObservedCorpseThoughtAfterRitualExecutionTicks = 60000;

		// Token: 0x04002909 RID: 10505
		private static readonly IntRange ExplodeFilthCountRange = new IntRange(2, 5);
	}
}
