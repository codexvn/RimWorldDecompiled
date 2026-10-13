using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A86 RID: 14982
	public class QuestNode_GetSiteTile : QuestNode
	{
		// Token: 0x06014771 RID: 83825 RVA: 0x0061C96C File Offset: 0x0061AB6C
		protected override bool TestRunInt(Slate slate)
		{
			PlanetTile var;
			if (!this.TryFindTile(slate, out var))
			{
				return false;
			}
			bool? value = this.clampRangeBySiteParts.GetValue(slate);
			bool flag = true;
			if ((value.GetValueOrDefault() == flag & value != null) && this.sitePartDefs.GetValue(slate) == null)
			{
				return false;
			}
			slate.Set<PlanetTile>(this.storeAs.GetValue(slate), var, false);
			return true;
		}

		// Token: 0x06014772 RID: 83826 RVA: 0x0061C9D0 File Offset: 0x0061ABD0
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			int num;
			PlanetTile var;
			if (!slate.TryGet<int>(this.storeAs.GetValue(slate), out num, false) && this.TryFindTile(QuestGen.slate, out var))
			{
				QuestGen.slate.Set<PlanetTile>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x06014773 RID: 83827 RVA: 0x0061CA24 File Offset: 0x0061AC24
		private bool TryFindTile(Slate slate, out PlanetTile tile)
		{
			bool value = this.canSelectSpace.GetValue(slate);
			Map map = slate.Get<Map>("map", null, false) ?? (value ? Find.RandomPlayerHomeMap : Find.RandomSurfacePlayerHomeMap);
			PlanetTile nearTile = (map != null) ? map.Tile : PlanetTile.Invalid;
			if (nearTile.Valid && nearTile.LayerDef.isSpace && !value)
			{
				nearTile = PlanetTile.Invalid;
			}
			int num = int.MaxValue;
			bool? value2 = this.clampRangeBySiteParts.GetValue(slate);
			if (value2 != null && value2.Value)
			{
				foreach (SitePartDef sitePartDef in this.sitePartDefs.GetValue(slate))
				{
					if (sitePartDef.conditionCauserDef != null)
					{
						num = Mathf.Min(num, sitePartDef.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange);
					}
				}
			}
			IntRange intRange;
			if (!slate.TryGet<IntRange>("siteDistRange", out intRange, false))
			{
				intRange = new IntRange(7, Mathf.Min(27, num));
			}
			else if (num != 2147483647)
			{
				intRange = new IntRange(Mathf.Min(intRange.min, num), Mathf.Min(intRange.max, num));
			}
			TileFinderMode tileFinderMode = this.preferCloserTiles.GetValue(slate) ? TileFinderMode.Near : TileFinderMode.Random;
			float num2 = (!ModsConfig.OdysseyActive) ? 0f : (this.selectLandmarkChance.GetValue(slate) ?? 0.5f);
			return TileFinder.TryFindNewSiteTile(out tile, nearTile, intRange.min, intRange.max, this.allowCaravans.GetValue(slate), this.allowedLandmarks.GetValue(slate), num2, this.canSelectComboLandmarks.GetValue(slate), tileFinderMode, false, value, null, null);
		}

		// Token: 0x0400E282 RID: 57986
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E283 RID: 57987
		public SlateRef<bool> preferCloserTiles;

		// Token: 0x0400E284 RID: 57988
		public SlateRef<bool> allowCaravans;

		// Token: 0x0400E285 RID: 57989
		public SlateRef<bool> canSelectSpace;

		// Token: 0x0400E286 RID: 57990
		public SlateRef<bool?> clampRangeBySiteParts;

		// Token: 0x0400E287 RID: 57991
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x0400E288 RID: 57992
		public SlateRef<List<LandmarkDef>> allowedLandmarks;

		// Token: 0x0400E289 RID: 57993
		public SlateRef<float?> selectLandmarkChance;

		// Token: 0x0400E28A RID: 57994
		public SlateRef<bool> canSelectComboLandmarks;
	}
}
