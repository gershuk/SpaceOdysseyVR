#nullable enable

using System;

using UnityEngine;

namespace SpaceOdysseyVR.HandTeleporter
{
    public enum TeleportSphereState
    {
        None = 0,
        Invisible = 1,
        Visible = 2,
        Moving = 3
    }

    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class TeleportSphere : MonoBehaviour
    {
        private MeshRenderer _meshRenderer;
        private ParticleSystem _particleSystem;
        private ParticleSystemRenderer _particleSystemRenderer;
        private TeleportSphereState _state;
        private Transform _transform;

        public Material Material
        {
            get => _meshRenderer.material;
            set
            {
                _meshRenderer.material = value;
                _particleSystemRenderer.material = value;
            }
        }

        public Vector3 Position
        {
            get => _transform.position;
            set => _transform.position = value;
        }

        public float Size
        {
            get => _transform.localScale.x == _transform.localScale.y
                && _transform.localScale.y == _transform.localScale.z
                ? _transform.localScale.x
                : throw new Exception("x!=y!=!z");
            set => _transform.localScale = new(value, value, value);
        }

        public TeleportSphereState State
        {
            get => _state;
            set
            {
                if (_state == value)
                    return;
                _state = value;
                _ = _state switch
                {
                    TeleportSphereState.Invisible => MakeInvisible(),
                    TeleportSphereState.Visible => MakeVisible(),
                    TeleportSphereState.Moving => MakeMoving(),
                    _ => throw new System.NotImplementedException(),
                };
            }
        }

        private TeleportSphere MakeInvisible ()
        {
            _meshRenderer.enabled = false;
            if (_particleSystem.isEmitting)
                _particleSystem.Stop();
            return this;
        }

        private TeleportSphere MakeMoving ()
        {
            _meshRenderer.enabled = true;
            _particleSystem.Play();
            return this;
        }

        private TeleportSphere MakeVisible ()
        {
            _meshRenderer.enabled = true;
            if (_particleSystem.isEmitting)
                _particleSystem.Stop();
            return this;
        }

        private void Start ()
        {
            _transform = transform;
            _meshRenderer = GetComponent<MeshRenderer>();
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
            State = TeleportSphereState.Invisible;
        }
    }
}