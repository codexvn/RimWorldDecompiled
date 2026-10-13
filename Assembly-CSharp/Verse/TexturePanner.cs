using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000545 RID: 1349
	public class TexturePanner
	{
		// Token: 0x06002306 RID: 8966 RVA: 0x000B67BC File Offset: 0x000B49BC
		public TexturePanner(Material material, Vector2 direction, float speed) : this(material, Shader.PropertyToID("_MainTex"), direction, speed)
		{
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000B67D1 File Offset: 0x000B49D1
		public TexturePanner(Material material, string property, Vector2 direction, float speed) : this(material, Shader.PropertyToID(property), direction, speed)
		{
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x000B67E3 File Offset: 0x000B49E3
		public TexturePanner(Material material, int propertyID, Vector2 direction, float speed)
		{
			this.material = material;
			this.propertyID = propertyID;
			this.direction = direction;
			this.direction.Normalize();
			this.speed = speed;
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x000B6814 File Offset: 0x000B4A14
		public virtual void Tick()
		{
			this.pan -= this.direction * this.speed * this.material.GetTextureScale(this.propertyID).x * Find.TickManager.TickRateMultiplier;
			this.material.SetTextureOffset(this.propertyID, this.pan);
		}

		// Token: 0x04001D44 RID: 7492
		protected Material material;

		// Token: 0x04001D45 RID: 7493
		protected int propertyID;

		// Token: 0x04001D46 RID: 7494
		protected Vector2 pan;

		// Token: 0x04001D47 RID: 7495
		public Vector2 direction;

		// Token: 0x04001D48 RID: 7496
		public float speed;
	}
}
