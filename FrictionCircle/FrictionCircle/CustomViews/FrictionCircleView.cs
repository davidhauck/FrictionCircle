using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FrictionCircle.CustomViews
{
    public class FrictionCircleView : SKCanvasView
    {
        private const string OuterText = "1.5g";
        private const string CenterText = "1.0g";
        private const string InnerText = "0.5g";

        private const int CircleStrokeWidth = 3;
        private const int TextStrokeWidth = 1;
        private const int TextSize = 20;
        private const int MarkerRadius = 7;

        private float _scaling;

        public static BindableProperty XAccProperty = BindableProperty.Create(nameof(XAcc), typeof(float), typeof(FrictionCircleView), 0f, defaultBindingMode:BindingMode.TwoWay, propertyChanged: OnXAccChanged);

        private static void OnXAccChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var frictionCircleView = bindable as FrictionCircleView;
            frictionCircleView.XAcc = (float)newValue;
            frictionCircleView.InvalidateSurface();
        }

        public float XAcc
        {
            get
            {
                return (float)this.GetValue(XAccProperty);
            }
            set
            {
                this.SetValue(XAccProperty, value);
            }
        }

        public static BindableProperty YAccProperty = BindableProperty.Create(nameof(YAcc), typeof(float), typeof(FrictionCircleView), 0f, defaultBindingMode:BindingMode.TwoWay, propertyChanged:OnYAccChanged);

        private static void OnYAccChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var frictionCircleView = bindable as FrictionCircleView;
            frictionCircleView.YAcc = (float)newValue;
            frictionCircleView.InvalidateSurface();
        }

        public float YAcc
        {
            get
            {
                return (float)this.GetValue(YAccProperty);
            }
            set
            {
                this.SetValue(YAccProperty, value);
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            int surfaceWidth = e.Info.Width;
            int surfaceHeight = e.Info.Height;
            SKCanvas canvas = e.Surface.Canvas;

            canvas.Clear();

            _scaling = e.Info.Width / (float)Width;

            float radius = Math.Min(surfaceWidth, surfaceHeight) / 2.0f;

            using (SKPaint paint = new SKPaint())
            {
                paint.Color = Color.Black.ToSKColor();
                paint.IsStroke = true;
                paint.StrokeWidth = ScaleFloat(CircleStrokeWidth);
                canvas.DrawCircle(surfaceWidth / 2.0f, surfaceHeight / 2.0f, radius, paint);
                canvas.DrawCircle(surfaceWidth / 2.0f, surfaceHeight / 2.0f,  2.0f * radius / 3.0f, paint);
                canvas.DrawCircle(surfaceWidth / 2.0f, surfaceHeight / 2.0f, radius / 3.0f, paint);
                canvas.DrawCircle(surfaceWidth / 2.0f, surfaceHeight / 2.0f, ScaleFloat(CircleStrokeWidth / 2f), paint);

                paint.StrokeWidth = ScaleFloat(TextStrokeWidth);
                paint.IsStroke = false;
                paint.TextSize = ScaleFloat(20);

                var textWidth = paint.MeasureText(OuterText);
                var textX = surfaceWidth / 2 - textWidth / 2;
                var textY = surfaceHeight / 2 - radius + ScaleFloat(20);
                canvas.DrawText(OuterText, textX, textY, paint);

                textWidth = paint.MeasureText(CenterText);
                textX = surfaceWidth / 2 - textWidth / 2;
                textY = textY + radius / 3.0f;
                canvas.DrawText(CenterText, textX, textY, paint);

                textWidth = paint.MeasureText(InnerText);
                textX = surfaceWidth / 2 - textWidth / 2;
                textY = textY + radius / 3.0f;
                canvas.DrawText(InnerText, textX, textY, paint);

                paint.Color = Color.Red.ToSKColor();
                paint.StrokeWidth = ScaleFloat(CircleStrokeWidth);
                float gScale = 2.0f * radius / 3.0f;
                float xPos = XAcc * gScale + surfaceWidth / 2f;
                float yPos = YAcc * gScale * -1 + surfaceHeight / 2f;
                canvas.DrawCircle(xPos, yPos, ScaleFloat(MarkerRadius), paint);
            }
        }

        private float ScaleFloat(float value)
        {
            return value * _scaling;
        }
    }
}
