using System;
using UnityStubs.Internal;

namespace UnityEngine;

public class GraphicsBuffer : IDisposable
{
	public enum Target
	{
		Append = 64,
		Constant = 512,
		CopyDestination = 8,
		CopySource = 4,
		Counter = 128,
		Index = 2,
		IndirectArguments = 256,
		Raw = 32,
		Structured = 16,
		Vertex = 1
	}

	public int count
	{
		get
		{
			StubWarner.LogStub("count", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GraphicsBuffer.cs", 7);
			return 0;
		}
	}

	public void Dispose()
	{
		StubWarner.LogStub("Dispose", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GraphicsBuffer.cs", 5);
	}

	public GraphicsBuffer(Target target, int count, int stride)
	{
	}

	public bool IsValid()
	{
		StubWarner.LogStub("IsValid", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GraphicsBuffer.cs", 8);
		return false;
	}

	public void Release()
	{
		StubWarner.LogStub("Release", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\GraphicsBuffer.cs", 9);
	}
}
