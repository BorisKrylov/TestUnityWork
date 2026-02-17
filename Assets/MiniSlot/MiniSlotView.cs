using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;
using UnityEngine.UI;

namespace MiniSlot
{
    /// <summary>
    /// Визуальная часть отображения барабана слотов:
    /// </summary>
    public class MiniSlotView : MonoBehaviourExtBind
    {
        private const int CellsCount = 5; // 3 визуальных + 2 буферных
        private const int CenterIndex = 2; // 0..4

        [Header("UI refs")]
        [SerializeField] private RectTransform _viewport;
        [SerializeField] private RectTransform _reelRoot;
        [SerializeField] private Image[] _cells = new Image[CellsCount];
        [SerializeField] private SlotIconConfig _iconConfig;
        
        [Header("Tuning")]
        [SerializeField] private float _maxSpeed = 1400f; 
        [SerializeField] private float _acceleration = 2200f;
        [SerializeField] private float _deceleration = 2600f;
        [SerializeField] private float _snapDuration = 0.28f;

        [Header("FX")]
        [SerializeField] private ParticleSystem _burstParticles;
        [SerializeField] private float _particlesZ = 5f;

        private readonly List<Sprite> _iconPool = new();
        
        private readonly int[] _cellIconIds = new int[CellsCount];

        private float _cellHeight;
        private float _offsetY;
        private float _speed;
        private float _targetSpeed;

        private bool _isSpinning;
        private bool _isSnapping;
        private float _snapT;
        private float _snapFrom;
        private float _snapTo;

        private Camera _cachedCamera;

        [OnAwake]
        private void OnAwake()
        {
            _cachedCamera = Camera.main;
        }

        [OnStart]
        private void OnStart()
        {
            _iconPool.Clear();
            _iconPool.AddRange(_iconConfig.Icons);
            
            for (var i = 0; i < CellsCount; i++)
            {
                _cellIconIds[i] = i % Mathf.Max(1, _iconPool.Count);
                _cells[i].sprite = _iconPool.Count > 0 ? _iconPool[_cellIconIds[i]] : null;
                _cells[i].preserveAspect = true;
            }
            
            RecalculateCellHeight();
            ApplyPositions();
        }

        [Bind(Consts.Events.SpinStart)]
        private void OnSpinStart()
        {
            _isSpinning = true;
            _isSnapping = false;
            _snapT = 0;
            _targetSpeed = Mathf.Max(0, _maxSpeed);
        }

        [Bind(Consts.Events.SpinStop)]
        private void OnSpinStop()
        {
            _targetSpeed = 0;
        }

        [Bind(Consts.Events.PlayBurst)]
        private void OnPlayBurst()
        {
            PlayBurstAtCenter();
        }

        [OnUpdate]
        private void UpdateThis()
        {
            float dt = Time.deltaTime;

            if (!_isSpinning && !_isSnapping)
                return;

            float rate = _targetSpeed > _speed ? _acceleration : _deceleration;
            _speed = Mathf.MoveTowards(_speed, _targetSpeed, rate * dt);

            if (!_isSnapping && _targetSpeed <= 0.01f && _speed <= 0.01f)
            {
                _speed = 0;
                BeginSnapToCenter();
            }

            if (_isSnapping)
            {
                _snapT += dt / Mathf.Max(0.0001f, _snapDuration);
                float time = Mathf.Clamp01(_snapT);
                float eased = time * time * (3f - 2f * time);
                _offsetY = Mathf.Lerp(_snapFrom, _snapTo, eased);

                if (time >= 1f)
                {
                    _isSnapping = false;
                    _isSpinning = false;
                    _speed = 0;
                    _targetSpeed = 0;
                    
                    if (Mathf.Abs(_snapTo - _cellHeight) < 0.001f)
                    {
                        StepOneCell();
                        _offsetY = 0;
                    }

                    ApplyPositions();
                    Settings.Invoke(Consts.Events.SlotStopped, GetCenterIconId());
                    return;
                }

                ApplyPositions();
                return;
            }
            
            NormalizationScrolling(dt);
            ApplyPositions();
        }

        private void NormalizationScrolling(float deltaTime)
        {
            _offsetY += _speed * deltaTime;
            if (_cellHeight > 0.001f)
            {
                while (_offsetY >= _cellHeight)
                {
                    _offsetY -= _cellHeight;
                    StepOneCell();
                }
            }
        }

        private void BeginSnapToCenter()
        {
            if (_cellHeight <= 0.001f)
                return;

            _isSnapping = true;
            _snapT = 0;
            _snapFrom = _offsetY;
            
            _snapTo = _offsetY > (_cellHeight * 0.5f) ? _cellHeight : 0f;
        }

        private void StepOneCell()
        {
            for (var i = 0; i < CellsCount - 1; i++)
            {
                _cellIconIds[i] = _cellIconIds[i + 1];
            }

            _cellIconIds[CellsCount - 1] = NextRandomIconId();

            for (var i = 0; i < CellsCount; i++)
            {
                _cells[i].sprite = _iconPool.Count > 0 ? _iconPool[_cellIconIds[i]] : null;
            }
        }

        private int NextRandomIconId()
        {
            if (_iconPool.Count <= 1)
                return 0;

            int lastTop = _cellIconIds[CellsCount - 1];
            int id = Random.Range(0, _iconPool.Count);
            if (id == lastTop)
            {
                id = (id + 1) % _iconPool.Count;
            }
            
            return id;
        }

        private void ApplyPositions()
        {
            RecalculateCellHeight();

            for (var i = 0; i < CellsCount; i++)
            {
                int rel = i - CenterIndex; // -2..2
                float y = (rel * _cellHeight) - _offsetY;
                _cells[i].rectTransform.anchoredPosition = new Vector2(0f, y);
            }
        }

        private void RecalculateCellHeight()
        {
            if (_viewport == null)
                return;
            
            _cellHeight = Mathf.Max(1f, _viewport.rect.height / 3f);
            
            for (var i = 0; i < CellsCount; i++)
            {
                RectTransform rt = _cells[i]?.rectTransform;
                if (rt == null) 
                    continue;
                
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _viewport.rect.width);
                rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _cellHeight);
            }
        }

        private int GetCenterIconId() 
            => _cellIconIds[CenterIndex];

        private void PlayBurstAtCenter()
        {
            if (_burstParticles == null)
                return;
            
            RectTransform centerCell = _cells[CenterIndex]?.rectTransform;
            
            if (centerCell == null)
            {
                _burstParticles.Play(true);
                return;
            }

            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(null, centerCell.position);
            Vector3 world = _cachedCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, _particlesZ));
            
            _burstParticles.transform.position = world;
            _burstParticles.Play(true);
        }
    }
}

