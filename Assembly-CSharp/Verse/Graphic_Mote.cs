using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A3 RID: 2211
	[StaticConstructorOnStartup]
	public class Graphic_Mote : Graphic_Single
	{
		// Token: 0x17000A3A RID: 2618
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool ForcePropertyBlock
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060037F6 RID: 14326 RVA: 0x0012AE86 File Offset: 0x00129086
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			this.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x060037F7 RID: 14327 RVA: 0x0012AE94 File Offset: 0x00129094
		public void DrawMoteInternal(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer)
		{
			Graphic_Mote.DrawMote(this.data, this.MatSingle, base.Color, loc, rot, thingDef, thing, 0, this.ForcePropertyBlock, null);
		}

		// Token: 0x060037F8 RID: 14328 RVA: 0x0012AEC8 File Offset: 0x001290C8
		public static void DrawMote(GraphicData data, Material material, Color color, Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, int layer, bool forcePropertyBlock = false, MaterialPropertyBlock overridePropertyBlock = null)
		{
			Mote mote = (Mote)thing;
			float alpha = mote.Alpha;
			if (alpha <= 0f)
			{
				return;
			}
			Color color2 = color * mote.instanceColor;
			color2.a *= alpha;
			Vector3 exactScale = mote.ExactScale;
			exactScale.x *= data.drawSize.x;
			exactScale.z *= data.drawSize.y;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(mote.DrawPos, Quaternion.AngleAxis(mote.exactRotation, Vector3.up), exactScale);
			if (!forcePropertyBlock && color2.IndistinguishableFrom(material.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color2);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, overridePropertyBlock ?? Graphic_Mote.propertyBlock);
		}

		// Token: 0x060037F9 RID: 14329 RVA: 0x0012AFB8 File Offset: 0x001291B8
		public static void DrawMote(GraphicData data, Material material, Vector3 loc, float rotation, int layer, bool forcePropertyBlock = false, MaterialPropertyBlock overridePropertyBlock = null)
		{
			Color color = data.color;
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(loc, Quaternion.AngleAxis(rotation, Vector3.up), new Vector3(data.drawSize.x, 1f, data.drawSize.y));
			if (!forcePropertyBlock && color.IndistinguishableFrom(material.color))
			{
				Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0);
				return;
			}
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, color);
			Graphics.DrawMesh(MeshPool.plane10, matrix, material, layer, null, 0, overridePropertyBlock ?? Graphic_Mote.propertyBlock);
		}

		// Token: 0x060037FA RID: 14330 RVA: 0x0012B058 File Offset: 0x00129258
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Mote(path=";
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

		// Token: 0x040029A9 RID: 10665
		protected static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
	}
}
