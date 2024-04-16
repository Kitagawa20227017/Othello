// ---------------------------------------------------------  
// RandomAIAlgorithms.cs
// 
// ランダムAIの処理
//   
// 作成日: 2024/4/11
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class RandomAIAlgorithms : MonoBehaviour
{

    #region 変数  

    [SerializeField,Header("SurfacePlateオブジェクト")]
    private GameObject _surfacePlate = default;

    [SerializeField, Header("gameManagerオブジェクト")]
    private GameObject _gameManager = default;

    [SerializeField, Header("PutStoneオブジェクト")]
    private GameObject _putStone = default;

    // GameManager取得用
    private GameManager _gameManagerScript = default;

    // SeachPutPossible取得用
    private SeachPutPossible _seachPutPossible = default;

    // StoneControl取得用
    private StoneControl _turnOver = default;

    // 自分が黒かどうか
    private bool _isBlack = false;

    // 自分が置けるマス
    private int[,] _myPutStone = new int[8, 8];

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _turnOver = _putStone.GetComponent<StoneControl>();
        if(_gameManagerScript.IsPlayerTurn == false)
        {
            _isBlack = true;
        }
        else
        {
            _isBlack = false;
        }
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>
    private void Update()
    {
        // 自分のターンのとき
        if(_gameManagerScript.IsPlayerTurn == false)
        {
            RandomAI();
        }
    }

    /// <summary>
    /// ランダムAI処理
    /// </summary>
    public void RandomAI()
    {
        // ランダムに選ぶマス
        int randomMass = 0;

        // 置けるマスの数
        int putStoneConut = 0;

        // 自分が白のとき
        if (!_isBlack)
        {
            // マスを選ぶ
            randomMass = Random.Range(1,_seachPutPossible.BlackCount + 1);

            // 置けるマスえおコピー
            for(int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
            {
                for(int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.Black[i, j];
                }
            }
        }
        // 自分が白のとき
        else
        {
            // マスを選ぶ
            randomMass = Random.Range(1, _seachPutPossible.WhiteCount + 1);

            // 置けるマスえおコピー
            for (int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.White[i, j];
                }
            }
        }

        // 置くマスを探す
        for (int i = 0; i < _myPutStone.GetLength(0); i++)
        {
            for (int j = 0; j < _myPutStone.GetLength(0); j++)
            {
                // 置けるマスのとき
                if(_myPutStone[i,j] == 1)
                {
                    putStoneConut++;
                }

                // 置くマスがあったとき
                if(putStoneConut == randomMass)
                {
                    _turnOver.PutStone(i, j);
                    return;
                }
            }
        }

    }

    #endregion

}
