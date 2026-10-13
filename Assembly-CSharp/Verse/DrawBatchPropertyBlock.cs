using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F6 RID: 1014
	public class DrawBatchPropertyBlock
	{
		// Token: 0x060019CA RID: 6602 RVA: 0x00082A11 File Offset: 0x00080C11
		public void Clear()
		{
			this.properties.Clear();
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x00082A1E File Offset: 0x00080C1E
		public void SetFloat(string name, float val)
		{
			this.SetFloat(Shader.PropertyToID(name), val);
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x00082A30 File Offset: 0x00080C30
		public void SetFloat(int propertyId, float val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Float,
				floatVal = val
			});
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x00082A69 File Offset: 0x00080C69
		public void SetColor(string name, Color val)
		{
			this.SetColor(Shader.PropertyToID(name), val);
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x00082A78 File Offset: 0x00080C78
		public void SetColor(int propertyId, Color val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Color,
				vectorVal = val
			});
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x00082AB6 File Offset: 0x00080CB6
		public void SetVector(string name, Vector4 val)
		{
			this.SetVector(Shader.PropertyToID(name), val);
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x00082AC8 File Offset: 0x00080CC8
		public void SetVector(int propertyId, Vector4 val)
		{
			this.properties.Add(new DrawBatchPropertyBlock.Property
			{
				propertyId = propertyId,
				type = DrawBatchPropertyBlock.PropertyType.Vector,
				vectorVal = val
			});
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x00082B04 File Offset: 0x00080D04
		public void Write(MaterialPropertyBlock propertyBlock)
		{
			foreach (DrawBatchPropertyBlock.Property property in this.properties)
			{
				property.Write(propertyBlock);
			}
		}

		// Token: 0x040016B9 RID: 5817
		private List<DrawBatchPropertyBlock.Property> properties = new List<DrawBatchPropertyBlock.Property>();

		// Token: 0x040016BA RID: 5818
		public string leakDebugString;

		// Token: 0x020003F7 RID: 1015
		private enum PropertyType
		{
			// Token: 0x040016BC RID: 5820
			Float,
			// Token: 0x040016BD RID: 5821
			Color,
			// Token: 0x040016BE RID: 5822
			Vector
		}

		// Token: 0x020003F8 RID: 1016
		private struct Property
		{
			// Token: 0x060019D3 RID: 6611 RVA: 0x00082B6C File Offset: 0x00080D6C
			public void Write(MaterialPropertyBlock propertyBlock)
			{
				switch (this.type)
				{
				case DrawBatchPropertyBlock.PropertyType.Float:
					propertyBlock.SetFloat(this.propertyId, this.floatVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Color:
					propertyBlock.SetColor(this.propertyId, this.vectorVal);
					return;
				case DrawBatchPropertyBlock.PropertyType.Vector:
					propertyBlock.SetVector(this.propertyId, this.vectorVal);
					return;
				default:
					return;
				}
			}

			// Token: 0x040016BF RID: 5823
			public int propertyId;

			// Token: 0x040016C0 RID: 5824
			public DrawBatchPropertyBlock.PropertyType type;

			// Token: 0x040016C1 RID: 5825
			public float floatVal;

			// Token: 0x040016C2 RID: 5826
			public Vector4 vectorVal;
		}
	}
}
