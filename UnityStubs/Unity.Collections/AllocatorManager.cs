using System;
using System.Runtime.InteropServices;
using UnityStubs.Internal;

namespace Unity.Collections;

public static class AllocatorManager
{
	public delegate int TryFunction(nint allocatorState, ref Block block);

	[StructLayout(LayoutKind.Sequential, Size = 1)]
	public struct Block : IDisposable
	{
		public void Dispose()
		{
			StubWarner.LogStub("Dispose", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AllocatorManager.cs", 12);
		}
	}

	public struct AllocatorHandle : IAllocator, IDisposable, IEquatable<AllocatorHandle>, IComparable<AllocatorHandle>
	{
		private Allocator _allocator;

		public const int InvalidChildAllocatorIndex = -1;

		public const int InvalidChildSafetyHandleIndex = -1;

		public TryFunction Function => null;

		public AllocatorHandle Handle
		{
			get
			{
				return this;
			}
			set
			{
				_allocator = value._allocator;
			}
		}

		public bool IsAutoDispose => _allocator == Allocator.Temp;

		public bool IsCustomAllocator => _allocator >= Allocator.FirstUserIndex;

		public Allocator ToAllocator => _allocator;

		public int CompareTo(AllocatorHandle other)
		{
			int allocator = (int)_allocator;
			return allocator.CompareTo((int)other._allocator);
		}

		public void Dispose()
		{
			StubWarner.LogStub("Dispose", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\UnityStubs\\AllocatorManager.cs", 19);
		}

		public bool Equals(AllocatorHandle other)
		{
			return _allocator == other._allocator;
		}

		public int Try(ref Block block)
		{
			return 0;
		}

		public static implicit operator AllocatorHandle(Allocator a)
		{
			AllocatorHandle result = default(AllocatorHandle);
			result._allocator = a;
			return result;
		}
	}

	public interface IAllocator : IDisposable
	{
		TryFunction Function { get; }

		AllocatorHandle Handle { get; set; }

		bool IsAutoDispose { get; }

		bool IsCustomAllocator { get; }

		Allocator ToAllocator { get; }

		int Try(ref Block block);
	}

	public const ushort FirstUserIndex = 64;

	public const int kErrorBufferOverflow = -1;

	public const int kErrorNone = 0;

	public const ushort MaxNumCustomAllocators = 32768;
}
