using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000C22 RID: 3106
	public static class LayerLoader
	{
		// Token: 0x06004FC9 RID: 20425 RVA: 0x0019A230 File Offset: 0x00198430
		public static void LoadFileIntoList(TextAsset ass, List<DiaNodeMold> NodeListToFill, List<DiaNodeList> ListListToFill, DiaNodeType NodesType)
		{
			XPathNavigator xpathNavigator = new XPathDocument(new StringReader(ass.text)).CreateNavigator();
			xpathNavigator.MoveToFirst();
			xpathNavigator.MoveToFirstChild();
			foreach (object obj in xpathNavigator.Select("Node"))
			{
				XPathNavigator xpathNavigator2 = (XPathNavigator)obj;
				try
				{
					TextReader textReader = new StringReader(xpathNavigator2.OuterXml);
					DiaNodeMold diaNodeMold = (DiaNodeMold)new XmlSerializer(typeof(DiaNodeMold)).Deserialize(textReader);
					diaNodeMold.nodeType = NodesType;
					NodeListToFill.Add(diaNodeMold);
					textReader.Dispose();
				}
				catch (Exception ex)
				{
					string str = "Exception deserializing ";
					string outerXml = xpathNavigator2.OuterXml;
					string str2 = ":\n";
					Exception innerException = ex.InnerException;
					Log.Message(str + outerXml + str2 + ((innerException != null) ? innerException.ToString() : null));
				}
			}
			foreach (object obj2 in xpathNavigator.Select("NodeList"))
			{
				XPathNavigator xpathNavigator3 = (XPathNavigator)obj2;
				try
				{
					TextReader textReader2 = new StringReader(xpathNavigator3.OuterXml);
					DiaNodeList item = (DiaNodeList)new XmlSerializer(typeof(DiaNodeList)).Deserialize(textReader2);
					ListListToFill.Add(item);
				}
				catch (Exception ex2)
				{
					string str3 = "Exception deserializing ";
					string outerXml2 = xpathNavigator3.OuterXml;
					string str4 = ":\n";
					Exception innerException2 = ex2.InnerException;
					Log.Message(str3 + outerXml2 + str4 + ((innerException2 != null) ? innerException2.ToString() : null));
				}
			}
		}

		// Token: 0x06004FCA RID: 20426 RVA: 0x0019A3EC File Offset: 0x001985EC
		public static void MarkNonRootNodes(List<DiaNodeMold> NodeList)
		{
			foreach (DiaNodeMold d in NodeList)
			{
				LayerLoader.RecursiveSetIsRootFalse(d);
			}
			foreach (DiaNodeMold diaNodeMold in NodeList)
			{
				foreach (DiaNodeMold diaNodeMold2 in NodeList)
				{
					foreach (DiaOptionMold diaOptionMold in diaNodeMold2.optionList)
					{
						bool flag = false;
						using (List<string>.Enumerator enumerator4 = diaOptionMold.ChildNodeNames.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								if (enumerator4.Current == diaNodeMold.name)
								{
									flag = true;
								}
							}
						}
						if (flag)
						{
							diaNodeMold.isRoot = false;
						}
					}
				}
			}
		}

		// Token: 0x06004FCB RID: 20427 RVA: 0x0019A540 File Offset: 0x00198740
		private static void RecursiveSetIsRootFalse(DiaNodeMold d)
		{
			foreach (DiaOptionMold diaOptionMold in d.optionList)
			{
				foreach (DiaNodeMold diaNodeMold in diaOptionMold.ChildNodes)
				{
					diaNodeMold.isRoot = false;
					LayerLoader.RecursiveSetIsRootFalse(diaNodeMold);
				}
			}
		}
	}
}
