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

public class MiniMaxAI : MonoBehaviour
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
    private AITurnOver _myScript3 = default;
    private bool _isWhite = false;
    private int[,] _mapCopy = new int[8, 8];
    private int[,] _myPutStone = new int[8, 8];
    private int _myColer = 0;

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
        _myScript3 = this.gameObject.GetComponent<AITurnOver>();
        if (_gameManagerScript.IsPlayerTurn == false)
        {
            _isWhite = true;
            _myColer = 1;
        }
        else
        {
            _isWhite = false;
            _myColer = -1;
        }
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>
    private void Update()
    {
        if (_gameManagerScript.IsPlayerTurn == false)
        {
            for(int i = 0; i < _mapCopy.GetLength(0); i++)
            {
                for(int j = 0; j < _mapCopy.GetLength(1); j++)
                {
                    _mapCopy[i, j] = _seachPutPossible.Map[i, j];
                }
            }
            RandomAI();
        }
    }

    public void RandomAI()
    {
        int score = 0;
        int maxScore = -10000;
        int x = 0;
        int z = 0;
        int myPutConut = 0;
        bool flag = false;
        if (!_isWhite)
        {
            for (int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.Black[i, j];
                    myPutConut = _seachPutPossible.BlackCount;
                }
            }
        }
        else
        {
            for (int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.White[i, j];
                    myPutConut = _seachPutPossible.WhiteCount;
                }
            }
        }

        if(myPutConut == 0)
        {
            return;
        }

        for (int i = 0; i < _myPutStone.GetLength(0); i++)
        {
            for (int j = 0; j < _myPutStone.GetLength(0); j++)
            {
                if (_myPutStone[i, j] == 1)
                {
                    score = _myScript3.TurnOver(_mapCopy,i,j,_myColer, _myColer, 0);
                    flag = true;
                }

                if (flag && score > maxScore)
                {
                    maxScore = score;
                    x = i;
                    z = j;
                }
                flag = false;
            }
        }

        _myScript.aaa(x, z);
        return;

    }

    #endregion

}
