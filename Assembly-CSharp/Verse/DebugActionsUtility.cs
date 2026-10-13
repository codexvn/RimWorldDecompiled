using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020009B6 RID: 2486
	public static class DebugActionsUtility
	{
		// Token: 0x06003EE7 RID: 16103 RVA: 0x00149580 File Offset: 0x00147780
		public static void DustPuffFrom(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				pawn.Drawer.Notify_DebugAffected();
			}
		}

		// Token: 0x06003EE8 RID: 16104 RVA: 0x001495A2 File Offset: 0x001477A2
		public static IEnumerable<float> PointsOptions(bool extended)
		{
			if (!extended)
			{
				yield return 35f;
				yield return 70f;
				yield return 100f;
				yield return 150f;
				yield return 200f;
				yield return 350f;
				yield return 500f;
				yield return 700f;
				yield return 1000f;
				yield return 1200f;
				yield return 1500f;
				yield return 2000f;
				yield return 3000f;
				yield return 4000f;
				yield return 5000f;
			}
			else
			{
				for (int i = 20; i < 100; i += 10)
				{
					yield return (float)i;
				}
				for (int i = 100; i < 500; i += 25)
				{
					yield return (float)i;
				}
				for (int i = 500; i < 1500; i += 50)
				{
					yield return (float)i;
				}
				for (int i = 1500; i <= 5000; i += 100)
				{
					yield return (float)i;
				}
			}
			yield return 6000f;
			yield return 7000f;
			yield return 8000f;
			yield return 9000f;
			yield return 10000f;
			yield break;
		}

		// Token: 0x06003EE9 RID: 16105 RVA: 0x001495B2 File Offset: 0x001477B2
		public static IEnumerable<int> PopulationOptions()
		{
			int num;
			for (int i = 1; i <= 20; i = num + 1)
			{
				yield return i;
				num = i;
			}
			for (int i = 30; i <= 50; i += 10)
			{
				yield return i;
			}
			yield break;
		}

		// Token: 0x06003EEA RID: 16106 RVA: 0x001495BB File Offset: 0x001477BB
		public static IEnumerable<int> RadiusOptions()
		{
			yield return 1;
			yield return 2;
			yield return 3;
			yield return 4;
			yield return 5;
			yield return 6;
			yield return 7;
			yield return 8;
			yield return 9;
			yield return 10;
			yield return 11;
			yield return 12;
			yield return 13;
			yield return 14;
			yield return 15;
			yield return 20;
			yield return 25;
			yield return 30;
			yield break;
		}
	}
}
