using UnityEngine;

/// <summary>
/// メッシュの実際のワールドサイズ（頂点の広がり × スケール）に応じて
/// _Tilingを自動設定し、SpriteRendererのTiledモードのように
/// テクスチャ密度を一定に保つ。
/// 
/// 平面ポリゴン（厚みなし）でも正しく動作する。
/// </summary>
[ExecuteAlways]
public class AutoTiling : MonoBehaviour
{
    [Header("テクスチャ密度 (1ワールドユニットあたりの繰り返し回数)")]
    [Tooltip("値が大きいほどテクスチャが細かく繰り返される")]
    public Vector2 texelsPerUnit = new Vector2(1f, 1f);

    [Header("メッシュの平面方向")]
    [Tooltip("メッシュがどの平面に広がっているか")]
    public PlaneAxis planeAxis = PlaneAxis.XY;

    [Header("対象マテリアルのインデックス (通常は0)")]
    public int materialIndex = 0;

    public enum PlaneAxis
    {
        XY,  // 2Dゲームや正面向き平面
        XZ,  // 地面・床
        YZ   // 横向き壁
    }

    private Renderer _renderer;
    private MeshFilter _meshFilter;
    private MaterialPropertyBlock _mpb;
    private static readonly int TilingId = Shader.PropertyToID("_Tiling");

    private Vector3 _lastScale;
    private Bounds _lastBounds;
    private bool _dirty = true;

    void OnEnable()
    {
        _renderer = GetComponent<Renderer>();
        _meshFilter = GetComponent<MeshFilter>();
        _mpb = new MaterialPropertyBlock();
        _dirty = true;
        UpdateTiling();
    }

    void Update()
    {
        Vector3 currentScale = transform.lossyScale;
        Mesh mesh = _meshFilter != null ? _meshFilter.sharedMesh : null;

        if (mesh != null && (currentScale != _lastScale || mesh.bounds != _lastBounds || _dirty))
        {
            UpdateTiling();
            _lastScale = currentScale;
            _lastBounds = mesh.bounds;
            _dirty = false;
        }
    }

    void UpdateTiling()
    {
        if (_renderer == null || _meshFilter == null) return;

        Mesh mesh = _meshFilter.sharedMesh;
        if (mesh == null) return;

        // メッシュのローカルバウンズ × ワールドスケール = 実際のワールドサイズ
        Vector3 boundsSize = mesh.bounds.size;
        Vector3 scale = transform.lossyScale;

        Vector3 worldSize = new Vector3(
            boundsSize.x * Mathf.Abs(scale.x),
            boundsSize.y * Mathf.Abs(scale.y),
            boundsSize.z * Mathf.Abs(scale.z)
        );

        // 平面の向きに応じて2軸を選択
        Vector2 planeSize = planeAxis switch
        {
            PlaneAxis.XY => new Vector2(worldSize.x, worldSize.y),
            PlaneAxis.XZ => new Vector2(worldSize.x, worldSize.z),
            PlaneAxis.YZ => new Vector2(worldSize.y, worldSize.z),
            _ => new Vector2(worldSize.x, worldSize.y)
        };

        // ワールドサイズ × 密度 = タイリング回数
        Vector2 tiling = new Vector2(
            planeSize.x * texelsPerUnit.x,
            planeSize.y * texelsPerUnit.y
        );

        // ゼロ防止
        if (tiling.x < 0.001f) tiling.x = 1f;
        if (tiling.y < 0.001f) tiling.y = 1f;

        _renderer.GetPropertyBlock(_mpb, materialIndex);
        _mpb.SetVector(TilingId, tiling);
        _renderer.SetPropertyBlock(_mpb, materialIndex);
    }

    /// <summary>
    /// 外部からメッシュ変更を通知する（ProceduralMesh等で頂点を直接変更した場合）
    /// </summary>
    public void NotifyMeshChanged()
    {
        _dirty = true;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (_renderer != null)
        {
            _dirty = true;
            UpdateTiling();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (_meshFilter == null || _meshFilter.sharedMesh == null) return;
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.3f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(_meshFilter.sharedMesh.bounds.center, _meshFilter.sharedMesh.bounds.size);
    }
#endif
}
