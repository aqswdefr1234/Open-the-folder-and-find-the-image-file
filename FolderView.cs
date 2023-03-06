using System;
using System.Collections.Generic;//리스트 사용을 위해
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

    [SerializeField] private Transform scrollViewContent;//클래스 가져오기위해서
    [SerializeField] private List<string> beforeFolder = new List<string>();
    private ScrollViewContentSize contentsize;//사용자 정의 클래스
    private string currentFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//using System;
    private bool isReturn = false;//리턴버튼을 눌러 이동했는지 확인

    private void Awake()
    {
        contentsize = scrollViewContent.GetComponent<ScrollViewContentSize>();
        LoadFiles(currentFolder);
    }

    public void LoadFiles(string folderPath)//폴더에 들어가거나, 뒤로가기 버튼을 누를때 실행된다.
    {
        if (isReturn == false)//뒤로가기 버튼을 누른것이 아닐 때
            beforeFolder.Add(currentFolder);//이동하기전 현재 폴더 값을 넣는다.
        
        currentFolder = folderPath;//이동할 폴더 값을 넣는다.
        Debug.Log(currentFolder);

        //DirectoryInfo : 디렉터리 및 하위 디렉터리를 만들고, 이동하고, 열거하는 인스턴스 메서드를 노출
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
                // 파일 처리 코드
                if (file.Extension == ".jpg" || file.Extension == ".jpeg" || file.Extension == ".png" || file.Extension == ".gif" || file.Extension == ".bmp")
                {
                    Transform buttonTransform = Instantiate(fileButtonPrefab, fileListContent);
                    buttonTransform.GetComponentInChildren<TMP_Text>().text = file.Name;
                    buttonTransform.GetComponent<Button>().onClick.AddListener(delegate { LoadImage(file.FullName); });
                }
            }
            else if (fsi is DirectoryInfo directory)
            {
                //directory.FullName 은 해당폴더의 전체 경로를 가져온다.
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
            }//클릭이벤트 할당시 기본적으로 void 함수만 할당할 수 있지만, delegate 사용시 인자가 있는 함수 사용가능.
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
    public void ReturnButton()//폴더를 클릭할 때 리턴 버튼에 들어가기전 위치가 저장됩니다.
    {
        if (beforeFolder.Count > 1)//개수가 1개이상 일 때
        {
            isReturn = true;
            LoadFiles(beforeFolder[beforeFolder.Count - 1]);//마지막 인덱스 값을 넣는다.
            Debug.Log("되돌아간 폴더" + beforeFolder[beforeFolder.Count - 1]);
            beforeFolder.RemoveAt(beforeFolder.Count - 1);
            isReturn = false;
        }
    }
}

/*
 AddListener() 메서드는 인자로 delegate를 받습니다. 이 delegate는 클릭 이벤트 발생 시 호출할 함수를 지정하는 역할을 합니다.
delegate는 함수를 객체처럼 취급할 수 있는 C#의 기능으로, 인자를 받을 수 있는 함수를 정의할 때 사용됩니다.

이를 사용하지 않고 인자를 받는 함수를 실행시키려면 다음과 같이 코드를 작성해야합니다.

GetComponent<Button>().onClick.AddListener(OnClick);
public void OnClick() {
    path = "";
    LoadFiles(path);
}

 */