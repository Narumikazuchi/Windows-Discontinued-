using System;

namespace Narumikazuchi.Windows.Win32
{
    internal struct __Point
    {
        #region Constructor

        public __Point(Int32 x, Int32 y)
        {
            this.X = x;
            this.Y = y;
        }

        #endregion

        #region Properties

        public Int32 X;
        public Int32 Y;

        #endregion
    }
}
