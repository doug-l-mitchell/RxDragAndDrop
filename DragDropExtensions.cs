using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DRHorton.TimeKeeping.WPF.Extensions
{
    public static class DragDropExtensions
    {
        public static IObservable<EventPattern<MouseEventArgs>> ObserveMouseLeftButtonDown(this UIElement element)
        {
            return Observable.FromEventPattern<MouseEventArgs>(element, "PreviewMouseLeftButtonDown");
        }
        public static IObservable<EventPattern<MouseEventArgs>> ObserveMouseLeftButtonUp(this UIElement element)
        {
            return Observable.FromEventPattern<MouseEventArgs>(element, "MouseLeftButtonUp");
        }
        public static IObservable<EventPattern<MouseEventArgs>> ObserveMouseMove(this UIElement element)
        {
            return Observable.FromEventPattern<MouseEventArgs>(element, "MouseMove");
        }
        public static IObservable<EventPattern<MouseEventArgs>> ObserveMouseLeave(this UIElement element)
        {
            return Observable.FromEventPattern<MouseEventArgs>(element, "MouseLeave");
        }
        private static bool MinimumDragSeen(Point start, Point end)
        {
            return Math.Abs(end.X - start.X) >= SystemParameters.MinimumHorizontalDragDistance
                || Math.Abs(end.Y - start.Y) >= SystemParameters.MinimumVerticalDragDistance;
        }

        public static IObservable<EventPattern<MouseEventArgs>> ObserveBeginMouseDrag(this UIElement element)
        {
            var mouseDown = element.ObserveMouseLeftButtonDown().Select(ep => ep.EventArgs.GetPosition(element));
            var mouseMove = element.ObserveMouseMove();
            
            var stop = Observable.Merge(
                element.ObserveMouseLeftButtonUp(),
                element.ObserveMouseLeave().Where(ep => ep.EventArgs.LeftButton == MouseButtonState.Pressed)
            );

            return mouseDown.SelectMany(
                md => mouseMove
                        .Where(ep => ep.EventArgs.LeftButton == MouseButtonState.Pressed)
                        .Where(ep => MinimumDragSeen(md, ep.EventArgs.GetPosition(element)))
                        .TakeUntil(stop)
                );

        }

        public static IObservable<EventPattern<DragEventArgs>> ObservePreviewDragEnter(this UIElement element)
        {
            return Observable.FromEventPattern<DragEventArgs>(element, "PreviewDragEnter");
        }
        public static IObservable<EventPattern<DragEventArgs>> ObservePreviewDragOver(this UIElement element)
        {
            return Observable.FromEventPattern<DragEventArgs>(element, "PreviewDragOver");
        }
        public static IObservable<EventPattern<DragEventArgs>> ObservePreviewDrop(this UIElement element)
        {
            return Observable.FromEventPattern<DragEventArgs>(element, "PreviewDrop");
        }
        public static IObservable<EventPattern<DragEventArgs>> ObservePreviewDragLeave(this UIElement element)
        {
            return Observable.FromEventPattern<DragEventArgs>(element, "PreviewDragLeave");
        }
    }
}
