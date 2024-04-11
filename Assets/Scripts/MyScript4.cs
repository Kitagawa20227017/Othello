// ---------------------------------------------------------  
// MyScript4.cs  
// 
//
//
// 作成日: 
// 作成者: 
// ---------------------------------------------------------  
using UnityEngine;
using System.Collections;

public class MyScript4 : MonoBehaviour
{

    #region 変数  

    private AITurnOver _myScript = default;

    #endregion

    #region メソッド  
     
    /// <summary>  
    /// 更新前処理  
    /// </summary>  
    private void Start ()
    {
        _myScript = this.gameObject.GetComponent<AITurnOver>();
    }


    public int lll(int[,] n, int[,] myPut, int myStoneColer, int turnStoneColer, int conut)
    {
        int score = 0;
        int maxScore = 0;
        for(int i = 0; i < myPut.GetLength(0);i++)
        {
            for(int j = 0;j< myPut.GetLength(1);j++)
            {
                if(myPut[i,j] == 1)
                {
                    score = _myScript.TurnOver(n, i, j, myStoneColer, turnStoneColer, conut);
                }

                if(myStoneColer == turnStoneColer && score > maxScore)
                {
                    maxScore = score;
                }
                else if (myStoneColer != turnStoneColer && score < maxScore)
                {
                    maxScore = score;
                }
            }
        }
        return maxScore;
    }


    #endregion

}
