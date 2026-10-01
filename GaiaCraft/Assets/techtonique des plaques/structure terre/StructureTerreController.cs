using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class StructureTerreController : MonoBehaviour
{
    [Header("Audio Clips earth part")]
    [SerializeField] private AudioClip _intro;
    [SerializeField] private AudioClip _intro_cut;
    [SerializeField] private AudioClip _litosphere;
    [SerializeField] private AudioClip _asthenosphere;
    [SerializeField] private AudioClip _manteau_inferieur;
    [SerializeField] private AudioClip _noyeau_externe;
    [SerializeField] private AudioClip _noyeau_interne;

    [Header("Audio Clips plates")]
    [SerializeField] private AudioClip _plaques_intro;
    [SerializeField] private AudioClip _plaques_african;
    [SerializeField] private AudioClip _plaques_north_america;
    [SerializeField] private AudioClip _plaques_south_america;
    [SerializeField] private AudioClip _plaques_pacific;
    [SerializeField] private AudioClip _plaques_antartic;
    [SerializeField] private AudioClip _plaques_arabian;
    [SerializeField] private AudioClip _plaques_eurasian;
    [SerializeField] private AudioClip _plaques_indian;
    [SerializeField] private AudioClip _plaques_caribbean;
    [SerializeField] private AudioClip _plaques_cocos;
    [SerializeField] private AudioClip _plaques_nazca;
    [SerializeField] private AudioClip _plaques_philippine;
    [SerializeField] private AudioClip _plaques_scotia;
    [SerializeField] private AudioClip _plaques_juan_de_fuca;
    [SerializeField] private AudioClip _plaques_juan_fernandez;
    [SerializeField] private AudioClip _plaques_australian;
    [SerializeField] private AudioClip _plaques_easter;

    [Header("Audio Clips collisions")]

    [Header("3 parts models")]
    [SerializeField] private GameObject _earthModel;
    [SerializeField] private GameObject _mapModel;
    [SerializeField] private GameObject _colidersModel;

    [Header("Plates parts")]
    [SerializeField] private GameObject _africanPlate;
    [SerializeField] private GameObject _northAmericaPlate;
    [SerializeField] private GameObject _southAmericaPlate;
    [SerializeField] private GameObject _pacificPlate;
    [SerializeField] private GameObject _antarticPlate;
    [SerializeField] private GameObject _arabianPlate;
    [SerializeField] private GameObject _eurasianPlate;
    [SerializeField] private GameObject _indianPlate;
    [SerializeField] private GameObject _caribbeanPlate;
    [SerializeField] private GameObject _cocosPlate;
    [SerializeField] private GameObject _nazcaPlate;
    [SerializeField] private GameObject _philippinePlate;
    [SerializeField] private GameObject _scotiaPlate;
    [SerializeField] private GameObject _juanDeFucaPlate;
    [SerializeField] private GameObject _juanFernandezPlate;
    [SerializeField] private GameObject _australianPlate;
    [SerializeField] private GameObject _easterPlate;

    [Header("Others")]

    [SerializeField] private AudioClip _pop_sound;
    [SerializeField] private GameObject _trailObject;
    [SerializeField] private GameObject _camera;

    

    private AudioSource _audioSource;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _endPosition = Vector3.zero;

    private bool introPlayed = false;
    private bool cutPlayed = false;

    private int _step = 0;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _earthModel.transform.localScale = new Vector3(0, 0, 0);

        _mapModel.SetActive(false);
        _africanPlate.SetActive(false);
        _northAmericaPlate.SetActive(false);
        _southAmericaPlate.SetActive(false);
        _pacificPlate.SetActive(false);
        _antarticPlate.SetActive(false);
        _arabianPlate.SetActive(false);
        _eurasianPlate.SetActive(false);
        _indianPlate.SetActive(false);
        _caribbeanPlate.SetActive(false);
        _cocosPlate.SetActive(false);
        _nazcaPlate.SetActive(false);
        _philippinePlate.SetActive(false);
        _scotiaPlate.SetActive(false);
        _juanDeFucaPlate.SetActive(false);
        _juanFernandezPlate.SetActive(false);
        _australianPlate.SetActive(false);
        _easterPlate.SetActive(false);

        _colidersModel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if on computer
        if(Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            _trailObject.SetActive(true);
            _trailObject.transform.position = ray.GetPoint(10);

            // if the ray hits the earth check the slice
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == _earthModel)
                {
                    if (_startPosition == Vector3.zero)
                    {
                        _startPosition = hit.point;
                    } else
                    {
                        _endPosition = hit.point;
                    }
                }
            } else if (_startPosition != Vector3.zero && _endPosition != Vector3.zero)
            {
                


                Vector3 _slicePlaneNormal = Vector3.Cross(_startPosition - _endPosition, _camera.transform.position - _startPosition).normalized;
                Point3D center = Geometry3D.FindCenter(new Point3D(_startPosition.x, _startPosition.y, _startPosition.z), new Point3D(_endPosition.x, _endPosition.y, _endPosition.z), new Point3D(_camera.transform.position.x, _camera.transform.position.y, _camera.transform.position.z));
                Vector3 _slicePlaneOrigin = new Vector3((float)center.X, (float)center.Y, (float)center.Z);

                GameObject internCore = SliceEarth(_earthModel.transform.GetChild(5).gameObject, _slicePlaneNormal, _slicePlaneOrigin); // internal core
                internCore.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                internCore.transform.position += _slicePlaneNormal * 0.01f;

                GameObject externCore = SliceEarth(_earthModel.transform.GetChild(5).gameObject, _slicePlaneNormal, _slicePlaneOrigin); // external core
                externCore.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                externCore.transform.position += _slicePlaneNormal * 0.008f;

                GameObject inferiorCoat = SliceEarth(_earthModel.transform.GetChild(5).gameObject, _slicePlaneNormal, _slicePlaneOrigin); // inferior coat
                inferiorCoat.transform.localScale = new Vector3(0.07f, 0.07f, 0.07f);
                inferiorCoat.transform.position += _slicePlaneNormal * 0.006f;

                GameObject asthenosphere = SliceEarth(_earthModel.transform.GetChild(5).gameObject, _slicePlaneNormal, _slicePlaneOrigin); // lithosphere
                asthenosphere.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                asthenosphere.transform.position += _slicePlaneNormal * 0.004f;

                GameObject lithosphere = SliceEarth(_earthModel.transform.GetChild(5).gameObject, _slicePlaneNormal, _slicePlaneOrigin); // lithosphere
                lithosphere.transform.localScale = new Vector3(0.096f, 0.096f, 0.096f);
                lithosphere.transform.position += _slicePlaneNormal * 0.002f;

                GameObject earth = SliceEarth(_earthModel.transform.GetChild(5).gameObject, _slicePlaneNormal, _slicePlaneOrigin); // earth
                earth.transform.localScale = new Vector3(0.09999999f, 0.09999999f, 0.09999999f);



                //Destroy(plane);

                _startPosition = Vector3.zero;
                _endPosition = Vector3.zero;

                Playcutearth();
            }
        } else
        {
            _trailObject.SetActive(false);
            _startPosition = Vector3.zero;
            _endPosition = Vector3.zero;
        }
       

        // if on mobile


        
    }

    public GameObject SliceEarth(GameObject sliceObject, Vector3 _slicePlaneNormal, Vector3 _slicePlaneOrigin)
    {
        // apply rotation to mesh to match the plane
        Mesh mesh = sliceObject.GetComponent<MeshFilter>().sharedMesh;

        // creta new mesh from the rotated vertices
        Mesh newMesh = new Mesh();
        newMesh.vertices = mesh.vertices;
        newMesh.triangles = mesh.triangles;
        newMesh.normals = mesh.normals;
        newMesh.uv = mesh.uv;

        // rotate the vertices following this rotation
        Vector3[] vertices = newMesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            // rotate vertices to match this rotation so that the slice is correct
            vertices[i] = this.transform.rotation * vertices[i];
        }
        newMesh.vertices = vertices;

        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();

        // apply the new mesh to the earth
        sliceObject.GetComponent<MeshFilter>().sharedMesh = newMesh;


        MeshSlicerScaffolding meshSlicer = sliceObject.GetComponent<MeshSlicerScaffolding>();
        GameObject[] parts = meshSlicer.SliceMesh(_slicePlaneNormal, _slicePlaneOrigin);
        foreach (GameObject part in parts)
        {
            part.transform.SetParent(_earthModel.transform);
            part.transform.SetAsFirstSibling();
            part.transform.position = _earthModel.transform.position;
        }
        parts[0].AddComponent<Rigidbody>();
        // add force to the rigidbody from the normal
        parts[0].GetComponent<Rigidbody>().AddForce(_slicePlaneNormal * 100);
        parts[0].transform.SetParent(null);

        parts[1].transform.rotation = sliceObject.transform.rotation;
        Destroy(sliceObject);
        GameObject slicedEarth = parts[1];


        // restore initial mesh rotation
        Mesh slicedMesh = slicedEarth.GetComponent<MeshFilter>().sharedMesh;
        Mesh newSlicedMesh = new Mesh();
        newSlicedMesh.name = "newEarth";
        newSlicedMesh.vertices = slicedMesh.vertices;
        newSlicedMesh.triangles = slicedMesh.triangles;
        newSlicedMesh.normals = slicedMesh.normals;
        newSlicedMesh.uv = slicedMesh.uv;

        // rotate the vertices back to the original rotation
        Vector3[] slicedVertices = newSlicedMesh.vertices;
        for (int i = 0; i < slicedVertices.Length; i++)
        {
            slicedVertices[i] = Quaternion.Inverse(this.transform.rotation) * slicedVertices[i];
        }
        newSlicedMesh.vertices = slicedVertices;
        newMesh.RecalculateBounds();
        newMesh.RecalculateNormals();
        newMesh.RecalculateTangents();

        slicedEarth.GetComponent<MeshFilter>().sharedMesh = newSlicedMesh;

        return slicedEarth;
    }

    public void PlayIntro()
    {
        if (introPlayed)
        {
            return;
        }
        StartCoroutine(GrowEarth());
        introPlayed = true;

    }

    IEnumerator GrowEarth()
    {
        _audioSource.PlayOneShot(_pop_sound);
        for (float i = 0; i < 1.5; i += 0.05f)
        {
            _earthModel.transform.localScale = new Vector3(i, i, i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1);
        _audioSource.PlayOneShot(_intro);
    }

    public void Playcutearth()
    {
        if (cutPlayed)
        {
            return;
        }
        StartCoroutine(PlayIntroCut());
        cutPlayed = true;
    }

    IEnumerator PlayIntroCut()
    {
        yield return new WaitForSeconds(1);
        _audioSource.PlayOneShot(_intro_cut);
        
        // wait for the intro to finish
        while (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        _audioSource.PlayOneShot(_litosphere);

        // make lithosphere blink
        GameObject lithosphere = _earthModel.transform.GetChild(1).gameObject;
        MeshRenderer lithosphereRenderer = lithosphere.GetComponent<MeshRenderer>();
        Material lithosphereMaterial = lithosphereRenderer.material;
        Color originalLithosphereColor = lithosphereMaterial.color;


        // wait for the audio to finish
        while (_audioSource.isPlaying)
        {
            lithosphereMaterial.color = originalLithosphereColor;
            yield return new WaitForSeconds(0.3f);
            lithosphereMaterial.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        lithosphereMaterial.color = originalLithosphereColor;

        _audioSource.PlayOneShot(_asthenosphere);

        // make asthenosphere blink
        GameObject asthenosphere = _earthModel.transform.GetChild(2).gameObject;
        MeshRenderer asthenosphereRenderer = asthenosphere.GetComponent<MeshRenderer>();
        Material asthenosphereMaterial = asthenosphereRenderer.material;
        Color originalAsthenosphereColor = asthenosphereMaterial.color;


        // wait for the audio to finish
        while (_audioSource.isPlaying)
        {
            asthenosphereMaterial.color = originalAsthenosphereColor;
            yield return new WaitForSeconds(0.3f);
            asthenosphereMaterial.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        asthenosphereMaterial.color = originalAsthenosphereColor;

        _audioSource.PlayOneShot(_manteau_inferieur);

        // make inferior coat blink
        GameObject inferiorCoat = _earthModel.transform.GetChild(3).gameObject;
        MeshRenderer inferiorCoatRenderer = inferiorCoat.GetComponent<MeshRenderer>();
        Material inferiorCoatMaterial = inferiorCoatRenderer.material;
        Color originalInferiorCoatColor = inferiorCoatMaterial.color;

        // wait for the audio to finish
        while (_audioSource.isPlaying)
        {
            inferiorCoatMaterial.color = originalInferiorCoatColor;
            yield return new WaitForSeconds(0.3f);
            inferiorCoatMaterial.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        inferiorCoatMaterial.color = originalInferiorCoatColor;

        _audioSource.PlayOneShot(_noyeau_externe);

        // make external core blink
        GameObject externalCore = _earthModel.transform.GetChild(4).gameObject;
        MeshRenderer externalCoreRenderer = externalCore.GetComponent<MeshRenderer>();
        Material externalCoreMaterial = externalCoreRenderer.material;
        Color originalExternalCoreColor = externalCoreMaterial.color;

        // wait for the audio to finish
        while (_audioSource.isPlaying)
        {
            externalCoreMaterial.color = originalExternalCoreColor;
            yield return new WaitForSeconds(0.3f);
            externalCoreMaterial.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        externalCoreMaterial.color = originalExternalCoreColor;

        _audioSource.PlayOneShot(_noyeau_interne);

        // make internal core blink
        GameObject internalCore = _earthModel.transform.GetChild(5).gameObject;
        MeshRenderer internalCoreRenderer = internalCore.GetComponent<MeshRenderer>();
        Material internalCoreMaterial = internalCoreRenderer.material;
        Color originalInternalCoreColor = internalCoreMaterial.color;

        // wait for the audio to finish
        while (_audioSource.isPlaying)
        {
            internalCoreMaterial.color = originalInternalCoreColor;
            yield return new WaitForSeconds(0.3f);
            internalCoreMaterial.color = Color.white;
            yield return new WaitForSeconds(0.3f);
        }
        internalCoreMaterial.color = originalInternalCoreColor;

        yield return new WaitForSeconds(3);

        // hide earth
        _earthModel.SetActive(false);

        yield return new WaitForSeconds(1);

        _mapModel.SetActive(true);
        StartCoroutine(playPlates());


    }

    IEnumerator playPlates()
    {
        const int timeToWait = 8;

        yield return new WaitForSeconds(1);
        _audioSource.PlayOneShot(_plaques_intro);

        while (_audioSource.isPlaying)
        {
            yield return new WaitForSeconds(0.1f);
        }

        _audioSource.PlayOneShot(_plaques_african);
        int time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _africanPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _africanPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_north_america);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _northAmericaPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _northAmericaPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_south_america);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _southAmericaPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _southAmericaPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_pacific);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _pacificPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _pacificPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_antartic);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _antarticPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _antarticPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_arabian);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _arabianPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _arabianPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_eurasian);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _eurasianPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _eurasianPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_indian);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _indianPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _indianPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_caribbean);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _caribbeanPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _caribbeanPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_cocos);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _cocosPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _cocosPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_nazca);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _nazcaPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _nazcaPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_philippine);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _philippinePlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _philippinePlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_scotia);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _scotiaPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _scotiaPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_juan_de_fuca);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _juanDeFucaPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _juanDeFucaPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_juan_fernandez);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _juanFernandezPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _juanFernandezPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_australian);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _australianPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _australianPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        _audioSource.PlayOneShot(_plaques_easter);
        time = 0;

        while (_audioSource.isPlaying && time < timeToWait)
        {
            _easterPlate.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _easterPlate.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            time++;
        }

        yield return new WaitForSeconds(1);

        _mapModel.SetActive(false);

        yield return new WaitForSeconds(1);

        _colidersModel.SetActive(true);

        StartCoroutine(PlayCollisions());

        
    }

    IEnumerator PlayCollisions()
    {
        yield return new WaitForSeconds(1);
    }

}

public class Point
{
    public double X { get; set; }
    public double Y { get; set; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
}

public class Circle
{
    public Point Center { get; set; }
    public double Radius { get; set; }

    public Circle(Point center, double radius)
    {
        Center = center;
        Radius = radius;
    }
}

public class Point3D
{
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }

    public Point3D(double x, double y, double
 z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public static Point3D CrossProduct(Point3D p1, Point3D p2)
    {
        return new Point3D(p1.Y * p2.Z - p1.Z * p2.Y, p1.Z * p2.X - p1.X * p2.Z, p1.X * p2.Y - p1.Y * p2.X);
    }

    public static double DotProduct(Point3D p1, Point3D p2)
    {
        return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
    }

    public static Point3D Subtract(Point3D p1, Point3D p2)
    {
        return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
    }
}

public class Geometry3D
{
    public static Point3D FindCenter(Point3D p1, Point3D p2, Point3D p3)
    {
        // Find midpoints
        Point3D midpoint1 = new Point3D((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        Point3D midpoint2 = new Point3D((p1.X + p3.X) / 2, (p1.Y + p3.Y) / 2, (p1.Z + p3.Z) / 2);
        Point3D midpoint3 = new Point3D((p2.X + p3.X) / 2, (p2.Y + p3.Y) / 2, (p2.Z + p3.Z) / 2);

        // Find direction vectors for the lines connecting the midpoints
        Point3D direction1 = new Point3D(midpoint2.X - midpoint1.X, midpoint2.Y - midpoint1.Y, midpoint2.Z - midpoint1.Z);
        Point3D direction2 = new Point3D(midpoint3.X - midpoint1.X, midpoint3.Y - midpoint1.Y, midpoint3.Z - midpoint1.Z);

        // Find the normal vector to the plane containing the midpoints
        Point3D normal = Point3D.CrossProduct(direction1, direction2);

        // Find the equation of the plane passing through the midpoints
        double d = -Point3D.DotProduct(normal, midpoint1);

        // Find the intersection of the plane with the line passing through midpoint1 and normal
        double t = -d / Point3D.DotProduct(normal, normal);
        return new Point3D(midpoint1.X + t * normal.X, midpoint1.Y + t * normal.Y, midpoint1.Z + t * normal.Z);
    }
}
