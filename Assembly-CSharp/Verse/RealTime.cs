using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D1E RID: 3358
	public static class RealTime
	{
		// Token: 0x17000D58 RID: 3416
		// (get) Token: 0x060055BE RID: 21950 RVA: 0x001BB1D8 File Offset: 0x001B93D8
		public static float LastRealTime
		{
			get
			{
				return RealTime.lastRealTime;
			}
		}

		// Token: 0x17000D59 RID: 3417
		// (get) Token: 0x060055BF RID: 21951 RVA: 0x001BB1DF File Offset: 0x001B93DF
		public static float UnpausedRealTime
		{
			get
			{
				return RealTime.unpausedTime;
			}
		}

		// Token: 0x060055C0 RID: 21952 RVA: 0x001BB1E8 File Offset: 0x001B93E8
		public static void Update()
		{
			RealTime.frameCount = Time.frameCount;
			RealTime.deltaTime = Time.deltaTime;
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			RealTime.realDeltaTime = realtimeSinceStartup - RealTime.lastRealTime;
			RealTime.lastRealTime = realtimeSinceStartup;
			if (Current.ProgramState == ProgramState.Playing)
			{
				if (Find.Maps != null && !Find.Maps.Empty<Map>())
				{
					RealTime.moteList.MoteListUpdate();
				}
				if (Current.Game != null)
				{
					RealTime.unpausedTime += RealTime.deltaTime * Find.TickManager.TickRateMultiplier;
				}
			}
			else
			{
				RealTime.moteList.Clear();
			}
			if (DebugSettings.lowFPS && Time.deltaTime < 100f)
			{
				Thread.Sleep((int)(100f - Time.deltaTime));
			}
		}

		// Token: 0x04003DFF RID: 15871
		public static float deltaTime;

		// Token: 0x04003E00 RID: 15872
		public static float realDeltaTime;

		// Token: 0x04003E01 RID: 15873
		public static RealtimeMoteList moteList = new RealtimeMoteList();

		// Token: 0x04003E02 RID: 15874
		public static int frameCount;

		// Token: 0x04003E03 RID: 15875
		private static float unpausedTime;

		// Token: 0x04003E04 RID: 15876
		private static float lastRealTime = 0f;
	}
}
