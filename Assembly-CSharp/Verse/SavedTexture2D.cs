using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020007FD RID: 2045
	public class SavedTexture2D : IExposable, IDisposable
	{
		// Token: 0x170009CD RID: 2509
		// (get) Token: 0x0600342B RID: 13355 RVA: 0x00111FDD File Offset: 0x001101DD
		// (set) Token: 0x0600342C RID: 13356 RVA: 0x00111FE5 File Offset: 0x001101E5
		public Texture2D Texture { get; private set; }

		// Token: 0x0600342D RID: 13357 RVA: 0x00111FEE File Offset: 0x001101EE
		public SavedTexture2D(Texture2D texture)
		{
			this.Texture = texture;
		}

		// Token: 0x0600342E RID: 13358 RVA: 0x00112000 File Offset: 0x00110200
		public void ExposeData()
		{
			int width = 0;
			int height = 0;
			byte textureFormat = 0;
			int mipCount = -1;
			bool linear = false;
			string name = "";
			int anisoLevel = 0;
			byte filterMode = 0;
			byte wrapModeU = 0;
			byte wrapModeV = 0;
			float mipMapBias = 0f;
			int requestedMipmapLevel = 0;
			bool ignoreMipmapLimit = false;
			int minimumMipmapLevel = 0;
			byte[] data = null;
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				width = this.Texture.width;
				height = this.Texture.height;
				textureFormat = (byte)this.Texture.format;
				mipCount = this.Texture.mipmapCount;
				linear = !this.Texture.isDataSRGB;
				name = this.Texture.name;
				anisoLevel = this.Texture.anisoLevel;
				filterMode = (byte)this.Texture.filterMode;
				wrapModeU = (byte)this.Texture.wrapModeU;
				wrapModeV = (byte)this.Texture.wrapModeV;
				mipMapBias = this.Texture.mipMapBias;
				requestedMipmapLevel = this.Texture.requestedMipmapLevel;
				ignoreMipmapLimit = this.Texture.ignoreMipmapLimit;
				minimumMipmapLevel = this.Texture.minimumMipmapLevel;
				data = this.Texture.GetRawTextureData();
			}
			Scribe_Values.Look<string>(ref name, "name", null, false);
			Scribe_Values.Look<int>(ref width, "width", 0, false);
			Scribe_Values.Look<int>(ref height, "height", 0, false);
			Scribe_Values.Look<byte>(ref textureFormat, "format", 0, false);
			Scribe_Values.Look<int>(ref mipCount, "mipCount", 0, false);
			Scribe_Values.Look<bool>(ref linear, "linear", false, false);
			Scribe_Values.Look<int>(ref anisoLevel, "anisoLevel", 0, false);
			Scribe_Values.Look<byte>(ref filterMode, "filterMode", 0, false);
			Scribe_Values.Look<byte>(ref wrapModeU, "wrapModeU", 0, false);
			Scribe_Values.Look<byte>(ref wrapModeV, "wrapModeV", 0, false);
			Scribe_Values.Look<float>(ref mipMapBias, "mipmapBias", 0f, false);
			Scribe_Values.Look<int>(ref requestedMipmapLevel, "requestedMipmapLevel", 0, false);
			Scribe_Values.Look<bool>(ref ignoreMipmapLimit, "ignoreMipmapLimit", false, false);
			Scribe_Values.Look<int>(ref minimumMipmapLevel, "minimumMipmapLevel", 0, false);
			DataExposeUtility.LookByteArray(ref data, "textureData");
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.Texture = new Texture2D(width, height, (TextureFormat)textureFormat, mipCount, linear);
				this.Texture.name = name;
				this.Texture.anisoLevel = anisoLevel;
				this.Texture.filterMode = (FilterMode)filterMode;
				this.Texture.wrapModeU = (TextureWrapMode)wrapModeU;
				this.Texture.wrapModeV = (TextureWrapMode)wrapModeV;
				this.Texture.mipMapBias = mipMapBias;
				this.Texture.requestedMipmapLevel = requestedMipmapLevel;
				this.Texture.ignoreMipmapLimit = ignoreMipmapLimit;
				this.Texture.minimumMipmapLevel = minimumMipmapLevel;
				this.Texture.LoadRawTextureData(data);
				this.Texture.Apply();
			}
		}

		// Token: 0x0600342F RID: 13359 RVA: 0x0011228C File Offset: 0x0011048C
		public static explicit operator SavedTexture2D(Texture2D tex)
		{
			return new SavedTexture2D(tex);
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x00112294 File Offset: 0x00110494
		public static explicit operator Texture2D(SavedTexture2D tex)
		{
			return tex.Texture;
		}

		// Token: 0x06003431 RID: 13361 RVA: 0x0011229C File Offset: 0x0011049C
		public override bool Equals(object obj)
		{
			SavedTexture2D savedTexture2D = obj as SavedTexture2D;
			return savedTexture2D != null && this.Texture.Equals(savedTexture2D.Texture);
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x001122C6 File Offset: 0x001104C6
		public override int GetHashCode()
		{
			return Gen.HashCombine<Texture2D>(74839, this.Texture);
		}

		// Token: 0x06003433 RID: 13363 RVA: 0x001122D8 File Offset: 0x001104D8
		public override string ToString()
		{
			return string.Format("SavedTexture({0})", this.Texture);
		}

		// Token: 0x06003434 RID: 13364 RVA: 0x001122EA File Offset: 0x001104EA
		public void Dispose()
		{
			UnityEngine.Object.Destroy(this.Texture);
		}
	}
}
