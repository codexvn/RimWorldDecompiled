using UnityEngine;

namespace UnityStubs.Internal;

public static class TextFocus
{
	internal static int ReadOnlyId;

	public static bool CapturesText
	{
		get
		{
			if (GUIUtility.keyboardControl != 0)
			{
				return GUIUtility.keyboardControl != ReadOnlyId;
			}
			return false;
		}
	}

	public static bool Consumes(KeyCode key)
	{
		int keyboardControl = GUIUtility.keyboardControl;
		if (keyboardControl == 0)
		{
			return false;
		}
		if (keyboardControl != ReadOnlyId)
		{
			return true;
		}
		return ReadOnlyKeeps(key, Input.GetKeyRaw(KeyCode.LeftControl) || Input.GetKeyRaw(KeyCode.RightControl));
	}

	internal static bool ReadOnlyKeeps(KeyCode key, bool ctrl)
	{
		bool flag = (((uint)(key - 273) <= 3u || (uint)(key - 278) <= 3u) ? true : false);
		bool flag2 = flag;
		if (!flag2)
		{
			bool flag3 = ctrl;
			if (flag3)
			{
				bool flag4 = ((key == KeyCode.A || key == KeyCode.C) ? true : false);
				flag3 = flag4;
			}
			flag2 = flag3;
		}
		return flag2;
	}
}
