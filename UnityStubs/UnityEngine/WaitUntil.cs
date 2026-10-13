using System;

namespace UnityEngine;

public class WaitUntil : CustomYieldInstruction
{
	private readonly Func<bool> _predicate;

	public override bool keepWaiting => !_predicate();

	public WaitUntil(Func<bool> predicate)
	{
		_predicate = predicate;
	}
}
