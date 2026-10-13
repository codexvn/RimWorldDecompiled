using Waylith.Diagnostics;

namespace Waylith.UnityCompat;

public static class UnityCompatDiagnostics
{
	public static void RegisterCrashDiagnostics()
	{
		CrashDiagnostics.Register(delegate
		{
			UnityWorld current = UnityWorld.Current;
			if (current != null)
			{
				UnityWorldDiagnosticsSnapshot diagnosticsSnapshot = current.GetDiagnosticsSnapshot();
				return $"UnityWorld: tracked={diagnosticsSnapshot.TrackedObjects}, gameObjects={diagnosticsSnapshot.LiveGameObjects}, roots={diagnosticsSnapshot.LiveRootGameObjects}, components={diagnosticsSnapshot.LiveComponents}, behaviours={diagnosticsSnapshot.LiveBehaviours}, activeBehaviours={diagnosticsSnapshot.ActiveBehaviours}, resources={diagnosticsSnapshot.ResourceCount}, pendingActivation={diagnosticsSnapshot.PendingActivationCount}, pendingStart={diagnosticsSnapshot.PendingStartCount}, pendingDestroy={diagnosticsSnapshot.PendingDestroyCount}, shutdownInvoked={diagnosticsSnapshot.ShutdownInvoked}";
			}
			return (string?)null;
		});
	}
}
