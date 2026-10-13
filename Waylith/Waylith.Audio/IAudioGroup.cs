namespace Waylith.Audio;

public interface IAudioGroup
{
	float Volume { set; }

	bool Muted { set; }

	bool Paused { set; }

	float Pitch { set; }
}
