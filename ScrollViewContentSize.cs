using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewContentSize : MonoBehaviour
{
    //��ũ�Ѻ��� content �κ��� ���̿� ���� ��ũ�ѹ��� ������ �� �ִ� ������ �����ǹǷ� ����Ʈ �κп� �߰��Ǵ� ������ ������ ����
    //��ũ�� ���� ���̸� ���� �ٲ��ش�.
    [SerializeField] private RectTransform uiObject;// ���� ���� �б� ���ؼ�
    [SerializeField] private Transform content;//������ ui ������ ���� ���� ���ؼ�
    [SerializeField] private int uiCount;//�ø�������� �ʵ�� ������ ������ �ν�����â���� Ȯ���Ҽ� �ֵ���
    float uiHeight = 0;
    public void ContentScaleChange()
    {
        uiHeight = uiObject.rect.height + 10f; //10�� ���� ����
        uiCount = content.childCount;

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, uiCount * uiHeight);
    }
}
