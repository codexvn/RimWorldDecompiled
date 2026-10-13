using System;
using Unity.Profiling;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CAB RID: 3243
	public static class MemoryUsageUtility
	{
		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x0600531F RID: 21279 RVA: 0x001ACB23 File Offset: 0x001AAD23
		public static long TrackedMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.totalMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x06005320 RID: 21280 RVA: 0x001ACB2F File Offset: 0x001AAD2F
		public static long OsMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.systemMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x06005321 RID: 21281 RVA: 0x001ACB3B File Offset: 0x001AAD3B
		public static long GraphicsMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.gfxMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x06005322 RID: 21282 RVA: 0x001ACB47 File Offset: 0x001AAD47
		public static long ManagedMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.gcMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x06005323 RID: 21283 RVA: 0x001ACB53 File Offset: 0x001AAD53
		public static long ManagedMemoryReservedBytes
		{
			get
			{
				return MemoryUsageUtility.gcReservedMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x06005324 RID: 21284 RVA: 0x001ACB5F File Offset: 0x001AAD5F
		public static long AudioMemoryUsageBytes
		{
			get
			{
				return MemoryUsageUtility.audioMemoryRecorder.CurrentValue;
			}
		}

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x06005325 RID: 21285 RVA: 0x001ACB6B File Offset: 0x001AAD6B
		public static long TextureMemoryUsageBytes
		{
			get
			{
				return (long)Texture.currentTextureMemory;
			}
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x001ACB74 File Offset: 0x001AAD74
		static MemoryUsageUtility()
		{
			MemoryUsageUtility.systemMemoryRecorder.Stop();
			MemoryUsageUtility.totalMemoryRecorder.Stop();
			MemoryUsageUtility.gfxMemoryRecorder.Stop();
			MemoryUsageUtility.gcMemoryRecorder.Stop();
			MemoryUsageUtility.gcReservedMemoryRecorder.Stop();
			MemoryUsageUtility.audioMemoryRecorder.Stop();
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x001ACC48 File Offset: 0x001AAE48
		public static void SetShouldRecord(bool shouldRecord)
		{
			if (shouldRecord)
			{
				MemoryUsageUtility.systemMemoryRecorder.Start();
				MemoryUsageUtility.totalMemoryRecorder.Start();
				MemoryUsageUtility.gfxMemoryRecorder.Start();
				MemoryUsageUtility.gcMemoryRecorder.Start();
				MemoryUsageUtility.gcReservedMemoryRecorder.Start();
				MemoryUsageUtility.audioMemoryRecorder.Start();
				return;
			}
			MemoryUsageUtility.systemMemoryRecorder.Stop();
			MemoryUsageUtility.totalMemoryRecorder.Stop();
			MemoryUsageUtility.gfxMemoryRecorder.Stop();
			MemoryUsageUtility.gcMemoryRecorder.Stop();
			MemoryUsageUtility.gcReservedMemoryRecorder.Stop();
			MemoryUsageUtility.audioMemoryRecorder.Stop();
		}

		// Token: 0x04003C09 RID: 15369
		private static ProfilerRecorder systemMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04003C0A RID: 15370
		private static ProfilerRecorder totalMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04003C0B RID: 15371
		private static ProfilerRecorder gfxMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Gfx Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04003C0C RID: 15372
		private static ProfilerRecorder gcMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Used Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04003C0D RID: 15373
		private static ProfilerRecorder gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory", 1, ProfilerRecorderOptions.Default);

		// Token: 0x04003C0E RID: 15374
		private static ProfilerRecorder audioMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Audio Used Memory", 1, ProfilerRecorderOptions.Default);
	}
}
