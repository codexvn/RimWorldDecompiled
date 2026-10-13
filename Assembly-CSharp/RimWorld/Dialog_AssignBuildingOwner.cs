using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003113 RID: 12563
	public class Dialog_AssignBuildingOwner : Window
	{
		// Token: 0x17002DE6 RID: 11750
		// (get) Token: 0x06011A6F RID: 72303 RVA: 0x00511ADF File Offset: 0x0050FCDF
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(520f, 500f);
			}
		}

		// Token: 0x06011A70 RID: 72304 RVA: 0x00511AF0 File Offset: 0x0050FCF0
		public Dialog_AssignBuildingOwner(CompAssignableToPawn assignable) : base(null)
		{
			this.assignable = assignable;
			this.doCloseButton = true;
			this.doCloseX = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011A71 RID: 72305 RVA: 0x00511B1C File Offset: 0x0050FD1C
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect outRect = new Rect(inRect);
			outRect.yMin += 20f;
			outRect.yMax -= 40f;
			float num = 0f;
			num += (float)this.assignable.AssignedPawnsForReading.Count * 35f;
			num += (float)this.assignable.AssigningCandidates.Count<Pawn>() * 35f;
			num += 7f;
			Rect viewRect = new Rect(0f, 0f, outRect.width, num);
			Widgets.AdjustRectsForScrollView(inRect, ref outRect, ref viewRect);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			this.SortTmpList(this.assignable.AssignedPawnsForReading);
			float num2 = 0f;
			for (int i = 0; i < Dialog_AssignBuildingOwner.tmpPawnSorted.Count; i++)
			{
				Pawn pawn = Dialog_AssignBuildingOwner.tmpPawnSorted[i];
				this.DrawAssignedRow(pawn, ref num2, viewRect, i);
			}
			if (this.assignable.AssignedPawnsForReading.Count > 0)
			{
				Rect rect = new Rect(0f, num2, viewRect.width, 7f);
				num2 += 7f;
				using (new TextBlock(Widgets.SeparatorLineColor))
				{
					Widgets.DrawLineHorizontal(rect.x, rect.y + rect.height / 2f, rect.width);
				}
			}
			this.SortTmpList(this.assignable.AssigningCandidates);
			for (int j = 0; j < Dialog_AssignBuildingOwner.tmpPawnSorted.Count; j++)
			{
				Pawn pawn2 = Dialog_AssignBuildingOwner.tmpPawnSorted[j];
				this.DrawUnassignedRow(pawn2, ref num2, viewRect, j);
			}
			Dialog_AssignBuildingOwner.tmpPawnSorted.Clear();
			Widgets.EndScrollView();
		}

		// Token: 0x06011A72 RID: 72306 RVA: 0x00511CF8 File Offset: 0x0050FEF8
		private void SortTmpList(IEnumerable<Pawn> collection)
		{
			Dialog_AssignBuildingOwner.tmpPawnSorted.Clear();
			Dialog_AssignBuildingOwner.tmpPawnSorted.AddRange(collection);
			Dialog_AssignBuildingOwner.tmpPawnSorted.SortBy((Pawn x) => x.LabelShort);
		}

		// Token: 0x06011A73 RID: 72307 RVA: 0x00511D38 File Offset: 0x0050FF38
		private void DrawAssignedRow(Pawn pawn, ref float y, Rect viewRect, int i)
		{
			Rect rect = new Rect(0f, y, viewRect.width, 35f);
			y += 35f;
			if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Rect rect2 = rect;
			rect2.width = rect.height;
			Widgets.ThingIcon(rect2, pawn, 1f, null, false, 1f, false);
			Rect rect3 = rect;
			rect3.xMin = rect.xMax - 165f - 10f;
			rect3 = rect3.ContractedBy(2f);
			if (Widgets.ButtonText(rect3, "BuildingUnassign".Translate(), true, true, true, null))
			{
				this.assignable.TryUnassignPawn(pawn, true, false);
				SoundDefOf.Click.PlayOneShotOnCamera(null);
			}
			Rect rect4 = rect;
			rect4.xMin = rect2.xMax + 10f;
			rect4.xMax = rect3.xMin - 10f;
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				Widgets.LabelEllipses(rect4, pawn.LabelCap);
			}
		}

		// Token: 0x06011A74 RID: 72308 RVA: 0x00511E68 File Offset: 0x00510068
		private void DrawUnassignedRow(Pawn pawn, ref float y, Rect viewRect, int i)
		{
			if (this.assignable.AssignedPawnsForReading.Contains(pawn))
			{
				return;
			}
			AcceptanceReport acceptanceReport = this.assignable.CanAssignTo(pawn);
			bool accepted = acceptanceReport.Accepted;
			Rect rect = new Rect(0f, y, viewRect.width, 35f);
			y += 35f;
			if (i % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			if (!accepted)
			{
				GUI.color = Color.gray;
			}
			Rect rect2 = rect;
			rect2.width = rect.height;
			Widgets.ThingIcon(rect2, pawn, 1f, null, false, 1f, false);
			Rect rect3 = rect;
			rect3.xMin = rect.xMax - 165f - 10f;
			rect3 = rect3.ContractedBy(2f);
			if (!Find.IdeoManager.classicMode && accepted && this.assignable.IdeoligionForbids(pawn))
			{
				Rect rect4 = rect;
				rect4.width = rect.height;
				rect4.x = rect.xMax - rect.height;
				rect4 = rect4.ContractedBy(2f);
				using (new TextBlock(TextAnchor.MiddleLeft))
				{
					Widgets.Label(rect3, "IdeoligionForbids".Translate());
				}
				IdeoUIUtility.DoIdeoIcon(rect4, pawn.ideo.Ideo, true, delegate
				{
					IdeoUIUtility.OpenIdeoInfo(pawn.ideo.Ideo);
					this.Close(true);
				});
			}
			else if (accepted)
			{
				TaggedString taggedString = this.assignable.AssignedAnything(pawn) ? "BuildingReassign".Translate() : "BuildingAssign".Translate();
				if (Widgets.ButtonText(rect3, taggedString, true, true, true, null))
				{
					this.assignable.TryAssignPawn(pawn);
					if (this.assignable.MaxAssignedPawnsCount == 1)
					{
						this.Close(true);
					}
					else
					{
						SoundDefOf.Click.PlayOneShotOnCamera(null);
					}
				}
			}
			Rect rect5 = rect;
			rect5.xMin = rect2.xMax + 10f;
			rect5.xMax = rect3.xMin - 10f;
			string label = pawn.LabelCap + (accepted ? "" : (" (" + acceptanceReport.Reason.StripTags() + ")"));
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				Widgets.LabelEllipses(rect5, label);
			}
		}

		// Token: 0x0400B5C8 RID: 46536
		private CompAssignableToPawn assignable;

		// Token: 0x0400B5C9 RID: 46537
		private Vector2 scrollPosition;

		// Token: 0x0400B5CA RID: 46538
		private const float EntryHeight = 35f;

		// Token: 0x0400B5CB RID: 46539
		private const int AssignButtonWidth = 165;

		// Token: 0x0400B5CC RID: 46540
		private const int SeparatorHeight = 7;

		// Token: 0x0400B5CD RID: 46541
		private static readonly List<Pawn> tmpPawnSorted = new List<Pawn>(16);
	}
}
