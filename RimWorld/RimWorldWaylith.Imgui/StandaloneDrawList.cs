using System;
using System.Runtime.InteropServices;
using Brutal.ImGuiApi;
using Brutal.Numerics;

namespace RimWorldWaylith.Imgui;

internal sealed class StandaloneDrawList : IDisposable
{
	private unsafe ImDrawList* list;

	public unsafe ImDrawListPtr Reset(float2 clipMin, float2 clipMax)
	{
		if (list == null)
		{
			list = (ImDrawList*)NativeMemory.AllocZeroed((nuint)sizeof(ImDrawList));
			new ImDrawListPtr(list).SetDrawListSharedData(ImGui.GetDrawListSharedData());
		}
		ImDrawListPtr imDrawListPtr = new ImDrawListPtr(list);
		imDrawListPtr.ResetForNewFrame();
		imDrawListPtr.PushTexture(ImGui.GetIO().Fonts.TexRef);
		imDrawListPtr.PushClipRect(in clipMin, in clipMax);
		return imDrawListPtr;
	}

	public unsafe void Dispose()
	{
		if (list != null)
		{
			ImDrawListPtr target = new ImDrawListPtr(list);
			target.ClearFreeMemory();
			target.SetDrawListSharedData(default(ImDrawListSharedDataPtr));
			NativeMemory.Free(list);
			list = null;
		}
	}
}
