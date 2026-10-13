using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.QuestGen
{
	// Token: 0x02003BA2 RID: 15266
	public class QuestPart_Noctoliths : QuestPart
	{
		// Token: 0x06014BB2 RID: 84914 RVA: 0x00635985 File Offset: 0x00633B85
		public QuestPart_Noctoliths()
		{
		}

		// Token: 0x06014BB3 RID: 84915 RVA: 0x006359B0 File Offset: 0x00633BB0
		public QuestPart_Noctoliths(MapParent mapParent, List<Thing> noctoliths, float points, string inSignalNoctolithDamaged, string inSignalNoctolithKilled)
		{
			this.mapParent = mapParent;
			this.noctoliths = noctoliths;
			this.points = points;
			this.inSignalNoctolithDamaged = inSignalNoctolithDamaged;
			this.inSignalNoctolithKilled = inSignalNoctolithKilled;
			for (int i = 0; i < 3; i++)
			{
				this.healthPercentages.Add(QuestPart_Noctoliths.HealthRangeToSpawnNoctols.RandomInRange);
			}
		}

		// Token: 0x06014BB4 RID: 84916 RVA: 0x00635A30 File Offset: 0x00633C30
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return;
			}
			Map map = this.mapParent.Map;
			Thing thing;
			if (signal.tag == this.inSignalNoctolithDamaged && signal.args.TryGetArg<Thing>("SUBJECT", out thing) && thing.Position.InBounds(map) && thing.def == ThingDefOf.Noctolith && this.damagedCount < 3 && (float)thing.HitPoints / (float)thing.MaxHitPoints <= this.healthPercentages[this.damagedCount] && !this.damagedNoctoliths.Contains(thing))
			{
				this.damagedNoctoliths.Add(thing);
				this.damagedCount++;
				this.SpawnNoctols(thing, map);
				return;
			}
			Thing item;
			if (signal.tag == this.inSignalNoctolithKilled && signal.args.TryGetArg<Thing>("SUBJECT", out item) && this.noctoliths.Contains(item))
			{
				this.killedCount++;
				if (!this.noctoliths.Any((Thing x) => !x.DestroyedOrNull()))
				{
					Find.LetterStack.ReceiveLetter("DarknessLiftingEarlyLetterLabel".Translate(), "DarknessLiftingEarlyLetterText".Translate(), LetterDefOf.PositiveEvent, null, 0, true);
					this.quest.End(QuestEndOutcome.Success, true, true);
					return;
				}
				if (this.killedCount == 1)
				{
					Find.LetterStack.ReceiveLetter("DarknessWaveringLetterLabel".Translate(), "DarknessWaveringLetterText".Translate(), LetterDefOf.PositiveEvent, null, 0, true);
				}
			}
		}

		// Token: 0x06014BB5 RID: 84917 RVA: 0x00635BE0 File Offset: 0x00633DE0
		private void SpawnNoctols(Thing noctolith, Map map)
		{
			IntVec3 position = noctolith.Position;
			float num = this.points * QuestPart_Noctoliths.DamagedNoctolithsToPointsScaleCurve.Evaluate((float)this.damagedCount);
			List<Pawn> noctolsForPoints = this.GetNoctolsForPoints(num, map);
			if (!noctolsForPoints.Any<Pawn>())
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			Lord lord = map.lordManager.lords.FirstOrDefault((Lord l) => l.LordJob is LordJob_AssaultColony && l.faction == Faction.OfEntities);
			if (lord == null)
			{
				flag2 = true;
				lord = LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_AssaultColony(Faction.OfEntities, false, false, false, false, false, false, false), map, null);
			}
			Predicate<IntVec3> <>9__2;
			Predicate<IntVec3> <>9__3;
			foreach (Pawn pawn in noctolsForPoints)
			{
				IntVec3 root = position;
				Map map2 = map;
				int squareRadius = 40;
				Predicate<IntVec3> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((IntVec3 c) => base.<SpawnNoctols>g__ValidSpawnCell|1(c, true)));
				}
				IntVec3 loc;
				if (!CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out loc, -1))
				{
					IntVec3 root2 = position;
					Map map3 = map;
					int squareRadius2 = 40;
					Predicate<IntVec3> validator2;
					if ((validator2 = <>9__3) == null)
					{
						validator2 = (<>9__3 = ((IntVec3 c) => base.<SpawnNoctols>g__ValidSpawnCell|1(c, false)));
					}
					if (!CellFinder.TryFindRandomCellNear(root2, map3, squareRadius2, validator2, out loc, -1))
					{
						continue;
					}
				}
				PawnGroundSpawner pawnGroundSpawner = (PawnGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PawnGroundSpawner, null);
				pawnGroundSpawner.Init(pawn, QuestPart_Noctoliths.TunnelDelayTicks);
				GenSpawn.Spawn(pawnGroundSpawner, loc, map, WipeMode.Vanish);
				flag = true;
				lord.AddPawn(pawn);
			}
			SoundDefOf.Noctolith_Destroyed.PlayOneShotOnCamera(null);
			if (flag)
			{
				Find.LetterStack.ReceiveLetter("NoctolAttackLetterLabel".Translate(), "NoctolAttackLetterText".Translate(), LetterDefOf.ThreatBig, new LookTargets(noctolsForPoints), null, null, null, null, 0, true);
				return;
			}
			if (flag2)
			{
				map.lordManager.RemoveLord(lord);
			}
		}

		// Token: 0x06014BB6 RID: 84918 RVA: 0x00635DD0 File Offset: 0x00633FD0
		private List<Pawn> GetNoctolsForPoints(float points, Map map)
		{
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Noctols,
				tile = map.Tile,
				faction = Faction.OfEntities
			};
			pawnGroupMakerParms.points = ((points > 0f) ? points : StorytellerUtility.DefaultThreatPointsNow(map)) * QuestPart_Noctoliths.NoctolPointsFactorRange.RandomInRange;
			pawnGroupMakerParms.points = Mathf.Max(pawnGroupMakerParms.points, pawnGroupMakerParms.faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, null) * 1.05f);
			IEnumerable<Pawn> enumerable = PawnGroupMakerUtility.GeneratePawns(pawnGroupMakerParms, true);
			if (enumerable == null)
			{
				return null;
			}
			return enumerable.ToList<Pawn>();
		}

		// Token: 0x06014BB7 RID: 84919 RVA: 0x00635E6A File Offset: 0x0063406A
		public override void Cleanup()
		{
			base.Cleanup();
			this.damagedNoctoliths.Clear();
			this.healthPercentages.Clear();
		}

		// Token: 0x06014BB8 RID: 84920 RVA: 0x00635E88 File Offset: 0x00634088
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<float>(ref this.points, "points", 0f, false);
			Scribe_Values.Look<string>(ref this.inSignalNoctolithDamaged, "inSignalNoctolithDamaged", null, false);
			Scribe_Values.Look<string>(ref this.inSignalNoctolithKilled, "inSignalNoctolithKilled", null, false);
			Scribe_Values.Look<int>(ref this.damagedCount, "damagedCount", 0, false);
			Scribe_Values.Look<int>(ref this.killedCount, "killedCount", 0, false);
			Scribe_Collections.Look<Thing>(ref this.damagedNoctoliths, "damagedNoctoliths", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<Thing>(ref this.noctoliths, "noctoliths", LookMode.Reference, Array.Empty<object>());
			Scribe_Collections.Look<float>(ref this.healthPercentages, "healthPercentages", LookMode.Value, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.damagedNoctoliths.RemoveAll((Thing x) => x == null);
				this.noctoliths.RemoveAll((Thing x) => x == null);
			}
		}

		// Token: 0x0400E6B8 RID: 59064
		private float points;

		// Token: 0x0400E6B9 RID: 59065
		private string inSignalNoctolithDamaged;

		// Token: 0x0400E6BA RID: 59066
		private string inSignalNoctolithKilled;

		// Token: 0x0400E6BB RID: 59067
		private int damagedCount;

		// Token: 0x0400E6BC RID: 59068
		private int killedCount;

		// Token: 0x0400E6BD RID: 59069
		private List<Thing> damagedNoctoliths = new List<Thing>();

		// Token: 0x0400E6BE RID: 59070
		private List<Thing> noctoliths = new List<Thing>();

		// Token: 0x0400E6BF RID: 59071
		private List<float> healthPercentages = new List<float>();

		// Token: 0x0400E6C0 RID: 59072
		private MapParent mapParent;

		// Token: 0x0400E6C1 RID: 59073
		private static readonly FloatRange NoctolPointsFactorRange = new FloatRange(0.8f, 1f);

		// Token: 0x0400E6C2 RID: 59074
		private static readonly FloatRange HealthRangeToSpawnNoctols = new FloatRange(0f, 0.99f);

		// Token: 0x0400E6C3 RID: 59075
		public static readonly SimpleCurve DamagedNoctolithsToPointsScaleCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0.35f),
				true
			},
			{
				new CurvePoint(2f, 0.35f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			}
		};

		// Token: 0x0400E6C4 RID: 59076
		private static readonly IntRange TunnelDelayTicks = new IntRange(60, 120);
	}
}
