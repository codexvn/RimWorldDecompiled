using System;
using System.Xml;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000162 RID: 354
	public class ShaderParameter
	{
		// Token: 0x06000A87 RID: 2695 RVA: 0x00031C58 File Offset: 0x0002FE58
		public void Apply(Material mat)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				mat.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				mat.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce("Texture for " + this.name + " is not yet loaded; file may be invalid, or main thread may not have loaded it yet", 27929440);
				}
				mat.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x00031CF0 File Offset: 0x0002FEF0
		public void Apply(MaterialPropertyBlock block)
		{
			switch (this.type)
			{
			case ShaderParameter.Type.Float:
				block.SetFloat(this.name, this.value.x);
				return;
			case ShaderParameter.Type.Vector:
				block.SetVector(this.name, this.value);
				return;
			case ShaderParameter.Type.Matrix:
				break;
			case ShaderParameter.Type.Texture:
				if (this.valueTex == null)
				{
					Log.ErrorOnce("Texture for " + this.name + " is not yet loaded; file may be invalid, or main thread may not have loaded it yet", 27929440);
				}
				block.SetTexture(this.name, this.valueTex);
				break;
			default:
				return;
			}
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00031D88 File Offset: 0x0002FF88
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ShaderParameter: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			string valstr = xmlRoot.FirstChild.Value;
			if (!valstr.NullOrEmpty() && valstr[0] == '(')
			{
				this.value = ParseHelper.FromStringVector4Adaptive(valstr);
				this.type = ShaderParameter.Type.Vector;
				return;
			}
			if (!valstr.NullOrEmpty() && valstr[0] == '/')
			{
				LongEventHandler.ExecuteWhenFinished(delegate
				{
					this.valueTex = ContentFinder<Texture2D>.Get(valstr.TrimStart('/'), true);
				});
				this.type = ShaderParameter.Type.Texture;
				return;
			}
			this.value = Vector4.one * ParseHelper.FromString<float>(valstr);
			this.type = ShaderParameter.Type.Float;
		}

		// Token: 0x04000582 RID: 1410
		[NoTranslate]
		private string name;

		// Token: 0x04000583 RID: 1411
		private Vector4 value;

		// Token: 0x04000584 RID: 1412
		private Texture2D valueTex;

		// Token: 0x04000585 RID: 1413
		private ShaderParameter.Type type;

		// Token: 0x02000163 RID: 355
		private enum Type
		{
			// Token: 0x04000587 RID: 1415
			Float,
			// Token: 0x04000588 RID: 1416
			Vector,
			// Token: 0x04000589 RID: 1417
			Matrix,
			// Token: 0x0400058A RID: 1418
			Texture
		}
	}
}
