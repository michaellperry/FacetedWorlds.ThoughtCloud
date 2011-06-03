using System.Windows.Messaging;
using System;

namespace FacetedWorlds.ThoughtCloud.SilverlightLocal
{
    public class LocalInstanceMessenger : IDisposable
    {
        private LocalMessageReceiver _receiver;
        private bool _singleInstance;

        public LocalInstanceMessenger()
        {
            _receiver = new LocalMessageReceiver("FacetedWorlds.ThoughtCloud");

            try
            {
                _receiver.Listen();
                _singleInstance = true;
            }
            catch (ListenFailedException x)
            {
                _singleInstance = false;
            }
        }

        public bool SingleInstance
        {
            get { return _singleInstance; }
        }

        public void Dispose()
        {
            _receiver.Dispose();
        }
    }
}
