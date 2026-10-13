using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FF5 RID: 12277
	public class Alert_NeedWarmClothes : Alert
	{
		// Token: 0x0601149B RID: 70811 RVA: 0x004F41F4 File Offset: 0x004F23F4
		public Alert_NeedWarmClothes()
		{
			this.defaultLabel = "NeedWarmClothes".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x0601149C RID: 70812 RVA: 0x004F425C File Offset: 0x004F245C
		private bool AnyColonistsNeedWarmClothes(Map map)
		{
			this.colonistsWithoutWarmClothes.Clear();
			this.needWarmClothesCount = 0;
			this.colonistsWithWarmClothesCount = 0;
			this.missingWarmClothesCount = 0;
			if (this.lowestTemperatureComing < ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null))
			{
				foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
				{
					if (!pawn.DevelopmentalStage.Baby() && !pawn.IsGhoul && !pawn.IsShambler && !pawn.apparel.AnyApparelLocked)
					{
						Pawn_StoryTracker story = pawn.story;
						if ((((story != null) ? story.traits : null) == null || !pawn.story.traits.HasTrait(TraitDefOf.Nudist)) && (pawn.Ideo == null || !pawn.Ideo.IdeoPrefersNudity()))
						{
							this.needWarmClothesCount++;
							if (pawn.GetStatValue(StatDefOf.ComfyTemperatureMin, true, -1) <= this.lowestTemperatureComing)
							{
								this.colonistsWithWarmClothesCount++;
							}
							else
							{
								this.colonistsWithoutWarmClothes.Add(pawn);
							}
						}
					}
				}
				this.missingWarmClothesCount = Mathf.Max(this.needWarmClothesCount - this.colonistsWithWarmClothesCount - this.FreeWarmClothesSetsCount(map), 0);
				return this.missingWarmClothesCount > 0;
			}
			return false;
		}

		// Token: 0x0601149D RID: 70813 RVA: 0x004F43CC File Offset: 0x004F25CC
		private int FreeWarmClothesSetsCount(Map map)
		{
			this.jackets.Clear();
			this.shirts.Clear();
			this.pants.Clear();
			this.tmpApparelList.Clear();
			this.tmpApparelList.AddRange(map.listerThings.ThingsInGroup(ThingRequestGroup.Apparel));
			foreach (IHaulSource haulSource in map.haulDestinationManager.AllHaulSourcesListForReading)
			{
				foreach (Thing thing in ((IEnumerable<Thing>)haulSource.GetDirectlyHeldThings()))
				{
					Apparel apparel = thing as Apparel;
					if (apparel != null)
					{
						this.tmpApparelList.Add(apparel);
					}
				}
			}
			for (int i = 0; i < this.tmpApparelList.Count; i++)
			{
				if (this.tmpApparelList[i].IsInAnyStorage())
				{
					float statValue = this.tmpApparelList[i].GetStatValue(StatDefOf.Insulation_Cold, true, 120);
					if (statValue > 0f)
					{
						if (this.tmpApparelList[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Torso))
						{
							if (this.tmpApparelList[i].def.apparel.layers.Contains(ApparelLayerDefOf.OnSkin))
							{
								this.shirts.Enqueue(this.tmpApparelList[i], -statValue);
							}
							else
							{
								this.jackets.Enqueue(this.tmpApparelList[i], -statValue);
							}
						}
						if (this.tmpApparelList[i].def.apparel.bodyPartGroups.Contains(BodyPartGroupDefOf.Legs))
						{
							this.pants.Enqueue(this.tmpApparelList[i], -statValue);
						}
					}
				}
			}
			this.tmpApparelList.Clear();
			float num = ThingDefOf.Human.GetStatValueAbstract(StatDefOf.ComfyTemperatureMin, null) - this.lowestTemperatureComing;
			if (num <= 0f)
			{
				return GenMath.Max(this.jackets.Count, this.shirts.Count, this.pants.Count);
			}
			int num2 = 0;
			while (this.jackets.Count > 0 || this.shirts.Count > 0 || this.pants.Count > 0)
			{
				float num3 = 0f;
				Thing thing2;
				float num4;
				if (this.jackets.TryDequeue(out thing2, out num4))
				{
					num3 += num4;
				}
				float num5;
				if (this.shirts.Count > 0 && num3 < num && this.shirts.TryDequeue(out thing2, out num5))
				{
					num3 += num5;
				}
				float num6;
				if (this.pants.Count > 0 && num3 < num && this.pants.TryDequeue(out thing2, out num6))
				{
					num3 += num6;
				}
				if (num3 < num)
				{
					break;
				}
				num2++;
			}
			this.jackets.Clear();
			this.shirts.Clear();
			this.pants.Clear();
			return num2;
		}

		// Token: 0x0601149E RID: 70814 RVA: 0x004F4700 File Offset: 0x004F2900
		public override TaggedString GetExplanation()
		{
			if (this.missingWarmClothesCount == this.needWarmClothesCount)
			{
				return "NeedWarmClothesDesc1All".Translate() + "\n\n" + "NeedWarmClothesDesc2".Translate(this.lowestTemperatureComing.ToStringTemperature("F0"));
			}
			return "NeedWarmClothesDesc1".Translate(this.missingWarmClothesCount) + "\n\n" + "NeedWarmClothesDesc2".Translate(this.lowestTemperatureComing.ToStringTemperature("F0"));
		}

		// Token: 0x0601149F RID: 70815 RVA: 0x004F4797 File Offset: 0x004F2997
		public override AlertReport GetReport()
		{
			if (this.MapWithMissingWarmClothes() == null)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.colonistsWithoutWarmClothes);
		}

		// Token: 0x060114A0 RID: 70816 RVA: 0x004F47B4 File Offset: 0x004F29B4
		private Map MapWithMissingWarmClothes()
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				Map map = maps[i];
				if (map.IsPlayerHome && !map.Biome.inVacuum)
				{
					this.lowestTemperatureComing = this.LowestTemperatureComing(map);
					if (this.lowestTemperatureComing < 5f && this.AnyColonistsNeedWarmClothes(map))
					{
						return map;
					}
				}
			}
			return null;
		}

		// Token: 0x060114A1 RID: 70817 RVA: 0x004F481C File Offset: 0x004F2A1C
		private float LowestTemperatureComing(Map map)
		{
			Twelfth twelfth = GenLocalDate.Twelfth(map);
			float a = this.GetTemperature(twelfth, map);
			for (int i = 0; i < 3; i++)
			{
				twelfth = twelfth.NextTwelfth();
				a = Mathf.Min(a, this.GetTemperature(twelfth, map));
			}
			return Mathf.Min(a, map.mapTemperature.OutdoorTemp);
		}

		// Token: 0x060114A2 RID: 70818 RVA: 0x004F486C File Offset: 0x004F2A6C
		private float GetTemperature(Twelfth twelfth, Map map)
		{
			return GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth);
		}

		// Token: 0x0400B33B RID: 45883
		private PriorityQueue<Thing, float> jackets = new PriorityQueue<Thing, float>();

		// Token: 0x0400B33C RID: 45884
		private PriorityQueue<Thing, float> shirts = new PriorityQueue<Thing, float>();

		// Token: 0x0400B33D RID: 45885
		private PriorityQueue<Thing, float> pants = new PriorityQueue<Thing, float>();

		// Token: 0x0400B33E RID: 45886
		private List<Pawn> colonistsWithoutWarmClothes = new List<Pawn>();

		// Token: 0x0400B33F RID: 45887
		private int needWarmClothesCount;

		// Token: 0x0400B340 RID: 45888
		private int colonistsWithWarmClothesCount;

		// Token: 0x0400B341 RID: 45889
		private int missingWarmClothesCount;

		// Token: 0x0400B342 RID: 45890
		private float lowestTemperatureComing;

		// Token: 0x0400B343 RID: 45891
		private List<Thing> tmpApparelList = new List<Thing>();

		// Token: 0x0400B344 RID: 45892
		private const int CheckNextTwelfthsCount = 3;

		// Token: 0x0400B345 RID: 45893
		private const float CanShowAlertOnlyIfTempBelow = 5f;
	}
}
