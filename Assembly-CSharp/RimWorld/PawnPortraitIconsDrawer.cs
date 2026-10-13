using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020034A7 RID: 13479
	[StaticConstructorOnStartup]
	public static class PawnPortraitIconsDrawer
	{
		// Token: 0x17003161 RID: 12641
		// (get) Token: 0x06012F3E RID: 77630 RVA: 0x0059DACC File Offset: 0x0059BCCC
		private static Texture2D SlaveryIcon
		{
			get
			{
				if (PawnPortraitIconsDrawer.slaveryIcon == null)
				{
					PawnPortraitIconsDrawer.slaveryIcon = ContentFinder<Texture2D>.Get("UI/Icons/Slavery", true);
				}
				return PawnPortraitIconsDrawer.slaveryIcon;
			}
		}

		// Token: 0x06012F3F RID: 77631 RVA: 0x0059DAF0 File Offset: 0x0059BCF0
		private static void CalculatePawnPortraitIcons(Pawn pawn, bool required, bool showIdeoIcon)
		{
			using (new ProfilerBlock("CalculatePawnPortraitIcons"))
			{
				PawnPortraitIconsDrawer.tmpPortraitIcons.Clear();
				Ideo ideo = pawn.Ideo;
				if (required)
				{
					PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
					{
						color = Color.white,
						icon = IdeoUIUtility.LockedTex,
						tooltip = "Required".Translate()
					});
				}
				if (ModsConfig.IdeologyActive && ideo != null)
				{
					if (!Find.IdeoManager.classicMode && showIdeoIcon)
					{
						PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
						{
							color = ideo.Color,
							icon = ideo.Icon,
							tooltip = ideo.memberName
						});
						Precept_Role role = ideo.GetRole(pawn);
						if (role != null)
						{
							PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
							{
								color = ideo.Color,
								icon = role.Icon,
								tooltip = role.TipLabel
							});
						}
						GUI.color = Color.white;
					}
					Faction homeFaction = pawn.HomeFaction;
					if (homeFaction != null && !homeFaction.IsPlayer && !homeFaction.Hidden)
					{
						PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
						{
							color = homeFaction.Color,
							icon = homeFaction.def.FactionIcon,
							tooltip = "Faction".Translate() + ": " + homeFaction.Name + "\n" + homeFaction.def.LabelCap
						});
					}
					if (pawn.IsSlave)
					{
						PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
						{
							color = Color.white,
							icon = PawnPortraitIconsDrawer.SlaveryIcon,
							tooltip = "RitualBeginSlaveDesc".Translate()
						});
					}
					if (pawn.IsPrisoner)
					{
						PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
						{
							color = Color.white,
							icon = PawnPortraitIconsDrawer.prisonerIcon,
							tooltip = null
						});
					}
					if (!pawn.Awake())
					{
						PawnPortraitIconsDrawer.tmpPortraitIcons.Add(new PawnPortraitIconsDrawer.PawnPortraitIcon
						{
							color = Color.white,
							icon = PawnPortraitIconsDrawer.sleepingIcon,
							tooltip = "RitualBeginSleepingDesc".Translate(pawn)
						});
					}
				}
			}
		}

		// Token: 0x06012F40 RID: 77632 RVA: 0x0059DDA4 File Offset: 0x0059BFA4
		public static void DrawPawnPortraitIcons(Rect portraitRect, Pawn p, bool required, bool grayedOut, ref float curX, ref float curY, float iconSize, bool showIdeoIcon, out bool tooltipActive)
		{
			PawnPortraitIconsDrawer.CalculatePawnPortraitIcons(p, required, showIdeoIcon);
			tooltipActive = false;
			foreach (PawnPortraitIconsDrawer.PawnPortraitIcon pawnPortraitIcon in PawnPortraitIconsDrawer.tmpPortraitIcons)
			{
				PawnPortraitIconsDrawer.PawnPortraitIcon localIcon = pawnPortraitIcon;
				Rect rect = new Rect(curX - iconSize, curY - iconSize, iconSize, iconSize);
				curX -= iconSize;
				if (curX - iconSize < portraitRect.x)
				{
					curX = portraitRect.xMax;
					curY -= iconSize + 2f;
				}
				GUI.color = (grayedOut ? pawnPortraitIcon.color.SaturationChanged(0f) : pawnPortraitIcon.color);
				Widgets.DrawTextureFitted(rect, pawnPortraitIcon.icon, 1f, 1f);
				GUI.color = Color.white;
				if (pawnPortraitIcon.tooltip != null)
				{
					if (Mouse.IsOver(rect))
					{
						tooltipActive = true;
					}
					TooltipHandler.TipRegion(rect, () => localIcon.tooltip, pawnPortraitIcon.icon.GetInstanceID() ^ p.thingIDNumber);
				}
			}
		}

		// Token: 0x0400C465 RID: 50277
		private static Texture2D prisonerIcon = ContentFinder<Texture2D>.Get("UI/Icons/Prisoner", true);

		// Token: 0x0400C466 RID: 50278
		private static Texture2D sleepingIcon = ContentFinder<Texture2D>.Get("UI/Icons/ColonistBar/Sleeping", true);

		// Token: 0x0400C467 RID: 50279
		private static Texture2D slaveryIcon;

		// Token: 0x0400C468 RID: 50280
		private static List<PawnPortraitIconsDrawer.PawnPortraitIcon> tmpPortraitIcons = new List<PawnPortraitIconsDrawer.PawnPortraitIcon>();

		// Token: 0x020034A8 RID: 13480
		private struct PawnPortraitIcon
		{
			// Token: 0x0400C469 RID: 50281
			public Color color;

			// Token: 0x0400C46A RID: 50282
			public Texture2D icon;

			// Token: 0x0400C46B RID: 50283
			public string tooltip;
		}
	}
}
