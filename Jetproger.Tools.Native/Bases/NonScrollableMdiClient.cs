using System;
using System.Windows.Forms;

namespace Tools
{
    public static unsafe partial class Native
    {
        public class NonScrollableMdiClient : NativeWindow
        {
            public void OnHandleDestroyed(object sender, EventArgs e)
            {
                ReleaseHandle();
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == WindowMessages.WM_SCROLL)
                {
                    return;
                }
                base.WndProc(ref m);
            }
        }
    }
}