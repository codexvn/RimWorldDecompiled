using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033AF RID: 13231
	[StaticConstructorOnStartup]
	public abstract class ITab_Pawn_Visitor : ITab
	{
		// Token: 0x06012992 RID: 76178 RVA: 0x0057F783 File Offset: 0x0057D983
		public ITab_Pawn_Visitor()
		{
			this.size = new Vector2(280f, 0f);
		}

		// Token: 0x06012993 RID: 76179 RVA: 0x0057F7A0 File Offset: 0x0057D9A0
		protected override void FillTab()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.PrisonerTab, KnowledgeAmount.FrameDisplayed);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, this.size.x, this.size.y).ContractedBy(10f);
			bool isPrisonerOfColony = this.SelPawn.IsPrisonerOfColony;
			bool isSlaveOfColony = this.SelPawn.IsSlaveOfColony;
			bool wildMan = this.SelPawn.IsWildMan();
			Listing_Standard listing_Standard = new Listing_Standard();
			listing_Standard.maxOneColumn = true;
			listing_Standard.Begin(rect);
			if (!isSlaveOfColony && !isPrisonerOfColony)
			{
				Rect rect2 = listing_Standard.GetRect(28f, 1f);
				rect2.width = 140f;
				MedicalCareUtility.MedicalCareSetter(rect2, ref this.SelPawn.playerSettings.medCare);
				listing_Standard.Gap(4f);
			}
			if (isPrisonerOfColony)
			{
				this.DoPrisonerTab(wildMan, listing_Standard);
			}
			if (isSlaveOfColony)
			{
				this.DoSlaveTab(listing_Standard);
			}
			listing_Standard.End();
			this.size = new Vector2(280f, listing_Standard.CurHeight + 20f);
		}

		// Token: 0x06012994 RID: 76180 RVA: 0x0057F8A4 File Offset: 0x0057DAA4
		private void DoSlaveTab(Listing_Standard listing)
		{
			Need_Suppression need_Suppression;
			if (!this.SelPawn.needs.TryGetNeed<Need_Suppression>(out need_Suppression))
			{
				return;
			}
			Rect rect = listing.Label("Suppression".Translate() + ": " + need_Suppression.CurLevel.ToStringPercent(), -1f, null);
			Rect rect2 = listing.GetRect(30f, 1f);
			Rect rect3 = rect2.ContractedBy(7f);
			need_Suppression.DrawSuppressionBar(rect3);
			Rect rect4 = new Rect(rect.x, rect.y, rect.width, rect.height + rect2.height);
			Widgets.DrawHighlightIfMouseover(rect4);
			TaggedString str = "SuppressionDesc".Translate();
			TooltipHandler.TipRegion(rect4, str);
			float statValue = this.SelPawn.GetStatValue(StatDefOf.SlaveSuppressionFallRate, true, -1);
			string t5 = StatDefOf.SlaveSuppressionFallRate.ValueToString(statValue, ToStringNumberSense.Absolute, true);
			Rect rect5 = listing.Label("SuppressionFallRate".Translate() + ": " + t5, -1f, null);
			if (Mouse.IsOver(rect5))
			{
				TaggedString t2 = "SuppressionFallRateDesc".Translate(0.2f.ToStringPercent(), 0.3f.ToStringPercent(), 0.1f.ToStringPercent(), 0.15f.ToStringPercent(), 0.15f.ToStringPercent(), 0.05f.ToStringPercent(), 0.15f.ToStringPercent());
				string explanationForTooltip = ((StatWorker_SuppressionFallRate)StatDefOf.SlaveSuppressionFallRate.Worker).GetExplanationForTooltip(StatRequest.For(this.SelPawn));
				TooltipHandler.TipRegion(rect5, t2 + ":\n\n" + explanationForTooltip);
				Widgets.DrawHighlight(rect5);
			}
			Rect rect6 = listing.Label(string.Format("{0}: {1}", "Terror".Translate(), this.SelPawn.GetStatValue(StatDefOf.Terror, true, -1).ToStringPercent()), -1f, null);
			if (Mouse.IsOver(rect6))
			{
				IOrderedEnumerable<Thought_MemoryObservationTerror> source = from t in TerrorUtility.GetTerrorThoughts(this.SelPawn)
				orderby t.intensity descending
				select t;
				TaggedString taggedString = "TerrorDescription".Translate() + ":" + "\n\n" + (from p in TerrorUtility.SuppressionFallRateOverTerror.Points
				select string.Format("- {0} {1}: {2}", "Terror".Translate(), (p.x / 100f).ToStringPercent(), (p.y / 100f).ToStringPercent())).ToLineList(null, false);
				if (source.Any<Thought_MemoryObservationTerror>())
				{
					string t3 = (from t in source
					select string.Format("{0}: {1}%", t.LabelCap, t.intensity)).ToLineList("- ", true);
					taggedString += "\n\n" + "TerrorCurrentThoughts".Translate() + ":" + "\n\n" + t3;
				}
				TooltipHandler.TipRegion(rect6, taggedString);
				Widgets.DrawHighlight(rect6);
			}
			float num = SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(this.SelPawn);
			TaggedString taggedString2 = "SlaveRebellionMTBDays".Translate() + ": ";
			if (!this.SelPawn.Awake())
			{
				taggedString2 += "NotWhileAsleep".Translate();
			}
			else if (num < 0f)
			{
				taggedString2 += "Never".Translate();
			}
			else
			{
				taggedString2 += ((int)(num * 60000f)).ToStringTicksToPeriod(true, false, true, true, false);
			}
			Rect rect7 = listing.Label(taggedString2, -1f, null);
			TooltipHandler.TipRegion(rect7, delegate()
			{
				TaggedString taggedString3 = "SlaveRebellionMTBDaysDescription".Translate();
				string text3 = SlaveRebellionUtility.GetSlaveRebellionMtbCalculationExplanation(this.SelPawn) + "\n" + SlaveRebellionUtility.GetAnySlaveRebellionExplanation(this.SelPawn);
				if (!text3.NullOrEmpty())
				{
					taggedString3 += "\n\n" + text3;
				}
				return taggedString3;
			}, "SlaveRebellionTooltip".GetHashCode());
			Widgets.DrawHighlightIfMouseover(rect7);
			this.DoSlavePriceListing(listing);
			Faction faction = this.SelPawn.SlaveFaction ?? this.SelPawn.Faction;
			TaggedString t4;
			if (faction == null || faction.IsPlayer || !faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
			{
				t4 = "None".Translate();
			}
			else
			{
				bool flag;
				bool flag2;
				int i = faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, faction.GetGoodwillGainForExit(this.SelPawn, true, out flag, out flag2));
				if (flag && !flag2)
				{
					t4 = faction.NameColored + " " + i.ToStringWithSign();
				}
				else if (!flag)
				{
					t4 = "None".Translate() + " (" + "UntendedInjury".Translate() + ")";
				}
				else if (flag2)
				{
					t4 = "None".Translate() + " (" + this.SelPawn.MentalState.InspectLine + ")";
				}
				else
				{
					t4 = "None".Translate();
				}
			}
			Rect rect8 = listing.Label("SlaveReleasePotentialRelationGains".Translate() + ": " + t4, -1f, null);
			TooltipHandler.TipRegionByKey(rect8, "SlaveReleaseRelationGainsDesc");
			Widgets.DrawHighlightIfMouseover(rect8);
			ITab_Pawn_Visitor.tmpSlaveInteractionModes.Clear();
			ITab_Pawn_Visitor.tmpSlaveInteractionModes.AddRange(from pim in DefDatabase<SlaveInteractionModeDef>.AllDefs
			orderby pim.listOrder
			select pim);
			int num2 = 32 * ITab_Pawn_Visitor.tmpSlaveInteractionModes.Count;
			Rect rect9 = listing.GetRect((float)num2, 1f).Rounded();
			Widgets.DrawMenuSection(rect9);
			Rect rect10 = rect9.ContractedBy(10f);
			Widgets.BeginGroup(rect10);
			SlaveInteractionModeDef currentSlaveIteractionMode = this.SelPawn.guest.slaveInteractionMode;
			Rect rect11 = new Rect(0f, 0f, rect10.width, 30f);
			Action <>9__6;
			foreach (SlaveInteractionModeDef slaveInteractionModeDef in ITab_Pawn_Visitor.tmpSlaveInteractionModes)
			{
				if (Widgets.RadioButtonLabeled(rect11, slaveInteractionModeDef.LabelCap, this.SelPawn.guest.slaveInteractionMode == slaveInteractionModeDef, false))
				{
					if (slaveInteractionModeDef == SlaveInteractionModeDefOf.Imprison && RestUtility.FindBedFor(this.SelPawn, this.SelPawn, false, false, new GuestStatus?(GuestStatus.Prisoner)) == null)
					{
						Messages.Message("CannotImprison".Translate() + ": " + "NoPrisonerBed".Translate(), this.SelPawn, MessageTypeDefOf.RejectInput, false);
						continue;
					}
					this.SelPawn.guest.slaveInteractionMode = slaveInteractionModeDef;
					if (slaveInteractionModeDef == SlaveInteractionModeDefOf.Execute && this.SelPawn.SlaveFaction != null && !this.SelPawn.SlaveFaction.HostileTo(Faction.OfPlayer))
					{
						TaggedString text = "ExectueNeutralFactionSlave".Translate(this.SelPawn.Named("PAWN"), this.SelPawn.SlaveFaction.Named("FACTION"));
						string buttonAText = "Confirm".Translate();
						Action buttonAAction = delegate()
						{
						};
						string buttonBText = "Cancel".Translate();
						Action buttonBAction;
						if ((buttonBAction = <>9__6) == null)
						{
							buttonBAction = (<>9__6 = delegate()
							{
								this.SelPawn.guest.slaveInteractionMode = currentSlaveIteractionMode;
							});
						}
						Dialog_MessageBox window = new Dialog_MessageBox(text, buttonAText, buttonAAction, buttonBText, buttonBAction, null, false, null, null, WindowLayer.Dialog);
						Find.WindowStack.Add(window);
					}
				}
				if (!slaveInteractionModeDef.description.NullOrEmpty() && Mouse.IsOver(rect11))
				{
					Widgets.DrawHighlight(rect11);
					string text2 = slaveInteractionModeDef.description;
					if (slaveInteractionModeDef == SlaveInteractionModeDefOf.Emancipate)
					{
						if (this.SelPawn.SlaveFaction == Faction.OfPlayer)
						{
							text2 += " " + "EmancipateCololonistTooltip".Translate();
						}
						else if (this.SelPawn.SlaveFaction != null)
						{
							text2 += " " + "EmancipateNonCololonistWithFactionTooltip".Translate(this.SelPawn.SlaveFaction.NameColored);
						}
						else
						{
							text2 += " " + "EmancipateNonCololonistWithoutFactionTooltip".Translate();
						}
					}
					TooltipHandler.TipRegion(rect11, text2);
				}
				rect11.y += 28f;
			}
			Widgets.EndGroup();
			ITab_Pawn_Visitor.tmpSlaveInteractionModes.Clear();
		}

		// Token: 0x06012995 RID: 76181 RVA: 0x00580170 File Offset: 0x0057E370
		private void DoPrisonerTab(bool wildMan, Listing_Standard listing)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = (int)PrisonBreakUtility.InitiatePrisonBreakMtbDays(this.SelPawn, stringBuilder, true);
			string text = "PrisonBreakMTBDays".Translate() + ": ";
			if (PrisonBreakUtility.IsPrisonBreaking(this.SelPawn))
			{
				text += "CurrentlyPrisonBreaking".Translate();
			}
			else if (num < 0)
			{
				text += "Never".Translate();
				Gene gene;
				if (PrisonBreakUtility.GenePreventsPrisonBreaking(this.SelPawn, out gene))
				{
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.AppendTagged("PrisonBreakingDisabledDueToGene".Translate(gene.def.Named("GENE")).Colorize(ColorLibrary.RedReadable));
				}
			}
			else
			{
				text += "PeriodDays".Translate(num).ToString().Colorize(ColoredText.DateTimeColor);
			}
			Rect rect = listing.Label(text, -1f, null);
			string text2 = "PrisonBreakMTBDaysDescription".Translate();
			if (stringBuilder.Length > 0)
			{
				string str = text2;
				string str2 = "\n\n";
				StringBuilder stringBuilder2 = stringBuilder;
				text2 = str + str2 + ((stringBuilder2 != null) ? stringBuilder2.ToString() : null);
			}
			TooltipHandler.TipRegion(rect, text2);
			Widgets.DrawHighlightIfMouseover(rect);
			if (!wildMan)
			{
				if (this.SelPawn.guest.Recruitable)
				{
					float num2 = (this.SelPawn.guest.resistance > 0f) ? Mathf.Max(0.1f, this.SelPawn.guest.resistance) : 0f;
					Rect rect2 = listing.Label("RecruitmentResistance".Translate() + ": " + num2.ToString("F1").Colorize(ColoredText.DateTimeColor), -1f, null);
					if (Mouse.IsOver(rect2))
					{
						TaggedString taggedString = "RecruitmentResistanceDesc".Translate();
						FloatRange value = this.SelPawn.kindDef.initialResistanceRange.Value;
						taggedString += string.Format("\n\n{0}: {1}~{2}", "RecruitmentResistanceFromPawnKind".Translate(this.SelPawn.kindDef.LabelCap), value.min, value.max);
						if (this.SelPawn.royalty != null)
						{
							RoyalTitle mostSeniorTitle = this.SelPawn.royalty.MostSeniorTitle;
							if (mostSeniorTitle != null && mostSeniorTitle.def.recruitmentResistanceOffset != 0f)
							{
								string str3 = (mostSeniorTitle.def.recruitmentResistanceOffset > 0f) ? "+" : "-";
								taggedString += "\n" + "RecruitmentResistanceRoyalTitleOffset".Translate(mostSeniorTitle.Label.CapitalizeFirst()) + (": " + str3) + mostSeniorTitle.def.recruitmentResistanceOffset.ToString();
							}
						}
						TooltipHandler.TipRegion(rect2, taggedString);
					}
					Widgets.DrawHighlightIfMouseover(rect2);
				}
				else
				{
					Rect rect3 = listing.Label("RecruitmentResistance".Translate() + ": " + "NonRecruitable".Translate(), -1f, null);
					string str4 = "NonRecruitableTip".Translate();
					TooltipHandler.TipRegion(rect3, str4);
					Widgets.DrawHighlightIfMouseover(rect3);
				}
				if (ModsConfig.IdeologyActive)
				{
					Rect rect4 = listing.Label("WillLevel".Translate() + ": " + this.SelPawn.guest.will.ToString("F1").Colorize(ColoredText.DateTimeColor), -1f, null);
					TaggedString taggedString2 = "WillLevelDesc".Translate(2.5f);
					if (!this.SelPawn.guest.EverEnslaved)
					{
						FloatRange value2 = this.SelPawn.kindDef.initialWillRange.Value;
						taggedString2 += string.Format("\n\n{0} : {1}~{2}", "WillFromPawnKind".Translate(this.SelPawn.kindDef.LabelCap), value2.min, value2.max);
					}
					TooltipHandler.TipRegion(rect4, taggedString2);
					Widgets.DrawHighlightIfMouseover(rect4);
				}
			}
			this.DoSlavePriceListing(listing);
			if (ITab_Pawn_Visitor.IsStudiable(this.SelPawn))
			{
				CompStudiable compStudiable = this.SelPawn.TryGetComp<CompStudiable>();
				if (compStudiable != null)
				{
					ITab_Entity.DoStudyPeriodListing(listing, compStudiable);
				}
				if (compStudiable != null)
				{
					ITab_Entity.DoKnowledgeGainListing(listing, compStudiable);
				}
			}
			TaggedString t;
			if (this.SelPawn.Faction == null || this.SelPawn.Faction.IsPlayer || !this.SelPawn.Faction.CanChangeGoodwillFor(Faction.OfPlayer, 1))
			{
				t = "None".Translate();
			}
			else
			{
				bool flag;
				bool flag2;
				int i = this.SelPawn.Faction.CalculateAdjustedGoodwillChange(Faction.OfPlayer, this.SelPawn.Faction.GetGoodwillGainForExit(this.SelPawn, true, out flag, out flag2));
				if (flag && !flag2)
				{
					t = this.SelPawn.Faction.NameColored + " " + i.ToStringWithSign();
				}
				else if (!flag)
				{
					t = "None".Translate() + " (" + "UntendedInjury".Translate() + ")";
				}
				else
				{
					t = "None".Translate() + " (" + this.SelPawn.MentalState.InspectLine + ")";
				}
			}
			Rect rect5 = listing.Label("PrisonerReleasePotentialRelationGains".Translate() + ": " + t, -1f, null);
			TooltipHandler.TipRegionByKey(rect5, "PrisonerReleaseRelationGainsDesc");
			Widgets.DrawHighlightIfMouseover(rect5);
			if (this.SelPawn.guilt.IsGuilty)
			{
				if (!this.SelPawn.InAggroMentalState)
				{
					listing.Label("ConsideredGuilty".Translate(this.SelPawn.guilt.TicksUntilInnocent.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)), -1f, null);
				}
				else
				{
					listing.Label("ConsideredGuiltyNoTimer".Translate() + " (" + this.SelPawn.MentalStateDef.label + ")", -1f, null);
				}
			}
			if (ModsConfig.IdeologyActive && this.SelPawn.guest.IsInteractionEnabled(PrisonerInteractionModeDefOf.Convert) && this.SelPawn.guest.ideoForConversion != null)
			{
				Rect rect6 = listing.GetRect(24f, 1f);
				Rect rect7 = new Rect(rect6.xMax - 24f - 4f, rect6.y, 24f, 24f);
				rect6.xMax = rect7.xMin;
				Text.Anchor = TextAnchor.MiddleLeft;
				Widgets.Label(rect6, "IdeoConversionTarget".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				Widgets.DrawHighlightIfMouseover(rect6);
				TooltipHandler.TipRegionByKey(rect6, "IdeoConversionTargetDesc");
				this.SelPawn.guest.ideoForConversion.DrawIcon(rect7.ContractedBy(2f));
				if (Mouse.IsOver(rect7))
				{
					Widgets.DrawHighlight(rect7);
					TooltipHandler.TipRegion(rect7, this.SelPawn.guest.ideoForConversion.name);
				}
				if (Widgets.ButtonInvisible(rect7, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
					{
						Ideo newIdeo = ideo;
						string text3 = ideo.name;
						Action action = delegate()
						{
							this.SelPawn.guest.ideoForConversion = newIdeo;
						};
						if (!this.ColonyHasAnyWardenOfIdeo(newIdeo, this.SelPawn.MapHeld))
						{
							text3 += " (" + "NoWardenOfIdeo".Translate(newIdeo.memberName.Named("MEMBERNAME")) + ")";
							action = null;
						}
						list.Add(new FloatMenuOption(text3, action, newIdeo.Icon, newIdeo.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
			}
			if (this.SelPawn.guest.finalResistanceInteractionData != null)
			{
				ResistanceInteractionData finalResistanceInteractionData = this.SelPawn.guest.finalResistanceInteractionData;
				Rect rect8 = listing.Label("LastRecruitment".Translate() + ": " + finalResistanceInteractionData.resistanceReduction.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute), -1f, null);
				if (Mouse.IsOver(rect8))
				{
					Widgets.DrawHighlight(rect8);
					TaggedString taggedString3 = "LastRecruitmentDescription".Translate(this.SelPawn, finalResistanceInteractionData.initiatorName);
					taggedString3 += "\n\n";
					taggedString3 += "StatsReport_BaseValue".Translate() + ": " + 1f.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute);
					taggedString3 += "\n\n" + "Mood".Translate() + ": x" + finalResistanceInteractionData.recruiteeMoodFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute);
					taggedString3 += "\n" + "RecruiterNegotiationAbility".Translate() + ": x" + finalResistanceInteractionData.initiatorNegotiationAbilityFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute);
					taggedString3 += "\n" + "OpinionOfRecruiter".Translate() + ": x" + finalResistanceInteractionData.recruiterOpinionFactor.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute);
					taggedString3 += "\n" + "StatsReport_FinalValue".Translate() + ": " + finalResistanceInteractionData.resistanceReduction.ToStringByStyle(ToStringStyle.FloatTwo, ToStringNumberSense.Absolute);
					TooltipHandler.TipRegion(rect8, taggedString3);
				}
			}
			listing.Gap(1f);
			Rect rect9 = listing.GetRect(24f, 1f).Rounded();
			TooltipHandler.TipRegionByKey(rect9, "MedicineQualityDescriptionPrisoner");
			Widgets.DrawHighlightIfMouseover(rect9);
			Rect rect10 = rect9;
			rect10.xMax = rect9.center.x - 4f;
			Rect rect11 = rect9;
			rect11.xMin = rect9.center.x + 4f;
			Text.Anchor = TextAnchor.MiddleLeft;
			Widgets.Label(rect10, string.Format("{0}:", "AllowMedicine".Translate()));
			Text.Anchor = TextAnchor.UpperLeft;
			Widgets.DrawButtonGraphic(rect11);
			MedicalCareUtility.MedicalCareSelectButton(rect11, this.SelPawn);
			listing.Gap(4f);
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.AddRange(from m in DefDatabase<PrisonerInteractionModeDef>.AllDefs
			where !m.isNonExclusiveInteraction && base.<DoPrisonerTab>g__CanUsePrisonerInteractionMode|0(this.SelPawn, m)
			select m into pim
			orderby pim.listOrder
			select pim);
			float height = 28f * (float)ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Count + 20f;
			Rect rect12 = listing.GetRect(height, 1f).Rounded();
			Widgets.DrawMenuSection(rect12);
			Rect rect13 = rect12.ContractedBy(10f);
			Widgets.BeginGroup(rect13);
			Rect rect14 = new Rect(0f, 0f, rect13.width, 28f);
			foreach (PrisonerInteractionModeDef mode in ITab_Pawn_Visitor.tmpPrisonerInteractionModes)
			{
				this.DrawExclusiveInteractionRow(ref rect14, mode);
			}
			Widgets.EndGroup();
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.AddRange(from m in DefDatabase<PrisonerInteractionModeDef>.AllDefs
			where m.isNonExclusiveInteraction && base.<DoPrisonerTab>g__CanUsePrisonerInteractionMode|0(this.SelPawn, m)
			select m into pim
			orderby pim.listOrder
			select pim);
			if (ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Any<PrisonerInteractionModeDef>())
			{
				listing.Gap(12f);
				height = 28f * (float)ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Count + 20f;
				Rect rect15 = listing.GetRect(height, 1f).Rounded();
				Widgets.DrawMenuSection(rect15);
				rect13 = rect15.ContractedBy(10f);
				Widgets.BeginGroup(rect13);
				Rect rect16 = new Rect(0f, 0f, rect13.width, 28f);
				foreach (PrisonerInteractionModeDef mode2 in ITab_Pawn_Visitor.tmpPrisonerInteractionModes)
				{
					this.DrawNonExclusiveInteractionRow(ref rect16, mode2);
				}
				Widgets.EndGroup();
			}
			ITab_Pawn_Visitor.tmpPrisonerInteractionModes.Clear();
		}

		// Token: 0x06012996 RID: 76182 RVA: 0x00580EFC File Offset: 0x0057F0FC
		private static bool IsStudiable(Pawn pawn)
		{
			CompStudiable compStudiable;
			return ModsConfig.AnomalyActive && pawn.TryGetComp(out compStudiable) && compStudiable.EverStudiable() && pawn.kindDef.studiableAsPrisoner && !pawn.everLostEgo;
		}

		// Token: 0x06012997 RID: 76183 RVA: 0x00580F40 File Offset: 0x0057F140
		private void DrawExclusiveInteractionRow(ref Rect rect, PrisonerInteractionModeDef mode)
		{
			float xMin = rect.xMin;
			if (mode.isChildInteraction)
			{
				Rect outerRect = rect;
				outerRect.x -= 3f;
				outerRect.width = outerRect.height;
				Widgets.DrawTextureFitted(outerRect, ITab_Pawn_Visitor.Subarrow, 0.75f, 1f);
				rect.xMin = outerRect.xMax + 2f;
			}
			if (Widgets.RadioButtonLabeled(rect, mode.LabelCap, this.SelPawn.guest.ExclusiveInteractionMode == mode, false))
			{
				this.SelPawn.guest.SetExclusiveInteraction(mode);
				this.InteractionModeChanged(mode);
			}
			if (!mode.description.NullOrEmpty() && Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, mode.description);
			}
			rect.y += 28f;
			rect.xMin = xMin;
		}

		// Token: 0x06012998 RID: 76184 RVA: 0x00581044 File Offset: 0x0057F244
		private void DrawNonExclusiveInteractionRow(ref Rect rect, PrisonerInteractionModeDef mode)
		{
			bool flag = this.SelPawn.guest.IsInteractionEnabled(mode);
			bool flag2 = flag;
			Widgets.CheckboxLabeled(rect, mode.LabelCap, ref flag, false, null, null, false, true);
			if (!mode.description.NullOrEmpty() && Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, mode.description);
			}
			rect.y += 28f;
			if (flag == flag2)
			{
				return;
			}
			this.SelPawn.guest.ToggleNonExclusiveInteraction(mode, flag);
			this.NonExclusiveInteractionToggled(mode, flag);
		}

		// Token: 0x06012999 RID: 76185 RVA: 0x005810F0 File Offset: 0x0057F2F0
		private void InteractionModeChanged(PrisonerInteractionModeDef newMode)
		{
			if (newMode == PrisonerInteractionModeDefOf.Enslave && this.SelPawn.MapHeld != null && !this.ColonyHasAnyWardenCapableOfEnslavement(this.SelPawn.MapHeld))
			{
				Messages.Message("MessageNoWardenCapableOfEnslavement".Translate(), this.SelPawn, MessageTypeDefOf.CautionInput, false);
			}
			if (newMode == PrisonerInteractionModeDefOf.Convert && this.SelPawn.guest.ideoForConversion == null)
			{
				this.SelPawn.guest.ideoForConversion = Faction.OfPlayer.ideos.PrimaryIdeo;
			}
			if (newMode == PrisonerInteractionModeDefOf.Execution && this.SelPawn.MapHeld != null && !this.ColonyHasAnyWardenCapableOfViolence(this.SelPawn.MapHeld))
			{
				Messages.Message("MessageCantDoExecutionBecauseNoWardenCapableOfViolence".Translate(), this.SelPawn, MessageTypeDefOf.CautionInput, false);
			}
		}

		// Token: 0x0601299A RID: 76186 RVA: 0x005811D0 File Offset: 0x0057F3D0
		public void NonExclusiveInteractionToggled(PrisonerInteractionModeDef mode, bool enabled)
		{
			if (ModsConfig.BiotechActive && mode == PrisonerInteractionModeDefOf.HemogenFarm)
			{
				BillStack billStack = this.SelPawn.BillStack;
				Bill bill;
				if (billStack == null)
				{
					bill = null;
				}
				else
				{
					List<Bill> bills = billStack.Bills;
					if (bills == null)
					{
						bill = null;
					}
					else
					{
						bill = bills.FirstOrDefault((Bill x) => x.recipe == RecipeDefOf.ExtractHemogenPack);
					}
				}
				Bill bill2 = bill;
				if (enabled)
				{
					if (bill2 == null && SanguophageUtility.CanSafelyBeQueuedForHemogenExtraction(this.SelPawn))
					{
						HealthCardUtility.CreateSurgeryBill(this.SelPawn, RecipeDefOf.ExtractHemogenPack, null, null, true);
						return;
					}
				}
				else if (bill2 != null)
				{
					this.SelPawn.BillStack.Bills.Remove(bill2);
				}
			}
		}

		// Token: 0x0601299B RID: 76187 RVA: 0x00581278 File Offset: 0x0057F478
		private void DoSlavePriceListing(Listing_Standard listing)
		{
			float statValue = this.SelPawn.GetStatValue(StatDefOf.MarketValue, true, -1);
			Rect rect = listing.Label("SlavePrice".Translate() + ": " + statValue.ToStringMoney(null), -1f, null);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TaggedString str = "SlavePriceDescription".Translate() + "\n\n" + StatDefOf.MarketValue.Worker.GetExplanationFull(StatRequest.For(this.SelPawn), StatDefOf.MarketValue.toStringNumberSense, statValue);
				TooltipHandler.TipRegion(rect, str);
			}
		}

		// Token: 0x0601299C RID: 76188 RVA: 0x00581320 File Offset: 0x0057F520
		private bool ColonyHasAnyBloodfeeder(Map map)
		{
			if (ModsConfig.BiotechActive)
			{
				using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsAndPrisonersSpawned.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.IsBloodfeeder())
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x0601299D RID: 76189 RVA: 0x00581388 File Offset: 0x0057F588
		private bool ColonyHasAnyWardenCapableOfViolence(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && !pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601299E RID: 76190 RVA: 0x005813FC File Offset: 0x0057F5FC
		private bool ColonyHasAnyWardenCapableOfEnslavement(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && new HistoryEvent(HistoryEventDefOf.EnslavedPrisoner, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0601299F RID: 76191 RVA: 0x00581484 File Offset: 0x0057F684
		private bool ColonyHasAnyWardenOfIdeo(Ideo ideo, Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.workSettings.WorkIsActive(WorkTypeDefOf.Warden) && pawn.Ideo == ideo)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400C12B RID: 49451
		private static readonly List<PrisonerInteractionModeDef> tmpPrisonerInteractionModes = new List<PrisonerInteractionModeDef>();

		// Token: 0x0400C12C RID: 49452
		private static readonly List<SlaveInteractionModeDef> tmpSlaveInteractionModes = new List<SlaveInteractionModeDef>();

		// Token: 0x0400C12D RID: 49453
		private static readonly Texture2D Subarrow = ContentFinder<Texture2D>.Get("UI/Misc/Subarrow", true);

		// Token: 0x0400C12E RID: 49454
		private const float SuppressionBarHeight = 30f;

		// Token: 0x0400C12F RID: 49455
		private const float SuppressionBarMargin = 7f;
	}
}
