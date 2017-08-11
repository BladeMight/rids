using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ri {
	class raw_input {
		#region WinAPI
		public static uint WM_INPUT = 0x00FF;
		[DllImport("user32.dll")]
		public static extern bool RegisterRawInputDevices([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=0)] RAWINPUTDEVICE[] pRawInputDevices, int uiNumDevices, int cbSize);
		[DllImport("user32.dll")]
    	public static extern int GetRawInputData(IntPtr hRawInput, RawInputCommand uiCommand, out RAWINPUT pData, ref int pcbSize, int cbSizeHeader);
		/// <summary>Value type for raw input devices.</summary>
		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUTDEVICE {
			/// <summary>Top level collection Usage page for the raw input device.</summary>
			public ushort UsagePage;
			/// <summary>Top level collection Usage for the raw input device. </summary>
			public ushort Usage;
			/// <summary>Mode flag that specifies how to interpret the information provided by UsagePage and Usage.</summary>
			public RawInputDeviceFlags Flags;
			/// <summary>Handle to the target device. If NULL, it follows the keyboard focus.</summary>
			public IntPtr WindowHandle;
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct RAWINPUT {
			public RAWINPUTHEADER Header;
	        [StructLayout(LayoutKind.Explicit)]
			public struct data {
	        	[FieldOffset(0)]
				public RAWKEYBOARD Keyboard;
	            [FieldOffset(0)]
				public RAWMOUSE Mouse;
				[FieldOffset(0)]
				public RAWHID Hid;
			}
			public data Data;
		}
		[StructLayout(LayoutKind.Sequential)]
	    public struct RAWINPUTHEADER {
	        /// <summary>Type of device the input is coming from.</summary>
	        public RawInputType Type;
	        /// <summary>Size of the packet of data.</summary>
	        public int Size;
	        /// <summary>Handle to the device sending the data.</summary>
	        public IntPtr Device;
	        /// <summary>wParam from the window message.</summary>
	        public IntPtr wParam;
	    }
	    [Flags()]
	    public enum RawInputDeviceFlags {
		    /// <summary>No flags.</summary>
		    None = 0,
		    /// <summary>If set, this removes the top level collection from the inclusion list. This tells the operating system to stop reading from a device which matches the top level collection.</summary>
		    Remove = 0x00000001,
		    /// <summary>If set, this specifies the top level collections to exclude when reading a complete usage page. This flag only affects a TLC whose usage page is already specified with PageOnly.</summary>
		    Exclude = 0x00000010,
		    /// <summary>If set, this specifies all devices whose top level collection is from the specified usUsagePage. Note that Usage must be zero. To exclude a particular top level collection, use Exclude.</summary>
		    PageOnly = 0x00000020,
		    /// <summary>If set, this prevents any devices specified by UsagePage or Usage from generating legacy messages. This is only for the mouse and keyboard.</summary>
		    NoLegacy = 0x00000030,
		    /// <summary>If set, this enables the caller to receive the input even when the caller is not in the foreground. Note that WindowHandle must be specified.</summary>
		    InputSink = 0x00000100,
		    /// <summary>If set, the mouse button click does not activate the other window.</summary>
		    CaptureMouse = 0x00000200,
		    /// <summary>If set, the application-defined keyboard device hotkeys are not handled. However, the system hotkeys; for example, ALT+TAB and CTRL+ALT+DEL, are still handled. By default, all keyboard hotkeys are handled. NoHotKeys can be specified even if NoLegacy is not specified and WindowHandle is NULL.</summary>
		    NoHotKeys = 0x00000200,
		    /// <summary>If set, application keys are handled.  NoLegacy must be specified.  Keyboard only.</summary>
		    AppKeys = 0x00000400
		}
		public enum RawInputType {
	        Mouse = 0,	
	        Keyboard = 1,	
	        HID = 2,	
	        Other = 3
	    }
		[Flags()]
	    public enum RawMouseFlags : ushort {
	        /// <summary>Relative to the last position.</summary>
	        MoveRelative = 0,
	        /// <summary>Absolute positioning.</summary>
	        MoveAbsolute = 1,
	        /// <summary>Coordinate data is mapped to a virtual desktop.</summary>
	        VirtualDesktop = 2,
	        /// <summary>Attributes for the mouse have changed.</summary>
	        AttributesChanged = 4
	    }
		[Flags()]
		public enum RawMouseButtons : ushort {
	        /// <summary>No button.</summary>
	        None = 0,
	        /// <summary>Left (button 1) down.</summary>
	        LeftDown = 0x0001,
	        /// <summary>Left (button 1) up.</summary>
	        LeftUp = 0x0002,
	        /// <summary>Right (button 2) down.</summary>
	        RightDown = 0x0004,
	        /// <summary>Right (button 2) up.</summary>
	        RightUp = 0x0008,
	        /// <summary>Middle (button 3) down.</summary>
	        MiddleDown = 0x0010,
	        /// <summary>Middle (button 3) up.</summary>
	        MiddleUp = 0x0020,
	        /// <summary>Button 4 down.</summary>
	        Button4Down = 0x0040,
	        /// <summary>Button 4 up.</summary>
	        Button4Up = 0x0080,
	        /// <summary>Button 5 down.</summary>
	        Button5Down = 0x0100,
	        /// <summary>Button 5 up.</summary>
	        Button5Up = 0x0200,
	        /// <summary>Mouse wheel moved.</summary>
	        MouseWheel = 0x0400
	    }
	    [StructLayout(LayoutKind.Sequential)]
	    public struct RAWMOUSE {
	        /// <summary>
	        /// The mouse state.
	        /// </summary>
	        public RawMouseFlags Flags;
	        [StructLayout(LayoutKind.Explicit)]
	        public struct data {
	            [FieldOffset(0)]
	            public uint Buttons;
	            /// <summary>
	            /// If the mouse wheel is moved, this will contain the delta amount.
	            /// </summary>
	            [FieldOffset(2)]
	            public ushort ButtonData;
	            /// <summary>
	            /// Flags for the event.
	            /// </summary>
	            [FieldOffset(0)]
	            public RawMouseButtons ButtonFlags;
	        }
	        public data Data;
	        /// <summary>
	        /// Raw button data.
	        /// </summary>
	        public uint RawButtons;
	        /// <summary>
	        /// The motion in the X direction. This is signed relative motion or
	        /// absolute motion, depending on the value of usFlags.
	        /// </summary>
	        public int LastX;
	        /// <summary>
	        /// The motion in the Y direction. This is signed relative motion or absolute motion,
	        /// depending on the value of usFlags.
	        /// </summary>
	        public int LastY;
	        /// <summary>
	        /// The device-specific additional information for the event.
	        /// </summary>
	        public uint ExtraInformation;
	    }
	    [StructLayout(LayoutKind.Sequential)]
		public struct RAWKEYBOARD {
			public short MakeCode;
			public short Flags;
			public short Reserved;
			public ushort VKey;
			public uint   Message;
			public UInt32  ExtraInformation;
		}
	    [StructLayout(LayoutKind.Sequential)]
		public struct RAWHID {
			public int SizeHid;
 			public int Count;
 			public IntPtr RawData;
		}
		public enum RawInputCommand {
	        /// <summary>
	        /// Get input data.
	        /// </summary>
	        Input = 0x10000003,
	        /// <summary>
	        /// Get header data.
	        /// </summary>
	        Header = 0x10000005
	    }
		#endregion
		class f : Form {
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WM_INPUT) {
					RAWINPUT input = new RAWINPUT();
		            int outSize = 0;
		            int size = Marshal.SizeOf(typeof(RAWINPUT));
		
		            outSize = GetRawInputData(m.LParam, RawInputCommand.Input, out input, ref size, Marshal.SizeOf(typeof(RAWINPUTHEADER)));
		            if (outSize != -1)
		            {
		                if (input.Header.Type == RawInputType.Mouse) {
				            var x = Cursor.Position.X;
				            var y = Cursor.Position.Y;
		            	    Console.WriteLine("Mouse: " + (x+input.Data.Mouse.LastX) + "x" + (y+input.Data.Mouse.LastY) + " " + input.Data.Mouse.Data.ButtonFlags.ToString());
		                }
		                if (input.Header.Type == RawInputType.Keyboard) {
		            		Console.WriteLine("Keyboard: Key: [" + (Keys) input.Data.Keyboard.VKey +  "], VK [" + input.Data.Keyboard.VKey + "], MSG: [" + input.Data.Keyboard.Message + "], FLG: [" + input.Data.Keyboard.Flags + "].");
		            		if (input.Data.Keyboard.VKey == (int)Keys.F4)
		            			UnRegisterMouseKeyboard();
		                }
		            }
				}
				base.WndProc(ref m);
			}
			
		}
		static void RegisterMouseKeyboard(IntPtr hwnd) {
			RAWINPUTDEVICE[] rids = new RAWINPUTDEVICE[2];
			// Keyboard device
			rids[0].UsagePage = 0x1;
            rids[0].Usage = 0x06;
            rids[0].Flags = RawInputDeviceFlags.InputSink;
            rids[0].WindowHandle = hwnd;
			// Mouse device
            rids[1].UsagePage = 0x01;
            rids[1].Usage = 0x02;
            rids[1].Flags = RawInputDeviceFlags.InputSink;
            rids[1].WindowHandle = hwnd;
            // Registering devices
            if(!RegisterRawInputDevices(rids, 2, Marshal.SizeOf(typeof(RAWINPUTDEVICE)))) {
                Console.WriteLine("Registering raw input devices failed!");
            }
		}
		static void UnRegisterMouseKeyboard() {
			RAWINPUTDEVICE[] rids = new RAWINPUTDEVICE[2];
			// Keyboard device
			rids[0].UsagePage = 0x1;
            rids[0].Usage = 0x06;
            rids[0].Flags = RawInputDeviceFlags.Remove;
            rids[0].WindowHandle = IntPtr.Zero;
			// Mouse device
            rids[1].UsagePage = 0x1;
            rids[1].Usage = 0x02;
            rids[1].Flags = RawInputDeviceFlags.Remove;
            rids[1].WindowHandle = IntPtr.Zero;
            // Registering devices
            if(!RegisterRawInputDevices(rids, 2, Marshal.SizeOf(typeof(RAWINPUTDEVICE)))) {
            	Console.WriteLine("Unregistering raw input devices failed!" + Marshal.GetLastWin32Error());
            }
		}
		static void Main() {
			var ff = new f();
			RegisterMouseKeyboard(ff.Handle);
			ff.Show();
			Application.Run();
		}
	}
}