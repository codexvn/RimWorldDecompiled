using System;
using RimWorld;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x02000338 RID: 824
	public class GameComponent_OnetimeNotification : GameComponent
	{
		// Token: 0x060014DB RID: 5339 RVA: 0x000623DC File Offset: 0x000605DC
		public GameComponent_OnetimeNotification(Game game)
		{
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x000623EC File Offset: 0x000605EC
		public override void GameComponentTick()
		{
			if (Find.TickManager.TicksGame % 2000 != 0 || !Rand.Chance(0.05f))
			{
				return;
			}
			if (this.sendAICoreRequestReminder)
			{
				if (ResearchProjectTagDefOf.ShipRelated.CompletedProjects() < 2)
				{
					return;
				}
				if (PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.AIPersonaCore, 1) || PlayerItemAccessibilityUtility.PlayerOrQuestRewardHas(ThingDefOf.Ship_ComputerCore, 1))
				{
					return;
				}
				Faction faction = Find.FactionManager.RandomNonHostileFaction(false, false, true, TechLevel.Undefined);
				if (faction == null || faction.leader == null)
				{
					return;
				}
				Find.LetterStack.ReceiveLetter("LetterLabelAICoreOffer".Translate(), "LetterAICoreOffer".Translate(faction.leader.LabelDefinite(), faction.NameColored, faction.leader.Named("PAWN")).Resolve().CapitalizeFirst(), LetterDefOf.NeutralEvent, GlobalTargetInfo.Invalid, faction, null, null, null, 0, true);
				this.sendAICoreRequestReminder = false;
			}
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x000624DE File Offset: 0x000606DE
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sendAICoreRequestReminder, "sendAICoreRequestReminder", false, false);
		}

		// Token: 0x04001377 RID: 4983
		public bool sendAICoreRequestReminder = true;
	}
}
