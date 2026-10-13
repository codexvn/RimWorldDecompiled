using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Verse
{
	// Token: 0x02000837 RID: 2103
	public class LoadableXmlAsset
	{
		// Token: 0x170009E1 RID: 2529
		// (get) Token: 0x06003567 RID: 13671 RVA: 0x0011BDF0 File Offset: 0x00119FF0
		public string FullFilePath
		{
			get
			{
				return this.fullFolderPath + Path.DirectorySeparatorChar.ToString() + this.name;
			}
		}

		// Token: 0x06003568 RID: 13672 RVA: 0x0011BE1C File Offset: 0x0011A01C
		public LoadableXmlAsset(string name, string text)
		{
			this.name = name;
			this.fullFolderPath = string.Empty;
			try
			{
				using (StringReader stringReader = new StringReader(text))
				{
					using (XmlReader xmlReader = XmlReader.Create(stringReader, LoadableXmlAsset.Settings))
					{
						this.xmlDoc = new XmlDocument();
						this.xmlDoc.Load(xmlReader);
					}
				}
			}
			catch (Exception arg)
			{
				Log.Warning(string.Format("Exception reading {0} as XML: {1}", name, arg));
				this.xmlDoc = null;
			}
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x0011BEC8 File Offset: 0x0011A0C8
		public unsafe LoadableXmlAsset(FileInfo file, ModContentPack mod)
		{
			this.mod = mod;
			this.name = file.Name;
			this.fullFolderPath = file.Directory.FullName;
			try
			{
				using (MemoryMappedFileSpanWrapper memoryMappedFileSpanWrapper = new MemoryMappedFileSpanWrapper(file))
				{
					ReadOnlySpan<byte> readOnlySpan = memoryMappedFileSpanWrapper.GetReadOnlySpan(0L, memoryMappedFileSpanWrapper.FileSize);
					if (readOnlySpan.Length >= 3 && *readOnlySpan[0] == 239 && *readOnlySpan[1] == 187 && *readOnlySpan[2] == 191)
					{
						ReadOnlySpan<byte> readOnlySpan2 = readOnlySpan;
						readOnlySpan = readOnlySpan2.Slice(3, readOnlySpan2.Length - 3);
					}
					using (StringReader stringReader = new StringReader(Encoding.UTF8.GetString(readOnlySpan)))
					{
						using (XmlReader xmlReader = XmlReader.Create(stringReader, LoadableXmlAsset.Settings))
						{
							this.xmlDoc = new XmlDocument();
							this.xmlDoc.Load(xmlReader);
						}
					}
				}
			}
			catch (Exception arg)
			{
				Log.Warning(string.Format("Exception reading {0} as XML: {1}", this.name, arg));
				this.xmlDoc = null;
			}
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x0011C014 File Offset: 0x0011A214
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x0400283C RID: 10300
		public readonly string name;

		// Token: 0x0400283D RID: 10301
		public readonly string fullFolderPath;

		// Token: 0x0400283E RID: 10302
		public readonly XmlDocument xmlDoc;

		// Token: 0x0400283F RID: 10303
		public readonly ModContentPack mod;

		// Token: 0x04002840 RID: 10304
		private const int BufferSize = 16000;

		// Token: 0x04002841 RID: 10305
		private static readonly XmlReaderSettings Settings = new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreWhitespace = true,
			CheckCharacters = false,
			Async = false
		};
	}
}
