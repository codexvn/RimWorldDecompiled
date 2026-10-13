using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.PlatformSpecific;

internal static class WaylandContentTypeSetter
{
	private readonly record struct WlDisplay(nint Handle)
	{
		public bool IsNull => Handle == 0;
	}

	private readonly record struct WlSurface(nint Handle)
	{
		public bool IsNull => Handle == 0;
	}

	private readonly record struct WlEventQueue(nint Handle)
	{
		public bool IsNull => Handle == 0;
	}

	private readonly record struct WlProxy(nint Handle)
	{
		public bool IsNull => Handle == 0;
	}

	[StructLayout(LayoutKind.Explicit, Size = 8)]
	private struct WlArgument
	{
		[FieldOffset(0)]
		public uint uintValue;

		[FieldOffset(0)]
		public unsafe byte* stringValue;

		[FieldOffset(0)]
		public unsafe void* objectValue;
	}

	private struct WlMessage
	{
		public unsafe byte* Name;

		public unsafe byte* Signature;

		public unsafe void** Types;
	}

	private struct WlInterface
	{
		public unsafe byte* Name;

		public int Version;

		public int MethodCount;

		public unsafe WlMessage* Methods;

		public int EventCount;

		public unsafe WlMessage* Events;
	}

	private struct RegistryListener
	{
		public unsafe delegate* unmanaged[Cdecl]<void*, void*, uint, byte*, uint, void> Global;

		public unsafe delegate* unmanaged[Cdecl]<void*, void*, uint, void> GlobalRemove;
	}

	private static readonly LogCategory Log = LogCategory.Make("WaylandContent");

	private const string DisableEnvironmentVariable = "WAYLITH_DISABLE_WAYLAND_GAME_CONTENT";

	private static WlDisplay _display;

	private static WlSurface _surface;

	private static WlEventQueue _queue;

	private static WlProxy _registry;

	private unsafe static RegistryListener* _listener;

	private unsafe static WlInterface* _regInterface;

	private unsafe static WlMessage* _regEvents;

	private unsafe static WlMessage* _regMethods;

	private unsafe static void** _regBindTypes;

	private static bool _initialized;

	private static bool _done;

	private static uint _managerName;

	private static bool _managerFound;

	private static int _pollFrames;

	private const string WaylandClientLibrary = "libwayland-client.so.0";

	public unsafe static void SetupRegistry(nint glfwWindowNativeHandle)
	{
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
		{
			_done = true;
			return;
		}
		if (IsDisabled())
		{
			_done = true;
			return;
		}
		try
		{
			WlDisplay display = GlfwGetWaylandDisplay();
			WlSurface surface = GlfwGetWaylandWindow(glfwWindowNativeHandle);
			if (display.IsNull || surface.IsNull)
			{
				_done = true;
				return;
			}
			WlEventQueue queue = DisplayCreateQueue(display);
			if (queue.IsNull)
			{
				_done = true;
				return;
			}
			WlProxy wlProxy;
			fixed (byte* name4 = (ReadOnlySpan<byte>)new byte[12]
			{
				119, 108, 95, 114, 101, 103, 105, 115, 116, 114,
				121, 0
			})
			{
				fixed (byte* name = (ReadOnlySpan<byte>)new byte[5] { 98, 105, 110, 100, 0 })
				{
					fixed (byte* signature = (ReadOnlySpan<byte>)new byte[5] { 117, 115, 117, 110, 0 })
					{
						fixed (byte* name2 = (ReadOnlySpan<byte>)new byte[7] { 103, 108, 111, 98, 97, 108, 0 })
						{
							fixed (byte* name3 = (ReadOnlySpan<byte>)new byte[14]
							{
								103, 108, 111, 98, 97, 108, 95, 114, 101, 109,
								111, 118, 101, 0
							})
							{
								fixed (byte* signature2 = (ReadOnlySpan<byte>)new byte[4] { 117, 115, 117, 0 })
								{
									fixed (byte* signature3 = (ReadOnlySpan<byte>)new byte[2] { 117, 0 })
									{
										_regBindTypes = (void**)NativeMemory.Alloc((nuint)4u * (nuint)sizeof(void*));
										*_regBindTypes = (_regBindTypes[1] = (_regBindTypes[2] = (_regBindTypes[3] = null)));
										_regMethods = (WlMessage*)NativeMemory.Alloc((nuint)sizeof(WlMessage));
										*_regMethods = new WlMessage
										{
											Name = name,
											Signature = signature,
											Types = _regBindTypes
										};
										_regEvents = (WlMessage*)NativeMemory.Alloc((nuint)2u * (nuint)sizeof(WlMessage));
										*_regEvents = new WlMessage
										{
											Name = name2,
											Signature = signature2,
											Types = null
										};
										_regEvents[1] = new WlMessage
										{
											Name = name3,
											Signature = signature3,
											Types = null
										};
										_regInterface = (WlInterface*)NativeMemory.Alloc((nuint)sizeof(WlInterface));
										*_regInterface = new WlInterface
										{
											Name = name4,
											Version = 1,
											MethodCount = 1,
											Methods = _regMethods,
											EventCount = 2,
											Events = _regEvents
										};
										WlArgument* ptr = stackalloc WlArgument[1];
										ptr->objectValue = null;
										wlProxy = ProxyMarshalArrayConstructorVersioned(new WlProxy(display.Handle), 1u, ptr, _regInterface, 1u);
									}
								}
							}
						}
					}
				}
			}
			if (wlProxy.IsNull)
			{
				FreeRegInterface();
				EventQueueDestroy(queue);
				_done = true;
				return;
			}
			ProxySetQueue(wlProxy, queue);
			_listener = (RegistryListener*)NativeMemory.Alloc((nuint)sizeof(RegistryListener));
			_listener->Global = (delegate* unmanaged[Cdecl]<void*, void*, uint, byte*, uint, void>)(delegate*<void*, void*, uint, byte*, uint, void>)(&OnGlobal);
			_listener->GlobalRemove = (delegate* unmanaged[Cdecl]<void*, void*, uint, void>)(delegate*<void*, void*, uint, void>)(&OnGlobalRemove);
			ProxyAddListener(wlProxy, _listener, null);
			DisplayFlush(display);
			_display = display;
			_surface = surface;
			_queue = queue;
			_registry = wlProxy;
			_managerFound = false;
			_managerName = 0u;
			_pollFrames = 0;
			_initialized = true;
			_done = false;
		}
		catch (Exception ex)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(33, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("WaylandGameContent setup failed: ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "SetupRegistry", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\WaylandContentTypeSetter.cs", 130);
			Cleanup();
		}
	}

	private static bool IsDisabled()
	{
		string environmentVariable = Environment.GetEnvironmentVariable("WAYLITH_DISABLE_WAYLAND_GAME_CONTENT");
		if (!string.Equals(environmentVariable, "1", StringComparison.Ordinal))
		{
			return string.Equals(environmentVariable, "true", StringComparison.OrdinalIgnoreCase);
		}
		return true;
	}

	public unsafe static bool TryApplyContentType()
	{
		if (_done || !_initialized)
		{
			return true;
		}
		try
		{
			DisplayDispatchQueuePending(_display, _queue);
			_pollFrames++;
			if (!_managerFound)
			{
				if (_pollFrames < 120)
				{
					return false;
				}
				Log.Warning("WaylandGameContent: wp_content_type_manager_v1 not found after 120 frames.", "TryApplyContentType", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\WaylandContentTypeSetter.cs", 159);
			}
			else
			{
				fixed (byte* ptr3 = (ReadOnlySpan<byte>)new byte[27]
				{
					119, 112, 95, 99, 111, 110, 116, 101, 110, 116,
					95, 116, 121, 112, 101, 95, 109, 97, 110, 97,
					103, 101, 114, 95, 118, 49, 0
				})
				{
					fixed (byte* name4 = (ReadOnlySpan<byte>)new byte[19]
					{
						119, 112, 95, 99, 111, 110, 116, 101, 110, 116,
						95, 116, 121, 112, 101, 95, 118, 49, 0
					})
					{
						fixed (byte* name = (ReadOnlySpan<byte>)new byte[8] { 100, 101, 115, 116, 114, 111, 121, 0 })
						{
							fixed (byte* name2 = (ReadOnlySpan<byte>)new byte[25]
							{
								103, 101, 116, 95, 115, 117, 114, 102, 97, 99,
								101, 95, 99, 111, 110, 116, 101, 110, 116, 95,
								116, 121, 112, 101, 0
							})
							{
								fixed (byte* name3 = (ReadOnlySpan<byte>)new byte[17]
								{
									115, 101, 116, 95, 99, 111, 110, 116, 101, 110,
									116, 95, 116, 121, 112, 101, 0
								})
								{
									fixed (byte* signature = (ReadOnlySpan<byte>)new byte[1] { 0 })
									{
										fixed (byte* signature2 = (ReadOnlySpan<byte>)new byte[3] { 110, 111, 0 })
										{
											fixed (byte* signature3 = (ReadOnlySpan<byte>)new byte[2] { 117, 0 })
											{
												void** ptr = stackalloc void*[2];
												*ptr = null;
												ptr[1] = null;
												WlMessage* ptr2 = stackalloc WlMessage[2];
												*ptr2 = new WlMessage
												{
													Name = name,
													Signature = signature,
													Types = null
												};
												ptr2[1] = new WlMessage
												{
													Name = name2,
													Signature = signature2,
													Types = ptr
												};
												WlInterface wlInterface = default(WlInterface);
												wlInterface.Name = ptr3;
												wlInterface.Version = 1;
												wlInterface.MethodCount = 2;
												wlInterface.Methods = ptr2;
												WlInterface wlInterface2 = wlInterface;
												WlArgument* ptr4 = stackalloc WlArgument[4];
												ptr4->uintValue = _managerName;
												ptr4[1].stringValue = ptr3;
												ptr4[2].uintValue = 1u;
												ptr4[3].objectValue = null;
												WlProxy proxy = ProxyMarshalArrayConstructorVersioned(_registry, 0u, ptr4, &wlInterface2, 1u);
												if (!proxy.IsNull)
												{
													try
													{
														WlArgument* ptr5 = stackalloc WlArgument[2];
														ptr5->objectValue = null;
														ptr5[1].objectValue = (void*)_surface.Handle;
														WlMessage* ptr6 = stackalloc WlMessage[2];
														*ptr6 = new WlMessage
														{
															Name = name,
															Signature = signature,
															Types = null
														};
														ptr6[1] = new WlMessage
														{
															Name = name3,
															Signature = signature3,
															Types = null
														};
														wlInterface = default(WlInterface);
														wlInterface.Name = name4;
														wlInterface.Version = 1;
														wlInterface.MethodCount = 2;
														wlInterface.Methods = ptr6;
														WlInterface wlInterface3 = wlInterface;
														WlProxy proxy2 = ProxyMarshalArrayConstructorVersioned(proxy, 1u, ptr5, &wlInterface3, 1u);
														if (!proxy2.IsNull)
														{
															WlArgument* ptr7 = stackalloc WlArgument[1];
															ptr7->uintValue = 3u;
															ProxyMarshalArray(proxy2, 1u, ptr7);
															ProxyDestroy(proxy2);
															DisplayFlush(_display);
															Log.Info("Wayland content type set to WP_CONTENT_TYPE_V1_TYPE_GAME.", "TryApplyContentType", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\WaylandContentTypeSetter.cs", 215);
														}
													}
													finally
													{
														ProxyDestroy(proxy);
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			LogCategory log = Log;
			bool shouldAppend;
			LogString<Warning> message = new LogString<Warning>(33, 1, log, out shouldAppend);
			if (shouldAppend)
			{
				message.AppendLiteral("WaylandGameContent apply failed: ");
				message.AppendFormatted(ex.Message);
			}
			log.Warning(message, "TryApplyContentType", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\PlatformSpecific\\WaylandContentTypeSetter.cs", 228);
		}
		Cleanup();
		return true;
	}

	public unsafe static void Cleanup()
	{
		if (!_registry.IsNull)
		{
			ProxyDestroy(_registry);
			_registry = default(WlProxy);
		}
		if (!_queue.IsNull)
		{
			EventQueueDestroy(_queue);
			_queue = default(WlEventQueue);
		}
		if (_listener != null)
		{
			NativeMemory.Free(_listener);
			_listener = null;
		}
		FreeRegInterface();
		_initialized = false;
		_done = true;
	}

	private unsafe static void FreeRegInterface()
	{
		if (_regBindTypes != null)
		{
			NativeMemory.Free(_regBindTypes);
			_regBindTypes = null;
		}
		if (_regMethods != null)
		{
			NativeMemory.Free(_regMethods);
			_regMethods = null;
		}
		if (_regEvents != null)
		{
			NativeMemory.Free(_regEvents);
			_regEvents = null;
		}
		if (_regInterface != null)
		{
			NativeMemory.Free(_regInterface);
			_regInterface = null;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private unsafe static void OnGlobal(void* data, void* registry, uint name, byte* @interface, uint version)
	{
		if (Marshal.PtrToStringAnsi((nint)@interface) == "wp_content_type_manager_v1")
		{
			_managerName = name;
			_managerFound = true;
		}
	}

	[UnmanagedCallersOnly(CallConvs = new Type[] { typeof(CallConvCdecl) })]
	private unsafe static void OnGlobalRemove(void* data, void* registry, uint name)
	{
	}

	[DllImport("glfw", EntryPoint = "glfwGetWaylandDisplay", ExactSpelling = true)]
	[LibraryImport("glfw", EntryPoint = "glfwGetWaylandDisplay")]
	private static extern WlDisplay GlfwGetWaylandDisplay();

	[DllImport("glfw", EntryPoint = "glfwGetWaylandWindow", ExactSpelling = true)]
	[LibraryImport("glfw", EntryPoint = "glfwGetWaylandWindow")]
	private static extern WlSurface GlfwGetWaylandWindow(nint glfwWindow);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_display_create_queue", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_display_create_queue")]
	private static extern WlEventQueue DisplayCreateQueue(WlDisplay display);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_event_queue_destroy", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_event_queue_destroy")]
	private static extern void EventQueueDestroy(WlEventQueue queue);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_proxy_set_queue", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_proxy_set_queue")]
	private static extern void ProxySetQueue(WlProxy proxy, WlEventQueue queue);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_proxy_add_listener", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_proxy_add_listener")]
	private unsafe static extern int ProxyAddListener(WlProxy proxy, void* listener, void* data);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_display_flush", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_display_flush")]
	private static extern int DisplayFlush(WlDisplay display);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_display_dispatch_queue_pending", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_display_dispatch_queue_pending")]
	private static extern int DisplayDispatchQueuePending(WlDisplay display, WlEventQueue queue);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_proxy_marshal_array_constructor_versioned", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_proxy_marshal_array_constructor_versioned")]
	private unsafe static extern WlProxy ProxyMarshalArrayConstructorVersioned(WlProxy proxy, uint opcode, WlArgument* args, WlInterface* iface, uint version);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_proxy_marshal_array", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_proxy_marshal_array")]
	private unsafe static extern void ProxyMarshalArray(WlProxy proxy, uint opcode, WlArgument* args);

	[DllImport("libwayland-client.so.0", EntryPoint = "wl_proxy_destroy", ExactSpelling = true)]
	[LibraryImport("libwayland-client.so.0", EntryPoint = "wl_proxy_destroy")]
	private static extern void ProxyDestroy(WlProxy proxy);
}
