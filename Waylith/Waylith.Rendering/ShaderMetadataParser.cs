using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Text.RegularExpressions.Generated;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public static class ShaderMetadataParser
{
	private readonly record struct Varying(string Name, string Type, char BaseType, int Components);

	private const int MaxSamplersPerStage = 16;

	public static MaterialLayout ExtractMaterialLayout(ReadOnlySpan<byte> sourceBytes, string shaderName, int materialSet = 2)
	{
		return ExtractMaterialLayout(Encoding.UTF8.GetString(sourceBytes), shaderName, materialSet);
	}

	public static MaterialLayout ExtractMaterialLayout(string source, string shaderName, int materialSet = 2)
	{
		bool loggedErrors;
		return ExtractMaterialLayout(source, shaderName, materialSet, out loggedErrors);
	}

	internal static MaterialLayout ExtractMaterialLayout(string source, string shaderName, int materialSet, out bool loggedErrors)
	{
		source = StripBlockComments(source);
		List<MaterialUniformSlot> list = new List<MaterialUniformSlot>();
		List<MaterialTextureSlot> list2 = new List<MaterialTextureSlot>();
		List<MaterialStorageBufferSlot> list3 = new List<MaterialStorageBufferSlot>();
		int uniformSize = 0;
		bool hasErrors = false;
		string source2 = StripLineComments(source);
		Dictionary<string, string> textureComments = CollectTextureComments(source, materialSet);
		ExtractTextureSlots(shaderName, source2, materialSet, list2, textureComments, ref hasErrors);
		ExtractStorageBufferSlots(source2, materialSet, list3);
		bool flag = CheckSamplerCount(shaderName, source2);
		foreach (Match item in UniformBlockRegex().Matches(source))
		{
			string value = item.Groups["layout"].Value;
			if (TryGetLayoutSet(value, out var value2) && value2 == materialSet && TryGetLayoutBinding(value, out var value3) && value3 == 0)
			{
				uniformSize = ExtractUniformBlockSlots(shaderName, item.Groups["body"].Value, list2, list, ref hasErrors);
			}
		}
		MaterialLayout materialLayout = new MaterialLayout(list.ToArray(), list2.ToArray(), uniformSize, list3.ToArray(), hasErrors);
		loggedErrors = materialLayout.HasErrors || flag;
		if (!materialLayout.HasErrors)
		{
			return materialLayout;
		}
		return new MaterialLayout(Array.Empty<MaterialUniformSlot>(), Array.Empty<MaterialTextureSlot>(), 0, null, hasErrors: true);
	}

	public static ShaderVertexInputLayout ExtractVertexInputLayout(string source, string shaderName)
	{
		source = StripBlockComments(source);
		List<ShaderVertexInputSlot> list = new List<ShaderVertexInputSlot>();
		List<ShaderInstanceInputSlot> list2 = new List<ShaderInstanceInputSlot>();
		bool hasErrors = false;
		foreach (Match item in ShaderVertexInputRegex().Matches(source))
		{
			string value = item.Groups["name"].Value;
			string value2 = item.Groups["kind"].Value;
			if (string.Equals(value2, "instance", StringComparison.Ordinal))
			{
				AddInstanceInputSlot(shaderName, item, value, list2, ref hasErrors);
				continue;
			}
			if (!string.Equals(value2, "vertex", StringComparison.Ordinal))
			{
				hasErrors |= LogError($"Shader '{shaderName}' vertex input '{value}' is missing a '// @vertex <semantic> required|optional' annotation.");
			}
			string value3 = item.Groups["semantic"].Value;
			if (!Enum.TryParse<VertexAttributeSemantic>(value3, ignoreCase: false, out var result))
			{
				hasErrors |= LogError($"Shader '{shaderName}' vertex input '{value}' has unknown vertex semantic '{value3}'.");
			}
			string value4 = item.Groups["presence"].Value;
			if (string.IsNullOrWhiteSpace(value4))
			{
				hasErrors |= LogError($"Shader '{shaderName}' vertex input '{value}' must be annotated as required or optional.");
			}
			bool flag = string.Equals(value4, "required", StringComparison.Ordinal);
			if (!flag && !string.Equals(value4, "optional", StringComparison.Ordinal))
			{
				hasErrors |= LogError($"Shader '{shaderName}' vertex input '{value}' must be annotated as required or optional.");
			}
			float[] components;
			bool flag2 = TryReadDefault(shaderName, "vertex input '" + value + "'", item.Groups["type"].Value, item.Groups["tail"].Value, ref hasErrors, out components, "vec4");
			if (flag && flag2)
			{
				hasErrors |= LogError($"Shader '{shaderName}' required vertex input '{value}' cannot declare @default.");
			}
			list.Add(new ShaderVertexInputSlot
			{
				location = int.Parse(item.Groups["location"].Value, CultureInfo.InvariantCulture),
				semantic = result,
				required = flag,
				defaultValue = (flag2 ? ToVertexDefaultValue(components) : GetVertexSemanticDefaultValue(result))
			});
		}
		ShaderVertexInputLayout shaderVertexInputLayout = new ShaderVertexInputLayout(list.ToArray(), list2.ToArray());
		foreach (string item2 in shaderVertexInputLayout.ConfigErrors())
		{
			hasErrors |= LogError("Shader '" + shaderName + "' has invalid vertex input layout: " + item2);
		}
		if (!hasErrors)
		{
			return shaderVertexInputLayout;
		}
		return new ShaderVertexInputLayout(new ShaderVertexInputSlot[1]
		{
			new ShaderVertexInputSlot
			{
				location = 0,
				semantic = VertexAttributeSemantic.Position,
				required = true
			}
		}, Array.Empty<ShaderInstanceInputSlot>(), hasErrors: true);
	}

	public static bool ValidateStageInterface(string vertexSource, string fragmentSource, string vertexName, string fragmentName)
	{
		if (!TryCollectVaryings(vertexSource, "out", out Dictionary<int, List<Varying>> varyings) || !TryCollectVaryings(fragmentSource, "in", out Dictionary<int, List<Varying>> varyings2))
		{
			return false;
		}
		bool flag = false;
		foreach (var (num2, list2) in varyings2)
		{
			foreach (Varying input in list2)
			{
				if (!varyings.TryGetValue(num2, out var value))
				{
					flag |= LogError($"Shader '{fragmentName}' reads '{input.Name}' ({input.Type}) at location {num2}, but '{vertexName}' writes nothing there.");
				}
				else if (!value.Exists((Varying output) => output.BaseType == input.BaseType && output.Components >= input.Components))
				{
					flag |= LogError($"Shader '{fragmentName}' reads '{input.Name}' ({input.Type}) at location {num2}, but '{vertexName}' writes '{value[0].Name}' ({value[0].Type}) there.");
				}
			}
		}
		return flag;
	}

	private static bool TryCollectVaryings(string source, string direction, out Dictionary<int, List<Varying>> varyings)
	{
		varyings = new Dictionary<int, List<Varying>>();
		source = StripLineComments(StripBlockComments(source));
		if (InterfaceBlockRegex().IsMatch(source))
		{
			return false;
		}
		foreach (Match item in VaryingRegex().Matches(source))
		{
			if (!string.Equals(item.Groups["dir"].Value, direction, StringComparison.Ordinal))
			{
				continue;
			}
			string value = item.Groups["layout"].Value;
			if (!TryGetLayoutLocation(value, out var value2))
			{
				continue;
			}
			if (value.Contains("component", StringComparison.Ordinal))
			{
				return false;
			}
			int result = 1;
			if (item.Groups["array"].Success && !int.TryParse(item.Groups["array"].Value.Trim('[', ']', ' ', '\t'), NumberStyles.None, CultureInfo.InvariantCulture, out result))
			{
				return false;
			}
			string value3 = item.Groups["type"].Value;
			if (!TryGetVaryingShape(value3, out var baseType, out var slotCount, out var components))
			{
				return false;
			}
			for (int i = 0; i < result * slotCount; i++)
			{
				if (!varyings.TryGetValue(value2 + i, out List<Varying> value4))
				{
					value4 = (varyings[value2 + i] = new List<Varying>());
				}
				value4.Add(new Varying(item.Groups["name"].Value, value3, baseType, components));
			}
		}
		return true;
	}

	private static bool TryGetVaryingShape(string type, out char baseType, out int slotCount, out int components)
	{
		slotCount = 1;
		Match match = VaryingTypeRegex().Match(type);
		if (!match.Success)
		{
			baseType = '\0';
			components = 0;
			return false;
		}
		baseType = (match.Groups["scalar"].Success ? match.Groups["scalar"].Value[0] : ((match.Groups["prefix"].Length > 0) ? match.Groups["prefix"].Value[0] : 'f'));
		if (baseType == 'd')
		{
			components = 0;
			return false;
		}
		if (match.Groups["columns"].Success)
		{
			slotCount = match.Groups["columns"].Value[0] - 48;
			components = (match.Groups["rows"].Success ? (match.Groups["rows"].Value[0] - 48) : slotCount);
			return true;
		}
		components = ((!match.Groups["size"].Success) ? 1 : (match.Groups["size"].Value[0] - 48));
		return true;
	}

	private static bool LogError(string message)
	{
		RenderLog.Log.Error(message, "LogError", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\ShaderMetadataParser.cs", 223);
		return true;
	}

	private static bool CheckSamplerCount(string shaderName, string source)
	{
		int num = 0;
		foreach (Match item in MaterialTextureRegex().Matches(source))
		{
			if (item.Groups["type"].Value.StartsWith("sampler", StringComparison.Ordinal))
			{
				num++;
			}
		}
		if (num > 16)
		{
			return LogError($"Shader '{shaderName}' declares {num} samplers, above Metal's limit of {16} per stage. Share one `sampler` across `texture*` declarations.");
		}
		return false;
	}

	private static void AddInstanceInputSlot(string shaderName, Match match, string inputName, List<ShaderInstanceInputSlot> slots, ref bool hasErrors)
	{
		string value = match.Groups["semantic"].Value;
		if (!Enum.TryParse<ShaderInstanceInputSemantic>(value, ignoreCase: false, out var result))
		{
			hasErrors |= LogError($"Shader '{shaderName}' instance input '{inputName}' has unknown instance semantic '{value}'.");
		}
		string value2 = match.Groups["presence"].Value;
		if (string.IsNullOrWhiteSpace(value2))
		{
			hasErrors |= LogError($"Shader '{shaderName}' instance input '{inputName}' must be annotated as required.");
		}
		if (string.Equals(value2, "optional", StringComparison.Ordinal))
		{
			hasErrors |= LogError($"Shader '{shaderName}' instance input '{inputName}' must use '@instance <semantic> required'.");
		}
		if (result == ShaderInstanceInputSemantic.ModelMatrix && !string.Equals(match.Groups["type"].Value, "mat4", StringComparison.Ordinal))
		{
			hasErrors |= LogError($"Shader '{shaderName}' instance input '{inputName}' with semantic {result} must be a mat4.");
		}
		slots.Add(new ShaderInstanceInputSlot
		{
			location = int.Parse(match.Groups["location"].Value, CultureInfo.InvariantCulture),
			semantic = result,
			required = string.Equals(value2, "required", StringComparison.Ordinal)
		});
	}

	private static void ExtractTextureSlots(string shaderName, string source, int materialSet, List<MaterialTextureSlot> slots, Dictionary<string, string> textureComments, ref bool hasErrors)
	{
		int num = 0;
		foreach (Match item in MaterialTextureRegex().Matches(source))
		{
			string value = item.Groups["layout"].Value;
			if (!TryGetLayoutSet(value, out var value2) || value2 != materialSet || !TryGetLayoutBinding(value, out var value3))
			{
				continue;
			}
			if (item.Groups["array"].Success)
			{
				hasErrors |= LogError($"Shader '{shaderName}' material sampler array '{item.Groups["name"].Value}' is not supported.");
			}
			string value4 = item.Groups["type"].Value;
			MaterialTextureKind materialTextureKind = ((value4 == "sampler") ? MaterialTextureKind.Sampler : (value4.StartsWith("texture", StringComparison.Ordinal) ? MaterialTextureKind.SampledImage : MaterialTextureKind.Combined));
			int resourceSlot = ((materialTextureKind == MaterialTextureKind.Sampler) ? (-1) : num++);
			string value5 = item.Groups["name"].Value;
			textureComments.TryGetValue(value5, out string value6);
			if (value6 == null)
			{
				value6 = string.Empty;
			}
			bool vertexVisible = VertexHintRegex().IsMatch(value6);
			MaterialTextureDefault textureDefault = MaterialTextureDefault.White;
			Match match2 = DefaultAnnotationRegex().Match(value6);
			if (match2.Success)
			{
				string text = match2.Groups["value"].Value.Trim().Split(' ', 2)[0];
				if (!match2.Groups["assign"].Success)
				{
					hasErrors |= LogError($"Shader '{shaderName}' material texture '{value5}' has '@default' without '=' (expected '@default = white|black|gray').");
				}
				else if (text.Length == 0 || !TryParseTextureDefault(text, out textureDefault))
				{
					hasErrors |= LogError($"Shader '{shaderName}' material texture '{value5}' has an unknown @default '{text}' (expected '@default = white|black|gray').");
				}
			}
			slots.Add(new MaterialTextureSlot(resourceSlot, value3, value5, ShaderPropertyId.GetOrCreate(value5), vertexVisible, materialTextureKind, textureDefault));
		}
	}

	public static bool TryParseTextureDefault(string source, out MaterialTextureDefault textureDefault)
	{
		MaterialTextureDefault materialTextureDefault;
		switch (source)
		{
		case "black":
			materialTextureDefault = MaterialTextureDefault.Black;
			break;
		case "gray":
		case "grey":
			materialTextureDefault = MaterialTextureDefault.Gray;
			break;
		default:
			materialTextureDefault = MaterialTextureDefault.White;
			break;
		}
		textureDefault = materialTextureDefault;
		if (((source == null || source.Length != 0) && !(source == "white")) || 1 == 0)
		{
			return textureDefault != MaterialTextureDefault.White;
		}
		return true;
	}

	private static Dictionary<string, string> CollectTextureComments(string source, int materialSet)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>(StringComparer.Ordinal);
		foreach (Match item in MaterialTextureCommentRegex().Matches(source))
		{
			if (TryGetLayoutSet(item.Groups["layout"].Value, out var value) && value == materialSet)
			{
				dictionary[item.Groups["name"].Value] = item.Groups["comment"].Value;
			}
		}
		return dictionary;
	}

	private static void ExtractStorageBufferSlots(string source, int materialSet, List<MaterialStorageBufferSlot> slots)
	{
		foreach (Match item in MaterialStorageBufferRegex().Matches(source))
		{
			string value = item.Groups["layout"].Value;
			if (TryGetLayoutSet(value, out var value2) && value2 == materialSet && TryGetLayoutBinding(value, out var value3))
			{
				string value4 = item.Groups["name"].Value;
				slots.Add(new MaterialStorageBufferSlot(value3, value4, ShaderPropertyId.GetOrCreate(value4)));
			}
		}
	}

	private static int ExtractUniformBlockSlots(string shaderName, string body, List<MaterialTextureSlot> textureSlots, List<MaterialUniformSlot> slots, ref bool hasErrors)
	{
		int num = 0;
		foreach (Match item in UniformMemberRegex().Matches(body))
		{
			string value = item.Groups["name"].Value;
			if (item.Groups["array"].Success)
			{
				hasErrors |= LogError($"Shader '{shaderName}' material uniform array '{value}' is not supported.");
			}
			string value2 = item.Groups["type"].Value;
			if (TryParseMaterialUniformType(value2, out var type))
			{
				int textureResourceSlot = 0;
				if (type == MaterialUniformType.Vector4 && TryGetTextureST(value, textureSlots, out textureResourceSlot))
				{
					type = MaterialUniformType.TextureST;
				}
				string value3 = item.Groups["comment"].Value;
				bool isColor = type == MaterialUniformType.Vector4 && HasColorAnnotation(value3);
				float[] components;
				bool flag = TryReadDefault(shaderName, "material uniform '" + value + "'", value2, value3, ref hasErrors, out components);
				MaterialUniformValue defaultValue = (flag ? ToDefaultValue(type, components) : MaterialUniformValue.Zero(type));
				num = Align(num, GetStd140Alignment(type));
				slots.Add(new MaterialUniformSlot(num, type, value, ShaderPropertyId.GetOrCreate(value), defaultValue, textureResourceSlot, isColor, flag));
				num += MaterialLayout.GetStd140Size(type);
			}
		}
		return Align(num, 16);
	}

	private static bool HasColorAnnotation(string comment)
	{
		int num = comment.IndexOf("@color", StringComparison.Ordinal);
		if (num < 0)
		{
			return false;
		}
		int num2 = num + "@color".Length;
		if (num2 < comment.Length)
		{
			if (!char.IsLetterOrDigit(comment[num2]))
			{
				return comment[num2] != '_';
			}
			return false;
		}
		return true;
	}

	private static bool TryReadDefault(string shaderName, string what, string glslType, string comment, ref bool hasErrors, out float[] components, string? alternateType = null)
	{
		components = Array.Empty<float>();
		Match match = DefaultAnnotationRegex().Match(comment);
		if (!match.Success)
		{
			return false;
		}
		if (!match.Groups["assign"].Success)
		{
			hasErrors |= LogError($"Shader '{shaderName}' {what} has '@default' without '=' (expected '@default = {DefaultExample(glslType)}').");
			return false;
		}
		string value = match.Groups["value"].Value.Trim();
		if (!TryParseDefaultLiteral(glslType, value, out components) && (alternateType == null || !TryParseDefaultLiteral(alternateType, value, out components)))
		{
			hasErrors |= LogError($"Shader '{shaderName}' {what} has a malformed @default '{value}' (expected '@default = {DefaultExample(glslType)}').");
			return false;
		}
		return true;
	}

	private static bool TryParseDefaultLiteral(string glslType, string value, out float[] components)
	{
		components = Array.Empty<float>();
		int componentCount = GetComponentCount(glslType);
		switch (componentCount)
		{
		case 0:
			return false;
		case 1:
		{
			Match match2 = LeadingNumberRegex().Match(value);
			if (!match2.Success)
			{
				return false;
			}
			components = new float[1] { ParseFloat(match2.Value) };
			return true;
		}
		default:
		{
			Match match = ConstructorLiteralRegex().Match(value);
			if (!match.Success || !string.Equals(match.Groups["type"].Value, glslType, StringComparison.Ordinal))
			{
				return false;
			}
			string[] array = match.Groups["args"].Value.Split(',');
			if (array.Length != 1 && array.Length != componentCount)
			{
				return false;
			}
			float[] array2 = new float[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!FullNumberRegex().IsMatch(text))
				{
					return false;
				}
				array2[i] = ParseFloat(text);
			}
			if (array2.Length == 1 && !glslType.StartsWith("mat", StringComparison.Ordinal))
			{
				components = new float[componentCount];
				Array.Fill(components, array2[0]);
			}
			else
			{
				components = array2;
			}
			return true;
		}
		}
	}

	private static int GetComponentCount(string glslType)
	{
		switch (glslType)
		{
		case "float":
		case "uint":
		case "int":
			return 1;
		case "ivec2":
		case "uvec2":
		case "vec2":
			return 2;
		case "ivec3":
		case "uvec3":
		case "vec3":
			return 3;
		case "ivec4":
		case "uvec4":
		case "vec4":
			return 4;
		case "mat3":
			return 9;
		case "mat4":
			return 16;
		default:
			return 0;
		}
	}

	private static string DefaultExample(string glslType)
	{
		return GetComponentCount(glslType) switch
		{
			1 => (glslType == "float") ? "1.0" : "1", 
			2 => glslType + "(x, y)", 
			3 => glslType + "(x, y, z)", 
			4 => glslType + "(x, y, z, w)", 
			_ => glslType + "(...)", 
		};
	}

	private static float ParseFloat(string value)
	{
		return float.Parse(value.TrimEnd('f', 'F', 'u', 'U'), NumberStyles.Float, CultureInfo.InvariantCulture);
	}

	private static MaterialUniformValue ToDefaultValue(MaterialUniformType type, float[] values)
	{
		return type switch
		{
			MaterialUniformType.Matrix3x3 => MaterialUniformValue.FromMatrix(type, ParseMatrix3x3(values)), 
			MaterialUniformType.Matrix4x4 => MaterialUniformValue.FromMatrix(type, ParseMatrix4x4(values)), 
			MaterialUniformType.Vector2 => MaterialUniformValue.FromVector(type, new Vector4(Get(values, 0), Get(values, 1), 0f, 0f)), 
			MaterialUniformType.Vector3 => MaterialUniformValue.FromVector(type, new Vector4(Get(values, 0), Get(values, 1), Get(values, 2), 0f)), 
			MaterialUniformType.Vector4 => MaterialUniformValue.FromVector(type, new Vector4(Get(values, 0), Get(values, 1), Get(values, 2), Get(values, 3))), 
			MaterialUniformType.TextureST => MaterialUniformValue.FromVector(type, new Vector4(Get(values, 0), Get(values, 1), Get(values, 2), Get(values, 3))), 
			MaterialUniformType.Int => MaterialUniformValue.FromInt((int)Get(values, 0)), 
			MaterialUniformType.Float => MaterialUniformValue.FromFloat(Get(values, 0)), 
			_ => MaterialUniformValue.Zero(type), 
		};
	}

	private static Vector4 ToVertexDefaultValue(float[] values)
	{
		if (values.Length == 1)
		{
			return new Vector4(values[0], values[0], values[0], values[0]);
		}
		return new Vector4(Get(values, 0), Get(values, 1), Get(values, 2), Get(values, 3));
	}

	private static Vector4 GetVertexSemanticDefaultValue(VertexAttributeSemantic semantic)
	{
		return semantic switch
		{
			VertexAttributeSemantic.Color => Vector4.One, 
			VertexAttributeSemantic.Normal => new Vector4(0f, 1f, 0f, 0f), 
			VertexAttributeSemantic.Tangent => new Vector4(1f, 0f, 0f, 1f), 
			_ => Vector4.Zero, 
		};
	}

	private static Matrix4x4 ParseMatrix3x3(float[] values)
	{
		if (values.Length == 1)
		{
			return new Matrix4x4(values[0], 0f, 0f, 0f, 0f, values[0], 0f, 0f, 0f, 0f, values[0], 0f, 0f, 0f, 0f, 0f);
		}
		return new Matrix4x4(Get(values, 0), Get(values, 1), Get(values, 2), 0f, Get(values, 3), Get(values, 4), Get(values, 5), 0f, Get(values, 6), Get(values, 7), Get(values, 8), 0f, 0f, 0f, 0f, 0f);
	}

	private static Matrix4x4 ParseMatrix4x4(float[] values)
	{
		if (values.Length == 1)
		{
			return new Matrix4x4(values[0], 0f, 0f, 0f, 0f, values[0], 0f, 0f, 0f, 0f, values[0], 0f, 0f, 0f, 0f, values[0]);
		}
		return new Matrix4x4(Get(values, 0), Get(values, 1), Get(values, 2), Get(values, 3), Get(values, 4), Get(values, 5), Get(values, 6), Get(values, 7), Get(values, 8), Get(values, 9), Get(values, 10), Get(values, 11), Get(values, 12), Get(values, 13), Get(values, 14), Get(values, 15));
	}

	private static bool TryParseMaterialUniformType(string glslType, out MaterialUniformType type)
	{
		type = glslType switch
		{
			"mat3" => MaterialUniformType.Matrix3x3, 
			"mat4" => MaterialUniformType.Matrix4x4, 
			"vec2" => MaterialUniformType.Vector2, 
			"vec3" => MaterialUniformType.Vector3, 
			"vec4" => MaterialUniformType.Vector4, 
			"int" => MaterialUniformType.Int, 
			"float" => MaterialUniformType.Float, 
			_ => MaterialUniformType.Matrix3x3, 
		};
		switch (glslType)
		{
		case "mat3":
		case "vec3":
		case "mat4":
		case "vec4":
		case "vec2":
		case "int":
		case "float":
			return true;
		default:
			return false;
		}
	}

	private static int GetStd140Alignment(MaterialUniformType type)
	{
		return type switch
		{
			MaterialUniformType.Matrix3x3 => 16, 
			MaterialUniformType.Matrix4x4 => 16, 
			MaterialUniformType.Vector2 => 8, 
			MaterialUniformType.Vector3 => 16, 
			MaterialUniformType.Vector4 => 16, 
			MaterialUniformType.TextureST => 16, 
			MaterialUniformType.Int => 4, 
			MaterialUniformType.Float => 4, 
			_ => throw new ArgumentOutOfRangeException("type"), 
		};
	}

	private static bool TryGetLayoutSet(string layout, out int value)
	{
		return int.TryParse(LayoutSetRegex().Match(layout).Groups[1].Value, out value);
	}

	private static bool TryGetLayoutBinding(string layout, out int value)
	{
		return int.TryParse(LayoutBindingRegex().Match(layout).Groups[1].Value, out value);
	}

	private static bool TryGetLayoutLocation(string layout, out int value)
	{
		return int.TryParse(LayoutLocationRegex().Match(layout).Groups[1].Value, out value);
	}

	private static bool TryGetTextureST(string name, List<MaterialTextureSlot> textureSlots, out int textureResourceSlot)
	{
		if (name.EndsWith("_ST", StringComparison.Ordinal))
		{
			string b = name.Substring(0, name.Length - 3);
			foreach (MaterialTextureSlot textureSlot in textureSlots)
			{
				if (string.Equals(textureSlot.PropertyName, b, StringComparison.Ordinal))
				{
					textureResourceSlot = textureSlot.ResourceSlot;
					return true;
				}
			}
		}
		textureResourceSlot = 0;
		return false;
	}

	private static int Align(int value, int alignment)
	{
		int num = alignment - 1;
		return (value + num) & ~num;
	}

	private static float Get(float[] values, int index)
	{
		if (index >= values.Length)
		{
			return 0f;
		}
		return values[index];
	}

	private static string StripBlockComments(string source)
	{
		return StripBlockRegex().Replace(source ?? string.Empty, string.Empty);
	}

	private static string StripLineComments(string source)
	{
		return StripLineRegex().Replace(source, string.Empty);
	}

	[GeneratedRegex("layout\\s*\\((?<layout>[^)]*)\\)\\s*uniform\\s+\\w+\\s*\\{(?<body>.*?)\\}\\s*\\w*\\s*;", RegexOptions.Singleline | RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex UniformBlockRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformBlockRegex_2.Instance;
	}

	[GeneratedRegex("(?:layout\\s*\\([^)]*\\)\\s*)?(?<type>mat3|mat4|vec2|vec3|vec4|int|float)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?<array>\\[[^\\]]+\\])?\\s*;\\s*(?://\\s*(?<comment>[^\\r\\n]+))?", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex UniformMemberRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__UniformMemberRegex_3.Instance;
	}

	[GeneratedRegex("layout\\s*\\((?<layout>[^)]*)\\)\\s*uniform\\s+(?<type>sampler[A-Za-z0-9_]*|texture[A-Za-z0-9_]+)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?<array>\\[[^\\]]+\\])?\\s*;", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex MaterialTextureRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialTextureRegex_4.Instance;
	}

	[GeneratedRegex("layout\\s*\\((?<layout>[^)]*)\\)\\s*uniform\\s+(?:sampler[A-Za-z0-9_]*|texture[A-Za-z0-9_]+)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?:\\[[^\\]]+\\])?\\s*;[^\\S\\r\\n]*//(?<comment>[^\\r\\n]*)", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex MaterialTextureCommentRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialTextureCommentRegex_5.Instance;
	}

	[GeneratedRegex("@vertex\\b", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex VertexHintRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VertexHintRegex_6.Instance;
	}

	[GeneratedRegex("layout\\s*\\((?<layout>[^)]*)\\)\\s*(?:(?:readonly|writeonly|restrict|coherent|volatile)\\s+)*buffer\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*\\{", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex MaterialStorageBufferRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__MaterialStorageBufferRegex_7.Instance;
	}

	[GeneratedRegex("layout\\s*\\(\\s*location\\s*=\\s*(?<location>\\d+)\\s*\\)\\s*in\\s+(?<type>[A-Za-z0-9_]+)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*;\\s*(?://\\s*@(?<kind>vertex|instance)(?:\\s+(?<semantic>[A-Za-z_][A-Za-z0-9_]*))?(?:\\s+(?<presence>required|optional))?(?<tail>[^\\r\\n]*))?", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex ShaderVertexInputRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__ShaderVertexInputRegex_8.Instance;
	}

	[GeneratedRegex("(?:layout\\s*\\((?<layout>[^)]*)\\)\\s*)(?:(?:flat|smooth|noperspective|centroid|sample|invariant|highp|mediump|lowp)\\s+)*(?<dir>in|out)\\s+(?<type>[A-Za-z_][A-Za-z0-9_]*)\\s+(?<name>[A-Za-z_][A-Za-z0-9_]*)\\s*(?<array>\\[[^\\]]*\\])?\\s*;", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex VaryingRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingRegex_9.Instance;
	}

	[GeneratedRegex("\\b(?:in|out)\\s+[A-Za-z_][A-Za-z0-9_]*\\s*\\{", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex InterfaceBlockRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__InterfaceBlockRegex_10.Instance;
	}

	[GeneratedRegex("^(?:(?<scalar>float|int|uint|bool|double)|(?<prefix>[iubd]?)vec(?<size>[234])|(?<prefix>d?)mat(?<columns>[234])(?:x(?<rows>[234]))?)$", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex VaryingTypeRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__VaryingTypeRegex_11.Instance;
	}

	[GeneratedRegex("@default\\b(?<assign>\\s*=)?(?<value>[^\\r\\n]*)", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex DefaultAnnotationRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__DefaultAnnotationRegex_12.Instance;
	}

	[GeneratedRegex("^\\s*(?<type>[a-z]+[234])\\s*\\((?<args>[^)]*)\\)", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex ConstructorLiteralRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__ConstructorLiteralRegex_13.Instance;
	}

	[GeneratedRegex("^[-+]?(?:\\d+(?:\\.\\d*)?|\\.\\d+)(?:[eE][-+]?\\d+)?[fFuU]?(?![A-Za-z0-9_.(])", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex LeadingNumberRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LeadingNumberRegex_14.Instance;
	}

	[GeneratedRegex("^[-+]?(?:\\d+(?:\\.\\d*)?|\\.\\d+)(?:[eE][-+]?\\d+)?[fFuU]?$", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex FullNumberRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__FullNumberRegex_15.Instance;
	}

	[GeneratedRegex("//.*?$", RegexOptions.Multiline | RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex StripLineRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__StripLineRegex_16.Instance;
	}

	[GeneratedRegex("/\\*.*?\\*/", RegexOptions.Singleline | RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex StripBlockRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__StripBlockRegex_17.Instance;
	}

	[GeneratedRegex("(?:^|,)\\s*set\\s*=\\s*(\\d+)", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex LayoutSetRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutSetRegex_18.Instance;
	}

	[GeneratedRegex("(?:^|,)\\s*binding\\s*=\\s*(\\d+)", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex LayoutBindingRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutBindingRegex_19.Instance;
	}

	[GeneratedRegex("(?:^|,)\\s*location\\s*=\\s*(\\d+)", RegexOptions.CultureInvariant)]
	[GeneratedCode("System.Text.RegularExpressions.Generator", "42.42.42.42")]
	private static Regex LayoutLocationRegex()
	{
		return _003CRegexGenerator_g_003EF51D127433C9DBA269DDD2A271109530A80B697E850A7C9EDE6E9DA3A382CE7BB__LayoutLocationRegex_20.Instance;
	}
}
