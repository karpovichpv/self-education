using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace AsyncResultImplementation
{
    class Message : IMessage
    {
        private IDictionary _dictionary;

        // Конструктор
        public Message(WaitCallback asyncTask,
                       AsyncCallback asyncCallback,
                       object asyncState)
        {
            _dictionary = new Hashtable();
            _dictionary.Add("asyncTask", asyncTask);
            _dictionary.Add("asyncCallback", asyncCallback);
            _dictionary.Add("asyncState", asyncState);
        }

        public IDictionary Properties
        {
            get { return _dictionary; }
        }
    }
}
