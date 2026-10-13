using System;
using System.Collections;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000130 RID: 304
	public static class XmlHelper
	{
		// Token: 0x06000963 RID: 2403 RVA: 0x0002CBD8 File Offset: 0x0002ADD8
		public static bool TryParseSingleDefault(object requester, XmlNode root, string fieldName)
		{
			XmlText xmlText;
			if (root.ChildNodes.Count != 1 || root.FirstChild.Isnt(out xmlText) || fieldName.NullOrEmpty() || fieldName == "li")
			{
				return false;
			}
			FieldInfo field = requester.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (field == null)
			{
				Log.Error("Tried to get default field which does not exist: " + fieldName + " on " + requester.GetType().Name);
				return false;
			}
			if (typeof(Def).IsAssignableFrom(field.FieldType))
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(requester, field.Name, xmlText.InnerText, null, null, null);
			}
			else
			{
				field.SetValue(requester, ParseHelper.FromString(xmlText.InnerText, field.FieldType));
			}
			return true;
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0002CC9C File Offset: 0x0002AE9C
		public static void ParseElements(object requester, XmlNode root, string rootField, string defaultField = null)
		{
			if (root.Name != "li")
			{
				FieldInfo field = requester.GetType().GetField(rootField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field == null)
				{
					Log.Error("Tried to get root field which does not exist: " + rootField + " on " + requester.GetType().Name);
					return;
				}
				if (typeof(Def).IsAssignableFrom(field.FieldType))
				{
					DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(requester, field.Name, root.Name, null, null, null);
				}
				else
				{
					field.SetValue(requester, ParseHelper.FromString(root.Name, field.FieldType));
				}
				if (XmlHelper.TryParseSingleDefault(requester, root, defaultField))
				{
					return;
				}
				foreach (object obj in root.ChildNodes)
				{
					XmlElement xmlElement = obj as XmlElement;
					if (xmlElement != null)
					{
						FieldInfo field2 = requester.GetType().GetField(xmlElement.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
						if (field2 == null)
						{
							Log.Error(string.Concat(new string[]
							{
								"Tried to set a value (",
								xmlElement.InnerText,
								") to a field which does not exist: ",
								xmlElement.Name,
								" on ",
								requester.GetType().Name
							}));
						}
						else if (typeof(Def).IsAssignableFrom(field2.FieldType))
						{
							DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(requester, field2.Name, xmlElement.InnerText, null, null, null);
						}
						else
						{
							if (typeof(IList).IsAssignableFrom(field2.FieldType))
							{
								IList list = (IList)Activator.CreateInstance(field2.FieldType);
								Type type = list.GetType().GenericTypeArguments[0];
								field2.SetValue(requester, list);
								if (typeof(Def).IsAssignableFrom(type))
								{
									Log.Error("Cannot use lists of defs in XML Helper Parse Elements");
									continue;
								}
								using (IEnumerator enumerator2 = xmlElement.ChildNodes.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										object obj2 = enumerator2.Current;
										XmlElement xmlElement2 = obj2 as XmlElement;
										if (xmlElement2 != null)
										{
											list.Add(ParseHelper.FromString(xmlElement2.InnerText, type));
										}
									}
									continue;
								}
							}
							field2.SetValue(requester, ParseHelper.FromString(xmlElement.InnerText, field2.FieldType));
						}
					}
				}
				return;
			}
			else
			{
				FieldInfo field3 = requester.GetType().GetField(rootField, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				if (field3 == null)
				{
					Log.Error("Tried to get root field which does not exist: " + rootField + " on " + requester.GetType().Name);
					return;
				}
				if (typeof(Def).IsAssignableFrom(field3.FieldType))
				{
					DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(requester, field3.Name, root.InnerText, null, null, null);
					return;
				}
				field3.SetValue(requester, ParseHelper.FromString(root.InnerText, field3.FieldType));
				return;
			}
		}

		// Token: 0x040004B0 RID: 1200
		private const BindingFlags Flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
