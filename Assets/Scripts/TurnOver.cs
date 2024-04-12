// ---------------------------------------------------------  
// TurnOver.cs  
//   
// 裏返す処理
//
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class TurnOver : MonoBehaviour
{

    #region 変数  

    [SerializeField, Header("GameManagerオブジェクト")]
    private GameObject _gameManager = default;

    // Transform取得用
    private Transform _parentTransform = default;

    // SeachPutPossibleスクリプト格納用
    private SeachPutPossible _seachPutPossible = default;

    // GameManagerスクリプト格納用
    private GameManager _gameManagerScript = default;

    #endregion

    #region プロパティ  
    #endregion

    #region メソッド  

    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        // 初期設定
        _parentTransform = this.transform;
        _seachPutPossible = this.gameObject.GetComponent<SeachPutPossible>();
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
    }

    /// <summary>
    /// 探索処理
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="stoneColer">石の色</param>
    public void Seach(int x, int z,int stoneColer)
    {
        // 現在のマスが範囲内で右隣に石があるとき
        if (z != _seachPutPossible.Map.GetLength(1) - 1 &&_seachPutPossible.Map[x, z + 1] != stoneColer && _seachPutPossible.Map[x, z + 1] != 0)
        {
            Seach1(x, z, stoneColer);
        }

        // 現在のマスが範囲内で左隣に石があるとき
        if (x != _seachPutPossible.Map.GetLength(0) - 1 && _seachPutPossible.Map[x + 1, z] != stoneColer && _seachPutPossible.Map[x + 1, z] != 0)
        {
            Seach2(x, z, stoneColer);
        }

        // 現在のマスが範囲内で下に石があるとき
        if (z != 0 && _seachPutPossible.Map[x, z - 1] != stoneColer && _seachPutPossible.Map[x, z - 1] != 0)
        {
            Seach3(x, z, stoneColer);
        }

        // 現在のマスが範囲内で上に石があるとき
        if (x != 0 && _seachPutPossible.Map[x - 1, z] != stoneColer && _seachPutPossible.Map[x - 1, z] != 0)
        {
            Seach4(x, z, stoneColer);
        }

        // 現在のマスが範囲内で左下に石があるとき
        if (x != _seachPutPossible.Map.GetLength(0) - 1 && z != _seachPutPossible.Map.GetLength(1) - 1 && _seachPutPossible.Map[x + 1, z + 1] != stoneColer && _seachPutPossible.Map[x + 1, z + 1] != 0)
        {
            Seach5(x, z, stoneColer);
        }

        // 現在のマスが範囲内で右上に石があるとき
        if (x != 0 && z != _seachPutPossible.Map.GetLength(1) - 1 && _seachPutPossible.Map[x - 1, z + 1] != stoneColer && _seachPutPossible.Map[x - 1, z + 1] != 0)
        {
            Seach6(x, z, stoneColer);
        }

        // 現在のマスが範囲内で右下に石があるとき
        if (x != _seachPutPossible.Map.GetLength(0) - 1 && z != 0 && _seachPutPossible.Map[x + 1, z - 1] != stoneColer && _seachPutPossible.Map[x + 1, z - 1] != 0)
        {
            Seach7(x, z, stoneColer);
        }

        // 現在のマスが範囲内で左上に石があるとき
        if (x != 0 && z != 0 && _seachPutPossible.Map[x - 1, z - 1] != stoneColer && _seachPutPossible.Map[x - 1, z - 1] != 0)
        {
            Seach8(x, z, stoneColer);
        }

        // ターン終了
        _gameManagerScript.aa();

    }

    /// <summary>
    /// 右隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach1(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = z + 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            // 何もなかったとき
            if (_seachPutPossible.Map[x, i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[x, i] == coler)
            {
                a = i - z - 1;
                break;
            }
        }

        for(int i = 1; i <= a; i++)
        {
            foreach(Transform child in _parentTransform)
            {
                if(child.transform.localPosition.x == x && child.transform.localPosition.z == z + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x,0,0);
                }
            }
        }
    }

    /// <summary>
    /// 左隣の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach2(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = x + 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            // 何もなかったとき
            if (_seachPutPossible.Map[i, z] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[i, z] == coler)
            {
                a = i - x - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.z == z && child.transform.localPosition.x == x + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach3(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = z - 2; i >= 0; i--)
        {
            // 何もなかったとき
            if (_seachPutPossible.Map[x,i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[x, i] == coler)
            {
                a = z - i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.z == z - i && child.transform.localPosition.x == x)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach4(int x, int z, int coler)
    {
        int a = default;
        // 隣の色を見ていく
        for (int i = x - 2; i >= 0; i--)
        {
            // 何もなかったとき
            if (_seachPutPossible.Map[i, z] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[i, z] == coler)
            {
                a = x - i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.z == z && child.transform.localPosition.x == x - i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 右下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach5(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x + i >= _seachPutPossible.Map.GetLength(0) || z + i >= _seachPutPossible.Map.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (_seachPutPossible.Map[x + i, z + i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[x + i, z + i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.x== x + i && child.transform.localPosition.z== z + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 右上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach6(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x - i < 0 || z + i >= _seachPutPossible.Map.GetLength(1))
            {
                break;
            }

            // 何もなかったとき
            if (_seachPutPossible.Map[x - i, z + i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[x - i, z + i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.x == x - i && child.transform.localPosition.z == z + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 左下の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach7(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x + i >= _seachPutPossible.Map.GetLength(0) || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (_seachPutPossible.Map[x + i, z - i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[x + i, z - i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.x == x + i && child.transform.localPosition.z == z - i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    /// <summary>
    /// 左上の探索
    /// </summary>
    /// <param name="x">縦軸</param>
    /// <param name="z">横軸</param>
    /// <param name="coler">石の色</param>
    private void Seach8(int x, int z, int coler)
    {
        int a = default;

        // 隣の色を見ていく
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x - i < 0 || z - i < 0)
            {
                break;
            }

            // 何もなかったとき
            if (_seachPutPossible.Map[x - i, z - i] == 0)
            {
                break;
            }
            // 同じ色があったとき
            else if (_seachPutPossible.Map[x - i, z - i] == coler)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _parentTransform)
            {
                if (child.transform.localPosition.x == x - i && child.transform.localPosition.z == z - i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    #endregion

}
