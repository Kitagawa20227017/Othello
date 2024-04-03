// ---------------------------------------------------------  
// SeachPutPossible.cs  
//
// 設置可能マスを調べる  
//
// 作成日:  
// 作成者:  
// ---------------------------------------------------------  
using UnityEngine;

public class SeachPutPossible : MonoBehaviour
{

    #region 変数  

    #region const定数

    // 白の石の向き
    private const int WHITE_STONE_ANGLE = 270;

    // 黒の石の向き
    private const int BLACK_STONE_ANGLE = 90;

    // 白の石の値
    private const int WHITE_STONE_INDEX = -1;

    // 黒の石の値
    private const int BLACK_STONE_INDEX = 1;

    #endregion

    [SerializeField]
    private GameObject _gameManager = default;

    private GameManager _gameManagerScript = default;

    private Transform _transform = default;
    private int[,] _surfacePlate = new int[8, 8];

    private int[,] _whiteSurfacePlate = new int[8, 8];
    private int[,] _blackSurfacePlate = new int[8, 8];

    private int _whiteStoneConut = 0;
    private int _blackStoneConut = 0;

    private int _whiteStoneSum = 0;
    private int _blackStoneSum = 0;

    #endregion

    #region プロパティ  

    public int[,] Map
    {
        get => _surfacePlate;
    }

    public int[,] Black
    {
        get => _blackSurfacePlate;
    }

    public int[,] White 
    {
        get => _whiteSurfacePlate;
    }

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _transform = this.transform;
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
        Seach();
    }

    public void Seach()
    {

        if (_transform.childCount >= 64)
        {
            foreach(int stone in _surfacePlate)
            {
                if(stone == 1)
                {
                    _blackStoneSum++;
                }
                else if(stone == -1)
                {
                    _whiteStoneSum++;
                }

            }
            string s = default;
            if (_blackStoneSum > _whiteStoneSum)
            {
                s = "黒勝ち";
            }
            else if (_blackStoneSum > _whiteStoneSum)
            {
                s = "白勝ち";
            }
            else if (_blackStoneSum == _whiteStoneSum)
            {
                s = "引き分け";
            }
            _gameManagerScript.nnnn(s);
            return;
        }

        // 初期化
        _whiteStoneConut = 0;
        _blackStoneConut = 0;
        for (int i = 0; i < _blackSurfacePlate.GetLength(0);i++)
        {
            for(int j = 0; j < _blackSurfacePlate.GetLength(1);j++)
            {
                _blackSurfacePlate[i, j] = 0;
                _whiteSurfacePlate[i, j] = 0;
                _surfacePlate[i, j] = 0;
            }
        }

        // 子オブジェクト探索
        foreach (Transform chlid in _transform)
        {
            // 座標をintにする
            int verticalAxis = (int)chlid.localPosition.x;
            int horizontalAxis = (int)chlid.localPosition.z;
            
            // 値がずれたら補正
            if(verticalAxis < chlid.localPosition.x)
            {
                verticalAxis++;
            }
            else if(verticalAxis > chlid.localPosition.x)
            {
                verticalAxis--;
            }

            // 値がずれたら補正
            if (horizontalAxis < chlid.localPosition.z)
            {
                horizontalAxis++;
            }
            else if (horizontalAxis > chlid.localPosition.z)
            {
                horizontalAxis--;
            }

            // 石が表か裏かを見る
            switch (chlid.transform.eulerAngles.x)
            {
                case BLACK_STONE_ANGLE:
                    _surfacePlate[verticalAxis, horizontalAxis] = BLACK_STONE_INDEX;
                    break;

                case WHITE_STONE_ANGLE:
                    _surfacePlate[verticalAxis, horizontalAxis] = WHITE_STONE_INDEX;
                    break;
            }
        }

        for (int i = 0; i < _surfacePlate.GetLength(0); i++)
        {
            for (int j = 0; j < _surfacePlate.GetLength(1); j++)
            {
                // 現在のマスが範囲内で石が置いてなく右隣に石があるとき
                if (j != _surfacePlate.GetLength(1) - 1 && _surfacePlate[i, j + 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach1(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左隣に石があるとき
                if (j != 0 && _surfacePlate[i, j - 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach2(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく下隣に石があるとき
                if (i != _surfacePlate.GetLength(0) - 1 && _surfacePlate[i + 1, j] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach3(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく上隣に石があるとき
                if (i != 0 && _surfacePlate[i - 1, j] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach4(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左下に石があるとき
                if (i != _surfacePlate.GetLength(0) - 1 && j != _surfacePlate.GetLength(1) - 1 && _surfacePlate[i + 1, j + 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach5(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右上に石があるとき
                if (i != 0 && j != _surfacePlate.GetLength(1) - 1 && _surfacePlate[i - 1, j + 1] != 0 && _surfacePlate[i, j] == 0)
                {
                   Seach6(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく右下に石があるとき
                if (i != _surfacePlate.GetLength(0) - 1 && j != 0 && _surfacePlate[i + 1, j - 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach7(i, j);
                }

                // 現在のマスが範囲内で石が置いてなく左上に石があるとき
                if (i != 0 && j != 0 && _surfacePlate[i - 1, j - 1] != 0 && _surfacePlate[i, j] == 0)
                {
                    Seach8(i, j);
                }
            }
        }

        // 白と黒の石がそれぞれ何個置けるか調べる
        for(int i = 0; i < _whiteSurfacePlate.GetLength(0); i++)
        {
            for(int j = 0; j < _whiteSurfacePlate.GetLength(1); j++)
            {
                _whiteStoneConut += _whiteSurfacePlate[i, j];
                _blackStoneConut += _blackSurfacePlate[i, j];
            }
        }

        if (_whiteStoneConut == 0 && !_gameManagerScript.IsTurn)
        {
            _gameManagerScript.aa();
        }
        else if(_blackStoneConut == 0 && _gameManagerScript.IsTurn)
        {
            _gameManagerScript.aa();
        }
    }

    private void Seach1(int x,int z)
    {
        int n = _surfacePlate[x, z +1];
        for(int i = z + 2; i < _surfacePlate.GetLength(1); i++)
        {
            if(_surfacePlate[x,i] == 0)
            {
                break;
            }
            else if (_surfacePlate[x, i] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach2(int x, int z)
    {
        int n = _surfacePlate[x, z - 1];
        for (int i = z - 2; i > 0; i--)
        {
            if (_surfacePlate[x, i] == 0)
            {
                break;
            }
            else if (_surfacePlate[x, i] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach3(int x, int z)
    {
        int n = _surfacePlate[x + 1, z];
        for (int i = x + 2; i < _surfacePlate.GetLength(0); i++)
        {
            if (_surfacePlate[i, z] == 0)
            {
                break;
            }
            else if (_surfacePlate[i, z] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach4(int x, int z)
    {
        int n = _surfacePlate[x - 1, z];
        for (int i = x - 2; i > 0; i++)
        {
            if (_surfacePlate[i, z] == 0)
            {
                break;
            }
            else if (_surfacePlate[i, z] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach5(int x, int z)
    {
        int n = _surfacePlate[x + 1, z + 1];
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            if(x + i >= _surfacePlate.GetLength(0) || z + i >= _surfacePlate.GetLength(1))
            {
                break;
            }

            if(_surfacePlate[x + i,z + i] == 0)
            {
                break;
            }
            else if(_surfacePlate[x + i, z + i] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach6(int x, int z)
    {
        int n = _surfacePlate[x - 1, z + 1];
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            if (x - i < 0 || z + i >= _surfacePlate.GetLength(1))
            {
                break;
            }

            if (_surfacePlate[x - i, z + i] == 0)
            {
                break;
            }
            else if (_surfacePlate[x - i, z + i] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach7(int x, int z)
    {
        int n = _surfacePlate[x + 1, z - 1];
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            if (x + i >= _surfacePlate.GetLength(0) || z - i < 0)
            {
                break;
            }

            if (_surfacePlate[x + i, z - i] == 0)
            {
                break;
            }
            else if (_surfacePlate[x + i, z - i] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void Seach8(int x, int z)
    {
        int n = _surfacePlate[x - 1, z - 1];
        for (int i = 2; i < _surfacePlate.GetLength(0); i++)
        {
            if (x - i < 0 || z - i < 0)
            {
                break;
            }

            if (_surfacePlate[x - i, z - i] == 0)
            {
                break;
            }
            else if (_surfacePlate[x - i, z - i] == -n)
            {
                aaa(n, x, z);
                break;
            }
        }
    }

    private void aaa(int n,int x,int z)
    {
        if(n == 1)
        {
            _blackSurfacePlate[x, z] = 1;
        }
        else if(n == -1)
        {
            _whiteSurfacePlate[x, z] = 1;
        }
    }

    #endregion

}
