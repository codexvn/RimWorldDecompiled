using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02003C42 RID: 15426
	public abstract class WorldFeatureTextMesh
	{
		// Token: 0x170033C0 RID: 13248
		// (get) Token: 0x06014EE4 RID: 85732
		public abstract bool Active { get; }

		// Token: 0x170033C1 RID: 13249
		// (get) Token: 0x06014EE5 RID: 85733
		public abstract Vector3 Position { get; }

		// Token: 0x170033C2 RID: 13250
		// (get) Token: 0x06014EE6 RID: 85734
		// (set) Token: 0x06014EE7 RID: 85735
		public abstract Color Color { get; set; }

		// Token: 0x170033C3 RID: 13251
		// (get) Token: 0x06014EE8 RID: 85736
		// (set) Token: 0x06014EE9 RID: 85737
		public abstract string Text { get; set; }

		// Token: 0x170033C4 RID: 13252
		// (set) Token: 0x06014EEA RID: 85738
		public abstract float Size { set; }

		// Token: 0x170033C5 RID: 13253
		// (get) Token: 0x06014EEB RID: 85739
		// (set) Token: 0x06014EEC RID: 85740
		public abstract Quaternion Rotation { get; set; }

		// Token: 0x170033C6 RID: 13254
		// (get) Token: 0x06014EED RID: 85741
		// (set) Token: 0x06014EEE RID: 85742
		public abstract Vector3 LocalPosition { get; set; }

		// Token: 0x06014EEF RID: 85743
		public abstract void SetActive(bool active);

		// Token: 0x06014EF0 RID: 85744
		public abstract void Destroy();

		// Token: 0x06014EF1 RID: 85745
		public abstract void Init();

		// Token: 0x06014EF2 RID: 85746
		public abstract void WrapAroundPlanetSurface(PlanetLayer layer);
	}
}
