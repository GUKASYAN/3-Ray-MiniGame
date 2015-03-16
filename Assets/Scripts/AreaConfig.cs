using UnityEngine;
using System.Collections;

public class AreaConfig : MonoBehaviour {

    private static int _width;
    private static int _height;
    public static int Width
    {
      get {return _width; }
      set
      {
        if (value > 0 && value <= 8)
          _width = value;
        else
        {
                _width = 8;
        }
      }
    }
    public static int Height
    {
      get { return _height; }
      set
      {
        if (value > 0 && value <= 8)
          _height = value;
        else
        {
        _height = 8;
        }
      }  
    }   
}
