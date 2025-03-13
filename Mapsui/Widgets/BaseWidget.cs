﻿using System;
using System.Runtime.CompilerServices;

namespace Mapsui.Widgets;

public abstract class BaseWidget : IWidget
{
    private HorizontalAlignment _horizontalAlignment = HorizontalAlignment.Right;

    /// <summary>
    /// Type of area to use for touch events. The default is WidgetArea. This needs to be set to 
    /// 'Map' in the constructor if widget want to receive manipulation events from all over the map.
    /// </summary>
    public InputAreaType InputAreaType { get; init; } = InputAreaType.Widget;

    /// <summary>
    /// Horizontal alignment of Widget
    /// </summary>
    public HorizontalAlignment HorizontalAlignment
    {
        get => _horizontalAlignment;
        set
        {
            if (_horizontalAlignment == value)
                return;
            _horizontalAlignment = value;
            Invalidate();
        }
    }

    private VerticalAlignment _verticalAlignment { get; set; } = VerticalAlignment.Bottom;

    /// <summary>
    /// Vertical alignment of Widget
    /// </summary>
    public VerticalAlignment VerticalAlignment
    {
        get => _verticalAlignment;
        set
        {
            if (_verticalAlignment == value)
                return;
            _verticalAlignment = value;
            Invalidate();
        }
    }

    private MRect _margin = new MRect(2);

    /// <summary>
    /// Margin outside of the widget
    /// </summary>
    public MRect Margin
    {
        get => _margin;
        set
        {
            if (_margin == value)
                return;
            _margin = value;
            Invalidate();
        }
    }

    private MPoint _position = new MPoint(0, 0);

    /// <summary>
    /// Position for absolute alignment
    /// </summary>
    public MPoint Position
    {
        get => _position;
        set
        {
            if (_position.Equals(value))
                return;
            _position = value;
            Invalidate();
        }
    }

    private double _width;

    /// <summary>
    /// Width of Widget
    /// </summary>
    public double Width
    {
        get => _width;
        set
        {
            if (_width == value)
                return;
            _width = value;
            Invalidate();
        }
    }

    private double _height;

    /// <summary>
    /// Height of Widget
    /// </summary>
    public double Height
    {
        get => _height;
        set
        {
            if (_height == value)
                return;
            _height = value;
            Invalidate();
        }
    }

    private MRect? _envelope;

    /// <summary>
    /// Envelope of Widget
    /// </summary>
    public MRect? Envelope
    {
        get => _envelope;
        set
        {
            if (_envelope == value)
                return;
            _envelope = value;
            Invalidate();
        }
    }

    private bool _enabled = true;

    /// <summary>
    /// Is Widget visible on screen
    /// </summary>
    public bool Enabled
    {
        get => _enabled;
        set
        {
            if (_enabled == value)
                return;
            _enabled = value;
            Invalidate();
        }
    }

    /// <summary>
    /// Flag for redrawing widget in the next drawing cycle
    /// </summary>
    public bool NeedsRedraw { get; set; } = false;

    public bool InputTransparent { get; init; }

    /// <summary>
    /// Event which is called if widget is tapped.
    /// </summary>
    public Func<IWidget, WidgetEventArgs, bool> Tapped { get; set; } = (s, e) => false;

    /// <summary>
    /// Event which is called if widget is pressed.
    /// </summary>
    public Func<IWidget, WidgetEventArgs, bool> PointerPressed { get; set; } = (s, e) => false;

    /// <summary>
    /// Event which is called if widget is moved.
    /// </summary>
    public Func<IWidget, WidgetEventArgs, bool> PointerMoved { get; set; } = (s, e) => false;

    /// <summary>
    /// Event which is called if widget is released.
    /// </summary>
    public Func<IWidget, WidgetEventArgs, bool> PointerReleased { get; set; } = (s, e) => false;



    public void UpdateEnvelope(double maxWidth, double maxHeight, double screenWidth, double screenHeight)
    {
        var minX = CalculatePositionX(0, screenWidth, maxWidth);
        var minY = CalculatePositionY(0, screenHeight, maxHeight);
        var maxX = HorizontalAlignment == HorizontalAlignment.Stretch ? screenWidth - Margin.Right : minX + maxWidth;
        var maxY = VerticalAlignment == VerticalAlignment.Stretch ? screenHeight - Margin.Bottom : minY + maxHeight;

        Envelope = new MRect(minX, minY, maxX, maxY);
    }

    public virtual void Invalidate([CallerMemberName] string name = "")
    {
        NeedsRedraw = true;
    }

    /// <inheritdoc/>
    public virtual bool OnTapped(WidgetEventArgs e)
    {
        return Tapped(this, e);
    }

    /// <inheritdoc/>
    public virtual bool OnPointerPressed(WidgetEventArgs e)
    {
        return PointerPressed(this, e);
    }

    /// <inheritdoc/>
    public virtual bool OnPointerMoved(WidgetEventArgs e)
    {
        return PointerMoved(this, e);
    }

    /// <inheritdoc/>
    public virtual bool OnPointerReleased(WidgetEventArgs e)
    {
        return PointerReleased(this, e);
    }

    private double CalculatePositionX(double left, double right, double width) => HorizontalAlignment switch
    {
        HorizontalAlignment.Left => Margin.Left,
        HorizontalAlignment.Center => (right - left - width) / 2f,
        HorizontalAlignment.Right => right - left - width - Margin.Right,
        HorizontalAlignment.Stretch => Margin.Left,
        HorizontalAlignment.Absolute => Position.X,
        _ => throw new ArgumentException("Unknown horizontal alignment: " + HorizontalAlignment)
    };

    private double CalculatePositionY(double top, double bottom, double height) => VerticalAlignment switch
    {
        VerticalAlignment.Top => Margin.Top,
        VerticalAlignment.Bottom => bottom - top - height - Margin.Bottom,
        VerticalAlignment.Center => (bottom - top - height) / 2f,
        VerticalAlignment.Stretch => Margin.Top,
        VerticalAlignment.Absolute => Position.Y,
        _ => throw new ArgumentException("Unknown vertical alignment: " + VerticalAlignment)
    };
}
