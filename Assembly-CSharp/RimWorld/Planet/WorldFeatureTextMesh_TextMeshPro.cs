using System;
using LudeonTK;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003C44 RID: 15428
	[StaticConstructorOnStartup]
	public class WorldFeatureTextMesh_TextMeshPro : WorldFeatureTextMesh
	{
		// Token: 0x06014F06 RID: 85766 RVA: 0x006441E3 File Offset: 0x006423E3
		private static void TextScale_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170033CE RID: 13262
		// (get) Token: 0x06014F07 RID: 85767 RVA: 0x006449B6 File Offset: 0x00642BB6
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170033CF RID: 13263
		// (get) Token: 0x06014F08 RID: 85768 RVA: 0x006449C8 File Offset: 0x00642BC8
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170033D0 RID: 13264
		// (get) Token: 0x06014F09 RID: 85769 RVA: 0x006449DA File Offset: 0x00642BDA
		// (set) Token: 0x06014F0A RID: 85770 RVA: 0x006449E7 File Offset: 0x00642BE7
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

		// Token: 0x170033D1 RID: 13265
		// (get) Token: 0x06014F0B RID: 85771 RVA: 0x006449F5 File Offset: 0x00642BF5
		// (set) Token: 0x06014F0C RID: 85772 RVA: 0x00644A02 File Offset: 0x00642C02
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

		// Token: 0x170033D2 RID: 13266
		// (set) Token: 0x06014F0D RID: 85773 RVA: 0x00644A10 File Offset: 0x00642C10
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = value * WorldFeatureTextMesh_TextMeshPro.TextScale;
			}
		}

		// Token: 0x170033D3 RID: 13267
		// (get) Token: 0x06014F0E RID: 85774 RVA: 0x00644A24 File Offset: 0x00642C24
		// (set) Token: 0x06014F0F RID: 85775 RVA: 0x00644A36 File Offset: 0x00642C36
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

		// Token: 0x170033D4 RID: 13268
		// (get) Token: 0x06014F10 RID: 85776 RVA: 0x00644A49 File Offset: 0x00642C49
		// (set) Token: 0x06014F11 RID: 85777 RVA: 0x00644A5B File Offset: 0x00642C5B
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

		// Token: 0x06014F12 RID: 85778 RVA: 0x00644A6E File Offset: 0x00642C6E
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x06014F13 RID: 85779 RVA: 0x00644A81 File Offset: 0x00642C81
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x06014F14 RID: 85780 RVA: 0x00644A94 File Offset: 0x00642C94
		public override void Init()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.GetComponent<TextMeshPro>();
			this.Color = new Color(1f, 1f, 1f, 0f);
			Material[] sharedMaterials = this.textMesh.GetComponent<MeshRenderer>().sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i].renderQueue = 3610;
			}
		}

		// Token: 0x06014F15 RID: 85781 RVA: 0x00644B0C File Offset: 0x00642D0C
		public override void WrapAroundPlanetSurface(PlanetLayer layer)
		{
			this.textMesh.ForceMeshUpdate(false, false);
			TMP_TextInfo textInfo = this.textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount == 0)
			{
				return;
			}
			float num = this.textMesh.bounds.extents.x * 2f;
			float num2 = layer.DistOnSurfaceToAngle(num);
			Matrix4x4 localToWorldMatrix = this.textMesh.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = this.textMesh.transform.worldToLocalMatrix;
			for (int i = 0; i < characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible)
				{
					int materialReferenceIndex = this.textMesh.textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = tmp_CharacterInfo.vertexIndex;
					Vector3 vector = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3];
					vector /= 4f;
					float num3 = vector.x / (num / 2f);
					bool flag = num3 >= 0f;
					num3 = Mathf.Abs(num3);
					float num4 = num2 / 2f * num3;
					float num5 = (180f - num4) / 2f;
					float num6 = 200f * Mathf.Tan(num4 / 2f * 0.017453292f);
					Vector3 vector2 = new Vector3(Mathf.Sin(num5 * 0.017453292f) * num6 * (flag ? 1f : -1f), vector.y, Mathf.Cos(num5 * 0.017453292f) * num6);
					Vector3 b = vector2 - vector;
					Vector3 vector3 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + b;
					Vector3 vector4 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + b;
					Vector3 vector5 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + b;
					Vector3 vector6 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] + b;
					Quaternion rotation = Quaternion.Euler(0f, num4 * (flag ? -1f : 1f), 0f);
					vector3 = rotation * (vector3 - vector2) + vector2;
					vector4 = rotation * (vector4 - vector2) + vector2;
					vector5 = rotation * (vector5 - vector2) + vector2;
					vector6 = rotation * (vector6 - vector2) + vector2;
					vector3 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector3).normalized * (layer.Radius + 0.4f));
					vector4 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector4).normalized * (layer.Radius + 0.4f));
					vector5 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector5).normalized * (layer.Radius + 0.4f));
					vector6 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector6).normalized * (layer.Radius + 0.4f));
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] = vector3;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] = vector4;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] = vector5;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] = vector6;
				}
			}
			this.textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		// Token: 0x0400E967 RID: 59751
		private TextMeshPro textMesh;

		// Token: 0x0400E968 RID: 59752
		public static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		// Token: 0x0400E969 RID: 59753
		[TweakValue("Interface.World", 0f, 5f)]
		private static float TextScale = 1f;
	}
}
