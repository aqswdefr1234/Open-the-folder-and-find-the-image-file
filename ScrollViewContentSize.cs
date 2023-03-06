using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewContentSize : MonoBehaviour
{
    //스크롤뷰의 content 부분의 높이에 따라 스크롤바의 움직일 수 있는 범위가 결정되므로 콘텐트 부분에 추가되는 프리팹 개수에 따라
    //스크롤 뷰의 높이를 직접 바꿔준다.
    [SerializeField] private RectTransform uiObject;// 높이 값을 읽기 위해서
    [SerializeField] private Transform content;//생성된 ui 프리팹 개수 세기 위해서
    [SerializeField] private int uiCount;//시리얼라이즈 필드로 선언한 이유는 인스펙터창에서 확인할수 있도록
    float uiHeight = 0;
    public void ContentScaleChange()
    {
        uiHeight = uiObject.rect.height + 10f; //10은 여유 공간
        uiCount = content.childCount;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, uiCount * uiHeight);
    }
}
