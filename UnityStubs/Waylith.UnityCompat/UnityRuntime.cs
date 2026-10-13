using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Waylith.Rendering;
using Waylith.Runtime;

namespace Waylith.UnityCompat;

public sealed class UnityRuntime
{
	public static UnityRuntime? Current { get; private set; }

	public UnityAppState App { get; } = new UnityAppState();

	public UnityScreenState Screen { get; } = new UnityScreenState();

	public UnityTimeState Time { get; } = new UnityTimeState();

	public UnityWorld World { get; } = new UnityWorld();

	public int FrameCount => Time.FrameCount;

	public WaylithRuntime Core { get; }

	public UnityRuntime(WaylithRuntime core)
	{
		Core = core;
		Current = this;
		UnityWorld.Current = World;
		World.ObjectDestroyed += Object.MarkDestroyed;
		World.MessageCoroutineStarter = delegate(object owner, IEnumerator routine)
		{
			CoroutineScheduler.Start(owner, routine);
		};
		AssetPrefetcher.AudioLoader = AudioClip.OpenForPrefetch;
		Screen.InitializeDefaults();
	}

	public void ConfigurePaths(string dataPath, string persistentDataPath, string temporaryCachePath, string? logDataPath = null, string? cacheDataPath = null)
	{
		Core.ConfigurePaths(dataPath, persistentDataPath, temporaryCachePath, logDataPath, cacheDataPath);
	}

	public void ConfigureScreen(int screenWidth, int screenHeight, bool fullscreen = false)
	{
		Screen.Width = screenWidth;
		Screen.Height = screenHeight;
		Screen.FullScreen = fullscreen;
	}

	public void ConfigureScreenWindow(int windowWidth, int windowHeight)
	{
		Screen.WindowWidth = windowWidth;
		Screen.WindowHeight = windowHeight;
	}

	public void Step(float deltaTime, float elapsedTime)
	{
		Time.BeginFrame(deltaTime, elapsedTime);
		SceneManager.ProcessPendingLoad();
		UnityFrame frame = new UnityFrame(Time.FrameCount, deltaTime, elapsedTime);
		World.Update(in frame);
		World.LateUpdate(in frame);
		Camera.DispatchPreCullForScreenCameras();
	}
}
