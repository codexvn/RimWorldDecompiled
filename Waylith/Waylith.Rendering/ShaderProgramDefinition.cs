using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Waylith.Rendering;

public sealed class ShaderProgramDefinition
{
	[CompilerGenerated]
	private sealed class _003CConfigErrors_003Ed__26 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ShaderProgramDefinition _003C_003E4__this;

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
		public _003CConfigErrors_003Ed__26(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			ShaderProgramDefinition shaderProgramDefinition = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (string.IsNullOrWhiteSpace(shaderProgramDefinition.vertexShaderFileName))
				{
					_003C_003E2__current = "vertexShaderFileName must be set.";
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0055;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0055;
			case 2:
				_003C_003E1__state = -1;
				goto IL_007d;
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_007d:
				if (shaderProgramDefinition.sceneGrab != null && string.IsNullOrWhiteSpace(shaderProgramDefinition.sceneGrab.name))
				{
					_003C_003E2__current = "sceneGrab must have a name.";
					_003C_003E1__state = 3;
					return true;
				}
				break;
				IL_0055:
				if (string.IsNullOrWhiteSpace(shaderProgramDefinition.fragmentShaderFileName))
				{
					_003C_003E2__current = "fragmentShaderFileName must be set.";
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_007d;
			}
			return false;
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
			_003CConfigErrors_003Ed__26 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CConfigErrors_003Ed__26(0)
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

	public string vertexShaderFileName = "SceneDefault.vert.glsl";

	public string fragmentShaderFileName = "SceneDefault.frag.glsl";

	public bool useTexture = true;

	public bool flipTextureST = true;

	public bool transparent;

	public bool depthTest = true;

	public bool depthWrite = true;

	public FaceCull cullMode = FaceCull.Back;

	public bool stencilEnabled;

	public StencilCompare stencilCompare = StencilCompare.Always;

	public StencilOp stencilPassOp;

	public StencilOp stencilFailOp;

	public StencilOp stencilDepthFailOp;

	public int stencilRef;

	public int stencilReadMask = 255;

	public int stencilWriteMask = 255;

	public bool colorWrite = true;

	public float depthBiasConstantFactor;

	public float depthBiasSlopeFactor;

	public ShaderBlendMode blendMode;

	public BlendFactor blendSrcFactor = BlendFactor.One;

	public BlendFactor blendDstFactor;

	public RenderQueueValue renderQueue = new RenderQueueValue(-1);

	public Dictionary<string, string>? passShaderOverrides;

	public Dictionary<string, TextureWrapMode>? textureWrapOverrides;

	public SceneGrabDeclaration? sceneGrab;

	[IteratorStateMachine(typeof(_003CConfigErrors_003Ed__26))]
	public IEnumerable<string> ConfigErrors()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CConfigErrors_003Ed__26(-2)
		{
			_003C_003E4__this = this
		};
	}

	public BlendState ResolveBlendState()
	{
		return blendMode switch
		{
			ShaderBlendMode.Opaque => BlendState.Opaque, 
			ShaderBlendMode.Alpha => BlendState.Alpha, 
			ShaderBlendMode.Additive => BlendState.Additive, 
			ShaderBlendMode.MultiplyDestination => BlendState.MultiplyDestination, 
			ShaderBlendMode.Custom => new BlendState(Enabled: true, blendSrcFactor, blendDstFactor, BlendOp.Add, blendSrcFactor, blendDstFactor, BlendOp.Add), 
			_ => transparent ? BlendState.Alpha : BlendState.Opaque, 
		};
	}

	public StencilState ResolveStencil()
	{
		if (!stencilEnabled)
		{
			return StencilState.Disabled;
		}
		return new StencilState(Enabled: true, stencilCompare, stencilPassOp, stencilFailOp, stencilDepthFailOp, (byte)stencilRef, (byte)stencilReadMask, (byte)stencilWriteMask);
	}

	public PipelineState ToPipelineState()
	{
		return new PipelineState(ResolveBlendState(), new DepthState(depthTest, depthWrite), cullMode, ResolveStencil(), colorWrite, new DepthBiasState(depthBiasConstantFactor, depthBiasSlopeFactor));
	}
}
