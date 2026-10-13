using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020024E9 RID: 9449
	[StaticConstructorOnStartup]
	public class Pawn_TrainingTracker : IExposable
	{
		// Token: 0x0600CE6E RID: 52846 RVA: 0x003C9D17 File Offset: 0x003C7F17
		public Pawn_TrainingTracker(Pawn pawn)
		{
			this.pawn = pawn;
			this.countDecayFrom = Find.TickManager.TicksGame;
		}

		// Token: 0x0600CE6F RID: 52847 RVA: 0x003C9D58 File Offset: 0x003C7F58
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.wantedTrainables, "wantedTrainables", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TrainableDef, int>>(ref this.steps, "steps", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<TrainableDef, bool>>(ref this.learned, "learned", Array.Empty<object>());
			Scribe_References.Look<Thing>(ref this.attackTarget, "attackTarget", false);
			Scribe_Values.Look<int>(ref this.countDecayFrom, "countDecayFrom", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.PawnTrainingTrackerPostLoadInit(this, ref this.wantedTrainables, ref this.steps, ref this.learned);
			}
		}

		// Token: 0x0600CE70 RID: 52848 RVA: 0x003C9DE7 File Offset: 0x003C7FE7
		public bool GetWanted(TrainableDef td)
		{
			return this.wantedTrainables[td];
		}

		// Token: 0x0600CE71 RID: 52849 RVA: 0x003C9DF5 File Offset: 0x003C7FF5
		private void SetWanted(TrainableDef td, bool wanted)
		{
			this.wantedTrainables[td] = wanted;
		}

		// Token: 0x0600CE72 RID: 52850 RVA: 0x003C9E04 File Offset: 0x003C8004
		internal int GetSteps(TrainableDef td)
		{
			return this.steps[td];
		}

		// Token: 0x0600CE73 RID: 52851 RVA: 0x003C9E14 File Offset: 0x003C8014
		public bool CanBeTrained(TrainableDef td)
		{
			if (this.steps[td] >= td.steps)
			{
				return false;
			}
			List<TrainableDef> prerequisites = td.prerequisites;
			if (!prerequisites.NullOrEmpty<TrainableDef>())
			{
				for (int i = 0; i < prerequisites.Count; i++)
				{
					if (!this.HasLearned(prerequisites[i]) || this.CanBeTrained(prerequisites[i]))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600CE74 RID: 52852 RVA: 0x003C9E78 File Offset: 0x003C8078
		public bool HasLearned(TrainableDef td)
		{
			TrainabilityDef trainability = TrainableUtility.GetTrainability(this.pawn);
			if (td.requiredTrainability != null && trainability != null)
			{
				int intelligenceOrder = trainability.intelligenceOrder;
				TrainabilityDef requiredTrainability = td.requiredTrainability;
				int? num = (requiredTrainability != null) ? new int?(requiredTrainability.intelligenceOrder) : null;
				if (intelligenceOrder < num.GetValueOrDefault() & num != null)
				{
					return false;
				}
			}
			return this.learned[td];
		}

		// Token: 0x0600CE75 RID: 52853 RVA: 0x003C9EE4 File Offset: 0x003C80E4
		public AcceptanceReport CanAssignToTrain(TrainableDef td)
		{
			bool flag;
			return this.CanAssignToTrain(td, out flag);
		}

		// Token: 0x0600CE76 RID: 52854 RVA: 0x003C9EFA File Offset: 0x003C80FA
		public AcceptanceReport CanAssignToTrain(TrainableDef td, out bool visible)
		{
			return Pawn_TrainingTracker.CanAssignToTrain(td, this.pawn.def, out visible, this.pawn);
		}

		// Token: 0x0600CE77 RID: 52855 RVA: 0x003C9F14 File Offset: 0x003C8114
		public static AcceptanceReport CanAssignToTrain(TrainableDef td, ThingDef pawnDef, out bool visible, Pawn pawn = null)
		{
			if (pawnDef.race.untrainableTags != null)
			{
				for (int i = 0; i < pawnDef.race.untrainableTags.Count; i++)
				{
					if (td.MatchesTag(pawnDef.race.untrainableTags[i]))
					{
						visible = false;
						return false;
					}
				}
			}
			if (ModsConfig.OdysseyActive && td.specialTrainable)
			{
				List<TrainableDef> specialTrainables = pawnDef.race.specialTrainables;
				if (specialTrainables == null || !specialTrainables.Contains(td))
				{
					visible = false;
					return false;
				}
			}
			float num = (pawn != null) ? pawn.BodySize : pawnDef.race.baseBodySize;
			string value = ((pawn != null) ? pawn.LabelShort : null) ?? pawnDef.label;
			TrainabilityDef trainabilityDef = TrainableUtility.GetTrainability(pawn) ?? pawnDef.race.trainability;
			if (pawnDef.race.trainableTags != null)
			{
				int j = 0;
				while (j < pawnDef.race.trainableTags.Count)
				{
					if (td.MatchesTag(pawnDef.race.trainableTags[j]))
					{
						if (num < td.minBodySize)
						{
							visible = true;
							return new AcceptanceReport("CannotTrainTooSmall".Translate(value).Resolve());
						}
						visible = true;
						return true;
					}
					else
					{
						j++;
					}
				}
			}
			if (!td.defaultTrainable && !td.specialTrainable)
			{
				visible = false;
				return false;
			}
			if (num < td.minBodySize)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainTooSmall".Translate(value).Resolve());
			}
			if (td.requiredTrainability != null && trainabilityDef.intelligenceOrder < td.requiredTrainability.intelligenceOrder)
			{
				visible = true;
				return new AcceptanceReport("CannotTrainNotSmartEnough".Translate(td.requiredTrainability.label).Resolve());
			}
			visible = true;
			return true;
		}

		// Token: 0x0600CE78 RID: 52856 RVA: 0x003CA0F8 File Offset: 0x003C82F8
		public TrainableDef NextTrainableToTrain()
		{
			List<TrainableDef> trainableDefsInListOrder = TrainableUtility.TrainableDefsInListOrder;
			for (int i = 0; i < trainableDefsInListOrder.Count; i++)
			{
				if (this.GetWanted(trainableDefsInListOrder[i]) && this.CanBeTrained(trainableDefsInListOrder[i]))
				{
					return trainableDefsInListOrder[i];
				}
			}
			return null;
		}

		// Token: 0x0600CE79 RID: 52857 RVA: 0x003CA144 File Offset: 0x003C8344
		public void Train(TrainableDef td, Pawn trainer, bool complete = false)
		{
			if (complete)
			{
				this.steps[td] = td.steps;
			}
			else
			{
				DefMap<TrainableDef, int> defMap = this.steps;
				int num = defMap[td];
				defMap[td] = num + 1;
			}
			if (this.steps[td] >= td.steps)
			{
				this.learned[td] = true;
				if (td == TrainableDefOf.Obedience && trainer != null && this.pawn.playerSettings != null && this.pawn.playerSettings.Master == null)
				{
					this.pawn.playerSettings.Master = trainer;
				}
			}
		}

		// Token: 0x0600CE7A RID: 52858 RVA: 0x003CA1E0 File Offset: 0x003C83E0
		public void SetWantedRecursive(TrainableDef td, bool checkOn)
		{
			this.SetWanted(td, checkOn);
			if (checkOn)
			{
				if (td.prerequisites != null)
				{
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						this.SetWantedRecursive(td.prerequisites[i], true);
					}
					return;
				}
			}
			else
			{
				foreach (TrainableDef td2 in from t in DefDatabase<TrainableDef>.AllDefsListForReading
				where t.prerequisites != null && t.prerequisites.Contains(td)
				select t)
				{
					this.SetWantedRecursive(td2, false);
				}
			}
		}

		// Token: 0x0600CE7B RID: 52859 RVA: 0x003CA29C File Offset: 0x003C849C
		public void TrainingTrackerTickRare()
		{
			if (this.pawn.Suspended)
			{
				this.countDecayFrom += 250;
				return;
			}
			if (!this.pawn.Spawned)
			{
				this.countDecayFrom += 250;
				return;
			}
			if (this.steps[TrainableDefOf.Tameness] == 0)
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
				return;
			}
			if (Find.TickManager.TicksGame < this.countDecayFrom + TrainableUtility.DegradationPeriodTicks(this.pawn))
			{
				return;
			}
			if (this.pawn.RaceProps.animalType == AnimalType.Dryad)
			{
				return;
			}
			TrainableDef trainableDef = (from kvp in this.steps
			where kvp.Value > 0
			select kvp.Key).Except((from kvp in this.steps
			where kvp.Value > 0 && kvp.Key.prerequisites != null
			select kvp).SelectMany((KeyValuePair<TrainableDef, int> kvp) => kvp.Key.prerequisites)).RandomElement<TrainableDef>();
			if (trainableDef == TrainableDefOf.Tameness && !TrainableUtility.TamenessCanDecay(this.pawn))
			{
				this.countDecayFrom = Find.TickManager.TicksGame;
				return;
			}
			this.countDecayFrom = Find.TickManager.TicksGame;
			DefMap<TrainableDef, int> defMap = this.steps;
			TrainableDef def = trainableDef;
			int value = defMap[def] - 1;
			defMap[def] = value;
			if (this.steps[trainableDef] <= 0 && this.learned[trainableDef])
			{
				this.learned[trainableDef] = false;
				if (this.pawn.Faction == Faction.OfPlayer)
				{
					if (trainableDef == TrainableDefOf.Tameness)
					{
						this.pawn.SetFaction(null, null);
						Messages.Message("MessageAnimalReturnedWild".Translate(this.pawn.LabelShort, this.pawn), this.pawn, MessageTypeDefOf.NegativeEvent, true);
						return;
					}
					Messages.Message("MessageAnimalLostSkill".Translate(this.pawn.LabelShort, trainableDef.LabelCap, this.pawn.Named("ANIMAL")), this.pawn, MessageTypeDefOf.NegativeEvent, true);
				}
			}
		}

		// Token: 0x0600CE7C RID: 52860 RVA: 0x003CA520 File Offset: 0x003C8720
		private static bool CanEverDoAttackTarget(Pawn pawn)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			Pawn_TrainingTracker training = pawn.training;
			return training != null && training.HasLearned(TrainableDefOf.AttackTarget);
		}

		// Token: 0x0600CE7D RID: 52861 RVA: 0x003CA544 File Offset: 0x003C8744
		private static AcceptanceReport CanDoAttackTarget(Pawn pawn)
		{
			if (!Pawn_TrainingTracker.CanEverDoAttackTarget(pawn))
			{
				return false;
			}
			if (pawn.playerSettings.Master == null)
			{
				return "NoMaster".Translate();
			}
			if (!pawn.playerSettings.Master.Spawned || pawn.playerSettings.Master.Map != pawn.Map)
			{
				return "MasterNotSpawned".Translate();
			}
			return true;
		}

		// Token: 0x0600CE7E RID: 52862 RVA: 0x003CA5BD File Offset: 0x003C87BD
		public IEnumerable<Gizmo> GetGizmos()
		{
			if (!Pawn_TrainingTracker.CanEverDoAttackTarget(this.pawn))
			{
				yield break;
			}
			Pawn master = this.pawn.playerSettings.Master;
			AcceptanceReport report = Pawn_TrainingTracker.CanDoAttackTarget(this.pawn);
			yield return new Command_Target
			{
				defaultLabel = "AnimalAttackTarget".Translate(),
				defaultDesc = "AnimalAttackTargetDesc".Translate(),
				targetingParams = TargetingParameters.ForAttackAny(),
				hotKey = KeyBindingDefOf.Misc2,
				icon = Pawn_TrainingTracker.AttackTargetTexture,
				groupKey = (report.Accepted ? 23452345 : -1),
				action = delegate(LocalTargetInfo target)
				{
					IEnumerable<Pawn> enumerable = Find.Selector.SelectedObjects.Where(delegate(object x)
					{
						Pawn pawn2 = x as Pawn;
						return pawn2 != null && Pawn_TrainingTracker.CanDoAttackTarget(pawn2);
					}).Cast<Pawn>();
					if (!target.Cell.InHorDistOf(master.Position, 25.9f))
					{
						Messages.Message("MessageNotInRangeOfMaster".Translate(), MessageTypeDefOf.RejectInput, false);
						return;
					}
					string text = null;
					foreach (Pawn pawn in enumerable)
					{
						Action meleeAttackAction = FloatMenuUtility.GetMeleeAttackAction(pawn, target, out text, true);
						if (meleeAttackAction != null)
						{
							meleeAttackAction();
						}
						pawn.training.attackTarget = target.Thing;
						pawn.mindState.enemyTarget = target.Thing;
					}
					if (!text.NullOrEmpty())
					{
						Messages.Message(text, MessageTypeDefOf.RejectInput, false);
					}
				},
				Disabled = !report.Accepted,
				disabledReason = report.Reason,
				onUpdate = delegate(LocalTargetInfo _)
				{
					if (report.Accepted)
					{
						Pawn_PlayerSettings playerSettings = this.pawn.playerSettings;
						if (((playerSettings != null) ? playerSettings.Master : null) != null)
						{
							GenDraw.DrawRadiusRing(this.pawn.playerSettings.Master.Position, 25.9f);
						}
					}
				}
			};
			if (this.attackTarget != null && this.pawn.jobs.curJob.def == JobDefOf.AttackMelee && this.pawn.jobs.curJob.targetA == this.attackTarget)
			{
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CancelAttack".Translate();
				command_Action.defaultDesc = "CancelAttackDesc".Translate();
				command_Action.icon = Pawn_TrainingTracker.CancelIcon;
				command_Action.groupKey = 76547456;
				command_Action.action = delegate()
				{
					foreach (Pawn pawn in Find.Selector.SelectedObjects.Where(delegate(object x)
					{
						Pawn pawn2 = x as Pawn;
						bool flag;
						if (pawn2 == null)
						{
							flag = (null != null);
						}
						else
						{
							Pawn_TrainingTracker training = pawn2.training;
							flag = (((training != null) ? training.attackTarget : null) != null);
						}
						return flag && pawn2.mindState.enemyTarget == pawn2.training.attackTarget;
					}).Cast<Pawn>())
					{
						pawn.training.attackTarget = null;
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				};
				yield return command_Action;
			}
			yield break;
		}

		// Token: 0x0600CE7F RID: 52863 RVA: 0x003CA5CD File Offset: 0x003C87CD
		public void Debug_MakeDegradeHappenSoon()
		{
			this.countDecayFrom = Find.TickManager.TicksGame - TrainableUtility.DegradationPeriodTicks(this.pawn) - 500;
		}

		// Token: 0x04008C74 RID: 35956
		public Pawn pawn;

		// Token: 0x04008C75 RID: 35957
		private DefMap<TrainableDef, bool> wantedTrainables = new DefMap<TrainableDef, bool>();

		// Token: 0x04008C76 RID: 35958
		private DefMap<TrainableDef, int> steps = new DefMap<TrainableDef, int>();

		// Token: 0x04008C77 RID: 35959
		private DefMap<TrainableDef, bool> learned = new DefMap<TrainableDef, bool>();

		// Token: 0x04008C78 RID: 35960
		public Thing attackTarget;

		// Token: 0x04008C79 RID: 35961
		private int countDecayFrom;

		// Token: 0x04008C7A RID: 35962
		public const float AttackTargetRange = 25.9f;

		// Token: 0x04008C7B RID: 35963
		public static readonly Texture2D AttackTargetTexture = ContentFinder<Texture2D>.Get("UI/Commands/AnimalAttack", true);

		// Token: 0x04008C7C RID: 35964
		private static readonly Texture2D CancelIcon = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
