using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001659 RID: 5721
	public class LordJob_HateChant : LordJob
	{
		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x060086E3 RID: 34531 RVA: 0x0028468C File Offset: 0x0028288C
		public override bool ShouldExistWithoutPawns
		{
			get
			{
				return this.spawningFinished = false;
			}
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x060086E4 RID: 34532 RVA: 0x002846A4 File Offset: 0x002828A4
		private int CallInterval
		{
			get
			{
				if (!this.smallGroup)
				{
					return LordJob_HateChant.CallIntervalSeconds.RandomInRange.SecondsToTicks();
				}
				return LordJob_HateChant.CallIntervalSecondsSmallGroup.RandomInRange.SecondsToTicks();
			}
		}

		// Token: 0x060086E6 RID: 34534 RVA: 0x00284720 File Offset: 0x00282920
		public override StateGraph CreateGraph()
		{
			List<Pawn> list = new List<Pawn>();
			if (!this.spawningStarted)
			{
				list.AddRange(this.lord.ownedPawns);
				for (int i = 0; i < list.Count; i++)
				{
					this.lord.RemovePawn(list[i]);
				}
				this.spawningStarted = true;
			}
			this.assaultLossThreshold = Mathf.Min(Mathf.CeilToInt(LordJob_HateChant.AssaultLossPercent.RandomInRange * (float)(this.lord.ownedPawns.Count + list.Count)), 10);
			StateGraph stateGraph = new StateGraph();
			LordToil_SpawnHateChantersDelayed lordToil_SpawnHateChantersDelayed = new LordToil_SpawnHateChantersDelayed(list, "PositionsReady", 5, 0.8f);
			stateGraph.AddToil(lordToil_SpawnHateChantersDelayed);
			LordToil_PsychicRitualParticipantGoto traversalToil = new LordToil_PsychicRitualParticipantGoto(10800);
			stateGraph.AddToil(traversalToil);
			Transition transition = new Transition(lordToil_SpawnHateChantersDelayed, traversalToil, false, true);
			transition.AddTrigger(new Trigger_Signal("PositionsReady"));
			transition.AddPreAction(new TransitionAction_Custom(delegate()
			{
				this.participants = this.GenerateParticipants().ToList<PsychicRitualParticipant>();
				traversalToil.SetParticipants(this.participants);
				this.spawningFinished = true;
			}));
			stateGraph.AddTransition(transition, false);
			LordToil_HateChant chantToil = new LordToil_HateChant();
			stateGraph.AddToil(chantToil);
			Transition transition2 = new Transition(traversalToil, chantToil, false, true);
			transition2.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && (traversalToil.AllPawnsArrived || traversalToil.TimedOut)));
			transition2.AddPreAction(new TransitionAction_Custom(delegate()
			{
				chantToil.SetParticipants(this.participants);
			}));
			stateGraph.AddTransition(transition2, false);
			LordToil_AssaultColony lordToil_AssaultColony = new LordToil_AssaultColony(false, false);
			stateGraph.AddToil(lordToil_AssaultColony);
			Transition transition3 = new Transition(chantToil, lordToil_AssaultColony, false, true);
			transition3.AddTrigger(new Trigger_Custom((TriggerSignal signal) => signal.type == TriggerSignalType.Tick && this.lostPawns >= this.assaultLossThreshold));
			transition3.AddPostAction(new TransitionAction_Custom(delegate()
			{
				foreach (Sustainer sustainer in this.sustainers)
				{
					sustainer.End();
				}
				this.sustainers.Clear();
				Find.LetterStack.ReceiveLetter("HateChantersAttackingLabel".Translate(), "HateChantersAttackingText".Translate(), LetterDefOf.ThreatBig, null, 0, true);
				this.EnableSelfResurrect();
				this.MakePawnsWait();
			}));
			stateGraph.AddTransition(transition3, false);
			return stateGraph;
		}

		// Token: 0x060086E7 RID: 34535 RVA: 0x002848FC File Offset: 0x00282AFC
		public override void Notify_PawnLost(Pawn p, PawnLostCondition condition)
		{
			base.Notify_PawnLost(p, condition);
			if (condition == PawnLostCondition.Killed)
			{
				this.lostPawns++;
				p.addCorpseToLord = true;
			}
			else if (condition == PawnLostCondition.Incapped)
			{
				this.lostPawns++;
			}
			this.motes.RemoveWhere((Mote m) => m.link1.Target == p);
			if (this.lostPawns == 1 && this.assaultLossThreshold > 1)
			{
				Messages.Message("MessageHateChantersAbsorbed".Translate(), MessageTypeDefOf.NeutralEvent, true);
			}
		}

		// Token: 0x060086E8 RID: 34536 RVA: 0x00284998 File Offset: 0x00282B98
		public override void Notify_CorpseLost(Corpse c)
		{
			base.Notify_CorpseLost(c);
			this.lostPawns++;
		}

		// Token: 0x060086E9 RID: 34537 RVA: 0x002849AF File Offset: 0x00282BAF
		public override void LordJobTick()
		{
			base.LordJobTick();
			if (this.lord.CurLordToil is LordToil_HateChant)
			{
				this.TickSustainersAndMotes();
			}
		}

		// Token: 0x060086EA RID: 34538 RVA: 0x002849D0 File Offset: 0x00282BD0
		private void TickSustainersAndMotes()
		{
			if (this.sustainers.Count == 0)
			{
				this.InitializeSfx();
				this.nextCallTick = Find.TickManager.TicksGame + this.CallInterval;
				foreach (Pawn thing in this.lord.ownedPawns)
				{
					this.motes.Add(MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_HateChantShadow, new Vector3(0f, 0f, 0f), 1f, -1f));
					this.motes.Add(MoteMaker.MakeAttachedOverlay(thing, ThingDefOf.Mote_PsychicRitualInvocation, new Vector3(0f, 0f, 0f), 1f, -1f));
				}
			}
			if (this.lord.ownedPawns.Count > 0)
			{
				if (base.Map.IsHashIntervalTick(300))
				{
					this.callMotes.RemoveAll((KeyValuePair<IntVec3, Mote> m) => m.Value.Destroyed);
				}
				if (Find.TickManager.TicksGame == this.nextCallTick)
				{
					Pawn pawn = this.lord.ownedPawns.RandomElement<Pawn>();
					if (!this.callMotes.ContainsKey(pawn.Position))
					{
						SoundDefOf.Pawn_HateChanter_Call.PlayOneShot(SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None));
						Mote value = MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_HateChantCall, Vector3.zero, 1f, -1f);
						this.callMotes.Add(pawn.Position, value);
					}
					this.nextCallTick = Find.TickManager.TicksGame + this.CallInterval;
				}
				foreach (Mote mote in this.motes)
				{
					if (mote != null)
					{
						mote.Maintain();
					}
				}
			}
			foreach (Sustainer sustainer in this.sustainers)
			{
				if (sustainer != null && !sustainer.Ended)
				{
					sustainer.Maintain();
				}
			}
		}

		// Token: 0x060086EB RID: 34539 RVA: 0x00284C40 File Offset: 0x00282E40
		private IEnumerable<PsychicRitualParticipant> GenerateParticipants()
		{
			this.chantingPositions.Clear();
			List<Pawn> pawns = this.lord.ownedPawns;
			int minDistanceBetween = Mathf.RoundToInt(LordJob_HateChant.MinDistBetweenChantersCurve.Evaluate((float)pawns.Count));
			int num;
			for (int i = 0; i < pawns.Count; i = num)
			{
				yield return new PsychicRitualParticipant(new ValueTuple<Pawn, IntVec3>(pawns[i], this.GenerateChantingPosition(pawns[i], minDistanceBetween)));
				num = i + 1;
			}
			if (DebugViewSettings.drawHateChanterPositions)
			{
				Material mat = DebugSolidColorMats.MaterialOf(Color.green * new Color(1f, 1f, 1f, 0.4f));
				using (HashSet<IntVec3>.Enumerator enumerator = this.chantingPositions.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IntVec3 c = enumerator.Current;
						base.Map.debugDrawer.FlashCell(c, mat, null, 100000);
					}
					yield break;
				}
			}
			yield break;
		}

		// Token: 0x060086EC RID: 34540 RVA: 0x00284C50 File Offset: 0x00282E50
		private IntVec3 GenerateChantingPosition(Pawn pawn, int minDistanceBetween)
		{
			LordJob_HateChant.<>c__DisplayClass35_0 CS$<>8__locals1 = new LordJob_HateChant.<>c__DisplayClass35_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.minDistanceBetween = minDistanceBetween;
			CS$<>8__locals1.minDistFromEdge = LordJob_HateChant.MinDistFromEdgeRange.RandomInRange;
			IntVec3 intVec = CellFinder.RandomClosewalkCellNear(pawn.Position, base.Map, 30, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateChantingPosition>g__Validator|0));
			if (intVec.CloseToEdge(base.Map, CS$<>8__locals1.minDistFromEdge))
			{
				intVec = CellFinder.RandomClosewalkCellNear(pawn.Position, base.Map, 30, new Predicate<IntVec3>(CS$<>8__locals1.<GenerateChantingPosition>g__FallbackValidator|1));
			}
			this.chantingPositions.Add(intVec);
			return intVec;
		}

		// Token: 0x060086ED RID: 34541 RVA: 0x00284CE8 File Offset: 0x00282EE8
		private void InitializeSfx()
		{
			this.nextCallTick = Find.TickManager.TicksGame + LordJob_HateChant.CallIntervalSeconds.RandomInRange.SecondsToTicks();
			if (this.smallGroup)
			{
				List<IntVec3> list = new List<IntVec3>();
				foreach (IntVec3 item in this.chantingPositions)
				{
					list.Add(item);
				}
				IntVec3 cell = Gen.AveragePosition(list).ToIntVec3().ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell, base.Map, false), MaintenanceType.PerTick)));
				return;
			}
			ValueTuple<int, float> valueTuple = default(ValueTuple<int, float>);
			ValueTuple<int, float> valueTuple2 = default(ValueTuple<int, float>);
			ValueTuple<int, float> valueTuple3 = default(ValueTuple<int, float>);
			ValueTuple<int, float> valueTuple4 = default(ValueTuple<int, float>);
			foreach (IntVec3 intVec in this.chantingPositions)
			{
				if (intVec.x < 15)
				{
					valueTuple3.Item1++;
					valueTuple3.Item2 += (float)intVec.z;
				}
				else if (intVec.x > base.Map.Size.x - 15)
				{
					valueTuple4.Item1++;
					valueTuple4.Item2 += (float)intVec.z;
				}
				else if (intVec.z < 15)
				{
					valueTuple.Item1++;
					valueTuple.Item2 += (float)intVec.x;
				}
				else if (intVec.z > base.Map.Size.z - 15)
				{
					valueTuple2.Item1++;
					valueTuple2.Item2 += (float)intVec.x;
				}
			}
			if (valueTuple.Item1 > 0)
			{
				valueTuple.Item2 /= (float)valueTuple.Item1;
				IntVec3 cell2 = new IntVec3(Mathf.RoundToInt(valueTuple.Item2), 0, 15).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell2, base.Map, false), MaintenanceType.PerTick)));
			}
			if (valueTuple2.Item1 > 0)
			{
				valueTuple2.Item2 /= (float)valueTuple2.Item1;
				IntVec3 cell3 = new IntVec3(Mathf.RoundToInt(valueTuple2.Item2), 0, base.Map.Size.z - 15).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell3, base.Map, false), MaintenanceType.PerTick)));
			}
			if (valueTuple3.Item1 > 0)
			{
				valueTuple3.Item2 /= (float)valueTuple3.Item1;
				IntVec3 cell4 = new IntVec3(15, 0, Mathf.RoundToInt(valueTuple3.Item2)).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell4, base.Map, false), MaintenanceType.PerTick)));
			}
			if (valueTuple4.Item1 > 0)
			{
				valueTuple4.Item2 /= (float)valueTuple4.Item1;
				IntVec3 cell5 = new IntVec3(base.Map.Size.x - 15, 0, Mathf.RoundToInt(valueTuple4.Item2)).ClampInsideMap(base.Map);
				this.sustainers.Add(SoundDefOf.HateChant_Sighing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(cell5, base.Map, false), MaintenanceType.PerTick)));
			}
		}

		// Token: 0x060086EE RID: 34542 RVA: 0x002850B8 File Offset: 0x002832B8
		private void EnableSelfResurrect()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				Hediff_DeathRefusal firstHediff = ownedPawns[i].health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
				if (firstHediff != null)
				{
					firstHediff.AIEnabled = true;
				}
			}
			List<Corpse> ownedCorpses = this.lord.ownedCorpses;
			for (int j = 0; j < ownedCorpses.Count; j++)
			{
				Hediff_DeathRefusal firstHediff2 = ownedCorpses[j].InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
				if (firstHediff2 != null)
				{
					firstHediff2.AIEnabled = true;
					firstHediff2.Notify_PawnDied(null, null);
				}
			}
		}

		// Token: 0x060086EF RID: 34543 RVA: 0x00285160 File Offset: 0x00283360
		private void MakePawnsWait()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				PawnUtility.ForceWait(ownedPawns[i], LordJob_HateChant.PawnWaitBeforeAssaultSeconds.RandomInRange.SecondsToTicks(), null, false, false);
			}
		}

		// Token: 0x060086F0 RID: 34544 RVA: 0x002851AC File Offset: 0x002833AC
		public override void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lostPawns, "lostPawns", 0, false);
			Scribe_Values.Look<int>(ref this.assaultLossThreshold, "assaultLossThreshold", 0, false);
			Scribe_Values.Look<bool>(ref this.spawningFinished, "spawningFinished", false, false);
			Scribe_Values.Look<bool>(ref this.spawningStarted, "spawningStarted", false, false);
			Scribe_Values.Look<bool>(ref this.smallGroup, "smallGroup", false, false);
			Scribe_Collections.Look<IntVec3>(ref this.chantingPositions, "chantingPositions", LookMode.Value);
		}

		// Token: 0x04005815 RID: 22549
		private static readonly SimpleCurve MinDistBetweenChantersCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 6f),
				true
			},
			{
				new CurvePoint(30f, 5f),
				true
			},
			{
				new CurvePoint(50f, 4f),
				true
			},
			{
				new CurvePoint(70f, 3f),
				true
			}
		};

		// Token: 0x04005816 RID: 22550
		public bool spawningFinished;

		// Token: 0x04005817 RID: 22551
		public bool spawningStarted;

		// Token: 0x04005818 RID: 22552
		public bool smallGroup;

		// Token: 0x04005819 RID: 22553
		private int lostPawns;

		// Token: 0x0400581A RID: 22554
		private int assaultLossThreshold;

		// Token: 0x0400581B RID: 22555
		private HashSet<IntVec3> chantingPositions = new HashSet<IntVec3>();

		// Token: 0x0400581C RID: 22556
		private List<PsychicRitualParticipant> participants = new List<PsychicRitualParticipant>();

		// Token: 0x0400581D RID: 22557
		private List<Sustainer> sustainers = new List<Sustainer>();

		// Token: 0x0400581E RID: 22558
		private readonly List<Mote> motes = new List<Mote>();

		// Token: 0x0400581F RID: 22559
		private readonly Dictionary<IntVec3, Mote> callMotes = new Dictionary<IntVec3, Mote>();

		// Token: 0x04005820 RID: 22560
		private int nextCallTick;

		// Token: 0x04005821 RID: 22561
		private const string SpawnFinishedSignal = "PositionsReady";

		// Token: 0x04005822 RID: 22562
		private const int SpawningBatchSize = 5;

		// Token: 0x04005823 RID: 22563
		private const float SpawningIntervalSeconds = 0.8f;

		// Token: 0x04005824 RID: 22564
		private const int MaxAssaultLossThreshold = 10;

		// Token: 0x04005825 RID: 22565
		private const int SustainerDistFromEdge = 15;

		// Token: 0x04005826 RID: 22566
		private const int GotoToilTimeout = 10800;

		// Token: 0x04005827 RID: 22567
		private const int CallMoteCleanupInterval = 300;

		// Token: 0x04005828 RID: 22568
		private static readonly IntRange MinDistFromEdgeRange = new IntRange(5, 25);

		// Token: 0x04005829 RID: 22569
		private static readonly FloatRange AssaultLossPercent = new FloatRange(0.1f, 0.3f);

		// Token: 0x0400582A RID: 22570
		private static readonly FloatRange PawnWaitBeforeAssaultSeconds = new FloatRange(0.1f, 6f);

		// Token: 0x0400582B RID: 22571
		private static readonly FloatRange CallIntervalSeconds = new FloatRange(3f, 5f);

		// Token: 0x0400582C RID: 22572
		private static readonly FloatRange CallIntervalSecondsSmallGroup = new FloatRange(7f, 12f);
	}
}
