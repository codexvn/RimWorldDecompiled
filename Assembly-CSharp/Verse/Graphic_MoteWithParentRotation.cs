using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A6 RID: 2214
	public class Graphic_MoteWithParentRotation : Graphic_Mote
	{
		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003807 RID: 14343 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003808 RID: 14344 RVA: 0x0012B394 File Offset: 0x00129594
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			MoteAttached moteAttached = (MoteAttached)thing;
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			if (moteAttached != null && moteAttached.link1.Linked)
			{
				Graphic_Mote.propertyBlock.SetInt(ShaderPropertyIDs.Rotation, moteAttached.link1.Target.Thing.Rotation.AsInt);
			}
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06003809 RID: 14345 RVA: 0x0012B40C File Offset: 0x0012960C
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Graphic_MoteWithParentRotation(path=";
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
