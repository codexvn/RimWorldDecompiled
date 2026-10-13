using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002E39 RID: 11833
	public class CompSpawnerPawn : ThingComp
	{
		// Token: 0x17002AAF RID: 10927
		// (get) Token: 0x06010A8A RID: 68234 RVA: 0x004CD70F File Offset: 0x004CB90F
		private CompProperties_SpawnerPawn Props
		{
			get
			{
				return (CompProperties_SpawnerPawn)this.props;
			}
		}

		// Token: 0x17002AB0 RID: 10928
		// (get) Token: 0x06010A8B RID: 68235 RVA: 0x004CD71C File Offset: 0x004CB91C
		public Lord Lord
		{
			get
			{
				return CompSpawnerPawn.FindLordToJoin(this.parent, this.Props.lordJob, this.Props.shouldJoinParentLord, null, this.Props.lordJoinRadius);
			}
		}

		// Token: 0x17002AB1 RID: 10929
		// (get) Token: 0x06010A8C RID: 68236 RVA: 0x004CD74C File Offset: 0x004CB94C
		private float SpawnedPawnsPoints
		{
			get
			{
				this.FilterOutUnspawnedPawns();
				float num = 0f;
				for (int i = 0; i < this.spawnedPawns.Count; i++)
				{
					num += this.spawnedPawns[i].kindDef.combatPower;
				}
				return num;
			}
		}

		// Token: 0x17002AB2 RID: 10930
		// (get) Token: 0x06010A8D RID: 68237 RVA: 0x004CD795 File Offset: 0x004CB995
		public bool Active
		{
			get
			{
				return this.pawnsLeftToSpawn != 0 && !this.Dormant;
			}
		}

		// Token: 0x17002AB3 RID: 10931
		// (get) Token: 0x06010A8E RID: 68238 RVA: 0x004CD7AC File Offset: 0x004CB9AC
		public CompCanBeDormant DormancyComp
		{
			get
			{
				CompCanBeDormant result;
				if ((result = this.dormancyCompCached) == null)
				{
					result = (this.dormancyCompCached = this.parent.TryGetComp<CompCanBeDormant>());
				}
				return result;
			}
		}

		// Token: 0x17002AB4 RID: 10932
		// (get) Token: 0x06010A8F RID: 68239 RVA: 0x004CD7D7 File Offset: 0x004CB9D7
		public bool Dormant
		{
			get
			{
				return this.DormancyComp != null && !this.DormancyComp.Awake;
			}
		}

		// Token: 0x06010A90 RID: 68240 RVA: 0x004CD7F4 File Offset: 0x004CB9F4
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			if (this.chosenKind == null)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.Props.maxPawnsToSpawn != IntRange.Zero)
			{
				this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
			}
		}

		// Token: 0x06010A91 RID: 68241 RVA: 0x004CD84C File Offset: 0x004CBA4C
		public static Lord FindLordToJoin(Thing spawner, Type lordJobType, bool shouldTryJoinParentLord, Func<Thing, List<Pawn>> spawnedPawnSelector = null, float lordJoinRadius = 2.1474836E+09f)
		{
			if (spawner.Spawned)
			{
				if (shouldTryJoinParentLord)
				{
					Building building = spawner as Building;
					Lord lord = (building != null) ? building.GetLord() : null;
					if (lord != null)
					{
						return lord;
					}
				}
				if (spawnedPawnSelector == null)
				{
					spawnedPawnSelector = delegate(Thing s)
					{
						CompSpawnerPawn compSpawnerPawn = s.TryGetComp<CompSpawnerPawn>();
						if (compSpawnerPawn == null)
						{
							return null;
						}
						return compSpawnerPawn.spawnedPawns;
					};
				}
				Predicate<Pawn> hasJob = delegate(Pawn x)
				{
					Lord lord2 = x.GetLord();
					return lord2 != null && lord2.LordJob.GetType() == lordJobType;
				};
				Pawn foundPawn = null;
				RegionTraverser.BreadthFirstTraverse(spawner.GetRegion(RegionType.Set_Passable), (Region from, Region to) => true, delegate(Region r)
				{
					List<Thing> list = r.ListerThings.ThingsOfDef(spawner.def);
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].Faction == spawner.Faction)
						{
							List<Pawn> list2 = spawnedPawnSelector(list[i]);
							if (list2 != null)
							{
								foundPawn = list2.Find(hasJob);
							}
							if (foundPawn != null && foundPawn.Position.InHorDistOf(spawner.Position, lordJoinRadius))
							{
								return true;
							}
						}
					}
					return false;
				}, 40, RegionType.Set_Passable);
				if (foundPawn != null)
				{
					return foundPawn.GetLord();
				}
			}
			return null;
		}

		// Token: 0x06010A92 RID: 68242 RVA: 0x004CD970 File Offset: 0x004CBB70
		public static Lord CreateNewLord(Thing byThing, bool aggressive, float defendRadius, Type lordJobType)
		{
			IntVec3 invalid;
			if (!CellFinder.TryFindRandomCellNear(byThing.Position, byThing.Map, 5, (IntVec3 c) => c.Standable(byThing.Map) && byThing.Map.reachability.CanReach(c, byThing, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false)), out invalid, -1))
			{
				string str = "Found no place for pawns to defend ";
				Thing byThing2 = byThing;
				Log.Error(str + ((byThing2 != null) ? byThing2.ToString() : null));
				invalid = IntVec3.Invalid;
			}
			return LordMaker.MakeNewLord(byThing.Faction, Activator.CreateInstance(lordJobType, new object[]
			{
				new SpawnedPawnParams
				{
					aggressive = aggressive,
					defendRadius = defendRadius,
					defSpot = invalid,
					spawnerThing = byThing
				}
			}) as LordJob, byThing.Map, null);
		}

		// Token: 0x06010A93 RID: 68243 RVA: 0x004CDA34 File Offset: 0x004CBC34
		private void SpawnInitialPawns()
		{
			int num = 0;
			Pawn pawn;
			while (num < this.Props.initialPawnsCount && this.TrySpawnPawn(out pawn))
			{
				num++;
			}
			this.SpawnPawnsUntilPoints(this.Props.initialPawnsPoints);
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x06010A94 RID: 68244 RVA: 0x004CDA78 File Offset: 0x004CBC78
		public void SpawnPawnsUntilPoints(float points)
		{
			int num = 0;
			while (this.SpawnedPawnsPoints < points)
			{
				num++;
				if (num > 1000)
				{
					Log.Error("Too many iterations.");
					break;
				}
				Pawn pawn;
				if (!this.TrySpawnPawn(out pawn))
				{
					break;
				}
			}
			this.CalculateNextPawnSpawnTick();
		}

		// Token: 0x06010A95 RID: 68245 RVA: 0x004CDABA File Offset: 0x004CBCBA
		private void CalculateNextPawnSpawnTick()
		{
			this.CalculateNextPawnSpawnTick(this.Props.pawnSpawnIntervalDays.RandomInRange * 60000f);
		}

		// Token: 0x06010A96 RID: 68246 RVA: 0x004CDAD8 File Offset: 0x004CBCD8
		public void CalculateNextPawnSpawnTick(float delayTicks)
		{
			if (Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f)
			{
				this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)(delayTicks / (CompSpawnerPawn.RateFactorForBugCount.Evaluate((float)this.spawnedPawns.Count) * Find.Storyteller.difficulty.enemyReproductionRateFactor));
				return;
			}
			this.nextPawnSpawnTick = Find.TickManager.TicksGame + (int)delayTicks;
		}

		// Token: 0x06010A97 RID: 68247 RVA: 0x004CDB4C File Offset: 0x004CBD4C
		private void FilterOutUnspawnedPawns()
		{
			for (int i = this.spawnedPawns.Count - 1; i >= 0; i--)
			{
				if (!this.spawnedPawns[i].Spawned)
				{
					this.spawnedPawns.RemoveAt(i);
				}
			}
		}

		// Token: 0x06010A98 RID: 68248 RVA: 0x004CDB90 File Offset: 0x004CBD90
		private PawnKindDef RandomPawnKindDef()
		{
			float curPoints = this.SpawnedPawnsPoints;
			IEnumerable<PawnKindDef> source = this.Props.spawnablePawnKinds;
			if (this.Props.maxSpawnedPawnsPoints > -1f)
			{
				source = from x in source
				where curPoints + x.combatPower <= this.Props.maxSpawnedPawnsPoints
				select x;
			}
			PawnKindDef result;
			if (source.TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06010A99 RID: 68249 RVA: 0x004CDBF4 File Offset: 0x004CBDF4
		private bool TrySpawnPawn(out Pawn pawn)
		{
			if (!this.canSpawnPawns)
			{
				pawn = null;
				return false;
			}
			if (!this.Props.chooseSingleTypeToSpawn)
			{
				this.chosenKind = this.RandomPawnKindDef();
			}
			if (this.chosenKind == null)
			{
				pawn = null;
				return false;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(this.chosenKind, this.parent.Faction, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			if (this.chosenKind.RaceProps.IsMechanoid)
			{
				request.AllowedDevelopmentalStages = DevelopmentalStage.Newborn;
			}
			else
			{
				int index = this.chosenKind.lifeStages.Count - 1;
				request.FixedBiologicalAge = new float?(this.chosenKind.race.race.lifeStageAges[index].minAge);
			}
			pawn = PawnGenerator.GeneratePawn(request);
			this.spawnedPawns.Add(pawn);
			GenSpawn.Spawn(pawn, CellFinder.RandomClosewalkCellNear(this.parent.Position, this.parent.Map, this.Props.pawnSpawnRadius, null), this.parent.Map, WipeMode.Vanish);
			(this.Lord ?? CompSpawnerPawn.CreateNewLord(this.parent, this.aggressive, this.Props.defendRadius, this.Props.lordJob)).AddPawn(pawn);
			SoundDef spawnSound = this.Props.spawnSound;
			if (spawnSound != null)
			{
				spawnSound.PlayOneShot(this.parent);
			}
			if (this.pawnsLeftToSpawn > 0)
			{
				this.pawnsLeftToSpawn--;
			}
			this.SendMessage();
			return true;
		}

		// Token: 0x06010A9A RID: 68250 RVA: 0x004CDDF3 File Offset: 0x004CBFF3
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && this.Active && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
		}

		// Token: 0x06010A9B RID: 68251 RVA: 0x004CDE18 File Offset: 0x004CC018
		public override void CompTick()
		{
			if (this.Active && this.parent.Spawned && this.nextPawnSpawnTick == -1)
			{
				this.SpawnInitialPawns();
			}
			if (this.parent.Spawned)
			{
				this.FilterOutUnspawnedPawns();
				if (this.Active && Find.TickManager.TicksGame >= this.nextPawnSpawnTick)
				{
					Pawn pawn;
					if ((this.Props.maxSpawnedPawnsPoints < 0f || this.SpawnedPawnsPoints < this.Props.maxSpawnedPawnsPoints) && Find.Storyteller.difficulty.enemyReproductionRateFactor > 0f && this.TrySpawnPawn(out pawn) && pawn.caller != null)
					{
						pawn.caller.DoCall(false);
					}
					this.CalculateNextPawnSpawnTick();
				}
			}
		}

		// Token: 0x06010A9C RID: 68252 RVA: 0x004CDED8 File Offset: 0x004CC0D8
		public void SendMessage()
		{
			if (!this.Props.spawnMessageKey.NullOrEmpty() && MessagesRepeatAvoider.MessageShowAllowed(this.Props.spawnMessageKey, 0.1f))
			{
				Messages.Message(this.Props.spawnMessageKey.Translate(), this.parent, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06010A9D RID: 68253 RVA: 0x004CDF39 File Offset: 0x004CC139
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn pawn",
					icon = TexCommand.ReleaseAnimals,
					action = delegate()
					{
						Pawn pawn;
						this.TrySpawnPawn(out pawn);
					}
				};
			}
			yield break;
		}

		// Token: 0x06010A9E RID: 68254 RVA: 0x004CDF4C File Offset: 0x004CC14C
		public override string CompInspectStringExtra()
		{
			if (!this.Props.showNextSpawnInInspect || this.nextPawnSpawnTick <= 0 || this.chosenKind == null)
			{
				return null;
			}
			if (this.pawnsLeftToSpawn == 0 && !this.Props.noPawnsLeftToSpawnKey.NullOrEmpty())
			{
				return this.Props.noPawnsLeftToSpawnKey.Translate();
			}
			string text;
			if (!this.Dormant)
			{
				text = (this.Props.nextSpawnInspectStringKey ?? "SpawningNextPawnIn").Translate(this.chosenKind.LabelCap, (this.nextPawnSpawnTick - Find.TickManager.TicksGame).ToStringTicksToDays("F1"));
			}
			else
			{
				if (this.Props.nextSpawnInspectStringKeyDormant == null)
				{
					return null;
				}
				text = this.Props.nextSpawnInspectStringKeyDormant.Translate() + ": " + this.chosenKind.LabelCap;
			}
			if (this.pawnsLeftToSpawn > 0 && !this.Props.pawnsLeftToSpawnKey.NullOrEmpty())
			{
				text = text + ("\n" + this.Props.pawnsLeftToSpawnKey.Translate() + ": ") + this.pawnsLeftToSpawn.ToString();
			}
			return text;
		}

		// Token: 0x06010A9F RID: 68255 RVA: 0x004CE09C File Offset: 0x004CC29C
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.nextPawnSpawnTick, "nextPawnSpawnTick", 0, false);
			Scribe_Values.Look<int>(ref this.pawnsLeftToSpawn, "pawnsLeftToSpawn", -1, false);
			Scribe_Collections.Look<Pawn>(ref this.spawnedPawns, "spawnedPawns", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.aggressive, "aggressive", false, false);
			Scribe_Values.Look<bool>(ref this.canSpawnPawns, "canSpawnPawns", true, false);
			Scribe_Defs.Look<PawnKindDef>(ref this.chosenKind, "chosenKind");
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.spawnedPawns.RemoveAll((Pawn x) => x == null);
				if (this.pawnsLeftToSpawn == -1 && this.Props.maxPawnsToSpawn != IntRange.Zero)
				{
					this.pawnsLeftToSpawn = this.Props.maxPawnsToSpawn.RandomInRange;
				}
			}
		}

		// Token: 0x0400ADAD RID: 44461
		public int nextPawnSpawnTick = -1;

		// Token: 0x0400ADAE RID: 44462
		public int pawnsLeftToSpawn = -1;

		// Token: 0x0400ADAF RID: 44463
		public List<Pawn> spawnedPawns = new List<Pawn>();

		// Token: 0x0400ADB0 RID: 44464
		public bool aggressive = true;

		// Token: 0x0400ADB1 RID: 44465
		public bool canSpawnPawns = true;

		// Token: 0x0400ADB2 RID: 44466
		private PawnKindDef chosenKind;

		// Token: 0x0400ADB3 RID: 44467
		private static readonly SimpleCurve RateFactorForBugCount = new SimpleCurve
		{
			{
				0f,
				1f,
				true
			},
			{
				5f,
				0.5f,
				true
			}
		};

		// Token: 0x0400ADB4 RID: 44468
		private CompCanBeDormant dormancyCompCached;
	}
}
