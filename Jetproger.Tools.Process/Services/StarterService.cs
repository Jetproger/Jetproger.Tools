﻿using System.ServiceProcess;

namespace Jetproger.Tools.Process.Services
{
    public class StarterService : ServiceBase
    {
        public void Start()
        {
            OnStart(new string[0]);
        }

        public void Start(string[] args)
        {
            OnStart(args);
        }
    }
}