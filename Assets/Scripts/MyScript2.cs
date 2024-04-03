// ---------------------------------------------------------  
// MyScript2.cs  
//   
// 設置
//
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class MyScript2 : MonoBehaviour
{

    #region 変数  

    [SerializeField,Header("SurfacePlateオブジェクト")]
    private GameObject _surfacePlate = default;

    [SerializeField, Header("GameManegerオブジェクト")]
    private GameObject _gameManeger = default;

    [SerializeField,Header("Stonesオブジェクト")]
    private Transform _stones = default;

    private GameObject[,] _putStones = new GameObject[8, 8];
    private Transform _parentTransform = default;
    private SeachPutPossible _seachPutPossible = default;
    private GameManager _gameManagerScript = default;
    private TurnOver _turnOver = default;
    private GameObject _gameObject = default;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _turnOver = _surfacePlate.GetComponent<TurnOver>();
        _parentTransform = this.gameObject.transform;
        _seachPutPossible = _surfacePlate.GetComponent<SeachPutPossible>();
        _gameManagerScript = _gameManeger.GetComponent<GameManager>();
        foreach(Transform chlid in _parentTransform)
        {
            int verticalAxis = Mathf.FloorToInt(chlid.localPosition.x);
            int horizontalAxis = Mathf.FloorToInt(chlid.localPosition.z);
            _putStones[verticalAxis, horizontalAxis] = chlid.gameObject;
            chlid.gameObject.SetActive(false);
        }
        WhitePutStone();
    }

    public void WhitePutStone()
    {
        for(int i = 0; i < _seachPutPossible.White.GetLength(0); i++)
        {
            for (int j = 0; j < _seachPutPossible.White.GetLength(1); j++)
            {
                if(_seachPutPossible.White[i,j] == 1)
                {
                    _putStones[i, j].SetActive(true);
                }
            }
        }
    }

    public void BlackPutStone()
    {
        for (int i = 0; i < _seachPutPossible.Black.GetLength(0); i++)
        {
            for (int j = 0; j < _seachPutPossible.Black.GetLength(1); j++)
            {
                if (_seachPutPossible.Black[i, j] == 1)
                {
                    _putStones[i, j].SetActive(true);
                }
            }
        }
    }

    public void aaa(int x,int z)
    {
        if(!_gameManagerScript.IsTurn)
        {

            foreach (Transform chlid in _parentTransform)
            {
                chlid.gameObject.SetActive(false);
            }

            foreach (Transform chlid in _stones)
            {
                _gameObject = chlid.gameObject;
                break;
            }

            _gameObject.transform.parent = _surfacePlate.transform;
            _gameObject.transform.localPosition = new Vector3(x, 0, z);
            _gameObject.transform.eulerAngles = new Vector3(90, 0, 0);
            _gameObject.SetActive(true);
            _seachPutPossible.Map[x, z] = 1;
            _turnOver.Seach(x,z,1);
        }
        else if(_gameManagerScript.IsTurn)
        {

            foreach (Transform chlid in _parentTransform)
            {
                chlid.gameObject.SetActive(false);
            }

            foreach (Transform chlid in _stones)
            {
                _gameObject = chlid.gameObject;
                break;
            }

            _gameObject.transform.parent = _surfacePlate.transform;
            _gameObject.transform.localPosition = new Vector3(x, 0, z);
            _gameObject.transform.eulerAngles = new Vector3(270, 0, 0);
            _gameObject.SetActive(true);
            _seachPutPossible.Map[x, z] = -1;
            _turnOver.Seach(x, z, -1);
        }
    }

    #endregion

}
