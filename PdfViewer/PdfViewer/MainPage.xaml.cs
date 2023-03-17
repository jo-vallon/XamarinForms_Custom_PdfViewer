using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PdfViewer
{
    public partial class MainPage : ContentPage
    {
        double x, y;
        double currentScale = 1;
        double startScale = 1;
        double xOffset = 0;
        double yOffset = 0;
        double h,w = 0;

        private double _scale = 1;
        public double ScaleImg
        {
            get { return _scale; }
            set
            {
                _scale = value;
                OnPropertyChanged();
            }
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            h = img_pdf.Height;
            w = img_pdf.Width;

            x = 0; y = 0;
            img_pdf.TranslationX = 0;
            img_pdf.TranslationY = 0;
            img_pdf.Scale = 1;
        }

        private void PinchGestureRecognizer_PinchUpdated(object sender, PinchGestureUpdatedEventArgs e)
        {
            try
            {
                if (e.Status == GestureStatus.Started)
                {
                    // Store the current scale factor applied to the wrapped user interface element,
                    // and zero the components for the center point of the translate transform.
                    startScale = Content.Scale;
                    //Content.AnchorX = 0;
                    //Content.AnchorY = 0;
                }
                if (e.Status == GestureStatus.Running)
                {
                    // Calculate the scale factor to be applied.
                    currentScale += (e.Scale - 1) * startScale;
                    currentScale = Math.Max(1, currentScale);

                    // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                    // so get the X pixel coordinate.
                    double renderedX = Content.X + xOffset;
                    double deltaX = renderedX / Width;
                    double deltaWidth = Width / (Content.Width * startScale);
                    double originX = (e.ScaleOrigin.X - deltaX) * deltaWidth;

                    // The ScaleOrigin is in relative coordinates to the wrapped user interface element,
                    // so get the Y pixel coordinate.
                    double renderedY = Content.Y + yOffset;
                    double deltaY = renderedY / Height;
                    double deltaHeight = Height / (Content.Height * startScale);
                    double originY = (e.ScaleOrigin.Y - deltaY) * deltaHeight;

                    // Calculate the transformed element pixel coordinates.
                    double targetX = xOffset - (originX * Content.Width) * (currentScale - startScale);
                    double targetY = yOffset - (originY * Content.Height) * (currentScale - startScale);

                    // Apply translation based on the change in origin.
                    Content.TranslationX = targetX.Clamp(-Content.Width * (currentScale - 1), 0);
                    Content.TranslationY = targetY.Clamp(-Content.Height * (currentScale - 1), 0);

                    // Apply scale factor.
                    Content.Scale = currentScale;
                }
                if (e.Status == GestureStatus.Completed)
                {
                    // Store the translation delta's of the wrapped user interface element.
                    xOffset = Content.TranslationX;
                    yOffset = Content.TranslationY;
                }
            }
            catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }

        private void btn_moin_Clicked(object sender, EventArgs e)
        {
            try
            {
                x = 0; y = 0;
                img_pdf.TranslationX = 0;
                img_pdf.TranslationY = 0;
                img_pdf.Scale = 2;
            }
            catch(Exception ex) { Console.WriteLine(); }
        }

        private void btn_5_Clicked(object sender, EventArgs e)
        {
            try
            {
                x = 0; y = 0;
                img_pdf.TranslationX = 0;
                img_pdf.TranslationY = 0;
                img_pdf.Scale = 5;
            }
            catch (Exception ex) { Console.WriteLine(); }
        }

        private void btn_10_Clicked(object sender, EventArgs e)
        {
            try
            {
                x = 0; y = 0;
                img_pdf.TranslationX = 0;
                img_pdf.TranslationY = 0;
                img_pdf.Scale = 10;

                img_pdf.HeightRequest = img_pdf.Height * 10;
                img_pdf.WidthRequest = img_pdf.Width * 10;

                cont.Padding = 300;
            }
            catch (Exception ex) { Console.WriteLine(); }
        }

        private void btn_plus_Clicked(object sender, EventArgs e)
        {
            try
            {
                x = 0; y = 0;
                img_pdf.TranslationX = 0;
                img_pdf.TranslationY = 0;
                img_pdf.Scale = 1;
            }
            catch (Exception ex) { Console.WriteLine(); }
        }

        private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            try
            {
                switch (e.StatusType)
                {
                    case GestureStatus.Running:
                        // Translate and ensure we don't pan beyond the wrapped user interface element bounds.
                        img_pdf.TranslationX = Math.Max( x + e.TotalX, -Math.Abs(cont.Width));
                        //img_pdf.TranslationX = x + e.TotalX;
                        img_pdf.TranslationY = Math.Max( y + e.TotalY, -Math.Abs(cont.Height));
                        //img_pdf.TranslationY = y + e.TotalY;
                        break;

                    case GestureStatus.Completed:
                        // Store the translation applied during the pan
                        x = img_pdf.TranslationX;
                        y = img_pdf.TranslationY;
                        break;
                }
            }
            catch(Exception ex) 
            { 
                Console.WriteLine(ex.Message);
            }
        }
    }
}
