using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Threading;
using Brutal;
using Brutal.Collections;
using Brutal.GlfwApi;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.Numerics;
using Brutal.VulkanApi;
using Brutal.VulkanApi.Abstractions;
using Waylith.Audio;
using Waylith.CLI;
using Waylith.Diagnostics;
using Waylith.Graphics;
using Waylith.Graphics.Meshes;
using Waylith.Input;
using Waylith.Jobs;
using Waylith.Logging;
using Waylith.PlatformSpecific;
using Waylith.Profiling;
using Waylith.Rendering;
using Waylith.Rendering.Compute;
using Waylith.Rendering.Culling;
using Waylith.Runtime;

namespace Waylith;

public sealed class EngineApplication : IDisposable
{
	private const int MinReachableWindowExtent = 160;

	private readonly Stopwatch _stopwatch = Stopwatch.StartNew();

	private readonly NativeList<VkClearValue> _clearValues = new NativeList<VkClearValue>
	{
		new VkClearColorValue
		{
			Float32 = new float4(0f, 0f, 0f, 1f)
		},
		new VkClearDepthStencilValue
		{
			Depth = 0f,
			Stencil = 0
		}
	};

	private int _disposed;

	private bool _timerPeriodSet;

	private int2 _windowedPosition;

	private int2 _windowedSize;

	private bool _glfwInitialized;

	private bool _quitLifecycleInvoked;

	private bool _macNativeFullscreenActive;

	private bool _audioOutputSuspended;

	private bool _windowShown;

	private DisplayMode? _pendingDisplayMode;

	private int2? _pendingExclusiveResolution;

	private int2? _pendingBorderlessResolution;

	private bool _displayModeChangeRebuildRequested;

	private bool _displayModeToggleRequested;

	private bool _applyingDisplayMode;

	private string? _shutdownRequestedReason;

	private RenderThread? _renderThread;

	private MainThreadSynchronizationContext? _mainThreadSyncContext;

	private GlfwInputSource? _inputSource;

	public Action? OnUpdate;

	public Action? OnShutdown;

	private Action<Mesh>? _meshUploadHandler;

	private Action<Mesh>? _meshReleaseHandler;

	private float? _focusLostAt;

	private float? _noRenderSince;

	public static EngineApplication Instance { get; private set; }

	public EngineApplicationOptions Options { get; }

	public TimeSpan Elapsed => _stopwatch.Elapsed;

	public string? ShutdownRequestedReason => _shutdownRequestedReason;

	public WaylithRuntime Runtime { get; } = new WaylithRuntime();

	public GraphicsDevice? GraphicsDevice { get; private set; }

	public Instance VulkanInstance => GraphicsDevice.Instance;

	public PhysicalDevice PhysicalDevice => GraphicsDevice.PhysicalDevice;

	public DeviceEx Device => GraphicsDevice.Device;

	public Queue GraphicsQueue => GraphicsDevice.GraphicsQueue;

	public Queue PresentQueue => GraphicsDevice.PresentQueue;

	public bool Headless => GraphicsDevice == null;

	public GlfwWindow Window { get; private set; }

	public WindowRenderContext? RenderContext { get; private set; }

	public FrameRenderer FrameRenderer { get; } = new FrameRenderer();

	public int ExitCode { get; set; }

	private static bool IsWayland => Glfw.PInvoke.GetPlatform() == 393219;

	public event Action<EngineApplication>? PrePollEvents;

	public event Action<EngineApplication>? BeforeGraphicsShutdown;

	public event Action<EngineApplication>? FrameEnqueued;

	[DllImport("winmm.dll")]
	private static extern int timeBeginPeriod(uint uPeriod);

	[DllImport("winmm.dll")]
	private static extern int timeEndPeriod(uint uPeriod);

	[DllImport("libc")]
	private static extern int setenv(string name, string value, int overwrite);

	public EngineApplication(EngineApplicationOptions options)
	{
		Instance = this;
		Options = options;
		VerboseLog.Log(RuntimeInformation.FrameworkDescription);
		if (Options.Headless)
		{
			AudioBackend audioBackend = Options.AudioBackend;
			if (audioBackend != null)
			{
				audioBackend.Initialize(noAudio: true);
				AudioBackend.Current = audioBackend;
				VerboseLog.Log(audioBackend.Name + " audio initialized on the null output.");
			}
			EngineWindow.Width = Options.InitialWindowSize.X;
			EngineWindow.Height = Options.InitialWindowSize.Y;
			EngineWindow.WindowWidth = Options.InitialWindowSize.X;
			EngineWindow.WindowHeight = Options.InitialWindowSize.Y;
			Window = null;
			return;
		}
		if (Platform.IsMacOS)
		{
			setenv("MVK_CONFIG_USE_METAL_ARGUMENT_BUFFERS", "0", 1);
			if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("VK_DRIVER_FILES") ?? Environment.GetEnvironmentVariable("VK_ICD_FILENAMES")))
			{
				string path = Path.Combine(AppContext.BaseDirectory, "Resources", "vulkan", "icd.d");
				setenv("VK_DRIVER_FILES", Path.Combine(path, Options.UseKosmicKrisp ? "libkosmickrisp_icd.json" : "MoltenVK_icd.json"), 1);
			}
			AddToPathVariable("VK_ADD_LAYER_PATH", Path.Combine(AppContext.BaseDirectory, "Resources", "vulkan", "explicit_layer.d"));
			NativeLibrary.Load("vulkan.1", typeof(EngineApplication).Assembly, null);
		}
		if (Platform.IsLinux && !NativeLibrary.TryLoad("libvulkan.so", out var _))
		{
			string path2 = Path.Combine(AppContext.BaseDirectory, "libvulkan.so");
			if (!File.Exists(path2) && NativeLibrary.TryLoad("libvulkan.so.1", out var _))
			{
				string[] array = File.ReadAllLines("/proc/self/maps");
				foreach (string text in array)
				{
					int num = text.IndexOf('/', StringComparison.Ordinal);
					if (num > 0 && text.Substring(num).Contains("/libvulkan.so.1", StringComparison.Ordinal))
					{
						VerboseLog.Log("No libvulkan.so found on system. Creating symlink in working directory to " + text.Substring(num));
						File.CreateSymbolicLink(path2, text.Substring(num));
						string text2 = Path.Combine(AppContext.BaseDirectory, "runtimes/linux-x64/native/");
						if (Directory.Exists(text2))
						{
							File.CreateSymbolicLink(Path.Combine(text2, "libvulkan.so"), text.Substring(num));
						}
						break;
					}
				}
			}
		}
		try
		{
			Glfw.ErrorCallback += OnGlfwError;
			RenderLog.Log.IsCategoryEnabled = true;
			RenderLog.Log.AppendCallerInfo = false;
			if (Environment.OSVersion.Platform == PlatformID.Unix && (!string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ENABLE_VULKAN_RENDERDOC_CAPTURE")) || !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("FORCE_X11"))))
			{
				Glfw.SetInitHint(GlfwInitHint.Platform, 393220);
			}
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				if (Platform.IsLinux && IsGnomeDesktop())
				{
					setenv("__GL_THREADED_OPTIMIZATIONS", "0", 1);
				}
				else
				{
					Glfw.SetInitHint(GlfwInitHint.WaylandLibdecor, 229378);
				}
			}
			VerboseLog.Log("Initializing GLFW...");
			if (!Glfw.Init())
			{
				throw new InvalidOperationException("Failed to initialize GLFW.");
			}
			_glfwInitialized = true;
			VerboseLog.Log("GLFW initialized.");
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				if (timeBeginPeriod(1u) == 0)
				{
					_timerPeriodSet = true;
				}
				else
				{
					RenderLog.Log.Warning("timeBeginPeriod(1) failed, target framerate may not work.", ".ctor", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\EngineApplication.cs", 238);
				}
			}
			Vulkan.EnumerateInstanceExtensionProperties_Count(default(ReadOnlySpan<byte>));
			if (!Glfw.VulkanSupported())
			{
				throw new InvalidOperationException("GLFW reports that Vulkan is not supported.");
			}
			Glfw.WindowHint(GlfwWindowHint.ClientApi, 0);
			Glfw.WindowHint((GlfwWindowHint)155649, Options.WaylandAppId);
			Glfw.WindowHint(GlfwWindowHint.Visible, 0);
			Glfw.WindowHint(GlfwWindowHint.Resizable, (!EngineWindow.SizeLocked) ? 1 : 0);
			int2? initialWindowPosition = Options.InitialWindowPosition;
			if (initialWindowPosition.HasValue)
			{
				int2 valueOrDefault = initialWindowPosition.GetValueOrDefault();
				Glfw.WindowHint(GlfwWindowHint.PositionX, valueOrDefault.X);
				Glfw.WindowHint(GlfwWindowHint.PositionY, valueOrDefault.Y);
			}
			VerboseLog.Log($"Creating window ({Options.InitialWindowSize.X}x{Options.InitialWindowSize.Y})...");
			Window = Glfw.CreateWindow(new GlfwWindow.CreateInfo
			{
				Title = Options.Title,
				Size = Options.InitialWindowSize
			});
			Glfw.WindowHint(GlfwWindowHint.Resizable, 1);
			Glfw.WindowHint(GlfwWindowHint.PositionX, int.MinValue);
			Glfw.WindowHint(GlfwWindowHint.PositionY, int.MinValue);
			_windowedPosition = Window.Position;
			_windowedSize = Window.Size;
			EngineWindow.WindowPositionX = _windowedPosition.X;
			EngineWindow.WindowPositionY = _windowedPosition.Y;
			VerboseLog.Log("Window created.");
			VerboseLog.Log("Creating Vulkan instance...");
			Instance instance = Waylith.Graphics.GraphicsDevice.CreateInstance(Options.Title, "Waylith", Options.DisallowFullscreenExclusive);
			VerboseLog.Log("Vulkan instance created.");
			AudioBackend audioBackend2 = Options.AudioBackend;
			if (audioBackend2 != null)
			{
				VerboseLog.Log("Initializing " + audioBackend2.Name + " audio...");
				audioBackend2.Initialize(noAudio: false);
				AudioBackend.Current = audioBackend2;
			}
			VerboseLog.Log("Creating Vulkan surface and graphics device...");
			VkSurfaceKHR surface = CreateSurface(instance, Window);
			GraphicsDevice = Waylith.Graphics.GraphicsDevice.Create(instance, surface, Options.FramesInFlight, new GpuPreference(Options.PreferredGpuName, Options.PreferredGpuUuid), Options.EnableVideoDecode, Options.DisallowFullscreenExclusive);
			VerboseLog.Log("Graphics device created.");
			VerboseLog.Log("Creating window render context...");
			RenderContext = new WindowRenderContext(GraphicsDevice, Window, surface, Options.ColorFormat, Options.DepthFormat, Options.PreferredPresentMode, Options.SampleCount, Options.FramesInFlight);
			VerboseLog.Log("Window render context created.");
			ConfigureGpuServices();
			EngineWindow.Width = RenderContext.Extent.Width;
			EngineWindow.Height = RenderContext.Extent.Height;
			int2 size2 = Window.Size;
			EngineWindow.WindowWidth = size2.X;
			EngineWindow.WindowHeight = size2.Y;
			EngineWindow.AvailableResolutions = EnumerateMonitorResolutions();
			EngineWindow.VSyncModeChanged += delegate(VSyncMode mode)
			{
				RenderContext.SetVSyncMode(mode);
			};
			EngineWindow.DisplayModeChanged += SetDisplayMode;
			EngineWindow.ResolutionChanged += OnResolutionChanged;
			EngineWindow.WindowPositionChanged += OnWindowedPositionChanged;
			Window.OnFramebufferSize += delegate(GlfwWindow _, int2 size)
			{
				EngineWindow.Width = size.X;
				EngineWindow.Height = size.Y;
			};
			Window.OnSize += delegate(GlfwWindow _, int2 size)
			{
				EngineWindow.WindowWidth = size.X;
				EngineWindow.WindowHeight = size.Y;
			};
			Window.OnPosition += delegate(GlfwWindow window, int2 position)
			{
				if (!_applyingDisplayMode && EngineWindow.DisplayMode == DisplayMode.Windowed && !window.IsIconified)
				{
					_windowedPosition = position;
					EngineWindow.WindowPositionX = position.X;
					EngineWindow.WindowPositionY = position.Y;
				}
			};
			Window.OnContentScale += OnContentScale;
			OnContentScale(Window, Window.ContentScale);
			_inputSource = new GlfwInputSource(this);
			WaylandContentTypeSetter.SetupRegistry(Window.NativeHandle);
			static void OnContentScale(GlfwWindow window, float2 scale)
			{
				EngineWindow.Dpi = 96f * scale.Y;
			}
		}
		catch
		{
			Dispose();
			throw;
		}
		static void AddToPathVariable(string variable, string value)
		{
			string environmentVariable = Environment.GetEnvironmentVariable(variable);
			setenv(variable, string.IsNullOrEmpty(environmentVariable) ? value : $"{environmentVariable}{Path.PathSeparator}{value}", 1);
		}
	}

	private void ConfigureGpuServices()
	{
		ComputeKernelCache.Configure(GraphicsDevice.Device);
		ComputeDispatchQueue.Configure(GraphicsDevice);
		ComputeBufferResourceManager.Configure(RenderContext.FramesInFlight);
		GpuMemoryReport.Configure(GraphicsDevice);
		GpuTextureService.Configure(GraphicsDevice, RenderContext.FramesInFlight);
		RenderTexture.Configure(GraphicsDevice);
		TextureBlockEncoder.Configure(GraphicsDevice);
		_meshUploadHandler = GraphicsDevice.MeshUploads.Enqueue;
		_meshReleaseHandler = GraphicsDevice.MeshUploads.Release;
		Mesh.UploadRequested += _meshUploadHandler;
		Mesh.ReleaseRequested += _meshReleaseHandler;
		FrameRenderer.RenderThreadFrameStarting += GraphicsDevice.MeshUploads.BeginRenderFrame;
		FrameRenderer.RenderThreadFrameStarting += GpuTextureService.BeginRenderFrame;
		FrameRenderer.RenderThreadFrameStarting += ComputeBufferResourceManager.OnRenderFrameStarting;
	}

	private void DetachMeshUploadWiring()
	{
		if (_meshUploadHandler != null)
		{
			Mesh.UploadRequested -= _meshUploadHandler;
		}
		if (_meshReleaseHandler != null)
		{
			Mesh.ReleaseRequested -= _meshReleaseHandler;
		}
		_meshUploadHandler = null;
		_meshReleaseHandler = null;
	}

	private static void ShutdownGpuServices()
	{
		TextureBlockEncoder.Shutdown();
		GpuTextureService.Shutdown();
		OcclusionQuery.Shutdown();
		ComputeDispatchQueue.Shutdown();
		ComputeKernelCache.DisposeAll();
		TextureResourceManager.DisposeAllImmediate();
	}

	public static int Run(EngineApplicationOptions? options = null, Action<EngineApplication>? initialize = null)
	{
		if (options == null)
		{
			options = new EngineApplicationOptions();
		}
		EngineSwitches.Apply(options);
		if (Platform.IsLinux)
		{
			try
			{
				File.WriteAllText("/proc/self/coredump_filter", "0x3f\n");
			}
			catch
			{
			}
		}
		RestartManager.WaitForPrecedingInstanceExit(options.CommandLineAccessor);
		CrashDialog.Enabled = options.ShowCrashDialog && !options.Headless;
		if (options.CrashReportInstructions != null)
		{
			CrashDialog.ReportInstructions = options.CrashReportInstructions;
		}
		EngineApplication app = null;
		using (ProcessLifetime.Create(() => app))
		{
			int result;
			try
			{
				app = new EngineApplication(options);
				VerboseLog.Log("Engine application constructed; running initialize...");
				initialize?.Invoke(app);
				VerboseLog.Log("Initialize complete; entering run loop.");
				result = app.RunLoop();
				RestartManager.LaunchReplacementIfRequested(options.CommandLineAccessor);
			}
			catch (Exception ex)
			{
				string text = CrashReporter.TryWriteReport(ex, app, "EngineApplication.Run");
				try
				{
					app?.Dispose();
				}
				catch
				{
				}
				if (!string.IsNullOrWhiteSpace(text))
				{
					Console.Error.WriteLine("Crash report written to: " + text);
				}
				Console.Error.WriteLine(ex);
				CrashDialog.TryShow(ex, app?.Runtime.App.ProductName ?? options.Title, text);
				result = 1;
			}
			finally
			{
				try
				{
					app?.Dispose();
				}
				catch
				{
				}
			}
			return result;
		}
	}

	public int RunLoop()
	{
		_mainThreadSyncContext = new MainThreadSynchronizationContext(Environment.CurrentManagedThreadId);
		SynchronizationContext.SetSynchronizationContext(_mainThreadSyncContext);
		if (Options.Headless)
		{
			return RunHeadlessLoop();
		}
		float num = 0f;
		if (_renderThread == null)
		{
			_renderThread = new RenderThread(RenderContext, FrameRenderer);
		}
		VerboseLog.Log("Starting render thread...");
		_renderThread.Start();
		VerboseLog.Log("Render thread started; entering main loop.");
		ExceptionDispatchInfo exceptionDispatchInfo = null;
		if (!Options.HideWindow)
		{
			Window.Visible = true;
			_windowShown = true;
			ApplyPendingDisplayMode();
		}
		try
		{
			while (!Window.ShouldClose)
			{
				FrameProfiler.BeginFrame();
				bool flag = false;
				bool flag2 = false;
				try
				{
					using (FrameProfiler.Scope("DrainRenderThreadCallbacks"))
					{
						FrameRenderer.DrainRenderThreadCallbacksOnMainThread();
					}
					_renderThread.ThrowIfFaulted();
					float num2 = (float)_stopwatch.Elapsed.TotalSeconds;
					float deltaTime = num2 - num;
					num = num2;
					using (FrameProfiler.Scope("GpuMemory"))
					{
						GpuMemoryHistory.Update(num2);
						using (FrameProfiler.Scope("TextureResidency"))
						{
							TextureResidency.Update();
						}
						using (FrameProfiler.Scope("TextureBuildBatch"))
						{
							TextureBuildBatch.OnFrameStart();
						}
						GpuMemoryReport.CheckUsageCap(Options.VramWarnCapBytes, num2);
					}
					this.PrePollEvents?.Invoke(this);
					InputState.BeginFrame();
					using (FrameProfiler.Scope("PollEvents"))
					{
						Glfw.PollEvents();
					}
					WaylandContentTypeSetter.TryApplyContentType();
					if (_displayModeToggleRequested)
					{
						_displayModeToggleRequested = false;
						EngineWindow.SetDisplayMode((EngineWindow.DisplayMode == DisplayMode.Windowed) ? DisplayMode.Borderless : DisplayMode.Windowed);
					}
					bool flag3 = !(EngineWindow.IsFocused = Window.IsFocused) && !EngineWindow.RunInBackground && !Options.HideWindow;
					if (flag3 != _audioOutputSuspended)
					{
						_audioOutputSuspended = flag3;
						Options.AudioBackend?.SetOutputPaused(flag3);
					}
					if (flag3)
					{
						float valueOrDefault = _focusLostAt.GetValueOrDefault();
						if (!_focusLostAt.HasValue)
						{
							valueOrDefault = num2;
							_focusLostAt = valueOrDefault;
						}
						using (FrameProfiler.Scope("FocusLossIdle"))
						{
							Glfw.WaitEventsTimeout(0.1);
						}
						continue;
					}
					float? focusLostAt = _focusLostAt;
					bool shouldAppend;
					if (focusLostAt.HasValue)
					{
						float valueOrDefault2 = focusLostAt.GetValueOrDefault();
						_focusLostAt = null;
						LogCategory log = RenderLog.Log;
						LogCategory logCategory = log;
						LogString<Information> message = new LogString<Information>(28, 1, log, out shouldAppend);
						if (shouldAppend)
						{
							message.AppendLiteral("Focus regained after ");
							message.AppendFormatted(num2 - valueOrDefault2, 0, "F1");
							message.AppendLiteral(" s idle");
						}
						logCategory.Info(message, "RunLoop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\EngineApplication.cs", 593);
					}
					bool flag4 = !RenderContext.IsFramebufferMinimized() && (!Options.HideWindow || !IsWayland);
					if (!flag4)
					{
						float valueOrDefault = _noRenderSince.GetValueOrDefault();
						if (!_noRenderSince.HasValue)
						{
							valueOrDefault = num2;
							_noRenderSince = valueOrDefault;
						}
					}
					else
					{
						focusLostAt = _noRenderSince;
						if (focusLostAt.HasValue)
						{
							float valueOrDefault3 = focusLostAt.GetValueOrDefault();
							_noRenderSince = null;
							LogCategory log = RenderLog.Log;
							LogCategory logCategory2 = log;
							LogString<Information> message = new LogString<Information>(71, 2, log, out shouldAppend);
							if (shouldAppend)
							{
								message.AppendLiteral("Rendering resumed after ");
								message.AppendFormatted(num2 - valueOrDefault3, 0, "F1");
								message.AppendLiteral(" s minimized, ");
								message.AppendFormatted((double)TextureResourceManager.PendingDisposalBytes / 1048576.0, 0, "F1");
								message.AppendLiteral(" MiB of texture disposals pending");
							}
							logCategory2.Info(message, "RunLoop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\EngineApplication.cs", 604);
						}
					}
					if (flag4)
					{
						ApplyPendingScreenSettings();
						bool flag5 = _renderThread.ConsumeSurfaceRecreateRequest();
						bool displayModeChangeRebuildRequested = _displayModeChangeRebuildRequested;
						_displayModeChangeRebuildRequested = false;
						bool flag6 = _renderThread.ConsumeResizeRequest() || RenderContext.HasFramebufferSizeMismatch() || displayModeChangeRebuildRequested;
						if (flag5 || flag6)
						{
							using (FrameProfiler.Scope("RenderTargetRebuild"))
							{
								_renderThread.WaitUntilIdle();
								if (flag5)
								{
									RenderContext.RecreateSurfaceAndRebuild();
								}
								else
								{
									RenderContext.Rebuild();
								}
								FrameRenderer.NotifyRenderTargetRebuilt(RenderContext);
								_renderThread.MarkRenderTargetRebuilt();
							}
						}
					}
					BeginFrame(deltaTime, num2);
					flag2 = true;
					using (FrameProfiler.Scope("MainThreadSyncContext"))
					{
						_mainThreadSyncContext?.Pump();
					}
					using (FrameProfiler.Scope("GameUpdate"))
					{
						OnUpdate?.Invoke();
					}
					if (Window.ShouldClose || !string.IsNullOrEmpty(_shutdownRequestedReason))
					{
						continue;
					}
					if (!flag4)
					{
						RetireGpuResourcesWithoutRendering();
						ThrottleToTargetFrameRate(num2);
						continue;
					}
					FrameTiming timing = new FrameTiming(Time.FrameCount, deltaTime, num2);
					using (FrameProfiler.Scope("BeginMainThreadFrame"))
					{
						FrameRenderer.BeginMainThreadFrame(in timing);
					}
					RenderFrame frame;
					using (FrameProfiler.Scope("CaptureFrame"))
					{
						frame = FrameRenderer.CaptureFrame(in timing, _clearValues, requestRenderContextRebuild: false);
					}
					using (FrameProfiler.Scope("RenderEnqueue"))
					{
						_renderThread.Enqueue(frame);
					}
					this.FrameEnqueued?.Invoke(this);
					flag2 = false;
					EndFrame();
					if (FrameProfiler.IgnoreThrottle)
					{
						FrameProfiler.EndFrame();
						flag = true;
						ThrottleToTargetFrameRate(num2);
					}
					else
					{
						using (FrameProfiler.Scope("Throttle"))
						{
							ThrottleToTargetFrameRate(num2);
						}
					}
				}
				finally
				{
					if (flag2)
					{
						EndFrame();
					}
					if (!flag)
					{
						FrameProfiler.EndFrame();
					}
				}
			}
		}
		catch (Exception source)
		{
			RenderLog.Log.Error("!!!CRASH!!!", "RunLoop", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\EngineApplication.cs", 704);
			exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source);
		}
		finally
		{
			try
			{
				_renderThread.Stop();
			}
			catch (Exception source2) when (exceptionDispatchInfo == null)
			{
				exceptionDispatchInfo = ExceptionDispatchInfo.Capture(source2);
			}
			FrameRenderer.DrainRenderThreadCallbacksOnMainThread();
		}
		ExecuteQuitLifecycle("WindowClose");
		exceptionDispatchInfo?.Throw();
		return ExitCode;
	}

	private int RunHeadlessLoop()
	{
		float num = 0f;
		while (_shutdownRequestedReason == null)
		{
			float num2 = (float)_stopwatch.Elapsed.TotalSeconds;
			float deltaTime = num2 - num;
			num = num2;
			this.PrePollEvents?.Invoke(this);
			InputState.BeginFrame();
			BeginFrame(deltaTime, num2);
			try
			{
				_mainThreadSyncContext?.Pump();
				OnUpdate?.Invoke();
			}
			finally
			{
				EndFrame();
			}
		}
		ExecuteQuitLifecycle("HeadlessShutdown");
		return ExitCode;
	}

	public void DumpDiagnostics(TextWriter? writer = null, string context = "EngineApplication.DumpDiagnostics")
	{
		CrashReporter.WriteDiagnostics(writer ?? Console.Out, this, context);
	}

	public string? TryWriteDiagnosticsReport(string context = "EngineApplication.DumpDiagnostics")
	{
		return CrashReporter.TryWriteDiagnosticsReport(this, context);
	}

	private static void BeginFrame(float deltaTime, float elapsedTime)
	{
		Time.BeginFrame(deltaTime, elapsedTime);
		NativeAllocationRuntime.BeginFrame(Time.FrameCount);
		WaylithJobRuntime.BeginFrame(Time.FrameCount);
	}

	private static void EndFrame()
	{
		NativeAllocationRuntime.EndFrame(Time.FrameCount);
		Time.EndFrame();
	}

	public void WaitForRenderThreadIdle()
	{
		_renderThread?.WaitUntilIdle();
	}

	public void Quit(int exitCode = 0)
	{
		ExitCode = exitCode;
		RequestShutdown($"Quit({exitCode})");
	}

	public void RequestShutdown(string reason)
	{
		if (_shutdownRequestedReason == null)
		{
			_shutdownRequestedReason = reason;
		}
		try
		{
			if (Window != null)
			{
				Window.ShouldClose = true;
			}
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		if (Interlocked.Exchange(ref _disposed, 1) != 0)
		{
			return;
		}
		ExecuteQuitLifecycle("Dispose");
		try
		{
			FrameProfiler.Shutdown();
		}
		catch
		{
		}
		WaylandContentTypeSetter.Cleanup();
		try
		{
			_renderThread?.Dispose();
			_renderThread = null;
			GraphicsDevice?.Device.WaitIdle();
			FrameRenderer.Dispose();
			DetachMeshUploadWiring();
			this.BeforeGraphicsShutdown?.Invoke(this);
			ShutdownGpuServices();
		}
		catch
		{
		}
		try
		{
			Options.AudioBackend?.Shutdown();
			AudioBackend.Current = null;
		}
		catch
		{
		}
		try
		{
			WaylithJobRuntime.Shutdown();
		}
		catch
		{
		}
		try
		{
			NativeAllocationRuntime.Shutdown();
		}
		catch
		{
		}
		RenderContext?.Dispose();
		GraphicsDevice?.Dispose();
		_clearValues?.Dispose();
		if (_timerPeriodSet)
		{
			timeEndPeriod(1u);
		}
		if (_glfwInitialized)
		{
			Glfw.ErrorCallback -= OnGlfwError;
			Glfw.Terminate();
		}
		try
		{
			FileLog.Shutdown();
		}
		catch
		{
		}
	}

	private void ExecuteQuitLifecycle(string reason)
	{
		if (_quitLifecycleInvoked)
		{
			return;
		}
		_quitLifecycleInvoked = true;
		if (_shutdownRequestedReason == null)
		{
			_shutdownRequestedReason = reason;
		}
		try
		{
			OnShutdown?.Invoke();
		}
		catch (Exception value)
		{
			Console.Error.WriteLine($"Failed to run game shutdown: {value}");
		}
		try
		{
			Runtime.App.FireShuttingDown();
		}
		catch
		{
		}
	}

	internal GlfwMonitor CurrentMonitor()
	{
		GlfwWindow window = Window;
		if (window.IsNull())
		{
			return Glfw.PrimaryMonitor;
		}
		GlfwMonitor monitor = window.Monitor;
		if (!monitor.IsNull())
		{
			return monitor;
		}
		if (Glfw.PInvoke.GetPlatform() == 393219)
		{
			return Glfw.PrimaryMonitor;
		}
		int2 position = window.Position;
		int2 size = window.Size;
		int num = position.X + size.X / 2;
		int num2 = position.Y + size.Y / 2;
		ReadOnlySpan<GlfwMonitor> monitors = Glfw.GetMonitors();
		for (int i = 0; i < monitors.Length; i++)
		{
			GlfwMonitor result = monitors[i];
			int2 position2 = result.Position;
			GlfwVidMode videoMode = result.VideoMode;
			if (num >= position2.X && num2 >= position2.Y && num < position2.X + videoMode.Width && num2 < position2.Y + videoMode.Height)
			{
				return result;
			}
		}
		return Glfw.PrimaryMonitor;
	}

	private Resolution[] EnumerateMonitorResolutions()
	{
		GlfwMonitor glfwMonitor = CurrentMonitor();
		if (glfwMonitor.IsNull())
		{
			return Array.Empty<Resolution>();
		}
		float2 @float = ((Glfw.PInvoke.GetPlatform() == 393218) ? glfwMonitor.ContentScale : new float2(1f, 1f));
		GlfwVidMode videoMode = glfwMonitor.VideoMode;
		ReadOnlySpan<GlfwVidMode> videoModes = glfwMonitor.VideoModes;
		List<Resolution> list = new List<Resolution>(videoModes.Length);
		for (int i = 0; i < videoModes.Length; i++)
		{
			GlfwVidMode glfwVidMode = videoModes[i];
			if (EngineWindow.DisplayMode == DisplayMode.ExclusiveFullscreen || (glfwVidMode.Width <= videoMode.Width && glfwVidMode.Height <= videoMode.Height))
			{
				list.Add(new Resolution((int)System.Math.Round((float)glfwVidMode.Width * @float.X), (int)System.Math.Round((float)glfwVidMode.Height * @float.Y), glfwVidMode.RefreshRate));
			}
		}
		return list.ToArray();
	}

	public void SetDisplayMode(DisplayMode mode)
	{
		if (!_windowShown)
		{
			_pendingDisplayMode = mode;
			EngineWindow.DisplayMode = mode;
			EngineWindow.AvailableResolutions = EnumerateMonitorResolutions();
			return;
		}
		GlfwMonitor monitor = CurrentMonitor();
		GlfwVidMode videoMode = monitor.VideoMode;
		int2 position = monitor.Position;
		if (EngineWindow.DisplayMode == DisplayMode.Windowed)
		{
			_windowedPosition = Window.Position;
			_windowedSize = Window.Size;
		}
		_applyingDisplayMode = true;
		switch (mode)
		{
		case DisplayMode.Windowed:
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && _macNativeFullscreenActive)
			{
				if (MacNativeFullscreen.SetFullscreen(Window.NativeHandle, fullscreen: false))
				{
					_macNativeFullscreenActive = false;
					break;
				}
				Window.SetWindowed(_windowedPosition, _windowedSize);
				Window.SetAttribute(GlfwWindowAttribute.Decorated, 1);
			}
			else
			{
				Window.SetWindowed(_windowedPosition, _windowedSize);
				Window.SetAttribute(GlfwWindowAttribute.Decorated, 1);
			}
			break;
		case DisplayMode.Borderless:
			if (EngineWindow.DisplayMode == DisplayMode.ExclusiveFullscreen)
			{
				Window.SetWindowed(_windowedPosition, _windowedSize);
				monitor = CurrentMonitor();
				videoMode = monitor.VideoMode;
				position = monitor.Position;
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				if (EngineWindow.DisplayMode == DisplayMode.ExclusiveFullscreen)
				{
					Window.SetAttribute(GlfwWindowAttribute.Decorated, 1);
				}
				if (MacNativeFullscreen.SetFullscreen(Window.NativeHandle, fullscreen: true))
				{
					_macNativeFullscreenActive = true;
				}
				else
				{
					Window.SetFullscreen(monitor, new int2(videoMode.Width, videoMode.Height), -1);
				}
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Window.SetAttribute(GlfwWindowAttribute.Decorated, 0);
				Window.SetWindowed(position, new int2(videoMode.Width, videoMode.Height));
			}
			else
			{
				Window.SetFullscreen(monitor, new int2(videoMode.Width, videoMode.Height), -1);
			}
			break;
		case DisplayMode.ExclusiveFullscreen:
			if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) && _macNativeFullscreenActive)
			{
				MacNativeFullscreen.SetFullscreen(Window.NativeHandle, fullscreen: false);
				_macNativeFullscreenActive = false;
			}
			Window.SetFullscreen(monitor, new int2(videoMode.Width, videoMode.Height), -1);
			break;
		}
		_applyingDisplayMode = false;
		_displayModeChangeRebuildRequested = true;
		EngineWindow.DisplayMode = mode;
		EngineWindow.AvailableResolutions = EnumerateMonitorResolutions();
	}

	private static bool IsGnomeDesktop()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("XDG_CURRENT_DESKTOP");
		if (!string.IsNullOrEmpty(environmentVariable))
		{
			string[] array = environmentVariable.Split(':');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Trim().Equals("GNOME", StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return (Environment.GetEnvironmentVariable("DESKTOP_SESSION") ?? string.Empty).Contains("gnome", StringComparison.OrdinalIgnoreCase);
	}

	private void ApplyPendingDisplayMode()
	{
		DisplayMode? pendingDisplayMode = _pendingDisplayMode;
		if (pendingDisplayMode.HasValue)
		{
			DisplayMode valueOrDefault = pendingDisplayMode.GetValueOrDefault();
			_pendingDisplayMode = null;
			SetDisplayMode(valueOrDefault);
		}
		int2? pendingExclusiveResolution = _pendingExclusiveResolution;
		if (pendingExclusiveResolution.HasValue)
		{
			int2 valueOrDefault2 = pendingExclusiveResolution.GetValueOrDefault();
			_pendingExclusiveResolution = null;
			if (EngineWindow.DisplayMode == DisplayMode.ExclusiveFullscreen)
			{
				Window.SetFullscreen(CurrentMonitor(), valueOrDefault2, -1);
			}
		}
		pendingExclusiveResolution = _pendingBorderlessResolution;
		if (pendingExclusiveResolution.HasValue)
		{
			int2 valueOrDefault3 = pendingExclusiveResolution.GetValueOrDefault();
			_pendingBorderlessResolution = null;
			if (EngineWindow.DisplayMode == DisplayMode.Borderless)
			{
				Window.Size = ClampToDesktop(valueOrDefault3);
			}
		}
	}

	private void OnWindowedPositionChanged(int x, int y)
	{
		if (TryResolveWindowPosition(new int2(x, y), _windowedSize, out var resolved))
		{
			_windowedPosition = resolved;
			EngineWindow.WindowPositionX = resolved.X;
			EngineWindow.WindowPositionY = resolved.Y;
			if (EngineWindow.DisplayMode == DisplayMode.Windowed)
			{
				Window.Position = resolved;
			}
		}
	}

	private bool TryResolveWindowPosition(int2 position, int2 size, out int2 resolved)
	{
		resolved = position;
		long num = 0L;
		int2 @int = default(int2);
		int2 int2 = default(int2);
		ReadOnlySpan<GlfwMonitor> monitors = Glfw.GetMonitors();
		for (int i = 0; i < monitors.Length; i++)
		{
			monitors[i].GetMonitorWorkArea(out var pos, out var size2);
			if (size2.X <= 0 || size2.Y <= 0)
			{
				continue;
			}
			int num2 = System.Math.Min(position.X + size.X, pos.X + size2.X) - System.Math.Max(position.X, pos.X);
			int num3 = System.Math.Min(position.Y + size.Y, pos.Y + size2.Y) - System.Math.Max(position.Y, pos.Y);
			if (num2 > 0 && num3 > 0)
			{
				long num4 = (long)num2 * (long)num3;
				if (num4 > num)
				{
					num = num4;
					@int = pos;
					int2 = size2;
				}
			}
		}
		if (num == 0L)
		{
			return false;
		}
		Window.GetWindowFrameSize(out var _, out var top, out var _, out var _);
		int num5 = System.Math.Min(160, size.X);
		int num6 = System.Math.Min(160, size.Y);
		int num7 = @int.X + num5 - size.X;
		int val = @int.X + int2.X - num5;
		int num8 = @int.Y + top;
		int val2 = @int.Y + int2.Y - num6;
		resolved = new int2(System.Math.Clamp(position.X, num7, System.Math.Max(num7, val)), System.Math.Clamp(position.Y, num8, System.Math.Max(num8, val2)));
		return true;
	}

	private void OnResolutionChanged(int width, int height)
	{
		int2 @int = new int2(width, height);
		switch (EngineWindow.DisplayMode)
		{
		case DisplayMode.Windowed:
			_windowedSize = @int;
			Window.Size = @int;
			break;
		case DisplayMode.ExclusiveFullscreen:
			if (!_windowShown)
			{
				_pendingExclusiveResolution = @int;
			}
			else
			{
				Window.SetFullscreen(CurrentMonitor(), @int, -1);
			}
			break;
		case DisplayMode.Borderless:
			if (!_windowShown)
			{
				_pendingBorderlessResolution = @int;
			}
			else
			{
				Window.Size = ClampToDesktop(@int);
			}
			break;
		}
	}

	internal void RequestDisplayModeToggle()
	{
		_displayModeToggleRequested = true;
	}

	private int2 ClampToDesktop(int2 size)
	{
		GlfwMonitor glfwMonitor = CurrentMonitor();
		if (glfwMonitor.IsNull())
		{
			return size;
		}
		GlfwVidMode videoMode = glfwMonitor.VideoMode;
		return new int2(System.Math.Min(size.X, videoMode.Width), System.Math.Min(size.Y, videoMode.Height));
	}

	private void RetireGpuResourcesWithoutRendering()
	{
		_renderThread.WaitUntilIdle();
		GraphicsDevice.Device.WaitIdle();
		FrameRenderer.NotifyRenderThreadFrameStarting();
	}

	private void ThrottleToTargetFrameRate(float frameStartElapsed)
	{
		int targetFrameRate = EngineWindow.TargetFrameRate;
		if (targetFrameRate > 0)
		{
			double num = 1.0 / (double)targetFrameRate;
			double num2 = _stopwatch.Elapsed.TotalSeconds - (double)frameStartElapsed;
			double num3 = num - num2;
			if (num3 > 0.001)
			{
				Thread.Sleep((int)((num3 - 0.001) * 1000.0));
			}
			while (_stopwatch.Elapsed.TotalSeconds - (double)frameStartElapsed < num)
			{
			}
		}
	}

	private void ApplyPendingScreenSettings()
	{
		if (RenderContext.ConsumePresentModeChange())
		{
			_renderThread.WaitUntilIdle();
			RenderContext.Rebuild();
			FrameRenderer.NotifyRenderTargetRebuilt(RenderContext);
			_renderThread.MarkRenderTargetRebuilt();
		}
	}

	private static void OnGlfwError(GlfwErrorCode error, string description)
	{
		if (error != GlfwErrorCode.FeatureUnavailable && error != GlfwErrorCode.FormatUnavailable)
		{
			LogCategory log = Glfw.Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(15, 2, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("GLFW Error (");
				message.AppendFormatted(error);
				message.AppendLiteral("): ");
				message.AppendFormatted(description);
			}
			log.Error(message, "OnGlfwError", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\EngineApplication.cs", 1273);
		}
	}

	private static VkSurfaceKHR CreateSurface(Instance instance, GlfwWindow window)
	{
		Glfw.CreateWindowSurface(instance.VkHandle, window, out var surface);
		if (surface == 0L)
		{
			string description;
			GlfwErrorCode error = Glfw.GetError(out description);
			throw new InvalidOperationException($"Failed to create window surface: {error} {description}");
		}
		return new VkSurfaceKHR((nint)surface);
	}
}
