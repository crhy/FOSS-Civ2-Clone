using System.Numerics;
using Raylib_cs;

namespace RaylibUI.Controls;

public class TextBox : BaseControl
{
    public override bool CanFocus => true;
    
    private int _editPosition = 0;

    private string _text;
    private readonly IControlLayout _controller;
    private readonly int _minWidth;
    private readonly Action<string>? _acceptAction;
    private bool _editMode = false;
    private int _editWidth;

    private const int TextMargin = 5;

    public string Text => _text;

    public TextBox(IControlLayout controller, string initialValue, int minWidth, Action<string>? acceptAction = null) : base(controller)
    {
        _controller = controller;
        _minWidth = minWidth;
        _acceptAction = acceptAction;
        SetText(initialValue);
    }

    public void SetText(string initialValue)
    {
        _text = initialValue;
        _editPosition = _text.Length;
        var size = Raylib.MeasureTextEx(Raylib.GetFontDefault(), _text, Styles.BaseFontSize, 1.0f);
        _editWidth = (int)size.X;
        Height = (int)(size.Y + TextMargin * 2);
    }

    public override void Draw(bool pulse)
    {
        Raylib.DrawRectangle((int)Location.X, (int)Location.Y, Width, Height, Color.WHITE);
        Raylib.DrawRectangleLines((int)Location.X, (int)Location.Y, Width, Height, Color.BLACK);
        Raylib.DrawText(_text, (int)Location.X+ 5, (int)Location.Y + 5, Styles.BaseFontSize, Color.BLACK);
        
        if (_editMode)
        {
            if (pulse)
            {
                Raylib.DrawRectangleRec(new Rectangle(Location.X + 5 + _editWidth + 1, Location.Y + 5, 1, 20), Color.BLACK);
            }
        }

        base.Draw(pulse);
    }

    public override void OnClick()
    {
        if (_controller.Focused != this)
        {
            _controller.Focused = this;
        }
    }

    public override void OnFocus()
    {
        _editMode = true;
    }

    public override void OnBlur()
    {
        _editMode = false;
    }

    public override Size GetPreferredSize(int width, int height)
    {
        return new Size(_minWidth, Height);
    }

    public override bool OnKeyPressed(KeyboardKey key)
    {
        switch (key)
        {
            case KeyboardKey.KEY_LEFT:
            {
                if (_editPosition > 0)
                {
                    SetEditPosition(_editPosition - 1);
                }

                return true;
            }
            case KeyboardKey.KEY_RIGHT:
                if (_editPosition < _text.Length)
                {
                    SetEditPosition(_editPosition + 1);
                }

                return true;
            case KeyboardKey.KEY_BACKSPACE:
                if (_editPosition > 0)
                {
                    SetEditPosition(_editPosition - 1);
                    _text = _text.Remove(_editPosition, 1);
                }

                return true;
            case KeyboardKey.KEY_DELETE:
                if (_editPosition < _text.Length)
                {
                    _text = _text.Remove(_editPosition, 1);
                }

                return true;
            case KeyboardKey.KEY_TAB:
                return base.OnKeyPressed(key);
            case KeyboardKey.KEY_ENTER:
                if (_acceptAction != null)
                {
                    _acceptAction(_text);
                    return true;
                }

                break;
            default:
                var charPressed = Raylib.GetCharPressed();
                if (charPressed > 0)
                {
                    _text = _text.Insert(_editPosition, Convert.ToChar(charPressed).ToString());
                    SetEditPosition(_editPosition + 1);
                    return true;
                }

                break;
        }

        return base.OnKeyPressed(key);
    }

    private void SetEditPosition(int newEditPosition)
    {
        _editPosition = newEditPosition;
        _editWidth = Raylib.MeasureText(_text.Substring(0,_editPosition), Styles.BaseFontSize);
    }
}