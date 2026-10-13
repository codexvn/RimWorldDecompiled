using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E0 RID: 736
	public class ShaderTypeDef : Def
	{
		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001223 RID: 4643 RVA: 0x000546EA File Offset: 0x000528EA
		public Shader Shader
		{
			get
			{
				if (this.shaderInt == null)
				{
					this.shaderInt = ShaderDatabase.LoadShader(this);
				}
				return this.shaderInt;
			}
		}

		// Token: 0x0400105A RID: 4186
		[NoTranslate]
		public string shaderPath;

		// Token: 0x0400105B RID: 4187
		[NoTranslate]
		public string uiShaderPath;

		// Token: 0x0400105C RID: 4188
		[Unsaved(false)]
		private Shader shaderInt;
	}
}
