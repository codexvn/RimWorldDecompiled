using System;

namespace Verse
{
	// Token: 0x0200015C RID: 348
	public static class MurmurHash
	{
		// Token: 0x06000A3E RID: 2622 RVA: 0x0002FEBC File Offset: 0x0002E0BC
		public static int GetInt(uint seed, uint input)
		{
			uint num = input * 3432918353U;
			num = (num << 15 | num >> 17);
			num *= 461845907U;
			uint num2 = seed ^ num;
			num2 = (num2 << 13 | num2 >> 19);
			num2 = num2 * 5U + 3864292196U;
			num2 ^= 2834544218U;
			num2 ^= num2 >> 16;
			num2 *= 2246822507U;
			num2 ^= num2 >> 13;
			num2 *= 3266489909U;
			return (int)(num2 ^ num2 >> 16);
		}

		// Token: 0x04000535 RID: 1333
		private const uint Const1 = 3432918353U;

		// Token: 0x04000536 RID: 1334
		private const uint Const2 = 461845907U;

		// Token: 0x04000537 RID: 1335
		private const uint Const3 = 3864292196U;

		// Token: 0x04000538 RID: 1336
		private const uint Const4Mix = 2246822507U;

		// Token: 0x04000539 RID: 1337
		private const uint Const5Mix = 3266489909U;

		// Token: 0x0400053A RID: 1338
		private const uint Const6StreamPosition = 2834544218U;
	}
}
