using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ImportEnExportTool
{
    /// <summary>
    /// Obsolete. laat ik er nog efkes in maar mag waarschijnlijk verwijderd worden.
    /// </summary>
    public class WaitCursor : IDisposable
    {
        private Cursor _previousCursor;

        public WaitCursor()
        {
            _previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}
