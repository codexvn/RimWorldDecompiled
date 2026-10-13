using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020033A3 RID: 13219
	public class ITab_Pawn_Feeding : ITab
	{
		// Token: 0x1700309F RID: 12447
		// (get) Token: 0x06012948 RID: 76104 RVA: 0x0057CC74 File Offset: 0x0057AE74
		public override bool IsVisible
		{
			get
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				return ChildcareUtility.CanSuckle(this.SelPawn, out breastfeedFailReason) && ChildcareUtility.HasBreastfeedCompatibleFactions(Faction.OfPlayer, this.SelPawn);
			}
		}

		// Token: 0x170030A0 RID: 12448
		// (get) Token: 0x06012949 RID: 76105 RVA: 0x0057CCA4 File Offset: 0x0057AEA4
		public static List<ThingDef> BabyConsumableFoods
		{
			get
			{
				if (ITab_Pawn_Feeding.babyConsumableFoods.Count == 0)
				{
					ITab_Pawn_Feeding.babyConsumableFoods.AddRange(from t in DefDatabase<ThingDef>.AllDefs
					where t.ingestible != null && t.ingestible.babiesCanIngest
					select t);
				}
				return ITab_Pawn_Feeding.babyConsumableFoods;
			}
		}

		// Token: 0x0601294A RID: 76106 RVA: 0x0057CCF8 File Offset: 0x0057AEF8
		public ITab_Pawn_Feeding()
		{
			this.size = new Vector2(500f, 500f);
			this.labelKey = "TabFeeding";
			this.tutorTag = "Feeding";
		}

		// Token: 0x0601294B RID: 76107 RVA: 0x0057CD4C File Offset: 0x0057AF4C
		public static void FillTab(Pawn baby, Rect rect, ref Vector2 scrollPositionAutoBreastfeed, ref Vector2 scrollPositionBabyConsumables, List<Pawn> allowedPawns = null)
		{
			RectDivider rect2 = new RectDivider(rect.ContractedBy(10f, 10f), 2074307015, null);
			RectDivider rectDivider = rect2.NewRow(rect2.Rect.height * 0.66f, VerticalJustification.Top, null);
			Widgets.ListSeparator(ref rectDivider, "AutofeedSectionHeader".Translate().CapitalizeFirst());
			List<Pawn> list = ITab_Pawn_Feeding.tmpAllowedFeedingPawns;
			list.Clear();
			list.AddRange(PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction);
			list.Remove(baby);
			list.RemoveAll(delegate(Pawn feeder)
			{
				ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
				int num;
				return !feeder.RaceProps.Humanlike || ChildcareUtility.CanSuckle(feeder, out breastfeedFailReason) || feeder.IsWorkTypeDisabledByAge(WorkTypeDefOf.Childcare, out num) || (allowedPawns != null && !allowedPawns.Contains(feeder));
			});
			Pawn surrogate = baby.GetBirthParent();
			Pawn mother = baby.GetMother();
			Pawn father = baby.GetFather();
			list.Sort(delegate(Pawn lhs, Pawn rhs)
			{
				int num = rhs.health.hediffSet.HasHediff(HediffDefOf.Lactating, false).CompareTo(lhs.health.hediffSet.HasHediff(HediffDefOf.Lactating, false));
				if (num != 0)
				{
					return num;
				}
				if (lhs == mother)
				{
					return -1;
				}
				if (rhs == mother)
				{
					return 1;
				}
				if (lhs == father)
				{
					return -1;
				}
				if (rhs == father)
				{
					return 1;
				}
				if (lhs == surrogate)
				{
					return -1;
				}
				if (rhs == surrogate)
				{
					return 1;
				}
				return 0;
			});
			if (list.Count == 0)
			{
				using (new TextBlock(TextAnchor.MiddleCenter))
				{
					Widgets.Label(rectDivider.Rect, "AutofeedNone".Translate());
					goto IL_1D3;
				}
			}
			using (new TextBlock(TextAnchor.MiddleLeft))
			{
				RectDivider rect3 = rectDivider.CreateViewRect(list.Count, 28f);
				Widgets.BeginScrollView(rectDivider.Rect, ref scrollPositionAutoBreastfeed, rect3, true);
				foreach (Pawn feeder2 in list)
				{
					RectDivider rectDivider2 = rect3.NewRow(28f, VerticalJustification.Top, null);
					ITab_Pawn_Feeding.DrawRow(baby, ref rectDivider2, feeder2, mother, father, surrogate);
				}
				Widgets.EndScrollView();
			}
			IL_1D3:
			Widgets.ListSeparator(ref rect2, "BabyFoodConsumables".Translate().CapitalizeFirst());
			List<ThingDef> list2 = ITab_Pawn_Feeding.BabyConsumableFoods;
			RectDivider rect4 = rect2.CreateViewRect(list2.Count, Text.LineHeight);
			Widgets.BeginScrollView(rect2, ref scrollPositionBabyConsumables, rect4, true);
			foreach (ThingDef thingDef in list2)
			{
				RectDivider rect5 = rect4.NewRow(Text.LineHeight, VerticalJustification.Top, null);
				Widgets.DefIcon(rect5.NewCol(Text.LineHeight, HorizontalJustification.Left, null), thingDef, null, 1f, null, false, null, null, null, 1f);
				Rect rect6 = rect5.NewCol(24f, HorizontalJustification.Right, null);
				Widgets.InfoCardButton(rect6.x, rect6.y, thingDef);
				if (baby.foodRestriction != null)
				{
					Rect rect7 = rect5.NewCol(24f, HorizontalJustification.Right, null);
					bool allowed = baby.foodRestriction.BabyFoodAllowed(thingDef);
					Widgets.Checkbox(rect7.x + 12f, rect7.y, ref allowed, 24f, false, true, null, null);
					baby.foodRestriction.SetBabyFoodAllowed(thingDef, allowed);
				}
				Widgets.Label(rect5, thingDef.LabelCap);
				if (Mouse.IsOver(rect5))
				{
					Widgets.DrawHighlight(rect5);
					TooltipHandler.TipRegion(rect5, string.Format("{0}\n\n{1}", thingDef.LabelCap, thingDef.description));
				}
			}
			Widgets.EndScrollView();
		}

		// Token: 0x0601294C RID: 76108 RVA: 0x0057D16C File Offset: 0x0057B36C
		protected override void FillTab()
		{
			using (TextBlock.Default())
			{
				ITab_Pawn_Feeding.FillTab(this.SelPawn, new Rect(Vector2.zero, this.size), ref this.scrollPositionAutoBreastfeed, ref this.scrollPositionBabyConsumables, null);
			}
		}

		// Token: 0x0601294D RID: 76109 RVA: 0x0057D1C8 File Offset: 0x0057B3C8
		private static FloatMenuOption GenerateFloatMenuOption(AutofeedMode setting, ITab_Pawn_Feeding.BabyFeederPair pair)
		{
			FloatMenuOption floatMenuOption = new FloatMenuOption(setting.Translate().CapitalizeFirst(), delegate()
			{
				pair.baby.mindState.SetAutofeeder(pair.feeder, setting);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			AutofeedMode setting2 = setting;
			Pawn feeder = pair.feeder;
			floatMenuOption.tooltip = new TipSignal?(setting2.GetTooltip(pair.baby, feeder));
			return floatMenuOption;
		}

		// Token: 0x0601294E RID: 76110 RVA: 0x0057D251 File Offset: 0x0057B451
		private static IEnumerable<Widgets.DropdownMenuElement<AutofeedMode>> MenuGenerator(ITab_Pawn_Feeding.BabyFeederPair pair)
		{
			yield return new Widgets.DropdownMenuElement<AutofeedMode>
			{
				option = ITab_Pawn_Feeding.GenerateFloatMenuOption(AutofeedMode.Never, pair),
				payload = AutofeedMode.Never
			};
			yield return new Widgets.DropdownMenuElement<AutofeedMode>
			{
				option = ITab_Pawn_Feeding.GenerateFloatMenuOption(AutofeedMode.Childcare, pair),
				payload = AutofeedMode.Childcare
			};
			yield return new Widgets.DropdownMenuElement<AutofeedMode>
			{
				option = ITab_Pawn_Feeding.GenerateFloatMenuOption(AutofeedMode.Urgent, pair),
				payload = AutofeedMode.Urgent
			};
			yield break;
		}

		// Token: 0x0601294F RID: 76111 RVA: 0x0057D264 File Offset: 0x0057B464
		private static void DrawRow(Pawn baby, ref RectDivider row, Pawn feeder, Pawn mother, Pawn father, Pawn surrogate)
		{
			AutofeedMode mode = baby.mindState.AutofeedSetting(feeder);
			string text = feeder.LabelShortCap;
			Hediff firstHediffOfDef = feeder.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Lactating, false);
			if (firstHediffOfDef != null)
			{
				text = text + " (" + firstHediffOfDef.LabelBaseCap + ")";
			}
			Widgets.Label(row.NewCol(220f, HorizontalJustification.Left, null), text);
			Rect rect = row.NewCol(120f, HorizontalJustification.Left, null);
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, mode.GetTooltip(baby, feeder));
			}
			ITab_Pawn_Feeding.BabyFeederPair target = new ITab_Pawn_Feeding.BabyFeederPair
			{
				baby = baby,
				feeder = feeder
			};
			Widgets.Dropdown<ITab_Pawn_Feeding.BabyFeederPair, AutofeedMode>(rect, target, (ITab_Pawn_Feeding.BabyFeederPair _target) => _target.baby.mindState.AutofeedSetting(_target.feeder), new Func<ITab_Pawn_Feeding.BabyFeederPair, IEnumerable<Widgets.DropdownMenuElement<AutofeedMode>>>(ITab_Pawn_Feeding.MenuGenerator), mode.Translate().CapitalizeFirst(), null, null, null, null, true);
			if (feeder == mother)
			{
				Widgets.Label(row, PawnRelationDefOf.Parent.labelFemale.CapitalizeFirst());
				return;
			}
			if (feeder == father)
			{
				Widgets.Label(row, PawnRelationDefOf.Parent.label.CapitalizeFirst());
				return;
			}
			if (feeder == surrogate)
			{
				Widgets.Label(row, PawnRelationDefOf.ParentBirth.GetGenderSpecificLabelCap(feeder));
			}
		}

		// Token: 0x0400C0EF RID: 49391
		private const float FeederNameWidthPx = 220f;

		// Token: 0x0400C0F0 RID: 49392
		private const float BreastfeedOptWidthPx = 120f;

		// Token: 0x0400C0F1 RID: 49393
		private const int contextHash = 2074307015;

		// Token: 0x0400C0F2 RID: 49394
		public const float TabWidth = 500f;

		// Token: 0x0400C0F3 RID: 49395
		private Vector2 scrollPositionAutoBreastfeed = Vector2.zero;

		// Token: 0x0400C0F4 RID: 49396
		private Vector2 scrollPositionBabyConsumables = Vector2.zero;

		// Token: 0x0400C0F5 RID: 49397
		private static List<ThingDef> babyConsumableFoods = new List<ThingDef>();

		// Token: 0x0400C0F6 RID: 49398
		private static List<Pawn> tmpAllowedFeedingPawns = new List<Pawn>(8);

		// Token: 0x020033A4 RID: 13220
		private struct BabyFeederPair
		{
			// Token: 0x0400C0F7 RID: 49399
			public Pawn baby;

			// Token: 0x0400C0F8 RID: 49400
			public Pawn feeder;
		}
	}
}
