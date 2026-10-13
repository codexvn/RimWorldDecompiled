using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LudeonTK;
using Verse;

namespace RimWorld
{
	// Token: 0x020025A6 RID: 9638
	public class VisitorGiftForPlayerUtility
	{
		// Token: 0x0600D32E RID: 54062 RVA: 0x003E3113 File Offset: 0x003E1313
		public static float ChanceToLeaveGift(Faction faction, Map map)
		{
			if (faction == null || faction.IsPlayer)
			{
				return 0f;
			}
			return 0.25f * VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(map) * VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction);
		}

		// Token: 0x0600D32F RID: 54063 RVA: 0x003E313C File Offset: 0x003E133C
		public static List<Thing> GenerateGifts(Faction faction, Map map)
		{
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.totalMarketValueRange = new FloatRange?(DiplomacyTuning.VisitorGiftTotalMarketValueRangeBase * DiplomacyTuning.VisitorGiftTotalMarketValueFactorFromPlayerWealthCurve.Evaluate(map.wealthWatcher.WealthTotal));
			return ThingSetMakerDefOf.VisitorGift.root.Generate(parms);
		}

		// Token: 0x0600D330 RID: 54064 RVA: 0x003E318C File Offset: 0x003E138C
		private static float PlayerWealthChanceFactor(Map map)
		{
			return DiplomacyTuning.VisitorGiftChanceFactorFromPlayerWealthCurve.Evaluate(map.wealthWatcher.WealthTotal);
		}

		// Token: 0x0600D331 RID: 54065 RVA: 0x003E31A3 File Offset: 0x003E13A3
		private static float FactionRelationsChanceFactor(Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				return 0f;
			}
			return DiplomacyTuning.VisitorGiftChanceFactorFromGoodwillCurve.Evaluate((float)faction.PlayerGoodwill);
		}

		// Token: 0x0600D332 RID: 54066 RVA: 0x003E31CC File Offset: 0x003E13CC
		private static Pawn GetGiftGiver(List<Pawn> possibleGivers, Faction faction)
		{
			if (possibleGivers.NullOrEmpty<Pawn>())
			{
				return null;
			}
			Pawn pawn = null;
			for (int i = 0; i < possibleGivers.Count; i++)
			{
				if (possibleGivers[i].RaceProps.Humanlike && possibleGivers[i].Faction == faction)
				{
					pawn = possibleGivers[i];
					break;
				}
			}
			if (pawn == null)
			{
				for (int j = 0; j < possibleGivers.Count; j++)
				{
					if (possibleGivers[j].Faction == faction)
					{
						pawn = possibleGivers[j];
						break;
					}
				}
			}
			if (pawn == null)
			{
				pawn = possibleGivers[0];
			}
			return pawn;
		}

		// Token: 0x0600D333 RID: 54067 RVA: 0x003E325C File Offset: 0x003E145C
		public static void GiveGift(List<Pawn> possibleGivers, Faction faction, List<Thing> gifts)
		{
			Pawn giftGiver = VisitorGiftForPlayerUtility.GetGiftGiver(possibleGivers, faction);
			if (giftGiver == null)
			{
				return;
			}
			TargetInfo target = TargetInfo.Invalid;
			for (int i = 0; i < gifts.Count; i++)
			{
				if (GenPlace.TryPlaceThing(gifts[i], giftGiver.Position, giftGiver.Map, ThingPlaceMode.Near, null, null, null, 1))
				{
					target = gifts[i];
				}
				else
				{
					gifts[i].Destroy(DestroyMode.Vanish);
				}
			}
			if (target.IsValid)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelVisitorsGaveGift".Translate(giftGiver.Faction.Name), "LetterVisitorsGaveGift".Translate(giftGiver.Faction.def.pawnsPlural, (from g in gifts
				select g.LabelCap).ToLineList("   -", false), giftGiver.Named("PAWN")).AdjustedFor(giftGiver, "PAWN", true), LetterDefOf.PositiveEvent, target, faction, null, null, null, 0, true);
			}
		}

		// Token: 0x0600D334 RID: 54068 RVA: 0x003E3380 File Offset: 0x003E1580
		public static void GiveRandomGift(List<Pawn> possibleGivers, Faction faction)
		{
			Pawn giftGiver = VisitorGiftForPlayerUtility.GetGiftGiver(possibleGivers, faction);
			if (giftGiver == null)
			{
				return;
			}
			VisitorGiftForPlayerUtility.GiveGift(possibleGivers, faction, VisitorGiftForPlayerUtility.GenerateGifts(faction, giftGiver.Map));
		}

		// Token: 0x0600D335 RID: 54069 RVA: 0x003E33AC File Offset: 0x003E15AC
		[DebugOutput]
		private static void VisitorGiftChance()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Current wealth factor (wealth=" + Find.CurrentMap.wealthWatcher.WealthTotal.ToString("F0") + "): ");
			stringBuilder.AppendLine(VisitorGiftForPlayerUtility.PlayerWealthChanceFactor(Find.CurrentMap).ToStringPercent());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Chance per faction:");
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && !faction.Hidden)
				{
					stringBuilder.Append(string.Concat(new string[]
					{
						faction.Name,
						" (",
						faction.PlayerGoodwill.ToStringWithSign(),
						", ",
						faction.PlayerRelationKind.GetLabelCap(),
						")"
					}));
					stringBuilder.Append(": " + VisitorGiftForPlayerUtility.ChanceToLeaveGift(faction, Find.CurrentMap).ToStringPercent());
					stringBuilder.AppendLine(" (rels factor: " + VisitorGiftForPlayerUtility.FactionRelationsChanceFactor(faction).ToStringPercent() + ")");
				}
			}
			int num = 0;
			for (int i = 0; i < 6; i++)
			{
				Dictionary<IIncidentTarget, int> dictionary;
				int[] array;
				List<Pair<IncidentDef, IncidentParms>> list;
				int num2;
				StorytellerUtility.DebugGetFutureIncidents(60, true, out dictionary, out array, out list, out num2, null, null, null, null);
				for (int j = 0; j < list.Count; j++)
				{
					if ((list[j].First == IncidentDefOf.VisitorGroup || list[j].First == IncidentDefOf.TraderCaravanArrival) && Rand.Chance(VisitorGiftForPlayerUtility.ChanceToLeaveGift(list[j].Second.faction ?? Find.FactionManager.RandomNonHostileFaction(false, false, false, TechLevel.Undefined), Find.CurrentMap)))
					{
						num++;
					}
				}
			}
			float num3 = (float)num / 6f;
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Calculated number of gifts received on average within the next 1 year");
			stringBuilder.AppendLine("(assuming current wealth and faction relations)");
			stringBuilder.Append("  = " + num3.ToString("0.##"));
			Log.Message(stringBuilder.ToString());
		}
	}
}
