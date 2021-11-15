using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

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
