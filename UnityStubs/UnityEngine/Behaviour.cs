using Waylith.UnityCompat;

namespace UnityEngine;

public class Behaviour : Component
{
	public bool enabled
	{
		get
		{
			return UnityRuntime.Current?.World.IsBehaviourEnabled(this) ?? true;
		}
		set
		{
			if (UnityRuntime.Current != null)
			{
				UnityRuntime.Current.World.SetBehaviourEnabled(this, value);
			}
		}
	}

	public bool isActiveAndEnabled => UnityRuntime.Current?.World.IsBehaviourActiveAndEnabled(this) ?? false;
}
