using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A72 RID: 6770
	public class RoomPartDef : Def
	{
		// Token: 0x1700173C RID: 5948
		// (get) Token: 0x060097EF RID: 38895 RVA: 0x002C59CC File Offset: 0x002C3BCC
		public RoomPartWorker Worker
		{
			get
			{
				RoomPartWorker result;
				if ((result = this.cachedWorker) == null)
				{
					result = (this.cachedWorker = (RoomPartWorker)Activator.CreateInstance(this.workerClass, new object[]
					{
						this
					}));
				}
				return result;
			}
		}

		// Token: 0x060097F0 RID: 38896 RVA: 0x002C5A06 File Offset: 0x002C3C06
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.workerClass == null || !this.workerClass.IsSubclassOf(typeof(RoomPartWorker)))
			{
				yield return "workerClass must be a subclass of RoomPartWorker";
			}
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040065A1 RID: 26017
		protected Type workerClass;

		// Token: 0x040065A2 RID: 26018
		private RoomPartWorker cachedWorker;
	}
}
