using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FE RID: 1022
	public interface IFleck
	{
		// Token: 0x060019EE RID: 6638
		void Setup(FleckCreationData creationData);

		// Token: 0x060019EF RID: 6639
		bool TimeInterval(float deltaTime, Map map);

		// Token: 0x060019F0 RID: 6640
		void Draw(DrawBatch batch);

		// Token: 0x060019F1 RID: 6641
		Vector3 GetPosition();
	}
}
