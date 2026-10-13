using System;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	// Token: 0x020020AF RID: 8367
	public static class MapNoiseUtility
	{
		// Token: 0x0600B725 RID: 46885 RVA: 0x0034B34C File Offset: 0x0034954C
		public static ModuleBase FalloffAtEdge(float width, Rot4 side, Map map)
		{
			ModuleBase moduleBase = new DistFromAxis(width);
			if (side == Rot4.North)
			{
				moduleBase = new Rotate(0.0, 90.0, 0.0, moduleBase);
				moduleBase = new Translate(0.0, 0.0, (double)(-(double)map.Size.z), moduleBase);
			}
			if (side == Rot4.East)
			{
				moduleBase = new Translate((double)(-(double)map.Size.x), 0.0, 0.0, moduleBase);
			}
			if (side == Rot4.South)
			{
				moduleBase = new Rotate(0.0, 90.0, 0.0, moduleBase);
			}
			return moduleBase;
		}

		// Token: 0x0600B726 RID: 46886 RVA: 0x0034B418 File Offset: 0x00349618
		public static ModuleBase FalloffAtAngle(float angle, float offsetPct, Map map)
		{
			ModuleBase input = new DistFromAxis_Directional((float)map.Size.x / 2f);
			input = new ScaleBias(0.5, 0.5, input);
			input = new Translate((double)((float)map.Size.x * (0.5f - offsetPct)), 0.0, 0.0, input);
			input = new Rotate(0.0, (double)angle, 0.0, input);
			return new Translate((double)((float)(-(float)map.Size.x) / 2f), 0.0, (double)((float)(-(float)map.Size.z) / 2f), input);
		}

		// Token: 0x0600B727 RID: 46887 RVA: 0x0034B4D8 File Offset: 0x003496D8
		public static ModuleBase CreateFalloffRadius(float radius, Vector2 offset, float exponent = 1f, bool invert = true)
		{
			ModuleBase moduleBase = new DistFromPoint(radius);
			if (invert)
			{
				moduleBase = new ScaleBias(-1.0, 1.0, moduleBase);
			}
			moduleBase = new Clamp(0.0, 1.0, moduleBase);
			moduleBase = new Power(moduleBase, new Const((double)exponent));
			return new Translate((double)(-(double)offset.x), 0.0, (double)(-(double)offset.y), moduleBase);
		}

		// Token: 0x0600B728 RID: 46888 RVA: 0x0034B550 File Offset: 0x00349750
		public static ModuleBase AddDisplacementNoise(ModuleBase baseShape, float frequency, float strength, int octaves = 4, int seed = -1)
		{
			if (seed < 0)
			{
				seed = Rand.Int;
			}
			ModuleBase moduleBase = new Perlin((double)frequency, 2.0, 0.5, octaves, seed, QualityMode.Medium);
			ModuleBase moduleBase2 = new Perlin((double)frequency, 2.0, 0.5, octaves, seed + 1, QualityMode.Medium);
			moduleBase = new Multiply(moduleBase, new Const((double)strength));
			moduleBase2 = new Multiply(moduleBase2, new Const((double)strength));
			return new Displace(baseShape, moduleBase, new Const(0.0), moduleBase2);
		}
	}
}
