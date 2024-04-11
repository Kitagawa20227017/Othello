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

    #region メソッド  
     
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
            MiniMax();
        }
    }

    public void MiniMax()
    {
        int score = 0;
        int maxScore = -10000;
        int x = 0;
        int z = 0;
        int myPutConut = 0;
        bool flag = false;

        // 自分が黒のとき
        if (!_isWhite)
        {
            // 置ける場所をコピーする
            for (int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.Black[i, j];
                    myPutConut = _seachPutPossible.BlackCount;
                }
            }
        }
        // 自分が白のとき
        else
        {
            // 置ける場所をコピーする
            for (int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.White[i, j];
                    myPutConut = _seachPutPossible.WhiteCount;
                }
            }
        }

        // 置ける場所がなかったらスキップ
        if(myPutConut == 0)
        {
            return;
        }

        // 探索開始
        for (int i = 0; i < _myPutStone.GetLength(0); i++)
        {
            for (int j = 0; j < _myPutStone.GetLength(0); j++)
            {
                // 石が置けるとき
                if (_myPutStone[i, j] == 1)
                {
                    // スコアを計算する
                    score = _myScript3.TurnOver(_mapCopy,i,j,_myColer, _myColer, 0);
                    flag = true;
                }

                // 新しいスコアが最大スコアより大きいとき
                if (flag && score > maxScore)
                {
                    // 最大スコアの更新
                    maxScore = score;

                    // 最大スコアのマスの記録
                    x = i;
                    z = j;
                }
                flag = false;
            }
        }

        // 最大スコアのマスを渡す
        _myScript.aaa(x, z);
        return;

    }

    #endregion

}
