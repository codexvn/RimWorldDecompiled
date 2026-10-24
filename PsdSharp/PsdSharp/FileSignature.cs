namespace PsdSharp;

public class FileSignature
{
	public readonly PsdFileType FileType;

	public readonly string Extension;

	public readonly byte[] Signature;

	public ushort FileVersion;

	public static FileSignature Psd = new FileSignature(PsdFileType.Psd, ".psd", new byte[4] { 56, 66, 80, 83 }, 1);

	public static FileSignature Psb = new FileSignature(PsdFileType.Psb, ".psb", new byte[4] { 56, 66, 80, 83 }, 2);

	private FileSignature(PsdFileType fileType, string extension, byte[] signature, ushort fileVersion)
	{
		FileType = fileType;
		Extension = extension;
		Signature = signature;
	}
}
