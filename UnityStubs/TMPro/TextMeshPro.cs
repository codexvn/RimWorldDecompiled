using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityStubs.Gui;
using Waylith.Rendering;

namespace TMPro;

public class TextMeshPro : TMP_Text
{
	private Mesh _mesh;

	private UnityEngine.Material _material;

	private string _layoutText;

	private float _layoutFontSize;

	private int _layoutFontIndex;

	private int _layoutAtlasVersion;

	private Color32 _layoutColor;

	private Vector3[] _flatVerts;

	private Vector3[] _uploadedVerts;

	public new Transform transform => base.transform;

	public override Mesh mesh => _mesh;

	public UnityEngine.Material fontMaterial => _material;

	private TMP_FontAsset ResolvedFont => font ?? TMP_FontAsset.Default;

	public void UpdateFontAsset()
	{
	}

	private void EnsureRenderObjects()
	{
		if ((object)_mesh == null)
		{
			_mesh = new Mesh
			{
				name = "TextMeshPro Mesh"
			};
		}
		if ((object)_material == null)
		{
			_material = BuildMaterial();
		}
		MeshFilter component = GetComponent<MeshFilter>();
		if (component != null)
		{
			component.sharedMesh = _mesh;
		}
		MeshRenderer component2 = GetComponent<MeshRenderer>();
		if (component2 != null)
		{
			component2.DrawnByOwner = true;
		}
		if (component2 != null && (component2.sharedMaterials == null || component2.sharedMaterials.Length == 0))
		{
			component2.sharedMaterials = new UnityEngine.Material[1] { _material };
		}
	}

	private UnityEngine.Material BuildMaterial()
	{
		UnityEngine.Material material = new UnityEngine.Material(UnityEngine.Shader.FromEngineShader("WorldText", ShaderRegistry.Resolve("WorldText")))
		{
			name = "TextMeshPro Material"
		};
		UnityEngine.Texture2D texture2D = TextMeshProBackend.Current?.GetAtlasTexture(ResolvedFont.fontIndex);
		if (texture2D != null)
		{
			material.mainTexture = texture2D;
		}
		return material;
	}

	private void LateUpdate()
	{
		if (_mesh == null || _material == null || !base.gameObject.activeInHierarchy)
		{
			return;
		}
		TMP_MeshInfo[] array = textInfo?.meshInfo;
		if (array != null && array.Length != 0 && array[0].vertexCount != 0)
		{
			ITextMeshProBackend current = TextMeshProBackend.Current;
			if (current != null && current.AtlasVersion != _layoutAtlasVersion)
			{
				RefreshUvs(current, array[0]);
			}
			_material.color = color;
			UnityEngine.Graphics.DrawMesh(_mesh, transform.localToWorldMatrix, _material, base.gameObject.layer, null);
		}
	}

	public override void ForceMeshUpdate(bool ignoreActiveState = false, bool forceTextReparsing = false)
	{
		EnsureRenderObjects();
		if (!TryRestoreFlatLayout())
		{
			GenerateTextMesh();
		}
	}

	private bool TryRestoreFlatLayout()
	{
		if (_flatVerts == null || _layoutText != (text ?? string.Empty) || _layoutFontSize != fontSize || _layoutFontIndex != ResolvedFont.fontIndex || TextMeshProBackend.Current?.AtlasVersion != _layoutAtlasVersion)
		{
			return false;
		}
		TMP_MeshInfo[] array = textInfo?.meshInfo;
		if (array == null || array.Length == 0 || array[0].vertices == null || array[0].vertices.Length != _flatVerts.Length)
		{
			return false;
		}
		Array.Copy(_flatVerts, array[0].vertices, _flatVerts.Length);
		Color32 color = this.color;
		if (!color.Equals(_layoutColor) && array[0].colors32 != null)
		{
			Array.Fill(array[0].colors32, color);
			_layoutColor = color;
		}
		return true;
	}

	private void RefreshUvs(ITextMeshProBackend backend, TMP_MeshInfo mi)
	{
		_layoutAtlasVersion = backend.AtlasVersion;
		Vector2[] uvs = mi.uvs0;
		TMP_CharacterInfo[] characterInfo = textInfo.characterInfo;
		if (_mesh == null || uvs == null || characterInfo == null || uvs.Length != mi.vertexCount)
		{
			return;
		}
		for (int i = 0; i < textInfo.characterCount; i++)
		{
			TMP_CharacterInfo tMP_CharacterInfo = characterInfo[i];
			if (tMP_CharacterInfo.isVisible && backend.TryGetGlyph(_layoutFontIndex, tMP_CharacterInfo.character, out var glyph) && glyph.found)
			{
				int vertexIndex = tMP_CharacterInfo.vertexIndex;
				uvs[vertexIndex] = new Vector2(glyph.u0, glyph.v1);
				uvs[vertexIndex + 1] = new Vector2(glyph.u0, glyph.v0);
				uvs[vertexIndex + 2] = new Vector2(glyph.u1, glyph.v0);
				uvs[vertexIndex + 3] = new Vector2(glyph.u1, glyph.v1);
			}
		}
		_mesh.uv = uvs;
		_mesh.UploadMeshData(markNoLongerReadable: false);
	}

	private static bool SameVerts(Vector3[] a, Vector3[] b)
	{
		if (a != null && b != null)
		{
			return MemoryMarshal.AsBytes((ReadOnlySpan<Vector3>)a).SequenceEqual(MemoryMarshal.AsBytes((ReadOnlySpan<Vector3>)b));
		}
		return false;
	}

	private void RememberUploaded(Vector3[] verts)
	{
		if (_uploadedVerts == null || _uploadedVerts.Length != verts.Length)
		{
			_uploadedVerts = new Vector3[verts.Length];
		}
		Array.Copy(verts, _uploadedVerts, verts.Length);
	}

	public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
	{
		if (!(_mesh == null) && textInfo?.meshInfo != null && textInfo.meshInfo.Length != 0)
		{
			TMP_MeshInfo tMP_MeshInfo = textInfo.meshInfo[0];
			if (tMP_MeshInfo.vertices != null && tMP_MeshInfo.vertices.Length != 0 && !SameVerts(tMP_MeshInfo.vertices, _uploadedVerts))
			{
				_mesh.vertices = tMP_MeshInfo.vertices;
				_mesh.RecalculateBounds();
				_mesh.UploadMeshData(markNoLongerReadable: false);
				RememberUploaded(tMP_MeshInfo.vertices);
			}
		}
	}

	private void GenerateTextMesh()
	{
		if (textInfo == null)
		{
			TMP_TextInfo tMP_TextInfo2 = (textInfo = new TMP_TextInfo());
		}
		_flatVerts = null;
		string text = this.text ?? string.Empty;
		int length = text.Length;
		if (textInfo.characterInfo == null || textInfo.characterInfo.Length < length)
		{
			textInfo.characterInfo = new TMP_CharacterInfo[Math.Max(length, 8)];
		}
		if (textInfo.meshInfo == null || textInfo.meshInfo.Length < 1)
		{
			textInfo.meshInfo = new TMP_MeshInfo[1];
		}
		textInfo.characterCount = length;
		ITextMeshProBackend current = TextMeshProBackend.Current;
		int fontIndex = ResolvedFont.fontIndex;
		if (current == null || !current.TryGetFontMetrics(fontIndex, out var metrics) || !metrics.found)
		{
			EmitEmpty(text, length);
			return;
		}
		float num = ((metrics.bakedPixelSize > 0f) ? metrics.bakedPixelSize : 1f);
		float num2 = ((fontSize <= 0f) ? 1f : fontSize) / num * 0.1f;
		List<float> list = new List<float>();
		int num3 = 0;
		float num4 = 0f;
		for (int i = 0; i < length; i++)
		{
			char c = text[i];
			TmpGlyphMetrics glyph;
			if (c == '\n')
			{
				list.Add(num4);
				num4 = 0f;
			}
			else if (current.TryGetGlyph(fontIndex, c, out glyph) && glyph.found)
			{
				num4 += glyph.advanceX * num2;
				if (glyph.visible && glyph.x1 > glyph.x0 && glyph.y1 > glyph.y0)
				{
					num3++;
				}
			}
		}
		list.Add(num4);
		int count = list.Count;
		float num5 = (metrics.ascent - metrics.descent) * num2;
		float num6 = (metrics.ascent - metrics.descent) * 0.5f * num2;
		Vector3[] array = new Vector3[num3 * 4];
		Vector2[] array2 = new Vector2[num3 * 4];
		Color32[] array3 = new Color32[num3 * 4];
		int[] array4 = new int[num3 * 6];
		Color32 color = this.color;
		Vector3 vector = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Vector3 vector2 = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		bool flag = false;
		int num7 = 0;
		int num8 = 0;
		float num9 = (0f - list[0]) * 0.5f;
		float num10 = (float)(count - 1) * 0.5f * num5;
		for (int j = 0; j < length; j++)
		{
			char c2 = text[j];
			if (c2 == '\n')
			{
				textInfo.characterInfo[j] = new TMP_CharacterInfo
				{
					character = c2,
					isVisible = false,
					materialReferenceIndex = 0,
					vertexIndex = 0
				};
				num8++;
				num9 = (0f - list[num8]) * 0.5f;
				num10 = (float)(count - 1) * 0.5f * num5 - (float)num8 * num5;
				continue;
			}
			int num11;
			int num12;
			if (current.TryGetGlyph(fontIndex, c2, out var glyph2))
			{
				num11 = (glyph2.found ? 1 : 0);
				if (num11 != 0 && glyph2.visible && glyph2.x1 > glyph2.x0)
				{
					num12 = ((glyph2.y1 > glyph2.y0) ? 1 : 0);
					goto IL_034f;
				}
			}
			else
			{
				num11 = 0;
			}
			num12 = 0;
			goto IL_034f;
			IL_034f:
			bool flag2 = (byte)num12 != 0;
			TMP_CharacterInfo tMP_CharacterInfo = new TMP_CharacterInfo
			{
				character = c2,
				isVisible = flag2,
				materialReferenceIndex = 0
			};
			if (flag2)
			{
				int num13 = (tMP_CharacterInfo.vertexIndex = num7 * 4);
				float x = num9 + glyph2.x0 * num2;
				float x2 = num9 + glyph2.x1 * num2;
				float y = (metrics.ascent - glyph2.y0) * num2 - num6 + num10;
				float y2 = (metrics.ascent - glyph2.y1) * num2 - num6 + num10;
				Vector3 vector3 = new Vector3(x, y2, 0f);
				Vector3 vector4 = new Vector3(x, y, 0f);
				Vector3 vector5 = new Vector3(x2, y, 0f);
				Vector3 vector6 = new Vector3(x2, y2, 0f);
				array[num13] = vector3;
				array[num13 + 1] = vector4;
				array[num13 + 2] = vector5;
				array[num13 + 3] = vector6;
				array2[num13] = new Vector2(glyph2.u0, glyph2.v1);
				array2[num13 + 1] = new Vector2(glyph2.u0, glyph2.v0);
				array2[num13 + 2] = new Vector2(glyph2.u1, glyph2.v0);
				array2[num13 + 3] = new Vector2(glyph2.u1, glyph2.v1);
				array3[num13] = color;
				array3[num13 + 1] = color;
				array3[num13 + 2] = color;
				array3[num13 + 3] = color;
				int num14 = num7 * 6;
				array4[num14] = num13;
				array4[num14 + 1] = num13 + 1;
				array4[num14 + 2] = num13 + 2;
				array4[num14 + 3] = num13;
				array4[num14 + 4] = num13 + 2;
				array4[num14 + 5] = num13 + 3;
				vector = Vector3.Min(vector, Vector3.Min(vector3, Vector3.Min(vector4, Vector3.Min(vector5, vector6))));
				vector2 = Vector3.Max(vector2, Vector3.Max(vector3, Vector3.Max(vector4, Vector3.Max(vector5, vector6))));
				flag = true;
				num7++;
			}
			else
			{
				tMP_CharacterInfo.vertexIndex = 0;
			}
			textInfo.characterInfo[j] = tMP_CharacterInfo;
			if (num11 != 0)
			{
				num9 += glyph2.advanceX * num2;
			}
		}
		textInfo.meshInfo[0] = new TMP_MeshInfo
		{
			vertices = array,
			uvs0 = array2,
			colors32 = array3,
			triangles = array4,
			vertexCount = array.Length
		};
		bounds = (flag ? new Bounds((vector + vector2) * 0.5f, vector2 - vector) : new Bounds(Vector3.zero, Vector3.zero));
		_layoutText = text;
		_layoutFontSize = fontSize;
		_layoutFontIndex = fontIndex;
		_layoutAtlasVersion = current.AtlasVersion;
		_layoutColor = color;
		_flatVerts = (Vector3[])array.Clone();
		ApplyToMesh();
	}

	private void EmitEmpty(string s, int n)
	{
		for (int i = 0; i < n; i++)
		{
			textInfo.characterInfo[i] = new TMP_CharacterInfo
			{
				character = s[i],
				isVisible = false
			};
		}
		textInfo.meshInfo[0] = new TMP_MeshInfo
		{
			vertices = Array.Empty<Vector3>(),
			uvs0 = Array.Empty<Vector2>(),
			colors32 = Array.Empty<Color32>(),
			triangles = Array.Empty<int>(),
			vertexCount = 0
		};
		bounds = new Bounds(Vector3.zero, Vector3.zero);
		_mesh?.Clear();
		_uploadedVerts = null;
	}

	private void ApplyToMesh()
	{
		if (!(_mesh == null))
		{
			TMP_MeshInfo tMP_MeshInfo = textInfo.meshInfo[0];
			_mesh.Clear();
			_uploadedVerts = null;
			if (tMP_MeshInfo.vertices != null && tMP_MeshInfo.vertices.Length != 0)
			{
				_mesh.vertices = tMP_MeshInfo.vertices;
				_mesh.uv = tMP_MeshInfo.uvs0;
				_mesh.triangles = tMP_MeshInfo.triangles;
				_mesh.RecalculateBounds();
				_mesh.UploadMeshData(markNoLongerReadable: false);
				RememberUploaded(tMP_MeshInfo.vertices);
			}
		}
	}
}
