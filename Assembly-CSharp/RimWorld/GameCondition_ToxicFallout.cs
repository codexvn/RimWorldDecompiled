using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4E RID: 7502
	public class GameCondition_ToxicFallout : GameCondition
	{
		// Token: 0x170019FA RID: 6650
		// (get) Token: 0x0600A813 RID: 43027 RVA: 0x00301765 File Offset: 0x002FF965
		public override int TransitionTicks
		{
			get
			{
				return 5000;
			}
		}

		// Token: 0x0600A814 RID: 43028 RVA: 0x00301ECB File Offset: 0x003000CB
		public override void Init()
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.ForbiddingDoors, OpportunityType.Critical);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.AllowedAreas, OpportunityType.Critical);
		}

		// Token: 0x0600A815 RID: 43029 RVA: 0x00301EE4 File Offset: 0x003000E4
		public override void GameConditionTick()
		{
			List<Map> affectedMaps = base.AffectedMaps;
			if (Find.TickManager.TicksGame % 3451 == 0)
			{
				for (int i = 0; i < affectedMaps.Count; i++)
				{
					this.DoPawnsToxicDamage(affectedMaps[i]);
				}
			}
			for (int j = 0; j < this.overlays.Count; j++)
			{
				for (int k = 0; k < affectedMaps.Count; k++)
				{
					this.overlays[j].TickOverlay(affectedMaps[k], 1f);
				}
			}
		}

		// Token: 0x0600A816 RID: 43030 RVA: 0x00301F6C File Offset: 0x0030016C
		private void DoPawnsToxicDamage(Map map)
		{
			IReadOnlyList<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				if (!allPawnsSpawned[i].kindDef.immuneToGameConditionEffects)
				{
					ToxicUtility.DoAirbornePawnToxicDamage(allPawnsSpawned[i], 1f);
				}
			}
		}

		// Token: 0x0600A817 RID: 43031 RVA: 0x00301FBC File Offset: 0x003001BC
		public override void DoCellSteadyEffects(IntVec3 c, Map map)
		{
			if (!c.Roofed(map))
			{
				List<Thing> thingList = c.GetThingList(map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing is Plant)
					{
						if (thing.def.plant.dieFromToxicFallout && Rand.Value < 0.0065f)
						{
							Plant plant = thing as Plant;
							if (plant != null && plant.IsCrop && MessagesRepeatAvoider.MessageShowAllowed("MessagePlantDiedOfPoison-" + thing.def.defName, 240f))
							{
								Messages.Message("MessagePlantDiedOfPoison".Translate(plant.GetCustomLabelNoCount(false)), new TargetInfo(plant.Position, map, false), MessageTypeDefOf.NegativeEvent, true);
							}
							thing.Kill(null, null);
						}
					}
					else if (thing.def.category == ThingCategory.Item)
					{
						CompRottable compRottable = thing.TryGetComp<CompRottable>();
						if (compRottable != null && compRottable.Stage < RotStage.Dessicated)
						{
							compRottable.RotProgress += 3000f;
						}
					}
				}
			}
		}

		// Token: 0x0600A818 RID: 43032 RVA: 0x003020E0 File Offset: 0x003002E0
		public override void GameConditionDraw(Map map)
		{
			for (int i = 0; i < this.overlays.Count; i++)
			{
				this.overlays[i].DrawOverlay(map);
			}
		}

		// Token: 0x0600A819 RID: 43033 RVA: 0x003018B6 File Offset: 0x002FFAB6
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 0.5f);
		}

		// Token: 0x0600A81A RID: 43034 RVA: 0x00302115 File Offset: 0x00300315
		public override SkyTarget? SkyTarget(Map map)
		{
			return new SkyTarget?(new SkyTarget(0.85f, this.ToxicFalloutColors, 1f, 1f));
		}

		// Token: 0x0600A81B RID: 43035 RVA: 0x00006AAC File Offset: 0x00004CAC
		public override float AnimalDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x0600A81C RID: 43036 RVA: 0x00006AAC File Offset: 0x00004CAC
		public override float PlantDensityFactor(Map map)
		{
			return 0f;
		}

		// Token: 0x0600A81D RID: 43037 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowEnjoyableOutsideNow(Map map)
		{
			return false;
		}

		// Token: 0x0600A81E RID: 43038 RVA: 0x00302136 File Offset: 0x00300336
		public override List<SkyOverlay> SkyOverlays(Map map)
		{
			return this.overlays;
		}

		// Token: 0x0400744F RID: 29775
		private const float MaxSkyLerpFactor = 0.5f;

		// Token: 0x04007450 RID: 29776
		private const float SkyGlow = 0.85f;

		// Token: 0x04007451 RID: 29777
		private SkyColorSet ToxicFalloutColors = new SkyColorSet(new ColorInt(216, 255, 0).ToColor, new ColorInt(234, 200, 255).ToColor, new Color(0.6f, 0.8f, 0.5f), 0.85f);

		// Token: 0x04007452 RID: 29778
		private readonly List<SkyOverlay> overlays = new List<SkyOverlay>
		{
			new WeatherOverlay_Fallout()
		};

		// Token: 0x04007453 RID: 29779
		private const float PlantKillChance = 0.0065f;

		// Token: 0x04007454 RID: 29780
		private const float CorpseRotProgressAdd = 3000f;
	}
}
