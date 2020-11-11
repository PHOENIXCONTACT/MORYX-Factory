﻿using System;
using Moryx.ClientFramework.Kernel;

namespace StartProject.UI
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var heartOfLead = new HeartOfLead(args);
            heartOfLead.Initialize();
            heartOfLead.Start();
        }
    }
}