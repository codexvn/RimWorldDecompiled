using System;
using Gilzoide.ManagedJobs;
using RimWorld.Planet;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using Verse;
using Verse.Glow;

// Token: 0x02003EF5 RID: 16117
[DOTSCompilerGenerated]
internal class __JobReflectionRegistrationOutput__13226535839579459250
{
	// Token: 0x0601618A RID: 90506 RVA: 0x0069C6CC File Offset: 0x0069A8CC
	public static void CreateJobReflectionData()
	{
		try
		{
			IJobExtensions.EarlyJobInit<ManagedJob>();
			IJobForExtensions.EarlyJobInit<ManagedJobFor>();
			IJobParallelForExtensions.EarlyJobInit<ManagedJobParallelFor>();
			IJobParallelForTransformExtensions.EarlyJobInit<ManagedJobParallelForTransform>();
			IJobParallelForExtensions.EarlyJobInit<DynamicDrawManager.CullJob>();
			IJobParallelForExtensions.EarlyJobInit<DynamicDrawManager.ComputeSilhouetteMatricesJob>();
			IJobParallelForExtensions.EarlyJobInit<GlowGrid.CombineColorsJob>();
			IJobExtensions.EarlyJobInit<PathFinderJob>();
			IJobParallelForExtensions.EarlyJobInit<PathGridJob>();
			IJobParallelForExtensions.EarlyJobInit<ComputeGlowGridsJob>();
			IJobParallelForExtensions.EarlyJobInit<FastTileFinder.ComputeQueryJob>();
			IJobExtensions.EarlyJobInit<DisposeJob<ManagedJob>>();
			IJobExtensions.EarlyJobInit<DisposeJob<ManagedJobFor>>();
			IJobExtensions.EarlyJobInit<DisposeJob<ManagedJobParallelFor>>();
			IJobExtensions.EarlyJobInit<DisposeJob<ManagedJobParallelForTransform>>();
		}
		catch (Exception ex)
		{
			EarlyInitHelpers.JobReflectionDataCreationFailed(ex);
		}
	}

	// Token: 0x0601618B RID: 90507 RVA: 0x0069C748 File Offset: 0x0069A948
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	public static void EarlyInit()
	{
		__JobReflectionRegistrationOutput__13226535839579459250.CreateJobReflectionData();
	}
}
