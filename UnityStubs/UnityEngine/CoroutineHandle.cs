using System.Collections;
using System.Collections.Generic;

namespace UnityEngine;

internal sealed class CoroutineHandle
{
	internal readonly Stack<IEnumerator> Stack = new Stack<IEnumerator>();

	internal CoroutineSuspendKind SuspendKind;

	internal float ResumeTime;

	internal CoroutineHandle AwaitedHandle;

	internal AsyncOperation AwaitedOperation;

	internal bool IsDone;

	internal object Owner;
}
