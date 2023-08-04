using Basler.Pylon;
using deneme.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace deneme.Helpers
{
    public class CameraHelper
    {
        private Stopwatch stopWatch = new Stopwatch();
        public static CameraHelper Instance { get; } = new CameraHelper();

        private void OnImageGrabbed(Object sender, ImageGrabbedEventArgs e)
        {
            //if (InvokeRequired)
            //{
            //    // If called from a different thread, we must use the Invoke method to marshal the call to the proper GUI thread.
            //    // The grab result will be disposed after the event call. Clone the event arguments for marshaling to the GUI thread.
            //    BeginInvoke(new EventHandler<ImageGrabbedEventArgs>(OnImageGrabbed), sender, e.Clone());
            //    return;
            //}

            try
            {
                // Acquire the image from the camera. Only show the latest image. The camera may acquire images faster than the images can be displayed.

                // Get the grab result.
                IGrabResult grabResult = e.GrabResult;

                // Check if the image can be displayed.
                if (grabResult.IsValid)
                {
                    // Reduce the number of displayed images to a reasonable amount if the camera is acquiring images very fast.
                    if (!stopWatch.IsRunning || stopWatch.ElapsedMilliseconds > 33)
                    {
                        stopWatch.Restart();

                        Bitmap bitmap = GrabResultConverterHelper.GrabResultTo_Bitmap(grabResult);

                        BitmapImage bitmapImage = GrabResultConverterHelper.BitmapToImageSource(bitmap);

                        // Assign a temporary variable to dispose the bitmap after assigning the new bitmap to the display control.
                        
                        // Provide the display control with the new bitmap. This action automatically updates the display.
                        
                    }
                }
            }
            catch (Exception exception)
            {
                //ShowException(exception);
            }
            finally
            {
                // Dispose the grab result if needed for returning it to the grab loop.
                e.DisposeGrabResultIfClone();
            }
        }


        public BitmapImage TakeOneFrame(CameraSettings cameraSettings)
        {

            try
            {
                using (Camera camera = new Camera())
                {
                    camera.CameraOpened += Configuration.AcquireSingleFrame;

                    camera.Open();

                    SetCameraSettings(camera, cameraSettings);

                    var grabResult = camera.StreamGrabber.GrabOne(5000);

                    if (grabResult.GrabSucceeded)
                    {
                        var bitmap = GrabResultConverterHelper.GrabResultTo_Bitmap(grabResult);
                        return GrabResultConverterHelper.BitmapToImageSource(bitmap);
                    } 
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}_{MethodBase.GetCurrentMethod()!.Name}");
            }
            return null;
        }

        private void SetCameraSettings(Camera camera, CameraSettings cameraSettings)
        {
            camera.Parameters[PLCamera.Width].TrySetValue(cameraSettings.Width, IntegerValueCorrection.Nearest);
            camera.Parameters[PLCamera.Height].TrySetValue(cameraSettings.Height, IntegerValueCorrection.Nearest);
            camera.Parameters[PLCamera.Gain].TrySetValue(cameraSettings.Gain);
            camera.Parameters[PLCamera.ExposureTime].TrySetValue(cameraSettings.ExposureTime);
        }
    }
}
