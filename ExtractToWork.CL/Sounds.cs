using System;
using System.Media;

namespace ExtractToWork.CL
{
    public class Sounds
    {
        private SoundPlayer _player;

        public Sounds()
        {
            _player = new SoundPlayer();
        }

        public void Click()
        {
            _player.SoundLocation = Environment.CurrentDirectory + "\\Sounds\\click.wav";
            _player.Play();
        }

        public void Done()
        {
            _player.SoundLocation = Environment.CurrentDirectory + "\\Sounds\\done_2.wav";
            _player.Play();
        }
    }
}
