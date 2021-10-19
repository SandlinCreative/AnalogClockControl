using System;
using System.Collections;
using System.Drawing;
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

        private ArrayList shapeParts = new ArrayList();
        private ArrayList textBlockParts = new ArrayList();
        private Line        hourHand;
        private Line        minuteHand;
        private Line        secondHand;
        private Ellipse     border;
        private Ellipse     centerRing;
        private TextBlock   digital;
        private TextBlock   digital2;
        private Grid        ticks;

        private ContextMenu cm = new ContextMenu();


        public AnalogClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));

            this.ContextMenu = cm;
            var mi = new MenuItem();
            var mi2 = new MenuItem();
            var mi3 = new MenuItem();
            cm.Items.Add(mi2);
            cm.Items.Add(mi3);
            cm.Items.Add(mi);
            mi.Header = "Close";
            mi.Icon = MakeIcon("pack://application:,,,/Resources/ChromeClose_16x.png", 16);
            mi.Click += (s, e) => Application.Current.Shutdown();
            mi2.Header = "Dark Mode";
            mi3.Header = "Light Mode";
            mi2.Click += (s, e) => SetThemeColor(Colors.Black);
            mi3.Click += (s, e) => SetThemeColor(Colors.White);
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
            centerRing = Template.FindName("PART_Center2", this) as Ellipse;

            shapeParts.Add(hourHand);
            shapeParts.Add(minuteHand);
            shapeParts.Add(secondHand);
            shapeParts.Add(border);
            shapeParts.Add(centerRing);
            textBlockParts.Add(digital);
            textBlockParts.Add(digital2);

            dispTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispTimer.Tick += new EventHandler(UpdateClocks);
            dispTimer.IsEnabled = true;

            SetThemeColor(Colors.White);

            base.OnApplyTemplate();
        }

        private void SetThemeColor()
        {
            var rnd = new Random();
            var bytes = new byte[3];
            rnd.NextBytes(bytes);
            var rndColor = Color.FromRgb(bytes[0], bytes[1], bytes[2]);
            SetThemeColor(rndColor);
        }

        private void SetThemeColor(Color color)
        {
            var brush = new SolidColorBrush(color);

            foreach (Shape item in shapeParts)
                item.Stroke = brush;

            foreach (Line item in ticks.Children)
                item.Stroke = brush;

            foreach (TextBlock item in textBlockParts)
                item.Foreground = brush;

        }

        private void UpdateClocks(object sender, EventArgs e)
        {
            hourHand.RenderTransform = new RotateTransform((DateTime.Now.Hour / 12.0) * 360, 0.5, 0.5);
            minuteHand.RenderTransform = new RotateTransform((DateTime.Now.Minute / 60.0) * 360, 0.5, 0.5);
            secondHand.RenderTransform = new RotateTransform((DateTime.Now.Second / 60.0) * 360, 0.5, 0.5);

            digital.Text = DateTime.Now.ToString("hh:mm:ss");
            digital2.Text = DateTime.Now.ToString("tt");

            //SetThemeColor();
        }

        private Image MakeIcon(string path, int pixelWidth)
        {
            System.Windows.Media.Imaging.BitmapImage srcBmp = new System.Windows.Media.Imaging.BitmapImage();
            srcBmp.BeginInit();
            srcBmp.UriSource = new Uri(path);
            srcBmp.DecodePixelWidth = pixelWidth;
            srcBmp.EndInit();
            Image icon = new Image();
            icon.Source = srcBmp;
            return icon;
        }
    }
}