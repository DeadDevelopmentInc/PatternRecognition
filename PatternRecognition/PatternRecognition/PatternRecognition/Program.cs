﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PatternRecognition
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.Run(new MainWindow());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
    }
}
