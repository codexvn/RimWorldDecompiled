using System;
using System.Runtime.CompilerServices;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C2 RID: 450
	public static class ToilMaker
	{
		// Token: 0x06000C66 RID: 3174 RVA: 0x0003A92C File Offset: 0x00038B2C
		public static Toil MakeToil([CallerMemberName] string debugName = null)
		{
			Toil toil = SimplePool<Toil>.Get();
			if (!toil.inPool)
			{
				Log.Error("Toil was marked as not in pool while coming from pool");
			}
			toil.inPool = false;
			toil.debugName = debugName;
			return toil;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0003A953 File Offset: 0x00038B53
		public static void ReturnToPool(this Toil toil)
		{
			toil.Clear();
			if (toil.inPool)
			{
				Log.Error("Toil was marked as already being in the pool while being returned to pool");
				return;
			}
			if (SimplePool<Toil>.FreeItemsCount >= 5000)
			{
				return;
			}
			toil.inPool = true;
			SimplePool<Toil>.Return(toil);
		}

		// Token: 0x04000717 RID: 1815
		private const int MaxToilPoolSize = 5000;
	}
}
