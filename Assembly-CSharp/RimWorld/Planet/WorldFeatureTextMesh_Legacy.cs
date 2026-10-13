using System;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C43 RID: 15427
	public class WorldFeatureTextMesh_Legacy : WorldFeatureTextMesh
	{
		// Token: 0x06014EF4 RID: 85748 RVA: 0x006441E3 File Offset: 0x006423E3
		private static void TextScaleFactor_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170033C7 RID: 13255
		// (get) Token: 0x06014EF5 RID: 85749 RVA: 0x006447F1 File Offset: 0x006429F1
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170033C8 RID: 13256
		// (get) Token: 0x06014EF6 RID: 85750 RVA: 0x00644803 File Offset: 0x00642A03
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170033C9 RID: 13257
		// (get) Token: 0x06014EF7 RID: 85751 RVA: 0x00644815 File Offset: 0x00642A15
		// (set) Token: 0x06014EF8 RID: 85752 RVA: 0x00644822 File Offset: 0x00642A22
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x170033CA RID: 13258
		// (get) Token: 0x06014EF9 RID: 85753 RVA: 0x00644830 File Offset: 0x00642A30
		// (set) Token: 0x06014EFA RID: 85754 RVA: 0x0064483D File Offset: 0x00642A3D
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x170033CB RID: 13259
		// (set) Token: 0x06014EFB RID: 85755 RVA: 0x0064484B File Offset: 0x00642A4B
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = Mathf.RoundToInt(value * WorldFeatureTextMesh_Legacy.TextScaleFactor);
			}
		}

		// Token: 0x170033CC RID: 13260
		// (get) Token: 0x06014EFC RID: 85756 RVA: 0x00644864 File Offset: 0x00642A64
		// (set) Token: 0x06014EFD RID: 85757 RVA: 0x00644876 File Offset: 0x00642A76
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x170033CD RID: 13261
		// (get) Token: 0x06014EFE RID: 85758 RVA: 0x00644889 File Offset: 0x00642A89
		// (set) Token: 0x06014EFF RID: 85759 RVA: 0x0064489B File Offset: 0x00642A9B
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x06014F00 RID: 85760 RVA: 0x006448AE File Offset: 0x00642AAE
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06014F01 RID: 85761 RVA: 0x006448C1 File Offset: 0x00642AC1
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06014F02 RID: 85762 RVA: 0x006448D4 File Offset: 0x00642AD4
		public override void Init()
		{
			GameObject gameObject = new GameObject("World feature name (legacy)");
			gameObject.layer = WorldCameraManager.WorldLayer;
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.AddComponent<TextMesh>();
			this.textMesh.color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.anchor = TextAnchor.MiddleCenter;
			this.textMesh.alignment = TextAlignment.Center;
			this.textMesh.GetComponent<MeshRenderer>().sharedMaterial.renderQueue = 3610;
			this.Color = new Color(1f, 1f, 1f, 0f);
			this.textMesh.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);
		}

		// Token: 0x06014F03 RID: 85763 RVA: 0x000026EA File Offset: 0x000008EA
		public override void WrapAroundPlanetSurface(PlanetLayer layer)
		{
		}

		// Token: 0x0400E962 RID: 59746
		private TextMesh textMesh;

		// Token: 0x0400E963 RID: 59747
		private const float TextScale = 0.23f;

		// Token: 0x0400E964 RID: 59748
		private const int MinFontSize = 13;

		// Token: 0x0400E965 RID: 59749
		private const int MaxFontSize = 40;

		// Token: 0x0400E966 RID: 59750
		[TweakValue("Interface.World", 0f, 10f)]
		private static float TextScaleFactor = 7.5f;
	}
}
