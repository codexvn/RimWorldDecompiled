using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Waylith.Rendering;

namespace Verse;

public static class ShaderDefRegistry
{
	public static void Clear()
	{
		ShaderRegistry.Clear();
	}

	public static void RegisterAll()
	{
		RegisterShaderDirectories();
		foreach (ShaderDef allDef in DefDatabase<ShaderDef>.AllDefs)
		{
			Register(allDef);
		}
		UnityEngine.Shader.MarkEngineRegistryReady();
	}

	public static void HotReload()
	{
		ShaderHotReloader current = ShaderHotReloader.Current;
		if (current == null)
		{
			return;
		}
		RegisterShaderDirectories();
		foreach (ShaderDef allDef in DefDatabase<ShaderDef>.AllDefs)
		{
			if (allDef.shaderProgram != null)
			{
				if (ShaderRegistry.TryResolve(allDef.defName, out Waylith.Rendering.Shader shader) && shader.Key == allDef.defName && shader.CanReloadFromSource)
				{
					current.Redefine(shader, allDef.shaderProgram, allDef.shaderKeywords, allDef.shaderProgram.ToPipelineState(), allDef.iconVersion?.defName);
				}
				else
				{
					Register(allDef);
				}
			}
		}
		current.RequestReload();
	}

	public static void RegisterShaderDirectories()
	{
		ShaderHotReloader current = ShaderHotReloader.Current;
		List<string> list = new List<string>();
		List<ModContentPack> runningModsListForReading = LoadedModManager.RunningModsListForReading;
		for (int num = runningModsListForReading.Count - 1; num >= 0; num--)
		{
			ModContentPack modContentPack = runningModsListForReading[num];
			int num2;
			if (current != null && current.WatchContentShaders)
			{
				ModMetaData modMetaData = modContentPack.ModMetaData;
				num2 = ((modMetaData != null && modMetaData.Source == ContentSource.ModsFolder) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool flag = (byte)num2 != 0;
			foreach (string item in modContentPack.foldersToLoadDescendingOrder)
			{
				string text = Path.Combine(item, "Shaders");
				if (Directory.Exists(text))
				{
					list.Add(text);
					if (flag)
					{
						current.Watch(text);
					}
				}
			}
		}
		ShaderSourceLoader.SetOverrideShaderDirectories(list);
	}

	private static void Register(ShaderDef def)
	{
		if (def?.shaderProgram != null)
		{
			ShaderProgramDefinition shaderProgram = def.shaderProgram;
			ShaderRegistry.Register(new Waylith.Rendering.Shader(def.defName, def.defName, shaderProgram, def.shaderKeywords, null, shaderProgram.ToPipelineState(), def.iconVersion?.defName));
		}
	}
}
