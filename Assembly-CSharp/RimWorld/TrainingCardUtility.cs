using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003255 RID: 12885
	[StaticConstructorOnStartup]
	public static class TrainingCardUtility
	{
		// Token: 0x060121E9 RID: 74217 RVA: 0x0054E0F8 File Offset: 0x0054C2F8
		public static void DrawTrainingCard(Rect rect, Pawn pawn)
		{
			Text.Font = GameFont.Small;
			RenameUIUtility.DrawRenameButton(new Rect(TrainingCardUtility.TrainabilityLeft, TrainingCardUtility.TrainabilityTop, 30f, 30f), pawn);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.Begin(rect);
			TrainabilityDef trainability = TrainableUtility.GetTrainability(pawn);
			if (trainability != null)
			{
				listing_Standard.Label("CreatureTrainability".Translate(pawn.def.label).CapitalizeFirst() + ": " + trainability.LabelCap, 22f, null);
			}
			float statValue = pawn.GetStatValue(StatDefOf.Wildness, true, -1);
			listing_Standard.Label("CreatureWildness".Translate(pawn.def.label).CapitalizeFirst() + ": " + statValue.ToStringPercent(), 22f, StatDefOf.Wildness.Worker.GetExplanationFull(StatRequest.For(pawn), StatDefOf.Wildness.toStringNumberSense, statValue));
			if (pawn.training.HasLearned(TrainableDefOf.Obedience))
			{
				Rect rect2 = listing_Standard.GetRect(25f, 1f);
				Widgets.Label(rect2, "Master".Translate() + ": ");
				rect2.xMin = rect2.center.x;
				if (pawn.RaceProps.playerCanChangeMaster || !ModsConfig.IdeologyActive)
				{
					TrainableUtility.MasterSelectButton(rect2, pawn, false);
				}
				else
				{
					Pawn_PlayerSettings playerSettings = pawn.playerSettings;
					if (((playerSettings != null) ? playerSettings.Master : null) != null)
					{
						Widgets.Label(rect2, TrainableUtility.MasterString(pawn).Truncate(rect2.width, null));
						TooltipHandler.TipRegion(rect2, "DryadCannotChangeMaster".Translate(pawn.Named("ANIMAL"), pawn.playerSettings.Master.Named("MASTER")).CapitalizeFirst());
					}
				}
				listing_Standard.Gap(12f);
				Rect rect3 = listing_Standard.GetRect(25f, 1f);
				bool followDrafted = pawn.playerSettings.followDrafted;
				Widgets.CheckboxLabeled(rect3, "CreatureFollowDrafted".Translate(), ref followDrafted, false, null, null, false, true);
				pawn.playerSettings.followDrafted = followDrafted;
				Rect rect4 = listing_Standard.GetRect(25f, 1f);
				bool followFieldwork = pawn.playerSettings.followFieldwork;
				Widgets.CheckboxLabeled(rect4, "CreatureFollowFieldwork".Translate(), ref followFieldwork, false, null, null, false, true);
				pawn.playerSettings.followFieldwork = followFieldwork;
			}
			if (ModsConfig.OdysseyActive)
			{
				if (pawn.training.HasLearned(TrainableDefOf.Forage))
				{
					Rect rect5 = listing_Standard.GetRect(25f, 1f);
					bool animalForage = pawn.playerSettings.animalForage;
					Widgets.CheckboxLabeled(rect5, "ForageEnabled".Translate(), ref animalForage, false, null, null, false, true);
					pawn.playerSettings.animalForage = animalForage;
				}
				if (pawn.training.HasLearned(TrainableDefOf.Dig))
				{
					Rect rect6 = listing_Standard.GetRect(25f, 1f);
					bool animalDig = pawn.playerSettings.animalDig;
					Widgets.CheckboxLabeled(rect6, "DigEnabled".Translate(), ref animalDig, false, null, null, false, true);
					pawn.playerSettings.animalDig = animalDig;
				}
			}
			if (pawn.RaceProps.showTrainables)
			{
				listing_Standard.Gap(12f);
				foreach (TrainableDef td in TrainableUtility.TrainableDefsInListOrder)
				{
					TrainingCardUtility.TryDrawTrainableRow(listing_Standard, pawn, td);
				}
			}
			listing_Standard.End();
		}

		// Token: 0x060121EA RID: 74218 RVA: 0x0054E48C File Offset: 0x0054C68C
		public static float TotalHeightForPawn(Pawn p)
		{
			if (p == null)
			{
				return 0f;
			}
			int num = 0;
			if (p.RaceProps.showTrainables)
			{
				for (int i = 0; i < DefDatabase<TrainableDef>.AllDefsListForReading.Count; i++)
				{
					bool flag;
					p.training.CanAssignToTrain(DefDatabase<TrainableDef>.AllDefsListForReading[i], out flag);
					if (flag)
					{
						num++;
					}
				}
			}
			float num2 = 112f + 28f * (float)num;
			if (p.training.HasLearned(TrainableDefOf.Obedience))
			{
				num2 += 75f;
				num2 += 12f;
			}
			if (ModsConfig.OdysseyActive)
			{
				if (p.training.HasLearned(TrainableDefOf.Forage))
				{
					num2 += 25f;
				}
				if (p.training.HasLearned(TrainableDefOf.Dig))
				{
					num2 += 25f;
				}
			}
			return num2;
		}

		// Token: 0x060121EB RID: 74219 RVA: 0x0054E554 File Offset: 0x0054C754
		private static void TryDrawTrainableRow(Listing listing, Pawn pawn, TrainableDef td)
		{
			bool flag = pawn.training.HasLearned(td);
			bool flag2;
			AcceptanceReport canTrain = pawn.training.CanAssignToTrain(td, out flag2);
			if (!flag2)
			{
				return;
			}
			Rect rect = listing.GetRect(28f, 1f);
			Widgets.DrawHighlightIfMouseover(rect);
			Rect rect2 = rect;
			rect2.width -= 50f;
			rect2.xMin += (float)td.indent * 10f;
			Rect rect3 = rect;
			rect3.xMin = rect3.xMax - 50f + 17f;
			TrainingCardUtility.DoTrainableCheckbox(rect2, pawn, td, canTrain, true, false);
			if (flag)
			{
				GUI.color = Color.green;
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, pawn.training.GetSteps(td).ToString() + " / " + td.steps.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			if (DebugSettings.godMode && !pawn.training.HasLearned(td))
			{
				Rect rect4 = rect3;
				rect4.yMin = rect4.yMax - 10f;
				rect4.xMin = rect4.xMax - 10f;
				if (Widgets.ButtonText(rect4, "+", true, true, true, null))
				{
					pawn.training.Train(td, pawn.Map.mapPawns.FreeColonistsSpawned.RandomElement<Pawn>(), false);
				}
			}
			TrainingCardUtility.DoTrainableTooltip(rect, pawn, td, canTrain);
			GUI.color = Color.white;
		}

		// Token: 0x060121EC RID: 74220 RVA: 0x0054E6CC File Offset: 0x0054C8CC
		public static void DoTrainableCheckbox(Rect rect, Pawn pawn, TrainableDef td, AcceptanceReport canTrain, bool drawLabel, bool doTooltip)
		{
			bool flag = pawn.training.HasLearned(td);
			bool wanted = pawn.training.GetWanted(td);
			bool flag2 = wanted;
			Texture2D texChecked = flag ? TrainingCardUtility.LearnedTrainingTex : null;
			Texture2D texUnchecked = flag ? TrainingCardUtility.LearnedNotTrainingTex : null;
			if (drawLabel)
			{
				Widgets.CheckboxLabeled(rect, td.LabelCap, ref wanted, !canTrain.Accepted, texChecked, texUnchecked, false, true);
			}
			else
			{
				Widgets.Checkbox(rect.position, ref wanted, rect.width, !canTrain.Accepted, true, texChecked, texUnchecked);
			}
			if (wanted != flag2)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTraining, KnowledgeAmount.Total);
				pawn.training.SetWantedRecursive(td, wanted);
			}
			if (doTooltip)
			{
				TrainingCardUtility.DoTrainableTooltip(rect, pawn, td, canTrain);
			}
		}

		// Token: 0x060121ED RID: 74221 RVA: 0x0054E780 File Offset: 0x0054C980
		private static void DoTrainableTooltip(Rect rect, Pawn pawn, TrainableDef td, AcceptanceReport canTrain)
		{
			if (!Mouse.IsOver(rect))
			{
				return;
			}
			TooltipHandler.TipRegion(rect, delegate()
			{
				string text = td.LabelCap + "\n\n" + td.description;
				if (!canTrain.Accepted)
				{
					text = text + "\n\n" + canTrain.Reason;
				}
				else if (!td.prerequisites.NullOrEmpty<TrainableDef>())
				{
					text += "\n";
					for (int i = 0; i < td.prerequisites.Count; i++)
					{
						if (!pawn.training.HasLearned(td.prerequisites[i]))
						{
							text += "\n" + "TrainingNeedsPrerequisite".Translate(td.prerequisites[i].LabelCap);
						}
					}
				}
				return text;
			}, (int)(rect.y * 612f + rect.x));
		}

		// Token: 0x0400BC32 RID: 48178
		public const float RowHeight = 28f;

		// Token: 0x0400BC33 RID: 48179
		public const float RenameButtonSize = 30f;

		// Token: 0x0400BC34 RID: 48180
		private const float InfoHeaderHeight = 50f;

		// Token: 0x0400BC35 RID: 48181
		[TweakValue("Interface", -100f, 300f)]
		private static float TrainabilityLeft = 220f;

		// Token: 0x0400BC36 RID: 48182
		[TweakValue("Interface", -100f, 300f)]
		private static float TrainabilityTop = 0f;

		// Token: 0x0400BC37 RID: 48183
		public static readonly Texture2D LearnedTrainingTex = ContentFinder<Texture2D>.Get("UI/Icons/FixedCheck", true);

		// Token: 0x0400BC38 RID: 48184
		public static readonly Texture2D LearnedNotTrainingTex = ContentFinder<Texture2D>.Get("UI/Icons/FixedCheckOff", true);
	}
}
