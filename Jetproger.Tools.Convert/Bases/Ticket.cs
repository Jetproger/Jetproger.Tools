using System;
using System.Runtime.Serialization;

namespace Jc
{
    [Serializable]
    [DataContract]
    public class Ticket
    {
        private bool _isException;
        private string _text;
        private string _description;

        [DataMember]
        public bool IsException
        {
            get { return _isException; }
            set { _isException = value; }
        }

        [DataMember]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [DataMember]
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}