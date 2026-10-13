using System;
using System.Buffers.Binary;

namespace Waylith.Profiling.Instrumentation;

internal ref struct ILReader
{
	private static class ThrowHelper
	{
		public static void ThrowInvalidProgramException()
		{
			throw new InvalidProgramException("Invalid IL or CLR metadata");
		}
	}

	private int _currentOffset;

	private readonly ReadOnlySpan<byte> _ilBytes;

	public readonly int Offset => _currentOffset;

	public readonly int Size => _ilBytes.Length;

	public readonly bool HasNext => _currentOffset < _ilBytes.Length;

	public ILReader(ReadOnlySpan<byte> ilBytes, int currentOffset = 0)
	{
		_ilBytes = ilBytes;
		_currentOffset = currentOffset;
	}

	public byte ReadILByte()
	{
		if (_currentOffset + 1 > _ilBytes.Length)
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		return _ilBytes[_currentOffset++];
	}

	public ushort ReadILUInt16()
	{
		if (!BinaryPrimitives.TryReadUInt16LittleEndian(_ilBytes.Slice(_currentOffset), out var value))
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		_currentOffset += 2;
		return value;
	}

	public uint ReadILUInt32()
	{
		if (!BinaryPrimitives.TryReadUInt32LittleEndian(_ilBytes.Slice(_currentOffset), out var value))
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		_currentOffset += 4;
		return value;
	}

	public int ReadILToken()
	{
		return (int)ReadILUInt32();
	}

	public ulong ReadILUInt64()
	{
		if (!BinaryPrimitives.TryReadUInt64LittleEndian(_ilBytes.Slice(_currentOffset), out var value))
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		_currentOffset += 8;
		return value;
	}

	public float ReadILFloat()
	{
		if (!BinaryPrimitives.TryReadSingleLittleEndian(_ilBytes.Slice(_currentOffset), out var value))
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		_currentOffset += 4;
		return value;
	}

	public double ReadILDouble()
	{
		if (!BinaryPrimitives.TryReadDoubleLittleEndian(_ilBytes.Slice(_currentOffset), out var value))
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		_currentOffset += 8;
		return value;
	}

	public ILOpcode ReadILOpcode()
	{
		ILOpcode iLOpcode = (ILOpcode)ReadILByte();
		if (iLOpcode == ILOpcode.prefix1)
		{
			iLOpcode = (ILOpcode)(256 + ReadILByte());
		}
		return iLOpcode;
	}

	public readonly ILOpcode PeekILOpcode()
	{
		ILOpcode iLOpcode = (ILOpcode)_ilBytes[_currentOffset];
		if (iLOpcode == ILOpcode.prefix1)
		{
			if (_currentOffset + 2 > _ilBytes.Length)
			{
				ThrowHelper.ThrowInvalidProgramException();
			}
			iLOpcode = (ILOpcode)(256 + _ilBytes[_currentOffset + 1]);
		}
		return iLOpcode;
	}

	public readonly int PeekILToken()
	{
		if (!BinaryPrimitives.TryReadInt32LittleEndian(_ilBytes.Slice(_currentOffset), out var value))
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		return value;
	}

	public void Skip(ILOpcode opcode)
	{
		if (!opcode.IsValid())
		{
			ThrowHelper.ThrowInvalidProgramException();
		}
		if (opcode != ILOpcode.switch_)
		{
			int num = (((ILOpcode)(byte)opcode == opcode) ? 1 : 2);
			_currentOffset += opcode.GetSize() - num;
		}
		else
		{
			uint num2 = ReadILUInt32();
			_currentOffset += checked((int)(num2 * 4));
		}
	}

	public void Seek(int offset)
	{
		_currentOffset = offset;
	}

	public int ReadBranchDestination(ILOpcode currentOpcode)
	{
		if ((currentOpcode >= ILOpcode.br_s && currentOpcode <= ILOpcode.blt_un_s) || currentOpcode == ILOpcode.leave_s)
		{
			return (sbyte)ReadILByte() + Offset;
		}
		return (int)ReadILUInt32() + Offset;
	}
}
