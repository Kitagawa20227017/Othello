// ---------------------------------------------------------  
// MyScript.cs  
// 
//
//
// 作成日: 
// 作成者: 
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class MyScript : MonoBehaviour
{

    #region 変数  

    [SerializeField]
    private GameObject _surfacePlate = default;

    [SerializeField]
    private GameObject _gameManager = default;

    [SerializeField]
    private GameObject _putStone = default;

    private GameManager _gameManagerScript = default;
    private SeachPutPossible _seachPutPossible = default;
    private MyScript2 _myScript = default;
    private bool _isWhite = false;
    private int[,] _copy = new int[8, 8];

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 初期化処理  
    /// </summary>  
    private void Awake()
    {
    }
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _myScript = _putStone.GetComponent<MyScript2>();
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        if (_gameManagerScript.IsPlayerTurn == false)
        {
            _isWhite = true;
        }
        else
        {
            _isWhite = false;
        }
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>
    private void Update()
    {
        if (_gameManagerScript.IsPlayerTurn == false)
        {
            for(int i = 0; i < _copy.GetLength(0); i++)
            {
                for(int j = 0; j < _copy.GetLength(1); j++)
                {
                    _copy[i, j] = _seachPutPossible.Map[i, j];
                }
            }
            RandomAI();
        }
    }

    public void RandomAI()
    {
        int putStoneConut = 0;
        if (!_isWhite)
        {
            for (int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
                {
                   
                }
            }
        }
        else
        {
            for (int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
                {
                   
                }
            }
        }

        for (int i = 0; i < _copy.GetLength(0); i++)
        {
            for (int j = 0; j < _copy.GetLength(0); j++)
            {
                if (_copy[i, j] == 1)
                {
                    putStoneConut++;
                }

                
            }
        }

    }

    #endregion

}
