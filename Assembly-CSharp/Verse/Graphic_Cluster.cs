using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200088F RID: 2191
	public class Graphic_Cluster : Graphic_Collection
	{
		// Token: 0x17000A26 RID: 2598
		// (get) Token: 0x06003794 RID: 14228 RVA: 0x00129230 File Offset: 0x00127430
		public override Material MatSingle
		{
			get
			{
				return this.subGraphics[Rand.Range(0, this.subGraphics.Length)].MatSingle;
			}
		}

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x06003795 RID: 14229 RVA: 0x0012924C File Offset: 0x0012744C
		protected virtual float PositionVariance
		{
			get
			{
				return 0.45f;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06003796 RID: 14230 RVA: 0x00129253 File Offset: 0x00127453
		protected virtual float SizeVariance
		{
			get
			{
				return 0.2f;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06003797 RID: 14231 RVA: 0x0012925A File Offset: 0x0012745A
		private float SizeFactorMin
		{
			get
			{
				return 1f - this.SizeVariance;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x06003798 RID: 14232 RVA: 0x00129268 File Offset: 0x00127468
		private float SizeFactorMax
		{
			get
			{
				return 1f + this.SizeVariance;
			}
		}

		// Token: 0x06003799 RID: 14233 RVA: 0x00129276 File Offset: 0x00127476
		public override void DrawWorker(Vector3 loc, Rot4 rot, ThingDef thingDef, Thing thing, float extraRotation)
		{
			Log.ErrorOnce("Graphic_Cluster cannot draw realtime.", 9432243);
		}

		// Token: 0x0600379A RID: 14234 RVA: 0x00129287 File Offset: 0x00127487
		protected virtual Vector3 GetCenter(Thing thing, int index)
		{
			return thing.TrueCenter();
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x00129290 File Offset: 0x00127490
		protected virtual int ScatterCount(Thing thing)
		{
			Filth filth = thing as Filth;
			int result;
			if (filth == null)
			{
				result = 3;
			}
			else
			{
				result = filth.thickness;
			}
			return result;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x001292B4 File Offset: 0x001274B4
		public Graphic SubGraphicFor(Thing thing)
		{
			if (thing == null)
			{
				return this.subGraphics[0];
			}
			int num = thing.OverrideGraphicIndex ?? thing.thingIDNumber;
			return this.subGraphics[num % this.subGraphics.Length];
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x00129300 File Offset: 0x00127500
		public override void Print(SectionLayer layer, Thing thing, float extraRotation)
		{
			Rand.PushState(thing.Position.GetHashCode());
			int num = this.ScatterCount(thing);
			Filth filth = thing as Filth;
			for (int i = 0; i < num; i++)
			{
				Material matSingle = this.MatSingle;
				Vector3 center = this.GetCenter(thing, i) + new Vector3(Rand.Range(-this.PositionVariance, this.PositionVariance), 0f, Rand.Range(-this.PositionVariance, this.PositionVariance));
				Vector2 size = new Vector2(Rand.Range(this.data.drawSize.x * this.SizeFactorMin, this.data.drawSize.x * this.SizeFactorMax), Rand.Range(this.data.drawSize.y * this.SizeFactorMin, this.data.drawSize.y * this.SizeFactorMax));
				float rot = (float)Rand.RangeInclusive(0, 360) + extraRotation;
				if (((filth != null) ? filth.drawInstances : null) != null && filth.drawInstances.Count == num)
				{
					rot = filth.drawInstances[i].rotation;
				}
				bool flipUv = Rand.Value < 0.5f;
				Vector2[] uvs;
				Color32 color;
				Graphic.TryGetTextureAtlasReplacementInfo(matSingle, thing.def.category.ToAtlasGroup(), flipUv, true, out matSingle, out uvs, out color);
				Printer_Plane.PrintPlane(layer, center, size, matSingle, rot, flipUv, uvs, new Color32[]
				{
					color,
					color,
					color,
					color
				}, 0.01f, 0f);
			}
			Rand.PopState();
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x001294B4 File Offset: 0x001276B4
		public override string ToString()
		{
			string[] array = new string[5];
			array[0] = "Scatter(subGraphic[0]=";
			int num = 1;
			Graphic graphic = this.subGraphics[0];
			array[num] = ((graphic != null) ? graphic.ToString() : null);
			array[2] = ", count=";
			array[3] = this.subGraphics.Length.ToString();
			array[4] = ")";
			return string.Concat(array);
		}
	}
}
