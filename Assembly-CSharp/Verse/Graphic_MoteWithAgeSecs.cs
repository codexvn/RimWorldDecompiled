using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008A5 RID: 2213
	public class Graphic_MoteWithAgeSecs : Graphic_Mote
	{
		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003803 RID: 14339 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool ForcePropertyBlock
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003804 RID: 14340 RVA: 0x0012B190 File Offset: 0x00129390
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Graphic_Mote.propertyBlock.SetColor(ShaderPropertyIDs.Color, this.color);
			if (thing == null)
			{
				bool? flag;
				if (thingDef == null)
				{
					flag = null;
				}
				else
				{
					MoteProperties mote = thingDef.mote;
					flag = ((mote != null) ? new bool?(mote.realTime) : null);
				}
				bool? flag2 = flag;
				bool valueOrDefault = flag2.GetValueOrDefault();
				TickManager tickManager = Find.TickManager;
				int? num = (tickManager != null) ? new int?(tickManager.TicksGame) : null;
				float valueOrDefault2 = ((num != null) ? new float?((float)num.GetValueOrDefault() / 60f) : null).GetValueOrDefault();
				Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, valueOrDefault ? Time.realtimeSinceStartup : valueOrDefault2);
				Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, valueOrDefault2);
				Graphic_Mote.DrawMote(this.data, this.MatSingle, loc, 0f, 0, true, null);
				return;
			}
			Mote mote2 = (Mote)thing;
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecs, mote2.AgeSecs);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.AgeSecsPausable, mote2.AgeSecsPausable);
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObject, (float)Gen.HashCombineInt(mote2.spawnTick, mote2.DrawPos.GetHashCode()));
			Graphic_Mote.propertyBlock.SetFloat(ShaderPropertyIDs.RandomPerObjectOffsetRandom, (float)Gen.HashCombineInt(mote2.spawnTick, mote2.offsetRandom));
			base.DrawMoteInternal(loc, rot, thingDef, thing, 0);
		}

		// Token: 0x06003805 RID: 14341 RVA: 0x0012B31C File Offset: 0x0012951C
		public override string ToString()
		{
			string[] array = new string[7];
			array[0] = "Graphic_MoteWithAgeSecs(path=";
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
