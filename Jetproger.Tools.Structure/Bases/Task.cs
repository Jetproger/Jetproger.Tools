using System;
using System.Collections.Generic;

namespace Jetproger.Tools.Structure.Bases
{
    [Serializable]
    public class Task : MarshalByRefObject
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Permission { get; set; }
        public string AssemblyName { get; set; }
        public string TypeName { get; set; }
        public bool IsolateCheck { get; set; }
        public bool IsolateWork { get; set; }
        public bool Remote { get; set; }
        public List<TaskItem> Items { get; set; }

        public Task()
        {
            Items = new List<TaskItem>();
            IsolateWork = true;
        }

        public override string ToString()
        {
            return Code;
        }
    }
}