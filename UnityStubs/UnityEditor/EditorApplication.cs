using System;
using Waylith.UnityCompat;

namespace UnityEditor;

public class EditorApplication
{
	public static bool isPlaying
	{
		get
		{
			return UnityRuntime.Current?.App.IsPlaying ?? true;
		}
		set
		{
			if (UnityRuntime.Current != null)
			{
				UnityRuntime.Current.App.IsPlaying = value;
				if (!value)
				{
					EditorApplication.quitting?.Invoke();
					UnityRuntime.Current.App.RequestQuit("EditorApplication.isPlaying=false");
				}
			}
		}
	}

	public static event Action quitting;

	public static event Action<PlayModeStateChange> playModeStateChanged;
}
