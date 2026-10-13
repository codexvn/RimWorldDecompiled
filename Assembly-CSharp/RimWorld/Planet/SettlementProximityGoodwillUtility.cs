using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C20 RID: 15392
	public static class SettlementProximityGoodwillUtility
	{
		// Token: 0x170033A3 RID: 13219
		// (get) Token: 0x06014E07 RID: 85511 RVA: 0x00640BF8 File Offset: 0x0063EDF8
		public static int MaxDist
		{
			get
			{
				return Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Last<CurvePoint>().x);
			}
		}

		// Token: 0x06014E08 RID: 85512 RVA: 0x00640C1C File Offset: 0x0063EE1C
		public static void CheckSettlementProximityGoodwillChange()
		{
			if (Find.TickManager.TicksGame == 0 || Find.TickManager.TicksGame % 900000 != 0)
			{
				return;
			}
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Clear();
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction == Faction.OfPlayer)
				{
					SettlementProximityGoodwillUtility.AppendProximityGoodwillOffsets(settlement.Tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, true, false);
				}
			}
			if (!SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Any<Pair<Settlement, int>>())
			{
				return;
			}
			SettlementProximityGoodwillUtility.SortProximityGoodwillOffsets(SettlementProximityGoodwillUtility.tmpGoodwillOffsets);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			bool flag = false;
			TaggedString text = "LetterFactionBaseProximity".Translate() + "\n\n" + SettlementProximityGoodwillUtility.ProximityGoodwillOffsetsToString(SettlementProximityGoodwillUtility.tmpGoodwillOffsets).ToLineList(" - ", false);
			for (int j = 0; j < allFactionsListForReading.Count; j++)
			{
				Faction faction = allFactionsListForReading[j];
				if (faction != Faction.OfPlayer)
				{
					int num = 0;
					for (int k = 0; k < SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Count; k++)
					{
						if (SettlementProximityGoodwillUtility.tmpGoodwillOffsets[k].First.Faction == faction)
						{
							num += SettlementProximityGoodwillUtility.tmpGoodwillOffsets[k].Second;
						}
					}
					if (num != 0)
					{
						FactionRelationKind playerRelationKind = faction.PlayerRelationKind;
						Faction.OfPlayer.TryAffectGoodwillWith(faction, num, false, false, HistoryEventDefOf.SettlementProximity, null);
						flag = true;
						faction.TryAppendRelationKindChangedInfo(ref text, playerRelationKind, faction.PlayerRelationKind, null);
					}
				}
			}
			if (flag)
			{
				Find.LetterStack.ReceiveLetter("LetterLabelFactionBaseProximity".Translate(), text, LetterDefOf.NegativeEvent, null, 0, true);
			}
		}

		// Token: 0x06014E09 RID: 85513 RVA: 0x00640DD8 File Offset: 0x0063EFD8
		public static void AppendProximityGoodwillOffsets(PlanetTile tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
		{
			int maxDist = SettlementProximityGoodwillUtility.MaxDist;
			List<Settlement> settlements = Find.WorldObjects.Settlements;
			for (int i = 0; i < settlements.Count; i++)
			{
				Settlement settlement = settlements[i];
				if (settlement.Faction != null && settlement.Faction != Faction.OfPlayer && (!ignorePermanentlyHostile || !settlement.Faction.def.permanentEnemy) && (!ignoreIfAlreadyMinGoodwill || settlement.Faction.PlayerGoodwill != -100) && !(tile == settlement.Tile))
				{
					int num = Find.WorldGrid.TraversalDistanceBetween(tile, settlement.Tile, false, maxDist, false);
					if (num != 2147483647)
					{
						float x = (float)num * settlement.Tile.LayerDef.rangeDistanceFactor;
						int num2 = Mathf.RoundToInt(DiplomacyTuning.Goodwill_PerQuadrumFromSettlementProximity.Evaluate(x));
						if (num2 != 0)
						{
							outOffsets.Add(new Pair<Settlement, int>(settlement, num2));
						}
					}
				}
			}
		}

		// Token: 0x06014E0A RID: 85514 RVA: 0x00640EC4 File Offset: 0x0063F0C4
		private static void AppendGravshipAttackOffsets(PlanetTile tile, List<Pair<Settlement, int>> outOffsets, bool ignoreIfAlreadyMinGoodwill, bool ignorePermanentlyHostile)
		{
			Settlement settlement = Find.WorldObjects.SettlementAt(tile);
			if (settlement == null || settlement.Faction.IsPlayer)
			{
				return;
			}
			if (ignorePermanentlyHostile && settlement.Faction.def.permanentEnemy)
			{
				return;
			}
			if (ignoreIfAlreadyMinGoodwill && settlement.Faction.PlayerGoodwill == -100)
			{
				return;
			}
			outOffsets.Add(new Pair<Settlement, int>(settlement, Faction.OfPlayer.GoodwillToMakeHostile(settlement.Faction)));
		}

		// Token: 0x06014E0B RID: 85515 RVA: 0x00640F34 File Offset: 0x0063F134
		public static void SortProximityGoodwillOffsets(List<Pair<Settlement, int>> offsets)
		{
			offsets.SortBy((Pair<Settlement, int> x) => x.First.Faction.loadID, (Pair<Settlement, int> x) => -Mathf.Abs(x.Second));
		}

		// Token: 0x06014E0C RID: 85516 RVA: 0x00640F85 File Offset: 0x0063F185
		public static IEnumerable<string> ProximityGoodwillOffsetsToString(List<Pair<Settlement, int>> offsets)
		{
			int num;
			for (int i = 0; i < offsets.Count; i = num + 1)
			{
				yield return offsets[i].First.LabelCap + ": " + "ProximitySingleGoodwillChange".Translate(offsets[i].Second.ToStringWithSign(), offsets[i].First.Faction.Name);
				num = i;
			}
			yield break;
		}

		// Token: 0x06014E0D RID: 85517 RVA: 0x00640F95 File Offset: 0x0063F195
		private static IEnumerable<TaggedString> GetConfirmationDescriptions(PlanetTile tile, Building_GravEngine gravEngine = null)
		{
			Settlement settlement = Find.WorldObjects.SettlementAt(tile);
			if (ModsConfig.OdysseyActive && gravEngine != null && settlement != null && !settlement.Faction.IsPlayer)
			{
				if (settlement.Faction.HostileTo(Faction.OfPlayer))
				{
					yield return "ConfirmLandOnHostileFactionBase".Translate(settlement.Faction);
				}
				else
				{
					yield return "ConfirmLandOnNeutralFactionBase".Translate(settlement.Faction);
				}
			}
			SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Clear();
			SettlementProximityGoodwillUtility.AppendProximityGoodwillOffsets(tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, false, true);
			if (SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Any<Pair<Settlement, int>>())
			{
				yield return "ConfirmSettleNearFactionBase".Translate(SettlementProximityGoodwillUtility.MaxDist - 1, 15);
			}
			float value;
			if (ModsConfig.BiotechActive && NoxiousHazeUtility.TryGetNoxiousHazeMTB(tile, out value))
			{
				yield return "ConfirmSettleNearPollution".Translate(value);
			}
			foreach (BiomeDef biomeDef in tile.Tile.Biomes)
			{
				if (!biomeDef.settleWarning.NullOrEmpty())
				{
					yield return biomeDef.settleWarning;
				}
			}
			IEnumerator<BiomeDef> enumerator = null;
			if (ModsConfig.OdysseyActive && tile.LayerDef == PlanetLayerDefOf.Orbit)
			{
				yield return "OrbitalWarning".Translate();
				if (gravEngine != null)
				{
					yield return "OrbitalWarning_Gravship".Translate();
					foreach (TaggedString taggedString in gravEngine.GetOrbitalWarnings())
					{
						yield return taggedString;
					}
					IEnumerator<TaggedString> enumerator2 = null;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06014E0E RID: 85518 RVA: 0x00640FAC File Offset: 0x0063F1AC
		private static IEnumerable<string> GetConfirmationEffects(PlanetTile tile, bool gravship)
		{
			SettlementProximityGoodwillUtility.tmpGoodwillOffsets.Clear();
			if (gravship && ModsConfig.OdysseyActive)
			{
				SettlementProximityGoodwillUtility.AppendGravshipAttackOffsets(tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, false, true);
			}
			SettlementProximityGoodwillUtility.AppendProximityGoodwillOffsets(tile, SettlementProximityGoodwillUtility.tmpGoodwillOffsets, false, true);
			SettlementProximityGoodwillUtility.SortProximityGoodwillOffsets(SettlementProximityGoodwillUtility.tmpGoodwillOffsets);
			foreach (string text in SettlementProximityGoodwillUtility.ProximityGoodwillOffsetsToString(SettlementProximityGoodwillUtility.tmpGoodwillOffsets))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06014E0F RID: 85519 RVA: 0x00640FC4 File Offset: 0x0063F1C4
		public static void CheckConfirmSettle(PlanetTile tile, Action settleAction, Action cancelAction = null, Building_GravEngine gravEngine = null)
		{
			IEnumerable<TaggedString> confirmationDescriptions = SettlementProximityGoodwillUtility.GetConfirmationDescriptions(tile, gravEngine);
			if (confirmationDescriptions.Any<TaggedString>())
			{
				TaggedString taggedString = new TaggedString("");
				foreach (TaggedString t in confirmationDescriptions)
				{
					if (!taggedString.NullOrEmpty())
					{
						taggedString += "\n\n";
					}
					taggedString += t;
				}
				if (gravEngine != null)
				{
					taggedString += "\n\n" + "ConfirmLand".Translate();
				}
				else
				{
					taggedString += "\n\n" + "ConfirmSettle".Translate();
				}
				IEnumerable<string> confirmationEffects = SettlementProximityGoodwillUtility.GetConfirmationEffects(tile, gravEngine != null);
				if (confirmationEffects.Any<string>())
				{
					taggedString += "\n\n" + confirmationEffects.ToLineList(" - ", false);
				}
				Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(taggedString, settleAction, cancelAction, false, null, WindowLayer.Dialog));
				return;
			}
			settleAction();
		}

		// Token: 0x0400E8A8 RID: 59560
		private static readonly List<Pair<Settlement, int>> tmpGoodwillOffsets = new List<Pair<Settlement, int>>();
	}
}
