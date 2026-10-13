namespace UnityEngine;

internal enum CoroutineSuspendKind
{
	NextFrame,
	EndOfFrame,
	WaitSeconds,
	AwaitCoroutine,
	AwaitOperation
}
