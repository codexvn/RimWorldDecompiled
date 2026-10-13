using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020021A4 RID: 8612
	[StaticConstructorOnStartup]
	public class WeatherEvent_LightningStrike : WeatherEvent_LightningFlash
	{
		// Token: 0x0600BC2E RID: 48174 RVA: 0x003689E0 File Offset: 0x00366BE0
		public WeatherEvent_LightningStrike(Map map) : base(map)
		{
		}

		// Token: 0x0600BC2F RID: 48175 RVA: 0x003689F4 File Offset: 0x00366BF4
		public WeatherEvent_LightningStrike(Map map, IntVec3 forcedStrikeLoc) : base(map)
		{
			this.strikeLoc = forcedStrikeLoc;
		}

		// Token: 0x0600BC30 RID: 48176 RVA: 0x00368A0F File Offset: 0x00366C0F
		public override void FireEvent()
		{
			WeatherEvent_LightningStrike.DoStrike(this.strikeLoc, this.map, ref this.boltMesh);
		}

		// Token: 0x0600BC31 RID: 48177 RVA: 0x00368A28 File Offset: 0x00366C28
		public static void DoStrike(IntVec3 strikeLoc, Map map, ref Mesh boltMesh)
		{
			SoundDefOf.Thunder_OffMap.PlayOneShotOnCamera(map);
			if (!strikeLoc.IsValid)
			{
				strikeLoc = CellFinderLoose.RandomCellWith((IntVec3 sq) => sq.Standable(map) && !map.roofGrid.Roofed(sq), map, 1000);
			}
			boltMesh = LightningBoltMeshPool.RandomBoltMesh;
			if (!strikeLoc.Fogged(map))
			{
				GenExplosion.DoExplosion(strikeLoc, map, 1.9f, DamageDefOf.Flame, null, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
				Vector3 loc = strikeLoc.ToVector3Shifted();
				for (int i = 0; i < 4; i++)
				{
					FleckMaker.ThrowSmoke(loc, map, 1.5f);
					FleckMaker.ThrowMicroSparks(loc, map);
					FleckMaker.ThrowLightningGlow(loc, map, 1.5f);
				}
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(strikeLoc, map, false), MaintenanceType.None);
			SoundDefOf.Thunder_OnMap.PlayOneShot(info);
		}

		// Token: 0x0600BC32 RID: 48178 RVA: 0x00368B6F File Offset: 0x00366D6F
		public override void WeatherEventDraw()
		{
			Graphics.DrawMesh(this.boltMesh, this.strikeLoc.ToVector3ShiftedWithAltitude(AltitudeLayer.Weather), Quaternion.identity, FadedMaterialPool.FadedVersionOf(WeatherEvent_LightningStrike.LightningMat, this.LightningBrightness), 0);
		}

		// Token: 0x04008198 RID: 33176
		private IntVec3 strikeLoc = IntVec3.Invalid;

		// Token: 0x04008199 RID: 33177
		private Mesh boltMesh;

		// Token: 0x0400819A RID: 33178
		private static readonly Material LightningMat = MatLoader.LoadMat("Weather/LightningBolt", -1);
	}
}
