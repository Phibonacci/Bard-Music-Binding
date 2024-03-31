using System.Collections.Generic;

namespace Win32
{
    internal static class KeyCodes
    {
        internal static uint GodotKeyCodeToWin32ScanCode(uint godotKeyCode)
        {
            return godotKeyCodeToWin32ScanCode.GetValueOrDefault(godotKeyCode);
        }

        static readonly uint SPECIAL = (1 << 22);

        private static readonly Dictionary<uint, uint> godotKeyCodeToWin32ScanCode = new() {
            { 0x00, 0 }, // NONE
            // CURSOR/FUNCTION/BROWSER/MULTIMEDIA/MISC KEYS
            { SPECIAL | 0x01, 110 }, // ESCAPE
            { SPECIAL | 0x02, 15 }, // TAB
            { SPECIAL | 0x03, 0 }, // BACKTAB
            { SPECIAL | 0x04, 14 }, // BACKSPACE
            { SPECIAL | 0x05, 28 }, // ENTER
            { SPECIAL | 0x06, 156 }, // KP_ENTER
            { SPECIAL | 0x07, 210 }, // INSERT
            { SPECIAL | 0x08, 211 }, // KEY_DELETE // "DELETE" is a reserved word on Windows.
            { SPECIAL | 0x09, 197 }, // PAUSE
            { SPECIAL | 0x0A, 0 }, // PRINT
            { SPECIAL | 0x0B, 183 }, // SYSREQ
            { SPECIAL | 0x0C, 0 }, // CLEAR
            { SPECIAL | 0x0D, 199 }, // HOME
            { SPECIAL | 0x0E, 207 }, // END
            { SPECIAL | 0x0F, 203 }, // LEFT
            { SPECIAL | 0x10, 200 }, // UP
            { SPECIAL | 0x11, 205 }, // RIGHT
            { SPECIAL | 0x12, 208 }, // DOWN
            { SPECIAL | 0x13, 201 }, // PAGEUP
            { SPECIAL | 0x14, 209 }, // PAGEDOWN
            { SPECIAL | 0x15, 42 }, // SHIFT
            { SPECIAL | 0x16, 29 }, // CTRL
            { SPECIAL | 0x17, 219 }, // META
            { SPECIAL | 0x18, 56 }, // ALT
            { SPECIAL | 0x19, 58 }, // CAPSLOCK
            { SPECIAL | 0x1A, 69 }, // NUMLOCK
            { SPECIAL | 0x1B, 70 }, // SCROLLLOCK
            { SPECIAL | 0x1C, 59 }, // F1
            { SPECIAL | 0x1D, 60 }, // F2
            { SPECIAL | 0x1E, 61 }, // F3
            { SPECIAL | 0x1F, 62 }, // F4
            { SPECIAL | 0x20, 63 }, // F5
            { SPECIAL | 0x21, 64 }, // F6
            { SPECIAL | 0x22, 65 }, // F7
            { SPECIAL | 0x23, 66 }, // F8
            { SPECIAL | 0x24, 67 }, // F9
            { SPECIAL | 0x25, 68 }, // F10
            { SPECIAL | 0x26, 87 }, // F11
            { SPECIAL | 0x27, 88 }, // F12
            { SPECIAL | 0x28, 100 }, // F13
            { SPECIAL | 0x29, 101 }, // F14
            { SPECIAL | 0x2A, 102 }, // F15
            { SPECIAL | 0x2B, 0 }, // F16
            { SPECIAL | 0x2C, 0 }, // F17
            { SPECIAL | 0x2D, 0 }, // F18
            { SPECIAL | 0x2E, 0 }, // F19
            { SPECIAL | 0x2F, 0 }, // F20
            { SPECIAL | 0x30, 0 }, // F21
            { SPECIAL | 0x31, 0 }, // F22
            { SPECIAL | 0x32, 0 }, // F23
            { SPECIAL | 0x33, 0 }, // F24
            { SPECIAL | 0x34, 0 }, // F25
            { SPECIAL | 0x35, 0 }, // F26
            { SPECIAL | 0x36, 0 }, // F27
            { SPECIAL | 0x37, 0 }, // F28
            { SPECIAL | 0x38, 0 }, // F29
            { SPECIAL | 0x39, 0 }, // F30
            { SPECIAL | 0x3A, 0 }, // F31
            { SPECIAL | 0x3B, 0 }, // F32
            { SPECIAL | 0x3C, 0 }, // F33
            { SPECIAL | 0x3D, 0 }, // F34
            { SPECIAL | 0x3E, 0 }, // F35
            { SPECIAL | 0x81, 55 }, // KP_MULTIPLY
            { SPECIAL | 0x82, 181 }, // KP_DIVIDE
            { SPECIAL | 0x83, 74 }, // KP_SUBTRACT
            { SPECIAL | 0x84, 83 }, // KP_PERIOD
            { SPECIAL | 0x85, 78 }, // KP_ADD
            { SPECIAL | 0x86, 82 }, // KP_0
            { SPECIAL | 0x87, 79 }, // KP_1
            { SPECIAL | 0x88, 80 }, // KP_2
            { SPECIAL | 0x89, 81 }, // KP_3
            { SPECIAL | 0x8A, 75 }, // KP_4
            { SPECIAL | 0x8B, 76 }, // KP_5
            { SPECIAL | 0x8C, 77 }, // KP_6
            { SPECIAL | 0x8D, 71 }, // KP_7
            { SPECIAL | 0x8E, 72 }, // KP_8
            { SPECIAL | 0x8F, 73 }, // KP_9
            { SPECIAL | 0x42, 221 }, // MENU
            { SPECIAL | 0x43, 0 }, // HYPER
            { SPECIAL | 0x45, 0 }, // HELP
            { SPECIAL | 0x48, 234 }, // BACK
            { SPECIAL | 0x49, 233 }, // FORWARD
            { SPECIAL | 0x4A, 232 }, // STOP
            { SPECIAL | 0x4B, 231 }, // REFRESH
            { SPECIAL | 0x4C, 174 }, // VOLUMEDOWN
            { SPECIAL | 0x4D, 160 }, // VOLUMEMUTE
            { SPECIAL | 0x4E, 175 }, // VOLUMEUP
            { SPECIAL | 0x54, 162 }, // MEDIAPLAY
            { SPECIAL | 0x55, 164 }, // MEDIASTOP
            { SPECIAL | 0x56, 144 }, // MEDIAPREVIOUS
            { SPECIAL | 0x57, 153 }, // MEDIANEXT
            { SPECIAL | 0x58, 0 }, // MEDIARECORD
            { SPECIAL | 0x59, 178 }, // HOMEPAGE
            { SPECIAL | 0x5A, 230 }, // FAVORITES
            { SPECIAL | 0x5B, 229 }, // SEARCH
            { SPECIAL | 0x5C, 0 }, // STANDBY
            { SPECIAL | 0x5D, 0 }, // OPENURL
            { SPECIAL | 0x5E, 236 }, // LAUNCHMAIL
            { SPECIAL | 0x5F, 237 }, // LAUNCHMEDIA
            { SPECIAL | 0x60, 0 }, // LAUNCH0
            { SPECIAL | 0x61, 0 }, // LAUNCH1
            { SPECIAL | 0x62, 0 }, // LAUNCH2
            { SPECIAL | 0x63, 0 }, // LAUNCH3
            { SPECIAL | 0x64, 0 }, // LAUNCH4
            { SPECIAL | 0x65, 0 }, // LAUNCH5
            { SPECIAL | 0x66, 0 }, // LAUNCH6
            { SPECIAL | 0x67, 0 }, // LAUNCH7
            { SPECIAL | 0x68, 0 }, // LAUNCH8
            { SPECIAL | 0x69, 0 }, // LAUNCH9
            { SPECIAL | 0x6A, 0 }, // LAUNCHA
            { SPECIAL | 0x6B, 0 }, // LAUNCHB
            { SPECIAL | 0x6C, 0 }, // LAUNCHC
            { SPECIAL | 0x6D, 0 }, // LAUNCHD
            { SPECIAL | 0x6E, 0 }, // LAUNCHE
            { SPECIAL | 0x6F, 0 }, // LAUNCHF

            { SPECIAL | 0x70, 0 }, // GLOBE
            { SPECIAL | 0x71, 0 }, // KEYBOARD
            { SPECIAL | 0x72, 0 }, // JIS_EISU
            { SPECIAL | 0x73, 112 }, // JIS_KANA

            { SPECIAL | 0x7FFFFF, 0 }, // UNKNOWN

            // PRINTABLE LATIN 1 CODES
            { 0x0020, 57 }, // SPACE
            { 0x0021, 53 }, // EXCLAM
            { 0x0022, 0 }, // QUOTEDBL
            { 0x0023, 0 }, // NUMBERSIGN
            { 0x0024, 27 }, // DOLLAR
            { 0x0025, 0 }, // PERCENT
            { 0x0026, 0 }, // AMPERSAND
            { 0x0027, 40 }, // APOSTROPHE
            { 0x0028, 0 }, // PARENLEFT
            { 0x0029, 0 }, // PARENRIGHT
            { 0x002A, 43 }, // ASTERISK
            { 0x002B, 0 }, // PLUS
            { 0x002C, 51 }, // COMMA
            { 0x002D, 12 }, // MINUS
            { 0x002E, 52 }, // PERIOD
            { 0x002F, 53 }, // SLASH
            { 0x0030, 11 }, // KEY_0
            { 0x0031, 2 }, // KEY_1
            { 0x0032, 3 }, // KEY_2
            { 0x0033, 4 }, // KEY_3
            { 0x0034, 5 }, // KEY_4
            { 0x0035, 6 }, // KEY_5
            { 0x0036, 7 }, // KEY_6
            { 0x0037, 8 }, // KEY_7
            { 0x0038, 9 }, // KEY_8
            { 0x0039, 10 }, // KEY_9
            { 0x003A, 146 }, // COLON
            { 0x003B, 39 }, // SEMICOLON
            { 0x003C, 86 }, // LESS
            { 0x003D, 13 }, // EQUAL
            { 0x003E, 0 }, // GREATER
            { 0x003F, 115 }, // QUESTION
            { 0x0040, 145 }, // AT
            { 0x0041, 30 }, // A
            { 0x0042, 48 }, // B
            { 0x0043, 46 }, // C
            { 0x0044, 32 }, // D
            { 0x0045, 18 }, // E
            { 0x0046, 33 }, // F
            { 0x0047, 34 }, // G
            { 0x0048, 35 }, // H
            { 0x0049, 23 }, // I
            { 0x004A, 36 }, // J
            { 0x004B, 37 }, // K
            { 0x004C, 38 }, // L
            { 0x004D, 50 }, // M
            { 0x004E, 49 }, // N
            { 0x004F, 24 }, // O
            { 0x0050, 25 }, // P
            { 0x0051, 16 }, // Q
            { 0x0052, 19 }, // R
            { 0x0053, 31 }, // S
            { 0x0054, 20 }, // T
            { 0x0055, 22 }, // U
            { 0x0056, 47 }, // V
            { 0x0057, 17 }, // W
            { 0x0058, 45 }, // X
            { 0x0059, 21 }, // Y
            { 0x005A, 44 }, // Z
            { 0x005B, 26 }, // BRACKETLEFT
            { 0x005C, 43 }, // BACKSLASH
            { 0x005D, 27 }, // BRACKETRIGHT
            { 0x005E, 0 }, // ASCIICIRCUM
            { 0x005F, 0 }, // UNDERSCORE
            { 0x0060, 41 }, // QUOTELEFT
            { 0x007B, 0 }, // BRACELEFT
            { 0x007C, 0 }, // BAR
            { 0x007D, 0 }, // BRACERIGHT
            { 0x007E, 0 }, // ASCIITILDE
            { 0x00A5, 125 }, // YEN
            { 0x00A7, 0 }, // SECTION
	    };

    }
}
