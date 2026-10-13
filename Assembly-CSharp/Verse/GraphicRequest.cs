using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000883 RID: 2179
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x0600374D RID: 14157 RVA: 0x00128100 File Offset: 0x00126300
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
			this.texture = null;
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x00128170 File Offset: 0x00126370
		public GraphicRequest(Type graphicClass, Texture2D texture, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters, string maskPath)
		{
			this.graphicClass = graphicClass;
			this.texture = texture;
			this.maskPath = maskPath;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = (shaderParameters.NullOrEmpty<ShaderParameter>() ? null : shaderParameters);
			this.path = null;
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x001281E0 File Offset: 0x001263E0
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			return Gen.HashCombine<List<ShaderParameter>>(Gen.HashCombine<int>(Gen.HashCombine<GraphicData>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Vector2>(Gen.HashCombine<Shader>(Gen.HashCombine<string>(Gen.HashCombine<Texture2D>(Gen.HashCombine<string>(Gen.HashCombine<Type>(0, this.graphicClass), this.path), this.texture), this.maskPath), this.shader), this.drawSize), this.color), this.colorTwo), this.graphicData), this.renderQueue), this.shaderParameters);
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x0012827A File Offset: 0x0012647A
		public override string ToString()
		{
			return string.Format("{0}: {1}", this.graphicClass, this.path);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x00128292 File Offset: 0x00126492
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x001282AC File Offset: 0x001264AC
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.texture == other.texture && this.maskPath == other.maskPath && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x00128386 File Offset: 0x00126586
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x00128390 File Offset: 0x00126590
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400296B RID: 10603
		public Type graphicClass;

		// Token: 0x0400296C RID: 10604
		public Texture2D texture;

		// Token: 0x0400296D RID: 10605
		public string path;

		// Token: 0x0400296E RID: 10606
		public string maskPath;

		// Token: 0x0400296F RID: 10607
		public Shader shader;

		// Token: 0x04002970 RID: 10608
		public Vector2 drawSize;

		// Token: 0x04002971 RID: 10609
		public Color color;

		// Token: 0x04002972 RID: 10610
		public Color colorTwo;

		// Token: 0x04002973 RID: 10611
		public GraphicData graphicData;

		// Token: 0x04002974 RID: 10612
		public int renderQueue;

		// Token: 0x04002975 RID: 10613
		public List<ShaderParameter> shaderParameters;
	}
}
