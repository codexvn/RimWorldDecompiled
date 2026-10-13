using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace Verse
{
	// Token: 0x02000839 RID: 2105
	public static class XmlInheritance
	{
		// Token: 0x0600356D RID: 13677 RVA: 0x0011C044 File Offset: 0x0011A244
		static XmlInheritance()
		{
			foreach (Type type in GenTypes.AllTypes)
			{
				foreach (FieldInfo fieldInfo in type.GetFields())
				{
					if (fieldInfo.IsDefined(typeof(XmlInheritanceAllowDuplicateNodes), false))
					{
						XmlInheritance.allowDuplicateNodesFieldNames.Add(fieldInfo.Name);
					}
				}
			}
		}

		// Token: 0x0600356E RID: 13678 RVA: 0x0011C0FC File Offset: 0x0011A2FC
		public static void TryRegisterAllFrom(LoadableXmlAsset xmlAsset, ModContentPack mod)
		{
			if (xmlAsset.xmlDoc == null)
			{
				return;
			}
			DeepProfiler.Start("XmlInheritance.TryRegisterAllFrom");
			foreach (object obj in xmlAsset.xmlDoc.DocumentElement.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					XmlInheritance.TryRegister(xmlNode, mod);
				}
			}
			DeepProfiler.End();
		}

		// Token: 0x0600356F RID: 13679 RVA: 0x0011C180 File Offset: 0x0011A380
		public static void TryRegister(XmlNode node, ModContentPack mod)
		{
			XmlAttribute xmlAttribute = node.Attributes["Name"];
			XmlAttribute xmlAttribute2 = node.Attributes["ParentName"];
			XmlAttribute xmlAttribute3 = node.Attributes["MayRequire"];
			if (xmlAttribute == null && xmlAttribute2 == null)
			{
				return;
			}
			if (xmlAttribute3 != null && !ModsConfig.AreAllActive(xmlAttribute3.Value.ToLower()))
			{
				return;
			}
			List<XmlInheritance.XmlInheritanceNode> list = null;
			if (xmlAttribute != null && XmlInheritance.nodesByName.TryGetValue(xmlAttribute.Value, out list))
			{
				int i = 0;
				while (i < list.Count)
				{
					if (list[i].mod == mod)
					{
						if (mod == null)
						{
							Log.Error("XML error: Could not register node named \"" + xmlAttribute.Value + "\" because this name is already used.");
							return;
						}
						Log.Error(string.Concat(new string[]
						{
							"XML error: Could not register node named \"",
							xmlAttribute.Value,
							"\" in mod ",
							mod.ToString(),
							" because this name is already used in this mod."
						}));
						return;
					}
					else
					{
						i++;
					}
				}
			}
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = new XmlInheritance.XmlInheritanceNode();
			xmlInheritanceNode.xmlNode = node;
			xmlInheritanceNode.mod = mod;
			XmlInheritance.unresolvedNodes.Add(xmlInheritanceNode);
			if (xmlAttribute != null)
			{
				if (list != null)
				{
					list.Add(xmlInheritanceNode);
					return;
				}
				list = new List<XmlInheritance.XmlInheritanceNode>();
				list.Add(xmlInheritanceNode);
				XmlInheritance.nodesByName.Add(xmlAttribute.Value, list);
			}
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x0011C2C8 File Offset: 0x0011A4C8
		public static void Resolve()
		{
			XmlInheritance.ResolveParentsAndChildNodesLinks();
			XmlInheritance.ResolveXmlNodes();
		}

		// Token: 0x06003571 RID: 13681 RVA: 0x0011C2D4 File Offset: 0x0011A4D4
		public static XmlNode GetResolvedNodeFor(XmlNode originalNode)
		{
			if (originalNode.Attributes["ParentName"] != null)
			{
				XmlInheritance.XmlInheritanceNode xmlInheritanceNode;
				if (XmlInheritance.resolvedNodes.TryGetValue(originalNode, out xmlInheritanceNode))
				{
					return xmlInheritanceNode.resolvedXmlNode;
				}
				if (XmlInheritance.unresolvedNodes.Any((XmlInheritance.XmlInheritanceNode x) => x.xmlNode == originalNode))
				{
					Log.Error("XML error: XML node \"" + originalNode.Name + "\" has not been resolved yet. There's probably a Resolve() call missing somewhere.");
				}
				else
				{
					Log.Error("XML error: Tried to get resolved node for node \"" + originalNode.Name + "\" which uses a ParentName attribute, but it is not in a resolved nodes collection, which means that it was never registered or there was an error while resolving it.");
				}
			}
			return originalNode;
		}

		// Token: 0x06003572 RID: 13682 RVA: 0x0011C37E File Offset: 0x0011A57E
		public static void Clear()
		{
			XmlInheritance.resolvedNodes.Clear();
			XmlInheritance.unresolvedNodes.Clear();
			XmlInheritance.nodesByName.Clear();
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x0011C3A0 File Offset: 0x0011A5A0
		private static void ResolveParentsAndChildNodesLinks()
		{
			for (int i = 0; i < XmlInheritance.unresolvedNodes.Count; i++)
			{
				XmlAttribute xmlAttribute = XmlInheritance.unresolvedNodes[i].xmlNode.Attributes["ParentName"];
				if (xmlAttribute != null)
				{
					XmlInheritance.unresolvedNodes[i].parent = XmlInheritance.GetBestParentFor(XmlInheritance.unresolvedNodes[i], xmlAttribute.Value);
					if (XmlInheritance.unresolvedNodes[i].parent != null)
					{
						XmlInheritance.unresolvedNodes[i].parent.children.Add(XmlInheritance.unresolvedNodes[i]);
					}
				}
			}
		}

		// Token: 0x06003574 RID: 13684 RVA: 0x0011C448 File Offset: 0x0011A648
		private static void ResolveXmlNodes()
		{
			List<XmlInheritance.XmlInheritanceNode> list = (from x in XmlInheritance.unresolvedNodes
			where x.parent == null || x.parent.resolvedXmlNode != null
			select x).ToList<XmlInheritance.XmlInheritanceNode>();
			for (int i = 0; i < list.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(list[i]);
			}
			for (int j = 0; j < XmlInheritance.unresolvedNodes.Count; j++)
			{
				if (XmlInheritance.unresolvedNodes[j].resolvedXmlNode == null)
				{
					Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + XmlInheritance.unresolvedNodes[j].xmlNode.Name + "\". Full node: " + XmlInheritance.unresolvedNodes[j].xmlNode.OuterXml);
				}
				else
				{
					XmlInheritance.resolvedNodes.Add(XmlInheritance.unresolvedNodes[j].xmlNode, XmlInheritance.unresolvedNodes[j]);
				}
			}
			XmlInheritance.unresolvedNodes.Clear();
		}

		// Token: 0x06003575 RID: 13685 RVA: 0x0011C53C File Offset: 0x0011A73C
		private static void ResolveXmlNodesRecursively(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.resolvedXmlNode != null)
			{
				Log.Error("XML error: Cyclic inheritance hierarchy detected for node \"" + node.xmlNode.Name + "\". Full node: " + node.xmlNode.OuterXml);
				return;
			}
			XmlInheritance.ResolveXmlNodeFor(node);
			for (int i = 0; i < node.children.Count; i++)
			{
				XmlInheritance.ResolveXmlNodesRecursively(node.children[i]);
			}
		}

		// Token: 0x06003576 RID: 13686 RVA: 0x0011C5AC File Offset: 0x0011A7AC
		private static XmlInheritance.XmlInheritanceNode GetBestParentFor(XmlInheritance.XmlInheritanceNode node, string parentName)
		{
			XmlInheritance.XmlInheritanceNode xmlInheritanceNode = null;
			List<XmlInheritance.XmlInheritanceNode> list;
			if (XmlInheritance.nodesByName.TryGetValue(parentName, out list))
			{
				if (node.mod == null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].mod == null)
						{
							xmlInheritanceNode = list[i];
							break;
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							if (xmlInheritanceNode == null || list[j].mod.loadOrder < xmlInheritanceNode.mod.loadOrder)
							{
								xmlInheritanceNode = list[j];
							}
						}
					}
				}
				else
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k].mod != null && list[k].mod.loadOrder <= node.mod.loadOrder && (xmlInheritanceNode == null || list[k].mod.loadOrder > xmlInheritanceNode.mod.loadOrder))
						{
							xmlInheritanceNode = list[k];
						}
					}
					if (xmlInheritanceNode == null)
					{
						for (int l = 0; l < list.Count; l++)
						{
							if (list[l].mod == null)
							{
								xmlInheritanceNode = list[l];
								break;
							}
						}
					}
				}
			}
			if (xmlInheritanceNode == null)
			{
				Log.Error(string.Concat(new string[]
				{
					"XML error: Could not find parent node named \"",
					parentName,
					"\" for node \"",
					node.xmlNode.Name,
					"\". Full node: ",
					node.xmlNode.OuterXml
				}));
				return null;
			}
			return xmlInheritanceNode;
		}

		// Token: 0x06003577 RID: 13687 RVA: 0x0011C734 File Offset: 0x0011A934
		private static void ResolveXmlNodeFor(XmlInheritance.XmlInheritanceNode node)
		{
			if (node.parent == null)
			{
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			if (node.parent.resolvedXmlNode == null)
			{
				Log.Error("XML error: Internal error. Tried to resolve node whose parent has not been resolved yet. This means that this method was called in incorrect order.");
				node.resolvedXmlNode = node.xmlNode;
				return;
			}
			XmlInheritance.CheckForDuplicateNodes(node.xmlNode, node.xmlNode);
			XmlNode xmlNode = node.parent.resolvedXmlNode.CloneNode(true);
			XmlInheritance.RecursiveNodeCopyOverwriteElements(node.xmlNode, xmlNode);
			node.resolvedXmlNode = xmlNode;
		}

		// Token: 0x06003578 RID: 13688 RVA: 0x0011C7B0 File Offset: 0x0011A9B0
		private static void RecursiveNodeCopyOverwriteElements(XmlNode child, XmlNode current)
		{
			DeepProfiler.Start("RecursiveNodeCopyOverwriteElements");
			try
			{
				XmlAttribute xmlAttribute = child.Attributes["Inherit"];
				if (xmlAttribute != null && xmlAttribute.Value.ToLower() == "false")
				{
					XmlNode nextSibling;
					for (XmlNode xmlNode = current.FirstChild; xmlNode != null; xmlNode = nextSibling)
					{
						nextSibling = xmlNode.NextSibling;
						current.RemoveChild(xmlNode);
					}
					foreach (object obj in child)
					{
						XmlNode node = (XmlNode)obj;
						XmlNode newChild = current.OwnerDocument.ImportNode(node, true);
						current.AppendChild(newChild);
					}
					using (IEnumerator enumerator = child.Attributes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj2 = enumerator.Current;
							XmlAttribute xmlAttribute2 = (XmlAttribute)obj2;
							if (!(xmlAttribute2.Name == "Inherit"))
							{
								XmlAttribute xmlAttribute3 = current.OwnerDocument.CreateAttribute(xmlAttribute2.Name);
								xmlAttribute3.Value = xmlAttribute2.Value;
								current.Attributes.Append(xmlAttribute3);
							}
						}
						return;
					}
				}
				current.Attributes.RemoveAll();
				XmlAttributeCollection attributes = child.Attributes;
				for (int i = 0; i < attributes.Count; i++)
				{
					XmlAttribute node2 = (XmlAttribute)current.OwnerDocument.ImportNode(attributes[i], true);
					current.Attributes.Append(node2);
				}
				bool flag = false;
				XmlNode xmlNode2 = null;
				foreach (object obj3 in child)
				{
					XmlNode xmlNode3 = (XmlNode)obj3;
					if (xmlNode3.NodeType == XmlNodeType.Text)
					{
						xmlNode2 = xmlNode3;
					}
					else if (xmlNode3.NodeType == XmlNodeType.Element)
					{
						flag = true;
					}
				}
				if (xmlNode2 != null)
				{
					DeepProfiler.Start("RecursiveNodeCopyOverwriteElements - Remove all current nodes");
					foreach (object obj4 in current.ChildNodes)
					{
						XmlNode xmlNode4 = (XmlNode)obj4;
						if (xmlNode4.NodeType != XmlNodeType.Attribute)
						{
							current.RemoveChild(xmlNode4);
						}
					}
					DeepProfiler.End();
					XmlNode newChild2 = current.OwnerDocument.ImportNode(xmlNode2, true);
					current.AppendChild(newChild2);
				}
				else
				{
					if (!flag)
					{
						bool flag2 = false;
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (((XmlNode)enumerator.Current).NodeType == XmlNodeType.Element)
								{
									flag2 = true;
									break;
								}
							}
						}
						if (flag2)
						{
							goto IL_39D;
						}
						using (IEnumerator enumerator = current.ChildNodes.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj5 = enumerator.Current;
								XmlNode xmlNode5 = (XmlNode)obj5;
								if (xmlNode5.NodeType != XmlNodeType.Attribute)
								{
									current.RemoveChild(xmlNode5);
								}
							}
							return;
						}
					}
					foreach (object obj6 in child)
					{
						XmlNode xmlNode6 = (XmlNode)obj6;
						if (xmlNode6.NodeType == XmlNodeType.Element)
						{
							if (XmlInheritance.IsListElement(xmlNode6))
							{
								XmlNode newChild3 = current.OwnerDocument.ImportNode(xmlNode6, true);
								current.AppendChild(newChild3);
							}
							else
							{
								XmlElement xmlElement = current[xmlNode6.Name];
								if (xmlElement != null)
								{
									XmlInheritance.RecursiveNodeCopyOverwriteElements(xmlNode6, xmlElement);
								}
								else
								{
									XmlNode newChild4 = current.OwnerDocument.ImportNode(xmlNode6, true);
									current.AppendChild(newChild4);
								}
							}
						}
					}
					IL_39D:;
				}
			}
			finally
			{
				DeepProfiler.End();
			}
		}

		// Token: 0x06003579 RID: 13689 RVA: 0x0011CC28 File Offset: 0x0011AE28
		private static void CheckForDuplicateNodes(XmlNode node, XmlNode root)
		{
			XmlInheritance.tempUsedNodeNames.Clear();
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (!XmlInheritance.IsListElement(xmlNode) && !XmlInheritance.tempUsedNodeNames.Add(xmlNode.Name))
					{
						Log.Error(string.Concat(new string[]
						{
							"XML error: Duplicate XML node name ",
							xmlNode.Name,
							" in this XML block: ",
							node.OuterXml,
							(node != root) ? ("\n\nRoot node: " + root.OuterXml) : ""
						}));
					}
					XmlInheritance.CheckForDuplicateNodes(xmlNode, root);
				}
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x0011CD04 File Offset: 0x0011AF04
		private static bool IsListElement(XmlNode node)
		{
			return node.Name == "li" || (node.ParentNode != null && XmlInheritance.allowDuplicateNodesFieldNames.Contains(node.ParentNode.Name));
		}

		// Token: 0x04002842 RID: 10306
		private static Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode> resolvedNodes = new Dictionary<XmlNode, XmlInheritance.XmlInheritanceNode>();

		// Token: 0x04002843 RID: 10307
		private static List<XmlInheritance.XmlInheritanceNode> unresolvedNodes = new List<XmlInheritance.XmlInheritanceNode>();

		// Token: 0x04002844 RID: 10308
		private static Dictionary<string, List<XmlInheritance.XmlInheritanceNode>> nodesByName = new Dictionary<string, List<XmlInheritance.XmlInheritanceNode>>();

		// Token: 0x04002845 RID: 10309
		public static HashSet<string> allowDuplicateNodesFieldNames = new HashSet<string>();

		// Token: 0x04002846 RID: 10310
		private const string NameAttributeName = "Name";

		// Token: 0x04002847 RID: 10311
		private const string ParentNameAttributeName = "ParentName";

		// Token: 0x04002848 RID: 10312
		private const string InheritAttributeName = "Inherit";

		// Token: 0x04002849 RID: 10313
		private static HashSet<string> tempUsedNodeNames = new HashSet<string>();

		// Token: 0x0200083A RID: 2106
		private class XmlInheritanceNode
		{
			// Token: 0x0400284A RID: 10314
			public XmlNode xmlNode;

			// Token: 0x0400284B RID: 10315
			public XmlNode resolvedXmlNode;

			// Token: 0x0400284C RID: 10316
			public ModContentPack mod;

			// Token: 0x0400284D RID: 10317
			public XmlInheritance.XmlInheritanceNode parent;

			// Token: 0x0400284E RID: 10318
			public List<XmlInheritance.XmlInheritanceNode> children = new List<XmlInheritance.XmlInheritanceNode>();
		}
	}
}
