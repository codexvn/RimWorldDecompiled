using System;
using System.Linq;
using System.Threading;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000132 RID: 306
	public static class UnityData
	{
		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0002D255 File Offset: 0x0002B455
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x0002D268 File Offset: 0x0002B468
		public static bool Is32BitBuild
		{
			get
			{
				return IntPtr.Size == 4;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0002D272 File Offset: 0x0002B472
		public static bool Is64BitBuild
		{
			get
			{
				return IntPtr.Size == 8;
			}
		}

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06000978 RID: 2424 RVA: 0x0002D27C File Offset: 0x0002B47C
		public static int MaxJobWorkerThreadCount
		{
			get
			{
				return UnityData.maximumJobWorkerThreads;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0002D283 File Offset: 0x0002B483
		public static int MaxJobWorkerCount
		{
			get
			{
				return UnityData.maximumJobWorkerCount;
			}
		}

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0002D28A File Offset: 0x0002B48A
		public static bool ComputeShadersSupported
		{
			get
			{
				return UnityData.computeShadersSupported;
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x0002D291 File Offset: 0x0002B491
		public static int GetIdealBatchCount(int items)
		{
			return Mathf.Max(items / UnityData.maximumJobWorkerThreads, 4);
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600097C RID: 2428 RVA: 0x0002D2A0 File Offset: 0x0002B4A0
		// (remove) Token: 0x0600097D RID: 2429 RVA: 0x0002D2D4 File Offset: 0x0002B4D4
		public static event Action DisposeStatic;

		// Token: 0x0600097E RID: 2430 RVA: 0x0002D307 File Offset: 0x0002B507
		public static void DisposeStaticResources()
		{
			Action disposeStatic = UnityData.DisposeStatic;
			if (disposeStatic != null)
			{
				disposeStatic();
			}
			UnityData.DisposeStatic = null;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x0002D31F File Offset: 0x0002B51F
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.");
			}
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0002D33C File Offset: 0x0002B53C
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.maximumJobWorkerThreads = JobsUtility.ThreadIndexCount + 1;
			UnityData.maximumJobWorkerCount = JobsUtility.JobWorkerCount;
			UnityData.computeShadersSupported = (SystemInfo.supportsComputeShaders && SystemInfo.graphicsMemorySize > 5000 && !UnityData.IsIntegratedGraphicsCard() && !Environment.GetCommandLineArgs().Contains("-disable-compute-shaders"));
			UnityData.initialized = true;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x0002D3D0 File Offset: 0x0002B5D0
		private static bool IsIntegratedGraphicsCard()
		{
			return SystemInfo.graphicsDeviceVendorID == 32902 || SystemInfo.graphicsDeviceVendorID == 4203 || UnityData.IsAmdIntegratedGraphics();
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x0002D3F4 File Offset: 0x0002B5F4
		private static bool IsAmdIntegratedGraphics()
		{
			if (SystemInfo.graphicsDeviceVendorID != 4098)
			{
				return false;
			}
			string text = SystemInfo.graphicsDeviceName.ToLowerInvariant();
			int num = text.IndexOf("m graphics", StringComparison.InvariantCulture);
			return (num > 1 && char.IsDigit(text[num - 1])) || text.Contains("vega") || (text.Contains("integrated") || text.Contains("apu")) || text.Contains("ryzen");
		}

		// Token: 0x040004B3 RID: 1203
		private static bool initialized;

		// Token: 0x040004B4 RID: 1204
		public static bool isEditor;

		// Token: 0x040004B5 RID: 1205
		public static string dataPath;

		// Token: 0x040004B6 RID: 1206
		public static RuntimePlatform platform;

		// Token: 0x040004B7 RID: 1207
		public static string persistentDataPath;

		// Token: 0x040004B8 RID: 1208
		private static int mainThreadId;

		// Token: 0x040004B9 RID: 1209
		private static int maximumJobWorkerThreads;

		// Token: 0x040004BA RID: 1210
		private static int maximumJobWorkerCount;

		// Token: 0x040004BB RID: 1211
		private static bool computeShadersSupported;
	}
}
