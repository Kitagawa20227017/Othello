// ---------------------------------------------------------  
// MyScript1.cs  
//   
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class RandomAIAlgorithms : MonoBehaviour
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
    private StoneControl _turnOver = default;
    private bool _isBlack = false;
    private int[,] _myPutStone = new int[8, 8];

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
        if(_gameManagerScript.IsPlayerTurn == false)
        {
            RandomAI();
        }
    }

    public void RandomAI()
    {
        int randomMass = 0;
        int putStoneConut = 0;
        if (!_isBlack)
        {
            randomMass = Random.Range(1,_seachPutPossible.BlackCount + 1);
            for(int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
            {
                for(int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.Black[i, j];
                }
            }
        }
        else
        {
            randomMass = Random.Range(1, _seachPutPossible.WhiteCount + 1);
            for (int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
            {
                for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
                {
                    _myPutStone[i, j] = _seachPutPossible.White[i, j];
                }
            }
        }

        for (int i = 0; i < _myPutStone.GetLength(0); i++)
        {
            for (int j = 0; j < _myPutStone.GetLength(0); j++)
            {
                if(_myPutStone[i,j] == 1)
                {
                    putStoneConut++;
                }

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
