using deneme.Models;
using deneme.Utilities.ReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace deneme.Validations.Camera
{
    public class CameraSettingsValidator
    {
        public static ValidationResult IsValid(CameraSettings cameraSettings)
        {
            bool isValid = true;
            string errorMessage = string.Empty;

            if (cameraSettings.Width > 3088)
            {
                isValid = false;
                errorMessage += "Width cannot be greater than 3088\n";
            }
            if (cameraSettings.Width < 376)
            {
                isValid = false;
                errorMessage += "Width cannot be less than 376\n";
            }
            if (cameraSettings.Width == -1)
            {
                isValid = false;
                errorMessage += "Width value must be numerical\n";

            }
            if (cameraSettings.Height > 2064)
            {
                isValid = false;
                errorMessage += "Height cannot be greater than 2064\n";
            }
            if (cameraSettings.Height < 320)
            {
                isValid = false;
                errorMessage += "Height cannot be less than 320\n";
            }
            if (cameraSettings.Height == -1)
            {
                isValid = false;
                errorMessage += "Height value must be numerical\n";
            }

            if (isValid)
                return new ValidationResult() { IsValid = true, Message = string.Empty };

            return new ValidationResult() { IsValid = isValid, Message = errorMessage };
        }
    }
}
