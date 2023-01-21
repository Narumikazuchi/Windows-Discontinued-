namespace Narumikazuchi.Windows;

public partial class Utility
{
    [DllImport("Kernel32.dll", SetLastError = true)]
    static private extern UInt32 FormatMessage(UInt32 dwFlags,
                                               IntPtr lpSource,
                                               UInt32 dwMessageId,
                                               UInt32 dwLanguageId,
                                               ref IntPtr lpBuffer,
                                               UInt32 nSize,
                                               IntPtr pArguments);

    [DllImport("Kernel32.dll", SetLastError = true)]
    static private extern IntPtr LocalFree(IntPtr Name);

    private const UInt32 FORMAT_MESSAGE_ALLOCATE_BUFFER = 0x00000100;
    private const UInt32 FORMAT_MESSAGE_IGNORE_INSERTS = 0x00000200;
    private const UInt32 FORMAT_MESSAGE_FROM_SYSTEM = 0x00001000;
}