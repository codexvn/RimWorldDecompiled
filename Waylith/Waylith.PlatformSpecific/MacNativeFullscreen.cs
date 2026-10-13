using System;
using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.PlatformSpecific;

internal static class MacNativeFullscreen
{
	private static readonly LogCategory Log = LogCategory.Make("MacFullscreen");

	private const uint NSWindowStyleMaskFullScreen = 16384u;

	private const uint NSWindowCollectionBehaviorFullScreenPrimary = 128u;

	private static readonly nint StyleMaskSelector = SelRegisterName("styleMask");

	private static readonly nint CollectionBehaviorSelector = SelRegisterName("collectionBehavior");

	private static readonly nint SetCollectionBehaviorSelector = SelRegisterName("setCollectionBehavior:");

	private static readonly nint ToggleFullScreenSelector = SelRegisterName("toggleFullScreen:");

	public static bool IsFullscreen(nint glfwWindow)
	{
		nint cocoaWindow = GetCocoaWindow(glfwWindow);
		if (cocoaWindow == 0)
		{
			return false;
		}
		return (MsgSendUIntPtr(cocoaWindow, StyleMaskSelector) & 0x4000) != 0;
	}

	public static bool SetFullscreen(nint glfwWindow, bool fullscreen)
	{
		nint cocoaWindow = GetCocoaWindow(glfwWindow);
		if (cocoaWindow == 0)
		{
			return false;
		}
		if ((MsgSendUIntPtr(cocoaWindow, StyleMaskSelector) & 0x4000) != 0 == fullscreen)
		{
			return true;
		}
		nuint num = MsgSendUIntPtr(cocoaWindow, CollectionBehaviorSelector);
		if ((num & 0x80) == 0)
		{
			MsgSendVoidUIntPtr(cocoaWindow, SetCollectionBehaviorSelector, num | 0x80);
		}
		MsgSendVoidIntPtr(cocoaWindow, ToggleFullScreenSelector, 0);
		return true;
	}

	private static nint GetCocoaWindow(nint glfwWindow)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || glfwWindow == 0)
		{
			return 0;
		}
		try
		{
			return GlfwGetCocoaWindow(glfwWindow);
		}
		catch (Exception ex) when (((ex is DllNotFoundException || ex is EntryPointNotFoundException) ? 1 : 0) != 0)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Error> message = new LogString<Error>(47, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("Unable to access the GLFW Cocoa window handle: ");
				message.AppendFormatted(ex.Message);
			}
			log.Error(message, "GetCocoaWindow", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\MacNativeFullscreen.cs", 62);
			return 0;
		}
	}

	[DllImport("glfw", EntryPoint = "glfwGetCocoaWindow", ExactSpelling = true)]
	[LibraryImport("glfw", EntryPoint = "glfwGetCocoaWindow")]
	private static extern nint GlfwGetCocoaWindow(nint window);

	[LibraryImport("libobjc.A.dylib", EntryPoint = "sel_registerName", StringMarshalling = StringMarshalling.Utf8)]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	private unsafe static nint SelRegisterName(string name)
	{
		nint num = 0;
		Utf8StringMarshaller.ManagedToUnmanagedIn managedToUnmanagedIn = default(Utf8StringMarshaller.ManagedToUnmanagedIn);
		try
		{
			Span<byte> buffer = stackalloc byte[Utf8StringMarshaller.ManagedToUnmanagedIn.BufferSize];
			managedToUnmanagedIn.FromManaged(name, buffer);
			return __PInvoke(managedToUnmanagedIn.ToUnmanaged());
		}
		finally
		{
			managedToUnmanagedIn.Free();
		}
		[DllImport("libobjc.A.dylib", EntryPoint = "sel_registerName", ExactSpelling = true)]
		static extern unsafe nint __PInvoke(byte* __name_native);
	}

	[DllImport("libobjc.A.dylib", EntryPoint = "objc_msgSend", ExactSpelling = true)]
	[LibraryImport("libobjc.A.dylib", EntryPoint = "objc_msgSend")]
	private static extern nuint MsgSendUIntPtr(nint receiver, nint selector);

	[DllImport("libobjc.A.dylib", EntryPoint = "objc_msgSend", ExactSpelling = true)]
	[LibraryImport("libobjc.A.dylib", EntryPoint = "objc_msgSend")]
	private static extern void MsgSendVoidUIntPtr(nint receiver, nint selector, nuint value);

	[DllImport("libobjc.A.dylib", EntryPoint = "objc_msgSend", ExactSpelling = true)]
	[LibraryImport("libobjc.A.dylib", EntryPoint = "objc_msgSend")]
	private static extern void MsgSendVoidIntPtr(nint receiver, nint selector, nint value);
}
