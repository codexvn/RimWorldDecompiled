using UnityStubs.Internal;

namespace UnityEngine;

public class ComputeShader : Object
{
	public void Dispatch(int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ)
	{
		StubWarner.LogStub("Dispatch", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 7);
	}

	public int FindKernel(string name)
	{
		StubWarner.LogStub("FindKernel", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 8);
		return -1;
	}

	public void SetBuffer(int kernelIndex, int nameID, GraphicsBuffer buffer)
	{
		StubWarner.LogStub("SetBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 9);
	}

	public void SetBuffer(int kernelIndex, string name, ComputeBuffer buffer)
	{
		StubWarner.LogStub("SetBuffer", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 10);
	}

	public void SetFloat(string name, float val)
	{
		StubWarner.LogStub("SetFloat", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 11);
	}

	public void SetInt(string name, int val)
	{
		StubWarner.LogStub("SetInt", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 12);
	}

	public void SetInts(int nameID, params int[] values)
	{
		StubWarner.LogStub("SetInts", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 13);
	}

	public void SetInts(string name, params int[] values)
	{
		StubWarner.LogStub("SetInts", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 14);
	}

	public void SetTexture(int kernelIndex, string name, Texture texture)
	{
		StubWarner.LogStub("SetTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 15);
	}

	public void SetTexture(int kernelIndex, int nameID, Texture texture)
	{
		StubWarner.LogStub("SetTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 16);
	}

	public void SetTexture(int kernelIndex, string name, Texture texture, int mipLevel)
	{
		StubWarner.LogStub("SetTexture", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 17);
	}

	public void SetVector(string name, Vector4 val)
	{
		StubWarner.LogStub("SetVector", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\ComputeShader.cs", 18);
	}
}
