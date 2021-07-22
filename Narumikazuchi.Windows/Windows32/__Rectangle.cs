using System;

namespace Narumikazuchi.Windows.Win32
{
    internal struct __Rectangle
    {
        #region Constructor

        public __Rectangle(Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        #endregion

        #region Properties

        public Int32 Left;
        public Int32 Top;
        public Int32 Right;
        public Int32 Bottom;

        #endregion
    }
}
