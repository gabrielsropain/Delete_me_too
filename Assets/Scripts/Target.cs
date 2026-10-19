using System;
using UnityEngine;

// 1. Abstraction
public abstract class Target : MonoBehaviour, IHittable
{
    [Header("Target Stats")]
    [Tooltip("How many paintballs it takes for the target to break. ")]
    [SerializeField] private int maxHits = 1;

    [Tooltip("Points the player gets from breaking the target")]
    [SerializeField] private int points = 10;

    [Header("Hit Flash")]
    [Tooltip("How long the target flashes when hit ")]
    [SerializeField] private float flashDuration = 0.1f;

    //3. Encapsulation - Internal State
    private int _hitsRemaining;
    private Renderer _targetRenderer;
    private Color _baseColor = Color.white;
    private float flashTimer = 0f;

    // 3. Encapsulation - Properties
    public int Points => points;
    public int HitsRemaining => _hitsRemaining;
    public bool IsDefeated { get; private set; }

    // 3. Encapsulation - Protected functions and variables - Private for everybody, public for children.

    protected Color BaseColor
    {
        get { return _baseColor; }
        set { _baseColor = value; }
    }

    protected virtual void Awake()
    {
        _hitsRemaining = Mathf.Max(1, maxHits);
        _targetRenderer = GetComponent<Renderer>(); //TODO: Check if we do it in child

        _baseColor = _targetRenderer.material.color;
    }

    protected virtual void Update()
    {
        if (IsDefeated) return;

        Move(); //4. Polymorfisim 
        UpdateFlash();
    }

    private void UpdateFlash()
    {
        if (flashTimer <= 0f) return;

        flashTimer -= Time.deltaTime;
        if (flashTimer <= 0f)
        {
            SetRendererColor(_baseColor);
        }
    }

    private void SetRendererColor(Color baseColor)
    {
        _targetRenderer.material.color = baseColor;
    }

    protected abstract void Move();

    public void OnPaintballHit(Color paintballColor, Vector3 hitPoint)
    {
        if (IsDefeated) return;

        _hitsRemaining--;

        OnHit(paintballColor, hitPoint);

        if(_hitsRemaining <= 0)
        {
            IsDefeated = true;
            OnDefeated();
        }
    }

    private void OnHit(Color painballColor, Vector3 hitPoint)
    {
        Flash(painballColor);
    }

    private void Flash(Color paintballColor)
    {
        if (flashDuration <= 0f) return;
        flashTimer = flashDuration;
        SetRendererColor(paintballColor);
    }

    private void OnDefeated()
    {
        // TODO: Add points to the Score Manager.
        Destroy(gameObject);
    }
}
