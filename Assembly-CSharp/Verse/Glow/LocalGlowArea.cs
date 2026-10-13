using System;
using LudeonTK;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace Verse.Glow
{
	// Token: 0x02000DF7 RID: 3575
	public struct LocalGlowArea : IDisposable
	{
		// Token: 0x06005AEC RID: 23276 RVA: 0x001D2EBC File Offset: 0x001D10BC
		public static LocalGlowArea AllocateNew()
		{
			LocalGlowArea result = new LocalGlowArea
			{
				inUse = false,
				colors = new UnsafeList<Color32>(6561, Allocator.Persistent, NativeArrayOptions.ClearMemory)
			};
			result.colors.Resize(6561, NativeArrayOptions.ClearMemory);
			return result;
		}

		// Token: 0x06005AED RID: 23277 RVA: 0x001D2F06 File Offset: 0x001D1106
		public void Take()
		{
			this.inUse = true;
		}

		// Token: 0x06005AEE RID: 23278 RVA: 0x001D2F0F File Offset: 0x001D110F
		public void Return()
		{
			NativeArrayUtility.MemClear<Color32>(this.colors);
			this.inUse = false;
		}

		// Token: 0x06005AEF RID: 23279 RVA: 0x001D2F23 File Offset: 0x001D1123
		public void Dispose()
		{
			ref this.colors.EnsureDisposed<Color32>();
		}

		// Token: 0x040041E0 RID: 16864
		public bool inUse;

		// Token: 0x040041E1 RID: 16865
		public UnsafeList<Color32> colors;
	}
}
