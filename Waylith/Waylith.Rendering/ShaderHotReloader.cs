using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;
using Brutal.VulkanApi;
using Waylith.Graphics;
using Waylith.Rendering.Compute;
using Waylith.Rendering.PostFx;

namespace Waylith.Rendering;

public sealed class ShaderHotReloader : IDisposable
{
	private sealed record WatchedDirectory(string SourceDirectory, string? MirrorDirectory, FileSystemWatcher Watcher);

	private const long DebounceMs = 150L;

	private readonly FrameRenderer frameRenderer;

	private readonly GraphicsDevice graphicsDevice;

	private readonly List<WatchedDirectory> watchedDirectories = new List<WatchedDirectory>();

	private readonly List<Action> pendingRedefinitions = new List<Action>();

	private int dirty;

	private long pendingSinceTicks;

	private int reloadQueued;

	public static ShaderHotReloader? Current { get; set; }

	public bool WatchContentShaders { get; init; }

	public static event Action? ShadersReloaded;

	public ShaderHotReloader(FrameRenderer frameRenderer, GraphicsDevice graphicsDevice)
	{
		this.frameRenderer = frameRenderer;
		this.graphicsDevice = graphicsDevice;
	}

	public void Watch(string sourceDirectory, string? mirrorDirectory = null)
	{
		string fullPath = Path.GetFullPath(sourceDirectory);
		bool shouldAppend;
		LogCategory log;
		if (!Directory.Exists(fullPath))
		{
			log = RenderLog.Log;
			LogCategory logCategory = log;
			LogString<Warning> message = new LogString<Warning>(66, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("ShaderHotReloader: source directory '");
				message.AppendFormatted(fullPath);
				message.AppendLiteral("' not found; not watching it.");
			}
			logCategory.Warning(message, "Watch", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderHotReloader.cs", 55);
			return;
		}
		lock (watchedDirectories)
		{
			foreach (WatchedDirectory watchedDirectory in watchedDirectories)
			{
				if (string.Equals(watchedDirectory.SourceDirectory, fullPath, StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(fullPath, "*.glsl")
			{
				NotifyFilter = (NotifyFilters.FileName | NotifyFilters.Size | NotifyFilters.LastWrite),
				IncludeSubdirectories = true
			};
			fileSystemWatcher.Changed += OnFileChanged;
			fileSystemWatcher.Created += OnFileChanged;
			fileSystemWatcher.Renamed += OnFileChanged;
			fileSystemWatcher.EnableRaisingEvents = true;
			watchedDirectories.Add(new WatchedDirectory(fullPath, mirrorDirectory, fileSystemWatcher));
		}
		log = RenderLog.Log;
		LogCategory logCategory2 = log;
		LogString<Information> message2 = new LogString<Information>(50, 1, log, out shouldAppend);
		if (shouldAppend)
		{
			message2.AppendLiteral("ShaderHotReloader: watching '");
			message2.AppendFormatted(fullPath);
			message2.AppendLiteral("' for shader changes.");
		}
		logCategory2.Info(message2, "Watch", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderHotReloader.cs", 77);
	}

	private void OnFileChanged(object sender, FileSystemEventArgs e)
	{
		Volatile.Write(ref pendingSinceTicks, Environment.TickCount64);
		Volatile.Write(ref dirty, 1);
	}

	public void Tick()
	{
		if (Volatile.Read(in dirty) != 0 && Environment.TickCount64 - Volatile.Read(in pendingSinceTicks) >= 150)
		{
			Volatile.Write(ref dirty, 0);
			if (!TrySyncSourcesToMirrors())
			{
				Volatile.Write(ref dirty, 1);
			}
			else
			{
				RequestReload();
			}
		}
	}

	public void Redefine(Shader shader, ShaderProgramDefinition definition, IEnumerable<string>? keywords, PipelineState? pipelineState, string? iconShaderKey)
	{
		lock (pendingRedefinitions)
		{
			pendingRedefinitions.Add(delegate
			{
				shader.Redefine(definition, keywords, pipelineState, iconShaderKey);
			});
		}
	}

	public void RequestReload()
	{
		if (Interlocked.Exchange(ref reloadQueued, 1) == 0)
		{
			frameRenderer.AddNextFrameCallback(ReloadOnRenderThread);
		}
	}

	private bool TrySyncSourcesToMirrors()
	{
		try
		{
			lock (watchedDirectories)
			{
				foreach (WatchedDirectory watchedDirectory in watchedDirectories)
				{
					if (watchedDirectory.MirrorDirectory == null || string.Equals(watchedDirectory.SourceDirectory, Path.GetFullPath(watchedDirectory.MirrorDirectory), StringComparison.OrdinalIgnoreCase))
					{
						continue;
					}
					foreach (string item in Directory.EnumerateFiles(watchedDirectory.SourceDirectory, "*.glsl", SearchOption.AllDirectories))
					{
						string text = Path.Combine(watchedDirectory.MirrorDirectory, Path.GetRelativePath(watchedDirectory.SourceDirectory, item));
						Directory.CreateDirectory(Path.GetDirectoryName(text));
						File.Copy(item, text, overwrite: true);
					}
				}
			}
			return true;
		}
		catch (IOException)
		{
			return false;
		}
		catch (UnauthorizedAccessException)
		{
			return false;
		}
	}

	private void ReloadOnRenderThread()
	{
		Volatile.Write(ref reloadQueued, 0);
		bool shouldAppend;
		try
		{
			graphicsDevice.Device.WaitIdle();
			Action[] array;
			lock (pendingRedefinitions)
			{
				array = pendingRedefinitions.ToArray();
				pendingRedefinitions.Clear();
			}
			Action[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i]();
			}
			ShaderSourceLoader.ClearLayoutCaches();
			foreach (Shader allShader in ShaderRegistry.AllShaders)
			{
				if (!allShader.CanReloadFromSource)
				{
					continue;
				}
				try
				{
					allShader.ReloadLayouts();
				}
				catch (Exception ex)
				{
					LogCategory log = RenderLog.Log;
					LogCategory logCategory = log;
					LogString<Error> message = new LogString<Error>(52, 2, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("ShaderHotReloader: kept the previous layouts of '");
						message.AppendFormatted(allShader.Key);
						message.AppendLiteral("': ");
						message.AppendFormatted(ex.Message);
					}
					logCategory.Error(message, "ReloadOnRenderThread", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderHotReloader.cs", 183);
				}
				graphicsDevice.ShaderLayoutCache.Invalidate(allShader);
			}
			MeshBatchRenderer.ReloadAllShaders();
			ComputeKernelCache.ReloadAll();
			PostEffectPass.ReloadAll();
			ShaderHotReloader.ShadersReloaded?.Invoke();
			RenderLog.Log.Info("ShaderHotReloader: shaders reloaded.", "ReloadOnRenderThread", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderHotReloader.cs", 193);
		}
		catch (Exception value)
		{
			LogCategory log = RenderLog.Log;
			LogCategory logCategory2 = log;
			LogString<Error> message = new LogString<Error>(34, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("ShaderHotReloader: reload failed: ");
				message.AppendFormatted(value);
			}
			logCategory2.Error(message, "ReloadOnRenderThread", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderHotReloader.cs", 197);
		}
	}

	public void Dispose()
	{
		lock (watchedDirectories)
		{
			foreach (WatchedDirectory watchedDirectory in watchedDirectories)
			{
				watchedDirectory.Watcher.EnableRaisingEvents = false;
				watchedDirectory.Watcher.Changed -= OnFileChanged;
				watchedDirectory.Watcher.Created -= OnFileChanged;
				watchedDirectory.Watcher.Renamed -= OnFileChanged;
				watchedDirectory.Watcher.Dispose();
			}
			watchedDirectories.Clear();
		}
	}
}
