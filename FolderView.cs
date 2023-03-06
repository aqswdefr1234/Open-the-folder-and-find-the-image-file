using System;
using System.Collections.Generic;//����Ʈ ����� ����
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
public class FolderView : MonoBehaviour
{
    public TMP_Text folderNameText;
    public Transform fileButtonPrefab;
    public Transform fileListContent;
    public RawImage rawImage;

    [SerializeField] private Transform scrollViewContent;//Ŭ���� �����������ؼ�
    [SerializeField] private List<string> beforeFolder = new List<string>();
    private ScrollViewContentSize contentsize;//����� ���� Ŭ����
    private string currentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//using System;
    private bool isReturn = false;//���Ϲ�ư�� ���� �̵��ߴ��� Ȯ��

    private void Awake()
    {
        contentsize = scrollViewContent.GetComponent<ScrollViewContentSize>();
        LoadFiles(currentFolder);
    }

    public void LoadFiles(string folderPath)//������ ���ų�, �ڷΰ��� ��ư�� ������ ����ȴ�.
    {
        if (isReturn == false)//�ڷΰ��� ��ư�� �������� �ƴ� ��
            beforeFolder.Add(currentFolder);//�̵��ϱ��� ���� ���� ���� �ִ´�.
        
        currentFolder = folderPath;//�̵��� ���� ���� �ִ´�.
        Debug.Log(currentFolder);

        //DirectoryInfo : ���͸� �� ���� ���͸��� �����, �̵��ϰ�, �����ϴ� �ν��Ͻ� �޼��带 ����
        DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
        
        if (!directoryInfo.Exists)
        {
            Debug.LogError("Directory not found");
            return;
        }

        folderNameText.text = directoryInfo.Name;

        // Clear old buttons from the list
        foreach (Transform child in fileListContent)
        {
            Destroy(child.gameObject);
        }
        FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
        foreach (FileSystemInfo fsi in fileSystemInfos)
        {
            if (fsi is FileInfo file)
            {
                // ���� ó�� �ڵ�
                if (file.Extension == ".jpg" || file.Extension == ".jpeg" || file.Extension == ".png" || file.Extension == ".gif" || file.Extension == ".bmp")
                {
                    Transform buttonTransform = Instantiate(fileButtonPrefab, fileListContent);
                    buttonTransform.GetComponentInChildren<TMP_Text>().text = file.Name;
                    buttonTransform.GetComponent<Button>().onClick.AddListener(delegate { LoadImage(file.FullName); });
                }
            }
            else if (fsi is DirectoryInfo directory)
            {
                //directory.FullName �� �ش������� ��ü ��θ� �����´�.
                Transform buttonTransform = Instantiate(fileButtonPrefab, fileListContent);
                if(directory.Name.Length != 0)
                {
                    if (directory.Name.Length <= 10)
                        buttonTransform.GetComponentInChildren<TMP_Text>().text = directory.Name;
                    else if (directory.Name.Length > 10)
                        buttonTransform.GetComponentInChildren<TMP_Text>().text = directory.Name.Substring(0, 8) + "..." + directory.Name.Substring(directory.Name.Length - 3, 3);
                }
                
                buttonTransform.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f);
                buttonTransform.GetComponent<Button>().onClick.AddListener(delegate { LoadFiles(directory.FullName); });
            }//Ŭ���̺�Ʈ �Ҵ�� �⺻������ void �Լ��� �Ҵ��� �� ������, delegate ���� ���ڰ� �ִ� �Լ� ��밡��.
        }
        contentsize.ContentScaleChange();
    }
    public void LoadImage(string filePath)
    {
        byte[] fileData = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);
        rawImage.texture = texture;
    }
    public void ReturnButton()//������ Ŭ���� �� ���� ��ư�� ������ ��ġ�� ����˴ϴ�.
    {
        if (beforeFolder.Count > 1)//������ 1���̻� �� ��
        {
            isReturn = true;
            LoadFiles(beforeFolder[beforeFolder.Count - 1]);//������ �ε��� ���� �ִ´�.
            Debug.Log("�ǵ��ư� ����" + beforeFolder[beforeFolder.Count - 1]);
            beforeFolder.RemoveAt(beforeFolder.Count - 1);
            isReturn = false;
        }
    }
}

/*
 AddListener() �޼���� ���ڷ� delegate�� �޽��ϴ�. �� delegate�� Ŭ�� �̺�Ʈ �߻� �� ȣ���� �Լ��� �����ϴ� ������ �մϴ�.
delegate�� �Լ��� ��üó�� ����� �� �ִ� C#�� �������, ���ڸ� ���� �� �ִ� �Լ��� ������ �� ���˴ϴ�.

�̸� ������� �ʰ� ���ڸ� �޴� �Լ��� �����Ű���� ������ ���� �ڵ带 �ۼ��ؾ��մϴ�.

GetComponent<Button>().onClick.AddListener(OnClick);
public void OnClick() {
    path = "";
    LoadFiles(path);
}

 */