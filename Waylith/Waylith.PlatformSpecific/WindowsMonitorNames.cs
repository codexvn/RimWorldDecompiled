using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Brutal.GlfwApi;
using Waylith.Logging;

namespace Waylith.PlatformSpecific;

[SupportedOSPlatform("windows")]
internal static class WindowsMonitorNames
{
	private struct LUID
	{
		public uint LowPart;

		public int HighPart;
	}

	[StructLayout(LayoutKind.Explicit, Size = 72)]
	private struct DISPLAYCONFIG_PATH_INFO
	{
		[FieldOffset(0)]
		public LUID sourceAdapterId;

		[FieldOffset(8)]
		public uint sourceId;

		[FieldOffset(20)]
		public LUID targetAdapterId;

		[FieldOffset(28)]
		public uint targetId;
	}

	[StructLayout(LayoutKind.Sequential, Size = 64)]
	private struct DISPLAYCONFIG_MODE_INFO
	{
	}

	private struct DISPLAYCONFIG_DEVICE_INFO_HEADER
	{
		public uint type;

		public uint size;

		public LUID adapterId;

		public uint id;
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct DISPLAYCONFIG_SOURCE_DEVICE_NAME
	{
		public DISPLAYCONFIG_DEVICE_INFO_HEADER header;

		public unsafe fixed char viewGdiDeviceName[32];
	}

	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	private struct DISPLAYCONFIG_TARGET_DEVICE_NAME
	{
		public DISPLAYCONFIG_DEVICE_INFO_HEADER header;

		public uint flags;

		public uint outputTechnology;

		public ushort edidManufactureId;

		public ushort edidProductCodeId;

		public uint connectorInstance;

		public unsafe fixed char monitorFriendlyDeviceName[64];

		public unsafe fixed char monitorDevicePath[128];
	}

	private const uint QDC_ONLY_ACTIVE_PATHS = 2u;

	private const uint DISPLAYCONFIG_DEVICE_INFO_GET_SOURCE_NAME = 1u;

	private const uint DISPLAYCONFIG_DEVICE_INFO_GET_TARGET_NAME = 2u;

	private const uint DISPLAYCONFIG_TARGET_FRIENDLY_NAME_FROM_EDID = 1u;

	private const uint DISPLAYCONFIG_TARGET_FRIENDLY_NAME_FORCED = 2u;

	private const int ERROR_INSUFFICIENT_BUFFER = 122;

	public static string?[] Resolve(ReadOnlySpan<GlfwMonitor> monitors)
	{
		string[] array = new string[monitors.Length];
		try
		{
			Dictionary<string, string> dictionary = QueryFriendlyNames();
			for (int i = 0; i < monitors.Length; i++)
			{
				string win32Adapter = Glfw.PInvoke.GetWin32Adapter(monitors[i]);
				if (win32Adapter != null && dictionary.TryGetValue(win32Adapter, out var value))
				{
					array[i] = value;
				}
				VerboseLog.Log($"Monitor {i}: adapter '{win32Adapter}' -> '{array[i] ?? monitors[i].Name}'{((array[i] == null) ? " (glfw name, no DisplayConfig match)" : "")}");
			}
		}
		catch (Exception ex)
		{
			VerboseLog.Log("DisplayConfig monitor names unavailable: " + ex.Message);
		}
		return array;
	}

	private unsafe static Dictionary<string, string> QueryFriendlyNames()
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
		int displayConfigBufferSizes;
		while (true)
		{
			displayConfigBufferSizes = GetDisplayConfigBufferSizes(2u, out var numPathArrayElements, out var numModeInfoArrayElements);
			if (displayConfigBufferSizes != 0)
			{
				break;
			}
			DISPLAYCONFIG_PATH_INFO[] array = new DISPLAYCONFIG_PATH_INFO[numPathArrayElements];
			DISPLAYCONFIG_MODE_INFO[] array2 = new DISPLAYCONFIG_MODE_INFO[numModeInfoArrayElements];
			fixed (DISPLAYCONFIG_PATH_INFO* pathArray = array)
			{
				fixed (DISPLAYCONFIG_MODE_INFO* modeInfoArray = array2)
				{
					displayConfigBufferSizes = QueryDisplayConfig(2u, ref numPathArrayElements, pathArray, ref numModeInfoArrayElements, modeInfoArray, null);
				}
			}
			switch (displayConfigBufferSizes)
			{
			case 122:
				break;
			default:
				throw new InvalidOperationException($"QueryDisplayConfig failed ({displayConfigBufferSizes})");
			case 0:
			{
				for (int i = 0; i < numPathArrayElements; i++)
				{
					DISPLAYCONFIG_PATH_INFO dISPLAYCONFIG_PATH_INFO = array[i];
					DISPLAYCONFIG_SOURCE_DEVICE_NAME dISPLAYCONFIG_SOURCE_DEVICE_NAME = default(DISPLAYCONFIG_SOURCE_DEVICE_NAME);
					dISPLAYCONFIG_SOURCE_DEVICE_NAME.header.type = 1u;
					dISPLAYCONFIG_SOURCE_DEVICE_NAME.header.size = (uint)sizeof(DISPLAYCONFIG_SOURCE_DEVICE_NAME);
					dISPLAYCONFIG_SOURCE_DEVICE_NAME.header.adapterId = dISPLAYCONFIG_PATH_INFO.sourceAdapterId;
					dISPLAYCONFIG_SOURCE_DEVICE_NAME.header.id = dISPLAYCONFIG_PATH_INFO.sourceId;
					if (DisplayConfigGetDeviceInfo(&dISPLAYCONFIG_SOURCE_DEVICE_NAME.header) != 0)
					{
						continue;
					}
					DISPLAYCONFIG_TARGET_DEVICE_NAME dISPLAYCONFIG_TARGET_DEVICE_NAME = default(DISPLAYCONFIG_TARGET_DEVICE_NAME);
					dISPLAYCONFIG_TARGET_DEVICE_NAME.header.type = 2u;
					dISPLAYCONFIG_TARGET_DEVICE_NAME.header.size = (uint)sizeof(DISPLAYCONFIG_TARGET_DEVICE_NAME);
					dISPLAYCONFIG_TARGET_DEVICE_NAME.header.adapterId = dISPLAYCONFIG_PATH_INFO.targetAdapterId;
					dISPLAYCONFIG_TARGET_DEVICE_NAME.header.id = dISPLAYCONFIG_PATH_INFO.targetId;
					if (DisplayConfigGetDeviceInfo(&dISPLAYCONFIG_TARGET_DEVICE_NAME.header) == 0 && (dISPLAYCONFIG_TARGET_DEVICE_NAME.flags & 3) != 0)
					{
						string text = new string(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(dISPLAYCONFIG_SOURCE_DEVICE_NAME.viewGdiDeviceName));
						string text2 = new string(MemoryMarshal.CreateReadOnlySpanFromNullTerminated(dISPLAYCONFIG_TARGET_DEVICE_NAME.monitorFriendlyDeviceName)).Trim();
						if (text.Length > 0 && text2.Length > 0)
						{
							dictionary.TryAdd(text, text2);
						}
					}
				}
				return dictionary;
			}
			}
		}
		throw new InvalidOperationException($"GetDisplayConfigBufferSizes failed ({displayConfigBufferSizes})");
	}

	[LibraryImport("user32.dll")]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	private unsafe static int GetDisplayConfigBufferSizes(uint flags, out uint numPathArrayElements, out uint numModeInfoArrayElements)
	{
		numPathArrayElements = 0u;
		numModeInfoArrayElements = 0u;
		int result;
		fixed (uint* _numModeInfoArrayElements_native = &numModeInfoArrayElements)
		{
			fixed (uint* _numPathArrayElements_native = &numPathArrayElements)
			{
				result = __PInvoke(flags, _numPathArrayElements_native, _numModeInfoArrayElements_native);
			}
		}
		return result;
		[DllImport("user32.dll", EntryPoint = "GetDisplayConfigBufferSizes", ExactSpelling = true)]
		static extern unsafe int __PInvoke(uint __flags_native, uint* __numPathArrayElements_native, uint* __numModeInfoArrayElements_native);
	}

	[LibraryImport("user32.dll")]
	[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "42.42.42.42")]
	[SkipLocalsInit]
	private unsafe static int QueryDisplayConfig(uint flags, ref uint numPathArrayElements, DISPLAYCONFIG_PATH_INFO* pathArray, ref uint numModeInfoArrayElements, DISPLAYCONFIG_MODE_INFO* modeInfoArray, void* currentTopologyId)
	{
		int result;
		fixed (uint* _numModeInfoArrayElements_native = &numModeInfoArrayElements)
		{
			fixed (uint* _numPathArrayElements_native = &numPathArrayElements)
			{
				result = __PInvoke(flags, _numPathArrayElements_native, pathArray, _numModeInfoArrayElements_native, modeInfoArray, currentTopologyId);
			}
		}
		return result;
		[DllImport("user32.dll", EntryPoint = "QueryDisplayConfig", ExactSpelling = true)]
		static extern unsafe int __PInvoke(uint __flags_native, uint* __numPathArrayElements_native, DISPLAYCONFIG_PATH_INFO* __pathArray_native, uint* __numModeInfoArrayElements_native, DISPLAYCONFIG_MODE_INFO* __modeInfoArray_native, void* __currentTopologyId_native);
	}

	[DllImport("user32.dll", ExactSpelling = true)]
	[LibraryImport("user32.dll")]
	private unsafe static extern int DisplayConfigGetDeviceInfo(DISPLAYCONFIG_DEVICE_INFO_HEADER* requestPacket);
}
