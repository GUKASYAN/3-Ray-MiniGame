using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AreaCheck : MonoBehaviour {
    enum Check {NONE, START, END};
    public bool CheckBoard(List<GameObject> _sourceBoard, bool _start)
    {         
      int _maxWidth = AreaConfig.Width; 
      List<GameObject> _checkLine = new List<GameObject>();
      List<int> blackList = new List<int>();
      CheckLine(_sourceBoard, out blackList, _start);
      if (_start && blackList.Count > 0) 
        return true; 
      for (int i=0;i<blackList.Count;i++)
      {
        Destroy(_sourceBoard[blackList[i]]); 
      }
      for (int i = 0; i < blackList.Count; i++)
      {
        _sourceBoard.RemoveAt(blackList[i]-i); 
      }
      if (blackList.Count > 0) 
        return true;  
      return false;
    }    
    public void CheckLine(List<GameObject> sourceLine, out  List<int> blackList, bool start)
    { 
      Check check = Check.NONE;
      blackList = new List<int>(); 
      float _query;
      if (start) _query = (float)sourceLine.Count / (float)AreaConfig.Width + 0.01f;
      else _query = 1;
      int _begin,_end;
      // Check Horizontal
      _begin = 0;
      _end = sourceLine.Count-1;
      for (int j = 0; j < _query; j++, _begin+=AreaConfig.Width, _end+=AreaConfig.Width)
      {         
        for (int i = _begin; i <= _end-2 && i<sourceLine.Count - 2; i++)
        {
          if (sourceLine[i].name[1] == sourceLine[i + 1].name[1] && sourceLine[i + 1].name[1] == sourceLine[i + 2].name[1])
          { 
            if (start)
            {
              blackList.Add(i + 2);
              return;
            }
            check = Check.START;
            if (i == sourceLine.Count - 3)
            {
              blackList.Add(i);
              blackList.Add(i + 1);
              blackList.Add(i + 2);
              check = Check.NONE; 
              return;
            } 
          }  
          else if (check == Check.START) 
            check = Check.END;
          else
          {
            check = Check.NONE; 
          }
          if (check == Check.START)
          {
            blackList.Add(i); 
          }
          if (check == Check.END)
          { 
            blackList.Add(i);
            blackList.Add(i + 1);
            check = Check.NONE;
            return;//
          }
        }
      }  
        // Check Vertical
           int _count = 0;
           _begin = 0;
           _end = 0;
           if (sourceLine.Count > AreaConfig.Width * 2)
           {
             _end = sourceLine.Count;
             while (_end > 0)
             {
               _end -= AreaConfig.Width;
               _count++;
             }
             _end += AreaConfig.Width;
             int i;
             if (start) 
               i = _end - 1;
             else 
               i = 0;
             for (; i < _end; i++)
             {
               for (int j = 0; j < _count - 2; j++)
               {
                 if (
                    sourceLine[((j * AreaConfig.Width) + i)].name[1]
                    == sourceLine[(((j + 1) * AreaConfig.Width) + i)].name[1]
                    &&
                    sourceLine[(((j + 1) * AreaConfig.Width) + i)].name[1]
                    == sourceLine[(((j + 2) * AreaConfig.Width) + i)].name[1]
                    )
                 { 
                   if (!blackList.Contains(((j * AreaConfig.Width) + i))) 
                     blackList.Add(((j * AreaConfig.Width) + i));
                   if (!blackList.Contains((((j + 1) * AreaConfig.Width) + i))) 
                     blackList.Add((((j + 1) * AreaConfig.Width) + i));
                   if (!blackList.Contains((((j + 2) * AreaConfig.Width) + i))) 
                     blackList.Add((((j + 2) * AreaConfig.Width) + i));
                   return; 
                 }
               }    
             }
         }
    } 
}
