// ---------------------------------------------------------  
// TitleUI.cs  
// 
// タイトルシーンUI処理
//
// 作成日: 2024/4/16
// 作成者: 北川 稔明
// ---------------------------------------------------------  
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUI : MonoBehaviour
{

    #region メソッド  

    // 初級が選択されたとき
    public void ElementaryLevel()
    {
        SceneManager.LoadScene("ElementaryLevel");
    }

    //中級が選択されたとき
    public void IntermediateLevel()
    {
        SceneManager.LoadScene("IntermediateLevel");
    }

    // 上級が選択されたとき
    public void AdvancedLevel()
    {
        SceneManager.LoadScene("AdvancedLevel");
    }

    /// <summary>
    /// 終了が押されたとき
    /// </summary>
    public void GameEnd()
    {
        // ゲーム終了
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
        #else
                Application.Quit();//ゲームプレイ終了
        #endif
    }

    #endregion

}
