using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02003232 RID: 12850
	[StaticConstructorOnStartup]
	public static class PermitsCardUtility
	{
		// Token: 0x17002EBD RID: 11965
		// (get) Token: 0x06012101 RID: 73985 RVA: 0x005478F8 File Offset: 0x00545AF8
		private static bool ShowSwitchFactionButton
		{
			get
			{
				int num = 0;
				foreach (Faction faction in Find.FactionManager.AllFactionsVisible)
				{
					if (!faction.IsPlayer && !faction.def.permanentEnemy && !faction.temporary)
					{
						using (IEnumerator<RoyalTitlePermitDef> enumerator2 = DefDatabase<RoyalTitlePermitDef>.AllDefs.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								if (enumerator2.Current.faction == faction.def)
								{
									num++;
									break;
								}
							}
						}
					}
				}
				return num > 1;
			}
		}

		// Token: 0x06012102 RID: 73986 RVA: 0x005479AC File Offset: 0x00545BAC
		private static int TotalReturnPermitsCost(Pawn pawn)
		{
			int num = 8;
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].OnCooldown && allFactionPermits[i].Permit.royalAid != null)
				{
					num += allFactionPermits[i].Permit.royalAid.favorCost;
				}
			}
			return num;
		}

		// Token: 0x06012103 RID: 73987 RVA: 0x00547A14 File Offset: 0x00545C14
		public static void DrawRecordsCard(Rect rect, Pawn pawn)
		{
			if (!ModLister.CheckRoyalty("Permit"))
			{
				return;
			}
			rect.yMax -= 4f;
			if (PermitsCardUtility.ShowSwitchFactionButton)
			{
				Rect rect2 = new Rect(rect.x, rect.y, 32f, 32f);
				if (Widgets.ButtonImage(rect2, PermitsCardUtility.SwitchFactionIcon, true, null))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Faction faction in Find.FactionManager.AllFactionsVisibleInViewOrder)
					{
						if (!faction.IsPlayer && !faction.def.permanentEnemy)
						{
							Faction localFaction = faction;
							list.Add(new FloatMenuOption(localFaction.Name, delegate()
							{
								PermitsCardUtility.selectedFaction = localFaction;
								PermitsCardUtility.selectedPermit = null;
							}, localFaction.def.FactionIcon, localFaction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, HorizontalJustification.Left, false));
						}
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				TooltipHandler.TipRegion(rect2, "SwitchFaction_Desc".Translate());
			}
			if (PermitsCardUtility.selectedFaction.def.HasRoyalTitles)
			{
				string label = "ReturnAllPermits".Translate();
				Rect rect3 = new Rect(rect.xMax - 180f, rect.y - 4f, 180f, 51f);
				int num = PermitsCardUtility.TotalReturnPermitsCost(pawn);
				if (Widgets.ButtonText(rect3, label, true, true, true, null))
				{
					if (!pawn.royalty.PermitsFromFaction(PermitsCardUtility.selectedFaction).Any<FactionPermit>())
					{
						Messages.Message("NoPermitsToReturn".Translate(pawn.Named("PAWN")), new LookTargets(pawn), MessageTypeDefOf.RejectInput, false);
					}
					else if (pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction) < num)
					{
						Messages.Message("NotEnoughFavor".Translate(num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR"), pawn.Named("PAWN"), pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction).Named("CURFAVOR")), MessageTypeDefOf.RejectInput, true);
					}
					else
					{
						string str = "ReturnAllPermits_Confirm".Translate(8.Named("BASEFAVORCOST"), num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR"), PermitsCardUtility.selectedFaction.Named("FACTION"));
						Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, delegate()
						{
							pawn.royalty.RefundPermits(8, PermitsCardUtility.selectedFaction);
						}, true, null, WindowLayer.Dialog));
					}
				}
				TooltipHandler.TipRegion(rect3, "ReturnAllPermits_Desc".Translate(8.Named("BASEFAVORCOST"), num.Named("FAVORCOST"), PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("FAVOR")));
			}
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(PermitsCardUtility.selectedFaction);
			Rect rect4 = new Rect(rect.xMax - 360f - 4f, rect.y - 4f, 360f, 55f);
			string text = "CurrentTitle".Translate() + ": " + ((currentTitle != null) ? currentTitle.GetLabelFor(pawn).CapitalizeFirst() : "None".Translate()) + "\n" + "UnusedPermits".Translate() + ": " + pawn.royalty.GetPermitPoints(PermitsCardUtility.selectedFaction).ToString();
			if (!PermitsCardUtility.selectedFaction.def.royalFavorLabel.NullOrEmpty())
			{
				text = string.Concat(new string[]
				{
					text,
					"\n",
					PermitsCardUtility.selectedFaction.def.royalFavorLabel.CapitalizeFirst(),
					": ",
					pawn.royalty.GetFavor(PermitsCardUtility.selectedFaction).ToString()
				});
			}
			Widgets.Label(rect4, text);
			rect.yMin += 55f;
			Rect rect5 = new Rect(rect);
			rect5.width *= 0.33f;
			PermitsCardUtility.DoLeftRect(rect5, pawn);
			PermitsCardUtility.DoRightRect(new Rect(rect)
			{
				xMin = rect5.xMax + 10f
			}, pawn);
		}

		// Token: 0x06012104 RID: 73988 RVA: 0x00547F50 File Offset: 0x00546150
		private static void DoLeftRect(Rect rect, Pawn pawn)
		{
			float num = 0f;
			RoyalTitleDef currentTitle = pawn.royalty.GetCurrentTitle(PermitsCardUtility.selectedFaction);
			Rect rect2 = new Rect(rect);
			Widgets.BeginGroup(rect2);
			if (PermitsCardUtility.selectedPermit != null)
			{
				Text.Font = GameFont.Medium;
				Rect rect3 = new Rect(0f, num, rect2.width, 0f);
				Widgets.LabelCacheHeight(ref rect3, PermitsCardUtility.selectedPermit.LabelCap, true, false);
				Text.Font = GameFont.Small;
				num += rect3.height;
				if (!PermitsCardUtility.selectedPermit.description.NullOrEmpty())
				{
					Rect rect4 = new Rect(0f, num, rect2.width, 0f);
					Widgets.LabelCacheHeight(ref rect4, PermitsCardUtility.selectedPermit.description, true, false);
					num += rect4.height + 16f;
				}
				Rect rect5 = new Rect(0f, num, rect2.width, 0f);
				string text = "Cooldown".Translate() + ": " + "PeriodDays".Translate(PermitsCardUtility.selectedPermit.cooldownDays);
				if (PermitsCardUtility.selectedPermit.royalAid != null && PermitsCardUtility.selectedPermit.royalAid.favorCost > 0 && !PermitsCardUtility.selectedFaction.def.royalFavorLabel.NullOrEmpty())
				{
					text = text + ("\n" + "CooldownUseFavorCost".Translate(PermitsCardUtility.selectedFaction.def.royalFavorLabel.Named("HONOR")).CapitalizeFirst() + ": ") + PermitsCardUtility.selectedPermit.royalAid.favorCost.ToString();
				}
				if (PermitsCardUtility.selectedPermit.minTitle != null)
				{
					text = text + "\n" + "RequiresTitle".Translate(PermitsCardUtility.selectedPermit.minTitle.GetLabelForBothGenders()).Resolve().Colorize((currentTitle != null && currentTitle.seniority >= PermitsCardUtility.selectedPermit.minTitle.seniority) ? Color.white : ColorLibrary.RedReadable);
				}
				if (PermitsCardUtility.selectedPermit.prerequisite != null)
				{
					text = text + "\n" + "UpgradeFrom".Translate(PermitsCardUtility.selectedPermit.prerequisite.LabelCap).Resolve().Colorize(PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit.prerequisite, pawn) ? Color.white : ColorLibrary.RedReadable);
				}
				Widgets.LabelCacheHeight(ref rect5, text, true, false);
				num += rect5.height + 4f;
				Rect rect6 = new Rect(0f, rect2.height - 50f, rect2.width, 50f);
				if (PermitsCardUtility.selectedPermit.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction) && !PermitsCardUtility.PermitUnlocked(PermitsCardUtility.selectedPermit, pawn) && Widgets.ButtonText(rect6, "AcceptPermit".Translate(), true, true, true, null))
				{
					SoundDefOf.Quest_Accepted.PlayOneShotOnCamera(null);
					pawn.royalty.AddPermit(PermitsCardUtility.selectedPermit, PermitsCardUtility.selectedFaction);
				}
			}
			Widgets.EndGroup();
		}

		// Token: 0x06012105 RID: 73989 RVA: 0x00548284 File Offset: 0x00546484
		private static void DoRightRect(Rect rect, Pawn pawn)
		{
			Widgets.DrawMenuSection(rect);
			if (PermitsCardUtility.selectedFaction == null)
			{
				return;
			}
			List<RoyalTitlePermitDef> allDefsListForReading = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;
			Rect outRect = rect.ContractedBy(10f);
			Rect rect2 = default(Rect);
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				RoyalTitlePermitDef permit = allDefsListForReading[i];
				if (PermitsCardUtility.CanDrawPermit(permit))
				{
					rect2.width = Mathf.Max(rect2.width, PermitsCardUtility.DrawPosition(permit).x + 200f + 26f);
					rect2.height = Mathf.Max(rect2.height, PermitsCardUtility.DrawPosition(permit).y + 50f + 26f);
				}
			}
			Widgets.BeginScrollView(outRect, ref PermitsCardUtility.rightScrollPosition, rect2, true);
			Widgets.BeginGroup(rect2.ContractedBy(10f));
			PermitsCardUtility.DrawLines();
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				RoyalTitlePermitDef royalTitlePermitDef = allDefsListForReading[j];
				if (PermitsCardUtility.CanDrawPermit(royalTitlePermitDef))
				{
					Vector2 vector = PermitsCardUtility.DrawPosition(royalTitlePermitDef);
					Rect rect3 = new Rect(vector.x, vector.y, 200f, 50f);
					Color color = Widgets.NormalOptionColor;
					Color color2 = PermitsCardUtility.PermitUnlocked(royalTitlePermitDef, pawn) ? TexUI.OldFinishedResearchColor : TexUI.AvailResearchColor;
					Color borderColor;
					if (PermitsCardUtility.selectedPermit == royalTitlePermitDef)
					{
						borderColor = TexUI.HighlightBorderResearchColor;
						color2 += TexUI.HighlightBgResearchColor;
					}
					else
					{
						borderColor = TexUI.DefaultBorderResearchColor;
					}
					if (!royalTitlePermitDef.AvailableForPawn(pawn, PermitsCardUtility.selectedFaction) && !PermitsCardUtility.PermitUnlocked(royalTitlePermitDef, pawn))
					{
						color = Color.red;
					}
					if (Widgets.CustomButtonText(ref rect3, string.Empty, color2, color, borderColor, default(Color), false, 1f, true, true, 1f))
					{
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						PermitsCardUtility.selectedPermit = royalTitlePermitDef;
					}
					TextAnchor anchor = Text.Anchor;
					Color color3 = GUI.color;
					GUI.color = color;
					Text.Anchor = TextAnchor.MiddleCenter;
					Widgets.Label(rect3, royalTitlePermitDef.LabelCap);
					GUI.color = color3;
					Text.Anchor = anchor;
				}
			}
			Widgets.EndGroup();
			Widgets.EndScrollView();
		}

		// Token: 0x06012106 RID: 73990 RVA: 0x0054848C File Offset: 0x0054668C
		private static void DrawLines()
		{
			Vector2 start = default(Vector2);
			Vector2 end = default(Vector2);
			List<RoyalTitlePermitDef> allDefsListForReading = DefDatabase<RoyalTitlePermitDef>.AllDefsListForReading;
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < allDefsListForReading.Count; j++)
				{
					RoyalTitlePermitDef royalTitlePermitDef = allDefsListForReading[j];
					if (PermitsCardUtility.CanDrawPermit(royalTitlePermitDef))
					{
						Vector2 vector = PermitsCardUtility.DrawPosition(royalTitlePermitDef);
						start.x = vector.x;
						start.y = vector.y + 25f;
						RoyalTitlePermitDef prerequisite = royalTitlePermitDef.prerequisite;
						if (prerequisite != null)
						{
							Vector2 vector2 = PermitsCardUtility.DrawPosition(prerequisite);
							end.x = vector2.x + 200f;
							end.y = vector2.y + 25f;
							if ((i == 1 && PermitsCardUtility.selectedPermit == royalTitlePermitDef) || PermitsCardUtility.selectedPermit == prerequisite)
							{
								Widgets.DrawLine(start, end, TexUI.HighlightLineResearchColor, 4f);
							}
							else if (i == 0)
							{
								Widgets.DrawLine(start, end, TexUI.DefaultLineResearchColor, 2f);
							}
						}
					}
				}
			}
		}

		// Token: 0x06012107 RID: 73991 RVA: 0x00548598 File Offset: 0x00546798
		private static bool PermitUnlocked(RoyalTitlePermitDef permit, Pawn pawn)
		{
			if (pawn.royalty.HasPermit(permit, PermitsCardUtility.selectedFaction))
			{
				return true;
			}
			List<FactionPermit> allFactionPermits = pawn.royalty.AllFactionPermits;
			for (int i = 0; i < allFactionPermits.Count; i++)
			{
				if (allFactionPermits[i].Permit.prerequisite == permit && allFactionPermits[i].Faction == PermitsCardUtility.selectedFaction)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06012108 RID: 73992 RVA: 0x00548601 File Offset: 0x00546801
		private static Vector2 DrawPosition(RoyalTitlePermitDef permit)
		{
			Vector2 a = new Vector2(permit.uiPosition.x * 200f, permit.uiPosition.y * 50f);
			return a + a * PermitsCardUtility.PermitOptionSpacing;
		}

		// Token: 0x06012109 RID: 73993 RVA: 0x0054863A File Offset: 0x0054683A
		private static bool CanDrawPermit(RoyalTitlePermitDef permit)
		{
			return permit.permitPointCost > 0 && (permit.faction == null || permit.faction == PermitsCardUtility.selectedFaction.def);
		}

		// Token: 0x0400BB7E RID: 47998
		private static Vector2 rightScrollPosition;

		// Token: 0x0400BB7F RID: 47999
		public static RoyalTitlePermitDef selectedPermit;

		// Token: 0x0400BB80 RID: 48000
		public static Faction selectedFaction;

		// Token: 0x0400BB81 RID: 48001
		private const float LeftRectPercent = 0.33f;

		// Token: 0x0400BB82 RID: 48002
		private const float TitleHeight = 55f;

		// Token: 0x0400BB83 RID: 48003
		private const float ReturnButtonWidth = 180f;

		// Token: 0x0400BB84 RID: 48004
		private const float PermitOptionWidth = 200f;

		// Token: 0x0400BB85 RID: 48005
		private const float PermitOptionHeight = 50f;

		// Token: 0x0400BB86 RID: 48006
		private const float AcceptButtonHeight = 50f;

		// Token: 0x0400BB87 RID: 48007
		private const float SwitchFactionsButtonSize = 32f;

		// Token: 0x0400BB88 RID: 48008
		private const float LineWidthNotSelected = 2f;

		// Token: 0x0400BB89 RID: 48009
		private const float LineWidthSelected = 4f;

		// Token: 0x0400BB8A RID: 48010
		private const int ReturnPermitsCost = 8;

		// Token: 0x0400BB8B RID: 48011
		private static readonly Vector2 PermitOptionSpacing = new Vector2(0.25f, 0.35f);

		// Token: 0x0400BB8C RID: 48012
		private static readonly Texture2D SwitchFactionIcon = ContentFinder<Texture2D>.Get("UI/Icons/SwitchFaction", true);
	}
}
