using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000812 RID: 2066
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x170009D8 RID: 2520
		// (set) Token: 0x06003493 RID: 13459 RVA: 0x0011400F File Offset: 0x0011220F
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06003494 RID: 13460 RVA: 0x00114020 File Offset: 0x00112220
		public MaterialRequest(Texture tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = true;
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x00114078 File Offset: 0x00112278
		public MaterialRequest(Texture tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = true;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x001140CC File Offset: 0x001122CC
		public MaterialRequest(Texture tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = true;
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x0011411C File Offset: 0x0011231C
		public MaterialRequest(Shader shader)
		{
			this.shader = shader;
			this.mainTex = null;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
			this.secondaryTex = null;
			this.needsMainTex = false;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x00114170 File Offset: 0x00112370
		public override int GetHashCode()
		{
			return Gen.HashCombine<Texture>(Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombineInt(Gen.HashCombine<Texture2D>(Gen.HashCombine<Texture>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<Shader>(0, this.shader), this.color), this.colorTwo), this.mainTex), this.maskTex), this.renderQueue), this.shaderParameters), this.secondaryTex);
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x001141D8 File Offset: 0x001123D8
		public override bool Equals(object obj)
		{
			if (obj is MaterialRequest)
			{
				MaterialRequest other = (MaterialRequest)obj;
				return this.Equals(other);
			}
			return false;
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x00114200 File Offset: 0x00112400
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.secondaryTex == this.secondaryTex && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x0011429D File Offset: 0x0011249D
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x001142A7 File Offset: 0x001124A7
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x001142B4 File Offset: 0x001124B4
		public override string ToString()
		{
			string[] array = new string[15];
			array[0] = "MaterialRequest(";
			array[1] = this.shader.name;
			array[2] = ", ";
			array[3] = this.mainTex.name;
			array[4] = ", ";
			array[5] = this.color.ToString();
			array[6] = ", ";
			array[7] = this.colorTwo.ToString();
			array[8] = ", ";
			int num = 9;
			Texture2D texture2D = this.maskTex;
			array[num] = ((texture2D != null) ? texture2D.ToString() : null);
			array[10] = ", ";
			int num2 = 11;
			Texture texture = this.secondaryTex;
			array[num2] = ((texture != null) ? texture.ToString() : null);
			array[12] = ", ";
			array[13] = this.renderQueue.ToString();
			array[14] = ")";
			return string.Concat(array);
		}

		// Token: 0x0400279B RID: 10139
		public Shader shader;

		// Token: 0x0400279C RID: 10140
		public Texture mainTex;

		// Token: 0x0400279D RID: 10141
		public Color color;

		// Token: 0x0400279E RID: 10142
		public Color colorTwo;

		// Token: 0x0400279F RID: 10143
		public Texture2D maskTex;

		// Token: 0x040027A0 RID: 10144
		public Texture secondaryTex;

		// Token: 0x040027A1 RID: 10145
		public int renderQueue;

		// Token: 0x040027A2 RID: 10146
		public bool needsMainTex;

		// Token: 0x040027A3 RID: 10147
		public List<ShaderParameter> shaderParameters;
	}
}
