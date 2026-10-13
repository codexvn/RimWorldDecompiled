using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace LudeonTK
{
	// Token: 0x020000B3 RID: 179
	public static class NativeArrayUtility
	{
		// Token: 0x060003CB RID: 971 RVA: 0x000166CC File Offset: 0x000148CC
		public static void EnsureDisposed<[IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000166DC File Offset: 0x000148DC
		public static void EnsureDisposed<[IsUnmanaged] T>(this NativeList<T> array) where T : struct, ValueType
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000166EC File Offset: 0x000148EC
		public static void EnsureDisposed(this NativeBitArray array)
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000166FC File Offset: 0x000148FC
		public static void EnsureDisposed<[IsUnmanaged] TElement, [IsUnmanaged] TPriority, [IsUnmanaged] TComparer>(this NativePriorityQueue<TElement, TPriority, TComparer> array) where TElement : struct, ValueType where TPriority : struct, ValueType where TComparer : struct, ValueType, IComparer<TPriority>
		{
			if (array.IsCreated)
			{
				array.Dispose();
			}
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0001670C File Offset: 0x0001490C
		public static void EnsureDisposed<[IsUnmanaged] T>(this UnsafeList<T> list) where T : struct, ValueType
		{
			if (list.IsCreated)
			{
				list.Dispose();
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0001671C File Offset: 0x0001491C
		public static void EnsureDisposed<[IsUnmanaged] T, [IsUnmanaged] U>(this UnsafeHeap<T, U> list) where T : struct, ValueType where U : struct, ValueType, IComparer<T>
		{
			if (list.IsCreated)
			{
				list.Dispose();
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001672C File Offset: 0x0001492C
		public static void EnsureDisposed(this UnsafeBitArray bitArray)
		{
			if (bitArray.IsCreated)
			{
				bitArray.Dispose();
			}
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0001673C File Offset: 0x0001493C
		public static void MemClear<[IsUnmanaged] T>(NativeArray<T> array) where T : struct, ValueType
		{
			UnsafeUtility.MemClear(array.GetUnsafePtr<T>(), (long)array.Length * (long)sizeof(T));
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00016759 File Offset: 0x00014959
		public unsafe static void MemClear<[IsUnmanaged] T>(UnsafeList<T> list) where T : struct, ValueType
		{
			UnsafeUtility.MemClear((void*)list.Ptr, (long)list.Length * (long)sizeof(T));
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00016776 File Offset: 0x00014976
		public unsafe static void MemClear<[IsUnmanaged] T>(NativeList<T> list) where T : struct, ValueType
		{
			UnsafeUtility.MemClear((void*)list.GetUnsafePtr<T>(), (long)list.Length * (long)sizeof(T));
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00016793 File Offset: 0x00014993
		public static void Clear<[IsUnmanaged] T>(this NativeArray<T> array) where T : struct, ValueType
		{
			NativeArrayUtility.MemClear<T>(array);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x0001679B File Offset: 0x0001499B
		public static void Clear<[IsUnmanaged] T>(this NativeList<T> array) where T : struct, ValueType
		{
			NativeArrayUtility.MemClear<T>(array);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x000167A3 File Offset: 0x000149A3
		public static void Clear<[IsUnmanaged] T>(this UnsafeList<T> array) where T : struct, ValueType
		{
			NativeArrayUtility.MemClear<T>(array);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x000167AB File Offset: 0x000149AB
		public static T[] GetInternalArray<T>(List<T> list)
		{
			return NativeArrayUtility.ArrayAccessor<T>.Getter(list);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x000167B8 File Offset: 0x000149B8
		public unsafe static NativeArray<T> GetNativeArrayCopy<[IsUnmanaged] T>(List<T> source, Allocator allocator, bool clearMemory = false) where T : struct, ValueType
		{
			T[] internalArray = NativeArrayUtility.GetInternalArray<T>(source);
			NativeArray<T> nativeArray = new NativeArray<T>(source.Count, allocator, clearMemory ? NativeArrayOptions.ClearMemory : NativeArrayOptions.UninitializedMemory);
			fixed (T[] array = internalArray)
			{
				void* source2;
				if (internalArray == null || array.Length == 0)
				{
					source2 = null;
				}
				else
				{
					source2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(nativeArray), source2, (long)source.Count * (long)UnsafeUtility.SizeOf<T>());
			}
			return nativeArray;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00016814 File Offset: 0x00014A14
		public unsafe static NativeArray<T> GetNativeArrayCopy<[IsUnmanaged] T>(T[] source, Allocator allocator, bool clearMemory = false) where T : struct, ValueType
		{
			NativeArray<T> nativeArray = new NativeArray<T>(source.Length, allocator, clearMemory ? NativeArrayOptions.ClearMemory : NativeArrayOptions.UninitializedMemory);
			fixed (T[] array = source)
			{
				void* source2;
				if (source == null || array.Length == 0)
				{
					source2 = null;
				}
				else
				{
					source2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(nativeArray), source2, (long)source.Length * (long)UnsafeUtility.SizeOf<T>());
			}
			return nativeArray;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00016864 File Offset: 0x00014A64
		public unsafe static void CopyNativeToArray<[IsUnmanaged] T>(T[] destination, NativeArray<T> source) where T : struct, ValueType
		{
			if (source.Length != destination.Length)
			{
				throw new Exception(string.Format("Attempted to copy a native array of length {0} into an array with length {1}", source.Length, destination.Length));
			}
			fixed (T[] array = destination)
			{
				void* destination2;
				if (destination == null || array.Length == 0)
				{
					destination2 = null;
				}
				else
				{
					destination2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(destination2, NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(source), (long)destination.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000168D4 File Offset: 0x00014AD4
		public unsafe static void CopyUnsafeListToArray<[IsUnmanaged] T>(T[] destination, UnsafeList<T> source) where T : struct, ValueType
		{
			if (source.Length != destination.Length)
			{
				throw new Exception(string.Format("Attempted to copy a unsafe array of length {0} into an array with length {1}", source.Length, destination.Length));
			}
			fixed (T[] array = destination)
			{
				void* destination2;
				if (destination == null || array.Length == 0)
				{
					destination2 = null;
				}
				else
				{
					destination2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(destination2, (void*)source.Ptr, (long)destination.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x060003DD RID: 989 RVA: 0x00016944 File Offset: 0x00014B44
		public static void CopyNativeListToList<[IsUnmanaged] T>(List<T> destination, NativeList<T> source) where T : struct, ValueType
		{
			destination.Clear();
			destination.Capacity = Mathf.Max(source.Length, destination.Capacity);
			for (int i = 0; i < source.Length; i++)
			{
				destination.Add(source[i]);
			}
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00016990 File Offset: 0x00014B90
		public static void CopyUnsafeListToList<[IsUnmanaged] T>(List<T> destination, UnsafeList<T> source) where T : struct, ValueType
		{
			destination.Clear();
			destination.Capacity = Mathf.Max(destination.Count, destination.Capacity);
			for (int i = 0; i < source.Length; i++)
			{
				destination.Add(source[i]);
			}
		}

		// Token: 0x060003DF RID: 991 RVA: 0x000169DC File Offset: 0x00014BDC
		public static void CopyArrayToList<[IsUnmanaged] T>(List<T> destination, NativeArray<T> source) where T : struct, ValueType
		{
			destination.Clear();
			destination.Capacity = Mathf.Max(destination.Count, destination.Capacity);
			for (int i = 0; i < source.Length; i++)
			{
				destination.Add(source[i]);
			}
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00016A28 File Offset: 0x00014C28
		public unsafe static void CopyArrayToNative<[IsUnmanaged] T>(NativeArray<T> destination, T[] source) where T : struct, ValueType
		{
			if (source.Length != destination.Length)
			{
				throw new Exception(string.Format("Attempted to copy an array of length {0} into a native array with length {1}", source.Length, destination.Length));
			}
			fixed (T[] array = source)
			{
				void* source2;
				if (source == null || array.Length == 0)
				{
					source2 = null;
				}
				else
				{
					source2 = (void*)(&array[0]);
				}
				UnsafeUtility.MemCpy(NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<T>(destination), source2, (long)source.Length * (long)UnsafeUtility.SizeOf<T>());
			}
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x00016A96 File Offset: 0x00014C96
		public static void CopyFrom(this NativeBitArray destination, NativeBitArray source)
		{
			destination.Copy(0, ref source, 0, Mathf.Min(destination.Length, source.Length));
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x00016AB8 File Offset: 0x00014CB8
		public static void Reverse<[IsUnmanaged] T>(this NativeList<T> list) where T : struct, ValueType
		{
			int length = list.Length;
			int i = 0;
			int num = length - 1;
			while (i < num)
			{
				int index = i;
				ref NativeList<T> ptr = ref list;
				int index2 = num;
				T value = list[num];
				T value2 = list[i];
				list[index] = value;
				ptr[index2] = value2;
				i++;
				num--;
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x00016B15 File Offset: 0x00014D15
		public static NativeArray<T> EmptyArray<[IsUnmanaged] T>() where T : struct, ValueType
		{
			return new NativeArray<T>(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x00016B1F File Offset: 0x00014D1F
		public static NativeList<T> EmptyList<[IsUnmanaged] T>() where T : struct, ValueType
		{
			return new NativeList<T>(0, Allocator.Persistent);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00016B2D File Offset: 0x00014D2D
		public static NativeBitArray EmptyBitArray()
		{
			return new NativeBitArray(0, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x020000B4 RID: 180
		private static class ArrayAccessor<T>
		{
			// Token: 0x060003E6 RID: 998 RVA: 0x00016B3C File Offset: 0x00014D3C
			static ArrayAccessor()
			{
				DynamicMethod dynamicMethod = new DynamicMethod("get", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, CallingConventions.Standard, typeof(T[]), new Type[]
				{
					typeof(List<T>)
				}, typeof(NativeArrayUtility.ArrayAccessor<T>), true);
				ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, typeof(List<T>).GetField("_items", BindingFlags.Instance | BindingFlags.NonPublic));
				ilgenerator.Emit(OpCodes.Ret);
				NativeArrayUtility.ArrayAccessor<T>.Getter = (Func<List<T>, T[]>)dynamicMethod.CreateDelegate(typeof(Func<List<T>, T[]>));
			}

			// Token: 0x04000264 RID: 612
			public static readonly Func<List<T>, T[]> Getter;
		}
	}
}
