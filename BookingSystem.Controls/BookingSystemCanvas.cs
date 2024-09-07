using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BookingSystem.Controls
{
    public class BookingSystemCanvas : Canvas
    {
        private readonly List<Visual> visuals = new();
        private readonly List<DrawingVisual> hits = new();

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        protected override int VisualChildrenCount => visuals.Count;

        public IReadOnlyList<Visual> Visuals => visuals;

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            AddVisualChild(visual); 
            AddLogicalChild(visual); 
        }

        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);
            RemoveVisualChild(visual); 
            RemoveLogicalChild(visual); 
        }

        public void ClearVisuals()
        {
            foreach (var visual in visuals.ToArray()) 
            {
                DeleteVisual(visual);
            }
        }

        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            return hitResult?.VisualHit as DrawingVisual;
        }

        public List<DrawingVisual> GetVisuals(Geometry region)
        {
            hits.Clear();
            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);
            HitTestResultCallback callback = new HitTestResultCallback(this.HitTestCallback);
            VisualTreeHelper.HitTest(this, null, callback, parameters);
            return hits;
        }

        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            GeometryHitTestResult geometryResult = (GeometryHitTestResult)result;
            if (result.VisualHit is DrawingVisual visual && geometryResult.IntersectionDetail != IntersectionDetail.Empty)
            {
                hits.Add(visual);
            }
            return HitTestResultBehavior.Continue;
        }
    }
}