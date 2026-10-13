using System;
using System.Collections.Generic;
using System.Threading;
using Brutal.Logging;
using Brutal.Logging.Internal;

namespace Waylith.Rendering;

public static class SceneGrabRegistry
{
	public const string SceneColorName = "SceneColor";

	private static readonly Dictionary<string, SceneGrabPoint> PointsByName = new Dictionary<string, SceneGrabPoint>(StringComparer.Ordinal);

	private static readonly Lock Sync = new Lock();

	public static void DefineFrom(ShaderProgramDefinition program)
	{
		SceneGrabDeclaration sceneGrab = program.sceneGrab;
		string text = sceneGrab?.name;
		if (text != null && text.Length > 0 && (int)sceneGrab.queue >= 0)
		{
			Define(sceneGrab.name, sceneGrab.queue);
		}
	}

	public static SceneGrabPoint Define(string name, int queue)
	{
		using (Sync.EnterScope())
		{
			if (PointsByName.TryGetValue(name, out SceneGrabPoint value))
			{
				if (value.Queue != queue)
				{
					LogCategory log = RenderLog.Log;
					bool shouldAppend;
					LogString<Warning> message = new LogString<Warning>(63, 3, log, out shouldAppend);
					if (shouldAppend)
					{
						message.AppendLiteral("Scene grab '");
						message.AppendFormatted(name);
						message.AppendLiteral("' redefined at queue ");
						message.AppendFormatted(queue);
						message.AppendLiteral("; keeping the original queue ");
						message.AppendFormatted(value.Queue);
						message.AppendLiteral(".");
					}
					log.Warning(message, "Define", "D:\\Workspaces\\rimworld-builds\\RimWorldWaylith\\Engine\\Waylith\\Rendering\\SceneGrab.cs", 62);
				}
				return value;
			}
			SceneGrabPoint sceneGrabPoint = new SceneGrabPoint(name, queue);
			PointsByName[name] = sceneGrabPoint;
			GlobalShaderState.SetTexture(sceneGrabPoint.TexturePropertyId, sceneGrabPoint.Texture);
			return sceneGrabPoint;
		}
	}

	internal static void RedefineFrom(ShaderProgramDefinition program)
	{
		SceneGrabDeclaration sceneGrab = program.sceneGrab;
		string text = sceneGrab?.name;
		if (text == null || text.Length <= 0 || (int)sceneGrab.queue < 0)
		{
			return;
		}
		using (Sync.EnterScope())
		{
			if (PointsByName.TryGetValue(sceneGrab.name, out SceneGrabPoint value))
			{
				value.Queue = sceneGrab.queue;
				return;
			}
		}
		Define(sceneGrab.name, sceneGrab.queue);
	}

	public static bool TryResolve(string name, out SceneGrabPoint point)
	{
		using (Sync.EnterScope())
		{
			return PointsByName.TryGetValue(name, out point);
		}
	}
}
