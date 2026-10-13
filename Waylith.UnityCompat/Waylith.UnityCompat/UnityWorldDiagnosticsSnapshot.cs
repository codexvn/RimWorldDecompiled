namespace Waylith.UnityCompat;

public readonly record struct UnityWorldDiagnosticsSnapshot(int TrackedObjects, int LiveGameObjects, int LiveRootGameObjects, int LiveComponents, int LiveBehaviours, int ActiveBehaviours, int DestroyedStates, int AssetTokens, int ResourceCount, int PendingActivationCount, int PendingStartCount, int PendingDestroyCount, bool ShutdownInvoked);
