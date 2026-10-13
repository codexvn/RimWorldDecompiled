using System;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C00 RID: 15360
	public interface ISlateRef
	{
		// Token: 0x17003384 RID: 13188
		// (get) Token: 0x06014D4E RID: 85326
		// (set) Token: 0x06014D4F RID: 85327
		string SlateRef { get; set; }

		// Token: 0x06014D50 RID: 85328
		bool TryGetConvertedValue<T>(Slate slate, out T value);
	}
}
