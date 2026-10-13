namespace Waylith.Profiling.Instrumentation;

internal static class ILOpcodeHelper
{
	private const byte VariableSize = byte.MaxValue;

	private const byte Invalid = 254;

	private static readonly byte[] s_opcodeSizes = new byte[287]
	{
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 2, 2, 2, 2, 2, 2,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 2, 5, 9, 5, 9, 254, 1, 1, 5,
		5, 5, 1, 2, 2, 2, 2, 2, 2, 2,
		2, 2, 2, 2, 2, 2, 5, 5, 5, 5,
		5, 5, 5, 5, 5, 5, 5, 5, 5, 255,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 5, 5, 5, 5, 5, 5, 5, 1, 254,
		254, 5, 1, 5, 5, 5, 5, 5, 5, 5,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		5, 5, 1, 5, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 1, 1, 5, 5, 5, 254, 254, 254, 254,
		254, 254, 254, 254, 254, 254, 254, 254, 254, 1,
		1, 1, 1, 1, 1, 1, 1, 254, 254, 254,
		254, 254, 254, 254, 5, 1, 254, 254, 5, 254,
		254, 254, 254, 254, 254, 254, 254, 254, 5, 1,
		1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
		1, 5, 2, 1, 1, 254, 254, 254, 254, 254,
		254, 254, 254, 254, 254, 254, 254, 254, 254, 254,
		254, 254, 254, 254, 254, 254, 254, 254, 254, 254,
		254, 254, 254, 254, 1, 254, 2, 2, 2, 2,
		2, 2, 6, 6, 254, 4, 4, 4, 4, 4,
		4, 2, 254, 2, 3, 2, 2, 6, 6, 2,
		2, 3, 2, 254, 6, 2, 2
	};

	public static int GetSize(this ILOpcode opcode)
	{
		return s_opcodeSizes[(int)opcode];
	}

	public static bool IsValid(this ILOpcode opcode)
	{
		if ((uint)opcode < (uint)s_opcodeSizes.Length)
		{
			return s_opcodeSizes[(int)opcode] != 254;
		}
		return false;
	}

	public static bool IsBranch(this ILOpcode opcode)
	{
		if (opcode >= ILOpcode.br && opcode <= ILOpcode.blt_un)
		{
			return true;
		}
		if (opcode >= ILOpcode.br_s && opcode <= ILOpcode.blt_un_s)
		{
			return true;
		}
		return opcode switch
		{
			ILOpcode.leave_s => true, 
			ILOpcode.leave => true, 
			_ => false, 
		};
	}

	public static bool IsUnconditionalBranch(this ILOpcode opcode)
	{
		if (opcode != ILOpcode.br && opcode != ILOpcode.br_s && opcode != ILOpcode.leave)
		{
			return opcode == ILOpcode.leave_s;
		}
		return true;
	}
}
