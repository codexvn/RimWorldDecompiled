namespace Unity.Profiling;

public enum ProfilerRecorderOptions
{
	CollectOnlyOnCurrentThread = 4,
	Default = 24,
	GpuRecorder = 64,
	KeepAliveDuringDomainReload = 2,
	None = 0,
	StartImmediately = 1,
	SumAllSamplesInFrame = 16,
	WrapAroundWhenCapacityReached = 8
}
