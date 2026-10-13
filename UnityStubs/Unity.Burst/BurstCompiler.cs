using System;

namespace Unity.Burst;

public static class BurstCompiler
{
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
	public class StaticTypeReinitAttribute : Attribute
	{
		public readonly Type reinitType;

		public StaticTypeReinitAttribute(Type toReinit)
		{
			reinitType = toReinit;
		}
	}

	public static bool IsEnabled => false;

	public static nint CompileILPPMethod2(RuntimeMethodHandle burstMethodHandle)
	{
		return IntPtr.Zero;
	}

	public unsafe static void* GetILPPMethodFunctionPointer2(nint ilppMethod, RuntimeMethodHandle managedMethodHandle, RuntimeTypeHandle delegateTypeHandle)
	{
		return null;
	}
}
