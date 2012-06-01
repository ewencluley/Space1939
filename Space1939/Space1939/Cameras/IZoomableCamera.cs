using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Space1939.Cameras
{
    interface IZoomableCamera
    {
        void zoomCam(float x);

        float getZoom();

        float getZoomBounds(int x);
    }
}
