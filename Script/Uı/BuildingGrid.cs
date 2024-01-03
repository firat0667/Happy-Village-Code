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
    public Transform player; // Oyuncunun transform bileþeni
    public LayerMask planeLayer; // Düzlemi tanýmak için kullanýlacak katman
     private GameObject _detectedPlane; // Tespit edilen düzlemi bu GameObject'e atayacaðýz
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
    public float yOffset = -1.0f;    // Hedef objenin aþaðýsýna yerleþtirilecek offset

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
        // buralarý  touchphase ile yapmayý unutma ************************************
      
        PlayerRaycastGround();
        Raycasts();
        if (_flyingBuilding != null)
        {
            // Hedef objenin dünya pozisyonunu ekran koordinatlarýna çevirin
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // Ýlk dokunma olayýný al

                if (touch.phase == TouchPhase.Began)
                {
                    // Dokunma baþladýðýnda BuildRotatePanel'ý kapatýn.
                    BuildRotatePanel.SetActive(false);
                    // Diðer iþlemleri burada gerçekleþtirebilirsiniz.
                }
            }
               
            // Butonun pozisyonunu ayarlayýn

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
            // Ýþaretli düðmeye týklandýðýnda inþaatý tamamla
            _isBuildingDone = false;
          
            CompleteBuilding();
            BuildPrefab.FirstPos=BuildPrefab.transform.position;
            BuildManager.Instance.SaveWorld();
            Collider[] childColliders = BuildPrefab.GetComponentsInChildren<Collider>();
            // Seçili objenin tüm child collider'larýný geçici olarak devre dýþý býrakýn.
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
        // Ýnþaat tamamlandýðýnda yapýlacak iþlemler
        // Örneðin: Animasyonlarý ve diðer efektleri burada tetikleyebilirsiniz.
        Debug.Log("Ýnþaat tamamlandý!");
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
                    Touch touch = Input.GetTouch(0); // Ýlk dokunma olayýný al
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
          
            Touch touch = Input.GetTouch(0); // Ýlk dokunma olayýný al
            if (touch.phase == TouchPhase.Began)
            {
                // Dokunma baþladýðýnda raycast iþlemi yap
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, BuildObjectLayer))
                {
                    // Raycast ile týklanan nesne bu layera ait
                    GameObject clickedObject = hit.collider.gameObject;
                    // Eðer týklanan nesne BuildObject ise ve _isbuilding true ise, _flyingBuilding'e atayýn
                    if (_isbuilding)
                    {
                        if (clickedObject.transform.parent != BuildManager.Instance.Builds)
                            clickedObject = clickedObject.transform.parent.gameObject;
                        Building buildingComponent = clickedObject.GetComponent<Building>();
                        if (buildingComponent != null)
                        {
                            Debug.Log("týkladý"+ buildingComponent.gameObject);
                            // Týklanan nesne bir Building bileþeni içeriyorsa, _flyingBuilding'i bu nesneye atayýn
                            _flyingBuilding = buildingComponent;
                            _flayingYpos = _flyingBuilding.GetComponent<Building>().Ypos;
                            Collider[] childColliders = _flyingBuilding.GetComponentsInChildren<Collider>();

                            // Seçili objenin tüm child collider'larýný geçici olarak devre dýþý býrakýn.
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
        // Animasyon tamamlandýðýnda _flyingBuilding'i null olarak ayarla
        _flyingBuilding = null;
       _isBuildingDone = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = UnityEngine.Color.red; // Ray'ýn rengini ayarlayabilirsiniz.

        if (_flyingBuilding != null)
        {
            // Raycast'ýn baþlangýç ve bitiþ noktalarýný hesaplayýn
            raycastOrigin = _flyingBuilding.transform;
            Vector3 raycastStart = _flyingBuilding.transform.position;
            Vector3 raycastEnd = raycastStart + Vector3.down * rayLength;

            // Gizmos ile ray'ý çizin
            Gizmos.DrawLine(raycastStart, raycastEnd);
        }
    }

    private void PlayerRaycastGround()
    {
        RaycastHit hit;

        // Oyuncunun altýna bir ray fýrlatýyoruz
        if (Physics.Raycast(player.position, Vector3.down, out hit, Mathf.Infinity,planeLayer))
        {
            // Raycast bir düzlemle temas etti
            // Tespit edilen düzlemi GameObject'e atayabiliriz
            _detectedPlane = hit.transform.gameObject;
            if (transform.position != hit.transform.position)
            {
                transform.position = hit.transform.position;
            }

        }
        else
        {
            // Oyuncu hiçbir düzlemde deðilse detectedPlane'i null yapabiliriz
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
