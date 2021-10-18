using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalogClockControl.CustomControls
{
    public class AnalogClock : Control
    {
        private readonly DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Send);

        private Line hourHand;
        private Line minuteHand;
        private Line secondHand;
        private TextBlock digital;
        private TextBlock digital2;
        private Grid ticks;
        private Ellipse border;


        static AnalogClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));
        }

        public override void OnApplyTemplate()
        {
            hourHand = Template.FindName("PART_HourHand", this) as Line;
            minuteHand = Template.FindName("PART_MinuteHand", this) as Line;
            secondHand = Template.FindName("PART_SecondHand", this) as Line;
            digital = Template.FindName("PART_Digital", this) as TextBlock;
            digital2 = Template.FindName("PART_Digital2", this) as TextBlock;
            ticks = Template.FindName("PART_ClockTicks", this) as Grid;
            border = Template.FindName("PART_Border", this) as Ellipse;

            dispTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispTimer.Tick += new EventHandler(UpdateClocks);
            dispTimer.IsEnabled = true;

            SetWhiteTheme();

            base.OnApplyTemplate();
        }

        private void SetWhiteTheme()
        {
            var brush = new SolidColorBrush(Color.FromRgb(255,255,255));
            var kids = ticks.Children;
            foreach (Line item in kids)
            {
                item.Stroke = brush;
            }
            border.Stroke = brush;
            hourHand.Stroke = brush;
            minuteHand.Stroke = brush;
            digital.Foreground = brush;
            digital2.Foreground = brush;
        }
        private void SetBlackTheme()
        {

        }

        private void UpdateClocks(object sender, EventArgs e)
        {
            hourHand.RenderTransform = new RotateTransform((DateTime.Now.Hour / 12.0) * 360, 0.5, 0.5);
            minuteHand.RenderTransform = new RotateTransform((DateTime.Now.Minute / 60.0) * 360, 0.5, 0.5);
            secondHand.RenderTransform = new RotateTransform((DateTime.Now.Second / 60.0) * 360, 0.5, 0.5);

            digital.Text = DateTime.Now.ToString("hh:mm:ss");
            digital2.Text = DateTime.Now.ToString("tt");
        }
    }
}