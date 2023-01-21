using System;
using System.Collections.Generic;

using FMODUnity;

using UnityEngine;

namespace Neutronium.Audio
{
	public class AudioManager
	{
		private static AudioManager _instance;

		/// <summary>
		/// Returns the AudioManager singleton instance.
		/// </summary>
		public static AudioManager Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AudioManager();
				}
				return _instance;
			}
		}

		private readonly Dictionary<string, FMOD.Sound> _loadedSounds = new Dictionary<string, FMOD.Sound>();

		private AudioManager()
		{ }

		/// <summary>
		/// Loads a sound from a file.
		/// </summary>
		/// <param name="filePath">The path to the sound file.</param>
		/// <param name="id">A unique string for the sound asset.</param>
		/// <param name="positional">Toggles 3D positional audio.</param>
		/// <param name="looping">Toggles whether the sound automatically loops.</param>
		/// <param name="oneAtATime">Toggles whether multiple instances of the sound can be played at the same time.</param>
		/// <returns>True if the sound was loaded.</returns>
		public bool LoadSound(string filePath, string id, bool positional, bool looping, bool oneAtATime)
		{
			FMOD.MODE mode = FMOD.MODE.DEFAULT | FMOD.MODE.CREATESAMPLE;

			if (positional)
				mode |= FMOD.MODE._3D | FMOD.MODE._3D_WORLDRELATIVE;

			if (looping)
				mode |= FMOD.MODE.LOOP_NORMAL;

			if (oneAtATime)
				mode |= FMOD.MODE.UNIQUE;

			FMOD.RESULT rc = RuntimeManager.CoreSystem.createSound(filePath, mode, out FMOD.Sound sound);

			if (rc == FMOD.RESULT.OK)
			{
				_loadedSounds.Add(id, sound);
				return true;
			}
			else
			{
				UnityEngine.Debug.LogError($"Neutronium.Audio: Failed to create sound. (id={id}, error={rc})");
				return false;
			}
		}

		public SoundHandle PrepareSound(string id, float volume, Vector3 position)
		{
			if (_loadedSounds.TryGetValue(id, out FMOD.Sound sound))
			{
				return PrepareSound(id, sound, volume, position);
			}

			return SoundHandle.Invalid;
		}

		/// <summary>
		/// Starts playing the sound for the given id.
		/// </summary>
		/// <param name="id">The sound id.</param>
		/// <returns>A SoundHandle instance that can be used to modify the played sound.</returns>
		public SoundHandle PlaySound(string id)
		{
			if (_loadedSounds.TryGetValue(id, out FMOD.Sound sound))
			{
				float volume = 1f;
				Vector3 position = SoundListenerController.Instance.transform.position;
				return PlaySound(id, sound, volume, position);
			}

			return SoundHandle.Invalid;
		}

		/// <summary>
		/// Starts playing the sound for the given id, with a specific volume.
		/// </summary>
		/// <param name="id">The sound id.</param>
		/// <param name="volume">A value in [0,1] to scale the volume.</param>
		/// <returns>A SoundHandle instance that can be used to modify the played sound.</returns>
		public SoundHandle PlaySound(string id, float volume)
		{
			if (_loadedSounds.TryGetValue(id, out FMOD.Sound sound))
			{
				Vector3 position = SoundListenerController.Instance.transform.position;
				return PlaySound(id, sound, volume, position);
			}

			return SoundHandle.Invalid;
		}

		public SoundHandle PlaySound(string id, Vector3 position)
		{
			if (_loadedSounds.TryGetValue(id, out FMOD.Sound sound))
			{
				float volume = 1f;
				return PlaySound(id, sound, volume, position);
			}

			return SoundHandle.Invalid;
		}

		/// <summary>
		/// Starts playing the sound for the given id, with a specific volume and 3D position.
		/// </summary>
		/// <param name="id">The sound id.</param>
		/// <param name="volume">A value in [0,1] to scale the volume.</param>
		/// <param name="position">A position in the scene for the sound to play from. (positional only)</param>
		/// <returns>A SoundHandle instance that can be used to modify the played sound.</returns>
		public SoundHandle PlaySound(string id, float volume, Vector3 position)
		{
			if (_loadedSounds.TryGetValue(id, out FMOD.Sound sound))
			{
				return PlaySound(id, sound, volume, position);
			}

			return SoundHandle.Invalid;
		}

		private SoundHandle PlaySound(string id, FMOD.Sound sound, float volume, Vector3 position)
		{
			SoundHandle handle = PrepareSound(id, sound, volume, position);
			if (handle.Valid)
			{
				handle.Channel.setPaused(false);
			}
			return handle;
		}

		private SoundHandle PrepareSound(string id, FMOD.Sound sound, float volume, Vector3 position)
		{
			FMOD.RESULT r1 = RuntimeManager.CoreSystem.getMasterChannelGroup(out FMOD.ChannelGroup cg);
			if (r1 == FMOD.RESULT.OK)
			{
				FMOD.RESULT r2 = RuntimeManager.CoreSystem.playSound(sound, cg, true, out FMOD.Channel channel);
				if (r2 == FMOD.RESULT.OK)
				{
					FMOD.VECTOR pos = CameraController.Instance.GetVerticallyScaledPosition(position, false).ToFMODVector();
					FMOD.VECTOR vel = new FMOD.VECTOR();
					channel.set3DAttributes(ref pos, ref vel);

					channel.setVolume(volume);

					SoundHandle handle = new SoundHandle(id, channel);

					return new SoundHandle(id, channel);
				}
			}

			return SoundHandle.Invalid;
		}
	}
}
