using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000240 RID: 576
	public class AsymmetricLinkData
	{
		// Token: 0x040009D5 RID: 2517
		public LinkFlags linkFlags;

		// Token: 0x040009D6 RID: 2518
		public bool linkToDoors;

		// Token: 0x040009D7 RID: 2519
		public AsymmetricLinkData.BorderData drawDoorBorderEast;

		// Token: 0x040009D8 RID: 2520
		public AsymmetricLinkData.BorderData drawDoorBorderWest;

		// Token: 0x02000241 RID: 577
		public class BorderData
		{
			// Token: 0x17000266 RID: 614
			// (get) Token: 0x06000ED2 RID: 3794 RVA: 0x00045728 File Offset: 0x00043928
			public Material Mat
			{
				get
				{
					if (this.colorMat == null)
					{
						this.colorMat = SolidColorMaterials.SimpleSolidColorMaterial(this.color, false);
					}
					return this.colorMat;
				}
			}

			// Token: 0x040009D9 RID: 2521
			public Color color = Color.black;

			// Token: 0x040009DA RID: 2522
			public Vector2 size;

			// Token: 0x040009DB RID: 2523
			public Vector3 offset;

			// Token: 0x040009DC RID: 2524
			private Material colorMat;
		}
	}
}
