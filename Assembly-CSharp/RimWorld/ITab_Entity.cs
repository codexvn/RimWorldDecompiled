using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200339B RID: 13211
	public class ITab_Entity : ITab
	{
		// Token: 0x17003094 RID: 12436
		// (get) Token: 0x06012921 RID: 76065 RVA: 0x0057B628 File Offset: 0x00579828
		public override bool IsVisible
		{
			get
			{
				if (!ModsConfig.AnomalyActive)
				{
					return false;
				}
				Building_HoldingPlatform building_HoldingPlatform = base.SelThing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					return building_HoldingPlatform.HeldPawn != null;
				}
				return base.SelThing is Pawn && base.SelThing.IsOnHoldingPlatform;
			}
		}

		// Token: 0x17003095 RID: 12437
		// (get) Token: 0x06012922 RID: 76066 RVA: 0x0057B674 File Offset: 0x00579874
		protected override Pawn SelPawn
		{
			get
			{
				Pawn selPawn = base.SelPawn;
				if (selPawn != null)
				{
					return selPawn;
				}
				Building_HoldingPlatform building_HoldingPlatform = base.SelThing as Building_HoldingPlatform;
				if (building_HoldingPlatform != null)
				{
					return building_HoldingPlatform.HeldPawn;
				}
				return null;
			}
		}

		// Token: 0x06012923 RID: 76067 RVA: 0x0057B6A4 File Offset: 0x005798A4
		public ITab_Entity()
		{
			this.size = new Vector2(280f, 0f);
			this.labelKey = "TabEntity";
			this.tutorTag = "Entity";
		}

		// Token: 0x06012924 RID: 76068 RVA: 0x0057B6D7 File Offset: 0x005798D7
		public override void OnOpen()
		{
			if (!ModLister.CheckAnomaly("entity itab"))
			{
				this.CloseTab();
				return;
			}
			base.OnOpen();
		}

		// Token: 0x06012925 RID: 76069 RVA: 0x0057B6F4 File Offset: 0x005798F4
		protected override void FillTab()
		{
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.maxOneColumn = true;
			listing_Standard.Begin(rect);
			Thing thing = this.SelPawn.ParentHolder as Thing;
			CompEntityHolder compEntityHolder;
			if (thing != null && thing.TryGetComp(out compEntityHolder))
			{
				StatDef containmentStrength = StatDefOf.ContainmentStrength;
				float containmentStrength2 = compEntityHolder.ContainmentStrength;
				string text = containmentStrength.description + "\n\n" + containmentStrength.Worker.GetExplanationFull(StatRequest.For(thing), containmentStrength.toStringNumberSense, containmentStrength2);
				Widgets.DrawHighlightIfMouseover(listing_Standard.Label(containmentStrength.LabelCap + ": " + containmentStrength.ValueToString(containmentStrength2, ToStringNumberSense.Absolute, true), -1f, new TipSignal?(new TipSignal(text, this.GetHashCode() + 1))));
			}
			StringBuilder stringBuilder = new StringBuilder();
			float num = ContainmentUtility.InitiateEscapeMtbDays(this.SelPawn, stringBuilder);
			int numTicks = Mathf.FloorToInt(num * 60000f);
			TaggedString taggedString = "HoldingPlatformEscapeMTBDays".Translate() + ": ";
			if (num < 0f)
			{
				taggedString += "Never".Translate();
			}
			else
			{
				taggedString += numTicks.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor);
			}
			string text2 = "HoldingPlatformEscapeMTBDaysDesc".Translate();
			if (stringBuilder.Length > 0)
			{
				string str = text2;
				string str2 = "\n\n";
				StringBuilder stringBuilder2 = stringBuilder;
				text2 = str + str2 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
			}
			Widgets.DrawHighlightIfMouseover(listing_Standard.Label(taggedString, -1f, new TipSignal?(new TipSignal(text2, this.GetHashCode() + 2))));
			CompStudiable compStudiable = this.SelPawn.TryGetComp<CompStudiable>();
			if (compStudiable != null)
			{
				ITab_Entity.DoStudyPeriodListing(listing_Standard, compStudiable);
			}
			if (compStudiable != null)
			{
				ITab_Entity.DoKnowledgeGainListing(listing_Standard, compStudiable);
			}
			listing_Standard.Gap(1f);
			Rect rect2 = listing_Standard.GetRect(24f, 1f).Rounded();
			TooltipHandler.TipRegionByKey(rect2, "MedicineQualityDescriptionEntity");
			Widgets.DrawHighlightIfMouseover(rect2);
			Rect rect3 = rect2;
			rect3.xMax = rect2.center.x - 4f;
			Rect rect4 = rect2;
			rect4.xMin = rect2.center.x + 4f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect3, string.Format("{0}:", "AllowMedicine".Translate()));
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawButtonGraphic(rect4);
			MedicalCareUtility.MedicalCareSelectButton(rect4, this.SelPawn);
			listing_Standard.Gap(4f);
			CompHoldingPlatformTarget compHoldingPlatformTarget = this.SelPawn.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null)
			{
				float height = 132f;
				Rect rect5 = listing_Standard.GetRect(height, 1f).Rounded();
				Widgets.DrawMenuSection(rect5);
				Rect rect6 = rect5.ContractedBy(10f);
				Widgets.BeginGroup(rect6);
				Rect rect7 = new Rect(0f, 0f, rect6.width, 28f);
				if (Widgets.RadioButtonLabeled(rect7, "EntityStudyMode_MaintainOnly".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.MaintainOnly, false))
				{
					compHoldingPlatformTarget.containmentMode = EntityContainmentMode.MaintainOnly;
				}
				Widgets.DrawHighlightIfMouseover(rect7);
				TooltipHandler.TipRegion(rect7, "EntityStudyMode_MaintainOnlyDesc".Translate());
				rect7.y += 28f;
				if (Widgets.RadioButtonLabeled(rect7, "EntityStudyMode_Study".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Study, false))
				{
					compHoldingPlatformTarget.containmentMode = EntityContainmentMode.Study;
				}
				Widgets.DrawHighlightIfMouseover(rect7);
				TooltipHandler.TipRegion(rect7, "EntityStudyMode_StudyDesc".Translate());
				rect7.y += 28f;
				if (Widgets.RadioButtonLabeled(rect7, "EntityStudyMode_Release".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release, false))
				{
					compHoldingPlatformTarget.containmentMode = EntityContainmentMode.Release;
				}
				Widgets.DrawHighlightIfMouseover(rect7);
				TooltipHandler.TipRegion(rect7, "EntityStudyMode_ReleaseDesc".Translate());
				rect7.y += 28f;
				if (Widgets.RadioButtonLabeled(rect7, "EntityStudyMode_Execute".Translate(), compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Execute, !compHoldingPlatformTarget.Props.canBeExecuted))
				{
					if (!compHoldingPlatformTarget.Props.canBeExecuted)
					{
						Messages.Message("CantBeExecuted".Translate(), MessageTypeDefOf.RejectInput, false);
					}
					else
					{
						compHoldingPlatformTarget.containmentMode = EntityContainmentMode.Execute;
					}
				}
				Widgets.DrawHighlightIfMouseover(rect7);
				TooltipHandler.TipRegion(rect7, "EntityStudyMode_ExecuteDesc".Translate() + (compHoldingPlatformTarget.Props.canBeExecuted ? "" : ("\n\n" + "CantBeExecuted".Translate().ToString())));
				rect7.y += 28f;
				Widgets.EndGroup();
				listing_Standard.Gap(12f);
				height = 48f;
				Rect rect8 = listing_Standard.GetRect(height, 1f).Rounded();
				Widgets.DrawMenuSection(rect8);
				rect6 = rect8.ContractedBy(10f);
				Widgets.BeginGroup(rect6);
				string text3 = null;
				if (!ResearchProjectDefOf.BioferriteExtraction.IsFinished)
				{
					text3 = "RequiresBioferriteExtraction".Translate();
				}
				else
				{
					Building_HoldingPlatform heldPlatform = compHoldingPlatformTarget.HeldPlatform;
					if (heldPlatform != null && heldPlatform.HasAttachedBioferriteHarvester)
					{
						text3 = "BioferriteHarvesterAttached".Translate();
					}
				}
				Rect rect9 = new Rect(0f, 0f, rect6.width, 28f);
				Widgets.CheckboxLabeled(rect9, "EntityStudyMode_Extract".Translate(), ref compHoldingPlatformTarget.extractBioferrite, text3 != null, null, null, false, false);
				Widgets.DrawHighlightIfMouseover(rect9);
				TaggedString taggedString2 = "EntityStudyMode_ExtractDesc".Translate();
				if (text3 != null)
				{
					taggedString2 += "\n\n" + text3.Colorize(ColoredText.WarningColor);
				}
				TooltipHandler.TipRegion(rect9, taggedString2);
				Widgets.EndGroup();
			}
			listing_Standard.End();
			this.size = new Vector2(280f, listing_Standard.CurHeight + 10f + 24f);
		}

		// Token: 0x06012926 RID: 76070 RVA: 0x0057BD20 File Offset: 0x00579F20
		public static void DoStudyPeriodListing(Listing_Standard listing, CompStudiable studiable)
		{
			if (studiable.Props.frequencyTicks > 0)
			{
				Widgets.DrawHighlightIfMouseover(listing.Label("StudyInterval".Translate() + ": " + studiable.Props.frequencyTicks.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor), -1f, "StudyIntervalDesc".Translate()));
			}
		}

		// Token: 0x06012927 RID: 76071 RVA: 0x0057BD94 File Offset: 0x00579F94
		public static void DoKnowledgeGainListing(Listing_Standard listing, CompStudiable studiable)
		{
			string text = "StudyKnowledgeGainDesc".Translate();
			CompHoldingPlatformTarget compHoldingPlatformTarget = studiable.Pawn.TryGetComp<CompHoldingPlatformTarget>();
			string text2 = "";
			if (compHoldingPlatformTarget != null && compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
			{
				CompEntityHolder comp = compHoldingPlatformTarget.HeldPlatform.GetComp<CompEntityHolder>();
				float studyKnowledgeAmountMultiplier = ContainmentUtility.GetStudyKnowledgeAmountMultiplier(studiable.Pawn, comp);
				text2 += string.Format("  - {0}: x{1:F1}", "FactorContainmentStrength".Translate(), studyKnowledgeAmountMultiplier);
				if (compHoldingPlatformTarget.HeldPlatform.HasAttachedElectroharvester)
				{
					text2 += string.Format("\n  - {0}: x{1:F1}", "FactorElectroharvester".Translate(), 0.5f);
				}
			}
			CompActivity compActivity;
			if (studiable.Pawn.TryGetComp(out compActivity))
			{
				if (text2 != "")
				{
					text2 += "\n";
				}
				text2 += string.Format("  - {0}: x{1:F1}", "FactorActivity".Translate(), compActivity.ActivityResearchFactor);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text = text + "\n\n" + text2;
			}
			Widgets.DrawHighlightIfMouseover(listing.Label("StudyKnowledgeGain".Translate() + ": " + (studiable.AdjustedAnomalyKnowledgePerStudy * 5f).ToStringDecimalIfSmall() + " (" + studiable.KnowledgeCategory.label + ")", -1f, text));
		}
	}
}
