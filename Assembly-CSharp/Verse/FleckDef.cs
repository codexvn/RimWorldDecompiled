using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003F9 RID: 1017
	public class FleckDef : Def
	{
		// Token: 0x1700049D RID: 1181
		// (get) Token: 0x060019D4 RID: 6612 RVA: 0x00082BD0 File Offset: 0x00080DD0
		public float Lifetime
		{
			get
			{
				return this.fadeInTime + this.solidTime + this.fadeOutTime;
			}
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00082BE6 File Offset: 0x00080DE6
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.fleckSystemClass == null)
			{
				yield return "FleckDef without system class type set!";
			}
			else if (!typeof(FleckSystem).IsAssignableFrom(this.fleckSystemClass))
			{
				yield return "FleckDef has system class type assigned which is not assignable to FleckSystemBase!";
			}
			if (this.graphicData == null && this.randomGraphics.NullOrEmpty<GraphicData>())
			{
				yield return "Fleck graphic data and random graphics are null!";
			}
			else if (this.graphicData != null && !typeof(Graphic_Fleck).IsAssignableFrom(this.graphicData.graphicClass))
			{
				yield return "Fleck graphic class is not derived from Graphic_Fleck!";
			}
			else if (!this.randomGraphics.NullOrEmpty<GraphicData>())
			{
				if (this.randomGraphics.Any((GraphicData g) => !typeof(Graphic_Fleck).IsAssignableFrom(g.graphicClass)))
				{
					yield return "random fleck graphic class is not derived from Graphic_Fleck!";
				}
			}
			yield break;
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00082BF8 File Offset: 0x00080DF8
		public GraphicData GetGraphicData(int id)
		{
			if (this.graphicData != null)
			{
				return this.graphicData;
			}
			Rand.PushState(id);
			GraphicData result;
			try
			{
				result = this.randomGraphics.RandomElement<GraphicData>();
			}
			finally
			{
				Rand.PopState();
			}
			return result;
		}

		// Token: 0x040016C3 RID: 5827
		public Type fleckSystemClass;

		// Token: 0x040016C4 RID: 5828
		public AltitudeLayer altitudeLayer;

		// Token: 0x040016C5 RID: 5829
		public float altitudeLayerIncOffset;

		// Token: 0x040016C6 RID: 5830
		public bool drawGUIOverlay;

		// Token: 0x040016C7 RID: 5831
		public GraphicData graphicData;

		// Token: 0x040016C8 RID: 5832
		public List<GraphicData> randomGraphics;

		// Token: 0x040016C9 RID: 5833
		public bool realTime;

		// Token: 0x040016CA RID: 5834
		public bool attachedToHead;

		// Token: 0x040016CB RID: 5835
		public bool useAttachLink;

		// Token: 0x040016CC RID: 5836
		public float fadeInTime;

		// Token: 0x040016CD RID: 5837
		public float solidTime = 1f;

		// Token: 0x040016CE RID: 5838
		public float fadeOutTime;

		// Token: 0x040016CF RID: 5839
		public Vector3 acceleration = Vector3.zero;

		// Token: 0x040016D0 RID: 5840
		public FloatRange speedPerTime = FloatRange.Zero;

		// Token: 0x040016D1 RID: 5841
		public float growthRate;

		// Token: 0x040016D2 RID: 5842
		public List<CurvedScaler> scalers;

		// Token: 0x040016D3 RID: 5843
		public bool collide;

		// Token: 0x040016D4 RID: 5844
		public SoundDef landSound;

		// Token: 0x040016D5 RID: 5845
		public Vector3 unattachedDrawOffset;

		// Token: 0x040016D6 RID: 5846
		public Vector3 attachedDrawOffset;

		// Token: 0x040016D7 RID: 5847
		public bool rotateTowardsMoveDirection;

		// Token: 0x040016D8 RID: 5848
		public float rotateTowardsMoveDirectionExtraAngle;

		// Token: 0x040016D9 RID: 5849
		public bool drawOffscreen;

		// Token: 0x040016DA RID: 5850
		public FloatRange archHeight = FloatRange.Zero;

		// Token: 0x040016DB RID: 5851
		public FloatRange archDuration;

		// Token: 0x040016DC RID: 5852
		public SimpleCurve archCurve;

		// Token: 0x040016DD RID: 5853
		public Vector3 scalingAnchor = Vector3.one * 0.5f;
	}
}
