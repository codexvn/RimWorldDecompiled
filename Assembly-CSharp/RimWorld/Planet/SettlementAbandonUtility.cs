using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x02003DCB RID: 15819
	[StaticConstructorOnStartup]
	public static class SettlementAbandonUtility
	{
		// Token: 0x06015940 RID: 88384 RVA: 0x00675CFC File Offset: 0x00673EFC
		public static Command AbandonCommand(MapParent settlement)
		{
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "CommandAbandonHome".Translate();
			command_Action.defaultDesc = "CommandAbandonHomeDesc".Translate();
			command_Action.icon = SettlementAbandonUtility.AbandonCommandTex;
			command_Action.action = delegate()
			{
				SettlementAbandonUtility.TryAbandonViaInterface(settlement);
			};
			command_Action.Order = 3000f;
			if (SettlementAbandonUtility.AllColonistsThere(settlement))
			{
				command_Action.Disable("CommandAbandonHomeFailAllColonistsThere".Translate());
			}
			return command_Action;
		}

		// Token: 0x06015941 RID: 88385 RVA: 0x00675D94 File Offset: 0x00673F94
		public static bool AllColonistsThere(MapParent settlement)
		{
			return !CaravanUtility.PlayerHasAnyCaravan() && !Find.Maps.Any((Map x) => x.info.parent != settlement && x.mapPawns.FreeColonistsSpawned.Any<Pawn>());
		}

		// Token: 0x06015942 RID: 88386 RVA: 0x00675DD0 File Offset: 0x00673FD0
		public static void TryAbandonViaInterface(MapParent settlement)
		{
			ArchotechSettlement archotechSettlement = settlement as ArchotechSettlement;
			if (archotechSettlement != null && archotechSettlement.AnyArchotechBuildingRequiresStudy)
			{
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation("AbandonArchotechStructuresWarningDialog".Translate(), delegate()
				{
					SettlementAbandonUtility.TryAbandonWithColonyCheck(settlement);
				}, false, null, WindowLayer.Dialog));
				return;
			}
			SettlementAbandonUtility.TryAbandonWithColonyCheck(settlement);
		}

		// Token: 0x06015943 RID: 88387 RVA: 0x00675E38 File Offset: 0x00674038
		private static void TryAbandonWithColonyCheck(MapParent settlement)
		{
			Map map = settlement.Map;
			if (map == null)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			List<Pawn> source = map.mapPawns.PawnsInFaction(Faction.OfPlayer);
			if (source.Count<Pawn>() != 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (Pawn pawn in from x in source
				orderby x.IsColonist descending
				select x)
				{
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.AppendLine();
					}
					stringBuilder2.Append("    " + pawn.LabelCap);
				}
				stringBuilder.Append(ModsConfig.BiotechActive ? "ConfirmAbandonHomeWithColonyPawnsIncMechs".Translate(stringBuilder2) : "ConfirmAbandonHomeWithColonyPawns".Translate(stringBuilder2));
			}
			PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(map.mapPawns.AllPawns, PawnDiedOrDownedThoughtsKind.Banished, stringBuilder, null, "\n\n" + "ConfirmAbandonHomeNegativeThoughts_Everyone".Translate(), "ConfirmAbandonHomeNegativeThoughts");
			if (stringBuilder.Length == 0)
			{
				SettlementAbandonUtility.Abandon(settlement);
				SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				return;
			}
			Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(stringBuilder.ToString(), delegate()
			{
				SettlementAbandonUtility.Abandon(settlement);
			}, false, null, WindowLayer.Dialog));
		}

		// Token: 0x06015944 RID: 88388 RVA: 0x00675FDC File Offset: 0x006741DC
		private static void Abandon(MapParent settlement)
		{
			settlement.Abandon(false);
			Find.GameEnder.CheckOrUpdateGameOver();
		}

		// Token: 0x0400EFDB RID: 61403
		private static readonly Texture2D AbandonCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/AbandonHome", true);
	}
}
