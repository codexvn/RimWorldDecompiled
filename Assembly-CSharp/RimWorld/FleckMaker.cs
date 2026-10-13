using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020037DF RID: 14303
	public static class FleckMaker
	{
		// Token: 0x06013D1A RID: 81178 RVA: 0x005E2304 File Offset: 0x005E0504
		public static FleckCreationData GetDataStatic(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = loc,
				scale = scale,
				ageTicksOverride = -1
			};
		}

		// Token: 0x06013D1B RID: 81179 RVA: 0x005E233A File Offset: 0x005E053A
		public static void Static(IntVec3 cell, Map map, FleckDef fleckDef, float scale = 1f)
		{
			FleckMaker.Static(cell.ToVector3Shifted(), map, fleckDef, scale);
		}

		// Token: 0x06013D1C RID: 81180 RVA: 0x005E234B File Offset: 0x005E054B
		public static void Static(Vector3 loc, Map map, FleckDef fleckDef, float scale = 1f)
		{
			map.flecks.CreateFleck(FleckMaker.GetDataStatic(loc, map, fleckDef, scale));
		}

		// Token: 0x06013D1D RID: 81181 RVA: 0x005E2364 File Offset: 0x005E0564
		public static FleckCreationData GetDataThrowMetaIcon(IntVec3 cell, Map map, FleckDef fleckDef, float velocitySpeed = 0.42f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = cell.ToVector3Shifted() + new Vector3(0.35f, 0f, 0.35f) + new Vector3(Rand.Value, 0f, Rand.Value) * 0.1f,
				velocityAngle = (float)Rand.Range(30, 60),
				velocitySpeed = velocitySpeed,
				rotationRate = Rand.Range(-3f, 3f),
				scale = 0.7f,
				ageTicksOverride = -1
			};
		}

		// Token: 0x06013D1E RID: 81182 RVA: 0x005E240F File Offset: 0x005E060F
		public static void ThrowMetaIcon(IntVec3 cell, Map map, FleckDef fleckDef, float velocitySpeed = 0.42f)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			map.flecks.CreateFleck(FleckMaker.GetDataThrowMetaIcon(cell, map, fleckDef, velocitySpeed));
		}

		// Token: 0x06013D1F RID: 81183 RVA: 0x005E2430 File Offset: 0x005E0630
		public static FleckCreationData GetDataAttachedOverlay(Thing thing, FleckDef fleckDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			return new FleckCreationData
			{
				def = fleckDef,
				spawnPosition = thing.DrawPos + offset,
				solidTimeOverride = new float?(solidTimeOverride),
				scale = scale,
				ageTicksOverride = -1
			};
		}

		// Token: 0x06013D20 RID: 81184 RVA: 0x005E247F File Offset: 0x005E067F
		public static void AttachedOverlay(Thing thing, FleckDef fleckDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			if (thing.Destroyed)
			{
				return;
			}
			thing.MapHeld.flecks.CreateFleck(FleckMaker.GetDataAttachedOverlay(thing, fleckDef, offset, scale, solidTimeOverride));
		}

		// Token: 0x06013D21 RID: 81185 RVA: 0x005E24A8 File Offset: 0x005E06A8
		public static void ThrowShamblerParticles(Thing thing)
		{
			if (!thing.Spawned)
			{
				return;
			}
			FleckCreationData dataAttachedOverlay = FleckMaker.GetDataAttachedOverlay(thing, FleckDefOf.FleckShamblerDecay, Vector3.zero, Rand.Range(0.8f, 1.2f), -1f);
			dataAttachedOverlay.spawnPosition += new Vector3(Rand.Range(-0.1f, 0.1f), 0f, Rand.Range(0f, 0.5f));
			dataAttachedOverlay.rotationRate = Rand.Range(-12f, 12f);
			dataAttachedOverlay.velocityAngle = Rand.Range(-35f, 35f);
			dataAttachedOverlay.velocitySpeed = Rand.Range(0.1f, 0.3f);
			dataAttachedOverlay.solidTimeOverride = new float?(Rand.Range(0.7f, 1f));
			thing.MapHeld.flecks.CreateFleck(dataAttachedOverlay);
		}

		// Token: 0x06013D22 RID: 81186 RVA: 0x005E2590 File Offset: 0x005E0790
		public static void ThrowMetaPuffs(CellRect rect, Map map)
		{
			if (!Find.TickManager.Paused)
			{
				for (int i = rect.minX; i <= rect.maxX; i++)
				{
					for (int j = rect.minZ; j <= rect.maxZ; j++)
					{
						FleckMaker.ThrowMetaPuffs(new TargetInfo(new IntVec3(i, 0, j), map, false));
					}
				}
			}
		}

		// Token: 0x06013D23 RID: 81187 RVA: 0x005E25EC File Offset: 0x005E07EC
		public static void ThrowMetaPuffs(TargetInfo targ)
		{
			Vector3 a = targ.HasThing ? targ.Thing.TrueCenter() : targ.Cell.ToVector3Shifted();
			int num = Rand.RangeInclusive(4, 6);
			for (int i = 0; i < num; i++)
			{
				FleckMaker.ThrowMetaPuff(a + new Vector3(Rand.Range(-0.5f, 0.5f), 0f, Rand.Range(-0.5f, 0.5f)), targ.Map);
			}
		}

		// Token: 0x06013D24 RID: 81188 RVA: 0x005E2670 File Offset: 0x005E0870
		public static void ThrowMetaPuff(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.MetaPuff, 1.9f);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.78f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D25 RID: 81189 RVA: 0x005E26E0 File Offset: 0x005E08E0
		public static void ThrowAirPuffUp(Vector3 loc, Map map)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f)), map, FleckDefOf.AirPuff, 1.5f);
			dataStatic.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			dataStatic.velocityAngle = (float)Rand.Range(-45, 45);
			dataStatic.velocitySpeed = Rand.Range(1.2f, 1.5f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D26 RID: 81190 RVA: 0x005E2788 File Offset: 0x005E0988
		public static void ThrowBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f)), map, FleckDefOf.AirPuff, Rand.Range(0.6f, 0.7f));
			dataStatic.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			dataStatic.velocityAngle = throwAngle + (float)Rand.Range(-10, 10);
			dataStatic.velocitySpeed = Rand.Range(0.1f, 0.8f);
			dataStatic.velocity = new Vector3?(inheritVelocity * 0.5f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D27 RID: 81191 RVA: 0x005E2851 File Offset: 0x005E0A51
		public static void ThrowDustPuff(IntVec3 cell, Map map, float scale)
		{
			FleckMaker.ThrowDustPuff(cell.ToVector3() + new Vector3(Rand.Value, 0f, Rand.Value), map, scale);
		}

		// Token: 0x06013D28 RID: 81192 RVA: 0x005E287C File Offset: 0x005E0A7C
		public static void ThrowDustPuff(Vector3 loc, Map map, float scale)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.DustPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D29 RID: 81193 RVA: 0x005E28F0 File Offset: 0x005E0AF0
		public static void ThrowDustPuffThick(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.DustPuffThick, scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D2A RID: 81194 RVA: 0x005E295C File Offset: 0x005E0B5C
		public static void ThrowTornadoDustPuff(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.TornadoDustPuff, 1.9f * scale);
			dataStatic.rotationRate = (float)Rand.Range(-60, 60);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.6f, 0.75f);
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D2B RID: 81195 RVA: 0x005E29DC File Offset: 0x005E0BDC
		public static void ThrowSmoke(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Smoke, Rand.Range(1.5f, 2.5f) * size);
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(30, 40);
			dataStatic.velocitySpeed = Rand.Range(0.5f, 0.7f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D2C RID: 81196 RVA: 0x005E2A5C File Offset: 0x005E0C5C
		public static void ThrowFireGlow(Vector3 c, Map map, float size)
		{
			if (!c.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			Vector3 vector = c + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, FleckDefOf.FireGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 0.12f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D2D RID: 81197 RVA: 0x005E2B0C File Offset: 0x005E0D0C
		public static void ThrowHeatGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (!vector.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(vector, map, FleckDefOf.HeatGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 0.12f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D2E RID: 81198 RVA: 0x005E2BC4 File Offset: 0x005E0DC4
		public static void ThrowMicroSparks(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			loc -= new Vector3(0.5f, 0f, 0.5f);
			loc += new Vector3(Rand.Value, 0f, Rand.Value);
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.MicroSparks, Rand.Range(0.8f, 1.2f));
			dataStatic.rotationRate = Rand.Range(-12f, 12f);
			dataStatic.velocityAngle = (float)Rand.Range(35, 45);
			dataStatic.velocitySpeed = 1.2f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D2F RID: 81199 RVA: 0x005E2C70 File Offset: 0x005E0E70
		public static void ThrowLightningGlow(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f), map, FleckDefOf.LightningGlow, Rand.Range(4f, 6f) * size);
			dataStatic.rotationRate = Rand.Range(-3f, 3f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = 1.2f;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D30 RID: 81200 RVA: 0x005E2D14 File Offset: 0x005E0F14
		public static void PlaceFootprint(Vector3 loc, Map map, float rot)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, FleckDefOf.Footprint, 0.5f);
			dataStatic.rotation = rot;
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D31 RID: 81201 RVA: 0x005E2D52 File Offset: 0x005E0F52
		public static void ThrowHorseshoe(Pawn thrower, IntVec3 targetCell)
		{
			FleckMaker.ThrowObjectAt(thrower, targetCell, FleckDefOf.Horseshoe);
		}

		// Token: 0x06013D32 RID: 81202 RVA: 0x005E2D60 File Offset: 0x005E0F60
		public static void ThrowStone(Pawn thrower, IntVec3 targetCell)
		{
			FleckMaker.ThrowObjectAt(thrower, targetCell, FleckDefOf.Stone);
		}

		// Token: 0x06013D33 RID: 81203 RVA: 0x005E2D70 File Offset: 0x005E0F70
		private static void ThrowObjectAt(Pawn thrower, IntVec3 targetCell, FleckDef fleck)
		{
			if (!thrower.Position.ShouldSpawnMotesAt(thrower.Map, true))
			{
				return;
			}
			float num = Rand.Range(3.8f, 5.6f);
			Vector3 vector = targetCell.ToVector3Shifted() + Vector3Utility.RandomHorizontalOffset((1f - (float)thrower.skills.GetSkill(SkillDefOf.Shooting).Level / 20f) * 1.8f);
			vector.y = thrower.DrawPos.y;
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(thrower.DrawPos, thrower.Map, fleck, 1f);
			dataStatic.rotationRate = (float)Rand.Range(-300, 300);
			dataStatic.velocityAngle = (vector - dataStatic.spawnPosition).AngleFlat();
			dataStatic.velocitySpeed = num;
			dataStatic.airTimeLeft = new float?((float)Mathf.RoundToInt((dataStatic.spawnPosition - vector).MagnitudeHorizontal() / num));
			thrower.Map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D34 RID: 81204 RVA: 0x005E2E74 File Offset: 0x005E1074
		public static void ThrowExplosionCell(IntVec3 cell, Map map, FleckDef fleckDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(cell.ToVector3Shifted(), map, fleckDef, 1f);
			dataStatic.rotation = (float)(90 * Rand.RangeInclusive(0, 3));
			dataStatic.instanceColor = new Color?(color);
			map.flecks.CreateFleck(dataStatic);
			if (Rand.Value < 0.7f)
			{
				FleckMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x06013D35 RID: 81205 RVA: 0x005E2EE4 File Offset: 0x005E10E4
		public static void ThrowExplosionInterior(Vector3 loc, Map map, FleckDef fleckDef)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(loc, map, fleckDef, Rand.Range(3f, 4.5f));
			dataStatic.rotationRate = Rand.Range(-30f, 30f);
			dataStatic.velocityAngle = (float)Rand.Range(0, 360);
			dataStatic.velocitySpeed = Rand.Range(0.48f, 0.72f);
			map.flecks.CreateFleck(dataStatic);
		}

		// Token: 0x06013D36 RID: 81206 RVA: 0x005E2F60 File Offset: 0x005E1160
		public static void WaterSplash(Vector3 loc, Map map, float size, float velocity)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			map.flecks.CreateFleck(new FleckCreationData
			{
				def = FleckDefOf.WaterSplash,
				targetSize = size,
				velocitySpeed = velocity,
				spawnPosition = loc
			});
		}

		// Token: 0x06013D37 RID: 81207 RVA: 0x005E2FB0 File Offset: 0x005E11B0
		public static void WaterRipple(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map, true))
			{
				return;
			}
			map.flecks.CreateFleck(new FleckCreationData
			{
				def = FleckDefOf.WaterRipple,
				spawnPosition = loc,
				scale = size,
				ageTicksOverride = -1
			});
		}

		// Token: 0x06013D38 RID: 81208 RVA: 0x005E3000 File Offset: 0x005E1200
		public static void ConnectingLine(Vector3 start, Vector3 end, FleckDef fleckDef, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			FleckCreationData dataStatic = FleckMaker.GetDataStatic(start + vector * 0.5f, map, fleckDef, 1f);
			dataStatic.exactScale = new Vector3?(new Vector3(x, 1f, width));
			dataStatic.rotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			map.flecks.CreateFleck(dataStatic);
		}
	}
}
