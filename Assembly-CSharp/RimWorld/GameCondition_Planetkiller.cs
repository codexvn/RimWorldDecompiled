using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001D4B RID: 7499
	public class GameCondition_Planetkiller : GameCondition
	{
		// Token: 0x170019F5 RID: 6645
		// (get) Token: 0x0600A804 RID: 43012 RVA: 0x00301A7C File Offset: 0x002FFC7C
		public override string TooltipString
		{
			get
			{
				Vector2 location;
				if (Find.CurrentMap != null)
				{
					location = Find.WorldGrid.LongLatOf(Find.CurrentMap.Tile);
				}
				else
				{
					location = default(Vector2);
				}
				string text = this.def.LabelCap;
				text += "\n";
				text = text + "\n" + this.Description;
				text = string.Concat(new string[]
				{
					text,
					"\n",
					"ImpactDate".Translate().CapitalizeFirst(),
					": ",
					GenDate.DateFullStringAt((long)GenDate.TickGameToAbs(this.startTick + base.Duration), location).Colorize(ColoredText.DateTimeColor)
				});
				return string.Concat(new string[]
				{
					text,
					"\n",
					"TimeLeft".Translate().CapitalizeFirst(),
					": ",
					base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false).Colorize(ColoredText.DateTimeColor)
				});
			}
		}

		// Token: 0x0600A805 RID: 43013 RVA: 0x00301B98 File Offset: 0x002FFD98
		public override void GameConditionTick()
		{
			base.GameConditionTick();
			if (base.TicksLeft <= 179)
			{
				Find.ActiveLesson.Deactivate();
				if (base.TicksLeft == 179)
				{
					SoundDefOf.PlanetkillerImpact.PlayOneShotOnCamera(null);
				}
				if (base.TicksLeft == 90)
				{
					ScreenFader.StartFade(GameCondition_Planetkiller.FadeColor, 1f);
				}
			}
		}

		// Token: 0x0600A806 RID: 43014 RVA: 0x00301BF3 File Offset: 0x002FFDF3
		public override void End()
		{
			base.End();
			this.Impact();
		}

		// Token: 0x0600A807 RID: 43015 RVA: 0x00301C01 File Offset: 0x002FFE01
		private void Impact()
		{
			ScreenFader.SetColor(Color.clear);
			GenGameEnd.EndGameDialogMessage("GameOverPlanetkillerImpact".Translate(Find.World.info.name), false, GameCondition_Planetkiller.FadeColor);
		}

		// Token: 0x04007448 RID: 29768
		private const int SoundDuration = 179;

		// Token: 0x04007449 RID: 29769
		private const int FadeDuration = 90;

		// Token: 0x0400744A RID: 29770
		private static readonly Color FadeColor = Color.white;
	}
}
