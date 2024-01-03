using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.Arm;

public class BuildingGrid : MonoBehaviour
{
    public static BuildingGrid Instance;
    public Vector2Int GridSize=new Vector2Int(10,10);
    private Building[,] _grid;
  [SerializeField]private Building _flyingBuilding;
    public Camera MainCamera;
    // Start is called before the first frame update
    public Transform player; // Oyuncunun transform bile�eni
    public LayerMask planeLayer; // D�zlemi tan�mak i�in kullan�lacak katman
     private GameObject _detectedPlane; // Tespit edilen d�zlemi bu GameObject'e atayaca��z
    private float _flayingYpos;
   [HideInInspector] public Transform raycastOrigin;
    public float rayLength = 5;
   [HideInInspector] public GameObject Rayhit;
    public GameObject GameUi;
    public GameObject PanelUi;
    public bool Isshop;
    [Header("RotationBuild")]
    public GameObject BuildRotatePanel;
    public bool isBuildingComplete = false;
    private bool _isBuildingDone = false;
    public Building BuildPrefab;
    public Transform TurnAround;
    public float yOffset = -1.0f;    // Hedef objenin a�a��s�na yerle�tirilecek offset

   [SerializeField] private Animator _buildPanelAnim;
   [SerializeField] private bool _isbuilding;

    public delegate void UpdateRequirementsUIAction();
    public static event UpdateRequirementsUIAction OnUpdateRequirementsUI;

    public LayerMask BuildObjectLayer;

    public GameObject ButtonObject;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _grid=new Building[GridSize.x,GridSize.y];
    }
   
    private void Update()
    {
        // buralar�  touchphase ile yapmay� unutma ************************************
      
        PlayerRaycastGround();
        Raycasts();
        if (_flyingBuilding != null)
        {
            // Hedef objenin d�nya pozisyonunu ekran koordinatlar�na �evirin
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // �lk dokunma olay�n� al

                if (touch.phase == TouchPhase.Began)
                {
                    // Dokunma ba�lad���nda BuildRotatePanel'� kapat�n.
                    BuildRotatePanel.SetActive(false);
                    // Di�er i�lemleri burada ger�ekle�tirebilirsiniz.
                }
            }
               
            // Butonun pozisyonunu ayarlay�n

        }
    }
  
    public void UiPanelUpgrade()
    {
        OnUpdateRequirementsUI?.Invoke();
    }
    public void TickButtonClick()
    {
        if (!isBuildingComplete)
        {
            if(BuildPrefab.transform.parent==null)
                ButtonObject.GetComponent<PurchaseMenu>().IsResourcesFull();

            BuildPrefab.gameObject.transform.parent = BuildManager.Instance.Builds;
            // ��aretli d��meye t�kland���nda in�aat� tamamla
            _isBuildingDone = false;
          
            CompleteBuilding();
            BuildPrefab.FirstPos=BuildPrefab.transform.position;
            BuildManager.Instance.SaveWorld();
            Collider[] childColliders = BuildPrefab.GetComponentsInChildren<Collider>();
            // Se�ili objenin t�m child collider'lar�n� ge�ici olarak devre d��� b�rak�n.
            foreach (Collider childCollider in childColliders)
            {
                childCollider.enabled = true;
            }
            // Daha sonra bu nesneyi ekleyebilirsiniz:
            BuildPrefab = null;
            TurnAround = null;
            // Verileri kaydet
        }
        
    }
     public void BuildPanelOpen()
    {
        if (!_isbuilding)
        {
            _buildPanelAnim.Play("BuildOpen");
            _isbuilding = true;
            isBuildingComplete = false;
            PlayerSowAbility.Instance.gameObject.SetActive(false);
            Isshop = true;
        }
        else
        {
            _buildPanelAnim.Play("BuildClose");
            _isbuilding = false;
            GameUi.SetActive(true);
            PanelUi.SetActive(false);
            Isshop = false;
            if (BuildPrefab != null && BuildPrefab.transform.parent!=BuildManager.Instance.Builds)
            {
                Destroy(BuildPrefab.gameObject);
                BuildPrefab=null;
            }
         else  if (_flyingBuilding != null && _flyingBuilding.transform.parent != BuildManager.Instance.Builds)
            {
                Destroy(_flyingBuilding.gameObject);
                _flyingBuilding = null;
            }
            else if(_flyingBuilding != null && _flyingBuilding.transform.parent == BuildManager.Instance.Builds)
            {
                _flyingBuilding.GetComponent<Building>().SetAllChildrenNormal();
                _flyingBuilding = null;
              
            }

            BuildRotatePanel.SetActive(false);
            _flyingBuilding = null;
            isBuildingComplete = true;
            PlayerSowAbility.Instance.gameObject.SetActive(true);
        }
           
    }
  
    private void CompleteBuilding()
    {
        // �n�aat tamamland���nda yap�lacak i�lemler
        // �rne�in: Animasyonlar� ve di�er efektleri burada tetikleyebilirsiniz.
        Debug.Log("�n�aat tamamland�!");
        BuildRotatePanel.SetActive(false);
       
    }

    private void Raycasts()
    {
        RemovePosObject();
        if (_flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            var ray2 = MainCamera.ScreenPointToRay(Input.mousePosition);
            if (groundPlane.Raycast(ray2, out float position))
            {
                Vector3 worldPosition = ray2.GetPoint(position);
                _flyingBuilding.transform.position = worldPosition;
            }
            if (groundPlane.Raycast(ray2, out float pos))
            {
                Vector3 worldPosition = ray2.GetPoint(pos);
                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                if (x > transform.position.x && (x + _flyingBuilding.Size.x - 1 > transform.position.x + GridSize.x / 2)) _flyingBuilding.IsAvailable = false;
                if (x < transform.position.x && (x < transform.position.x - GridSize.x / 2)) _flyingBuilding.IsAvailable = false;
                if (y > transform.position.z && (y + _flyingBuilding.Size.y - 1 > transform.position.z + GridSize.y / 2)) _flyingBuilding.IsAvailable = false;
                if (y < transform.position.z && (y < transform.position.z - GridSize.y / 2)) _flyingBuilding.IsAvailable = false;

                _flyingBuilding.transform.position = new Vector3(x, _flayingYpos, y);
                _flyingBuilding.SetAllChildrenTransparent();
                if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0); // �lk dokunma olay�n� al
                    if (touch.phase == TouchPhase.Ended && _flyingBuilding.IsAvailable)
                    {

                        BuildRotatePanel.SetActive(true);
                        //  _flyingBuilding = null;
                        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(_flyingBuilding.transform.position);
                        BuildRotatePanel.transform.position = new Vector3(targetScreenPosition.x + 75f, targetScreenPosition.y + yOffset, targetScreenPosition.z);
                        PlaceFlyingBuilding();
                    }
                }
            }
        }
       
    }

    private void RemovePosObject()
    {
        if (Input.touchCount > 0&& _flyingBuilding==null)
        {
          
            Touch touch = Input.GetTouch(0); // �lk dokunma olay�n� al
            if (touch.phase == TouchPhase.Began)
            {
                // Dokunma ba�lad���nda raycast i�lemi yap
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildObjectLayer))
                {
                    // Raycast ile t�klanan nesne bu layera ait
                    GameObject clickedObject = hit.collider.gameObject;
                    // E�er t�klanan nesne BuildObject ise ve _isbuilding true ise, _flyingBuilding'e atay�n
                    if (_isbuilding)
                    {
                        if (clickedObject.transform.parent != BuildManager.Instance.Builds)
                            clickedObject = clickedObject.transform.parent.gameObject;
                        Building buildingComponent = clickedObject.GetComponent<Building>();
                        if (buildingComponent != null)
                        {
                            Debug.Log("t�klad�"+ buildingComponent.gameObject);
                            // T�klanan nesne bir Building bile�eni i�eriyorsa, _flyingBuilding'i bu nesneye atay�n
                            _flyingBuilding = buildingComponent;
                            _flayingYpos = _flyingBuilding.GetComponent<Building>().Ypos;
                            Collider[] childColliders = _flyingBuilding.GetComponentsInChildren<Collider>();

                            // Se�ili objenin t�m child collider'lar�n� ge�ici olarak devre d��� b�rak�n.
                            foreach (Collider childCollider in childColliders)
                            {
                                childCollider.enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void PlaceFlyingBuilding()
    {
        if(_flyingBuilding != null)
        _flyingBuilding.SetAllChildrenNormal();
        // Animasyon tamamland���nda _flyingBuilding'i null olarak ayarla
        _flyingBuilding = null;
       _isBuildingDone = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red; // Ray'�n rengini ayarlayabilirsiniz.

        if (_flyingBuilding != null)
        {
            // Raycast'�n ba�lang�� ve biti� noktalar�n� hesaplay�n
            raycastOrigin = _flyingBuilding.transform;
            Vector3 raycastStart = _flyingBuilding.transform.position;
            Vector3 raycastEnd = raycastStart + Vector3.down * rayLength;

            // Gizmos ile ray'� �izin
            Gizmos.DrawLine(raycastStart, raycastEnd);
        }
    }

    private void PlayerRaycastGround()
    {
        RaycastHit hit;

        // Oyuncunun alt�na bir ray f�rlat�yoruz
        if (Physics.Raycast(player.position, Vector3.down, out hit, Mathf.Infinity,planeLayer))
        {
            // Raycast bir d�zlemle temas etti
            // Tespit edilen d�zlemi GameObject'e atayabiliriz
            _detectedPlane = hit.transform.gameObject;
            if (transform.position != hit.transform.position)
            {
                transform.position = hit.transform.position;
            }

        }
        else
        {
            // Oyuncu hi�bir d�zlemde de�ilse detectedPlane'i null yapabiliriz
            _detectedPlane = null;
        }
    }
    public void CancelBuild()
    {
        if (BuildPrefab.transform.parent == null)
            Destroy(BuildPrefab.gameObject);
        BuildManager.Instance.DeleteBuilding(BuildPrefab.gameObject);
        _flyingBuilding = null;
        _isBuildingDone = false;
       
    }

    public void RightRotate()
    {
        // Implement right rotation logic here
        // For example, rotate the object to the right by a certain angle.
        if (_isBuildingDone)
      BuildPrefab.GetComponent<Building>().customPivot.transform.RotateAround(TurnAround.position, Vector3.up,90f);
    }

    public void StartPlacingBuild(Building buildingPrafab)
    {
        if (_flyingBuilding != null)
            {
                Destroy(_flyingBuilding.gameObject);
            }
            _flyingBuilding = Instantiate(buildingPrafab);
        _flayingYpos = _flyingBuilding.GetComponent<Building>().Ypos;
        
    }
}
