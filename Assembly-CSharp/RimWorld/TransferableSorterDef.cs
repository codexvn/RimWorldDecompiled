using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B21 RID: 6945
	public class TransferableSorterDef : Def
	{
		// Token: 0x170017DB RID: 6107
		// (get) Token: 0x06009B26 RID: 39718 RVA: 0x002D0B19 File Offset: 0x002CED19
		public TransferableComparer Comparer
		{
			get
			{
				if (this.comparerInt == null)
				{
					this.comparerInt = (TransferableComparer)Activator.CreateInstance(this.comparerClass);
				}
				return this.comparerInt;
			}
		}

		// Token: 0x04006B6D RID: 27501
		public Type comparerClass;

		// Token: 0x04006B6E RID: 27502
		[Unsaved(false)]
		private TransferableComparer comparerInt;
	}
}
