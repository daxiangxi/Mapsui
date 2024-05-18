﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapsui.Layers;
using Mapsui.Nts;
using Mapsui.Samples.Common;
using Mapsui.Styles;
using NetTopologySuite.Geometries;

namespace Mapsui.Tests.Common.Maps;

public class BitmapSymbolInCollectionSample : ISample
{
    public string Name => "Collection with Bitmap Symbol";
    public string Category => "Tests";

    public Task<Map> CreateMapAsync() => Task.FromResult(CreateMap());


    public static Map CreateMap()
    {
        var layer = new MemoryLayer
        {
            Style = null,
            Features = CreateFeatures(),
            Name = "Points with bitmaps"
        };

        var map = new Map
        {
            BackColor = Color.WhiteSmoke,
        };

        map.Navigator.ZoomToBox(layer.Extent!.Grow(layer.Extent.Width * 2));

        map.Layers.Add(layer);

        return map;
    }

    public static IEnumerable<IFeature> CreateFeatures()
    {
        var circleImagePath = new Uri("embeddedresource://Mapsui.Samples.Common.Images.circle.png");
        var checkeredIconImagePath = new Uri("embeddedresource://Mapsui.Samples.Common.Images.checkered.png");

        // This test was created the easy way, by copying BitmapSymbol and the GeometryCollection. A test 
        // written specifically for GeometryCollection would probably look different.

        return
        [
            new GeometryFeature
            {
                Geometry = new GeometryCollection([new Point(50, 50)]),
                Styles = [new VectorStyle { Fill = new Brush(Color.Red) }]
            },
            new GeometryFeature
            {
                Geometry = new GeometryCollection([new Point(50, 100)]),
                Styles = [new SymbolStyle { BitmapPath = circleImagePath }]
            },
            new GeometryFeature
            {
                Geometry = new GeometryCollection([new Point(100, 50)]),
                Styles = [new SymbolStyle { BitmapPath = checkeredIconImagePath }]
            },
            new GeometryFeature
            {
                Geometry = new GeometryCollection([new Point(100, 100)]),
                Styles = [new VectorStyle { Fill = new Brush(Color.Green) }]
            }
        ];
    }
}
