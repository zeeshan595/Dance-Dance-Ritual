using UnityEngine;
using System.Collections;
using System;

#pragma warning disable 0649

public class Painting : MonoBehaviour
{
    [SerializeField]
    private int playerNum = 0;
    [SerializeField]
    private Color brushColor;
    [SerializeField]
    public float brushSize = 1.0f;
    [SerializeField]
    private GameObject brushPrefab, brushOutline, brushCountainer;
    [SerializeField]
    private GameObject pointer;
    [SerializeField]
    private Camera sceneCam, CanvasCam;
    [SerializeField]
    public RenderTexture renderTexture;
    [SerializeField]
    private Material baseMaterial;
    [SerializeField]
    private Texture baseTexture;
    [SerializeField]
    private GameObject background;
    [SerializeField]
    private int MAX_BRUSHES = 1000;

    [System.NonSerialized]
    public bool isSaving = false;
    private int brushCount = 0;
    private PaintControls controls;

    private void Start()
    {
        controls = GetComponent<PaintControls>();
        StartCoroutine(LoadTexture());
    }

    private IEnumerator LoadTexture()
    {
        WWW w = new WWW("file://" + Settings.savePath + playerNum + ".png");
        yield return w;
        if (w.error == null)
        {
            baseTexture = w.texture;
        }
        baseMaterial.mainTexture = baseTexture;
    }

    private void Update()
    {
        UpdateBrushOutline();
        if (!isSaving)
            DoPainting();

        //Update Pointer
        ((RectTransform)pointer.transform).anchoredPosition = controls.cursorPos() - new Vector2((sceneCam.rect.x * Screen.width), 0);

        brushSize = Mathf.Clamp(brushSize, 0.1f, 2.0f);
    }

    private void UpdateBrushOutline()
    {
        Vector3 uvWorldPosition = Vector3.zero;
        if (GetWorldUVFromRay(ref uvWorldPosition) && !isSaving)
        {
            brushOutline.SetActive(true);
            brushOutline.transform.position = uvWorldPosition + brushCountainer.transform.position;
            brushOutline.transform.localScale = Vector3.one * brushSize;
        }
        else
        {
            brushOutline.SetActive(false);
        }
    }

    private void DoPainting()
    {
        float isPainting = 0;

        if (PlayerManager.players[playerNum].controller == Player.ControllerType.Controller)
            isPainting = XboxCtrlrInput.XCI.GetAxis(XboxCtrlrInput.XboxAxis.RightTrigger, (XboxCtrlrInput.XboxController)PlayerManager.players[playerNum].controllerNum);
        else if (PlayerManager.players[playerNum].controller == Player.ControllerType.Keyboard)
        {
            if (Input.GetKey(KeyCode.Mouse0))
                isPainting = 1.0f;
            else
                isPainting = 0.0f;
        }

        if (isPainting > 0.5f)
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (GetWorldUVFromRay(ref uvWorldPosition))
            {
                GameObject brushObj = (GameObject)Instantiate(brushPrefab);
                brushObj.GetComponent<SpriteRenderer>().color = brushColor;
                brushObj.transform.SetParent(brushCountainer.transform);
                brushObj.transform.localPosition = uvWorldPosition;
                brushObj.transform.localScale = Vector3.one * brushSize;
            }
            brushCount++;
            if (brushCount >= MAX_BRUSHES)
            {
                brushOutline.SetActive(false);
                isSaving = true;
                StartCoroutine(SaveTexture());
            }
        }

        RaycastHit hit;
        Ray cursorRay = sceneCam.ScreenPointToRay(controls.cursorPos());
        if (Physics.Raycast(cursorRay, out hit, 50))
        {
            if (hit.collider.tag == "Player" + playerNum.ToString())
            {
                PaintColor col = hit.collider.gameObject.GetComponent<PaintColor>();
                if (col)
                {
                    brushColor = col.color;
                }
            }
        }
    }

    private bool GetWorldUVFromRay(ref Vector3 WorldUV)
    {
        RaycastHit hit;
        Ray cursorRay = sceneCam.ScreenPointToRay(controls.cursorPos());
        Debug.DrawRay(cursorRay.origin, cursorRay.direction * 5, Color.red);
        if (Physics.Raycast(cursorRay, out hit, 200))
        {
            MeshCollider collider = hit.collider as MeshCollider;
            if (collider == null || collider.sharedMesh == null || collider.tag != "Player" + playerNum.ToString())
            {
                return false;
            }

            Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
            WorldUV.x = (pixelUV.x * 2) - CanvasCam.orthographicSize;
            WorldUV.y = (pixelUV.y * 2) - CanvasCam.orthographicSize;
            WorldUV.z = 0.0f;
            return true;
        }
        else
            return false;
    }

    public IEnumerator SaveTexture()
    {
        isSaving = true;
        brushOutline.SetActive(false);
        brushOutline.transform.position = new Vector3(500, 500, 500);
        yield return new WaitForSeconds(0.1f);
        brushCount = 0;
        RenderTexture.active = renderTexture;
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        baseMaterial.mainTexture = tex; //Put the painted texture as the base
        foreach (Transform child in brushCountainer.transform)
        {
            Destroy(child.gameObject);
        }

        if (!System.IO.Directory.Exists(Settings.savePath))
            System.IO.Directory.CreateDirectory(Settings.savePath);
        var bytes = tex.EncodeToPNG();
        System.IO.File.WriteAllBytes(Settings.savePath + playerNum + ".png", bytes);

        isSaving = false;
    }
}