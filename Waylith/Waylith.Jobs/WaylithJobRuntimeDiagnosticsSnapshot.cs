namespace Waylith.Jobs;

public readonly record struct WaylithJobRuntimeDiagnosticsSnapshot(int CurrentFrame, int TotalHandles, int PendingHandles, int RunningHandles, int CompletedHandles, int FaultedHandles);
