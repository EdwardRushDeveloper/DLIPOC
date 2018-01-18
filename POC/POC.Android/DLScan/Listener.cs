using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Lib.Bcr;
using Com.Lib.Bcr.Utils;
using Java.Lang;
using BCR = Com;


namespace POC.Droid.DLScan
{
    public class BarcodeConnection : BCR.Lib.Bcr.McBcrConnection
    {
        public BarcodeConnection (Android.Content.Context context) : base(context)
        {

            


        }


    }
    [Register("POC/DLScan/POC/InternalListener")]
    public class InternalListener : Java.Lang.Object, IMiBcrListener,  IDisposable
    {
        public EventHandler<ScannedEventArgs> OnScannedHandler;
        public EventHandler<StatusChangedEventArgs> OnStatusChangedHandler;
        private object sender;

        public InternalListener(object sender) : base(JNIEnv.StartCreateInstance("POC/DLScan/POC/InternalListener", "()V", Array.Empty<JValue>()), JniHandleOwnership.TransferLocalRef)
        {
            JNIEnv.FinishCreateInstance(base.Handle, "()V", Array.Empty<JValue>());
            this.sender = sender;
        }

        internal static bool __IsEmpty(InternalListener value)
        {
            return ((value.OnScannedHandler == null) && (value.OnStatusChangedHandler == null));
        }


        public void OnScanned(BARCODE.TYPE p0, string p1, int p2)
        {
            this.OnScannedHandler?.Invoke(this.sender, new ScannedEventArgs(p0, p1, p2));
        }

        public void OnStatusChanged(int p0)
        {
            this.OnStatusChangedHandler?.Invoke(this.sender, new StatusChangedEventArgs(p0));
        }
    }

    public class Listener : IDisposable
    {
        

        public static Listener Current {get; set;}

        public static void Create(Android.Content.Context context)
        {
            new Listener(context);
        }
        BCR.Lib.Bcr.McBcrConnection _connection = null;
        
        public Listener(Android.Content.Context context)
        {
            _connection = new BCR.Lib.Bcr.McBcrConnection(context);
            InternalListener l = new InternalListener(context);

            _connection.SetListener(l);

            _connection.Scan(true);
            Current = this;
        }

        public void Close()
        {
            if(_connection != null)
            {
                _connection.Dispose();
                _connection = null;
            }
        }
        public void Dispose()
        {
          if(_connection != null)
            {
                _connection.Dispose();
                _connection = null;

            }
        }
    }
}