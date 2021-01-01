using InputManager;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace NumpadMouse {
    public partial class Form1 : Form {

        static bool bDown = false, bLeft=false, bUp=false, bRight=false, bStop, bHalfSpeed;
        static int speed = 3;

        public Form1() {
            InitializeComponent();
        }

        void KeyboardHook_KeyUp(int vkCode) {
            //Everytime the users releases a certain key up,
            //your application will go to this line
            //Use the vKCode argument to determine which key has been released

            if (vkCode == 0x64) bLeft   = false;
            if (vkCode == 0x68) bUp     = false;
            if (vkCode == 0x66) bRight  = false;
            if (vkCode == 0x65) bDown   = false;
            if (vkCode == 110) bStop    = false;
            if (vkCode == 0xA2) bHalfSpeed = false;
            if (vkCode == 0x60) {
                InputManager.Mouse.ButtonUp(Mouse.MouseKeys.Left);
            }
            if (vkCode == 0x61 ){
                InputManager.Mouse.ButtonUp(Mouse.MouseKeys.Right);
            }
        }

        void KeyboardHook_KeyDown(int vkCode) {
            //Everytime the users holds a certain key down,
            //your application will go to this line
            //Use the vKCode argument to determine which key is held down
            if (vkCode == 0x64) bLeft   = true;
            if (vkCode == 0x68) bUp     = true;
            if (vkCode == 0x66) bRight  = true;
            if (vkCode == 0x65) bDown   = true;
            if (vkCode == 110) bStop    = true;
            if (vkCode == 0xA2) bHalfSpeed = true;

            if (vkCode == 0x60) {
                InputManager.Mouse.ButtonDown(Mouse.MouseKeys.Left);
            }
            if (vkCode == 0x61) {
                InputManager.Mouse.ButtonDown(Mouse.MouseKeys.Right);
            }
            if (vkCode == 98) {
                speed--;
                if (speed < 0) speed = 0;
                Thread.Sleep(10);
            }
            if (vkCode == 99) {
                speed++;
                if (speed < 0) speed = 0;
                Thread.Sleep(10);
            }


        }

        public static void ThreadProc() {
            
            while (!bStop) {
                int mouseX = 0;
                int mouseY = 0;

                if (bHalfSpeed) {
                    if (bLeft) mouseX -= (int)(speed/2);
                    if (bRight) mouseX += (int)(speed / 2);
                    if (bUp) mouseY -= (int)(speed / 2);
                    if (bDown) mouseY += (int)(speed / 2);
                }
                else {
                    if (bLeft) mouseX -= speed;
                    if (bRight) mouseX += speed;
                    if (bUp) mouseY -= speed;
                    if (bDown) mouseY += speed;
                }
                
                InputManager.Mouse.MoveRelative(mouseX, mouseY);
                Thread.Sleep(5);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

            KeyboardHook.KeyDown += new KeyboardHook.KeyDownEventHandler(KeyboardHook_KeyDown);
            KeyboardHook.KeyUp += new KeyboardHook.KeyUpEventHandler(KeyboardHook_KeyUp);
            KeyboardHook.InstallHook();

            Thread t = new Thread(new ThreadStart(ThreadProc));

            t.Start();
            
        }

        private void button1_Click(object sender, EventArgs e) {
            
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private void label1_Click(object sender, EventArgs e) {

        }
    }


}
