using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200243F RID: 9279
	public class SolidBioDatabase
	{
		// Token: 0x0600C8CB RID: 51403 RVA: 0x003AA791 File Offset: 0x003A8991
		public static void Clear()
		{
			SolidBioDatabase.allBios.Clear();
			PawnNameDatabaseSolid.Clear();
		}

		// Token: 0x0600C8CC RID: 51404 RVA: 0x003AA7A4 File Offset: 0x003A89A4
		public static void LoadAllBios()
		{
			TextAsset[] array = Resources.LoadAll<TextAsset>("Backstories/Solid");
			List<PawnBio> list = new List<PawnBio>();
			TextAsset[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				SolidBioDatabase.ParseTextAsset(array2[i], list);
			}
			foreach (PawnBio pawnBio in list)
			{
				pawnBio.name.ResolveMissingPieces(null);
				if (pawnBio.childhood == null || pawnBio.adulthood == null)
				{
					PawnNameDatabaseSolid.AddPlayerContentName(pawnBio.name, pawnBio.gender);
				}
				else
				{
					pawnBio.ResolveReferences();
					SolidBioDatabase.allBios.Add(pawnBio);
				}
			}
		}

		// Token: 0x0600C8CD RID: 51405 RVA: 0x003AA85C File Offset: 0x003A8A5C
		private static void ParseTextAsset(TextAsset textObject, List<PawnBio> outputBios)
		{
			StringReader stringReader = new StringReader(textObject.text);
			XmlReader xmlReader = XmlReader.Create(stringReader, SolidBioDatabase.Settings);
			Dictionary<string, PawnBio> dictionary = new Dictionary<string, PawnBio>();
			PawnBio pawnBio = null;
			bool flag = false;
			string text = null;
			bool flag2 = false;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			while (xmlReader.Read())
			{
				XmlNodeType nodeType = xmlReader.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType == XmlNodeType.EndElement)
					{
						if (xmlReader.Name == "PawnBio")
						{
							if (text2 != null || text3 != null || text4 != null)
							{
								pawnBio.name = new NameTriple(text2, text4, text3);
							}
							if (flag && text != null)
							{
								dictionary[text] = pawnBio;
							}
							else
							{
								outputBios.Add(pawnBio);
							}
							pawnBio = null;
							flag = false;
							text = null;
						}
						else if (xmlReader.Name == "Name")
						{
							flag2 = false;
						}
					}
				}
				else if (xmlReader.Name == "PawnBio")
				{
					flag = (xmlReader.GetAttribute("Abstract") == "True");
					string attribute = xmlReader.GetAttribute("ParentName");
					text = xmlReader.GetAttribute("Name");
					text2 = null;
					text3 = null;
					text4 = null;
					PawnBio pawnBio2;
					if (attribute != null && dictionary.TryGetValue(attribute, out pawnBio2))
					{
						pawnBio = new PawnBio
						{
							gender = pawnBio2.gender,
							childhood = pawnBio2.childhood,
							adulthood = pawnBio2.adulthood,
							pirateKing = pawnBio2.pirateKing,
							rare = pawnBio2.rare
						};
					}
					else
					{
						pawnBio = new PawnBio();
					}
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Name"))
				{
					flag2 = true;
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "First") || (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "firstInt") && flag2))
				{
					text2 = xmlReader.ReadString();
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Last") || (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "lastInt") && flag2))
				{
					text3 = xmlReader.ReadString();
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Nick") || (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "nickInt") && flag2))
				{
					text4 = xmlReader.ReadString();
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Childhood"))
				{
					string defName = xmlReader.ReadString();
					pawnBio.childhood = DefDatabase<BackstoryDef>.GetNamed(defName, false);
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Adulthood"))
				{
					string defName2 = xmlReader.ReadString();
					pawnBio.adulthood = DefDatabase<BackstoryDef>.GetNamed(defName2, false);
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Gender"))
				{
					GenderPossibility gender;
					if (Enum.TryParse<GenderPossibility>(xmlReader.ReadString(), out gender))
					{
						pawnBio.gender = gender;
					}
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "PirateKing"))
				{
					pawnBio.pirateKing = bool.Parse(xmlReader.ReadString());
				}
				else if (SolidBioDatabase.<ParseTextAsset>g__strcmp|4_0(xmlReader.Name, "Rare"))
				{
					pawnBio.rare = bool.Parse(xmlReader.ReadString());
				}
			}
			xmlReader.Close();
			stringReader.Close();
		}

		// Token: 0x0600C8D0 RID: 51408 RVA: 0x003AABB6 File Offset: 0x003A8DB6
		[CompilerGenerated]
		internal static bool <ParseTextAsset>g__strcmp|4_0(string a, string b)
		{
			return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
		}

		// Token: 0x040088AC RID: 34988
		public static List<PawnBio> allBios = new List<PawnBio>();

		// Token: 0x040088AD RID: 34989
		private static readonly XmlReaderSettings Settings = new XmlReaderSettings
		{
			IgnoreComments = true,
			IgnoreWhitespace = true,
			CheckCharacters = false,
			Async = false
		};
	}
}
