using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Xml;

namespace Verse
{
	// Token: 0x02000830 RID: 2096
	public static class DirectXmlToObjectNew
	{
		// Token: 0x0600352D RID: 13613 RVA: 0x00118338 File Offset: 0x00116538
		public static Def DefFromNodeNew(XmlNode node, LoadableXmlAsset loadingAsset)
		{
			if (node.NodeType != XmlNodeType.Element)
			{
				return null;
			}
			XmlAttribute xmlAttribute = node.Attributes["Abstract"];
			if (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase))
			{
				return null;
			}
			XmlNode resolvedNodeFor = XmlInheritance.GetResolvedNodeFor(node);
			string text = node.Name;
			XmlAttribute xmlAttribute2 = resolvedNodeFor.Attributes["Class"];
			if (xmlAttribute2 != null)
			{
				text = xmlAttribute2.Value;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(text, null);
			if (typeInAnyAssembly == null || !GenTypes.IsDef(typeInAnyAssembly))
			{
				Log.ErrorOnce(string.Concat(new string[]
				{
					"Type ",
					text,
					" is not a Def type or could not be found, in file ",
					(loadingAsset != null) ? loadingAsset.name : "(unknown)",
					". Context: ",
					node.OuterXml
				}), text.GetHashCode());
				return null;
			}
			Def def = null;
			try
			{
				DirectXmlToObjectNew.ParseValueAndReturnDefDelegate defParserForType = DirectXmlToObjectNew.GetDefParserForType(typeInAnyAssembly);
				DeepProfiler.Start(string.Format("ParseValueAndReturnDef (for {0})", typeInAnyAssembly));
				def = defParserForType(0, 0, node, typeInAnyAssembly);
				DeepProfiler.End();
				def.ResolveDefNameHash();
			}
			catch (Exception arg)
			{
				Log.Error(string.Format("Exception loading def from file {0}: {1}", (loadingAsset != null) ? loadingAsset.name : "(unknown)", arg));
			}
			return def;
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x00118474 File Offset: 0x00116674
		public static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate GetFieldSetterForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type", "Cannot get field setter for null type.");
			}
			if (DirectXmlToObjectNew.TypeCanBeDeserializedWithSharedBody(type))
			{
				type = typeof(object);
			}
			DirectXmlToObjectNew.ParseValueAndSetFieldDelegate parseValueAndSetFieldDelegate;
			if (DirectXmlToObjectNew.parseMethods.TryGetValue(type, out parseValueAndSetFieldDelegate))
			{
				return parseValueAndSetFieldDelegate;
			}
			DeepProfiler.Start("CreateFieldSetterForType");
			parseValueAndSetFieldDelegate = DirectXmlToObjectNew.CreateFieldSetterForType(type);
			DeepProfiler.End();
			DirectXmlToObjectNew.parseMethods[type] = parseValueAndSetFieldDelegate;
			return parseValueAndSetFieldDelegate;
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x001184E4 File Offset: 0x001166E4
		public static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate GetListItemAdderForType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type", "Cannot get list item adder for null type.");
			}
			if (DirectXmlToObjectNew.TypeCanBeDeserializedWithSharedBody(type))
			{
				type = typeof(object);
			}
			DirectXmlToObjectNew.ParseValueAndAddListItemDelegate parseValueAndAddListItemDelegate;
			if (DirectXmlToObjectNew.parseListItemMethods.TryGetValue(type, out parseValueAndAddListItemDelegate))
			{
				return parseValueAndAddListItemDelegate;
			}
			DeepProfiler.Start("CreateListItemAdderForType");
			parseValueAndAddListItemDelegate = DirectXmlToObjectNew.CreateListItemAdderForType(type);
			DeepProfiler.End();
			DirectXmlToObjectNew.parseListItemMethods[type] = parseValueAndAddListItemDelegate;
			return parseValueAndAddListItemDelegate;
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x00118554 File Offset: 0x00116754
		public static DirectXmlToObjectNew.ParseValueAndReturnDefDelegate GetDefParserForType(Type type)
		{
			if (DirectXmlToObjectNew.TypeCanBeDeserializedWithSharedBody(type))
			{
				type = typeof(Def);
			}
			DirectXmlToObjectNew.ParseValueAndReturnDefDelegate parseValueAndReturnDefDelegate;
			if (DirectXmlToObjectNew.parseDefMethods.TryGetValue(type, out parseValueAndReturnDefDelegate))
			{
				return parseValueAndReturnDefDelegate;
			}
			parseValueAndReturnDefDelegate = DirectXmlToObjectNew.CreateDefParserForType(type);
			DirectXmlToObjectNew.parseDefMethods[type] = parseValueAndReturnDefDelegate;
			return parseValueAndReturnDefDelegate;
		}

		// Token: 0x06003531 RID: 13617 RVA: 0x0011859C File Offset: 0x0011679C
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForType(Type type)
		{
			if (XmlToObjectUtils.CustomDataLoadMethodOf(type) != null)
			{
				return DirectXmlToObjectNew.CreateFieldSetterForCustomLoadable(type);
			}
			if (GenTypes.IsSlateRef(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForSlateRef(type);
			}
			if (type == typeof(string))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForString();
			}
			if (GenTypes.HasFlagsAttribute(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForFlagsEnum(type);
			}
			if (GenTypes.IsList(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForList(type);
			}
			if (GenTypes.IsDictionary(type))
			{
				return DirectXmlToObjectNew.CreateFieldSetterForDict(type);
			}
			return DirectXmlToObjectNew.CreateFieldSetterForGeneralType(type);
		}

		// Token: 0x06003532 RID: 13618 RVA: 0x00118618 File Offset: 0x00116818
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForType(Type type)
		{
			if (XmlToObjectUtils.CustomDataLoadMethodOf(type) != null)
			{
				return DirectXmlToObjectNew.CreateListItemAdderForCustomLoadable(type);
			}
			if (GenTypes.IsSlateRef(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForSlateRef(type);
			}
			if (type == typeof(string))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForString();
			}
			if (GenTypes.HasFlagsAttribute(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForFlagsEnum(type);
			}
			if (GenTypes.IsList(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForList(type);
			}
			if (GenTypes.IsDictionary(type))
			{
				return DirectXmlToObjectNew.CreateListItemAdderForDict(type);
			}
			return DirectXmlToObjectNew.CreateListItemAdderForGeneralType(type);
		}

		// Token: 0x06003533 RID: 13619 RVA: 0x00118694 File Offset: 0x00116894
		private static DirectXmlToObjectNew.ParseValueAndReturnDefDelegate CreateDefParserForType(Type type)
		{
			if (!GenTypes.IsDef(type))
			{
				throw new InvalidOperationException("Cannot create a Def parser for type " + type.FullName + ". Only Def types are supported.");
			}
			string str = (type == typeof(object)) ? "SharedBody" : DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type);
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndReturnDef_" + str, type, new Type[]
			{
				typeof(int),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateComplexType(ilgenerator, type, localBuilder);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndReturnDefDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndReturnDefDelegate));
		}

		// Token: 0x06003534 RID: 13620 RVA: 0x0011877C File Offset: 0x0011697C
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForCustomLoadable(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetCustomLoadableField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateCustomLoadable(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06003535 RID: 13621 RVA: 0x00118834 File Offset: 0x00116A34
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForSlateRef(Type slateRefType)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetSlateRefField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(slateRefType), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateSlateRef(ilgenerator, slateRefType);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06003536 RID: 13622 RVA: 0x001188EC File Offset: 0x00116AEC
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForString()
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetStringField", null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateString(ilgenerator);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06003537 RID: 13623 RVA: 0x00118998 File Offset: 0x00116B98
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForFlagsEnum(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetFlagsEnumField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			DirectXmlToObjectNew.EmitIlToHandleSingleTextNodeViaParseHelper(ilgenerator, type, localBuilder, label2, label);
			ilgenerator.MarkLabel(label2);
			DirectXmlToObjectNew.EmitIlToParseFlagsEnum(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Unbox_Any, type);
			ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			ilgenerator.Emit(OpCodes.Box, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06003538 RID: 13624 RVA: 0x00118AB0 File Offset: 0x00116CB0
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForList(Type listType)
		{
			Type type = listType.GetGenericArguments()[0];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetListField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateList(ilgenerator, listType, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x00118B74 File Offset: 0x00116D74
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForDict(Type dictType)
		{
			Type type = dictType.GetGenericArguments()[0];
			Type type2 = dictType.GetGenericArguments()[1];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetDictField_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type) + "_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type2), null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitILToCreateAndPopulateDictionary(ilgenerator, dictType, type, type2);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x0600353A RID: 13626 RVA: 0x00118C4C File Offset: 0x00116E4C
		private static DirectXmlToObjectNew.ParseValueAndSetFieldDelegate CreateFieldSetterForGeneralType(Type type)
		{
			string str = (type == typeof(object)) ? "SharedBody" : DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type);
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndSetComplexTypeField_" + str, null, new Type[]
			{
				typeof(object),
				typeof(FieldInfo),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			Type innerIfNullable = type.GetInnerIfNullable();
			bool flag = innerIfNullable != type;
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			LocalBuilder localBuilder2 = flag ? ilgenerator.DeclareLocal(innerIfNullable) : null;
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			Label label4 = ilgenerator.DefineLabel();
			Label label5 = ilgenerator.DefineLabel();
			DirectXmlToObjectNew.EmitIlToHandleEmptyNode(ilgenerator, type, localBuilder, label3, label2);
			ilgenerator.MarkLabel(label3);
			DirectXmlToObjectNew.EmitIlToErrorAndMakeDefaultValueForCdata(ilgenerator, type, label4, label, localBuilder);
			ilgenerator.MarkLabel(label4);
			if (ParseHelper.HandlesType(type))
			{
				DirectXmlToObjectNew.EmitIlToHandleSingleTextNodeViaParseHelper(ilgenerator, innerIfNullable, flag ? localBuilder2 : localBuilder, label5, label);
			}
			ilgenerator.MarkLabel(label5);
			bool flag2 = !innerIfNullable.IsPrimitive && !innerIfNullable.IsEnum && innerIfNullable != typeof(Type);
			bool flag3 = innerIfNullable.IsValueType || innerIfNullable.GetConstructor(Type.EmptyTypes) != null;
			if (flag2 && flag3)
			{
				DirectXmlToObjectNew.EmitIlToCreateAndPopulateComplexType(ilgenerator, innerIfNullable, flag ? localBuilder2 : localBuilder);
			}
			ilgenerator.MarkLabel(label);
			if (flag)
			{
				ilgenerator.Emit(OpCodes.Ldloc, localBuilder2);
				ilgenerator.Emit(OpCodes.Newobj, type.GetConstructor(new Type[]
				{
					innerIfNullable
				}));
				ilgenerator.Emit(OpCodes.Stloc, localBuilder);
			}
			ilgenerator.MarkLabel(label2);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, type);
			}
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndSetFieldDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
		}

		// Token: 0x0600353B RID: 13627 RVA: 0x00118E80 File Offset: 0x00117080
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForCustomLoadable(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddCustomLoadableToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateCustomLoadable(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x00118F38 File Offset: 0x00117138
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForSlateRef(Type slateRefType)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddSlateRefToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(slateRefType), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateSlateRef(ilgenerator, slateRefType);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x0600353D RID: 13629 RVA: 0x00118FF0 File Offset: 0x001171F0
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForString()
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddStringToList", null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateString(ilgenerator);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x0600353E RID: 13630 RVA: 0x0011909C File Offset: 0x0011729C
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForFlagsEnum(Type type)
		{
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddFlagsEnumToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToParseFlagsEnum(ilgenerator, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x00119154 File Offset: 0x00117354
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForList(Type listType)
		{
			Type type = listType.GetGenericArguments()[0];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddListToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateList(ilgenerator, listType, type);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06003540 RID: 13632 RVA: 0x00119218 File Offset: 0x00117418
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForDict(Type dictType)
		{
			Type keyType = dictType.GetGenericArguments()[0];
			Type valueType = dictType.GetGenericArguments()[1];
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddDictToList_" + DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(dictType), null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			DirectXmlToObjectNew.EmitILToCreateAndPopulateDictionary(ilgenerator, dictType, keyType, valueType);
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06003541 RID: 13633 RVA: 0x001192E4 File Offset: 0x001174E4
		private static DirectXmlToObjectNew.ParseValueAndAddListItemDelegate CreateListItemAdderForGeneralType(Type type)
		{
			string str = (type == typeof(object)) ? "SharedBody" : DirectXmlToObjectNew.GetDynamicMethodNameSuffixForType(type);
			DynamicMethod dynamicMethod = new DynamicMethod("ParseAndAddComplexTypeToList_" + str, null, new Type[]
			{
				typeof(object),
				typeof(int),
				typeof(XmlNode),
				typeof(Type)
			}, typeof(DirectXmlToObjectNew.DummyTypeToHoldDynamicMethods));
			ILGenerator ilgenerator = dynamicMethod.GetILGenerator();
			LocalBuilder localBuilder = ilgenerator.DeclareLocal(type);
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			Label label4 = ilgenerator.DefineLabel();
			DirectXmlToObjectNew.EmitIlToHandleEmptyNode(ilgenerator, type, localBuilder, label2, label);
			ilgenerator.MarkLabel(label2);
			DirectXmlToObjectNew.EmitIlToErrorAndMakeDefaultValueForCdata(ilgenerator, type, label3, label, localBuilder);
			ilgenerator.MarkLabel(label3);
			if (ParseHelper.HandlesType(type))
			{
				DirectXmlToObjectNew.EmitIlToHandleSingleTextNodeViaParseHelper(ilgenerator, type, localBuilder, label4, label);
			}
			ilgenerator.MarkLabel(label4);
			DirectXmlToObjectNew.EmitIlToCreateAndPopulateComplexType(ilgenerator, type, localBuilder);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
			if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Box, type);
			}
			ilgenerator.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.IListAddItemMethod);
			ilgenerator.Emit(OpCodes.Pop);
			ilgenerator.Emit(OpCodes.Ret);
			return (DirectXmlToObjectNew.ParseValueAndAddListItemDelegate)dynamicMethod.CreateDelegate(typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate));
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x0011944C File Offset: 0x0011764C
		private static void EmitIlToCreateCustomLoadable(ILGenerator il, Type typeToInstantiate)
		{
			ConstructorInfo constructor = typeToInstantiate.GetConstructor(Type.EmptyTypes);
			MethodInfo methodInfo = XmlToObjectUtils.CustomDataLoadMethodOf(typeToInstantiate);
			if (constructor == null)
			{
				throw new InvalidOperationException("Type " + typeToInstantiate.FullName + " does not have a parameterless constructor, but needs one to use LoadDataFromXmlCustom.");
			}
			if (methodInfo == null)
			{
				throw new InvalidOperationException("Type " + typeToInstantiate.FullName + " does not have a method named LoadDataFromXmlCustom, but we are trying to create a parser for it using that method.");
			}
			il.Emit(OpCodes.Newobj, constructor);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.XmlInheritanceGetResolvedNodeForMethod);
			il.Emit(OpCodes.Call, methodInfo);
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x001194F4 File Offset: 0x001176F4
		private static void EmitIlToCreateSlateRef(ILGenerator il, Type slateRefType)
		{
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.InnerTextWithReplacedNewlinesOrXmlMethod);
			il.Emit(OpCodes.Ldtoken, slateRefType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
		}

		// Token: 0x06003544 RID: 13636 RVA: 0x00119548 File Offset: 0x00117748
		private static void EmitIlToCreateString(ILGenerator il)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNodeType));
			LocalBuilder local2 = il.DeclareLocal(typeof(string));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			Label label5 = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeHasChildNodesMethod);
			il.Emit(OpCodes.Brtrue, label);
			il.Emit(OpCodes.Ldstr, "");
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Br, label5);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Beq, label2);
			il.Emit(OpCodes.Ldstr, "XML node has more than one child node, which is unsupported for string parsing. Context: ");
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetOuterXmlMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringConcatMethod);
			il.Emit(OpCodes.Newobj, DirectXmlToObjectNew.InvalidOperationExceptionStringConstructor);
			il.Emit(OpCodes.Throw);
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNodeTypeMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Beq, label3);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Ldc_I4, 4);
			il.Emit(OpCodes.Beq, label4);
			il.Emit(OpCodes.Ldstr, "XML node has an unsupported child node type to parse as string. Expected Text or CDATA. Context: ");
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetOuterXmlMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringConcatMethod);
			il.Emit(OpCodes.Newobj, DirectXmlToObjectNew.InvalidOperationExceptionStringConstructor);
			il.Emit(OpCodes.Throw);
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldtoken, typeof(string));
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
			il.Emit(OpCodes.Castclass, typeof(string));
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Br, label5);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetValueMethod);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Br, label5);
			il.MarkLabel(label5);
			il.Emit(OpCodes.Ldloc, local2);
		}

		// Token: 0x06003545 RID: 13637 RVA: 0x00119850 File Offset: 0x00117A50
		private static void EmitIlToParseFlagsEnum(ILGenerator il, Type enumType)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local2 = il.DeclareLocal(enumType);
			LocalBuilder local3 = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local4 = il.DeclareLocal(typeof(int));
			LocalBuilder local5 = il.DeclareLocal(typeof(int));
			LocalBuilder local6 = il.DeclareLocal(enumType);
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local5);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Bge, label3);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldtoken, enumType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateListNodeMethod);
			il.Emit(OpCodes.Brfalse, label2);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldtoken, enumType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
			il.Emit(OpCodes.Unbox_Any, enumType);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Or);
			il.Emit(OpCodes.Stloc, local2);
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Box, enumType);
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x00119B0C File Offset: 0x00117D0C
		private static void EmitIlToCreateAndPopulateList(ILGenerator il, Type listType, Type itemType)
		{
			ConstructorInfo constructor = listType.GetConstructor(new Type[]
			{
				typeof(int)
			});
			bool flag = GenTypes.IsDef(itemType);
			LocalBuilder local = il.DeclareLocal(listType);
			LocalBuilder local2 = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local3 = il.DeclareLocal(typeof(int));
			LocalBuilder local4 = il.DeclareLocal(typeof(int));
			LocalBuilder local5 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local6 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local7 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local8 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local9 = il.DeclareLocal(typeof(string));
			LocalBuilder local10 = il.DeclareLocal(typeof(string));
			il.DeclareLocal(typeof(XmlAttribute));
			il.DeclareLocal(typeof(string));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			Label label5 = il.DefineLabel();
			Label label6 = il.DefineLabel();
			LocalBuilder local11 = il.DeclareLocal(typeof(Type));
			il.DefineLabel();
			Label loc = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "IsNull");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Ldstr, "true");
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringEqualsWithComparisonModeMethod);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Br, label4);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Newobj, constructor);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local4);
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Bge, label4);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldtoken, itemType);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateListNodeMethod);
			il.Emit(OpCodes.Brfalse, label3);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local9);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local10);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequire");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local7);
			il.Emit(OpCodes.Brfalse, label5);
			il.Emit(OpCodes.Ldloc, local7);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Stloc, local9);
			il.MarkLabel(label5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequireAnyOf");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local8);
			il.Emit(OpCodes.Brfalse, label6);
			il.Emit(OpCodes.Ldloc, local8);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Stloc, local10);
			il.MarkLabel(label6);
			if (flag)
			{
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Castclass, listType);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNameMethod);
				il.Emit(OpCodes.Box, typeof(string));
				il.Emit(OpCodes.Ldloc, local9);
				il.Emit(OpCodes.Ldloc, local10);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.RegisterListWantsCrossRefMethod.MakeGenericMethod(new Type[]
				{
					itemType
				}));
			}
			else
			{
				il.Emit(OpCodes.Ldloc, local9);
				il.Emit(OpCodes.Ldloc, local10);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateMayRequiresMethod);
				il.Emit(OpCodes.Brfalse, label3);
				il.Emit(OpCodes.Ldtoken, itemType);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveTypeForNodeFromTypeMethod);
				il.Emit(OpCodes.Stloc, local11);
				il.MarkLabel(loc);
				il.Emit(OpCodes.Ldloc, local11);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetListItemAdderForTypeMethod);
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Ldc_I4_0);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldloc, local11);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndAddListItemDelegateInvokeMethod);
			}
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldloc, local);
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x0011A1B0 File Offset: 0x001183B0
		private static void EmitILToCreateAndPopulateDictionary(ILGenerator il, Type dictionaryType, Type keyType, Type valueType)
		{
			bool flag = GenTypes.IsDef(keyType);
			bool flag2 = GenTypes.IsDef(valueType);
			bool flag3 = !flag && !flag2;
			Type type = typeof(KeyValuePair<, >).MakeGenericType(new Type[]
			{
				keyType,
				valueType
			});
			MethodInfo getMethod = type.GetProperty("Key").GetGetMethod();
			MethodInfo getMethod2 = type.GetProperty("Value").GetGetMethod();
			ConstructorInfo constructor = dictionaryType.GetConstructor(new Type[]
			{
				typeof(int)
			});
			LocalBuilder local = il.DeclareLocal(dictionaryType);
			LocalBuilder local2 = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local3 = il.DeclareLocal(typeof(int));
			LocalBuilder local4 = il.DeclareLocal(typeof(int));
			LocalBuilder local5 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local6 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local7 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local8 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local9 = il.DeclareLocal(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
			LocalBuilder local10 = il.DeclareLocal(typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate));
			LocalBuilder local11 = il.DeclareLocal(type);
			LocalBuilder local12 = il.DeclareLocal(typeof(object));
			LocalBuilder local13 = il.DeclareLocal(typeof(FieldInfo));
			LocalBuilder local14 = il.DeclareLocal(typeof(FieldInfo));
			LocalBuilder local15 = il.DeclareLocal(typeof(Type));
			LocalBuilder local16 = il.DeclareLocal(typeof(Type));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			if (flag3)
			{
				il.Emit(OpCodes.Ldloca, local11);
				il.Emit(OpCodes.Initobj, type);
				il.Emit(OpCodes.Ldtoken, type);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldstr, "key");
				il.Emit(OpCodes.Ldc_I4, 36);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.TypeGetFieldMethod);
				il.Emit(OpCodes.Stloc, local13);
				il.Emit(OpCodes.Ldtoken, type);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldstr, "value");
				il.Emit(OpCodes.Ldc_I4, 36);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.TypeGetFieldMethod);
				il.Emit(OpCodes.Stloc, local14);
			}
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "IsNull");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local8);
			il.Emit(OpCodes.Brfalse, label4);
			il.Emit(OpCodes.Ldloc, local8);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Ldstr, "true");
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringEqualsWithComparisonModeMethod);
			il.Emit(OpCodes.Brfalse, label4);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Br, label3);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Newobj, constructor);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local4);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Bge, label3);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Ldtoken, type);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ValidateListNodeMethod);
			il.Emit(OpCodes.Brfalse, label2);
			if (flag3)
			{
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldstr, "key");
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetItemByNameMethod);
				il.Emit(OpCodes.Stloc, local6);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldstr, "value");
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetItemByNameMethod);
				il.Emit(OpCodes.Stloc, local7);
				il.Emit(OpCodes.Ldloc, local11);
				il.Emit(OpCodes.Box, type);
				il.Emit(OpCodes.Stloc, local12);
				il.Emit(OpCodes.Ldtoken, keyType);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldloc, local6);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.ResolveTypeForNodeFromTypeMethod);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, local15);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetFieldSetterForTypeMethod);
				il.Emit(OpCodes.Stloc, local9);
				il.Emit(OpCodes.Ldloc, local9);
				il.Emit(OpCodes.Ldloc, local12);
				il.Emit(OpCodes.Ldloc, local13);
				il.Emit(OpCodes.Ldloc, local6);
				il.Emit(OpCodes.Ldloc, local15);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndSetFieldDelegateInvokeMethod);
				il.Emit(OpCodes.Ldtoken, valueType);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.TypeGetTypeFromHandleMethod);
				il.Emit(OpCodes.Ldloc, local7);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.ResolveTypeForNodeFromTypeMethod);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, local16);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetFieldSetterForTypeMethod);
				il.Emit(OpCodes.Stloc, local10);
				il.Emit(OpCodes.Ldloc, local10);
				il.Emit(OpCodes.Ldloc, local12);
				il.Emit(OpCodes.Ldloc, local14);
				il.Emit(OpCodes.Ldloc, local7);
				il.Emit(OpCodes.Ldloc, local16);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndSetFieldDelegateInvokeMethod);
				il.Emit(OpCodes.Ldloc, local12);
				il.Emit(OpCodes.Unbox_Any, type);
				il.Emit(OpCodes.Stloc, local11);
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Ldloca, local11);
				il.Emit(OpCodes.Call, getMethod);
				if (keyType.IsValueType)
				{
					il.Emit(OpCodes.Box, keyType);
				}
				il.Emit(OpCodes.Ldloca, local11);
				il.Emit(OpCodes.Call, getMethod2);
				if (valueType.IsValueType)
				{
					il.Emit(OpCodes.Box, valueType);
				}
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.DictionaryAddMethod);
			}
			else
			{
				MethodInfo meth = DirectXmlToObjectNew.RegisterDictionaryWantsCrossRefMethod.MakeGenericMethod(new Type[]
				{
					keyType,
					valueType
				});
				il.Emit(OpCodes.Ldloc, local);
				il.Emit(OpCodes.Ldloc, local5);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNameMethod);
				il.Emit(OpCodes.Call, meth);
			}
			il.MarkLabel(label2);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			il.Emit(OpCodes.Ldloc, local);
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x0011A9C4 File Offset: 0x00118BC4
		private static void EmitIlToCreateAndPopulateComplexType(ILGenerator il, Type typeBeingDeserialized, LocalBuilder instantiatedObjectLocal)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNodeList));
			LocalBuilder local2 = il.DeclareLocal(typeof(int));
			LocalBuilder local3 = il.DeclareLocal(typeof(int));
			LocalBuilder local4 = il.DeclareLocal(typeof(XmlNode));
			LocalBuilder local5 = il.DeclareLocal(typeof(FieldInfo));
			LocalBuilder local6 = il.DeclareLocal(typeof(Type));
			LocalBuilder localBuilder = il.DeclareLocal(typeof(object));
			LocalBuilder local7 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local8 = il.DeclareLocal(typeof(XmlAttribute));
			LocalBuilder local9 = il.DeclareLocal(typeof(string));
			LocalBuilder local10 = il.DeclareLocal(typeof(string));
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			Label label3 = il.DefineLabel();
			Label label4 = il.DefineLabel();
			Label label5 = il.DefineLabel();
			Label label6 = il.DefineLabel();
			Label label7 = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.XmlInheritanceGetResolvedNodeForMethod);
			il.Emit(OpCodes.Starg_S, 2);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetChildNodesMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetCountMethod);
			il.Emit(OpCodes.Stloc, local2);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloca, instantiatedObjectLocal);
				il.Emit(OpCodes.Initobj, typeBeingDeserialized);
			}
			else
			{
				il.Emit(OpCodes.Ldarg_3);
				il.Emit(OpCodes.Ldarg_2);
				il.Emit(OpCodes.Call, DirectXmlToObjectNew.MakeInstanceOfTypeForEmptyNodeMethod);
				il.Emit(OpCodes.Stloc, instantiatedObjectLocal);
			}
			il.Emit(OpCodes.Ldc_I4_0);
			il.Emit(OpCodes.Stloc, local3);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Ldloc, local2);
			il.Emit(OpCodes.Bge, label3);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloc, instantiatedObjectLocal);
				il.Emit(OpCodes.Box, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, localBuilder);
			}
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeListGetItemMethod);
			il.Emit(OpCodes.Stloc, local4);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Isinst, typeof(XmlComment));
			il.Emit(OpCodes.Brtrue, label2);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveFieldForNodeMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local5);
			il.Emit(OpCodes.Brfalse, label2);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ResolveTypeForNodeFromFieldMethod);
			il.Emit(OpCodes.Stloc, local6);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GenTypesIsDefMethod);
			il.Emit(OpCodes.Brfalse, label4);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GenTextIsNullOrEmptyMethod);
			il.Emit(OpCodes.Brfalse, label5);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, typeBeingDeserialized.IsValueType ? localBuilder : instantiatedObjectLocal);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.FieldSetValueMethod);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label5);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local9);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Stloc, local10);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequire");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local7);
			il.Emit(OpCodes.Brfalse, label6);
			il.Emit(OpCodes.Ldloc, local7);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringToLowerMethod);
			il.Emit(OpCodes.Stloc, local9);
			il.MarkLabel(label6);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "MayRequireAnyOf");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local8);
			il.Emit(OpCodes.Brfalse, label7);
			il.Emit(OpCodes.Ldloc, local8);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringToLowerMethod);
			il.Emit(OpCodes.Stloc, local10);
			il.MarkLabel(label7);
			il.Emit(OpCodes.Ldloc, typeBeingDeserialized.IsValueType ? localBuilder : instantiatedObjectLocal);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldloc, local9);
			il.Emit(OpCodes.Ldloc, local10);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.RegisterObjectWantsCrossRefMethod);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label4);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetFieldSetterForTypeMethod);
			il.Emit(OpCodes.Ldloc, typeBeingDeserialized.IsValueType ? localBuilder : instantiatedObjectLocal);
			il.Emit(OpCodes.Ldloc, local5);
			il.Emit(OpCodes.Ldloc, local4);
			il.Emit(OpCodes.Ldloc, local6);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseValueAndSetFieldDelegateInvokeMethod);
			il.MarkLabel(label2);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloc, localBuilder);
				il.Emit(OpCodes.Unbox_Any, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, instantiatedObjectLocal);
			}
			il.Emit(OpCodes.Ldloc, local3);
			il.Emit(OpCodes.Ldc_I4_1);
			il.Emit(OpCodes.Add);
			il.Emit(OpCodes.Stloc, local3);
			il.Emit(OpCodes.Br, label);
			il.MarkLabel(label3);
			MethodInfo methodInfo = XmlToObjectUtils.PostLoadMethodOf(typeBeingDeserialized);
			if (methodInfo != null)
			{
				il.Emit(OpCodes.Ldloc, instantiatedObjectLocal);
				il.Emit(OpCodes.Call, methodInfo);
				return;
			}
			il.Emit(OpCodes.Nop);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x0011B0C4 File Offset: 0x001192C4
		private static void EmitIlToHandleEmptyNode(ILGenerator il, Type typeBeingDeserialized, LocalBuilder localForResultValue, Label labelToJumpIfNotEmpty, Label endLabel)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlAttribute));
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetAttributesMethod);
			il.Emit(OpCodes.Ldstr, "IsNull");
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeCollectionGetItemByNameMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlAttributeGetValueMethod);
			il.Emit(OpCodes.Ldstr, "true");
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringEqualsWithComparisonModeMethod);
			il.Emit(OpCodes.Brfalse, label);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloca, localForResultValue);
				il.Emit(OpCodes.Initobj, typeBeingDeserialized);
			}
			else
			{
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			il.Emit(OpCodes.Br, endLabel);
			il.MarkLabel(label);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeHasChildNodesMethod);
			il.Emit(OpCodes.Brtrue, labelToJumpIfNotEmpty);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.MakeInstanceOfTypeForEmptyNodeMethod);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, typeBeingDeserialized);
			}
			il.Emit(OpCodes.Stloc, localForResultValue);
			il.Emit(OpCodes.Br, endLabel);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x0011B264 File Offset: 0x00119464
		private static void EmitIlToErrorAndMakeDefaultValueForCdata(ILGenerator il, Type typeBeingDeserialized, Label labelToJumpIfNotCdata, Label labelToJumpAfterCheck, LocalBuilder localForResultValue)
		{
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Brfalse, labelToJumpIfNotCdata);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetFirstChildMethod);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNodeTypeMethod);
			il.Emit(OpCodes.Ldc_I4, 4);
			il.Emit(OpCodes.Bne_Un, labelToJumpIfNotCdata);
			il.Emit(OpCodes.Ldstr, "CDATA can only be used for strings. Bad xml: ");
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetOuterXmlMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.StringConcatMethod);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.LogErrorMethod);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Ldloca_S, localForResultValue);
				il.Emit(OpCodes.Initobj, typeBeingDeserialized);
			}
			else
			{
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			il.Emit(OpCodes.Br, labelToJumpAfterCheck);
		}

		// Token: 0x0600354B RID: 13643 RVA: 0x0011B370 File Offset: 0x00119570
		private static void EmitIlToHandleSingleTextNodeViaParseHelper(ILGenerator il, Type typeBeingDeserialized, LocalBuilder localForResultValue, Label labelToJumpIfNotText, Label endLabel)
		{
			LocalBuilder local = il.DeclareLocal(typeof(XmlNode));
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.GetNodeOnlyChildMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Stloc, local);
			il.Emit(OpCodes.Brfalse, labelToJumpIfNotText);
			il.Emit(OpCodes.Ldloc, local);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetNodeTypeMethod);
			il.Emit(OpCodes.Ldc_I4, 3);
			il.Emit(OpCodes.Bne_Un, labelToJumpIfNotText);
			il.Emit(OpCodes.Ldarg_2);
			il.Emit(OpCodes.Callvirt, DirectXmlToObjectNew.XmlNodeGetInnerTextMethod);
			il.Emit(OpCodes.Ldarg_3);
			il.Emit(OpCodes.Call, DirectXmlToObjectNew.ParseHelperFromStringNonGenericMethod);
			if (typeBeingDeserialized.IsValueType)
			{
				il.Emit(OpCodes.Unbox_Any, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			else
			{
				il.Emit(OpCodes.Castclass, typeBeingDeserialized);
				il.Emit(OpCodes.Stloc, localForResultValue);
			}
			il.Emit(OpCodes.Br, endLabel);
		}

		// Token: 0x0600354C RID: 13644 RVA: 0x0011B47D File Offset: 0x0011967D
		public static FieldInfo ResolveFieldForNode(Type typeBeingDeserialized, XmlNode node, XmlNode parentForDebug)
		{
			return XmlToObjectUtils.DoFieldSearch(typeBeingDeserialized.GetInnerIfNullable(), node, parentForDebug);
		}

		// Token: 0x0600354D RID: 13645 RVA: 0x0011B48C File Offset: 0x0011968C
		public static Type ResolveTypeForNode(FieldInfo fieldBeingSet, XmlNode node)
		{
			return DirectXmlToObjectNew.ResolveTypeForNode(fieldBeingSet.FieldType, node);
		}

		// Token: 0x0600354E RID: 13646 RVA: 0x0011B49C File Offset: 0x0011969C
		public static Type ResolveTypeForNode(Type defaultType, XmlNode node)
		{
			XmlAttributeCollection attributes = node.Attributes;
			XmlAttribute xmlAttribute = (attributes != null) ? attributes["Class"] : null;
			if (xmlAttribute == null)
			{
				return defaultType;
			}
			Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly(xmlAttribute.Value, defaultType.GetInnerIfNullable().Namespace);
			if (typeInAnyAssembly != null)
			{
				return typeInAnyAssembly;
			}
			throw new ArgumentException("Could not find type named " + xmlAttribute.Value + " from node " + node.OuterXml, "node");
		}

		// Token: 0x0600354F RID: 13647 RVA: 0x0011B510 File Offset: 0x00119710
		public static XmlNode GetNodeOnlyChild(XmlNode node)
		{
			XmlNode firstChild = node.FirstChild;
			if (firstChild == null || firstChild.NextSibling != null)
			{
				return null;
			}
			return firstChild;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x0011B534 File Offset: 0x00119734
		public static object MakeInstanceOfTypeForEmptyNode(Type type, XmlNode node)
		{
			type = DirectXmlToObjectNew.ResolveTypeForNode(type, node);
			if (type.IsValueType)
			{
				return RuntimeHelpers.GetUninitializedObject(type);
			}
			object result;
			try
			{
				result = Activator.CreateInstance(type);
			}
			catch (MissingMethodException innerException)
			{
				throw new InvalidOperationException(string.Concat(new string[]
				{
					"Cannot deserialize XML node ",
					node.OuterXml,
					" to type ",
					type.FullName,
					" due to missing parameterless constructor."
				}), innerException);
			}
			return result;
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x0011B5B4 File Offset: 0x001197B4
		public static bool ValidateMayRequires(string mayRequire, string mayRequireAny)
		{
			if (!mayRequire.NullOrEmpty() && !ModsConfig.AreAllActive(mayRequire))
			{
				if (DirectXmlCrossRefLoader.MistypedMayRequire(mayRequire))
				{
					Log.Error("Faulty MayRequire: " + mayRequire);
				}
				return false;
			}
			return mayRequireAny.NullOrEmpty() || ModsConfig.IsAnyActiveOrEmpty(mayRequireAny.Split(',', StringSplitOptions.None), true);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x0011B607 File Offset: 0x00119807
		private static string GetDynamicMethodNameSuffixForType(Type type)
		{
			string fullName = type.FullName;
			return ((fullName != null) ? fullName.Replace('.', '_') : null) ?? "UnknownType";
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x0011B628 File Offset: 0x00119828
		private static Type GetInnerIfNullable(this Type type)
		{
			return Nullable.GetUnderlyingType(type) ?? type;
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x0011B638 File Offset: 0x00119838
		private static bool TypeCanBeDeserializedWithSharedBody(Type type)
		{
			if (type.IsValueType)
			{
				return false;
			}
			if (type.IsGenericType || type.IsConstructedGenericType)
			{
				return false;
			}
			if (XmlToObjectUtils.CustomDataLoadMethodOf(type) != null)
			{
				return false;
			}
			MethodInfo methodInfo = XmlToObjectUtils.PostLoadMethodOf(type);
			return (methodInfo == null || !(methodInfo.DeclaringType != typeof(Editable))) && !ParseHelper.HandlesType(type);
		}

		// Token: 0x04002806 RID: 10246
		private static readonly MethodInfo XmlNodeGetChildNodesMethod = typeof(XmlNode).GetProperty("ChildNodes").GetGetMethod();

		// Token: 0x04002807 RID: 10247
		private static readonly MethodInfo XmlNodeGetInnerTextMethod = typeof(XmlNode).GetProperty("InnerText").GetGetMethod();

		// Token: 0x04002808 RID: 10248
		private static readonly MethodInfo XmlNodeGetFirstChildMethod = typeof(XmlNode).GetProperty("FirstChild").GetGetMethod();

		// Token: 0x04002809 RID: 10249
		private static readonly MethodInfo XmlNodeHasChildNodesMethod = typeof(XmlNode).GetProperty("HasChildNodes").GetGetMethod();

		// Token: 0x0400280A RID: 10250
		private static readonly MethodInfo XmlNodeGetOuterXmlMethod = typeof(XmlNode).GetProperty("OuterXml").GetGetMethod();

		// Token: 0x0400280B RID: 10251
		private static readonly MethodInfo XmlNodeGetNodeTypeMethod = typeof(XmlNode).GetProperty("NodeType").GetGetMethod();

		// Token: 0x0400280C RID: 10252
		private static readonly MethodInfo XmlNodeGetValueMethod = typeof(XmlNode).GetProperty("Value").GetGetMethod();

		// Token: 0x0400280D RID: 10253
		private static readonly MethodInfo XmlNodeGetNameMethod = typeof(XmlNode).GetProperty("Name").GetGetMethod();

		// Token: 0x0400280E RID: 10254
		private static readonly MethodInfo XmlNodeGetAttributesMethod = typeof(XmlNode).GetProperty("Attributes").GetGetMethod();

		// Token: 0x0400280F RID: 10255
		private static readonly MethodInfo XmlNodeGetItemByNameMethod = typeof(XmlNode).GetMethod("get_Item", new Type[]
		{
			typeof(string)
		});

		// Token: 0x04002810 RID: 10256
		private static readonly MethodInfo XmlNodeListGetCountMethod = typeof(XmlNodeList).GetProperty("Count").GetGetMethod();

		// Token: 0x04002811 RID: 10257
		private static readonly MethodInfo XmlNodeListGetItemMethod = typeof(XmlNodeList).GetMethod("Item", new Type[]
		{
			typeof(int)
		});

		// Token: 0x04002812 RID: 10258
		private static readonly MethodInfo XmlAttributeCollectionGetItemByNameMethod = typeof(XmlAttributeCollection).GetMethod("get_ItemOf", new Type[]
		{
			typeof(string)
		});

		// Token: 0x04002813 RID: 10259
		private static readonly MethodInfo XmlAttributeGetValueMethod = typeof(XmlAttribute).GetProperty("Value").GetGetMethod();

		// Token: 0x04002814 RID: 10260
		private static readonly MethodInfo FieldSetValueMethod = typeof(FieldInfo).GetMethod("SetValue", new Type[]
		{
			typeof(object),
			typeof(object)
		});

		// Token: 0x04002815 RID: 10261
		private static readonly MethodInfo IListAddItemMethod = typeof(IList).GetMethod("Add", new Type[]
		{
			typeof(object)
		});

		// Token: 0x04002816 RID: 10262
		private static readonly MethodInfo ValidateListNodeMethod = typeof(DirectXmlToObject).GetMethod("ValidateListNode", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002817 RID: 10263
		private static readonly MethodInfo ParseHelperFromStringNonGenericMethod = typeof(ParseHelper).GetMethod("FromString", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(string),
			typeof(Type)
		}, null);

		// Token: 0x04002818 RID: 10264
		private static readonly MethodInfo InnerTextWithReplacedNewlinesOrXmlMethod = typeof(DirectXmlToObject).GetMethod("InnerTextWithReplacedNewlinesOrXML", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002819 RID: 10265
		private static readonly MethodInfo LogMessageMethod = typeof(Log).GetMethod("Message", new Type[]
		{
			typeof(string)
		});

		// Token: 0x0400281A RID: 10266
		private static readonly MethodInfo LogErrorMethod = typeof(Log).GetMethod("Error", new Type[]
		{
			typeof(string)
		});

		// Token: 0x0400281B RID: 10267
		private static readonly MethodInfo TypeGetTypeFromHandleMethod = typeof(Type).GetMethod("GetTypeFromHandle");

		// Token: 0x0400281C RID: 10268
		private static readonly MethodInfo TypeGetFieldMethod = typeof(Type).GetMethod("GetField", new Type[]
		{
			typeof(string),
			typeof(BindingFlags)
		});

		// Token: 0x0400281D RID: 10269
		private static readonly MethodInfo StringConcatMethod = typeof(string).GetMethod("Concat", new Type[]
		{
			typeof(string),
			typeof(string)
		});

		// Token: 0x0400281E RID: 10270
		private static readonly MethodInfo StringConcatObjObjMethod = typeof(string).GetMethod("Concat", new Type[]
		{
			typeof(object),
			typeof(object)
		});

		// Token: 0x0400281F RID: 10271
		private static readonly MethodInfo StringToLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

		// Token: 0x04002820 RID: 10272
		private static readonly MethodInfo StringEqualsWithComparisonModeMethod = typeof(string).GetMethod("Equals", new Type[]
		{
			typeof(string),
			typeof(StringComparison)
		});

		// Token: 0x04002821 RID: 10273
		private static readonly MethodInfo XmlInheritanceGetResolvedNodeForMethod = typeof(XmlInheritance).GetMethod("GetResolvedNodeFor", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002822 RID: 10274
		private static readonly MethodInfo DictionaryAddMethod = typeof(IDictionary).GetMethod("Add");

		// Token: 0x04002823 RID: 10275
		private static readonly MethodInfo RegisterObjectWantsCrossRefMethod = typeof(DirectXmlCrossRefLoader).GetMethod("RegisterObjectWantsCrossRef", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(object),
			typeof(FieldInfo),
			typeof(string),
			typeof(string),
			typeof(string),
			typeof(Type)
		}, null);

		// Token: 0x04002824 RID: 10276
		private static readonly MethodInfo RegisterDictionaryWantsCrossRefMethod = typeof(DirectXmlCrossRefLoader).GetMethods(BindingFlags.Static | BindingFlags.Public).First((MethodInfo m) => m.Name == "RegisterDictionaryWantsCrossRef");

		// Token: 0x04002825 RID: 10277
		private static readonly MethodInfo RegisterListWantsCrossRefMethod = typeof(DirectXmlCrossRefLoader).GetMethods(BindingFlags.Static | BindingFlags.Public).First((MethodInfo m) => m.Name == "RegisterListWantsCrossRef");

		// Token: 0x04002826 RID: 10278
		private static readonly MethodInfo GenTypesIsDefMethod = typeof(GenTypes).GetMethod("IsDef", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002827 RID: 10279
		private static readonly MethodInfo GenTypesGetTypeInAnyAssemblyMethod = typeof(GenTypes).GetMethod("GetTypeInAnyAssembly", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002828 RID: 10280
		private static readonly MethodInfo GenTextIsNullOrEmptyMethod = typeof(GenText).GetMethod("NullOrEmpty", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002829 RID: 10281
		private static readonly MethodInfo GetFieldSetterForTypeMethod = typeof(DirectXmlToObjectNew).GetMethod("GetFieldSetterForType", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400282A RID: 10282
		private static readonly MethodInfo GetListItemAdderForTypeMethod = typeof(DirectXmlToObjectNew).GetMethod("GetListItemAdderForType", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400282B RID: 10283
		private static readonly MethodInfo ResolveFieldForNodeMethod = typeof(DirectXmlToObjectNew).GetMethod("ResolveFieldForNode", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400282C RID: 10284
		private static readonly MethodInfo ResolveTypeForNodeFromFieldMethod = typeof(DirectXmlToObjectNew).GetMethod("ResolveTypeForNode", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(FieldInfo),
			typeof(XmlNode)
		}, null);

		// Token: 0x0400282D RID: 10285
		private static readonly MethodInfo ResolveTypeForNodeFromTypeMethod = typeof(DirectXmlToObjectNew).GetMethod("ResolveTypeForNode", BindingFlags.Static | BindingFlags.Public, null, new Type[]
		{
			typeof(Type),
			typeof(XmlNode)
		}, null);

		// Token: 0x0400282E RID: 10286
		private static readonly MethodInfo MakeInstanceOfTypeForEmptyNodeMethod = typeof(DirectXmlToObjectNew).GetMethod("MakeInstanceOfTypeForEmptyNode", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x0400282F RID: 10287
		private static readonly MethodInfo GetNodeOnlyChildMethod = typeof(DirectXmlToObjectNew).GetMethod("GetNodeOnlyChild", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002830 RID: 10288
		private static readonly MethodInfo ValidateMayRequiresMethod = typeof(DirectXmlToObjectNew).GetMethod("ValidateMayRequires", BindingFlags.Static | BindingFlags.Public);

		// Token: 0x04002831 RID: 10289
		private static readonly MethodInfo ParseValueAndSetFieldDelegateInvokeMethod = typeof(DirectXmlToObjectNew.ParseValueAndSetFieldDelegate).GetMethod("Invoke");

		// Token: 0x04002832 RID: 10290
		private static readonly MethodInfo ParseValueAndAddListItemDelegateInvokeMethod = typeof(DirectXmlToObjectNew.ParseValueAndAddListItemDelegate).GetMethod("Invoke");

		// Token: 0x04002833 RID: 10291
		private static readonly MethodInfo ParseValueAndReturnDefDelegateInvokeMethod = typeof(DirectXmlToObjectNew.ParseValueAndReturnDefDelegate).GetMethod("Invoke");

		// Token: 0x04002834 RID: 10292
		private static readonly ConstructorInfo InvalidOperationExceptionStringConstructor = typeof(InvalidOperationException).GetConstructor(new Type[]
		{
			typeof(string)
		});

		// Token: 0x04002835 RID: 10293
		private static readonly Dictionary<Type, DirectXmlToObjectNew.ParseValueAndSetFieldDelegate> parseMethods = new Dictionary<Type, DirectXmlToObjectNew.ParseValueAndSetFieldDelegate>();

		// Token: 0x04002836 RID: 10294
		private static readonly Dictionary<Type, DirectXmlToObjectNew.ParseValueAndAddListItemDelegate> parseListItemMethods = new Dictionary<Type, DirectXmlToObjectNew.ParseValueAndAddListItemDelegate>();

		// Token: 0x04002837 RID: 10295
		private static readonly Dictionary<Type, DirectXmlToObjectNew.ParseValueAndReturnDefDelegate> parseDefMethods = new Dictionary<Type, DirectXmlToObjectNew.ParseValueAndReturnDefDelegate>();

		// Token: 0x02000831 RID: 2097
		// (Invoke) Token: 0x06003557 RID: 13655
		public delegate void ParseValueAndSetFieldDelegate(object target, FieldInfo field, XmlNode node, Type typeBeingDeserialized);

		// Token: 0x02000832 RID: 2098
		// (Invoke) Token: 0x0600355B RID: 13659
		public delegate void ParseValueAndAddListItemDelegate(IList target, int unused, XmlNode node, Type itemType);

		// Token: 0x02000833 RID: 2099
		// (Invoke) Token: 0x0600355F RID: 13663
		public delegate Def ParseValueAndReturnDefDelegate(int unused, int unused2, XmlNode node, Type defType);

		// Token: 0x02000834 RID: 2100
		private class DummyTypeToHoldDynamicMethods
		{
		}
	}
}
