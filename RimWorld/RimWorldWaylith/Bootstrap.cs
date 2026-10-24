using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Brutal.Numerics;
using Brutal.VulkanApi;
using RimWorld;
using RimWorld.Planet;
using RimWorldWaylith.Imgui;
using RimWorldWaylith.Rendering;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;
using UnityStubs;
using UnityStubs.Gui;
using UnityStubs.Rendering;
using Verse;
using Verse.Steam;
using Waylith;
using Waylith.Assets;
using Waylith.Audio;
using Waylith.CLI;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Hardware;
using Waylith.Jobs;
using Waylith.Logging;
using Waylith.Profiling;
using Waylith.Rendering;
using Waylith.Rendering.Compute;
using Waylith.Rendering.PostFx;
using Waylith.UnityCompat;
using Waylith.Utils;
using Waylith.VideoDecode;

namespace RimWorldWaylith;

internal static class Bootstrap
{
	private const string CompanyName = "Ludeon Studios";

	internal const string ProductName = "RimWorld by Ludeon Studios";

	private const string MacBundleIdentifier = "ludeon.rimworld";

	private const int WorldTextFontIndex = 2;

	private static GameObject? _worldTextPrefabToken;

	private static AudioMixer? _audioMixer;

	internal static bool WatchShaders;

	private static MapRenderFeature? _mapFeature;

	private static CompositePass? _composite;

	private static WorldRenderFeature? _worldFeature;

	private static CameraTextureBaker? _cameraTextureBaker;

	private static ImGuiHost? _imguiHost;

	private static ImGuiRenderFeature? _imguiFeature;

	private static SceneColorTargets? _sceneTargets;

	private static GlfwInputBridge? _inputBridge;

	private static WindowsImeBridge? _imeBridge;

	private static UnityRuntime _unityRuntime;

	private static ImguiDispatch? _imguiDispatch;

	private static (int W, int H) _lastFramebufferSize;

	private static (int W, int H) _lastWindowSize;

	private static bool _lastFullScreen;

	private static bool _bootCompleteRaised;

	private static List<string>? _modAssetPackBakeRoots;

	private static int _framesSinceBootComplete;

	public static void Initialize(EngineApplication app)
	{
		VerboseLog.Log("Bootstrap: configuring runtime...");
		_unityRuntime = new UnityRuntime(app.Runtime);
		ConfigureRuntime(_unityRuntime, app.Options);
		ImportDefaults.SetOfficialRootsProvider(() => (UnityData.dataPath != null) ? new string[2]
		{
			Path.Combine(Application.dataPath, "Resources"),
			GenFilePaths.OfficialModsFolderPath
		} : null);
		TexturePixelData.Rgba32MipChainFilter = MipFilter.Kaiser;
		_unityRuntime.App.RegisterQuitHandler(delegate
		{
			app.Quit(_unityRuntime.App.ExitCode);
		});
		EngineApplication engineApplication = app;
		engineApplication.OnUpdate = (Action)Delegate.Combine(engineApplication.OnUpdate, (Action)delegate
		{
			if (_modAssetPackBakeRoots != null)
			{
				app.Quit(ModAssetPackBake.Run(_modAssetPackBakeRoots));
				_modAssetPackBakeRoots = null;
			}
			else
			{
				if (!app.Options.Headless)
				{
					SyncWindowSizeIfChanged(app);
				}
				EndOfFrame.Run();
				FrameStart.Run();
				_unityRuntime.BeginFrame(Waylith.Time.DeltaTime, Waylith.Time.Elapsed);
				if (_imguiHost != null)
				{
					_imguiDispatch?.DispatchInput(_imguiHost);
				}
				_unityRuntime.Update(Waylith.Time.DeltaTime, Waylith.Time.Elapsed);
				RaiseBootCompleteOnce();
				if (_composite != null)
				{
					_composite.Saturation = Current.ColorCorrectionCurves?.saturation ?? 1f;
				}
				AudioListener.SyncToBackend();
				AudioBackend.Current?.Update();
			}
		});
		ShaderWarmup.PrewarmEnabled = !app.Options.Headless;
		if (app.Options.Headless)
		{
			ShaderSourceLoader.RegisterShaderSourceDirectory(ResolveShaderSourceDirectory());
			app.PrePollEvents += OnHeadlessPrePollEvents;
		}
		else
		{
			ConfigureWindowIcon(app);
			VerboseLog.Log("Bootstrap: registering shaders...");
			ShaderSourceLoader.RegisterShaderSourceDirectory(ResolveShaderSourceDirectory());
			ConfigureTextureRuntime(app);
			ConfigureInput(app);
			WindowPlacement.Restore();
			VerboseLog.Log("Bootstrap: configuring frame renderer...");
			ConfigureFrameRenderer(app);
		}
		VerboseLog.Log("Bootstrap: registering execution orders and resources...");
		FrameProfiler.CurrentTickProvider = () => (Current.Game?.tickManager?.TicksGame).GetValueOrDefault();
		RegisterExecutionOrders(_unityRuntime.World);
		RegisterResources(app);
		if (ModAssetPackBake.Requested(out _modAssetPackBakeRoots))
		{
			VerboseLog.Log("Bootstrap: baking mod asset packs instead of booting the game");
			return;
		}
		DateTime utcNow = DateTime.UtcNow;
		VerboseLog.Log("Bootstrap: booting into Entry scene...");
		SpawnEntryScene();
		VerboseLog.Log($"Bootstrap: initialize finished. Entry scene boot took {(DateTime.UtcNow - utcNow).TotalMilliseconds:F0}ms");
	}

	private static void RaiseBootCompleteOnce()
	{
		if (_bootCompleteRaised)
		{
			RunAfterBootSwitches();
		}
		else if (PlayDataLoader.Loaded && !LongEventHandler.AnyEventNowOrWaiting)
		{
			_bootCompleteRaised = true;
			AssetBakeHooks.RaiseBootComplete();
		}
	}

	private static void RunAfterBootSwitches()
	{
		if (!LongEventHandler.AnyEventNowOrWaiting)
		{
			int num = _framesSinceBootComplete++;
			if (num == 120 && GenCommandLine.TryGetCommandLineArg("ScreenshotAfterBoot", out var value) && !string.IsNullOrWhiteSpace(value))
			{
				ScreenCapture.CaptureScreenshot(Path.GetFullPath(value));
			}
			if (num == 120 && GenCommandLine.CommandLineArgPassed("MemoryReportAfterBoot"))
			{
				ProcessMemoryReport.Log();
			}
			if (num == 180 && GenCommandLine.CommandLineArgPassed("QuitAfterBoot"))
			{
				Root.Shutdown();
			}
		}
	}

	private static void OnHeadlessPrePollEvents(EngineApplication app)
	{
		MeshDrawSubmission.ClearFrameSubmissions();
		InputSystem.BeginFrame();
	}

	private unsafe static void ConfigureWindowIcon(EngineApplication app)
	{
		if (typeof(Bootstrap).Assembly.GetManifestResourceStream("RimWorld.ico") is UnmanagedMemoryStream unmanagedMemoryStream)
		{
			WindowIconExtensions.SetIconFromIco(ico: new ReadOnlySpan<byte>(unmanagedMemoryStream.PositionPointer, (int)unmanagedMemoryStream.Length), window: app.Window);
		}
	}

	private static void ConfigureInput(EngineApplication app)
	{
		if (_inputBridge == null)
		{
			_inputBridge = new GlfwInputBridge(app);
		}
		if (OperatingSystem.IsWindows())
		{
			WindowsImeBridge.Current = _imeBridge ?? (_imeBridge = new WindowsImeBridge(app.Window));
		}
		app.PrePollEvents += OnPrePollEvents;
	}

	private static void OnPrePollEvents(EngineApplication app)
	{
		WindowPlacement.Track();
		app.GraphicsDevice.MeshUploads.BeginFrame(UnityEngine.Time.frameCount);
		MeshDrawSubmission.BeginFrame(app.GraphicsDevice, app.RenderContext);
		InputSystem.BeginFrame();
		if (OperatingSystem.IsWindows())
		{
			_imeBridge?.BeginFrame();
		}
	}

	private static void SyncWindowSizeIfChanged(EngineApplication app)
	{
		VkExtent2D extent = app.RenderContext.GetExtent();
		(int, int) tuple = (extent.Width, extent.Height);
		bool fullScreen = Screen.fullScreen;
		if (tuple.Item1 > 0 && tuple.Item2 > 0)
		{
			(int, int) lastFramebufferSize = _lastFramebufferSize;
			(int, int) tuple2 = tuple;
			if (lastFramebufferSize.Item1 != tuple2.Item1 || lastFramebufferSize.Item2 != tuple2.Item2 || _lastFullScreen != fullScreen)
			{
				_unityRuntime.ConfigureScreen(tuple.Item1, tuple.Item2, fullScreen);
				_lastFramebufferSize = tuple;
				_lastFullScreen = fullScreen;
			}
		}
		int2 size = app.Window.Size;
		if (size.X > 0 && size.Y > 0)
		{
			(int, int) tuple2 = _lastWindowSize;
			int x = size.X;
			int y = size.Y;
			if (tuple2.Item1 != x || tuple2.Item2 != y)
			{
				_unityRuntime.ConfigureScreenWindow(size.X, size.Y);
				_lastWindowSize = (W: size.X, H: size.Y);
			}
		}
	}

	private static void ConfigureFrameRenderer(EngineApplication app)
	{
		ComputeRenderFeature feature = new ComputeRenderFeature(app.GraphicsDevice.Device, app.RenderContext.FramesInFlight);
		app.FrameRenderer.AddFeature(feature);
		_sceneTargets = new SceneColorTargets(app.GraphicsDevice, app.RenderContext.ColorFormat, app.RenderContext.DepthFormat);
		_sceneTargets.EnsureSized(app.RenderContext.FramesInFlight, app.RenderContext.GetExtent().Width, app.RenderContext.GetExtent().Height);
		_worldFeature = new WorldRenderFeature(app.GraphicsDevice, app.RenderContext, app.FrameRenderer.RenderToMainThread, _sceneTargets);
		app.FrameRenderer.AddFeature(_worldFeature);
		_mapFeature = new MapRenderFeature(app.GraphicsDevice, app.RenderContext, app.FrameRenderer.RenderToMainThread, _sceneTargets);
		app.FrameRenderer.AddFeature(_mapFeature);
		_cameraTextureBaker = new CameraTextureBaker(app.GraphicsDevice, app.RenderContext);
		CameraTextureRenderer.Current = _cameraTextureBaker;
		app.FrameRenderer.AddFeature(new CameraCaptureFeature(app.GraphicsDevice, _cameraTextureBaker));
		app.BeforeGraphicsShutdown += delegate
		{
			CameraTextureRenderer.Current = null;
			_cameraTextureBaker?.Dispose();
		};
		CompositePass composite = (_composite = new CompositePass(ComputeDispatchQueue.BilinearClampSampler)
		{
			Source = _sceneTargets.Current
		});
		app.FrameRenderer.AddFeature(new PostFxFeature(_sceneTargets, composite, delegate
		{
			MapRenderFeature? mapFeature = _mapFeature;
			return (mapFeature != null && mapFeature.IsActive) || (_worldFeature?.IsActive ?? false);
		}));
		int snapshotRingLength = app.RenderContext.FramesInFlight + 2;
		_imguiHost = new ImGuiHost(app, snapshotRingLength);
		IGuiBackend backend = (GuiBackend.Current = new ImGuiGuiBackend(_imguiHost, _cameraTextureBaker));
		TextMeshProBackend.Current = _imguiHost.TmpFontBackend;
		_imguiDispatch = new ImguiDispatch((ImGuiGuiBackend)backend);
		_imguiFeature = new ImGuiRenderFeature(_imguiHost, delegate(ImGuiHost host)
		{
			_imguiDispatch.Draw(host);
		});
		app.FrameRenderer.AddFeature(_imguiFeature);
		ScreenCaptureFeature screenCapture = new ScreenCaptureFeature(app, _cameraTextureBaker);
		app.FrameRenderer.AddFeature(screenCapture);
		ScreenCaptureBackend.Current = screenCapture;
		EndOfFrame.DrivenByHost = true;
		app.FrameEnqueued += delegate
		{
			using (FrameProfiler.Scope("EndOfFrame"))
			{
				screenCapture.RunEndOfFrame();
			}
		};
		app.BeforeGraphicsShutdown += delegate
		{
			ScreenCaptureBackend.Current = null;
			screenCapture.Dispose();
		};
		app.BeforeGraphicsShutdown += delegate
		{
			_imguiFeature?.Dispose();
		};
		ConfigureLayerPassRouting();
		ConfigureShaderHotReload(app);
	}

	private static void ConfigureShaderHotReload(EngineApplication app)
	{
		ShaderHotReloader obj = new ShaderHotReloader(app.FrameRenderer, app.GraphicsDevice)
		{
			WatchContentShaders = WatchShaders
		};
		ShaderHotReloader.Current = obj;
		ShaderHotReloader reloader = obj;
		app.PrePollEvents += delegate
		{
			reloader.Tick();
		};
	}

	private static void ConfigureLayerPassRouting()
	{
		LayerPassRouter.RegisterLayerMask(UnityEngine.LayerMask.GetMask("World", "WorldText", "WorldSkybox"), RenderPassIds.WorldMain);
		LayerPassRouter.RegisterLayerMask(UnityEngine.LayerMask.GetMask("WaterDepth"), RenderPassIds.WaterDepth);
		LayerPassRouter.RegisterLayerMask(UnityEngine.LayerMask.GetMask("GravshipMask"), RenderPassIds.CaptureOnly);
		LayerPassRouter.SetDefault(RenderPassIds.MapMain);
	}

	private static string ResolveShaderSourceDirectory()
	{
		return Path.Combine(AppContext.BaseDirectory, "Rendering", "Shaders");
	}

	private static void ConfigureRuntime(UnityRuntime runtime, EngineApplicationOptions options)
	{
		runtime.App.ProductName = "RimWorld by Ludeon Studios";
		runtime.App.Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "0.0.0";
		runtime.App.ShuttingDown += OnAppShuttingDown;
		string text = ResolveCacheRootOverride();
		string directoryName = Path.GetDirectoryName(Environment.ProcessPath);
		string path = (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? Path.GetFullPath(Path.Combine(directoryName, "..", "..", "..")) : directoryName);
		runtime.App.IsEditor = false;
		runtime.ConfigurePaths(Path.Combine(path, "RimWorld_Data"), ResolvePersistentDataRoot(), Path.GetTempPath(), ResolveLogDataRoot(), text);
		DeleteOldUnityLogs();
		RuntimeShaderCompiler.ConfigureCacheDirectory(Path.Combine(text ?? ResolvePersistentDataRoot(), runtime.App.IsEditor ? "ShaderCache-editor" : "ShaderCache"));
		string text2 = Path.Combine(directoryName, "Assets.zip");
		AssetPack assetPack = AssetPackBoot.Open(runtime.App.DataPath, options.CommandLineAccessor, text2);
		if ((assetPack == null || assetPack.ContentHashes.Count == 0) && !options.CommandLineAccessor.HasSwitch("NoAssetPack") && !Directory.Exists(Path.Combine(runtime.App.DataPath, "Resources", "Textures")))
		{
			throw new InvalidOperationException("The game's asset pack (" + text2 + ") is missing or damaged, so the game cannot start. Verify the game files in Steam to restore it.");
		}
		AssetBakeHooks.BootComplete += delegate
		{
			List<string> list = new List<string>();
			list.Add(runtime.App.DataPath);
			list.AddRange(ModLister.AllInstalledMods.Select((ModMetaData mod) => mod.RootDir.FullName));
			GpuTextureService.BeginSweepCache(new global::_003C_003Ez__ReadOnlyList<string>(list));
		};
		WaylithJobRuntime.SetErrorHandler(delegate(string s)
		{
			Log.Error(s);
		});
		GraphicsDevice.SetErrorHandler(delegate(string s)
		{
			Log.Error(s);
		});
		GraphicsDevice.SetWarningHandler(delegate(string s)
		{
			Log.Warning(s);
		});
	}

	private static void OnAppShuttingDown()
	{
		if (OperatingSystem.IsWindows())
		{
			_imeBridge?.Dispose();
		}
		try
		{
			SteamManager.ShutdownSteam();
		}
		catch (Exception ex)
		{
			Log.Error("Error in ShutdownSteam(): " + ex);
		}
	}

	private static string? ResolveCacheRootOverride()
	{
		if (!GenCommandLine.TryGetCommandLineArg("savedatafolder", out var value) || string.IsNullOrWhiteSpace(value))
		{
			return null;
		}
		value = value.TrimEnd('\\', '/');
		return Path.GetFullPath((value.Length == 0) ? Path.DirectorySeparatorChar.ToString() : value);
	}

	private static void DeleteOldUnityLogs()
	{
		string text;
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			text = ResolvePersistentDataRoot();
		}
		else
		{
			InlineArray5<string> buffer = default(InlineArray5<string>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 0) = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 1) = "Library";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 2) = "Logs";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 3) = "Ludeon Studios";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 4) = "RimWorld by Ludeon Studios";
			text = Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray5<string>, string>(in buffer, 5));
		}
		string path = text;
		InlineArray2<string> buffer2 = default(InlineArray2<string>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<string>, string>(ref buffer2, 0) = "Player.log";
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray2<string>, string>(ref buffer2, 1) = "Player-prev.log";
		ReadOnlySpan<string> readOnlySpan = global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray2<string>, string>(in buffer2, 2);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			string path2 = readOnlySpan[i];
			string text2 = Path.Combine(path, path2);
			try
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception ex)
			{
				VerboseLog.Log("Bootstrap: could not delete the unity era log " + text2 + ": " + ex.Message);
			}
		}
	}

	internal static string ResolvePersistentDataRoot()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			return Path.Combine(folderPath, "Library", "Application Support", "ludeon.rimworld");
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			string text = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
			if (string.IsNullOrEmpty(text))
			{
				text = Path.Combine(folderPath, ".config");
			}
			return Path.Combine(text, "unity3d", "Ludeon Studios", "RimWorld by Ludeon Studios");
		}
		return Path.Combine(GetLocalAppDataLow(), "Ludeon Studios", "RimWorld by Ludeon Studios");
	}

	internal static string ResolveEarlyLogRoot()
	{
		return ResolveLogDataRoot();
	}

	internal static string ResolveLogDataRoot()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			InlineArray5<string> buffer = default(InlineArray5<string>);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 0) = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 1) = "Library";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 2) = "Application Support";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 3) = "Ludeon Studios";
			global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray5<string>, string>(ref buffer, 4) = "RimWorld by Ludeon Studios";
			return Path.Combine(global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray5<string>, string>(in buffer, 5));
		}
		return ResolvePersistentDataRoot();
	}

	private unsafe static string GetLocalAppDataLow()
	{
		Guid guid = new Guid("A520A1A4-1780-4FF6-BD18-167343C5AF16");
		if (SHGetKnownFolderPath(&guid, 0u, 0, out var path) != 0)
		{
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "LocalLow");
		}
		try
		{
			return Marshal.PtrToStringUni(path);
		}
		finally
		{
			Marshal.FreeCoTaskMem(path);
		}
	}

	[DllImport("shell32.dll")]
	private unsafe static extern int SHGetKnownFolderPath(Guid* folderId, uint flags, nint token, out nint path);

	private static void ConfigureTextureRuntime(EngineApplication app)
	{
		ConfigureTextureCompression();
		ComputeKernelCache.Configure(app.GraphicsDevice.Device);
		ComputeDispatchQueue.Configure(app.GraphicsDevice);
		ComputeBufferResourceManager.Configure(app.RenderContext.FramesInFlight);
		GpuMemoryReport.Configure(app.GraphicsDevice);
		Waylith.Rendering.RenderTexture.Configure(app.GraphicsDevice);
		TextureResidency.Policy = ((!GenCommandLine.CommandLineArgPassed("EagerTextureUpload")) ? TextureResidencyPolicy.OnDemand : TextureResidencyPolicy.OffloadUnderPressure);
		GpuTextureService.Configure(app.GraphicsDevice, app.RenderContext.FramesInFlight);
		TextureBlockEncoder.Configure(app.GraphicsDevice);
		UnityMeshUploadService.Configure(app.GraphicsDevice);
		VideoDecodeService.Configure(app.GraphicsDevice);
		HardwareProfile.BeginProbe(app.GraphicsDevice);
		app.BeforeGraphicsShutdown += delegate
		{
			VideoDecodeService.Shutdown();
			UnityMeshUploadService.Shutdown();
			GpuTextureService.Shutdown();
			TextureBlockEncoder.Shutdown();
			GpuTextureService.Shutdown();
			ComputeDispatchQueue.Shutdown();
			ComputeKernelCache.DisposeAll();
			TextureResourceManager.DisposeAllImmediate();
		};
	}

	private static void ConfigureTextureCompression()
	{
		BcEncodeTarget bcEncodeTarget = BcEncodeTarget.None;
		if (GenCommandLine.TryGetCommandLineArg("texFormat", out var value))
		{
			BcEncodeTarget bcEncodeTarget2;
			switch (value.Trim().ToLowerInvariant())
			{
			case "bc1":
			case "dxt1":
				bcEncodeTarget2 = BcEncodeTarget.Bc1;
				break;
			case "bc4":
				bcEncodeTarget2 = BcEncodeTarget.Bc4;
				break;
			case "bc5":
				bcEncodeTarget2 = BcEncodeTarget.Bc5;
				break;
			case "bc7":
				bcEncodeTarget2 = BcEncodeTarget.Bc7;
				break;
			default:
				bcEncodeTarget2 = BcEncodeTarget.None;
				break;
			}
			bcEncodeTarget = bcEncodeTarget2;
		}
		else
		{
			bcEncodeTarget = BcEncodeTarget.Bc7;
		}
		float num = 1f;
		if (GenCommandLine.TryGetCommandLineArg("texQuality", out var value2) && float.TryParse(value2, NumberStyles.Float, CultureInfo.InvariantCulture, out var result))
		{
			num = result;
		}
		TextureCompressionSettings.Configure(bcEncodeTarget, num);
		TextureCompressionSettings.AllowBc1 = false;
		TextureCompressionSettings.RawMaxSize = 16;
		TextureCompressionSettings.RawPaths = new string[1] { "/LUTs/" };
		TextureCompressionSettings.LoadTimeTarget = TextureCompressionSettings.SelectTarget;
		if (bcEncodeTarget != 0)
		{
			VerboseLog.Log($"[TextureCompression] Runtime block compression enabled: {bcEncodeTarget} (quality {num:0.##}).");
		}
	}

	private static void RegisterExecutionOrders(UnityWorld world)
	{
		world.RegisterExecutionOrder(typeof(CameraDriver), -103);
		world.RegisterExecutionOrder(typeof(WorldCameraDriver), -103);
		world.RegisterExecutionOrder(typeof(Root_Entry), -50);
		world.RegisterExecutionOrder(typeof(Root_Play), -20);
	}

	private static void RegisterResources(EngineApplication app)
	{
		UnityWorld world = _unityRuntime.World;
		if ((object)_worldTextPrefabToken == null)
		{
			_worldTextPrefabToken = GameObject.CreateAssetToken("WorldText");
		}
		world.RegisterResource("Sounds/AudioMixer", typeof(AudioMixer), GetAudioMixer());
		ConfigureWorldTextGameObject(_worldTextPrefabToken);
		world.RegisterPrefab("Prefabs/WorldText", typeof(GameObject), _worldTextPrefabToken, CreateWorldTextInstance);
	}

	private static object CreateWorldTextInstance()
	{
		GameObject gameObject = new GameObject("WorldText");
		ConfigureWorldTextGameObject(gameObject);
		gameObject.GetComponent<TextMeshPro>().ForceMeshUpdate();
		return gameObject;
	}

	private static void ConfigureWorldTextGameObject(GameObject go)
	{
		go.layer = UnityEngine.LayerMask.NameToLayer("WorldText");
		go.AddComponent<MeshFilter>();
		go.AddComponent<MeshRenderer>();
		go.AddComponent<TextMeshPro>().font = new TMP_FontAsset(2);
	}

	private static void SpawnEntryScene()
	{
		SceneManager.RegisterScene("Entry", BuildEntryScene);
		SceneManager.RegisterScene("Play", BuildPlayScene);
		SceneManager.SetInitialScene("Entry");
	}

	private static void BuildEntryScene()
	{
		Camera component = new GameObject("Camera", typeof(Camera), typeof(AudioListener)).GetComponent<Camera>();
		component.clearFlags = CameraClearFlags.Color;
		component.backgroundColor = new UnityEngine.Color(0f, 0f, 0f, 0f);
		component.allowHDR = false;
		GameObject gameObject = new GameObject("GameRoot");
		gameObject.AddComponent<Root_Entry>();
		gameObject.AddComponent<LatestVersionGetter>();
	}

	private static void BuildPlayScene()
	{
		GameObject gameObject = new GameObject("Camera", typeof(Camera), typeof(AudioListener));
		Camera component = gameObject.GetComponent<Camera>();
		component.orthographic = true;
		component.clearFlags = CameraClearFlags.Depth;
		component.allowHDR = false;
		component.backgroundColor = new UnityEngine.Color(0.1102941f, 0.10872806f, 0.1046172f, 1f / 51f);
		component.cullingMask = ~UnityEngine.LayerMask.GetMask("UI", "World", "WorldText", "WorldSkybox", "GravshipMask", "WaterDepth");
		gameObject.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
		gameObject.AddComponent<CameraDriver>();
		gameObject.AddComponent<ColorCorrectionCurves>();
		GameObject gameObject2 = new GameObject("Subcameras");
		gameObject2.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
		gameObject2.AddComponent<SubcameraDriver>();
		GameObject gameObject3 = new GameObject("GameRoot");
		gameObject3.AddComponent<LatestVersionGetter>();
		gameObject3.AddComponent<Root_Play>();
	}

	private static AudioMixer GetAudioMixer()
	{
		if (_audioMixer != null)
		{
			return _audioMixer;
		}
		_audioMixer = new AudioMixer
		{
			name = "AudioMixer"
		};
		RegisterAudioMixerGroup(_audioMixer, "MainOutput");
		RegisterAudioMixerGroup(_audioMixer, "World_RS");
		RegisterAudioMixerGroup(_audioMixer, "DuckLow");
		RegisterAudioMixerGroup(_audioMixer, "DuckMed");
		RegisterAudioMixerGroup(_audioMixer, "DuckHigh");
		RegisterAudioMixerGroup(_audioMixer, "DuckMax");
		RegisterAudioMixerGroup(_audioMixer, "Main_RS_Close");
		RegisterAudioMixerGroup(_audioMixer, "Main_RS_Medium");
		RegisterAudioMixerGroup(_audioMixer, "Main_RS_Far");
		RegisterAudioMixerGroup(_audioMixer, "DuckLow_RS_Close");
		RegisterAudioMixerGroup(_audioMixer, "DuckLow_RS_Medium");
		RegisterAudioMixerGroup(_audioMixer, "DuckLow_RS_Far");
		RegisterAudioMixerGroup(_audioMixer, "DuckMed_RS_Close");
		RegisterAudioMixerGroup(_audioMixer, "DuckMed_RS_Medium");
		RegisterAudioMixerGroup(_audioMixer, "DuckMed_RS_Far");
		RegisterAudioMixerGroup(_audioMixer, "DuckHigh_RS_Close");
		RegisterAudioMixerGroup(_audioMixer, "DuckHigh_RS_Medium");
		RegisterAudioMixerGroup(_audioMixer, "DuckHigh_RS_Far");
		RegisterAudioMixerGroup(_audioMixer, "DuckMax_RS_Close");
		RegisterAudioMixerGroup(_audioMixer, "DuckMax_RS_Medium");
		RegisterAudioMixerGroup(_audioMixer, "DuckMax_RS_Far");
		_audioMixer.SetFloat("WorldReverbSend", 0f);
		return _audioMixer;
	}

	private static void RegisterAudioMixerGroup(AudioMixer mixer, string path)
	{
		mixer.AddGroup(path, AudioBackend.Current.CreateGroup(path, null));
	}
}
