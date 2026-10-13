using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000895 RID: 2197
	[StaticConstructorOnStartup]
	public class Graphic_Fleck : Graphic_Single
	{
		// Token: 0x17000A2F RID: 2607
		// (get) Token: 0x060037B2 RID: 14258 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool AllowInstancing
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x00066B11 File Offset: 0x00064D11
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x00129A40 File Offset: 0x00127C40
		public virtual void DrawFleck(FleckDrawData drawData, DrawBatch batch)
		{
			Color value;
			if (drawData.overrideColor != null)
			{
				value = drawData.overrideColor.Value;
			}
			else
			{
				float alpha = drawData.alpha;
				if (alpha <= 0f)
				{
					if (drawData.propertyBlock != null)
					{
						batch.ReturnPropertyBlock(drawData.propertyBlock);
					}
					return;
				}
				value = base.Color * drawData.color;
				value.a *= alpha;
			}
			Vector3 scale = drawData.scale;
			scale.x *= this.data.drawSize.x;
			scale.z *= this.data.drawSize.y;
			Mesh mesh = MeshPool.plane10;
			float num = drawData.rotation;
			if (scale.x < 0f && scale.y >= 0f)
			{
				scale.x = -scale.x;
				mesh = MeshPool.plane10Flip;
			}
			else if (scale.x >= 0f && scale.y < 0f)
			{
				scale.y = -scale.y;
				mesh = MeshPool.plane10Flip;
				num += 180f;
			}
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(drawData.pos, Quaternion.AngleAxis(num, Vector3.up), scale);
			Material matSingle = this.MatSingle;
			batch.DrawMesh(mesh, matrix, matSingle, drawData.drawLayer, new Color?(value), this.data.renderInstanced && this.AllowInstancing, drawData.propertyBlock);
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x00129BBC File Offset: 0x00127DBC
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Fleck(path=";
			array[1] = this.path;
			array[2] = ", shader=";
			int num = 3;
			Shader shader = base.Shader;
			array[num] = ((shader != null) ? shader.ToString() : null);
			array[4] = ", color=";
			int num2 = 5;
			Color color = this.color;
			array[num2] = color.ToString();
			array[6] = ", colorTwo=unsupported)";
			return string.Concat(array);
		}
	}
}
