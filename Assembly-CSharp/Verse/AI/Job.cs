using System;
using System.Collections.Generic;
using RimWorld;
using RimWorld.Planet;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000E78 RID: 3704
	public class Job : IExposable, ILoadReferenceable
	{
		// Token: 0x17000EF9 RID: 3833
		// (get) Token: 0x06005D41 RID: 23873 RVA: 0x001DB880 File Offset: 0x001D9A80
		public RecipeDef RecipeDef
		{
			get
			{
				Bill bill = this.bill;
				if (bill == null)
				{
					return null;
				}
				return bill.recipe;
			}
		}

		// Token: 0x17000EFA RID: 3834
		// (get) Token: 0x06005D42 RID: 23874 RVA: 0x001DB893 File Offset: 0x001D9A93
		public JobDriver GetCachedDriverDirect
		{
			get
			{
				return this.cachedDriver;
			}
		}

		// Token: 0x06005D43 RID: 23875 RVA: 0x001DB89C File Offset: 0x001D9A9C
		public void Clear()
		{
			this.def = null;
			this.targetA = LocalTargetInfo.Invalid;
			this.targetB = LocalTargetInfo.Invalid;
			this.targetC = LocalTargetInfo.Invalid;
			this.targetQueueA = null;
			this.targetQueueB = null;
			this.count = -1;
			this.countQueue = null;
			this.loadID = -1;
			this.startTick = -1;
			this.expiryInterval = -1;
			this.checkOverrideOnExpire = false;
			this.playerForced = false;
			this.playerInterruptedForced = false;
			this.placedThings = null;
			this.maxNumMeleeAttacks = int.MaxValue;
			this.maxNumStaticAttacks = int.MaxValue;
			this.locomotionUrgency = LocomotionUrgency.Jog;
			this.haulMode = HaulMode.Undefined;
			this.bill = null;
			this.commTarget = null;
			this.plantDefToSow = null;
			this.verbToUse = null;
			this.haulOpportunisticDuplicates = false;
			this.exitMapOnArrival = false;
			this.failIfCantJoinOrCreateCaravan = false;
			this.killIncappedTarget = false;
			this.ignoreForbidden = false;
			this.ignoreDesignations = false;
			this.canBashDoors = false;
			this.canBashFences = false;
			this.canUseRangedWeapon = true;
			this.haulDroppedApparel = false;
			this.restUntilHealed = false;
			this.ignoreJoyTimeAssignment = false;
			this.doUntilGatheringEnded = false;
			this.overeat = false;
			this.ingestTotalCount = false;
			this.attackDoorIfTargetLost = false;
			this.takeExtraIngestibles = 0;
			this.expireRequiresEnemiesNearby = false;
			this.lord = null;
			this.collideWithPawns = false;
			this.forceSleep = false;
			this.interaction = null;
			this.endIfCantShootTargetFromCurPos = false;
			this.endIfCantShootInMelee = false;
			this.checkEncumbrance = false;
			this.followRadius = 0f;
			this.endAfterTendedOnce = false;
			this.quest = null;
			this.mote = null;
			this.reactingToMeleeThreat = false;
			this.wasOnMeditationTimeAssignment = false;
			this.psyfocusTargetLast = -1f;
			this.preventFriendlyFire = false;
			this.ropingPriority = RopingPriority.Closest;
			this.ropeToUnenclosedPens = false;
			this.thingDefToCarry = null;
			this.dutyTag = null;
			this.ritualTag = null;
			this.controlGroupTag = null;
			this.lookDirection = Direction8Way.Invalid;
			this.overrideFacing = Rot4.Invalid;
			this.forceMaintainFacing = false;
			this.takeInventoryDelay = 0;
			this.draftedTend = false;
			this.showSpeechBubbles = true;
			this.speechSoundFemale = null;
			this.speechSoundMale = null;
			this.speechFaceSpectatorsIfPossible = false;
			this.biosculpterCycleKey = null;
			this.startInvoluntarySleep = false;
			this.reportStringOverride = null;
			this.crawlingReportStringOverride = null;
			this.isLearningDesire = false;
			this.flying = false;
			this.swimming = false;
			this.intervalScalingTarget = TargetIndex.None;
			this.jobGiverThinkTree = null;
			this.jobGiver = null;
			this.workGiverDef = null;
			this.ability = null;
			this.source = null;
			if (this.cachedDriver != null)
			{
				this.cachedDriver.job = null;
			}
			this.cachedDriver = null;
			if (this.lastJobDriverMade != null)
			{
				this.lastJobDriverMade.job = null;
			}
			this.lastJobDriverMade = null;
		}

		// Token: 0x06005D44 RID: 23876 RVA: 0x001DBB4C File Offset: 0x001D9D4C
		public Job()
		{
		}

		// Token: 0x06005D45 RID: 23877 RVA: 0x001DBC04 File Offset: 0x001D9E04
		public Job(JobDef def) : this(def, null)
		{
		}

		// Token: 0x06005D46 RID: 23878 RVA: 0x001DBC13 File Offset: 0x001D9E13
		public Job(JobDef def, LocalTargetInfo targetA) : this(def, targetA, null)
		{
		}

		// Token: 0x06005D47 RID: 23879 RVA: 0x001DBC24 File Offset: 0x001D9E24
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06005D48 RID: 23880 RVA: 0x001DBD04 File Offset: 0x001D9F04
		public Job(JobDef def, LocalTargetInfo targetA, LocalTargetInfo targetB, LocalTargetInfo targetC)
		{
			this.def = def;
			this.targetA = targetA;
			this.targetB = targetB;
			this.targetC = targetC;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06005D49 RID: 23881 RVA: 0x001DBDEC File Offset: 0x001D9FEC
		public Job(JobDef def, LocalTargetInfo targetA, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.targetA = targetA;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06005D4A RID: 23882 RVA: 0x001DBED4 File Offset: 0x001DA0D4
		public Job(JobDef def, int expiryInterval, bool checkOverrideOnExpiry = false)
		{
			this.def = def;
			this.expiryInterval = expiryInterval;
			this.checkOverrideOnExpire = checkOverrideOnExpiry;
			this.loadID = Find.UniqueIDsManager.GetNextJobID();
		}

		// Token: 0x06005D4B RID: 23883 RVA: 0x001DBFB4 File Offset: 0x001DA1B4
		public Job Clone()
		{
			return new Job
			{
				def = this.def,
				targetA = this.targetA,
				targetB = this.targetB,
				targetC = this.targetC,
				targetQueueA = this.targetQueueA,
				targetQueueB = this.targetQueueB,
				globalTarget = this.globalTarget,
				count = this.count,
				countQueue = this.countQueue,
				loadID = this.loadID,
				expiryInterval = this.expiryInterval,
				checkOverrideOnExpire = this.checkOverrideOnExpire,
				playerForced = this.playerForced,
				showCarryingInspectLine = this.showCarryingInspectLine,
				placedThings = this.placedThings,
				maxNumMeleeAttacks = this.maxNumMeleeAttacks,
				maxNumStaticAttacks = this.maxNumStaticAttacks,
				locomotionUrgency = this.locomotionUrgency,
				haulMode = this.haulMode,
				bill = this.bill,
				commTarget = this.commTarget,
				plantDefToSow = this.plantDefToSow,
				thingDefToCarry = this.thingDefToCarry,
				verbToUse = this.verbToUse,
				haulOpportunisticDuplicates = this.haulOpportunisticDuplicates,
				exitMapOnArrival = this.exitMapOnArrival,
				failIfCantJoinOrCreateCaravan = this.failIfCantJoinOrCreateCaravan,
				killIncappedTarget = this.killIncappedTarget,
				ignoreForbidden = this.ignoreForbidden,
				ignoreDesignations = this.ignoreDesignations,
				canBashDoors = this.canBashDoors,
				canBashFences = this.canBashFences,
				canUseRangedWeapon = this.canUseRangedWeapon,
				haulDroppedApparel = this.haulDroppedApparel,
				restUntilHealed = this.restUntilHealed,
				ignoreJoyTimeAssignment = this.ignoreJoyTimeAssignment,
				doUntilGatheringEnded = this.doUntilGatheringEnded,
				overeat = this.overeat,
				ingestTotalCount = this.ingestTotalCount,
				attackDoorIfTargetLost = this.attackDoorIfTargetLost,
				takeExtraIngestibles = this.takeExtraIngestibles,
				expireRequiresEnemiesNearby = this.expireRequiresEnemiesNearby,
				expireOnEnemiesNearby = this.expireOnEnemiesNearby,
				intervalScalingTarget = this.intervalScalingTarget,
				lord = this.lord,
				collideWithPawns = this.collideWithPawns,
				forceSleep = this.forceSleep,
				interaction = this.interaction,
				endIfCantShootTargetFromCurPos = this.endIfCantShootTargetFromCurPos,
				endIfCantShootInMelee = this.endIfCantShootInMelee,
				checkEncumbrance = this.checkEncumbrance,
				followRadius = this.followRadius,
				endAfterTendedOnce = this.endAfterTendedOnce,
				quest = this.quest,
				mote = this.mote,
				psyfocusTargetLast = this.psyfocusTargetLast,
				wasOnMeditationTimeAssignment = this.wasOnMeditationTimeAssignment,
				reactingToMeleeThreat = this.reactingToMeleeThreat,
				preventFriendlyFire = this.preventFriendlyFire,
				ropingPriority = this.ropingPriority,
				ropeToUnenclosedPens = this.ropeToUnenclosedPens,
				showSpeechBubbles = this.showSpeechBubbles,
				lookDirection = this.lookDirection,
				overrideFacing = this.overrideFacing,
				forceMaintainFacing = this.forceMaintainFacing,
				dutyTag = this.dutyTag,
				ritualTag = this.ritualTag,
				controlGroupTag = this.controlGroupTag,
				takeInventoryDelay = this.takeInventoryDelay,
				draftedTend = this.draftedTend,
				speechFaceSpectatorsIfPossible = this.speechFaceSpectatorsIfPossible,
				speechSoundMale = this.speechSoundMale,
				speechSoundFemale = this.speechSoundFemale,
				biosculpterCycleKey = this.biosculpterCycleKey,
				reportStringOverride = this.reportStringOverride,
				crawlingReportStringOverride = this.crawlingReportStringOverride,
				startInvoluntarySleep = this.startInvoluntarySleep,
				isLearningDesire = this.isLearningDesire,
				flying = this.flying,
				swimming = this.swimming,
				jobGiverThinkTree = this.jobGiverThinkTree,
				jobGiver = this.jobGiver,
				workGiverDef = this.workGiverDef,
				ability = this.ability,
				source = this.source
			};
		}

		// Token: 0x06005D4C RID: 23884 RVA: 0x001DC3C2 File Offset: 0x001DA5C2
		public LocalTargetInfo GetTarget(TargetIndex ind)
		{
			switch (ind)
			{
			case TargetIndex.A:
				return this.targetA;
			case TargetIndex.B:
				return this.targetB;
			case TargetIndex.C:
				return this.targetC;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06005D4D RID: 23885 RVA: 0x001DC3F4 File Offset: 0x001DA5F4
		public List<LocalTargetInfo> GetTargetQueue(TargetIndex ind)
		{
			if (ind == TargetIndex.A)
			{
				if (this.targetQueueA == null)
				{
					this.targetQueueA = new List<LocalTargetInfo>();
				}
				return this.targetQueueA;
			}
			if (ind != TargetIndex.B)
			{
				throw new ArgumentException();
			}
			if (this.targetQueueB == null)
			{
				this.targetQueueB = new List<LocalTargetInfo>();
			}
			return this.targetQueueB;
		}

		// Token: 0x06005D4E RID: 23886 RVA: 0x001DC444 File Offset: 0x001DA644
		public void SetTarget(TargetIndex ind, LocalTargetInfo pack)
		{
			switch (ind)
			{
			case TargetIndex.A:
				this.targetA = pack;
				return;
			case TargetIndex.B:
				this.targetB = pack;
				return;
			case TargetIndex.C:
				this.targetC = pack;
				return;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x06005D4F RID: 23887 RVA: 0x001DC479 File Offset: 0x001DA679
		public void AddQueuedTarget(TargetIndex ind, LocalTargetInfo target)
		{
			this.GetTargetQueue(ind).Add(target);
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x001DC488 File Offset: 0x001DA688
		public void ExposeData()
		{
			ILoadReferenceable loadReferenceable = (ILoadReferenceable)this.commTarget;
			Scribe_References.Look<ILoadReferenceable>(ref loadReferenceable, "commTarget", false);
			this.commTarget = (ICommunicable)loadReferenceable;
			Scribe_References.Look<Verb>(ref this.verbToUse, "verbToUse", false);
			Scribe_References.Look<Bill>(ref this.bill, "bill", false);
			Scribe_References.Look<Lord>(ref this.lord, "lord", false);
			Scribe_References.Look<Quest>(ref this.quest, "quest", false);
			Scribe_Defs.Look<JobDef>(ref this.def, "def");
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_TargetInfo.Look(ref this.targetA, "targetA");
			Scribe_TargetInfo.Look(ref this.targetB, "targetB");
			Scribe_TargetInfo.Look(ref this.targetC, "targetC");
			Scribe_TargetInfo.Look(ref this.globalTarget, "globalTarget");
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueA, "targetQueueA", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<LocalTargetInfo>(ref this.targetQueueB, "targetQueueB", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.count, "count", -1, false);
			Scribe_Collections.Look<int>(ref this.countQueue, "countQueue", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.ignoreForbidden, "ignoreForbidden", false, false);
			Scribe_Values.Look<int>(ref this.startTick, "startTick", -1, false);
			Scribe_Values.Look<int>(ref this.expiryInterval, "expiryInterval", -1, false);
			Scribe_Values.Look<bool>(ref this.checkOverrideOnExpire, "checkOverrideOnExpire", false, false);
			Scribe_Values.Look<bool>(ref this.playerForced, "playerForced", false, false);
			Scribe_Values.Look<bool>(ref this.playerInterruptedForced, "playerInterruptedForced", false, false);
			Scribe_Values.Look<TargetIndex>(ref this.intervalScalingTarget, "intervalScalingTarget", TargetIndex.None, false);
			Scribe_Collections.Look<ThingCountClass>(ref this.placedThings, "placedThings", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.maxNumMeleeAttacks, "maxNumMeleeAttacks", int.MaxValue, false);
			Scribe_Values.Look<int>(ref this.maxNumStaticAttacks, "maxNumStaticAttacks", int.MaxValue, false);
			Scribe_Values.Look<bool>(ref this.exitMapOnArrival, "exitMapOnArrival", false, false);
			Scribe_Values.Look<bool>(ref this.failIfCantJoinOrCreateCaravan, "failIfCantJoinOrCreateCaravan", false, false);
			Scribe_Values.Look<bool>(ref this.killIncappedTarget, "killIncappedTarget", false, false);
			Scribe_Values.Look<bool>(ref this.haulOpportunisticDuplicates, "haulOpportunisticDuplicates", false, false);
			Scribe_Values.Look<HaulMode>(ref this.haulMode, "haulMode", HaulMode.Undefined, false);
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToSow, "plantDefToSow");
			Scribe_Defs.Look<ThingDef>(ref this.thingDefToCarry, "thingDefToCarry");
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotionUrgency, "locomotionUrgency", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.ignoreDesignations, "ignoreDesignations", false, false);
			Scribe_Values.Look<bool>(ref this.canBashDoors, "canBash", false, false);
			Scribe_Values.Look<bool>(ref this.canBashFences, "canBashFences", false, false);
			Scribe_Values.Look<bool>(ref this.canUseRangedWeapon, "canUseRangedWeapon", true, false);
			Scribe_Values.Look<bool>(ref this.haulDroppedApparel, "haulDroppedApparel", false, false);
			Scribe_Values.Look<bool>(ref this.restUntilHealed, "restUntilHealed", false, false);
			Scribe_Values.Look<bool>(ref this.ignoreJoyTimeAssignment, "ignoreJoyTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.overeat, "overeat", false, false);
			Scribe_Values.Look<bool>(ref this.ingestTotalCount, "ingestTotalCount", false, false);
			Scribe_Values.Look<bool>(ref this.attackDoorIfTargetLost, "attackDoorIfTargetLost", false, false);
			Scribe_Values.Look<int>(ref this.takeExtraIngestibles, "takeExtraIngestibles", 0, false);
			Scribe_Values.Look<bool>(ref this.expireRequiresEnemiesNearby, "expireRequiresEnemiesNearby", false, false);
			Scribe_Values.Look<bool>(ref this.collideWithPawns, "collideWithPawns", false, false);
			Scribe_Values.Look<bool>(ref this.forceSleep, "forceSleep", false, false);
			Scribe_Defs.Look<InteractionDef>(ref this.interaction, "interaction");
			Scribe_Values.Look<bool>(ref this.endIfCantShootTargetFromCurPos, "endIfCantShootTargetFromCurPos", false, false);
			Scribe_Values.Look<bool>(ref this.endIfCantShootInMelee, "endIfCantShootInMelee", false, false);
			Scribe_Values.Look<bool>(ref this.checkEncumbrance, "checkEncumbrance", false, false);
			Scribe_Values.Look<float>(ref this.followRadius, "followRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.endAfterTendedOnce, "endAfterTendedOnce", false, false);
			Scribe_Defs.Look<WorkGiverDef>(ref this.workGiverDef, "workGiverDef");
			Scribe_Defs.Look<ThinkTreeDef>(ref this.jobGiverThinkTree, "jobGiverThinkTree");
			Scribe_Values.Look<bool>(ref this.doUntilGatheringEnded, "doUntilGatheringEnded", false, false);
			Scribe_Values.Look<float>(ref this.psyfocusTargetLast, "psyfocusTargetLast", 0f, false);
			Scribe_Values.Look<bool>(ref this.wasOnMeditationTimeAssignment, "wasOnMeditationTimeAssignment", false, false);
			Scribe_Values.Look<bool>(ref this.reactingToMeleeThreat, "reactingToMeleeThreat", false, false);
			Scribe_Values.Look<bool>(ref this.preventFriendlyFire, "preventFriendlyFire", false, false);
			Scribe_Values.Look<RopingPriority>(ref this.ropingPriority, "ropingPriority", RopingPriority.Closest, false);
			Scribe_Values.Look<bool>(ref this.ropeToUnenclosedPens, "ropeToUnenclosedPens", false, false);
			Scribe_Values.Look<Direction8Way>(ref this.lookDirection, "lookDirection", Direction8Way.Invalid, false);
			Scribe_Values.Look<string>(ref this.dutyTag, "dutyTag", null, false);
			Scribe_Values.Look<string>(ref this.ritualTag, "ritualTag", null, false);
			Scribe_Values.Look<string>(ref this.controlGroupTag, "controlGroupTag", null, false);
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
			Scribe_References.Look<ILoadReferenceable>(ref this.source, "source", false);
			Scribe_Values.Look<int>(ref this.takeInventoryDelay, "takeInventoryDelay", 0, false);
			Scribe_Values.Look<bool>(ref this.draftedTend, "draftedTend", false, false);
			Scribe_Values.Look<bool>(ref this.showSpeechBubbles, "showSpeechBubbles", true, false);
			Scribe_Values.Look<Rot4>(ref this.overrideFacing, "overrideFacing", Rot4.Invalid, false);
			Scribe_Values.Look<bool>(ref this.forceMaintainFacing, "forceMaintainFacing", false, false);
			Scribe_Values.Look<bool>(ref this.speechFaceSpectatorsIfPossible, "speechFaceSpectatorsIfPossible", false, false);
			Scribe_Defs.Look<SoundDef>(ref this.speechSoundMale, "speechSoundMale");
			Scribe_Defs.Look<SoundDef>(ref this.speechSoundFemale, "speechSoundFemale");
			Scribe_Values.Look<string>(ref this.biosculpterCycleKey, "biosculpterCycleKey", null, false);
			Scribe_Values.Look<bool>(ref this.startInvoluntarySleep, "startInvoluntarySleep", false, false);
			Scribe_Values.Look<string>(ref this.reportStringOverride, "reportStringOverride", null, false);
			Scribe_Values.Look<string>(ref this.crawlingReportStringOverride, "crawlingReportStringOverride", null, false);
			Scribe_Values.Look<bool>(ref this.isLearningDesire, "isLearning", false, false);
			Scribe_Values.Look<int>(ref this.interactableIndex, "interactableIndex", 0, false);
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				ThinkNode thinkNode = this.jobGiver;
				this.jobGiverKey = ((thinkNode != null) ? thinkNode.UniqueSaveKey : -1);
			}
			Scribe_Values.Look<int>(ref this.jobGiverKey, "lastJobGiverKey", -1, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.jobGiverKey != -1 && !this.jobGiverThinkTree.TryGetThinkNodeWithSaveKey(this.jobGiverKey, out this.jobGiver))
				{
					Log.Warning("Could not find think node with key " + this.jobGiverKey.ToString());
				}
				if (this.verbToUse != null && this.verbToUse.BuggedAfterLoading)
				{
					this.verbToUse = null;
					Type type = base.GetType();
					Log.Warning(((type != null) ? type.ToString() : null) + " had a bugged verbToUse after loading.");
				}
			}
		}

		// Token: 0x06005D51 RID: 23889 RVA: 0x001DCB28 File Offset: 0x001DAD28
		public JobDriver MakeDriver(Pawn driverPawn)
		{
			JobDriver jobDriver = (JobDriver)Activator.CreateInstance(this.def.driverClass);
			jobDriver.pawn = driverPawn;
			jobDriver.job = this;
			this.lastJobDriverMade = jobDriver;
			return jobDriver;
		}

		// Token: 0x06005D52 RID: 23890 RVA: 0x001DCB64 File Offset: 0x001DAD64
		public JobDriver GetCachedDriver(Pawn driverPawn)
		{
			if (this.cachedDriver == null)
			{
				this.cachedDriver = this.MakeDriver(driverPawn);
			}
			if (this.cachedDriver.pawn != driverPawn)
			{
				Log.Error(string.Concat(new string[]
				{
					"Tried to use the same driver for 2 pawns: ",
					this.cachedDriver.ToStringSafe<JobDriver>(),
					", first pawn= ",
					this.cachedDriver.pawn.ToStringSafe<Pawn>(),
					", second pawn=",
					driverPawn.ToStringSafe<Pawn>()
				}));
			}
			return this.cachedDriver;
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x001DCBEC File Offset: 0x001DADEC
		public bool TryMakePreToilReservations(Pawn driverPawn, bool errorOnFailed)
		{
			return this.GetCachedDriver(driverPawn).TryMakePreToilReservations(errorOnFailed);
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x001DCBFB File Offset: 0x001DADFB
		public string GetReport(Pawn driverPawn)
		{
			return this.GetCachedDriver(driverPawn).GetReport();
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x001DCC09 File Offset: 0x001DAE09
		public LocalTargetInfo GetDestination(Pawn driverPawn)
		{
			return this.targetA;
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x001DCC11 File Offset: 0x001DAE11
		public bool CanBeginNow(Pawn pawn, bool whileLyingDown = false)
		{
			if (pawn.Downed)
			{
				whileLyingDown = true;
			}
			return !whileLyingDown || this.GetCachedDriver(pawn).CanBeginNowWhileLyingDown();
		}

		// Token: 0x06005D57 RID: 23895 RVA: 0x001DCC30 File Offset: 0x001DAE30
		public bool JobIsSameAs(Pawn pawn, Job other)
		{
			if (other == null)
			{
				return false;
			}
			if (this == other)
			{
				return true;
			}
			if (this.def != other.def || this.verbToUse != other.verbToUse || this.bill != other.bill)
			{
				return false;
			}
			bool? flag = this.GetCachedDriver(pawn).IsSameJobAs(other);
			if (flag != null)
			{
				return flag.Value;
			}
			return !(this.targetA != other.targetA) && !(this.targetB != other.targetB) && !(this.targetC != other.targetC) && this.commTarget == other.commTarget;
		}

		// Token: 0x06005D58 RID: 23896 RVA: 0x001DCCE0 File Offset: 0x001DAEE0
		public bool AnyTargetIs(LocalTargetInfo target)
		{
			return target.IsValid && (this.targetA == target || this.targetB == target || this.targetC == target || (this.targetQueueA != null && this.targetQueueA.Contains(target)) || (this.targetQueueB != null && this.targetQueueB.Contains(target)));
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x001DCD50 File Offset: 0x001DAF50
		public bool AnyTargetOutsideArea(Area zone)
		{
			if (Job.IsTargetOutsideArea(this.targetA, zone) || Job.IsTargetOutsideArea(this.targetB, zone) || Job.IsTargetOutsideArea(this.targetC, zone))
			{
				return true;
			}
			if (this.targetQueueA != null)
			{
				using (List<LocalTargetInfo>.Enumerator enumerator = this.targetQueueA.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Job.IsTargetOutsideArea(enumerator.Current, zone))
						{
							return true;
						}
					}
				}
			}
			if (this.targetQueueB != null)
			{
				using (List<LocalTargetInfo>.Enumerator enumerator = this.targetQueueB.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (Job.IsTargetOutsideArea(enumerator.Current, zone))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005D5A RID: 23898 RVA: 0x001DCE2C File Offset: 0x001DB02C
		private static bool IsTargetOutsideArea(LocalTargetInfo target, Area zone)
		{
			IntVec3 cell = target.Cell;
			return cell.IsValid && !zone[cell];
		}

		// Token: 0x06005D5B RID: 23899 RVA: 0x001DCE58 File Offset: 0x001DB058
		public override string ToString()
		{
			string text = string.Format("{0} ({1})", this.def, this.GetUniqueLoadID());
			if (this.targetA.IsValid)
			{
				text += string.Format(" A = {0}", this.targetA);
			}
			if (this.targetB.IsValid)
			{
				text += string.Format(" B = {0}", this.targetB);
			}
			if (this.targetC.IsValid)
			{
				text += string.Format(" C = {0}", this.targetC);
			}
			if (this.jobGiver != null)
			{
				text = string.Concat(new string[]
				{
					text,
					" Giver = ",
					this.jobGiver.GetType().Name,
					" [workGiverDef: ",
					(this.workGiverDef == null) ? "null" : this.workGiverDef.ToString(),
					"]"
				});
			}
			return text;
		}

		// Token: 0x06005D5C RID: 23900 RVA: 0x001DCF58 File Offset: 0x001DB158
		public string GetUniqueLoadID()
		{
			return "Job_" + this.loadID.ToString();
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x001DCF70 File Offset: 0x001DB170
		public void LogDetails(Pawn pawn)
		{
			string text = string.Format("{0}", this);
			JobDriver curDriver = pawn.jobs.curDriver;
			if (curDriver != null)
			{
				text += string.Format("\ndriver details - current toil [{0}]: {1}", curDriver.CurToilIndex, curDriver.CurToilString);
			}
			else
			{
				text += "\nno driver.";
			}
			Log.Message(text);
		}

		// Token: 0x04004353 RID: 17235
		public JobDef def;

		// Token: 0x04004354 RID: 17236
		public LocalTargetInfo targetA = LocalTargetInfo.Invalid;

		// Token: 0x04004355 RID: 17237
		public LocalTargetInfo targetB = LocalTargetInfo.Invalid;

		// Token: 0x04004356 RID: 17238
		public LocalTargetInfo targetC = LocalTargetInfo.Invalid;

		// Token: 0x04004357 RID: 17239
		public List<LocalTargetInfo> targetQueueA;

		// Token: 0x04004358 RID: 17240
		public List<LocalTargetInfo> targetQueueB;

		// Token: 0x04004359 RID: 17241
		public GlobalTargetInfo globalTarget = GlobalTargetInfo.Invalid;

		// Token: 0x0400435A RID: 17242
		public int count = -1;

		// Token: 0x0400435B RID: 17243
		public List<int> countQueue;

		// Token: 0x0400435C RID: 17244
		public int loadID = -1;

		// Token: 0x0400435D RID: 17245
		public int startTick = -1;

		// Token: 0x0400435E RID: 17246
		public int expiryInterval = -1;

		// Token: 0x0400435F RID: 17247
		public bool checkOverrideOnExpire;

		// Token: 0x04004360 RID: 17248
		public bool playerForced;

		// Token: 0x04004361 RID: 17249
		public bool playerInterruptedForced;

		// Token: 0x04004362 RID: 17250
		public bool showCarryingInspectLine = true;

		// Token: 0x04004363 RID: 17251
		public bool flying;

		// Token: 0x04004364 RID: 17252
		public bool swimming;

		// Token: 0x04004365 RID: 17253
		public TargetIndex intervalScalingTarget;

		// Token: 0x04004366 RID: 17254
		public List<ThingCountClass> placedThings;

		// Token: 0x04004367 RID: 17255
		public int maxNumMeleeAttacks = int.MaxValue;

		// Token: 0x04004368 RID: 17256
		public int maxNumStaticAttacks = int.MaxValue;

		// Token: 0x04004369 RID: 17257
		public LocomotionUrgency locomotionUrgency = LocomotionUrgency.Jog;

		// Token: 0x0400436A RID: 17258
		public HaulMode haulMode;

		// Token: 0x0400436B RID: 17259
		public Bill bill;

		// Token: 0x0400436C RID: 17260
		public ICommunicable commTarget;

		// Token: 0x0400436D RID: 17261
		public ThingDef plantDefToSow;

		// Token: 0x0400436E RID: 17262
		public ThingDef thingDefToCarry;

		// Token: 0x0400436F RID: 17263
		public Verb verbToUse;

		// Token: 0x04004370 RID: 17264
		public bool haulOpportunisticDuplicates;

		// Token: 0x04004371 RID: 17265
		public bool exitMapOnArrival;

		// Token: 0x04004372 RID: 17266
		public bool failIfCantJoinOrCreateCaravan;

		// Token: 0x04004373 RID: 17267
		public bool killIncappedTarget;

		// Token: 0x04004374 RID: 17268
		public bool ignoreForbidden;

		// Token: 0x04004375 RID: 17269
		public bool ignoreDesignations;

		// Token: 0x04004376 RID: 17270
		public bool canBashDoors;

		// Token: 0x04004377 RID: 17271
		public bool canBashFences;

		// Token: 0x04004378 RID: 17272
		public bool canUseRangedWeapon = true;

		// Token: 0x04004379 RID: 17273
		public bool haulDroppedApparel;

		// Token: 0x0400437A RID: 17274
		public bool restUntilHealed;

		// Token: 0x0400437B RID: 17275
		public bool ignoreJoyTimeAssignment;

		// Token: 0x0400437C RID: 17276
		public bool doUntilGatheringEnded;

		// Token: 0x0400437D RID: 17277
		public bool overeat;

		// Token: 0x0400437E RID: 17278
		public bool ingestTotalCount;

		// Token: 0x0400437F RID: 17279
		public bool attackDoorIfTargetLost;

		// Token: 0x04004380 RID: 17280
		public int takeExtraIngestibles;

		// Token: 0x04004381 RID: 17281
		public bool expireRequiresEnemiesNearby;

		// Token: 0x04004382 RID: 17282
		public bool expireOnEnemiesNearby;

		// Token: 0x04004383 RID: 17283
		public Lord lord;

		// Token: 0x04004384 RID: 17284
		public bool collideWithPawns;

		// Token: 0x04004385 RID: 17285
		public bool forceSleep;

		// Token: 0x04004386 RID: 17286
		public InteractionDef interaction;

		// Token: 0x04004387 RID: 17287
		public bool endIfCantShootTargetFromCurPos;

		// Token: 0x04004388 RID: 17288
		public bool endIfCantShootInMelee;

		// Token: 0x04004389 RID: 17289
		public bool checkEncumbrance;

		// Token: 0x0400438A RID: 17290
		public float followRadius;

		// Token: 0x0400438B RID: 17291
		public bool endAfterTendedOnce;

		// Token: 0x0400438C RID: 17292
		public Quest quest;

		// Token: 0x0400438D RID: 17293
		public Mote mote;

		// Token: 0x0400438E RID: 17294
		public float psyfocusTargetLast = -1f;

		// Token: 0x0400438F RID: 17295
		public bool wasOnMeditationTimeAssignment;

		// Token: 0x04004390 RID: 17296
		public bool reactingToMeleeThreat;

		// Token: 0x04004391 RID: 17297
		public bool preventFriendlyFire;

		// Token: 0x04004392 RID: 17298
		public RopingPriority ropingPriority;

		// Token: 0x04004393 RID: 17299
		public bool ropeToUnenclosedPens;

		// Token: 0x04004394 RID: 17300
		public bool showSpeechBubbles = true;

		// Token: 0x04004395 RID: 17301
		public Direction8Way lookDirection = Direction8Way.Invalid;

		// Token: 0x04004396 RID: 17302
		public Rot4 overrideFacing = Rot4.Invalid;

		// Token: 0x04004397 RID: 17303
		public bool forceMaintainFacing;

		// Token: 0x04004398 RID: 17304
		public string dutyTag;

		// Token: 0x04004399 RID: 17305
		public string ritualTag;

		// Token: 0x0400439A RID: 17306
		public string controlGroupTag;

		// Token: 0x0400439B RID: 17307
		public int takeInventoryDelay;

		// Token: 0x0400439C RID: 17308
		public bool draftedTend;

		// Token: 0x0400439D RID: 17309
		public bool speechFaceSpectatorsIfPossible;

		// Token: 0x0400439E RID: 17310
		public SoundDef speechSoundMale;

		// Token: 0x0400439F RID: 17311
		public SoundDef speechSoundFemale;

		// Token: 0x040043A0 RID: 17312
		public string biosculpterCycleKey;

		// Token: 0x040043A1 RID: 17313
		[MustTranslate]
		public string reportStringOverride;

		// Token: 0x040043A2 RID: 17314
		[MustTranslate]
		public string crawlingReportStringOverride;

		// Token: 0x040043A3 RID: 17315
		public bool startInvoluntarySleep;

		// Token: 0x040043A4 RID: 17316
		public bool isLearningDesire;

		// Token: 0x040043A5 RID: 17317
		public int interactableIndex = -1;

		// Token: 0x040043A6 RID: 17318
		public ThinkTreeDef jobGiverThinkTree;

		// Token: 0x040043A7 RID: 17319
		public ThinkNode jobGiver;

		// Token: 0x040043A8 RID: 17320
		public WorkGiverDef workGiverDef;

		// Token: 0x040043A9 RID: 17321
		public Ability ability;

		// Token: 0x040043AA RID: 17322
		public ILoadReferenceable source;

		// Token: 0x040043AB RID: 17323
		private JobDriver cachedDriver;

		// Token: 0x040043AC RID: 17324
		private JobDriver lastJobDriverMade;

		// Token: 0x040043AD RID: 17325
		private int jobGiverKey = -1;
	}
}
