using System;
using System.Runtime.InteropServices;
using System.Runtime.Loader;
using Brutal;

namespace Waylith.Diagnostics;

internal sealed class ProcessLifetime(Func<EngineApplication?> appAccessor) : IDisposable
{
	private readonly Func<EngineApplication?> _appAccessor = appAccessor;

	private readonly PosixSignalRegistration? _sigInt = RegisterPosixSignal(PosixSignal.SIGINT, appAccessor);

	private readonly PosixSignalRegistration? _sigTerm = RegisterPosixSignal(PosixSignal.SIGTERM, appAccessor);

	private readonly PosixSignalRegistration? _sigHup = RegisterPosixSignal(PosixSignal.SIGHUP, appAccessor);

	private bool _disposed;

	public ProcessLifetime()
		: this(() => (EngineApplication?)null)
	{
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			Console.CancelKeyPress -= OnCancelKeyPress;
			AppDomain.CurrentDomain.ProcessExit -= OnProcessExit;
			AppDomain.CurrentDomain.UnhandledException -= OnUnhandledException;
			AssemblyLoadContext.Default.Unloading -= OnUnloading;
			_sigInt?.Dispose();
			_sigTerm?.Dispose();
			_sigHup?.Dispose();
		}
	}

	public static ProcessLifetime Create(Func<EngineApplication?> appAccessor)
	{
		ProcessLifetime processLifetime = new ProcessLifetime(appAccessor);
		processLifetime.Hook();
		return processLifetime;
	}

	private void Hook()
	{
		Console.CancelKeyPress += OnCancelKeyPress;
		AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
		AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
		AssemblyLoadContext.Default.Unloading += OnUnloading;
	}

	private void OnCancelKeyPress(object? sender, ConsoleCancelEventArgs e)
	{
		e.Cancel = appAccessor() != null;
		TryRequestShutdown("Console.CancelKeyPress");
	}

	private void OnProcessExit(object? sender, EventArgs e)
	{
		TryDispose("ProcessExit");
	}

	private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
	{
		Exception ex = e.ExceptionObject as Exception;
		if (ex == null && e.ExceptionObject != null)
		{
			ex = new InvalidOperationException($"Unhandled non-Exception object: {e.ExceptionObject}");
		}
		EngineApplication engineApplication = appAccessor();
		string reportPath = CrashReporter.TryWriteReport(ex, engineApplication, "AppDomain.UnhandledException");
		TryDispose("UnhandledException");
		CrashDialog.TryShow(ex, engineApplication?.Runtime.App.ProductName ?? "Waylith", reportPath);
	}

	private void OnUnloading(AssemblyLoadContext context)
	{
		TryDispose("AssemblyLoadContext.Unloading");
	}

	private void TryRequestShutdown(string reason)
	{
		try
		{
			appAccessor()?.RequestShutdown(reason);
		}
		catch
		{
			TryDispose(reason);
		}
	}

	private void TryDispose(string reason)
	{
		try
		{
			EngineApplication engineApplication = appAccessor();
			if (engineApplication != null)
			{
				if (!string.IsNullOrWhiteSpace(reason))
				{
					engineApplication.RequestShutdown(reason);
				}
				engineApplication.Dispose();
			}
		}
		catch
		{
		}
	}

	private static PosixSignalRegistration? RegisterPosixSignal(PosixSignal signal, Func<EngineApplication?> appAccessor)
	{
		if (!Platform.IsLinux && !Platform.IsMacOS)
		{
			return null;
		}
		return PosixSignalRegistration.Create(signal, delegate(PosixSignalContext context)
		{
			EngineApplication engineApplication = appAccessor();
			context.Cancel = engineApplication != null;
			if (engineApplication == null)
			{
				return;
			}
			try
			{
				engineApplication.RequestShutdown($"PosixSignal:{signal}");
			}
			catch
			{
				try
				{
					engineApplication.Dispose();
				}
				catch
				{
				}
			}
		});
	}
}
