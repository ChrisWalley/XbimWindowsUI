using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using Xbim.Common;
using Xbim.Common.Geometry;
using Xbim.Presentation;

namespace XbimXplorer
{
    public class MyDrawingControl3D : DrawingControl3D
    {
        public void HighlightS(IPersistEntity ent)
        {
            HighlighSelected(ent);
        }

        private XbimRect3D _viewBounds
        {
            get
            {
                return ModelPositions.ViewSpaceBounds;
            }
        }

        public void Zoom()
        {
            if (SelectedEntity == null || Highlighted.Content == null || Highlighted.Content.Bounds.IsEmpty)
                return;
            var r3D = Highlighted.Content.Bounds;
            MyZoomTo(r3D);
        }

        private void MyZoomTo(Rect3D r3D)
        {
            if (r3D.IsEmpty)
                return;
            var bounds = new Rect3D(
                _viewBounds.X, _viewBounds.Y, _viewBounds.Z,
                _viewBounds.SizeX, _viewBounds.SizeY, _viewBounds.SizeZ
                );

            if (r3D.IsEmpty)
                return;
            var actualZoomBounds = r3D.Contains(bounds) ? bounds : r3D;
            Viewport.ZoomExtents(actualZoomBounds, 500);
        }
    }
}
