using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGL.libGame.loop
{
    public interface ILoop
    {
        void init();
        void cleanUp();
        void setTargetFPS(int fps);
        void setTargetHz(int hertz);
        void pause(bool pause);
        void start();
        void update(double time);
        void render(double interpolation);
        void stop();
    }
}
