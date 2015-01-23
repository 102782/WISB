using System;
using System.Linq;
using System.Runtime.InteropServices;
using WISboot.Native;

namespace WISboot
{
    static class EnumExtensions
    {

        #region DllImports
        /// <summary>
        /// Translates (maps) a virtual-key code into a scan code or character value,
        /// or translates a scan code into a virtual-key code. 
        /// </summary>
        /// <param name="uCode">
        /// The virtual key code or scan code for a key.
        /// How this value is interpreted depends on the value of the uMapType parameter. </param>
        /// <param name="uMapType">
        /// The translation to be performed.
        /// The value of this parameter depends on the value of the uCode parameter.
        /// </param>
        /// <returns>
        /// The return value is either a scan code, a virtual-key code, or a character value,
        /// depending on the value of uCode and uMapType.
        /// If there is no translation, the return value is zero.
        /// </returns>
        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, uint uMapType);

        /// <summary>
        /// Translates (maps) a virtual-key code into a scan code or character value, or translates a scan code into a virtual key code.
        /// The function translates the codes using the input language and an input locale identifier.
        /// </summary>
        /// <param name="uCode">
        /// The virtual-key code or scan code for a key. 
        /// How this value is interpreted depends on the value of the uMapType parameter. 
        /// Starting with Windows Vista, the high byte of the uCode value can contain either 0xe0 or 0xe1 to specify the extended scan code.
        /// </param>
        /// <param name="uMapType">
        /// The translation to be performed.
        /// The value of this parameter depends on the value of the uCode parameter.
        /// </param>
        /// <param name="dwhkl">
        /// Input locale identifier to use for translating the specified code.
        /// This parameter can be any input locale identifier previously returned by the LoadKeyboardLayout function.
        /// </param>
        /// <returns>
        /// The return value is either a scan code, a virtual-key code, or a character value,
        /// depending on the value of uCode and uMapType.
        /// If there is no translation, the return value is zero.
        /// </returns>
        [DllImport("user32.dll")]
        static extern uint MapVirtualKeyEx(uint uCode, uint uMapType, IntPtr dwhkl);

        /// <summary>
        /// Retrieves the active input locale identifier (formerly called the keyboard layout).
        /// </summary>
        /// <param name="idThread">
        /// The identifier of the thread to query, or 0 for the current thread.
        /// </param>
        /// <returns>
        /// The return value is the input locale identifier for the thread.
        /// The low word contains a Language Identifier for the input language and the high word contains a device handle to the physical layout of the keyboard.
        /// </returns>
        [DllImport("user32.dll")]
        static extern IntPtr GetKeyboardLayout(uint idThread);

        #endregion

        #region KeyCodePairList
        static readonly Tuple<VirtualKeyCode, ScanCode>[] KeyCodePairList = new[]
        {
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.UP, ScanCode.DIK_UP), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.DOWN, ScanCode.DIK_DOWN), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.LEFT, ScanCode.DIK_LEFT), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.RIGHT, ScanCode.DIK_RIGHT), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.INSERT, ScanCode.DIK_INSERT), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.HOME, ScanCode.DIK_HOME), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.PRIOR, ScanCode.DIK_PRIOR),
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.DELETE, ScanCode.DIK_DELETE), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.END, ScanCode.DIK_END), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.NEXT, ScanCode.DIK_NEXT),
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.LMENU, ScanCode.DIK_LMENU), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.RMENU, ScanCode.DIK_RMENU), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.LCONTROL, ScanCode.DIK_LCONTROL), 
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.RCONTROL, ScanCode.DIK_RCONTROL),
            new Tuple<VirtualKeyCode, ScanCode>(VirtualKeyCode.DIVIDE, ScanCode.DIK_DIVIDE)
        };
        #endregion

        /// <summary>
        /// Convert VirtualKeyCode to ScanCode, taken from dinput.h
        /// </summary>
        /// <param name="vk">
        /// Virtual key code.
        /// </param>
        /// <returns>
        /// Keyboard scan code.
        /// </returns>
        static public ScanCode ToDIK(this VirtualKeyCode vk)
        {
            foreach (var dik in KeyCodePairList.Where(p => p.Item1 == vk).Select(p => p.Item2))
            {
                return dik;
            }
            return (ScanCode)MapVirtualKeyEx((uint)vk, (uint)MapVirtualKeyMapTypes.MAPVK_VK_TO_VSC, GetKeyboardLayout(0));
        }
    }
}
