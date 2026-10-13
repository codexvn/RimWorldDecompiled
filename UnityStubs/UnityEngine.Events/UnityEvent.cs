using System;

namespace UnityEngine.Events;

public class UnityEvent : UnityEventBase
{
	public void AddListener(UnityAction call)
	{
		AddCall(call);
	}

	public void RemoveListener(UnityAction call)
	{
		RemoveCall(call);
	}

	public void Invoke()
	{
		Delegate[] array = Snapshot();
		for (int i = 0; i < array.Length; i++)
		{
			((UnityAction)array[i])();
		}
	}
}
public class UnityEvent<T0> : UnityEventBase
{
	public void AddListener(UnityAction<T0> call)
	{
		AddCall(call);
	}

	public void RemoveListener(UnityAction<T0> call)
	{
		RemoveCall(call);
	}

	public void Invoke(T0 arg0)
	{
		Delegate[] array = Snapshot();
		for (int i = 0; i < array.Length; i++)
		{
			((UnityAction<T0>)array[i])(arg0);
		}
	}
}
public class UnityEvent<T0, T1> : UnityEventBase
{
	public void AddListener(UnityAction<T0, T1> call)
	{
		AddCall(call);
	}

	public void RemoveListener(UnityAction<T0, T1> call)
	{
		RemoveCall(call);
	}

	public void Invoke(T0 arg0, T1 arg1)
	{
		Delegate[] array = Snapshot();
		for (int i = 0; i < array.Length; i++)
		{
			((UnityAction<T0, T1>)array[i])(arg0, arg1);
		}
	}
}
public class UnityEvent<T0, T1, T2> : UnityEventBase
{
	public void AddListener(UnityAction<T0, T1, T2> call)
	{
		AddCall(call);
	}

	public void RemoveListener(UnityAction<T0, T1, T2> call)
	{
		RemoveCall(call);
	}

	public void Invoke(T0 arg0, T1 arg1, T2 arg2)
	{
		Delegate[] array = Snapshot();
		for (int i = 0; i < array.Length; i++)
		{
			((UnityAction<T0, T1, T2>)array[i])(arg0, arg1, arg2);
		}
	}
}
