using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003194 RID: 12692
	public class Dialog_MedicalDefaults : Window
	{
		// Token: 0x17002E6B RID: 11883
		// (get) Token: 0x06011DB7 RID: 73143 RVA: 0x0052F5B2 File Offset: 0x0052D7B2
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(406f, 640f);
			}
		}

		// Token: 0x17002E6C RID: 11884
		// (get) Token: 0x06011DB8 RID: 73144 RVA: 0x0052F5C3 File Offset: 0x0052D7C3
		public override string CloseButtonText
		{
			get
			{
				return "OK".Translate();
			}
		}

		// Token: 0x06011DB9 RID: 73145 RVA: 0x0052F5D4 File Offset: 0x0052D7D4
		public Dialog_MedicalDefaults() : base(null)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
		}

		// Token: 0x06011DBA RID: 73146 RVA: 0x0052F600 File Offset: 0x0052D800
		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			using (new TextBlock(GameFont.Medium))
			{
				Widgets.Label(inRect, ref num, "DefaultMedicineSettings".Translate(), default(TipSignal));
			}
			Text.Font = GameFont.Small;
			Widgets.Label(inRect, ref num, "DefaultMedicineSettingsDesc".Translate(), default(TipSignal));
			num += 10f;
			Text.Anchor = TextAnchor.MiddleLeft;
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForColonist, "MedGroupColonists", "MedGroupColonistsDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForPrisoner, "MedGroupPrisoners", "MedGroupPrisonersDesc");
			if (ModsConfig.IdeologyActive)
			{
				this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForSlave, "MedGroupSlaves", "MedGroupSlavesDesc");
			}
			if (ModsConfig.AnomalyActive)
			{
				this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForGhouls, "MedGroupGhouls", "MedGroupGhoulsDesc");
			}
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForTamedAnimal, "MedGroupTamedAnimals", "MedGroupTamedAnimalsDesc");
			num += 17f;
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForFriendlyFaction, "MedGroupFriendlyFaction", "MedGroupFriendlyFactionDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForNeutralFaction, "MedGroupNeutralFaction", "MedGroupNeutralFactionDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForHostileFaction, "MedGroupHostileFaction", "MedGroupHostileFactionDesc");
			num += 17f;
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForNoFaction, "MedGroupNoFaction", "MedGroupNoFactionDesc");
			this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForWildlife, "MedGroupWildlife", "MedGroupWildlifeDesc");
			if (ModsConfig.AnomalyActive)
			{
				this.DoRow(inRect, ref num, ref Find.PlaySettings.defaultCareForEntities, "MedGroupEntities", "MedGroupEntitiesDesc");
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06011DBB RID: 73147 RVA: 0x0052F7FC File Offset: 0x0052D9FC
		private void DoRow(Rect rect, ref float y, ref MedicalCareCategory category, string labelKey, string tipKey)
		{
			Rect rect2 = new Rect(rect.x, y, rect.width, 28f);
			Rect rect3 = new Rect(rect.x, y, 230f, 28f);
			Rect rect4 = new Rect(230f, y, 140f, 28f);
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawLightHighlight(rect2);
			}
			TooltipHandler.TipRegionByKey(rect2, tipKey);
			Widgets.LabelFit(rect3, labelKey.Translate());
			MedicalCareUtility.MedicalCareSetter(rect4, ref category);
			y += 34f;
		}

		// Token: 0x0400B8B3 RID: 47283
		private const float MedicalCareLabelWidth = 230f;

		// Token: 0x0400B8B4 RID: 47284
		private const float VerticalGap = 6f;
	}
}
