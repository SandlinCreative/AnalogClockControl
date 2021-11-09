using System;
using System.Collections;
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
        private Line hourHand;
        private Line minuteHand;
        private Line secondHand;
        private Ellipse border;
        private Ellipse centerRing;
        private TextBlock digital;
        private TextBlock digital2;
        private TextBlock timezoneTxtBlk;
        private Grid ticks;
        private TimeZoneInfo Timezone;

        private MenuItem tzMenu = new MenuItem();

        static AnalogClock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));
        }

        public AnalogClock()
        {
            Timezone = TimeZoneInfo.Local;
            ContextMenu = new ContextMenu();
            var mi0 = new MenuItem();
            var mi = new MenuItem();
            var mi2 = new MenuItem();
            var mi3 = new MenuItem();
            ContextMenu.Items.Add(mi0);
            LoadTimezoneMenu();
            ContextMenu.Items.Add(mi2);
            ContextMenu.Items.Add(mi3);
            ContextMenu.Items.Add(mi);
            mi0.Header = "New Clock";
            mi.Header = "Close";
            //mi.Icon = MakeIcon("pack://application:,,,/Resources/ChromeClose_16x.png", 16);
            mi2.Header = "Dark Mode";
            mi3.Header = "Light Mode";
            mi0.Click += (s, e) => { Window newClock = new ClockWindow(); newClock.Show(); };
            mi.Click += (s, e) => Window.GetWindow(this).Close();
            mi2.Click += (s, e) => SetThemeColor(Colors.Black);
            mi3.Click += (s, e) => SetThemeColor(Colors.White);

            //mi3.Icon = MakeIcon("pack://application:,,,/Resources/Checkmark_12x_16x.png", 16);
        }

        private void LoadTimezoneMenu()
        {
            tzMenu.Header = "Timezone";
            MenuItem temp;
            foreach (TimeZoneInfo zone in TimeZoneInfo.GetSystemTimeZones())
            {
                temp = new MenuItem();
                temp.Header = zone;
                temp.Timezone = zone;
                temp.Click += SetTimezone;
                tzMenu.Items.Add(temp);
            }
            ContextMenu.Items.Add(tzMenu);
        }

        private void SetTimezone(object sender, RoutedEventArgs e)
        {
            TimeZoneInfo tzi = (sender as MenuItem).Timezone;
            Timezone = tzi;
            timezoneTxtBlk.Text = tzi.Id;
        }

        public override void OnApplyTemplate()
        {
            hourHand = Template.FindName("PART_HourHand", this) as Line;
            minuteHand = Template.FindName("PART_MinuteHand", this) as Line;
            secondHand = Template.FindName("PART_SecondHand", this) as Line;
            digital = Template.FindName("PART_Digital", this) as TextBlock;
            digital2 = Template.FindName("PART_Digital2", this) as TextBlock;
            timezoneTxtBlk = Template.FindName("PART_Timezone", this) as TextBlock;
            ticks = Template.FindName("PART_ClockTicks", this) as Grid;
            border = Template.FindName("PART_Border", this) as Ellipse;
            centerRing = Template.FindName("PART_Center2", this) as Ellipse;

            shapeParts.Add(hourHand);
            shapeParts.Add(minuteHand);
            //shapeParts.Add(secondHand);
            shapeParts.Add(border);
            shapeParts.Add(centerRing);
            textBlockParts.Add(digital);
            textBlockParts.Add(digital2);
            textBlockParts.Add(timezoneTxtBlk);

            dispTimer.Interval = TimeSpan.FromMilliseconds(1000);
            dispTimer.Tick += new EventHandler(UpdateClocks);
            dispTimer.IsEnabled = true;

            timezoneTxtBlk.Text = Timezone.Id;

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

        private DateTime Now() => TimeZoneInfo.ConvertTime(DateTime.Now, Timezone);

        private void UpdateClocks(object sender, EventArgs e)
        {
            hourHand.RenderTransform = new RotateTransform(((Now().Hour) / 12.0) * 360, 0.5, 0.5);
            minuteHand.RenderTransform = new RotateTransform((Now().Minute / 60.0) * 360, 0.5, 0.5);
            secondHand.RenderTransform = new RotateTransform((Now().Second / 60.0) * 360, 0.5, 0.5);

            digital.Text = Now().ToString("hh:mm:ss");
            digital2.Text = Now().ToString("tt");

            //SetThemeColor();
        }

        private System.Windows.Media.Imaging.BitmapImage srcBmp = new System.Windows.Media.Imaging.BitmapImage();
        private Image icon = new Image();

        private Image MakeIcon(string path, int pixelWidth)
        {
            srcBmp.BeginInit();
            srcBmp.UriSource = new Uri(path);
            srcBmp.DecodePixelWidth = pixelWidth;
            srcBmp.EndInit();
            icon.Source = srcBmp;

            return icon;
        }
    }

    public partial class MenuItem : System.Windows.Controls.MenuItem
    {
        public TimeZoneInfo Timezone;
    }
}