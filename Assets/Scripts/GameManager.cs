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
    private bool _isPlayerTurn = false;

    #endregion

    #region プロパティ  

    public bool IsTurn 
    {
        get => _isTurn; 
    }

    public bool IsPlayerTurn
    {
        get => _isPlayerTurn;
    }

    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary> 
    private void Awake()
    {
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _myScript = _putStone.GetComponent<MyScript2>();
        int preceding = Random.Range(0, 2);
        if (preceding == 0)
        {
            _isPlayerTurn = false;
        }
        else if (preceding == 1)
        {
            _isPlayerTurn = true;
        } 
    }

    public void aa()
    {
        _isTurn = !_isTurn;
        _isPlayerTurn = !_isPlayerTurn;
        _seachPutPossible.Seach();
        if (!IsTurn)
        {
            _myScript.WhitePutStone();
        }
        else
        {
            _myScript.BlackPutStone();
        }

        if(_isPlayerTurn)
        {

        }
        else
        {

        }
    }

    public void nnnn(string s)
    {
        foreach(Transform chlid in _putStone.transform)
        {
            chlid.gameObject.SetActive(false);
        }
    }

    #endregion

}
