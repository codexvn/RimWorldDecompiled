using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003138 RID: 12600
	public class Dialog_ChooseNewWanderers : Window
	{
		// Token: 0x17002E10 RID: 11792
		// (get) Token: 0x06011B7A RID: 72570 RVA: 0x0051AFFB File Offset: 0x005191FB
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(1020f, 764f);
			}
		}

		// Token: 0x17002E11 RID: 11793
		// (get) Token: 0x06011B7B RID: 72571 RVA: 0x00067615 File Offset: 0x00065815
		private static List<Pawn> StartingAndOptionalPawns
		{
			get
			{
				return Find.GameInitData.startingAndOptionalPawns;
			}
		}

		// Token: 0x17002E12 RID: 11794
		// (get) Token: 0x06011B7C RID: 72572 RVA: 0x0051B00C File Offset: 0x0051920C
		private static PawnGenerationRequest DefaultStartingPawnRequest
		{
			get
			{
				PawnKindDef basicMemberKind = Faction.OfPlayer.def.basicMemberKind;
				Faction ofPlayer = Faction.OfPlayer;
				PawnGenerationContext context = PawnGenerationContext.PlayerStarter;
				FloatRange? excludeBiologicalAgeRange = ModsConfig.BiotechActive ? new FloatRange?(Dialog_ChooseNewWanderers.ExcludeBiologicalAgeRange) : null;
				return new PawnGenerationRequest(basicMemberKind, ofPlayer, context, null, true, false, false, true, false, 50f, false, true, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, excludeBiologicalAgeRange, null, false, false, false, -1, 0, false);
			}
		}

		// Token: 0x06011B7D RID: 72573 RVA: 0x0051B0C9 File Offset: 0x005192C9
		public Dialog_ChooseNewWanderers() : base(null)
		{
			this.doCloseX = true;
			this.forcePause = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011B7E RID: 72574 RVA: 0x0051B0E8 File Offset: 0x005192E8
		public override void PreOpen()
		{
			base.PreOpen();
			Current.Game.InitData = new GameInitData();
			Find.GameInitData.startingPawnCount = 3;
			StartingPawnUtility.ClearAllStartingPawns();
			this.generationIndex = 0;
			while (Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count < 3)
			{
				StartingPawnUtility.SetGenerationRequest(this.generationIndex, Dialog_ChooseNewWanderers.DefaultStartingPawnRequest);
				StartingPawnUtility.AddNewPawn(this.generationIndex);
				this.generationIndex++;
			}
			this.curPawnIndex = 0;
		}

		// Token: 0x06011B7F RID: 72575 RVA: 0x0051B160 File Offset: 0x00519360
		public override void DoWindowContents(Rect rect)
		{
			if (!base.IsOpen)
			{
				return;
			}
			Rect rect2 = rect;
			Text.Font = GameFont.Medium;
			Widgets.Label(new Rect(0f, 0f, rect2.width, 45f), "ChooseNewWanderers".Translate());
			Text.Font = GameFont.Small;
			rect2.yMin += 45f;
			Rect rect3 = new Rect(rect.width - Dialog_ChooseNewWanderers.ConfirmButtonSize.x, rect.height - Dialog_ChooseNewWanderers.ConfirmButtonSize.y, Dialog_ChooseNewWanderers.ConfirmButtonSize.x, Dialog_ChooseNewWanderers.ConfirmButtonSize.y);
			if (Widgets.ButtonText(rect3, "Confirm".Translate(), true, true, true, null))
			{
				IncidentParms incidentParms = new IncidentParms();
				incidentParms.target = Find.AnyPlayerHomeMap;
				incidentParms.forced = true;
				Find.Storyteller.TryFire(new FiringIncident(IncidentDefOf.GameEndedWanderersJoin, null, incidentParms), false);
				Find.LetterStack.RemoveLetter(Find.LetterStack.LettersListForReading.Find((Letter letter) => letter.def == LetterDefOf.GameEnded));
				Find.GameEnder.gameEnding = false;
				Find.GameEnder.newWanderersCreatedTick = Find.TickManager.TicksGame;
				this.Close(true);
				Current.Game.InitData = null;
				return;
			}
			rect2.yMax -= rect3.height + 10f;
			Rect rect4 = rect2;
			rect4.width = 140f;
			this.DrawPawnList(rect4);
			Rect rect5 = rect2;
			rect5.xMin += 150f;
			Rect rect6 = rect5.BottomPartPixels(127f);
			rect5.yMax = rect6.yMin - 10f;
			StartingPawnUtility.DrawPortraitArea(rect5, this.curPawnIndex, true, true);
			StartingPawnUtility.DrawSkillSummaries(rect6);
		}

		// Token: 0x06011B80 RID: 72576 RVA: 0x0051B340 File Offset: 0x00519540
		private void DrawPawnList(Rect rect)
		{
			Rect rect2 = rect;
			rect2.height = 60f;
			rect2 = rect2.ContractedBy(4f);
			for (int i = 0; i < Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count; i++)
			{
				this.DoPawnRow(rect2, i);
				rect2.y += 60f;
			}
			rect2.y += 4f;
			if (Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count < 6 && Widgets.ButtonText(new Rect(rect2.x, rect2.y, rect2.width, 25f), "+", true, true, true, null))
			{
				Find.GameInitData.startingPawnCount++;
				StartingPawnUtility.AddNewPawn(this.generationIndex);
				this.generationIndex++;
			}
		}

		// Token: 0x06011B81 RID: 72577 RVA: 0x0051B41C File Offset: 0x0051961C
		private void DoPawnRow(Rect rect, int index)
		{
			Pawn pawn = Dialog_ChooseNewWanderers.StartingAndOptionalPawns[index];
			Widgets.DrawOptionBackground(rect, this.curPawnIndex == index);
			MouseoverSounds.DoRegion(rect);
			Widgets.BeginGroup(rect);
			Rect rect2 = rect.AtZero().ContractedBy(4f);
			GUI.color = new Color(1f, 1f, 1f, 0.2f);
			GUI.DrawTexture(new Rect(110f - Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.x / 2f, 40f - Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.y / 2f, Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.x, Dialog_ChooseNewWanderers.PawnSelectorPortraitSize.y), PortraitsCache.Get(pawn, Dialog_ChooseNewWanderers.PawnSelectorPortraitSize, Rot4.South, default(Vector3), 1f, true, true, true, true, null, null, false, null));
			GUI.color = Color.white;
			NameTriple nameTriple = pawn.Name as NameTriple;
			string label;
			if (nameTriple != null)
			{
				label = (string.IsNullOrEmpty(nameTriple.Nick) ? nameTriple.First : nameTriple.Nick);
			}
			else
			{
				label = pawn.LabelShort;
			}
			Widgets.Label(rect2.TopPart(0.5f).Rounded(), label);
			if (Text.CalcSize(pawn.story.TitleCap).x > rect2.width)
			{
				Widgets.Label(rect2.BottomPart(0.5f).Rounded(), pawn.story.TitleShortCap);
			}
			else
			{
				Widgets.Label(rect2.BottomPart(0.5f).Rounded(), pawn.story.TitleCap);
			}
			if (Mouse.IsOver(rect2) && Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count > 1 && Widgets.ButtonImage(new Rect(rect2.xMax - 15f, rect2.y, 15f, 15f), TexButton.Delete, true, null))
			{
				Find.GameInitData.startingPawnCount--;
				Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Remove(pawn);
				this.curPawnIndex = Math.Min(Dialog_ChooseNewWanderers.StartingAndOptionalPawns.Count - 1, this.curPawnIndex);
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.curPawnIndex = index;
				SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
			}
			Widgets.EndGroup();
		}

		// Token: 0x06011B82 RID: 72578 RVA: 0x0051B657 File Offset: 0x00519857
		public override void PostClose()
		{
			base.PostClose();
			if (Find.GameEnder.gameEnding)
			{
				StartingPawnUtility.ClearAllStartingPawns();
			}
		}

		// Token: 0x0400B69D RID: 46749
		private int curPawnIndex;

		// Token: 0x0400B69E RID: 46750
		private int generationIndex;

		// Token: 0x0400B69F RID: 46751
		private const float TitleAreaHeight = 45f;

		// Token: 0x0400B6A0 RID: 46752
		private const float PawnEntryHeight = 60f;

		// Token: 0x0400B6A1 RID: 46753
		private const float SkillSummaryHeight = 127f;

		// Token: 0x0400B6A2 RID: 46754
		private const float CrossIconSize = 15f;

		// Token: 0x0400B6A3 RID: 46755
		private const float TabAreaWidth = 140f;

		// Token: 0x0400B6A4 RID: 46756
		private static readonly Vector2 PawnSelectorPortraitSize = new Vector2(70f, 110f);

		// Token: 0x0400B6A5 RID: 46757
		private static readonly Vector2 ConfirmButtonSize = new Vector2(150f, 38f);

		// Token: 0x0400B6A6 RID: 46758
		private const int DefaultPawnCount = 3;

		// Token: 0x0400B6A7 RID: 46759
		private const int MinPawnCount = 1;

		// Token: 0x0400B6A8 RID: 46760
		private const int MaxPawnCount = 6;

		// Token: 0x0400B6A9 RID: 46761
		private static readonly FloatRange ExcludeBiologicalAgeRange = new FloatRange(12.1f, 13f);
	}
}
