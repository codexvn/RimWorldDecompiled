using System;
using System.IO;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000378 RID: 888
	public static class LanguageDataWriter
	{
		// Token: 0x060016BE RID: 5822 RVA: 0x00070F0C File Offset: 0x0006F10C
		public static void WriteBackstoryFile()
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(GenFilePaths.DevOutputFolderPath);
			if (!directoryInfo.Exists)
			{
				directoryInfo.Create();
			}
			if (new FileInfo(GenFilePaths.BackstoryOutputFilePath).Exists)
			{
				Find.WindowStack.Add(new Dialog_MessageBox("Cannot write: File already exists at " + GenFilePaths.BackstoryOutputFilePath, null, null, null, null, null, false, null, null, WindowLayer.Dialog));
				return;
			}
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			xmlWriterSettings.IndentChars = "\t";
			using (XmlWriter xmlWriter = XmlWriter.Create(GenFilePaths.BackstoryOutputFilePath, xmlWriterSettings))
			{
				xmlWriter.WriteStartDocument();
				xmlWriter.WriteStartElement("BackstoryTranslations");
				foreach (BackstoryDef backstoryDef in DefDatabase<BackstoryDef>.AllDefs)
				{
					xmlWriter.WriteStartElement(backstoryDef.identifier);
					xmlWriter.WriteElementString("title", backstoryDef.title);
					if (!backstoryDef.titleFemale.NullOrEmpty())
					{
						xmlWriter.WriteElementString("titleFemale", backstoryDef.titleFemale);
					}
					xmlWriter.WriteElementString("titleShort", backstoryDef.titleShort);
					if (!backstoryDef.titleShortFemale.NullOrEmpty())
					{
						xmlWriter.WriteElementString("titleShortFemale", backstoryDef.titleShortFemale);
					}
					xmlWriter.WriteElementString("desc", backstoryDef.description);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.WriteEndDocument();
			}
			Messages.Message("Fresh backstory translation file saved to " + GenFilePaths.BackstoryOutputFilePath, MessageTypeDefOf.NeutralEvent, false);
		}
	}
}
