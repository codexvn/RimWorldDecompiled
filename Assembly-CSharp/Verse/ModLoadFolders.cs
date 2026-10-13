using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200057F RID: 1407
	public class ModLoadFolders
	{
		// Token: 0x060024E7 RID: 9447 RVA: 0x000BE9A8 File Offset: 0x000BCBA8
		public List<LoadFolder> FoldersForVersion(string version)
		{
			if (this.foldersForVersion.ContainsKey(version))
			{
				return this.foldersForVersion[version];
			}
			return null;
		}

		// Token: 0x060024E8 RID: 9448 RVA: 0x000BE9C6 File Offset: 0x000BCBC6
		public List<string> DefinedVersions()
		{
			return this.foldersForVersion.Keys.ToList<string>();
		}

		// Token: 0x060024E9 RID: 9449 RVA: 0x000BE9D8 File Offset: 0x000BCBD8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			foreach (object obj in xmlRoot.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (!(xmlNode is XmlComment))
				{
					string text = xmlNode.Name.ToLower();
					if (text.StartsWith("v"))
					{
						text = text.Substring(1);
					}
					if (!this.foldersForVersion.ContainsKey(text))
					{
						this.foldersForVersion.Add(text, new List<LoadFolder>());
					}
					foreach (object obj2 in xmlNode.ChildNodes)
					{
						XmlNode xmlNode2 = (XmlNode)obj2;
						if (!(xmlNode2 is XmlComment))
						{
							XmlAttributeCollection attributes = xmlNode2.Attributes;
							XmlAttribute xmlAttribute = (attributes != null) ? attributes["IfModActive"] : null;
							List<string> requiredAnyOfPackageIds = null;
							if (xmlAttribute != null)
							{
								requiredAnyOfPackageIds = (from s in xmlAttribute.Value.Split(',', StringSplitOptions.None)
								select s.Trim()).ToList<string>();
							}
							XmlAttributeCollection attributes2 = xmlNode2.Attributes;
							XmlAttribute xmlAttribute2 = (attributes2 != null) ? attributes2["IfModActiveAll"] : null;
							List<string> requiredAllOfPackageIds = null;
							if (xmlAttribute2 != null)
							{
								requiredAllOfPackageIds = (from s in xmlAttribute2.Value.Split(',', StringSplitOptions.None)
								select s.Trim()).ToList<string>();
							}
							XmlAttributeCollection attributes3 = xmlNode2.Attributes;
							XmlAttribute xmlAttribute3 = (attributes3 != null) ? attributes3["IfModNotActive"] : null;
							List<string> disallowedAnyOfPackageIds = null;
							if (xmlAttribute3 != null)
							{
								disallowedAnyOfPackageIds = (from s in xmlAttribute3.Value.Split(',', StringSplitOptions.None)
								select s.Trim()).ToList<string>();
							}
							if (xmlNode2.InnerText == "/" || xmlNode2.InnerText == "\\")
							{
								this.foldersForVersion[text].Add(new LoadFolder("", requiredAnyOfPackageIds, requiredAllOfPackageIds, disallowedAnyOfPackageIds));
							}
							else
							{
								string text2 = xmlNode2.InnerText;
								char directorySeparatorChar = Path.DirectorySeparatorChar;
								char altDirectorySeparatorChar = Path.AltDirectorySeparatorChar;
								if (altDirectorySeparatorChar != directorySeparatorChar)
								{
									text2 = text2.Replace(altDirectorySeparatorChar, directorySeparatorChar);
								}
								this.foldersForVersion[text].Add(new LoadFolder(text2, requiredAnyOfPackageIds, requiredAllOfPackageIds, disallowedAnyOfPackageIds));
							}
						}
					}
				}
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x000BEC90 File Offset: 0x000BCE90
		public List<string> GetIssueList(ModMetaData mod)
		{
			List<string> list = new List<string>();
			if (this.foldersForVersion.Count > 0)
			{
				string text = null;
				foreach (string text2 in this.foldersForVersion.Keys)
				{
					if (this.foldersForVersion[text2].Count == 0)
					{
						list.Add("ModLoadFolderListEmpty".Translate(text2));
					}
					foreach (LoadFolder loadFolder in from f in this.foldersForVersion[text2]
					group f by f into g
					where g.Count<LoadFolder>() > 1
					select g.Key)
					{
						list.Add("ModLoadFolderRepeatingFolder".Translate(text2, loadFolder.folderName));
					}
					if (!VersionControl.IsWellFormattedVersionString(text2) && !text2.Equals("default", StringComparison.InvariantCultureIgnoreCase))
					{
						list.Add("ModLoadFolderMalformedVersion".Translate(text2));
					}
					if (text2.Equals("default"))
					{
						list.Add("ModLoadFolderDefaultDeprecated".Translate());
					}
					Version v;
					Version v2;
					if (text != null && VersionControl.TryParseVersionString(text2, out v) && VersionControl.TryParseVersionString(text, out v2) && v < v2)
					{
						list.Add("ModLoadFolderOutOfOrder".Translate(text2, text));
					}
					for (int i = 0; i < this.foldersForVersion[text2].Count; i++)
					{
						LoadFolder loadFolder2 = this.foldersForVersion[text2][i];
						if (!Directory.Exists(Path.Combine(mod.RootDir.FullName, loadFolder2.folderName)))
						{
							list.Add("ModLoadFolderDoesntExist".Translate(loadFolder2.folderName, text2));
						}
					}
					Version item;
					if (VersionControl.TryParseVersionString(text2, out item) && !mod.SupportedVersionsReadOnly.Contains(item))
					{
						list.Add("ModLoadFolderDefinesUnsupportedGameVersion".Translate(text2));
					}
					text = text2;
				}
			}
			return list;
		}

		// Token: 0x04001E67 RID: 7783
		private Dictionary<string, List<LoadFolder>> foldersForVersion = new Dictionary<string, List<LoadFolder>>();

		// Token: 0x04001E68 RID: 7784
		public const string defaultVersionName = "default";
	}
}
