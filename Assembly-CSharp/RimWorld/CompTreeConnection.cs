using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002EAC RID: 11948
	[StaticConstructorOnStartup]
	public class CompTreeConnection : ThingComp
	{
		// Token: 0x17002B54 RID: 11092
		// (get) Token: 0x06010D65 RID: 68965 RVA: 0x004D8F02 File Offset: 0x004D7102
		public CompProperties_TreeConnection Props
		{
			get
			{
				return (CompProperties_TreeConnection)this.props;
			}
		}

		// Token: 0x17002B55 RID: 11093
		// (get) Token: 0x06010D66 RID: 68966 RVA: 0x004D8F0F File Offset: 0x004D710F
		public Pawn ConnectedPawn
		{
			get
			{
				return this.connectedPawn;
			}
		}

		// Token: 0x17002B56 RID: 11094
		// (get) Token: 0x06010D67 RID: 68967 RVA: 0x004D8F17 File Offset: 0x004D7117
		public bool Connected
		{
			get
			{
				return this.ConnectedPawn != null;
			}
		}

		// Token: 0x17002B57 RID: 11095
		// (get) Token: 0x06010D68 RID: 68968 RVA: 0x004D8F22 File Offset: 0x004D7122
		public bool ConnectionTorn
		{
			get
			{
				return this.nextUntornTick >= Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002B58 RID: 11096
		// (get) Token: 0x06010D69 RID: 68969 RVA: 0x004D8F39 File Offset: 0x004D7139
		public bool HasProductionMode
		{
			get
			{
				return this.desiredMode != null;
			}
		}

		// Token: 0x17002B59 RID: 11097
		// (get) Token: 0x06010D6A RID: 68970 RVA: 0x004D8F44 File Offset: 0x004D7144
		public int UntornInDurationTicks
		{
			get
			{
				return this.nextUntornTick - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17002B5A RID: 11098
		// (get) Token: 0x06010D6B RID: 68971 RVA: 0x004D8F57 File Offset: 0x004D7157
		public GauranlenTreeModeDef Mode
		{
			get
			{
				return this.currentMode;
			}
		}

		// Token: 0x17002B5B RID: 11099
		// (get) Token: 0x06010D6C RID: 68972 RVA: 0x004D8F5F File Offset: 0x004D715F
		public PawnKindDef DryadKind
		{
			get
			{
				GauranlenTreeModeDef mode = this.Mode;
				return ((mode != null) ? mode.pawnKindDef : null) ?? PawnKindDefOf.Dryad_Basic;
			}
		}

		// Token: 0x17002B5C RID: 11100
		// (get) Token: 0x06010D6D RID: 68973 RVA: 0x004D8F7C File Offset: 0x004D717C
		public int MaxDryads
		{
			get
			{
				if (!this.Connected)
				{
					return this.Props.maxDryadsWild;
				}
				return (int)this.Props.maxDryadsPerConnectionStrengthCurve.Evaluate(this.ConnectionStrength);
			}
		}

		// Token: 0x17002B5D RID: 11101
		// (get) Token: 0x06010D6E RID: 68974 RVA: 0x004D8FA9 File Offset: 0x004D71A9
		private int SpawningDurationTicks
		{
			get
			{
				return (int)(60000f * this.Props.spawnDays);
			}
		}

		// Token: 0x17002B5E RID: 11102
		// (get) Token: 0x06010D6F RID: 68975 RVA: 0x004D8FBD File Offset: 0x004D71BD
		// (set) Token: 0x06010D70 RID: 68976 RVA: 0x004D8FC5 File Offset: 0x004D71C5
		public float DesiredConnectionStrength
		{
			get
			{
				return this.desiredConnectionStrength;
			}
			set
			{
				this.desiredConnectionStrength = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17002B5F RID: 11103
		// (get) Token: 0x06010D71 RID: 68977 RVA: 0x004D8FD3 File Offset: 0x004D71D3
		// (set) Token: 0x06010D72 RID: 68978 RVA: 0x004D8FDB File Offset: 0x004D71DB
		public float ConnectionStrength
		{
			get
			{
				return this.connectionStrength;
			}
			set
			{
				this.connectionStrength = Mathf.Clamp01(value);
			}
		}

		// Token: 0x17002B60 RID: 11104
		// (get) Token: 0x06010D73 RID: 68979 RVA: 0x004D8FE9 File Offset: 0x004D71E9
		private Material PodMat
		{
			get
			{
				if (this.cachedPodMat == null)
				{
					this.cachedPodMat = MaterialPool.MatFrom("Things/Building/Misc/DryadFormingPod/DryadFormingPod", ShaderDatabase.Cutout);
				}
				return this.cachedPodMat;
			}
		}

		// Token: 0x17002B61 RID: 11105
		// (get) Token: 0x06010D74 RID: 68980 RVA: 0x004D9014 File Offset: 0x004D7214
		private List<Thing> BuildingsReducingConnectionStrength
		{
			get
			{
				return GauranlenUtility.BuildingsAffectingConnectionStrengthAt(this.parent.Position, this.parent.Map, this.Props);
			}
		}

		// Token: 0x17002B62 RID: 11106
		// (get) Token: 0x06010D75 RID: 68981 RVA: 0x004D9038 File Offset: 0x004D7238
		public float ConnectionStrengthLossPerDay
		{
			get
			{
				float num = this.Props.connectionLossPerLevelCurve.Evaluate(this.ConnectionStrength);
				List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
				if (this.parent.Spawned && buildingsReducingConnectionStrength.Any<Thing>())
				{
					num += this.Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(this.ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
				}
				return num;
			}
		}

		// Token: 0x17002B63 RID: 11107
		// (get) Token: 0x06010D76 RID: 68982 RVA: 0x004D9094 File Offset: 0x004D7294
		public float ConnectionStrengthGainPerHourOfPruning
		{
			get
			{
				float num = this.Props.connectionStrengthGainPerHourPruningBase;
				if (StatDefOf.PruningSpeed.Worker.IsDisabledFor(this.ConnectedPawn))
				{
					num *= 0.75f;
				}
				else
				{
					num *= this.ConnectedPawn.GetStatValue(StatDefOf.PruningSpeed, true, -1);
				}
				if (this.Props.connectionStrengthGainPerPlantSkill != null)
				{
					num *= this.Props.connectionStrengthGainPerPlantSkill.Evaluate((float)this.ConnectedPawn.skills.GetSkill(SkillDefOf.Plants).Level);
				}
				return num;
			}
		}

		// Token: 0x17002B64 RID: 11108
		// (get) Token: 0x06010D77 RID: 68983 RVA: 0x004D9120 File Offset: 0x004D7320
		private float MinConnectionStrengthForSingleDryad
		{
			get
			{
				foreach (CurvePoint curvePoint in this.Props.maxDryadsPerConnectionStrengthCurve.Points)
				{
					if (curvePoint.y > 0f)
					{
						return curvePoint.x;
					}
				}
				return 0f;
			}
		}

		// Token: 0x06010D78 RID: 68984 RVA: 0x004D9198 File Offset: 0x004D7398
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Tree connection"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			if (!respawningAfterLoad)
			{
				this.lastPrunedTick = Find.TickManager.TicksGame;
				this.spawnTick = Find.TickManager.TicksGame + this.SpawningDurationTicks;
			}
		}

		// Token: 0x06010D79 RID: 68985 RVA: 0x004D91E8 File Offset: 0x004D73E8
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.leafEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.leafEffecter = null;
			Pawn pawn = this.ConnectedPawn;
			if (pawn != null)
			{
				Pawn_ConnectionsTracker connections = pawn.connections;
				if (connections != null)
				{
					connections.Notify_ConnectedThingDestroyed(this.parent);
				}
			}
			for (int i = this.dryads.Count - 1; i >= 0; i--)
			{
				Pawn_ConnectionsTracker connections2 = this.dryads[i].connections;
				if (connections2 != null)
				{
					connections2.Notify_ConnectedThingDestroyed(this.parent);
				}
				this.dryads[i].forceNoDeathNotification = true;
				this.dryads[i].Kill(null, null);
				this.dryads[i].forceNoDeathNotification = false;
			}
			if (this.Connected && this.ConnectedPawn.Faction == Faction.OfPlayer)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelConnectedTreeDestroyed".Translate(this.parent.Named("TREE")), "LetterTextConnectedTreeDestroyed".Translate(this.parent.Named("TREE"), this.ConnectedPawn.Named("CONNECTEDPAWN")), LetterDefOf.NegativeEvent, this.ConnectedPawn, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06010D7A RID: 68986 RVA: 0x004D9328 File Offset: 0x004D7528
		public override void Notify_MapRemoved()
		{
			Pawn pawn = this.ConnectedPawn;
			if (pawn != null)
			{
				Pawn_ConnectionsTracker connections = pawn.connections;
				if (connections != null)
				{
					connections.Notify_ConnectedThingLeftBehind(this.parent);
				}
			}
			for (int i = this.dryads.Count - 1; i >= 0; i--)
			{
				Pawn pawn2 = this.dryads[i];
				pawn2.forceNoDeathNotification = true;
				pawn2.Kill(null, null);
				pawn2.forceNoDeathNotification = false;
			}
		}

		// Token: 0x06010D7B RID: 68987 RVA: 0x004D9398 File Offset: 0x004D7598
		public override void CompTick()
		{
			if (!ModsConfig.IdeologyActive)
			{
				return;
			}
			if (Find.TickManager.TicksGame >= this.spawnTick)
			{
				this.SpawnDryad();
			}
			if (this.leafEffecter == null)
			{
				this.leafEffecter = EffecterDefOf.GauranlenLeavesBatch.Spawn();
				this.leafEffecter.Trigger(this.parent, this.parent, -1);
			}
			Effecter effecter = this.leafEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(this.parent, this.parent);
			}
			if (this.Connected && Find.TickManager.TicksGame - this.lastPrunedTick > 1)
			{
				this.ConnectionStrength -= this.ConnectionStrengthLossPerDay / 60000f;
			}
			if (this.parent.IsHashIntervalTick(300))
			{
				if (this.Mode == GauranlenTreeModeDefOf.Gaumaker && this.dryads.Count >= 3)
				{
					IntVec3 loc;
					if (this.gaumakerPod == null && this.TryGetGaumakerCell(out loc))
					{
						this.gaumakerPod = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.GaumakerCocoon, null), loc, this.parent.Map, WipeMode.Vanish);
						return;
					}
				}
				else if (this.gaumakerPod != null && !this.gaumakerPod.Destroyed)
				{
					this.gaumakerPod.Destroy(DestroyMode.Vanish);
					this.gaumakerPod = null;
				}
			}
		}

		// Token: 0x06010D7C RID: 68988 RVA: 0x004D94E8 File Offset: 0x004D76E8
		public void ConnectToPawn(Pawn pawn, float ritualQuality)
		{
			if (!ModLister.CheckIdeology("Tree connection"))
			{
				return;
			}
			if (this.ConnectionTorn)
			{
				return;
			}
			this.connectedPawn = pawn;
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections != null)
			{
				connections.ConnectTo(this.parent);
			}
			this.ConnectionStrength = this.Props.initialConnectionStrengthRange.LerpThroughRange(ritualQuality);
			this.lastPrunedTick = 0;
			for (int i = 0; i < this.dryads.Count; i++)
			{
				this.ResetDryad(this.dryads[i]);
				MentalState mentalState = this.dryads[i].MentalState;
				if (mentalState != null)
				{
					mentalState.RecoverFromState();
				}
			}
		}

		// Token: 0x06010D7D RID: 68989 RVA: 0x004D958C File Offset: 0x004D778C
		public void FinalizeMode()
		{
			this.currentMode = this.desiredMode;
			if (this.Connected)
			{
				MoteMaker.MakeStaticMote((this.ConnectedPawn.Position.ToVector3Shifted() + this.parent.Position.ToVector3Shifted()) / 2f, this.parent.Map, ThingDefOf.Mote_GauranlenCasteChanged, 1f, false, 0f);
			}
		}

		// Token: 0x06010D7E RID: 68990 RVA: 0x004D9604 File Offset: 0x004D7804
		public void Notify_PawnDied(Pawn p)
		{
			if (this.connectedPawn == p)
			{
				this.TearConnection();
				return;
			}
			for (int i = 0; i < this.dryads.Count; i++)
			{
				if (p == this.dryads[i])
				{
					if (this.Connected)
					{
						Pawn_NeedsTracker needs = this.ConnectedPawn.needs;
						if (needs != null)
						{
							Need_Mood mood = needs.mood;
							if (mood != null)
							{
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts != null)
								{
									thoughts.memories.TryGainMemory(ThoughtDefOf.DryadDied, null, null);
								}
							}
						}
						this.ConnectionStrength -= this.Props.connectionStrengthLossPerDryadDeath;
					}
					this.dryads.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x06010D7F RID: 68991 RVA: 0x004D96AE File Offset: 0x004D78AE
		public void RemoveDryad(Pawn oldDryad)
		{
			this.dryads.Remove(oldDryad);
		}

		// Token: 0x06010D80 RID: 68992 RVA: 0x004D96C0 File Offset: 0x004D78C0
		public bool ShouldReturnToTree(Pawn dryad)
		{
			if (this.dryads.NullOrEmpty<Pawn>() || !this.dryads.Contains(dryad))
			{
				return false;
			}
			int num = this.dryads.Count - this.MaxDryads;
			if (num <= 0)
			{
				return false;
			}
			this.tmpDryads.Clear();
			this.tmpDryads.AddRange(this.dryads);
			this.tmpDryads.SortBy((Pawn x) => x.kindDef == this.DryadKind, (Pawn x) => x.ageTracker.AgeChronologicalTicks);
			for (int i = 0; i < num; i++)
			{
				if (this.tmpDryads[i] == dryad)
				{
					this.tmpDryads.Clear();
					return true;
				}
			}
			this.tmpDryads.Clear();
			return false;
		}

		// Token: 0x06010D81 RID: 68993 RVA: 0x004D978C File Offset: 0x004D798C
		public bool ShouldEnterGaumakerPod(Pawn dryad)
		{
			if (this.gaumakerPod == null || this.gaumakerPod.Destroyed)
			{
				return false;
			}
			if (this.dryads.NullOrEmpty<Pawn>() || this.dryads.Count < 3 || !this.dryads.Contains(dryad))
			{
				return false;
			}
			this.tmpDryads.Clear();
			for (int i = 0; i < this.dryads.Count; i++)
			{
				if (this.dryads[i].kindDef == PawnKindDefOf.Dryad_Gaumaker)
				{
					this.tmpDryads.Add(this.dryads[i]);
				}
			}
			if (this.tmpDryads.Count < 3)
			{
				this.tmpDryads.Clear();
				return false;
			}
			this.tmpDryads.SortBy((Pawn x) => -x.ageTracker.AgeChronologicalTicks);
			for (int j = 0; j < 3; j++)
			{
				if (this.tmpDryads[j] == dryad)
				{
					this.tmpDryads.Clear();
					return true;
				}
			}
			this.tmpDryads.Clear();
			return false;
		}

		// Token: 0x06010D82 RID: 68994 RVA: 0x004D98A4 File Offset: 0x004D7AA4
		private void TearConnection()
		{
			Messages.Message("MessageConnectedPawnDied".Translate(this.parent.Named("TREE"), this.ConnectedPawn.Named("PAWN"), 1800000.ToStringTicksToDays("F1").Named("DURATION")), this.parent, MessageTypeDefOf.NegativeEvent, true);
			for (int i = 0; i < this.dryads.Count; i++)
			{
				this.ResetDryad(this.dryads[i]);
			}
			SoundDefOf.GauranlenConnectionTorn.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
			this.nextUntornTick = Find.TickManager.TicksGame + 1800000;
			this.connectedPawn = null;
			this.currentMode = null;
		}

		// Token: 0x06010D83 RID: 68995 RVA: 0x004D9978 File Offset: 0x004D7B78
		private void SpawnDryad()
		{
			this.spawnTick = Find.TickManager.TicksGame + (int)(60000f * this.Props.spawnDays);
			Pawn pawn = this.GenerateNewDryad(this.Props.pawnKind);
			GenSpawn.Spawn(pawn, this.parent.Position, this.parent.Map, WipeMode.Vanish).Rotation = Rot4.South;
			EffecterDefOf.DryadSpawn.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			SoundDefOf.Pawn_Dryad_Spawn.PlayOneShot(SoundInfo.InMap(pawn, MaintenanceType.None));
		}

		// Token: 0x06010D84 RID: 68996 RVA: 0x004D9A24 File Offset: 0x004D7C24
		public Pawn GenerateNewDryad(PawnKindDef dryadCaste)
		{
			Faction faction = null;
			PawnGenerationContext context = PawnGenerationContext.NonPlayer;
			Gender? fixedGender = new Gender?(Gender.Male);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(dryadCaste, faction, context, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, fixedGender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn, null, null, null, false, false, false, -1, 0, false));
			this.ResetDryad(pawn);
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections != null)
			{
				connections.ConnectTo(this.parent);
			}
			this.dryads.Add(pawn);
			return pawn;
		}

		// Token: 0x06010D85 RID: 68997 RVA: 0x004D9AEC File Offset: 0x004D7CEC
		private void ResetDryad(Pawn dryad)
		{
			if (this.Connected)
			{
				Faction faction = dryad.Faction;
				Pawn pawn = this.ConnectedPawn;
				if (faction != ((pawn != null) ? pawn.Faction : null))
				{
					Pawn pawn2 = this.ConnectedPawn;
					dryad.SetFaction((pawn2 != null) ? pawn2.Faction : null, null);
				}
			}
			if (dryad.training != null)
			{
				foreach (TrainableDef trainableDef in DefDatabase<TrainableDef>.AllDefs)
				{
					if (dryad.training.CanAssignToTrain(trainableDef).Accepted)
					{
						dryad.training.SetWantedRecursive(trainableDef, true);
						dryad.training.Train(trainableDef, this.ConnectedPawn, true);
						if (trainableDef == TrainableDefOf.Release)
						{
							dryad.playerSettings.followDrafted = true;
						}
					}
				}
			}
		}

		// Token: 0x06010D86 RID: 68998 RVA: 0x004D9BC0 File Offset: 0x004D7DC0
		public void Prune(int delta)
		{
			this.lastPrunedTick = Find.TickManager.TicksGame;
			this.ConnectionStrength += this.ConnectionStrengthGainPerHourOfPruning * (float)delta / 2500f;
		}

		// Token: 0x06010D87 RID: 68999 RVA: 0x004D9BF0 File Offset: 0x004D7DF0
		public bool ShouldBePrunedNow(bool forced)
		{
			if (this.ConnectionStrength >= this.desiredConnectionStrength)
			{
				return false;
			}
			if (!forced)
			{
				if (this.ConnectionStrength >= this.desiredConnectionStrength - 0.03f)
				{
					return false;
				}
				if ((float)Find.TickManager.TicksGame < (float)this.lastPrunedTick + this.TimeBetweenAutoPruning)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06010D88 RID: 69000 RVA: 0x004D9C44 File Offset: 0x004D7E44
		private float ClosestDistanceToBlockingBuilding(List<Thing> buildings)
		{
			float num = float.PositiveInfinity;
			for (int i = 0; i < buildings.Count; i++)
			{
				float num2 = buildings[i].Position.DistanceTo(this.parent.Position);
				if (num2 < num)
				{
					num = num2;
				}
			}
			return num;
		}

		// Token: 0x06010D89 RID: 69001 RVA: 0x004D9C8C File Offset: 0x004D7E8C
		private bool TryGetGaumakerCell(out IntVec3 cell)
		{
			cell = IntVec3.Invalid;
			return CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 3, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, this.parent.Map, ThingDefOf.Plant_PodGauranlen), out cell, -1) || CellFinder.TryFindRandomCellNear(this.parent.Position, this.parent.Map, 3, (IntVec3 c) => GauranlenUtility.CocoonAndPodCellValidator(c, this.parent.Map, ThingDefOf.Plant_TreeGauranlen), out cell, -1);
		}

		// Token: 0x06010D8A RID: 69002 RVA: 0x004D9CFF File Offset: 0x004D7EFF
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.Connected)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "ChangeMode".Translate();
				command_Action.defaultDesc = "ChangeModeDesc".Translate(this.parent.Named("TREE"));
				command_Action.icon = ((this.Mode == null) ? ContentFinder<Texture2D>.Get("UI/Gizmos/UpgradeDryads", true) : Widgets.GetIconFor(this.Mode.pawnKindDef.race, null, null, null));
				command_Action.action = delegate()
				{
					Find.WindowStack.Add(new Dialog_ChangeDryadCaste(this.parent));
				};
				if (!this.ConnectedPawn.Spawned || this.ConnectedPawn.Map != this.parent.Map)
				{
					command_Action.Disable("ConnectedPawnAway".Translate(this.ConnectedPawn.Named("PAWN")));
				}
				yield return command_Action;
				if (this.pruningGizmo == null)
				{
					this.pruningGizmo = new Gizmo_PruningConfig(this);
				}
				yield return this.pruningGizmo;
			}
			if (DebugSettings.ShowDevGizmos)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Spawn dryad",
					action = delegate()
					{
						this.SpawnDryad();
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Connection strength -10%",
					action = delegate()
					{
						this.ConnectionStrength -= 0.1f;
					}
				};
				yield return new Command_Action
				{
					defaultLabel = "DEV: Connection strength +10%",
					action = delegate()
					{
						this.ConnectionStrength += 0.1f;
					}
				};
			}
			yield break;
		}

		// Token: 0x06010D8B RID: 69003 RVA: 0x004D9D10 File Offset: 0x004D7F10
		public override void PostDraw()
		{
			if (this.dryads.Count < this.MaxDryads)
			{
				Matrix4x4 matrix = default(Matrix4x4);
				Vector3 pos = this.parent.Position.ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop.AltitudeFor()) + this.Props.spawningPodOffset;
				float num = this.Props.spawningPodSizeRange.LerpThroughRange(1f - (float)this.spawnTick - (float)Find.TickManager.TicksGame / (float)this.SpawningDurationTicks);
				matrix.SetTRS(pos, Quaternion.identity, new Vector3(num, 1f, num));
				Graphics.DrawMesh(MeshPool.plane10, matrix, this.PodMat, 0);
			}
		}

		// Token: 0x06010D8C RID: 69004 RVA: 0x004D9DC3 File Offset: 0x004D7FC3
		public override void PostDrawExtraSelectionOverlays()
		{
			Pawn pawn = this.connectedPawn;
			if (pawn == null)
			{
				return;
			}
			Pawn_ConnectionsTracker connections = pawn.connections;
			if (connections == null)
			{
				return;
			}
			connections.DrawConnectionLine(this.parent);
		}

		// Token: 0x06010D8D RID: 69005 RVA: 0x004D9DE8 File Offset: 0x004D7FE8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			string text2 = string.Empty;
			if (this.dryads.Count < this.MaxDryads)
			{
				text2 = "SpawningDryadIn".Translate(this.Props.pawnKind.Named("DRYAD"), (this.spawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false).Named("TIME")).Resolve();
			}
			if (this.ConnectionTorn)
			{
				text += "ConnectionTorn".Translate(this.UntornInDurationTicks.ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
			}
			else
			{
				text = text + "ConnectedPawn".Translate().Resolve() + ": " + (this.Connected ? this.connectedPawn.NameFullColored : "Nobody".Translate().CapitalizeFirst()).Resolve();
			}
			if (this.Connected)
			{
				if (this.lastPrunedTick >= 0 && Find.TickManager.TicksGame - this.lastPrunedTick <= 60)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"PruningConnectionStrength".Translate(),
						": ",
						"PerHour".Translate(this.ConnectionStrengthGainPerHourOfPruning.ToStringPercent()).Resolve()
					});
				}
				if (this.Mode != null)
				{
					text += "\n" + "GauranlenTreeMode".Translate() + ": " + this.Mode.LabelCap;
				}
				if (this.HasProductionMode && this.Mode != this.desiredMode)
				{
					text = text + "\n" + "WaitingForConnectorToChangeCaste".Translate(this.ConnectedPawn.Named("CONNECTEDPAWN")).Resolve();
				}
				if (!text2.NullOrEmpty())
				{
					text = text + "\n" + text2;
				}
				if (this.MaxDryads > 0)
				{
					text = text + "\n" + "DryadPlural".Translate() + string.Format(" ({0}/{1})", this.dryads.Count, this.MaxDryads);
					if (this.dryads.Count > 0)
					{
						text = text + ": " + (from x in this.dryads
						select x.NameShortColored.Resolve()).ToCommaList(false, false).CapitalizeFirst();
					}
				}
				else
				{
					text = text + "\n" + "NotEnoughConnectionStrengthForSingleDryad".Translate(this.MinConnectionStrengthForSingleDryad.ToStringPercent()).Colorize(ColorLibrary.RedReadable);
				}
				if (!this.HasProductionMode)
				{
					text = text + "\n" + "AlertGauranlenTreeWithoutDryadTypeLabel".Translate().Colorize(ColorLibrary.RedReadable);
				}
				if (this.Mode == GauranlenTreeModeDefOf.Gaumaker && this.MaxDryads < 3)
				{
					text = text + "\n" + "ConnectionStrengthTooWeakForGaumakerPod".Translate().Colorize(ColorLibrary.RedReadable);
				}
				string text3 = this.AffectingBuildingsDescription("ConnectionStrengthAffectedBy");
				if (!text3.NullOrEmpty())
				{
					text = text + "\n" + text3;
				}
			}
			else if (!text2.NullOrEmpty())
			{
				text = text + "\n" + text2;
			}
			return text;
		}

		// Token: 0x06010D8E RID: 69006 RVA: 0x004DA17C File Offset: 0x004D837C
		public string AffectingBuildingsDescription(string descKey)
		{
			List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
			if (buildingsReducingConnectionStrength.Count > 0)
			{
				IEnumerable<string> source = (from c in buildingsReducingConnectionStrength
				select GenLabel.ThingLabel(c, 1, false, true)).Distinct<string>();
				TaggedString taggedString = descKey.Translate() + ": " + source.Take(3).ToCommaList(false, false).CapitalizeFirst();
				if (source.Count<string>() > 3)
				{
					taggedString += " " + "Etc".Translate();
				}
				return taggedString;
			}
			return null;
		}

		// Token: 0x06010D8F RID: 69007 RVA: 0x004DA219 File Offset: 0x004D8419
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "ConnectedPawn".Translate(), (this.ConnectedPawn != null) ? this.ConnectedPawn.NameFullColored : "Nobody".Translate(), "ConnectedPawnDesc".Translate(1800000.ToStringTicksToPeriod(true, false, true, true, false).Named("DURATION"), this.parent.Named("TREE")), 6010, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.ConnectedPawn, -1, false)), false, false);
			if (this.Connected)
			{
				if (this.Mode != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "GauranlenTreeMode".Translate(), this.currentMode.LabelCap, "GauranlenTreeModeDesc".Translate() + "\n\n" + this.currentMode.LabelCap + ": " + this.currentMode.Description, 5990, null, Gen.YieldSingle<Dialog_InfoCard.Hyperlink>(new Dialog_InfoCard.Hyperlink(this.currentMode.pawnKindDef.race, -1)), false, false);
				}
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "ConnectionStrength".Translate(), this.ConnectionStrength.ToStringPercent(), "ConnectionStrengthDesc".Translate(), 6000, null, null, false, false);
			}
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "MaximumDryads".Translate(), this.MaxDryads.ToString(), "MaximumDryadsDesc".Translate() + "\n\n" + this.ConnectionStrengthToMaxDryadsDesc(), 5980, null, null, false, false);
			yield break;
		}

		// Token: 0x06010D90 RID: 69008 RVA: 0x004DA22C File Offset: 0x004D842C
		private string ConnectionStrengthToMaxDryadsDesc()
		{
			string text = "MaxDryadsBasedOnConnectionStrength".Translate() + ":\n -  " + "Unconnected".Translate() + ": " + this.Props.maxDryadsWild.ToString();
			foreach (CurvePoint curvePoint in this.Props.maxDryadsPerConnectionStrengthCurve)
			{
				text = text + ("\n -  " + "ConnectionStrengthDisplay".Translate(curvePoint.x.ToStringPercent()) + ": ") + curvePoint.y.ToString();
			}
			return text;
		}

		// Token: 0x06010D91 RID: 69009 RVA: 0x004DA30C File Offset: 0x004D850C
		public float PruningHoursToMaintain(float desired)
		{
			float num = this.Props.connectionLossPerLevelCurve.Evaluate(desired);
			List<Thing> buildingsReducingConnectionStrength = this.BuildingsReducingConnectionStrength;
			if (buildingsReducingConnectionStrength.Any<Thing>())
			{
				num += this.Props.connectionLossDailyPerBuildingDistanceCurve.Evaluate(this.ClosestDistanceToBlockingBuilding(buildingsReducingConnectionStrength));
			}
			return num / this.ConnectionStrengthGainPerHourOfPruning;
		}

		// Token: 0x06010D92 RID: 69010 RVA: 0x004DA35C File Offset: 0x004D855C
		public bool WillBeAffectedBy(ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			return MeditationUtility.CountsAsArtificialBuilding(def, faction) && GenAdj.OccupiedRect(pos, rotation, def.size).ClosestCellTo(this.parent.Position).InHorDistOf(this.parent.Position, this.Props.radiusToBuildingForConnectionStrengthLoss);
		}

		// Token: 0x06010D93 RID: 69011 RVA: 0x004DA3B8 File Offset: 0x004D85B8
		public override void PostExposeData()
		{
			Scribe_Defs.Look<GauranlenTreeModeDef>(ref this.currentMode, "currentMode");
			Scribe_Defs.Look<GauranlenTreeModeDef>(ref this.desiredMode, "desiredMode");
			Scribe_Values.Look<int>(ref this.nextUntornTick, "nextUntornTick", -1, false);
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", -1, false);
			Scribe_Values.Look<int>(ref this.lastPrunedTick, "lastPrunedTick", 0, false);
			Scribe_Values.Look<float>(ref this.desiredConnectionStrength, "desiredConnectionStrength", 0.5f, false);
			Scribe_Values.Look<float>(ref this.connectionStrength, "connectionStrength", 0f, false);
			Scribe_References.Look<Pawn>(ref this.connectedPawn, "connectedPawn", false);
			Scribe_References.Look<Thing>(ref this.gaumakerPod, "gaumakerPod", false);
			Scribe_Collections.Look<Pawn>(ref this.dryads, "dryads", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.dryads.RemoveAll((Pawn x) => x == null || x.Dead);
			}
		}

		// Token: 0x0400AF52 RID: 44882
		private Pawn connectedPawn;

		// Token: 0x0400AF53 RID: 44883
		private int nextUntornTick = -1;

		// Token: 0x0400AF54 RID: 44884
		private int spawnTick = -1;

		// Token: 0x0400AF55 RID: 44885
		private float connectionStrength;

		// Token: 0x0400AF56 RID: 44886
		private List<Pawn> dryads = new List<Pawn>();

		// Token: 0x0400AF57 RID: 44887
		private int lastPrunedTick;

		// Token: 0x0400AF58 RID: 44888
		private float desiredConnectionStrength = 0.5f;

		// Token: 0x0400AF59 RID: 44889
		private GauranlenTreeModeDef currentMode;

		// Token: 0x0400AF5A RID: 44890
		public Thing gaumakerPod;

		// Token: 0x0400AF5B RID: 44891
		public GauranlenTreeModeDef desiredMode;

		// Token: 0x0400AF5C RID: 44892
		private Material cachedPodMat;

		// Token: 0x0400AF5D RID: 44893
		private Effecter leafEffecter;

		// Token: 0x0400AF5E RID: 44894
		private Gizmo_PruningConfig pruningGizmo;

		// Token: 0x0400AF5F RID: 44895
		private const int ConnectionTornDurationTicks = 1800000;

		// Token: 0x0400AF60 RID: 44896
		private const int CheckPodSpawnInterval = 300;

		// Token: 0x0400AF61 RID: 44897
		public const int DryadsToCreatePod = 3;

		// Token: 0x0400AF62 RID: 44898
		private float TimeBetweenAutoPruning = 10000f;

		// Token: 0x0400AF63 RID: 44899
		private const float PruningConnectionStrengthDithering = 0.03f;

		// Token: 0x0400AF64 RID: 44900
		private const float PruningSpeedFactor_DisabledSkill = 0.75f;

		// Token: 0x0400AF65 RID: 44901
		private List<Pawn> tmpDryads = new List<Pawn>();
	}
}
