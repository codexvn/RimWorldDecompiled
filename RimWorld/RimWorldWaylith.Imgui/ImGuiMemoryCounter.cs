using System.Runtime.InteropServices;
using Brutal.ImGuiApi;
using Waylith.Runtime;

namespace RimWorldWaylith.Imgui;

internal static class ImGuiMemoryCounter
{
	private static bool _installed;

	public unsafe static void Install()
	{
		if (!_installed)
		{
			_installed = true;
			ImGui.PInvoke.SetAllocatorFunctions((nint)(delegate*<nuint, void*, void*>)(&Alloc), (nint)(delegate*<void*, void*, void>)(&Free), null);
		}
	}

	[UnmanagedCallersOnly]
	private unsafe static void* Alloc(nuint size, void* userData)
	{
		return (void*)NativeAllocationRuntime.Allocate((long)size, 16, AllocationKind.UI);
	}

	[UnmanagedCallersOnly]
	private unsafe static void Free(void* pointer, void* userData)
	{
		if (pointer != null)
		{
			NativeAllocationRuntime.Free((nint)pointer, AllocationKind.UI);
		}
	}
}
