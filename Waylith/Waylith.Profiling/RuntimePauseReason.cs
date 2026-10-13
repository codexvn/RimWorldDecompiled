namespace Waylith.Profiling;

public enum RuntimePauseReason
{
	Other,
	GC,
	AppDomainShutdown,
	ReJit,
	Shutdown,
	Debugger,
	GCPrep,
	DebuggerSweep,
	Profiler
}
