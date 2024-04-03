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

    [SerializeField]
    private GameObject _gameManager = default;

    private Transform _transform = default;
    private SeachPutPossible _seachPutPossible = default;
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
        _transform = this.transform;
        _seachPutPossible = this.gameObject.GetComponent<SeachPutPossible>();
        _gameManagerScript = _gameManager.GetComponent<GameManager>();
    }

    public void Seach(int x, int z,int stoneColer)
    {
        foreach (Transform chlid in _transform)
        {
            int verticalAxis = (int)chlid.localPosition.x;
            int horizontalAxis = (int)chlid.localPosition.z;

            if (verticalAxis < chlid.localPosition.x)
            {
                verticalAxis++;
            }
            else if (verticalAxis > chlid.localPosition.x)
            {
                verticalAxis--;
            }

            if (horizontalAxis < chlid.localPosition.z)
            {
                horizontalAxis++;
            }
            else if (horizontalAxis > chlid.localPosition.z)
            {
                horizontalAxis--;
            }

            switch (chlid.transform.eulerAngles.x)
            {
                case 90:
                    _seachPutPossible.Map[verticalAxis, horizontalAxis] = 1;
                    break;

                case 270:
                    _seachPutPossible.Map[verticalAxis, horizontalAxis] = -1;
                    break;
            }
        }

        if (z != _seachPutPossible.Map.GetLength(1) - 1 &&_seachPutPossible.Map[x, z + 1] != stoneColer && _seachPutPossible.Map[x, z + 1] != 0)
        {
            Seach1(x, z, stoneColer);
        }

        if (x != _seachPutPossible.Map.GetLength(0) - 1 && _seachPutPossible.Map[x + 1, z] != stoneColer && _seachPutPossible.Map[x + 1, z] != 0)
        {
            Seach2(x, z, stoneColer);
        }

        if (z != 0 && _seachPutPossible.Map[x, z - 1] != stoneColer && _seachPutPossible.Map[x, z - 1] != 0)
        {
            Seach3(x, z, stoneColer);
        }

        if (x != 0 && _seachPutPossible.Map[x - 1, z] != stoneColer && _seachPutPossible.Map[x - 1, z] != 0)
        {
            Seach4(x, z, stoneColer);
        }

        if (x != _seachPutPossible.Map.GetLength(0) - 1 && z != _seachPutPossible.Map.GetLength(1) - 1 && _seachPutPossible.Map[x + 1, z + 1] != stoneColer && _seachPutPossible.Map[x + 1, z + 1] != 0)
        {
            Seach5(x, z, stoneColer);
        }

        if (x != 0 && z != _seachPutPossible.Map.GetLength(1) - 1 && _seachPutPossible.Map[x - 1, z + 1] != stoneColer && _seachPutPossible.Map[x - 1, z + 1] != 0)
        {
            Seach6(x, z, stoneColer);
        }

        if (x != _seachPutPossible.Map.GetLength(0) - 1 && z != 0 && _seachPutPossible.Map[x + 1, z - 1] != stoneColer && _seachPutPossible.Map[x + 1, z - 1] != 0)
        {
            Seach7(x, z, stoneColer);
        }

        if (x != 0 && z != 0 && _seachPutPossible.Map[x - 1, z - 1] != stoneColer && _seachPutPossible.Map[x - 1, z - 1] != 0)
        {
            Seach8(x, z, stoneColer);
        }

        _gameManagerScript.aa();

    }

    private void Seach1(int x, int z, int n)
    {
        int a = default;
        for (int i = z + 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (_seachPutPossible.Map[x, i] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[x, i] == n)
            {
                a = i - z - 1;
                break;
            }
        }

        for(int i = 1; i <= a; i++)
        {
            foreach(Transform child in _transform)
            {
                if(child.transform.localPosition.x == x && child.transform.localPosition.z == z + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x,0,0);
                }
            }
        }
    }

    private void Seach2(int x, int z, int n)
    {
        int a = default;
        for (int i = x + 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (_seachPutPossible.Map[i, z] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[i, z] == n)
            {
                a = i - x - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
            {
                if (child.transform.localPosition.z == z && child.transform.localPosition.x == x + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    private void Seach3(int x, int z, int n)
    {
        int a = default;
        for (int i = z - 2; i > 0; i--)
        {
            if (_seachPutPossible.Map[x,i] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[x, i] == n)
            {
                a = z - i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
            {
                if (child.transform.localPosition.z == z - i && child.transform.localPosition.x == x)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    private void Seach4(int x, int z, int n)
    {
        int a = default;
        for (int i = x - 2; i > 0; i--)
        {
            if (_seachPutPossible.Map[i, z] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[i, z] == n)
            {
                a = x - i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
            {
                if (child.transform.localPosition.z == z && child.transform.localPosition.x == x - i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    private void Seach5(int x, int z, int n)
    {
        int a = default;
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x + i >= _seachPutPossible.Map.GetLength(0) || z + i >= _seachPutPossible.Map.GetLength(1))
            {
                break;
            }

            if (_seachPutPossible.Map[x + i, z + i] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[x + i, z + i] == n)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
            {
                if (child.transform.localPosition.x== x + i && child.transform.localPosition.z== z + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    private void Seach6(int x, int z, int n)
    {
        int a = default;
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x - i < 0 || z + i >= _seachPutPossible.Map.GetLength(1))
            {
                break;
            }

            if (_seachPutPossible.Map[x - i, z + i] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[x - i, z + i] == n)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
            {
                if (child.transform.localPosition.x == x - i && child.transform.localPosition.z == z + i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    private void Seach7(int x, int z, int n)
    {
        int a = default;
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x + i >= _seachPutPossible.Map.GetLength(0) || z - i < 0)
            {
                break;
            }

            if (_seachPutPossible.Map[x + i, z - i] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[x + i, z - i] == n)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
            {
                if (child.transform.localPosition.x == x + i && child.transform.localPosition.z == z - i)
                {
                    child.gameObject.transform.eulerAngles = new Vector3(-child.gameObject.transform.eulerAngles.x, 0, 0);
                }
            }
        }
    }

    private void Seach8(int x, int z, int n)
    {
        int a = default;
        for (int i = 2; i < _seachPutPossible.Map.GetLength(1); i++)
        {
            if (x - i < 0 || z - i < 0)
            {
                break;
            }

            if (_seachPutPossible.Map[x - i, z - i] == 0)
            {
                break;
            }
            else if (_seachPutPossible.Map[x - i, z - i] == n)
            {
                a = i - 1;
                break;
            }
        }

        for (int i = 1; i <= a; i++)
        {
            foreach (Transform child in _transform)
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
