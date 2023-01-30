using UnityEngine;

namespace SpaceOdysseyVR.Reactor
{
    public class StatusReactor : StatusLight.Status
    {
        private int _counter;
        private ReactorDragDrop[] _reactorDragDrops;

        public int Counter
        {
            get => _counter;
            set
            {
                if (_counter == value)
                    return;
                _counter = value;
                IsComplete = (Counter / _reactorDragDrops.Length) == 1;
            }
        }

        private void Start ()
        {
            _reactorDragDrops = GetComponentsInChildren<ReactorDragDrop>();
            foreach (var reactorDrop in _reactorDragDrops)
            {
                reactorDrop.OnUnitEnter += () => Counter++;
                reactorDrop.OnUnitExit += () => Counter--;
            }
        }

        [ContextMenu(nameof(MakeCrash))]
        public void MakeCrash ()
        {
            foreach (var reactorDrop in _reactorDragDrops)
            {
                reactorDrop.MakeCrash();
            }
        }
    }
}