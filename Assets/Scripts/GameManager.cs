// ---------------------------------------------------------  
// MyScript.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region 変数  

    [SerializeField]
    private GameObject _surfacePlate = default;

    [SerializeField]
    private GameObject _putStone = default;

    private SeachPutPossible _seachPutPossible = default;
    private MyScript2 _myScript = default;

    private bool _isTurn = false;

    #endregion

    #region プロパティ  

    public bool IsTurn 
    {
        get => _isTurn; 
    }

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _myScript = _putStone.GetComponent<MyScript2>();
    }

    public void aa()
    {
        _isTurn = !_isTurn;
        _seachPutPossible.Seach();
        if (!IsTurn)
        {
            _myScript.WhitePutStone();
        }
        else
        {
            _myScript.BlackPutStone();
        }
    }

    public void nnnn(string s)
    {
        Debug.Log(s);
    }

    #endregion

}
