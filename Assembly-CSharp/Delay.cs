using System;
using System.Collections;
using UnityEngine;
using Verse;

// Token: 0x0200004C RID: 76
public static class Delay
{
	// Token: 0x06000189 RID: 393 RVA: 0x0000A2B2 File Offset: 0x000084B2
	private static Coroutine StartCoroutine(IEnumerator coroutine)
	{
		if (!Delay.coroutineRunner)
		{
			Delay.coroutineRunner = new GameObject("CoroutineRunner").AddComponent<Delay.CoroutineRunner>();
		}
		return Delay.coroutineRunner.StartCoroutine(coroutine);
	}

	// Token: 0x0600018A RID: 394 RVA: 0x0000A2DF File Offset: 0x000084DF
	public static void AfterNSeconds(float seconds, Action action)
	{
		Delay.StartCoroutine(Delay.Internal_AfterNSeconds(seconds, action));
	}

	// Token: 0x0600018B RID: 395 RVA: 0x0000A2EE File Offset: 0x000084EE
	private static IEnumerator Internal_AfterNSeconds(float seconds, Action action)
	{
		yield return new WaitForSeconds(seconds);
		if (action != null)
		{
			action();
		}
		yield break;
	}

	// Token: 0x0600018C RID: 396 RVA: 0x0000A304 File Offset: 0x00008504
	public static void AfterNTicks(int ticks, Action action)
	{
		Delay.StartCoroutine(Delay.Internal_AfterNTicks(GenTicks.TicksGame + ticks, action));
	}

	// Token: 0x0600018D RID: 397 RVA: 0x0000A319 File Offset: 0x00008519
	private static IEnumerator Internal_AfterNTicks(int targetTick, Action action)
	{
		yield return new WaitUntil(() => GenTicks.TicksGame >= targetTick);
		if (action != null)
		{
			action();
		}
		yield break;
	}

	// Token: 0x040000C3 RID: 195
	private static Delay.CoroutineRunner coroutineRunner;

	// Token: 0x0200004D RID: 77
	private class CoroutineRunner : MonoBehaviour
	{
	}
}
