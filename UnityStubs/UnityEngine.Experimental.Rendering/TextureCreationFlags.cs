using System;

namespace UnityEngine.Experimental.Rendering;

[Flags]
public enum TextureCreationFlags
{
	Crunch = 0x40,
	DontInitializePixels = 4,
	DontUploadUponCreate = 0x400,
	IgnoreMipmapLimit = 0x800,
	MipChain = 1,
	None = 0
}
