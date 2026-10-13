using System.Collections;

namespace UnityEngine;

public class MonoBehaviour : Behaviour
{
	public bool useGUILayout
	{
		set
		{
		}
	}

	public Coroutine StartCoroutine(IEnumerator routine)
	{
		CoroutineHandle handle = CoroutineScheduler.Start(this, routine);
		return new Coroutine
		{
			Handle = handle
		};
	}

	public void StopCoroutine(Coroutine coroutine)
	{
		CoroutineScheduler.Stop(coroutine?.Handle);
	}

	public void StopAllCoroutines()
	{
		CoroutineScheduler.StopAll(this);
	}
}
