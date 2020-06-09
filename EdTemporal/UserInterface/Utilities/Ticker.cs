using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace EdTemporal.UserInterface.Utilities {
    // Source: http://www.jarloo.com/rumormill4/
    // This source code was copied from above as I could not
    // come up with or find a solution which lead me to this.
    //
    public class Ticker<T> where T : FrameworkElement {
        private readonly DispatcherTimer displayTimer = new DispatcherTimer();
        
        public EventHandler<ItemEventArgs<T>> ItemDisplayed;

        public bool IsRunning { get; set; }

        public void Stop() {
            displayTimer.Stop();
            IsRunning = false;
        }

        public void Start() {
            displayTimer.Start();
            displayTimer.Interval = new TimeSpan(0, 0, 0, 1);
            IsRunning = true;
        }

        public Ticker(Panel container) {
            SeperatorSize = 25;

            Container = container;

            Container.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            Container.Arrange(new Rect(Container.DesiredSize));

            Speed = new TimeSpan(0, 0, 0, 40);
            Items = new Queue<T>();

            displayTimer.Tick += displayTimer_Tick;
            displayTimer.Start();
            IsRunning = true;
        }

        public double SeperatorSize { get; set; }

        public TimeSpan Speed { get; set; }

        public Queue<T> Items { get; set; }
        public Panel Container { get; private set; }

        private void displayTimer_Tick(object sender, EventArgs e) {
            DisplayNextItem();
        }

        private void DisplayNextItem() {
            if (Items.Count == 0) return;

            T item = Items.Dequeue();

            Container.Children.Add(item);
            AnimateMove(item);

            if (ItemDisplayed != null) ItemDisplayed(this, new ItemEventArgs<T>(item));
        }

        private void AnimateMove(FrameworkElement e) {
            e.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            e.Arrange(new Rect(e.DesiredSize));

            double from = Container.ActualWidth;
            double to = -500;

            int unitsPerSec = Convert.ToInt32(Math.Abs(from - to) / Speed.TotalSeconds);
            int nextFire = Convert.ToInt32((e.ActualWidth + SeperatorSize) / unitsPerSec);

            displayTimer.Stop();
            displayTimer.Interval = new TimeSpan(0, 0, nextFire);
            displayTimer.Start();

            TaggedDoubleAnimation ani = new TaggedDoubleAnimation();
            ani.From = from;
            ani.To = to;
            ani.Duration = new Duration(Speed);
            ani.TargetElement = e;
            ani.Completed += ani_Completed;

            TranslateTransform trans = new TranslateTransform();
            e.RenderTransform = trans;

            trans.BeginAnimation(TranslateTransform.XProperty, ani, HandoffBehavior.Compose);
        }

        private void ani_Completed(object sender, EventArgs e) {
            Clock clock = (Clock)sender;
            TaggedDoubleAnimation ani = (TaggedDoubleAnimation)clock.Timeline;

            FrameworkElement element = ani.TargetElement;
            Container.Children.Remove(element);
        }
    }

    public class ItemEventArgs<T> : EventArgs {
        public ItemEventArgs(T item) {
            Item = item;
        }

        public T Item { get; private set; }
    }

    public class TaggedDoubleAnimation : DoubleAnimation {
        public FrameworkElement TargetElement { get; set; }

        protected override Freezable CreateInstanceCore() {
            return new TaggedDoubleAnimation { TargetElement = TargetElement };
        }
    }
}
