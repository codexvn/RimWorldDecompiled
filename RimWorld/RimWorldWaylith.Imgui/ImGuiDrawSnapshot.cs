using System;
using System.Collections.Generic;
using Brutal.ImGuiApi;
using Brutal.Numerics;

namespace RimWorldWaylith.Imgui;

internal sealed class ImGuiDrawSnapshot
{
	public ImDrawVert[] Vertices = Array.Empty<ImDrawVert>();

	public ushort[] Indices = Array.Empty<ushort>();

	public ImGuiDrawCommand[] Commands = Array.Empty<ImGuiDrawCommand>();

	public ImGuiMaterialDraw[] MaterialDraws = Array.Empty<ImGuiMaterialDraw>();

	public int FrameNumber;

	public int VertexCount;

	public int IndexCount;

	public int CommandCount;

	public int MaterialDrawCount;

	public float2 DisplayPos;

	public float2 DisplaySize;

	public float2 FramebufferScale;

	public bool HasContent
	{
		get
		{
			if (CommandCount > 0 && VertexCount > 0)
			{
				return IndexCount > 0;
			}
			return false;
		}
	}

	public void CopyFrom(ImDrawDataPtr drawData, IReadOnlyList<ImGuiMaterialDraw> materialDraws)
	{
		ImVector<ImDrawListPtr> cmdLists = drawData.CmdLists;
		int num = 0;
		for (int i = 0; i < cmdLists.Count; i++)
		{
			num += cmdLists[i].CmdBuffer.Count;
		}
		Begin(drawData.TotalVtxCount, drawData.TotalIdxCount, num, drawData.DisplayPos, drawData.DisplaySize, drawData.FramebufferScale, materialDraws);
		int vtxBase = 0;
		int idxBase = 0;
		for (int j = 0; j < cmdLists.Count; j++)
		{
			Append(cmdLists[j], ref vtxBase, ref idxBase);
		}
	}

	public void CopyFrom(ImDrawListPtr list, int firstIndex, float2 displaySize, float2 framebufferScale, IReadOnlyList<ImGuiMaterialDraw> materialDraws)
	{
		Begin(list.VtxBuffer.Count, list.IdxBuffer.Count, list.CmdBuffer.Count, new float2(0f, 0f), displaySize, framebufferScale, materialDraws);
		int vtxBase = 0;
		int idxBase = 0;
		Append(list, ref vtxBase, ref idxBase, firstIndex);
	}

	private void Begin(int vertexCount, int indexCount, int maxCmds, float2 displayPos, float2 displaySize, float2 framebufferScale, IReadOnlyList<ImGuiMaterialDraw> materialDraws)
	{
		VertexCount = vertexCount;
		IndexCount = indexCount;
		DisplayPos = displayPos;
		DisplaySize = displaySize;
		FramebufferScale = framebufferScale;
		CommandCount = 0;
		EnsureCapacity(ref Vertices, VertexCount);
		EnsureCapacity(ref Indices, IndexCount);
		EnsureCapacity(ref Commands, maxCmds);
		EnsureCapacity(ref MaterialDraws, materialDraws.Count);
		for (int i = 0; i < materialDraws.Count; i++)
		{
			MaterialDraws[i] = materialDraws[i];
		}
		MaterialDrawCount = materialDraws.Count;
	}

	private void Append(ImDrawListPtr list, ref int vtxBase, ref int idxBase, int firstIndex = 0)
	{
		ImVector<ImDrawVert> vtxBuffer = list.VtxBuffer;
		for (int i = 0; i < vtxBuffer.Count; i++)
		{
			Vertices[vtxBase + i] = vtxBuffer[i];
		}
		ImVector<ushort> idxBuffer = list.IdxBuffer;
		for (int j = 0; j < idxBuffer.Count; j++)
		{
			Indices[idxBase + j] = idxBuffer[j];
		}
		ImVector<ImDrawCmd> cmdBuffer = list.CmdBuffer;
		for (int k = 0; k < cmdBuffer.Count; k++)
		{
			ImDrawCmd imDrawCmd = cmdBuffer[k];
			if (imDrawCmd.ElemCount != 0 && imDrawCmd.UserCallback == 0)
			{
				int idxOffset = (int)imDrawCmd.IdxOffset;
				int num = idxOffset + (int)imDrawCmd.ElemCount;
				if (num > firstIndex)
				{
					idxOffset = Math.Max(idxOffset, firstIndex);
					Commands[CommandCount++] = new ImGuiDrawCommand
					{
						ClipRect = imDrawCmd.ClipRect,
						TexId = imDrawCmd.GetTexID().Value,
						ElemCount = (uint)(num - idxOffset),
						IdxOffset = idxOffset + idxBase,
						VtxOffset = (int)imDrawCmd.VtxOffset + vtxBase
					};
				}
			}
		}
		vtxBase += vtxBuffer.Count;
		idxBase += idxBuffer.Count;
	}

	private static void EnsureCapacity<T>(ref T[] array, int needed)
	{
		if (array.Length < needed)
		{
			int num;
			for (num = ((array.Length == 0) ? 1024 : array.Length); num < needed; num *= 2)
			{
			}
			array = new T[num];
		}
	}
}
