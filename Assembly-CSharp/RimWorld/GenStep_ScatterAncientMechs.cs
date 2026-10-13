using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF3 RID: 8179
	public class GenStep_ScatterAncientMechs : GenStep_Scatterer
	{
		// Token: 0x17001B74 RID: 7028
		// (get) Token: 0x0600B435 RID: 46133 RVA: 0x0033AFA1 File Offset: 0x003391A1
		private static IEnumerable<ThingDef> MechThingDefs
		{
			get
			{
				yield return ThingDefOf.AncientWarwalkerTorso;
				yield return ThingDefOf.AncientMiniWarwalkerRemains;
				yield return ThingDefOf.AncientWarspiderRemains;
				yield return ThingDefOf.AncientWarwalkerFoot;
				yield break;
			}
		}

		// Token: 0x17001B75 RID: 7029
		// (get) Token: 0x0600B436 RID: 46134 RVA: 0x0033AFAA File Offset: 0x003391AA
		public override int SeedPart
		{
			get
			{
				return 1034745625;
			}
		}

		// Token: 0x0600B437 RID: 46135 RVA: 0x0033AFB4 File Offset: 0x003391B4
		protected override float GetPlacementFactor(Map map)
		{
			float num = 1f;
			foreach (TileMutatorDef tileMutatorDef in map.TileInfo.Mutators)
			{
				num *= tileMutatorDef.junkDensityFactor;
			}
			return num;
		}

		// Token: 0x0600B438 RID: 46136 RVA: 0x0033B010 File Offset: 0x00339210
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter ancient outdoor building"))
			{
				return;
			}
			this.count = 1;
			this.allowInWaterBiome = false;
			this.thingToPlace = GenStep_ScatterAncientMechs.MechThingDefs.RandomElement<ThingDef>();
			base.Generate(map, parms);
		}

		// Token: 0x0600B439 RID: 46137 RVA: 0x0033B048 File Offset: 0x00339248
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			if (!base.CanScatterAt(c, map))
			{
				return false;
			}
			if (this.thingToPlace.rotatable)
			{
				int num = Rand.RangeInclusive(1, 4);
				for (int i = 0; i < 4; i++)
				{
					this.rotation = new Rot4((i + num) % 4);
					if (this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace))
					{
						return true;
					}
				}
			}
			this.rotation = this.thingToPlace.defaultPlacingRot;
			return this.CanPlaceThingAt(c, this.rotation, map, this.thingToPlace);
		}

		// Token: 0x0600B43A RID: 46138 RVA: 0x0033B0CF File Offset: 0x003392CF
		private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
		{
			return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
		}

		// Token: 0x0600B43B RID: 46139 RVA: 0x0033B0DC File Offset: 0x003392DC
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.thingToPlace, null), loc, map, this.rotation, WipeMode.Vanish, false, false);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.2f, 0, int.MaxValue, null);
			if (thing.def == ThingDefOf.AncientWarwalkerTorso)
			{
				if (Rand.Chance(0.75f))
				{
					int num = ThingDefOf.AncientWarwalkerTorso.size.z / 2 + ThingDefOf.AncientWarwalkerLeg.size.z / 2 + Rand.Range(1, 3);
					IntVec3 intVec = thing.Position + new IntVec3(Rand.Range(-3, 3), 0, -num).RotatedBy(this.rotation);
					if (this.CanPlaceThingAt(intVec, this.rotation, map, ThingDefOf.AncientWarwalkerLeg))
					{
						ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientWarwalkerLeg, null), intVec, map, this.rotation, WipeMode.Vanish, false, false), map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
					}
				}
				if (Rand.Chance(0.75f))
				{
					int newX = (ThingDefOf.AncientWarwalkerTorso.size.x / 2 + ThingDefOf.AncientWarwalkerClaw.size.x / 2 + Rand.Range(3, 5)) * (Rand.Bool ? -1 : 1);
					IntVec3 intVec2 = thing.Position + new IntVec3(newX, 0, Rand.Range(-1, 1)).RotatedBy(this.rotation);
					if (this.CanPlaceThingAt(intVec2, this.rotation, map, ThingDefOf.AncientWarwalkerClaw))
					{
						Thing thing2 = GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientWarwalkerClaw, null), intVec2, map, this.rotation, WipeMode.Vanish, false, false);
						ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_MachineBits, 0.5f, 1, int.MaxValue, null);
						ScatterDebrisUtility.ScatterFilthAroundThing(thing2, map, ThingDefOf.Filth_OilSmear, 0.2f, 0, int.MaxValue, null);
					}
				}
			}
		}

		// Token: 0x04007B9F RID: 31647
		private const float MechClawChance = 0.75f;

		// Token: 0x04007BA0 RID: 31648
		private const float MechLegChance = 0.75f;

		// Token: 0x04007BA1 RID: 31649
		private const float OilSmearChance = 0.2f;

		// Token: 0x04007BA2 RID: 31650
		private ThingDef thingToPlace;

		// Token: 0x04007BA3 RID: 31651
		private Rot4 rotation;
	}
}
