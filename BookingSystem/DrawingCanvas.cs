using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;

namespace Drawing
{
    public class DrawingCanvas : Canvas //  Canvas,  Background и другие свойства
    {
        private readonly List<ShapeVisual> visuals = new List<ShapeVisual>();

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= visuals.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
            return visuals[index].Visual;
        }

        protected override int VisualChildrenCount => visuals.Count;

        public void AddVisual(DrawingVisual visual, ShapeType type, string label)
        {
            if (visual == null) return;
            visuals.Add(new ShapeVisual(visual, type, label));
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        public void DeleteVisual(DrawingVisual visual)
        {
            if (visual == null) return;
            var shapeVisual = visuals.FirstOrDefault(s => s.Visual == visual);
            if (shapeVisual != null)
            {
                visuals.Remove(shapeVisual);
                RemoveVisualChild(visual);
                RemoveLogicalChild(visual);
            }
        }

        public ShapeVisual GetVisual(Point point)
        {
            HitTestResult hitResult = VisualTreeHelper.HitTest(this, point);
            if (hitResult != null)
            {
                var drawingVisual = hitResult.VisualHit as DrawingVisual;
                if (drawingVisual != null)
                {
                    return visuals.FirstOrDefault(v => v.Visual == drawingVisual);
                }
            }
            return null;
        }

        private readonly List<DrawingVisual> hits = new List<DrawingVisual>();

        public List<DrawingVisual> GetVisuals(Geometry region)
        {
            hits.Clear();
            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);
            HitTestResultCallback callback = HitTestCallback;
            VisualTreeHelper.HitTest(this, null, callback, parameters);
            return hits;
        }

        private HitTestResultBehavior HitTestCallback(HitTestResult result)
        {
            if (result is GeometryHitTestResult geometryResult)
            {
                DrawingVisual drawingVisual = result.VisualHit as DrawingVisual;
                if (drawingVisual != null && geometryResult.IntersectionDetail == IntersectionDetail.FullyInside)
                {
                    ShapeVisual shapeVisual = visuals.FirstOrDefault(v => v.Visual == drawingVisual);
                    if (shapeVisual != null)
                    {
                        hits.Add(shapeVisual.Visual);
                    }
                }
            }
            return HitTestResultBehavior.Continue;
        }

        // Метод для очистки визуализаций
        public void Clear()
        {
            foreach (var visual in visuals.ToList())
            {
                DeleteVisual(visual.Visual);
            }
        }
    }
}