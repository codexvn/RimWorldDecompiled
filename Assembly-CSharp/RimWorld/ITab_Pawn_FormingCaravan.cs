using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020033AA RID: 13226
	public class ITab_Pawn_FormingCaravan : ITab
	{
		// Token: 0x170030A3 RID: 12451
		// (get) Token: 0x06012963 RID: 76131 RVA: 0x0057D697 File Offset: 0x0057B897
		public override bool IsVisible
		{
			get
			{
				return this.SelPawn.IsFormingCaravan();
			}
		}

		// Token: 0x06012964 RID: 76132 RVA: 0x0057D6A4 File Offset: 0x0057B8A4
		public ITab_Pawn_FormingCaravan()
		{
			this.size = new Vector2(480f, 450f);
			this.labelKey = "TabFormingCaravan";
		}

		// Token: 0x06012965 RID: 76133 RVA: 0x0057D6D8 File Offset: 0x0057B8D8
		protected override void FillTab()
		{
			this.thingsToSelect.Clear();
			Rect outRect = new Rect(default(Vector2), this.size).ContractedBy(10f);
			outRect.yMin += 20f;
			Rect rect = new Rect(0f, 0f, outRect.width - 16f, Mathf.Max(this.lastDrawnHeight, outRect.height));
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, rect, true);
			float num = 0f;
			string status = ((LordJob_FormAndSendCaravan)this.SelPawn.GetLord().LordJob).Status;
			Widgets.Label(new Rect(0f, num, rect.width, 100f), status);
			num += 22f;
			num += 4f;
			this.DoPeopleAndAnimals(rect, ref num);
			num += 4f;
			this.DoItemsLists(rect, ref num);
			this.lastDrawnHeight = num;
			Widgets.EndScrollView();
			if (this.thingsToSelect.Any<Thing>())
			{
				ITab_Pawn_FormingCaravan.SelectNow(this.thingsToSelect);
				this.thingsToSelect.Clear();
			}
		}

		// Token: 0x06012966 RID: 76134 RVA: 0x0057D7FC File Offset: 0x0057B9FC
		public override void TabUpdate()
		{
			base.TabUpdate();
			if (this.SelPawn != null && this.SelPawn.GetLord() != null)
			{
				Lord lord = this.SelPawn.GetLord();
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], false, false, true);
				}
			}
		}

		// Token: 0x06012967 RID: 76135 RVA: 0x0057D860 File Offset: 0x0057BA60
		private void DoPeopleAndAnimals(Rect inRect, ref float curY)
		{
			Widgets.ListSeparator(ref curY, inRect.width, "CaravanMembers".Translate());
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lord.ownedPawns[i];
				if (pawn.IsFreeColonist)
				{
					num++;
					if (pawn.InMentalState)
					{
						num2++;
					}
				}
				else if (pawn.IsPrisoner)
				{
					num3++;
					if (pawn.InMentalState)
					{
						num4++;
					}
				}
				else if (pawn.RaceProps.Animal)
				{
					num5++;
					if (pawn.InMentalState)
					{
						num6++;
					}
					if (pawn.RaceProps.packAnimal)
					{
						num7++;
					}
				}
			}
			string pawnsCountLabel = this.GetPawnsCountLabel(num, num2, -1);
			string pawnsCountLabel2 = this.GetPawnsCountLabel(num3, num4, -1);
			string pawnsCountLabel3 = this.GetPawnsCountLabel(num5, num6, num7);
			float y = curY;
			float num8;
			this.DoPeopleAndAnimalsEntry(inRect, Faction.OfPlayer.def.pawnsPlural.CapitalizeFirst(), pawnsCountLabel, ref curY, out num8);
			float y2 = curY;
			float num9;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanPrisoners".Translate(), pawnsCountLabel2, ref curY, out num9);
			float y3 = curY;
			float num10;
			this.DoPeopleAndAnimalsEntry(inRect, "CaravanAnimals".Translate(), pawnsCountLabel3, ref curY, out num10);
			float width = Mathf.Max(new float[]
			{
				num8,
				num9,
				num10
			}) + 2f;
			Rect rect = new Rect(0f, y, width, 22f);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				this.HighlightColonists();
			}
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectColonistsLater();
			}
			Rect rect2 = new Rect(0f, y2, width, 22f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
				this.HighlightPrisoners();
			}
			if (Widgets.ButtonInvisible(rect2, true))
			{
				this.SelectPrisonersLater();
			}
			Rect rect3 = new Rect(0f, y3, width, 22f);
			if (Mouse.IsOver(rect3))
			{
				Widgets.DrawHighlight(rect3);
				this.HighlightAnimals();
			}
			if (Widgets.ButtonInvisible(rect3, true))
			{
				this.SelectAnimalsLater();
			}
		}

		// Token: 0x06012968 RID: 76136 RVA: 0x0057DAA0 File Offset: 0x0057BCA0
		private void DoPeopleAndAnimalsEntry(Rect inRect, string leftLabel, string rightLabel, ref float curY, out float drawnWidth)
		{
			Rect rect = new Rect(0f, curY, inRect.width, 100f);
			Widgets.Label(rect, leftLabel);
			rect.xMin += 120f;
			Widgets.Label(rect, rightLabel);
			curY += 22f;
			drawnWidth = 120f + Text.CalcSize(rightLabel).x;
		}

		// Token: 0x06012969 RID: 76137 RVA: 0x0057DB0C File Offset: 0x0057BD0C
		private void DoItemsLists(Rect inRect, ref float curY)
		{
			LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = (LordJob_FormAndSendCaravan)this.SelPawn.GetLord().LordJob;
			Rect rect = new Rect(0f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float a = 0f;
			Widgets.BeginGroup(rect);
			Widgets.ListSeparator(ref a, rect.width, "ItemsToLoad".Translate());
			bool flag = false;
			for (int i = 0; i < lordJob_FormAndSendCaravan.transferables.Count; i++)
			{
				TransferableOneWay transferableOneWay = lordJob_FormAndSendCaravan.transferables[i];
				if (transferableOneWay.CountToTransfer > 0 && transferableOneWay.HasAnyThing)
				{
					flag = true;
					this.DoThingRow(transferableOneWay.AnyThing.GetInnerIfMinified().def, transferableOneWay.CountToTransfer, transferableOneWay.things, rect.width, ref a);
				}
			}
			if (!flag)
			{
				Widgets.NoneLabel(ref a, rect.width, null);
			}
			Widgets.EndGroup();
			Rect rect2 = new Rect((inRect.width + 10f) / 2f, curY, (inRect.width - 10f) / 2f, inRect.height);
			float b = 0f;
			Widgets.BeginGroup(rect2);
			Widgets.ListSeparator(ref b, rect2.width, "LoadedItems".Translate());
			bool flag2 = false;
			for (int j = 0; j < lordJob_FormAndSendCaravan.lord.ownedPawns.Count; j++)
			{
				Pawn pawn = lordJob_FormAndSendCaravan.lord.ownedPawns[j];
				if (!pawn.inventory.UnloadEverything)
				{
					for (int k = 0; k < pawn.inventory.innerContainer.Count; k++)
					{
						Thing thing = pawn.inventory.innerContainer[k];
						flag2 = true;
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
						ITab_Pawn_FormingCaravan.tmpSingleThing.Add(thing);
						this.DoThingRow(thing.def, thing.stackCount, ITab_Pawn_FormingCaravan.tmpSingleThing, rect2.width, ref b);
						ITab_Pawn_FormingCaravan.tmpSingleThing.Clear();
					}
				}
			}
			if (!flag2)
			{
				Widgets.NoneLabel(ref b, rect.width, null);
			}
			Widgets.EndGroup();
			curY += Mathf.Max(a, b);
		}

		// Token: 0x0601296A RID: 76138 RVA: 0x0057DD54 File Offset: 0x0057BF54
		private void SelectColonistsLater()
		{
			Lord lord = this.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0601296B RID: 76139 RVA: 0x0057DDCC File Offset: 0x0057BFCC
		private void HighlightColonists()
		{
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsFreeColonist)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0601296C RID: 76140 RVA: 0x0057DE28 File Offset: 0x0057C028
		private void SelectPrisonersLater()
		{
			Lord lord = this.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0601296D RID: 76141 RVA: 0x0057DEA0 File Offset: 0x0057C0A0
		private void HighlightPrisoners()
		{
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].IsPrisoner)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x0601296E RID: 76142 RVA: 0x0057DEFC File Offset: 0x0057C0FC
		private void SelectAnimalsLater()
		{
			Lord lord = this.SelPawn.GetLord();
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					ITab_Pawn_FormingCaravan.tmpPawns.Add(lord.ownedPawns[i]);
				}
			}
			this.SelectLater(ITab_Pawn_FormingCaravan.tmpPawns);
			ITab_Pawn_FormingCaravan.tmpPawns.Clear();
		}

		// Token: 0x0601296F RID: 76143 RVA: 0x0057DF78 File Offset: 0x0057C178
		private void HighlightAnimals()
		{
			Lord lord = this.SelPawn.GetLord();
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				if (lord.ownedPawns[i].RaceProps.Animal)
				{
					TargetHighlighter.Highlight(lord.ownedPawns[i], true, true, false);
				}
			}
		}

		// Token: 0x06012970 RID: 76144 RVA: 0x0057DFD8 File Offset: 0x0057C1D8
		private void SelectLater(List<Thing> things)
		{
			this.thingsToSelect.Clear();
			this.thingsToSelect.AddRange(things);
		}

		// Token: 0x06012971 RID: 76145 RVA: 0x0057DFF4 File Offset: 0x0057C1F4
		public static void SelectNow(List<Thing> things)
		{
			if (!things.Any<Thing>())
			{
				return;
			}
			Find.Selector.ClearSelection();
			bool flag = false;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].SpawnedOrAnyParentSpawned)
				{
					if (!flag)
					{
						CameraJumper.TryJump(things[i], CameraJumper.MovementMode.Pan);
					}
					Find.Selector.Select(things[i], true, true);
					flag = true;
				}
			}
			if (!flag)
			{
				CameraJumper.TryJumpAndSelect(things[0], CameraJumper.MovementMode.Pan);
			}
		}

		// Token: 0x06012972 RID: 76146 RVA: 0x0057E074 File Offset: 0x0057C274
		private string GetPawnsCountLabel(int count, int countInMentalState, int countPackAnimals)
		{
			string text = count.ToString();
			bool flag = countInMentalState > 0;
			bool flag2 = count > 0 && countPackAnimals >= 0;
			if (flag || flag2)
			{
				text += " (";
				if (flag2)
				{
					text += countPackAnimals.ToString() + " " + "PackAnimalsCountLower".Translate();
				}
				if (flag)
				{
					if (flag2)
					{
						text += ", ";
					}
					text += countInMentalState.ToString() + " " + "InMentalStateCountLower".Translate();
				}
				text += ")";
			}
			return text;
		}

		// Token: 0x06012973 RID: 76147 RVA: 0x0057E12C File Offset: 0x0057C32C
		private void EndJobForEveryoneHauling(TransferableOneWay t, LordJob_FormAndSendCaravan lordJob)
		{
			List<Pawn> freeColonistsSpawned = base.SelThing.Map.mapPawns.FreeColonistsSpawned;
			for (int i = 0; i < freeColonistsSpawned.Count; i++)
			{
				if (freeColonistsSpawned[i].CurJobDef == JobDefOf.PrepareCaravan_GatherItems)
				{
					JobDriver_PrepareCaravan_GatherItems jobDriver_PrepareCaravan_GatherItems = freeColonistsSpawned[i].jobs.curDriver as JobDriver_PrepareCaravan_GatherItems;
					if (jobDriver_PrepareCaravan_GatherItems != null && jobDriver_PrepareCaravan_GatherItems.job.lord == lordJob.lord && jobDriver_PrepareCaravan_GatherItems.ToHaul != null && jobDriver_PrepareCaravan_GatherItems.ToHaul.def == t.ThingDef)
					{
						freeColonistsSpawned[i].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x06012974 RID: 76148 RVA: 0x0057E1D0 File Offset: 0x0057C3D0
		private void DropThingToLoad(TransferableOneWay t, int count)
		{
			t.ForceTo(t.CountToTransfer - count);
			this.EndJobForEveryoneHauling(t, (LordJob_FormAndSendCaravan)this.SelPawn.GetLord().LordJob);
		}

		// Token: 0x06012975 RID: 76149 RVA: 0x0057E1FC File Offset: 0x0057C3FC
		private void DoThingRow(ThingDef thingDef, int count, List<Thing> things, float width, ref float curY)
		{
			ITab_Pawn_FormingCaravan.<>c__DisplayClass33_0 CS$<>8__locals1 = new ITab_Pawn_FormingCaravan.<>c__DisplayClass33_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.thingDef = thingDef;
			CS$<>8__locals1.count = count;
			Rect rect = new Rect(0f, curY, width, 28f);
			CS$<>8__locals1.singleThingForClosure = ((things.Count == 1) ? things[0] : null);
			if (CS$<>8__locals1.count != 1 && Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonSpecificCountButtonTex, true, null))
			{
				Find.WindowStack.Add(new Dialog_Slider("RemoveSliderText".Translate(CS$<>8__locals1.thingDef.label), 1, CS$<>8__locals1.count, new Action<int>(CS$<>8__locals1.<DoThingRow>g__DoDropThing|0), int.MinValue, 1f));
			}
			rect.width -= 24f;
			if (Widgets.ButtonImage(new Rect(rect.x + rect.width - 24f, rect.y + (rect.height - 24f) / 2f, 24f, 24f), CaravanThingsTabUtility.AbandonButtonTex, true, null))
			{
				string value = CS$<>8__locals1.thingDef.label;
				if (things.Count == 1 && things[0] is Pawn)
				{
					value = ((Pawn)things[0]).LabelShortCap;
				}
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("ConfirmRemoveItemDialog".Translate(value), delegate()
				{
					base.<DoThingRow>g__DoDropThing|0(CS$<>8__locals1.count);
				}, false, null, WindowLayer.Dialog));
			}
			rect.width -= 24f;
			if (things.Count == 1)
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, things[0]);
			}
			else
			{
				Widgets.InfoCardButton(rect.width - 24f, curY, CS$<>8__locals1.thingDef);
			}
			rect.width -= 24f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_FormingCaravan.ThingHighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (CS$<>8__locals1.thingDef.DrawMatSingle != null && CS$<>8__locals1.thingDef.DrawMatSingle.mainTexture != null)
			{
				Rect rect2 = new Rect(4f, curY, 28f, 28f);
				if (things.Count == 1)
				{
					Widgets.ThingIcon(rect2, things[0], 1f, null, false, 1f, false);
				}
				else
				{
					Widgets.ThingIcon(rect2, CS$<>8__locals1.thingDef, null, null, 1f, null, null, 1f);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_FormingCaravan.ThingLabelColor;
			Rect rect3 = new Rect(36f, curY, rect.width - 36f, rect.height);
			string str;
			if (things.Count == 1 && CS$<>8__locals1.count == things[0].stackCount)
			{
				str = things[0].LabelCap;
			}
			else if (CS$<>8__locals1.thingDef == ThingDefOf.MinifiedThing)
			{
				str = GenLabel.ThingLabel(things[0].GetInnerIfMinified(), CS$<>8__locals1.count, true, true).CapitalizeFirst();
			}
			else
			{
				str = GenLabel.ThingLabel(CS$<>8__locals1.thingDef, null, CS$<>8__locals1.count).CapitalizeFirst();
			}
			Text.WordWrap = false;
			Widgets.Label(rect3, str.Truncate(rect3.width, null));
			Text.WordWrap = true;
			Text.Anchor = TextAnchor.UpperLeft;
			TooltipHandler.TipRegion(rect, str);
			if (Widgets.ButtonInvisible(rect, true))
			{
				this.SelectLater(things);
			}
			if (Mouse.IsOver(rect))
			{
				for (int i = 0; i < things.Count; i++)
				{
					TargetHighlighter.Highlight(things[i], true, true, false);
				}
			}
			curY += 28f;
		}

		// Token: 0x0400C107 RID: 49415
		private Vector2 scrollPosition;

		// Token: 0x0400C108 RID: 49416
		private float lastDrawnHeight;

		// Token: 0x0400C109 RID: 49417
		private List<Thing> thingsToSelect = new List<Thing>();

		// Token: 0x0400C10A RID: 49418
		private static List<Thing> tmpSingleThing = new List<Thing>();

		// Token: 0x0400C10B RID: 49419
		private const float TopPadding = 20f;

		// Token: 0x0400C10C RID: 49420
		private const float StandardLineHeight = 22f;

		// Token: 0x0400C10D RID: 49421
		private const float ExtraSpaceBetweenSections = 4f;

		// Token: 0x0400C10E RID: 49422
		private const float SpaceBetweenItemsLists = 10f;

		// Token: 0x0400C10F RID: 49423
		private const float ThingRowHeight = 28f;

		// Token: 0x0400C110 RID: 49424
		private const float ThingIconSize = 28f;

		// Token: 0x0400C111 RID: 49425
		private const float ThingLeftX = 36f;

		// Token: 0x0400C112 RID: 49426
		private static readonly Color ThingLabelColor = ITab_Pawn_Gear.ThingLabelColor;

		// Token: 0x0400C113 RID: 49427
		private static readonly Color ThingHighlightColor = ITab_Pawn_Gear.HighlightColor;

		// Token: 0x0400C114 RID: 49428
		private static List<Thing> tmpPawns = new List<Thing>();
	}
}
