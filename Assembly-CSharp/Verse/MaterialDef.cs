using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse;

public class MaterialDef : Def
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__13 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public MaterialDef _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CConfigErrors_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -6:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			case -7:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally5();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				MaterialDef materialDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ((Def)materialDef).ConfigErrors().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_007e;
				case 1:
					_003C_003E1__state = -3;
					goto IL_007e;
				case 2:
					_003C_003E1__state = -4;
					goto IL_00e9;
				case 3:
					_003C_003E1__state = -5;
					goto IL_0154;
				case 4:
					_003C_003E1__state = -6;
					goto IL_01bf;
				case 5:
					{
						_003C_003E1__state = -7;
						break;
					}
					IL_007e:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = materialDef.ValidateNamedProperties(materialDef.textures, "textures").GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_00e9;
					IL_00e9:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = materialDef.ValidateNamedProperties(materialDef.floats, "floats").GetEnumerator();
					_003C_003E1__state = -5;
					goto IL_0154;
					IL_01bf:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current3 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = materialDef.ValidateNamedProperties(materialDef.vectors, "vectors").GetEnumerator();
					_003C_003E1__state = -7;
					break;
					IL_0154:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current4 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = materialDef.ValidateNamedProperties(materialDef.colors, "colors").GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_01bf;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					string current5 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current5;
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003Em__Finally5();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CConfigErrors_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__13(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CValidateNamedProperties_003Ed__20<T> : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable where T : MaterialNamedProperty
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private List<T> properties;

		public List<T> _003C_003E3__properties;

		private string listName;

		public string _003C_003E3__listName;

		private int _003Ci_003E5__2;

		string IEnumerator<string>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CValidateNamedProperties_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (properties.NullOrEmpty())
				{
					return false;
				}
				_003Ci_003E5__2 = 0;
				goto IL_0156;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0146;
			case 2:
				{
					_003C_003E1__state = -1;
					goto IL_0146;
				}
				IL_0146:
				_003Ci_003E5__2++;
				goto IL_0156;
				IL_0156:
				if (_003Ci_003E5__2 < properties.Count)
				{
					if (properties[_003Ci_003E5__2] == null)
					{
						_003C_003E2__current = $"{listName}[{_003Ci_003E5__2}] is null.";
						_003C_003E1__state = 1;
						return true;
					}
					if (properties[_003Ci_003E5__2].name.NullOrEmpty())
					{
						_003C_003E2__current = $"{listName}[{_003Ci_003E5__2}] must set {"name"}.";
						_003C_003E1__state = 2;
						return true;
					}
					goto IL_0146;
				}
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CValidateNamedProperties_003Ed__20<T> _003CValidateNamedProperties_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CValidateNamedProperties_003Ed__ = this;
			}
			else
			{
				_003CValidateNamedProperties_003Ed__ = new _003CValidateNamedProperties_003Ed__20<T>(0);
			}
			_003CValidateNamedProperties_003Ed__.properties = _003C_003E3__properties;
			_003CValidateNamedProperties_003Ed__.listName = _003C_003E3__listName;
			return _003CValidateNamedProperties_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[NoTranslate]
	public string resourcePath;

	public ShaderDef shader;

	public int renderQueue = -1;

	public List<MaterialTextureProperty> textures;

	public List<MaterialFloatProperty> floats;

	public List<MaterialColorProperty> colors;

	public List<MaterialVectorProperty> vectors;

	[NoTranslate]
	public List<string> keywords;

	[Unsaved(false)]
	private Material materialInt;

	public Material Material => materialInt ?? (materialInt = CreateMaterial());

	public Material CreateMaterialInstance(int renderQueueOverride = -1)
	{
		Material material = MaterialAllocator.Create(Material);
		material.name = defName;
		if (renderQueueOverride != -1)
		{
			material.renderQueue = renderQueueOverride;
		}
		return material;
	}

	public Material CreateMaterial()
	{
		Material material = MaterialAllocator.Create(ResolveShader());
		material.name = defName;
		if (renderQueue != -1)
		{
			material.renderQueue = renderQueue;
		}
		ApplyTextures(material);
		ApplyFloats(material);
		ApplyColors(material);
		ApplyVectors(material);
		ApplyKeywords(material);
		return material;
	}

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__13))]
	public override IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Shader ResolveShader()
	{
		if (shader != null)
		{
			return shader.Shader;
		}
		return ShaderDatabase.DefaultShader;
	}

	private void ApplyTextures(Material material)
	{
		if (textures.NullOrEmpty())
		{
			return;
		}
		for (int i = 0; i < textures.Count; i++)
		{
			MaterialTextureProperty materialTextureProperty = textures[i];
			if (materialTextureProperty != null && !materialTextureProperty.name.NullOrEmpty() && !materialTextureProperty.path.NullOrEmpty())
			{
				int nameID = Shader.PropertyToID(materialTextureProperty.name);
				Texture2D texture2D = ContentFinder<Texture2D>.Get(materialTextureProperty.path, reportFailure: false);
				if (texture2D != null && materialTextureProperty.alphaFromGrayscale)
				{
					texture2D = MaterialDefUtility.AlphaFromGrayscale(materialTextureProperty.path, texture2D);
				}
				if (texture2D != null)
				{
					material.SetTexture(nameID, texture2D);
				}
				else
				{
					Log.Error($"{"MaterialDef"} '{defName}': could not load texture '{materialTextureProperty.path}' for property '{materialTextureProperty.name}'.");
				}
				material.SetTextureOffset(nameID, materialTextureProperty.offset);
				material.SetTextureScale(nameID, (materialTextureProperty.scale == default(Vector2)) ? Vector2.one : materialTextureProperty.scale);
			}
		}
	}

	private void ApplyFloats(Material material)
	{
		if (floats.NullOrEmpty())
		{
			return;
		}
		for (int i = 0; i < floats.Count; i++)
		{
			MaterialFloatProperty materialFloatProperty = floats[i];
			if (materialFloatProperty != null && !materialFloatProperty.name.NullOrEmpty())
			{
				material.SetFloat(materialFloatProperty.name, materialFloatProperty.value);
			}
		}
	}

	private void ApplyColors(Material material)
	{
		if (colors.NullOrEmpty())
		{
			return;
		}
		for (int i = 0; i < colors.Count; i++)
		{
			MaterialColorProperty materialColorProperty = colors[i];
			if (materialColorProperty != null && !materialColorProperty.name.NullOrEmpty())
			{
				material.SetColor(materialColorProperty.name, materialColorProperty.value);
			}
		}
	}

	private void ApplyVectors(Material material)
	{
		if (vectors.NullOrEmpty())
		{
			return;
		}
		for (int i = 0; i < vectors.Count; i++)
		{
			MaterialVectorProperty materialVectorProperty = vectors[i];
			if (materialVectorProperty != null && !materialVectorProperty.name.NullOrEmpty())
			{
				material.SetVector(materialVectorProperty.name, materialVectorProperty.value);
			}
		}
	}

	private void ApplyKeywords(Material material)
	{
		if (!keywords.NullOrEmpty())
		{
			for (int i = 0; i < keywords.Count; i++)
			{
				material.EnableKeyword(keywords[i]);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CValidateNamedProperties_003Ed__20<>))]
	private IEnumerable<string> ValidateNamedProperties<T>(List<T> properties, string listName) where T : MaterialNamedProperty
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CValidateNamedProperties_003Ed__20<T>(-2)
		{
			_003C_003E3__properties = properties,
			_003C_003E3__listName = listName
		};
	}
}
