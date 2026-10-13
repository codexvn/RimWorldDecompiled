using System;
using Unity.Collections;
using UnityStubs.Internal;

namespace UnityEngine.Rendering;

public class CommandBuffer : IDisposable
{
	public string name { get; set; }

	public void Dispose()
	{
	}

	public void Release()
	{
	}

	public void EnableScissorRect(Rect scissor)
	{
		StubWarner.LogStub("EnableScissorRect", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 8);
	}

	public void DisableScissorRect()
	{
		StubWarner.LogStub("DisableScissorRect", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 9);
	}

	public void Clear()
	{
		StubWarner.LogStub("Clear", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 10);
	}

	public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor)
	{
		StubWarner.LogStub("ClearRenderTarget", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 11);
	}

	public void ClearRenderTarget(RTClearFlags clearFlags, Color backgroundColor, float depth = 1f, uint stencil = 0u)
	{
		StubWarner.LogStub("ClearRenderTarget", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 12);
	}

	public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
	{
		StubWarner.LogStub("ClearRenderTarget", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 13);
	}

	public void DispatchCompute(ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ)
	{
		StubWarner.LogStub("DispatchCompute", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 14);
	}

	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material)
	{
		StubWarner.LogStub("DrawMesh", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 15);
	}

	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass)
	{
		StubWarner.LogStub("DrawMesh", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 16);
	}

	public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties)
	{
		StubWarner.LogStub("DrawMesh", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 17);
	}

	public void SetViewProjectionMatrices(Matrix4x4 view, Matrix4x4 proj)
	{
		StubWarner.LogStub("SetViewProjectionMatrices", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 18);
	}

	public void SetViewport(Rect pixelRect)
	{
		StubWarner.LogStub("SetViewport", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 19);
	}

	public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest)
	{
		StubWarner.LogStub("Blit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 20);
	}

	public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat)
	{
		StubWarner.LogStub("Blit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 21);
	}

	public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass)
	{
		StubWarner.LogStub("Blit", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 22);
	}

	public void GetTemporaryRT(int nameID, int width, int height)
	{
		StubWarner.LogStub("GetTemporaryRT", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 23);
	}

	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
	{
		StubWarner.LogStub("GetTemporaryRT", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 24);
	}

	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter)
	{
		StubWarner.LogStub("GetTemporaryRT", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 25);
	}

	public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format)
	{
		StubWarner.LogStub("GetTemporaryRT", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 26);
	}

	public void ReleaseTemporaryRT(int nameID)
	{
		StubWarner.LogStub("ReleaseTemporaryRT", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 27);
	}

	public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
	{
		StubWarner.LogStub("SetGlobalTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 28);
	}

	public void SetGlobalTexture(string name, RenderTargetIdentifier value)
	{
		StubWarner.LogStub("SetGlobalTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 29);
	}

	public void SetRenderTarget(RenderTargetIdentifier rt)
	{
		StubWarner.LogStub("SetRenderTarget", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 30);
	}

	public void IssuePluginEvent(nint callback, int eventID)
	{
		StubWarner.LogStub("IssuePluginEvent", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 31);
	}

	public void IssuePluginEventAndData(nint callback, int eventID, nint data)
	{
		StubWarner.LogStub("IssuePluginEventAndData", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 32);
	}

	public void IssuePluginEventAndDataWithFlags(nint callback, int eventID, CustomMarkerCallbackFlags flags, nint data)
	{
		StubWarner.LogStub("IssuePluginEventAndDataWithFlags", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 33);
	}

	public void SetBufferData<T0>(GraphicsBuffer buffer, NativeArray<T0> data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count)
	{
		StubWarner.LogStub("SetBufferData", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 35);
	}

	public void SetComputeIntParam(ComputeShader computeShader, int nameID, int val)
	{
		StubWarner.LogStub("SetComputeIntParam", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 36);
	}

	public void SetExecutionFlags(CommandBufferExecutionFlags flags)
	{
		StubWarner.LogStub("SetExecutionFlags", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 37);
	}

	public void SetRenderTarget(RenderTargetIdentifier rt, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
	{
		StubWarner.LogStub("SetRenderTarget", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\CommandBuffer.cs", 38);
	}
}
