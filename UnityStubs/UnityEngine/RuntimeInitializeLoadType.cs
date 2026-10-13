namespace UnityEngine;

public enum RuntimeInitializeLoadType
{
	AfterAssembliesLoaded = 2,
	AfterSceneLoad = 0,
	BeforeSceneLoad = 1,
	BeforeSplashScreen = 3,
	SubsystemRegistration = 4
}
