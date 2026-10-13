using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020031E8 RID: 12776
	[StaticConstructorOnStartup]
	public class Dialog_ManageDrugPolicies : Dialog_ManagePolicies<DrugPolicy>
	{
		// Token: 0x17002EA2 RID: 11938
		// (get) Token: 0x06011F6C RID: 73580 RVA: 0x0053AF47 File Offset: 0x00539147
		protected override string TitleKey
		{
			get
			{
				return "DrugPolicyTitle";
			}
		}

		// Token: 0x17002EA3 RID: 11939
		// (get) Token: 0x06011F6D RID: 73581 RVA: 0x0053AF4E File Offset: 0x0053914E
		protected override string TipKey
		{
			get
			{
				return "DrugPolicyTip";
			}
		}

		// Token: 0x17002EA4 RID: 11940
		// (get) Token: 0x06011F6E RID: 73582 RVA: 0x0017FF19 File Offset: 0x0017E119
		protected override float OffsetHeaderY
		{
			get
			{
				return 36f;
			}
		}

		// Token: 0x17002EA5 RID: 11941
		// (get) Token: 0x06011F6F RID: 73583 RVA: 0x0053AF55 File Offset: 0x00539155
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)Mathf.Min(Screen.width - 50, 1300), 720f);
			}
		}

		// Token: 0x06011F70 RID: 73584 RVA: 0x0053AF74 File Offset: 0x00539174
		public Dialog_ManageDrugPolicies(DrugPolicy policy) : base(policy)
		{
		}

		// Token: 0x06011F71 RID: 73585 RVA: 0x0053AF7D File Offset: 0x0053917D
		protected override DrugPolicy CreateNewPolicy()
		{
			return Current.Game.drugPolicyDatabase.MakeNewDrugPolicy();
		}

		// Token: 0x06011F72 RID: 73586 RVA: 0x0053AF8E File Offset: 0x0053918E
		protected override DrugPolicy GetDefaultPolicy()
		{
			return Current.Game.drugPolicyDatabase.DefaultDrugPolicy();
		}

		// Token: 0x06011F73 RID: 73587 RVA: 0x0053AF9F File Offset: 0x0053919F
		protected override void SetDefaultPolicy(DrugPolicy policy)
		{
			Current.Game.drugPolicyDatabase.SetDefault(policy);
		}

		// Token: 0x06011F74 RID: 73588 RVA: 0x0053AFB1 File Offset: 0x005391B1
		protected override AcceptanceReport TryDeletePolicy(DrugPolicy policy)
		{
			return Current.Game.drugPolicyDatabase.TryDelete(policy);
		}

		// Token: 0x06011F75 RID: 73589 RVA: 0x0053AFC3 File Offset: 0x005391C3
		protected override List<DrugPolicy> GetPolicies()
		{
			return Current.Game.drugPolicyDatabase.AllPolicies;
		}

		// Token: 0x06011F76 RID: 73590 RVA: 0x0053AFD4 File Offset: 0x005391D4
		protected override void DoContentsRect(Rect rect)
		{
			this.DoPolicyConfigArea(rect);
		}

		// Token: 0x06011F77 RID: 73591 RVA: 0x0053AFDD File Offset: 0x005391DD
		public override void PostOpen()
		{
			base.PostOpen();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}

		// Token: 0x06011F78 RID: 73592 RVA: 0x0053AFF0 File Offset: 0x005391F0
		private void DoPolicyConfigArea(Rect rect)
		{
			Rect rect2 = rect;
			rect2.y -= this.OffsetHeaderY;
			rect2.height = 36f;
			Rect rect3 = rect;
			rect3.yMin = rect2.yMax;
			this.DoColumnLabels(rect2);
			Widgets.DrawMenuSection(rect3);
			rect3 = rect3.ContractedBy(1f);
			if (base.SelectedPolicy.Count == 0)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect3, "NoDrugs".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			float height = (float)base.SelectedPolicy.Count * 35f;
			Rect viewRect = new Rect(0f, 0f, rect3.width, height);
			Widgets.AdjustRectsForScrollView(rect, ref rect3, ref viewRect);
			Widgets.BeginScrollView(rect3, ref this.scrollPosition, viewRect, true);
			DrugPolicy selectedPolicy = base.SelectedPolicy;
			for (int i = 0; i < selectedPolicy.Count; i++)
			{
				Rect rect4 = new Rect(0f, (float)i * 35f, viewRect.width, 35f);
				this.DoEntryRow(rect4, selectedPolicy[i], i);
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06011F79 RID: 73593 RVA: 0x0053B124 File Offset: 0x00539324
		private void CalculateColumnsWidths(Rect rect, out float addictionWidth, out float allowJoyWidth, out float scheduledWidth, out float drugIconWidth, out float drugNameWidth, out float frequencyWidth, out float moodThresholdWidth, out float joyThresholdWidth, out float takeToInventoryWidth)
		{
			float num = rect.width - 27f - 108f;
			drugIconWidth = 27f;
			drugNameWidth = num * 0.25f;
			addictionWidth = 36f;
			allowJoyWidth = 36f;
			scheduledWidth = 36f;
			frequencyWidth = num * 0.3f;
			moodThresholdWidth = num * 0.15f;
			joyThresholdWidth = num * 0.15f;
			takeToInventoryWidth = num * 0.15f;
		}

		// Token: 0x06011F7A RID: 73594 RVA: 0x0053B198 File Offset: 0x00539398
		private void DoColumnLabels(Rect rect)
		{
			rect.width -= 16f;
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			float num9;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9);
			float num10 = rect.x;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect2 = new Rect(num10 + 4f, rect.y, num5 + num4, rect.height);
			Widgets.Label(rect2, "DrugColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect2, "DrugNameColumnDesc");
			num10 += num5 + num4;
			Rect rect3 = new Rect(num10, rect.y, num9, rect.height);
			Widgets.Label(rect3, "TakeToInventoryColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect3, "TakeToInventoryColumnDesc");
			num10 += num9;
			Rect rect4 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect4, Dialog_ManageDrugPolicies.IconForAddiction);
			TooltipHandler.TipRegionByKey(rect4, "DrugUsageTipForAddiction");
			num10 += num;
			Rect rect5 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect5, Dialog_ManageDrugPolicies.IconForJoy);
			TooltipHandler.TipRegionByKey(rect5, "DrugUsageTipForJoy");
			num10 += num2;
			Rect rect6 = new Rect(num10, rect.yMax - 24f, 24f, 24f);
			GUI.DrawTexture(rect6, Dialog_ManageDrugPolicies.IconScheduled);
			TooltipHandler.TipRegionByKey(rect6, "DrugUsageTipScheduled");
			num10 += num3;
			Text.Anchor = TextAnchor.LowerCenter;
			Rect rect7 = new Rect(num10, rect.y, num6, rect.height);
			Widgets.Label(rect7, "FrequencyColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect7, "FrequencyColumnDesc");
			num10 += num6;
			Rect rect8 = new Rect(num10, rect.y, num7, rect.height);
			Widgets.Label(rect8, "MoodThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect8, "MoodThresholdColumnDesc");
			num10 += num7;
			Rect rect9 = new Rect(num10, rect.y, num8, rect.height);
			Widgets.Label(rect9, "JoyThresholdColumnLabel".Translate());
			TooltipHandler.TipRegionByKey(rect9, "JoyThresholdColumnDesc");
			num10 += num8;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06011F7B RID: 73595 RVA: 0x0053B3BC File Offset: 0x005395BC
		private void DoEntryRow(Rect rect, DrugPolicyEntry entry, int index)
		{
			float num;
			float num2;
			float num3;
			float num4;
			float num5;
			float num6;
			float num7;
			float num8;
			float num9;
			this.CalculateColumnsWidths(rect, out num, out num2, out num3, out num4, out num5, out num6, out num7, out num8, out num9);
			if (index % 2 == 1)
			{
				Widgets.DrawLightHighlight(rect);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			float num10 = rect.x;
			float num11 = (rect.height - num4) / 2f;
			Widgets.ThingIcon(new Rect(num10 + 5f, rect.y + num11, num4, num4), entry.drug, null, null, 1f, null, null, 1f);
			num10 += num4;
			Widgets.Label(new Rect(num10, rect.y, num5, rect.height).ContractedBy(4f), entry.drug.LabelCap);
			Widgets.InfoCardButton(new Rect
			{
				x = num10 + Text.CalcSize(entry.drug.LabelCap).x + 5f,
				y = rect.y + 8f,
				width = 16f,
				height = 16f
			}, entry.drug);
			num10 += num5;
			Widgets.TextFieldNumeric<int>(new Rect(num10, rect.y, num9, rect.height).ContractedBy(4f), ref entry.takeToInventory, ref entry.takeToInventoryTempBuffer, 0f, (float)PawnUtility.GetMaxAllowedToPickUp(entry.drug, null));
			num10 += num9;
			if (entry.drug.IsAddictiveDrug)
			{
				Widgets.Checkbox(num10, rect.y + 2f, ref entry.allowedForAddiction, 24f, false, true, null, null);
			}
			num10 += num;
			if (entry.drug.IsPleasureDrug)
			{
				Widgets.Checkbox(num10, rect.y + 2f, ref entry.allowedForJoy, 24f, false, true, null, null);
			}
			num10 += num2;
			Widgets.Checkbox(num10, rect.y + 2f, ref entry.allowScheduled, 24f, false, true, null, null);
			num10 += num3;
			if (entry.allowScheduled)
			{
				entry.daysFrequency = Widgets.FrequencyHorizontalSlider(new Rect(num10, rect.y + 2f, num6, rect.height).ContractedBy(4f), entry.daysFrequency, 0.1f, 25f, true);
				num10 += num6;
				string label;
				if (entry.onlyIfMoodBelow < 1f)
				{
					label = entry.onlyIfMoodBelow.ToStringPercent();
				}
				else
				{
					label = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfMoodBelow = Widgets.HorizontalSlider(new Rect(num10, rect.y + 2f, num7, rect.height).ContractedBy(4f), entry.onlyIfMoodBelow, 0.01f, 1f, true, label, null, null, -1f);
				num10 += num7;
				string label2;
				if (entry.onlyIfJoyBelow < 1f)
				{
					label2 = entry.onlyIfJoyBelow.ToStringPercent();
				}
				else
				{
					label2 = "NoDrugUseRequirement".Translate();
				}
				entry.onlyIfJoyBelow = Widgets.HorizontalSlider(new Rect(num10, rect.y + 2f, num8, rect.height).ContractedBy(4f), entry.onlyIfJoyBelow, 0.01f, 1f, true, label2, null, null, -1f);
				num10 += num8;
			}
			else
			{
				num10 += num6 + num7 + num8;
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0400BA16 RID: 47638
		private Vector2 scrollPosition;

		// Token: 0x0400BA17 RID: 47639
		private const float DrugEntryRowHeight = 35f;

		// Token: 0x0400BA18 RID: 47640
		private const float CellsPadding = 4f;

		// Token: 0x0400BA19 RID: 47641
		private const float EntryLineYOffset = 2f;

		// Token: 0x0400BA1A RID: 47642
		private const float InfoButtonSize = 16f;

		// Token: 0x0400BA1B RID: 47643
		private const float HeaderHeight = 36f;

		// Token: 0x0400BA1C RID: 47644
		private static readonly Texture2D IconForAddiction = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Addiction", true);

		// Token: 0x0400BA1D RID: 47645
		private static readonly Texture2D IconForJoy = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Joy", true);

		// Token: 0x0400BA1E RID: 47646
		private static readonly Texture2D IconScheduled = ContentFinder<Texture2D>.Get("UI/Icons/DrugPolicy/Schedule", true);

		// Token: 0x0400BA1F RID: 47647
		private const float UsageSpacing = 12f;
	}
}
