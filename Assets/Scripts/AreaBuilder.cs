using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

public class AreaBuilder : MonoBehaviour {

    public List<GameObject> ItemList;
    public Transform Background;
    public static List<GameObject> Board;
    public Transform ItemsParent; 
    private AreaCheck _areaCheck;
    public static bool _touch;
    
    void Awake()
    {
        AreaConfig.Width = 8;
        AreaConfig.Height = 8;
    }
    void Start()
    {
      _areaCheck = new AreaCheck();
      CreateBoard();
      RepositionItems(true); 
    }
    void Update()
    {
      if (Input.GetMouseButtonUp(0) && _touch)
      {
        RaycastHit2D hitInfo = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hitInfo)
        {
          int _index;
          _index = Board.IndexOf(hitInfo.transform.gameObject);
          Destroy(hitInfo.transform.gameObject);
          Board[_index] = (GameObject)Instantiate(ItemList[Random.Range(0, 6)], Vector3.zero, Quaternion.identity);
          Board[_index].transform.parent = ItemsParent;
          if (_areaCheck.CheckBoard(Board, false))
          {
            CreateBoard();
            RepositionItems(true);
          }
          RepositionItems(false);
         }
       }
    }  
    void CreateBoard()
    {
      if (Board == null)
      {
        Board = new List<GameObject>();
        Board.Add((GameObject)Instantiate(ItemList[Random.Range(0, 6)], Vector3.zero, Quaternion.identity));
        Board.Add((GameObject)Instantiate(ItemList[Random.Range(0, 6)], Vector3.zero, Quaternion.identity));
        Board[0].transform.parent = ItemsParent;
        Board[1].transform.parent = ItemsParent;
        Board[0].transform.localPosition = new Vector3(-0.5f, 3.5f, 0);
        Board[1].transform.localPosition = new Vector3(-0.5f, 3.5f, 0); 
      }
      while (Board.Count < AreaConfig.Width * AreaConfig.Height)
      { 
        Board.Add(ItemList[Random.Range(0, 6)]);
        if (Board.Count > 2 && !_areaCheck.CheckBoard(Board, true))
        {
         Board[Board.Count - 1] = (GameObject)Instantiate(Board[Board.Count - 1], Vector3.zero, Quaternion.identity);
         Board[Board.Count - 1].transform.parent = ItemsParent;
         Board[Board.Count - 1].transform.localPosition = new Vector3(-0.5f, 3.5f, 0);
        }
        else
        {
          Board.RemoveAt(Board.Count - 1);
        }  
      }
    }
    public void RepositionItems(bool animation)
    {
        Vector3 _pos = new Vector3(2.8f, -2.8f, 0);
        int _maxWidth = AreaConfig.Width;       
        for (int i = 0; i < Board.Count; i++)
        {
          if (animation)
          {
            _touch = false;
            StartCoroutine(MoveToPosition(Board[i], _pos, 2));
          }
          else
            Board[i].transform.localPosition = _pos;
          _pos.x -= 0.8f;
          if (i == _maxWidth - 1)
          {
            _pos.x = 2.8f;
            _pos.y += 0.8f;
            _maxWidth += AreaConfig.Width;
          }
        }
    }
    IEnumerator MoveToPosition(GameObject _item, Vector3 newPosition, float time)
    {
      float elapsedTime  = 0;
      Vector3 startingPos = _item.transform.localPosition;
      while (elapsedTime < time)
      {
        _item.transform.position = Vector3.Lerp(startingPos, newPosition, (elapsedTime / time));
        elapsedTime += Time.deltaTime;
        yield return new WaitForEndOfFrame();
      }
      _touch = true;
    }     
}
