namespace KnausTabbert.Display.GTX
{
    public class GtxSerialStatus
    {
        private bool _interruptMode = false;
        private bool _lastPage = true;
        private bool _confirmation = true;
        private bool _listMode = false;

        public bool InterruptMode
        {
            get { return _interruptMode; }
            set { _interruptMode = value; }
        }

        public bool LastPage
        {
            get { return _lastPage; }
            set { _lastPage = value; }
        }

        public bool Confirmation
        {
            get { return _confirmation; }
            set { _confirmation = value; }
        }

        public bool ListMode
        {
            get { return _listMode; }
            set { _listMode = value; }
        }

        public GtxSerialStatus()
        {
        }

        public GtxSerialStatus(bool interruptMode, bool lastPage, bool confirmation, bool listMode)
        {
            _interruptMode = interruptMode;
            _lastPage = lastPage;
            _confirmation = confirmation;
            _listMode = listMode;
        }

        public byte[] ToArray()
        {
            byte flags = ((byte)0)
                .SetBit(0, false)
                .SetBit(1, _interruptMode)
                .SetBit(2, !_lastPage)
                .SetBit(3, _confirmation)
                .SetBit(4, _listMode)
                .SetBit(5, false)
                .SetBit(6, true)
                .SetBit(7, true);

            return new[] { flags };
        }

        public override string ToString()
        {
            return string.Format("InterruptMode: {0}, LastPage: {1}, Confirmation: {2}, ListMode: {3}", InterruptMode, LastPage, Confirmation, ListMode);
        }
    }
}
