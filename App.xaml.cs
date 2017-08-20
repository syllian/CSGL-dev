using CSGL.libGame.loop;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace WPFGfxTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static class EntryClass
        {
            [STAThread()]
            private static void Main()
            {
                App app = new App();
                app.Run();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            CreateAndShowMainWindow(800, 600);
        }

        private static int width = 800;
        private static int height = 600;
        private static string title = "Writeable Bitmap";

        private Window mainWindow;


        private Image canvas = new Image();
        private static WriteableBitmap screen = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

        private static int _bytesPerPixel = (screen.Format.BitsPerPixel + 7) >> 3;


        // Define the rectangle of the writeable image we will modify. 
        // The size is that of the writeable bitmap.
        private static Int32Rect _rect = new Int32Rect(0, 0, screen.PixelWidth, screen.PixelHeight);

        // Stride is bytes per pixel times the number of pixels.
        // Stride is the byte width of a single rectangle row.
        private static int _stride = screen.PixelWidth * _bytesPerPixel;

        // Create a byte array for a the entire size of bitmap.
        private static int _arraySize = _stride * screen.PixelHeight;
        private static byte[] _colorArray = new byte[_arraySize];



        private void CreateAndShowMainWindow(int width, int height)
        {
            mainWindow = new Window();
            mainWindow.Title = title;
            mainWindow.Width = width;
            mainWindow.Height = height;

            
            canvas.Stretch = Stretch.None;
            canvas.Margin = new Thickness(0);

            
            // Define a StackPanel to host Controls
            StackPanel myStackPanel = new StackPanel();
            myStackPanel.Orientation = Orientation.Vertical;
            myStackPanel.Width = width;
            myStackPanel.Height = height;
            myStackPanel.VerticalAlignment = VerticalAlignment.Top;
            myStackPanel.HorizontalAlignment = HorizontalAlignment.Center;
            myStackPanel.Children.Add(canvas);
            mainWindow.Content = myStackPanel;
            mainWindow.Show();


            // DispatcherTimer setup
            // The DispatcherTimer will be used to update _random every
            //    second with a new random set of colors.
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.IsEnabled = true;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        //  System.Windows.Threading.DispatcherTimer.Tick handler
        //
        //  Updates the Image element with new random colors
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Update the color array with new random colors
            Random value = new Random();
            value.NextBytes(_colorArray);

            //Update writeable bitmap with the colorArray to the image.
            screen.WritePixels(_rect, _colorArray, _stride, 0);

            //Set the Image source.
            canvas.Source = screen;
        }
    }
}
