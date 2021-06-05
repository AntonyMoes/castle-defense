namespace Commands {
    public class PointTarget : CommandTargetable {
        public PointTarget() {
            type = CommandTargetType.Point;
        }

        int _usages;
        public int Usages {
            get => _usages;
            set {
                _usages = value;
                
                if (value == 0) {
                    _markedForDeath = true;
                }
            }
        }

        bool _markedForDeath;

        void Update() {
            if (!_markedForDeath) {
                return;
            }
            
            Destroy(gameObject);
        }
    }
}
