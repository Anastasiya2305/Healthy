using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Keeps a square GridLayoutGroup responsive to the board container size,
/// and scales tile icon visuals relative to each calculated cell.
/// </summary>
[ExecuteAlways]
[RequireComponent(typeof(GridLayoutGroup))]
[RequireComponent(typeof(RectTransform))]
public class ResponsiveTileGridLayout : MonoBehaviour
{
    [Header("Grid")]
    [SerializeField, Min(1)] private int columns = 8;
    [SerializeField, Min(1)] private int rows = 8;
    [SerializeField, Range(0f, 24f)] private float tileSpacingPx = 6f;

    [Header("Tile Visual")]
    [SerializeField, Range(0.5f, 1f)] private float tileFillPercent = 0.9f;

    [Header("Board Padding")]
    [SerializeField, Min(0f)] private float paddingLeft = 12f;
    [SerializeField, Min(0f)] private float paddingRight = 12f;
    [SerializeField, Min(0f)] private float paddingTop = 12f;
    [SerializeField, Min(0f)] private float paddingBottom = 12f;

    private GridLayoutGroup _grid;
    private RectTransform _rectTransform;

    private void Awake()
    {
        CacheComponents();
        ApplyLayout();
    }

    private void OnEnable()
    {
        CacheComponents();
        ApplyLayout();
    }

    private void OnValidate()
    {
        CacheComponents();
        ApplyLayout();
    }

    private void OnRectTransformDimensionsChange()
    {
        ApplyLayout();
    }

    private void CacheComponents()
    {
        if (_grid == null)
        {
            _grid = GetComponent<GridLayoutGroup>();
        }

        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }

    private void ApplyLayout()
    {
        if (_grid == null || _rectTransform == null)
        {
            return;
        }

        float boardWidth = Mathf.Max(1f, _rectTransform.rect.width - paddingLeft - paddingRight);
        float boardHeight = Mathf.Max(1f, _rectTransform.rect.height - paddingTop - paddingBottom);

        float cellWidth = (boardWidth - tileSpacingPx * (columns - 1)) / columns;
        float cellHeight = (boardHeight - tileSpacingPx * (rows - 1)) / rows;
        float cellSize = Mathf.Max(1f, Mathf.Min(cellWidth, cellHeight));

        _grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        _grid.constraintCount = columns;
        _grid.spacing = new Vector2(tileSpacingPx, tileSpacingPx);
        _grid.padding = new RectOffset(
            Mathf.RoundToInt(paddingLeft),
            Mathf.RoundToInt(paddingRight),
            Mathf.RoundToInt(paddingTop),
            Mathf.RoundToInt(paddingBottom));
        _grid.cellSize = new Vector2(cellSize, cellSize);

        ResizeTileIcons(cellSize * tileFillPercent);
    }

    private void ResizeTileIcons(float iconSize)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tileRoot = transform.GetChild(i);
            Image icon = tileRoot.GetComponentInChildren<Image>(true);
            if (icon == null)
            {
                continue;
            }

            icon.preserveAspect = true;
            RectTransform iconRect = icon.rectTransform;
            iconRect.anchorMin = new Vector2(0.5f, 0.5f);
            iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.pivot = new Vector2(0.5f, 0.5f);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, iconSize);
            iconRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, iconSize);
            iconRect.anchoredPosition = Vector2.zero;
        }
    }
}
