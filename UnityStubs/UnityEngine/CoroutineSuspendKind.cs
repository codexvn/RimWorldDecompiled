namespace UnityEngine;

internal enum CoroutineSuspendKind
{
	NextFrame,
	FixedUpdate,
	EndOfFrame,
	WaitSeconds,
	AwaitCoroutine,
	AwaitOperation
}
