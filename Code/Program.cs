using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace XNA_Debug
{

    static class Start
    {
        /// <summary>
        /// Punkt startowy całej aplikacji.
        /// </summary>
        static void Main()
        {

            using (KinectGame game = new KinectGame())
            {
                game.Run();

            }

        }
    }

}

