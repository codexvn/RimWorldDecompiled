using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Text;

namespace PsdSharp.Parsing;

internal class BigEndianReader(Stream input, Encoding encoding, bool leaveOpen = false) : IDisposable
{
	private bool _disposed;

	private byte[] _internalBuffer = Array.Empty<byte>();

	private long _numBytesRead;

	public Encoding Encoding => encoding;

	public long Position
	{
		get
		{
			if (!input.CanSeek)
			{
				return _numBytesRead;
			}
			return input.Position;
		}
	}

	public bool CanSeek => input.CanSeek;

	public long Length => input.Length;

	public long Read => new StreamReader(input).ReadToEnd().Length;

	public BigEndianReader(Stream input)
		: this(input, System.Text.Encoding.GetEncoding("ISO-8859-1"))
	{
	}

	private ReadOnlySpan<byte> InternalRead(Span<byte> buffer)
	{
		ThrowIfDisposed();
		if (buffer.Length == 0)
		{
			return buffer;
		}
		int num = 0;
		if (_internalBuffer.Length != 0)
		{
			num = Math.Min(_internalBuffer.Length, buffer.Length);
			_internalBuffer.AsSpan(0, num).CopyTo(buffer);
			if (num == _internalBuffer.Length)
			{
				_internalBuffer = Array.Empty<byte>();
			}
			else
			{
				byte[] array = new byte[_internalBuffer.Length - num];
				Array.Copy(_internalBuffer, num, array, 0, array.Length);
				_internalBuffer = array;
			}
			_numBytesRead += num;
		}
		if (num < buffer.Length)
		{
			Span<byte> buffer2 = buffer.Slice(num);
			int num2 = input.Read(buffer2);
			if (num2 < buffer2.Length)
			{
				throw new EndOfStreamException();
			}
			_numBytesRead += num2;
		}
		return buffer;
	}

	private bool TryReadFromInternalBuffer(out byte value)
	{
		if (_internalBuffer.Length != 0)
		{
			value = _internalBuffer[0];
			if (_internalBuffer.Length == 1)
			{
				_internalBuffer = Array.Empty<byte>();
			}
			else
			{
				byte[] array = new byte[_internalBuffer.Length - 1];
				Array.Copy(_internalBuffer, 1, array, 0, array.Length);
				_internalBuffer = array;
			}
			return true;
		}
		value = 0;
		return false;
	}

	private void ThrowIfDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("The BigEndianReader has been disposed.");
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing && !leaveOpen)
			{
				input.Dispose();
			}
			_disposed = true;
		}
		GC.SuppressFinalize(this);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public void Close()
	{
		Dispose(disposing: true);
	}

	public virtual byte ReadByte()
	{
		ThrowIfDisposed();
		if (TryReadFromInternalBuffer(out var value))
		{
			return value;
		}
		int num = input.ReadByte();
		if (num < 0)
		{
			throw new EndOfStreamException();
		}
		return (byte)num;
	}

	public virtual sbyte ReadSByte()
	{
		ThrowIfDisposed();
		if (TryReadFromInternalBuffer(out var value))
		{
			return (sbyte)value;
		}
		int num = input.ReadByte();
		if (num < 0)
		{
			throw new EndOfStreamException();
		}
		return (sbyte)num;
	}

	public virtual short ReadInt16()
	{
		Span<byte> buffer = stackalloc byte[2];
		return BinaryPrimitives.ReadInt16BigEndian(InternalRead(buffer));
	}

	public virtual ushort ReadUInt16()
	{
		Span<byte> buffer = stackalloc byte[2];
		return BinaryPrimitives.ReadUInt16BigEndian(InternalRead(buffer));
	}

	public virtual int ReadInt32()
	{
		Span<byte> buffer = stackalloc byte[4];
		return BinaryPrimitives.ReadInt32BigEndian(InternalRead(buffer));
	}

	public virtual uint ReadUInt32()
	{
		Span<byte> buffer = stackalloc byte[4];
		return BinaryPrimitives.ReadUInt32BigEndian(InternalRead(buffer));
	}

	public virtual long ReadInt64()
	{
		Span<byte> buffer = stackalloc byte[8];
		return BinaryPrimitives.ReadInt64BigEndian(InternalRead(buffer));
	}

	public virtual ulong ReadUInt64()
	{
		Span<byte> buffer = stackalloc byte[8];
		return BinaryPrimitives.ReadUInt64BigEndian(InternalRead(buffer));
	}

	public virtual float ReadSingle()
	{
		Span<byte> buffer = stackalloc byte[4];
		return BinaryPrimitives.ReadSingleBigEndian(InternalRead(buffer));
	}

	public virtual double ReadDouble()
	{
		Span<byte> buffer = stackalloc byte[8];
		return BinaryPrimitives.ReadDoubleBigEndian(InternalRead(buffer));
	}

	public virtual (string? String, ushort NumBytesRead) ReadPascalString(byte alignmentSize)
	{
		if (alignmentSize != 0 && alignmentSize != 2 && alignmentSize != 4)
		{
			throw new ArgumentOutOfRangeException("alignmentSize");
		}
		byte b = ReadByte();
		if (b == 0)
		{
			Span<byte> buffer = stackalloc byte[alignmentSize - 1];
			InternalRead(buffer);
			return (String: null, NumBytesRead: alignmentSize);
		}
		Span<byte> span = stackalloc byte[(int)b];
		InternalRead(span);
		int num = 1 + b;
		if (alignmentSize > 0)
		{
			int num2 = num % alignmentSize;
			if (num2 != 0)
			{
				Span<byte> buffer2 = stackalloc byte[alignmentSize - num2];
				InternalRead(buffer2);
				num += buffer2.Length;
			}
		}
		return (String: encoding.GetString(span), NumBytesRead: (ushort)num);
	}

	public virtual (string? String, long NumBytesRead) ReadUnicodeString()
	{
		uint num = ReadUInt32();
		if (num == 0)
		{
			return (String: null, NumBytesRead: 4L);
		}
		if (num > 16777216)
		{
			throw new InvalidDataException("The data is corrupted.");
		}
		int num2 = checked((int)num * 2);
		if (num2 <= 512)
		{
			Span<byte> span = stackalloc byte[num2];
			InternalRead(span);
			EnsureTerminationBytes(span);
			return (String: System.Text.Encoding.BigEndianUnicode.GetString(span), NumBytesRead: 4 + span.Length);
		}
		byte[] array = ArrayPool<byte>.Shared.Rent(num2);
		try
		{
			Span<byte> span2 = array.AsSpan(0, num2);
			InternalRead(span2);
			EnsureTerminationBytes(span2);
			return (String: System.Text.Encoding.BigEndianUnicode.GetString(span2), NumBytesRead: 4 + span2.Length);
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	public virtual string ReadFixedLengthString(uint length)
	{
		return ReadFixedLengthString((int)length);
	}

	public virtual string ReadFixedLengthString(int length)
	{
		Span<byte> span = stackalloc byte[length];
		InternalRead(span);
		return encoding.GetString(span);
	}

	private void EnsureTerminationBytes(Span<byte> buffer)
	{
		if (buffer[buffer.Length - 1] == 0)
		{
			if (buffer[buffer.Length - 2] == 0)
			{
				return;
			}
		}
		throw ParserUtils.DataCorrupt();
	}

	public virtual string ReadSignature()
	{
		return ReadFixedLengthString(4);
	}

	public virtual byte[] ReadUntilEnd()
	{
		if (input.CanSeek)
		{
			byte[] array = new byte[input.Length - input.Position];
			if (input.Read(array, 0, array.Length) >= array.Length)
			{
				return array;
			}
			throw new EndOfStreamException();
		}
		MemoryStream memoryStream = new MemoryStream();
		input.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}

	public virtual void ReadIntoBuffer(Span<byte> buffer)
	{
		if (buffer.Length != 0)
		{
			InternalRead(buffer);
		}
	}

	public virtual void CopyTo(Stream destination, long amountBytes)
	{
		byte[] array = new byte[16777216];
		while (amountBytes > 0)
		{
			int num = (int)Math.Min(array.Length, amountBytes);
			InternalRead(array.AsSpan(0, num));
			destination.Write(array, 0, num);
			amountBytes -= num;
		}
	}

	public virtual void CopyTo(Stream destination)
	{
		if (_internalBuffer.Length != 0)
		{
			destination.Write(_internalBuffer, 0, _internalBuffer.Length);
			_internalBuffer = Array.Empty<byte>();
		}
		input.CopyTo(destination);
	}

	public void Skip(int numberOfBytes)
	{
		Skip((long)numberOfBytes);
	}

	public void Skip(long numberOfBytes)
	{
		ThrowIfDisposed();
		if (numberOfBytes <= 0)
		{
			return;
		}
		if (numberOfBytes < 512)
		{
			Span<byte> buffer = stackalloc byte[(int)numberOfBytes];
			InternalRead(buffer);
			return;
		}
		byte[] array = ArrayPool<byte>.Shared.Rent(8192);
		try
		{
			while (numberOfBytes > 0)
			{
				Span<byte> buffer2 = array.AsSpan(0, (int)Math.Min(numberOfBytes, array.Length));
				InternalRead(buffer2);
				numberOfBytes -= buffer2.Length;
			}
		}
		finally
		{
			ArrayPool<byte>.Shared.Return(array);
		}
	}

	public void Backtrack(byte[] bytes)
	{
		byte[] array = new byte[_internalBuffer.Length + bytes.Length];
		Array.Copy(_internalBuffer, 0, array, 0, _internalBuffer.Length);
		Array.Copy(bytes, 0, array, _internalBuffer.Length, bytes.Length);
		_internalBuffer = array;
		_numBytesRead -= bytes.Length;
	}

	public void Seek(long position)
	{
		input.Seek(position, SeekOrigin.Begin);
	}
}
