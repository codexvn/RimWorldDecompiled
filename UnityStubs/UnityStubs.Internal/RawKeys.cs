using UnityEngine;

namespace UnityStubs.Internal;

public static class RawKeys
{
	public static bool GetKey(KeyCode key)
	{
		return Input.GetKeyRaw(key);
	}

	public static bool GetKeyDown(KeyCode key)
	{
		return Input.GetKeyDownRaw(key);
	}

	public static bool GetKeyDownOrRepeat(KeyCode key)
	{
		if (!Input.GetKeyDownRaw(key))
		{
			return Input.GetKeyRepeatRaw(key);
		}
		return true;
	}

	public static bool GetKeyUp(KeyCode key)
	{
		return Input.GetKeyUpRaw(key);
	}
}
