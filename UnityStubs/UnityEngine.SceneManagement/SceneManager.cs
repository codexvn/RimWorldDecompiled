using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityStubs.Internal;
using Waylith.UnityCompat;

namespace UnityEngine.SceneManagement;

public class SceneManager
{
	private static readonly Dictionary<string, Action> SceneBuilders = new Dictionary<string, Action>(StringComparer.Ordinal);

	private static string activeSceneName = string.Empty;

	private static string pendingSceneName;

	private static readonly List<AsyncOperation> pendingOperations = new List<AsyncOperation>();

	public static int sceneCount
	{
		get
		{
			StubWarner.LogStub("sceneCount", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SceneManager.cs", 14);
			return 0;
		}
	}

	public static event UnityAction<Scene, LoadSceneMode> sceneLoaded;

	public static event UnityAction<Scene> sceneUnloaded;

	public static event UnityAction<Scene, Scene> activeSceneChanged;

	public static Scene GetSceneAt(int index)
	{
		StubWarner.LogStub("GetSceneAt", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SceneManager.cs", 16);
		return default(Scene);
	}

	public static Scene GetActiveScene()
	{
		return new Scene(activeSceneName);
	}

	public static void RegisterScene(string sceneName, Action builder)
	{
		SceneBuilders[sceneName] = builder;
	}

	public static void SetInitialScene(string sceneName)
	{
		activeSceneName = sceneName;
		UnityWorld unityWorld = UnityRuntime.Current?.World;
		if (unityWorld != null)
		{
			unityWorld.ActiveSceneName = sceneName;
		}
		BuildScene(sceneName);
		SceneManager.sceneLoaded?.Invoke(new Scene(sceneName), LoadSceneMode.Single);
	}

	public static void LoadScene(string sceneName)
	{
		pendingSceneName = sceneName;
	}

	internal static void ProcessPendingLoad()
	{
		if (pendingSceneName == null)
		{
			return;
		}
		string text = pendingSceneName;
		pendingSceneName = null;
		UnityWorld unityWorld = UnityRuntime.Current?.World;
		unityWorld?.UnloadScene(activeSceneName);
		Scene arg = new Scene(activeSceneName);
		SceneManager.sceneUnloaded?.Invoke(arg);
		activeSceneName = text;
		if (unityWorld != null)
		{
			unityWorld.ActiveSceneName = text;
		}
		BuildScene(text);
		Scene scene = new Scene(text);
		SceneManager.sceneLoaded?.Invoke(scene, LoadSceneMode.Single);
		SceneManager.activeSceneChanged?.Invoke(arg, scene);
		foreach (AsyncOperation pendingOperation in pendingOperations)
		{
			pendingOperation.MarkDone();
		}
		pendingOperations.Clear();
	}

	public static void LoadScene(int sceneBuildIndex)
	{
		StubWarner.LogStub("LoadScene", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SceneManager.cs", 77);
	}

	public static AsyncOperation LoadSceneAsync(string sceneName)
	{
		LoadScene(sceneName);
		AsyncOperation asyncOperation = new AsyncOperation();
		pendingOperations.Add(asyncOperation);
		return asyncOperation;
	}

	public static AsyncOperation LoadSceneAsync(int sceneBuildIndex)
	{
		StubWarner.LogStub("LoadSceneAsync", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\SceneManager.cs", 90);
		return AsyncOperation.CreateCompleted();
	}

	private static void BuildScene(string sceneName)
	{
		if (SceneBuilders.TryGetValue(sceneName, out var value))
		{
			value();
		}
		else
		{
			Console.Error.WriteLine("[SceneManager] No scene builder registered for scene '" + sceneName + "'.");
		}
	}
}
