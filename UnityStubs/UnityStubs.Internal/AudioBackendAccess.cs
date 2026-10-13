using System;
using Waylith.Audio;

namespace UnityStubs.Internal;

internal static class AudioBackendAccess
{
	public static AudioBackend Backend => AudioBackend.Current ?? throw new InvalidOperationException("No audio backend is active");

	public static bool Ready => AudioBackend.Current?.IsInitialized ?? false;
}
