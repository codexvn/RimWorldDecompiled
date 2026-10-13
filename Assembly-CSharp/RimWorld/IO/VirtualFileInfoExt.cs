using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace RimWorld.IO
{
	// Token: 0x02003E9A RID: 16026
	public static class VirtualFileInfoExt
	{
		// Token: 0x06015F26 RID: 89894 RVA: 0x0068FCF0 File Offset: 0x0068DEF0
		public static XDocument LoadAsXDocument(this VirtualFile file)
		{
			XDocument result;
			using (Stream stream = file.CreateReadStream())
			{
				result = XDocument.Load(XmlReader.Create(stream), LoadOptions.SetLineInfo);
			}
			return result;
		}

		// Token: 0x06015F27 RID: 89895 RVA: 0x0068FD30 File Offset: 0x0068DF30
		public static XDocument LoadAsXDocument(string fileContents)
		{
			XDocument result;
			using (StringReader stringReader = new StringReader(fileContents))
			{
				result = XDocument.Load(stringReader, LoadOptions.SetLineInfo);
			}
			return result;
		}
	}
}
