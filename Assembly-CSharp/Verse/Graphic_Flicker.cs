using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000898 RID: 2200
	public class Graphic_Flicker : Graphic_Collection
	{
		// Token: 0x17000A31 RID: 2609
		// (get) Token: 0x060037BE RID: 14270 RVA: 0x00129230 File Offset: 0x00127430
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x060037BF RID: 14271 RVA: 0x00129DF4 File Offset: 0x00127FF4
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			if (thingDef == null)
			{
				string str = "Fire DrawWorker with null thingDef: ";
				Vector3 vector = loc;
				Log.ErrorOnce(str + vector.ToString(), 3427324);
				return;
			}
			if (this.subGraphics == null)
			{
				Log.ErrorOnce("Graphic_Flicker has no subgraphics " + ((thingDef != null) ? thingDef.ToString() : null), 358773632);
				return;
			}
			int num = Find.TickManager.TicksGame;
			if (thing != null)
			{
				num += Mathf.Abs(thing.thingIDNumber ^ 8453458);
			}
			int num2 = num / 15;
			int num3 = Mathf.Abs(num2 ^ ((thing != null) ? thing.thingIDNumber : 0) * 391) % this.subGraphics.Length;
			float num4 = 1f;
			CompFireOverlayBase compFireOverlayBase = null;
			Fire fire = thing as Fire;
			CompProperties_FireOverlay compProperties = thingDef.GetCompProperties<CompProperties_FireOverlay>();
			if (fire != null)
			{
				num4 = fire.fireSize;
			}
			else if (thing != null)
			{
				compFireOverlayBase = thing.TryGetComp<CompFireOverlayBase>();
				if (compFireOverlayBase != null)
				{
					num4 = compFireOverlayBase.FireSize;
				}
				else
				{
					compFireOverlayBase = thing.TryGetComp<CompDarklightOverlay>();
					if (compFireOverlayBase != null)
					{
						num4 = compFireOverlayBase.FireSize;
					}
				}
			}
			else if (compProperties != null)
			{
				num4 = compProperties.fireSize;
			}
			if (num3 < 0 || num3 >= this.subGraphics.Length)
			{
				Log.ErrorOnce("Fire drawing out of range: " + num3.ToString(), 7453435);
				num3 = 0;
			}
			Graphic graphic = this.subGraphics[num3];
			float num5 = (compFireOverlayBase == null) ? Mathf.Min(num4 / 1.2f, 1.2f) : num4;
			Vector3 a = GenRadial.RadialPattern[num2 % GenRadial.RadialPattern.Length].ToVector3() / GenRadial.MaxRadialPatternRadius;
			a *= 0.05f;
			Vector3 vector2 = loc + a * num4;
			bool flag;
			if (thing == null)
			{
				flag = (null != null);
			}
			else
			{
				Graphic graphic2 = thing.Graphic;
				flag = (((graphic2 != null) ? graphic2.data : null) != null);
			}
			if (flag)
			{
				vector2 += thing.Graphic.data.DrawOffsetForRot(rot);
			}
			if (compFireOverlayBase != null)
			{
				vector2 += compFireOverlayBase.Props.DrawOffsetForRot(rot);
			}
			Vector3 s = new Vector3(num5, 1f, num5);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(vector2, Quaternion.identity, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, graphic.MatSingle, 0);
		}

		// Token: 0x060037C0 RID: 14272 RVA: 0x0012A028 File Offset: 0x00128228
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "Flicker(subGraphic[0]=";
			int num = 1;
			Graphic graphic = this.subGraphics[0];
			array[num] = ((graphic != null) ? graphic.ToString() : null);
			array[2] = ", count=";
			array[3] = this.subGraphics.Length.ToString();
			array[4] = ")";
			return string.Concat(array);
		}

		// Token: 0x04002996 RID: 10646
		private const int BaseTicksPerFrameChange = 15;

		// Token: 0x04002997 RID: 10647
		private const float MaxOffset = 0.05f;
	}
}
