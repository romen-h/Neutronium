using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FMODUnity;

using UnityEngine;

namespace Neutronium.Audio
{
	public class SoundHandle
	{
		public static readonly SoundHandle Invalid = new SoundHandle();

		public bool Valid
		{ get; internal set; }

		public readonly string ID;
		public readonly FMOD.Channel Channel;

		internal SoundHandle()
		{
			Valid = false;
		}

		internal SoundHandle(string id, FMOD.Channel channel)
		{
			ID = id;
			Channel = channel;
			Valid = channel.handle != IntPtr.Zero;
		}

		public void Start()
		{
			if (Valid)
			{
				Channel.setPaused(false);
			}
		}

		public void Pause()
		{
			if (Valid)
			{
				Channel.setPaused(true);
			}
		}

		public float Volume
		{
			get
			{
				if (Valid)
				{
					Channel.getVolume(out float volume);
					return volume;
				}

				return 0f;
			}
			set
			{
				if (Valid)
				{
					Channel.setVolume(value);
				}
			}
		}

		public void Destroy()
		{
			if (Valid)
			{
				Channel.stop();
				Channel.clearHandle();
				Valid = false;
			}
		}

		public void SetPosition(Vector3 position)
		{
			FMOD.VECTOR pos = CameraController.Instance.GetVerticallyScaledPosition(position, false).ToFMODVector();
			FMOD.VECTOR vel = new FMOD.VECTOR();
			Channel.set3DAttributes(ref pos, ref vel);
		}
	}
}
