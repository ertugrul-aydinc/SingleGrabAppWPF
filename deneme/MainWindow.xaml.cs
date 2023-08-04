using Basler.Pylon;
using deneme.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using deneme.Models;
using deneme.Validations.Camera;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Windows.Media.Media3D;

namespace deneme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Stopwatch stopWatch = new();
        Basler.Pylon.Camera camera = new Basler.Pylon.Camera();

        public MainWindow()
        {
            InitializeComponent();

            

            //CameraHelper cameraHelper = new();
            //cameraHelper.TakeOneFrame();
        }

        //private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        //{
        //    //if (InvokeRequired)
        //    //{
        //    //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
        //    //    // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
        //    //    BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
        //    //    return;
        //    //}

        //    try
        //    {
        //        // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

        //        // Get the grab result.
        //        IGrabResult grabResult = e.GrabResult;

        //        // Check if the image can be displayed.
        //        if (grabResult.IsValid)
        //        {
        //            // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
        //            if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
        //            {
        //                stopWatch.Restart();

        //                Bitmap bitmap;

        //                this.Dispatcher.Invoke(() =>
        //                {
        //                    bitmap = GrabResultConverterHelper.GrabResultTo_Bitmap(grabResult);
        //                });

        //                BitmapImage bitmapImage;
        //                this.Dispatcher.Invoke(() =>
        //                {
        //                    var bitmapImage = GrabResultConverterHelper.BitmapToImageSource(bitmap);
        //                });
                        

        //                // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
        //                //BitmapImage bitmapImageOld = imageViewer.Source as BitmapImage;
        //                // Provide the display control with the new bitmap. This action automatically updates the display.

        //                this.Dispatcher.Invoke(() =>
        //                {
        //                    imageViewer.Source = bitmapImage;
        //                });
        //            }
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        //ShowException(exception);
        //    }
        //    finally
        //    {
        //        // Dispose the grab result if needed for returning it to the grab loop.
        //        e.DisposeGrabResultIfClone();
        //    }
        //}

        //private void ContinuousShot()
        //{
        //    try
        //    {
        //        camera.CameraOpened += Configuration.AcquireContinuous;

        //        camera.StreamGrabber.ImageGrabbed += OnImageGrabbed;

        //        camera.Open();
                
        //        // Start the grabbing of images until grabbing is stopped.
        //        Configuration.AcquireContinuous(camera, null);
        //        camera.StreamGrabber.Start(GrabStrategy.OneByOne, GrabLoop.ProvidedByStreamGrabber);
        //    }
        //    catch (Exception exception)
        //    {
        //        //ShowException(exception);
        //    }
        //}

        private void takePhoto_Click(object sender, RoutedEventArgs e)
        {
            //ContinuousShot();

            TakePhoto();
        }
       
        private void TakePhoto()
        {
            var settings = new CameraSettings()
            {
                Width = Convert.ToInt32(widthSlider.Value),
                Height = Convert.ToInt32(heightSlider.Value),
                Gain = gainSlider.Value,
                ExposureTime = exposureSlider.Value
            };

            var result = CameraSettingsValidator.IsValid(settings);

            if (result.IsValid)
                imageViewer.Source = CameraHelper.Instance.TakeOneFrame(settings);
            else
                MessageBox.Show(result.Message);

            
        }

        private int SetValueFromTextBox(string text)
        {
            if(!double.TryParse(text, out _)) return -1;
          
            return Convert.ToInt32(text);
        }

        private T SetValueFromTextBox<T>(string text) => (T)Convert.ChangeType(text, typeof(T));


        private double TextBoxToSlider(double min, double max, Slider slider, string text)
        {
            try
            {
                double value;

                if (double.TryParse(text, out value))
                    if (!(value < min || value > max))
                        slider.Value = value;

                return value;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message} --- {MethodBase.GetCurrentMethod()!.Name}");
                return -1;
            }
        }

        private void txtHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = TextBoxToSlider(320, 2064, heightSlider, txtHeight.Text);
            if (value % 2 != 0)
                heightSlider.Value = value - 1;
        }

        private void txtGain_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxToSlider(0, 36, gainSlider, txtGain.Text);
        }

        private void txtExposureTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBoxToSlider(8, 9999992, exposureSlider, txtExposureTime.Text);
        }

        private void txtWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            var value = TextBoxToSlider(376, 3088, widthSlider, txtWidth.Text);
            if (value % 2 != 0)
                widthSlider.Value = value - 1;
        }
    }
}
