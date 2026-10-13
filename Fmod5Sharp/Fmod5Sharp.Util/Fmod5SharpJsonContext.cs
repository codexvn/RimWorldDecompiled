using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Fmod5Sharp.Util;

[JsonSerializable(typeof(Dictionary<uint, FmodVorbisData>))]
[GeneratedCode("System.Text.Json.SourceGeneration", "10.0.14.7603")]
internal class Fmod5SharpJsonContext : JsonSerializerContext, IJsonTypeInfoResolver
{
	private JsonTypeInfo<byte[]>? _ByteArray;

	private JsonTypeInfo<FmodVorbisData>? _FmodVorbisData;

	private JsonTypeInfo<Dictionary<uint, FmodVorbisData>>? _DictionaryUInt32FmodVorbisData;

	private JsonTypeInfo<int>? _Int32;

	private JsonTypeInfo<uint>? _UInt32;

	private static readonly JsonSerializerOptions s_defaultOptions = new JsonSerializerOptions();

	private const BindingFlags InstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

	private static readonly JsonEncodedText PropName_headerBytes = JsonEncodedText.Encode("headerBytes");

	private static readonly JsonEncodedText PropName_seekBit = JsonEncodedText.Encode("seekBit");

	public JsonTypeInfo<byte[]> ByteArray => _ByteArray ?? (_ByteArray = (JsonTypeInfo<byte[]>)base.Options.GetTypeInfo(typeof(byte[])));

	public JsonTypeInfo<FmodVorbisData> FmodVorbisData => _FmodVorbisData ?? (_FmodVorbisData = (JsonTypeInfo<FmodVorbisData>)base.Options.GetTypeInfo(typeof(FmodVorbisData)));

	public JsonTypeInfo<Dictionary<uint, FmodVorbisData>> DictionaryUInt32FmodVorbisData => _DictionaryUInt32FmodVorbisData ?? (_DictionaryUInt32FmodVorbisData = (JsonTypeInfo<Dictionary<uint, FmodVorbisData>>)base.Options.GetTypeInfo(typeof(Dictionary<uint, FmodVorbisData>)));

	public JsonTypeInfo<int> Int32 => _Int32 ?? (_Int32 = (JsonTypeInfo<int>)base.Options.GetTypeInfo(typeof(int)));

	public JsonTypeInfo<uint> UInt32 => _UInt32 ?? (_UInt32 = (JsonTypeInfo<uint>)base.Options.GetTypeInfo(typeof(uint)));

	public static Fmod5SharpJsonContext Default { get; } = new Fmod5SharpJsonContext(new JsonSerializerOptions(s_defaultOptions));

	protected override JsonSerializerOptions? GeneratedSerializerOptions { get; } = s_defaultOptions;

	private JsonTypeInfo<byte[]> Create_ByteArray(JsonSerializerOptions options)
	{
		if (!TryGetTypeInfoForRuntimeCustomConverter(options, out JsonTypeInfo<byte[]> jsonTypeInfo))
		{
			jsonTypeInfo = JsonMetadataServices.CreateValueInfo<byte[]>(options, JsonMetadataServices.ByteArrayConverter);
		}
		jsonTypeInfo.OriginatingResolver = this;
		return jsonTypeInfo;
	}

	private JsonTypeInfo<FmodVorbisData> Create_FmodVorbisData(JsonSerializerOptions options)
	{
		if (!TryGetTypeInfoForRuntimeCustomConverter(options, out JsonTypeInfo<FmodVorbisData> jsonTypeInfo))
		{
			JsonObjectInfoValues<FmodVorbisData> objectInfo = new JsonObjectInfoValues<FmodVorbisData>
			{
				ObjectCreator = null,
				ObjectWithParameterizedConstructorCreator = (object[] args) => new FmodVorbisData((byte[])args[0], (int)args[1]),
				PropertyMetadataInitializer = (JsonSerializerContext _) => FmodVorbisDataPropInit(options),
				ConstructorParameterMetadataInitializer = FmodVorbisDataCtorParamInit,
				ConstructorAttributeProviderFactory = () => typeof(FmodVorbisData).GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, new Type[2]
				{
					typeof(byte[]),
					typeof(int)
				}, null),
				SerializeHandler = FmodVorbisDataSerializeHandler
			};
			jsonTypeInfo = JsonMetadataServices.CreateObjectInfo(options, objectInfo);
			jsonTypeInfo.NumberHandling = null;
		}
		jsonTypeInfo.OriginatingResolver = this;
		return jsonTypeInfo;
	}

	private static JsonPropertyInfo[] FmodVorbisDataPropInit(JsonSerializerOptions options)
	{
		JsonPropertyInfo[] array = new JsonPropertyInfo[2];
		JsonPropertyInfoValues<byte[]> propertyInfo = new JsonPropertyInfoValues<byte[]>
		{
			IsProperty = true,
			IsPublic = true,
			IsVirtual = false,
			DeclaringType = typeof(FmodVorbisData),
			Converter = null,
			Getter = (object obj) => ((FmodVorbisData)obj).HeaderBytes,
			Setter = delegate(object obj, byte[]? value)
			{
				((FmodVorbisData)obj).HeaderBytes = value;
			},
			IgnoreCondition = null,
			HasJsonInclude = false,
			IsExtensionData = false,
			NumberHandling = null,
			PropertyName = "HeaderBytes",
			JsonPropertyName = "headerBytes",
			AttributeProviderFactory = () => typeof(FmodVorbisData).GetProperty("HeaderBytes", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(byte[]), Array.Empty<Type>(), null)
		};
		array[0] = JsonMetadataServices.CreatePropertyInfo(options, propertyInfo);
		array[0].IsGetNullable = false;
		array[0].IsSetNullable = false;
		JsonPropertyInfoValues<int> propertyInfo2 = new JsonPropertyInfoValues<int>
		{
			IsProperty = true,
			IsPublic = true,
			IsVirtual = false,
			DeclaringType = typeof(FmodVorbisData),
			Converter = null,
			Getter = (object obj) => ((FmodVorbisData)obj).SeekBit,
			Setter = delegate(object obj, int value)
			{
				((FmodVorbisData)obj).SeekBit = value;
			},
			IgnoreCondition = null,
			HasJsonInclude = false,
			IsExtensionData = false,
			NumberHandling = null,
			PropertyName = "SeekBit",
			JsonPropertyName = "seekBit",
			AttributeProviderFactory = () => typeof(FmodVorbisData).GetProperty("SeekBit", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, typeof(int), Array.Empty<Type>(), null)
		};
		array[1] = JsonMetadataServices.CreatePropertyInfo(options, propertyInfo2);
		return array;
	}

	private void FmodVorbisDataSerializeHandler(Utf8JsonWriter writer, FmodVorbisData? value)
	{
		if (value == null)
		{
			writer.WriteNullValue();
			return;
		}
		writer.WriteStartObject();
		writer.WriteBase64String(PropName_headerBytes, value.HeaderBytes);
		writer.WriteNumber(PropName_seekBit, value.SeekBit);
		writer.WriteEndObject();
	}

	private static JsonParameterInfoValues[] FmodVorbisDataCtorParamInit()
	{
		return new JsonParameterInfoValues[2]
		{
			new JsonParameterInfoValues
			{
				Name = "headerBytes",
				ParameterType = typeof(byte[]),
				Position = 0,
				HasDefaultValue = false,
				DefaultValue = null,
				IsNullable = false
			},
			new JsonParameterInfoValues
			{
				Name = "seekBit",
				ParameterType = typeof(int),
				Position = 1,
				HasDefaultValue = false,
				DefaultValue = null,
				IsNullable = false
			}
		};
	}

	private JsonTypeInfo<Dictionary<uint, FmodVorbisData>> Create_DictionaryUInt32FmodVorbisData(JsonSerializerOptions options)
	{
		if (!TryGetTypeInfoForRuntimeCustomConverter(options, out JsonTypeInfo<Dictionary<uint, FmodVorbisData>> jsonTypeInfo))
		{
			JsonCollectionInfoValues<Dictionary<uint, FmodVorbisData>> collectionInfo = new JsonCollectionInfoValues<Dictionary<uint, FmodVorbisData>>
			{
				ObjectCreator = () => new Dictionary<uint, FmodVorbisData>(),
				SerializeHandler = null
			};
			jsonTypeInfo = JsonMetadataServices.CreateDictionaryInfo<Dictionary<uint, FmodVorbisData>, uint, FmodVorbisData>(options, collectionInfo);
			jsonTypeInfo.NumberHandling = null;
		}
		jsonTypeInfo.OriginatingResolver = this;
		return jsonTypeInfo;
	}

	private JsonTypeInfo<int> Create_Int32(JsonSerializerOptions options)
	{
		if (!TryGetTypeInfoForRuntimeCustomConverter(options, out JsonTypeInfo<int> jsonTypeInfo))
		{
			jsonTypeInfo = JsonMetadataServices.CreateValueInfo<int>(options, JsonMetadataServices.Int32Converter);
		}
		jsonTypeInfo.OriginatingResolver = this;
		return jsonTypeInfo;
	}

	private JsonTypeInfo<uint> Create_UInt32(JsonSerializerOptions options)
	{
		if (!TryGetTypeInfoForRuntimeCustomConverter(options, out JsonTypeInfo<uint> jsonTypeInfo))
		{
			jsonTypeInfo = JsonMetadataServices.CreateValueInfo<uint>(options, JsonMetadataServices.UInt32Converter);
		}
		jsonTypeInfo.OriginatingResolver = this;
		return jsonTypeInfo;
	}

	public Fmod5SharpJsonContext()
		: base(null)
	{
	}

	public Fmod5SharpJsonContext(JsonSerializerOptions options)
		: base(options)
	{
	}

	private static bool TryGetTypeInfoForRuntimeCustomConverter<TJsonMetadataType>(JsonSerializerOptions options, out JsonTypeInfo<TJsonMetadataType> jsonTypeInfo)
	{
		JsonConverter runtimeConverterForType = GetRuntimeConverterForType(typeof(TJsonMetadataType), options);
		if (runtimeConverterForType != null)
		{
			jsonTypeInfo = JsonMetadataServices.CreateValueInfo<TJsonMetadataType>(options, runtimeConverterForType);
			return true;
		}
		jsonTypeInfo = null;
		return false;
	}

	private static JsonConverter? GetRuntimeConverterForType(Type type, JsonSerializerOptions options)
	{
		for (int i = 0; i < options.Converters.Count; i++)
		{
			JsonConverter jsonConverter = options.Converters[i];
			if (jsonConverter != null && jsonConverter.CanConvert(type))
			{
				return ExpandConverter(type, jsonConverter, options, validateCanConvert: false);
			}
		}
		return null;
	}

	private static JsonConverter ExpandConverter(Type type, JsonConverter converter, JsonSerializerOptions options, bool validateCanConvert = true)
	{
		if (validateCanConvert && !converter.CanConvert(type))
		{
			throw new InvalidOperationException($"The converter '{converter.GetType()}' is not compatible with the type '{type}'.");
		}
		if (converter is JsonConverterFactory jsonConverterFactory)
		{
			converter = jsonConverterFactory.CreateConverter(type, options);
			if (converter == null || converter is JsonConverterFactory)
			{
				throw new InvalidOperationException($"The converter '{jsonConverterFactory.GetType()}' cannot return null or a JsonConverterFactory instance.");
			}
		}
		return converter;
	}

	public override JsonTypeInfo? GetTypeInfo(Type type)
	{
		base.Options.TryGetTypeInfo(type, out JsonTypeInfo typeInfo);
		return typeInfo;
	}

	JsonTypeInfo? IJsonTypeInfoResolver.GetTypeInfo(Type type, JsonSerializerOptions options)
	{
		if (type == typeof(byte[]))
		{
			return Create_ByteArray(options);
		}
		if (type == typeof(FmodVorbisData))
		{
			return Create_FmodVorbisData(options);
		}
		if (type == typeof(Dictionary<uint, FmodVorbisData>))
		{
			return Create_DictionaryUInt32FmodVorbisData(options);
		}
		if (type == typeof(int))
		{
			return Create_Int32(options);
		}
		if (type == typeof(uint))
		{
			return Create_UInt32(options);
		}
		return null;
	}
}
