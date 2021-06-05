using UnityEngine;

namespace Commands {
    public class Command {
        public readonly CommandTargetType TargetType;
        public readonly GameObject Target;
        readonly PointTarget _pointTarget;

        public Command(CommandTargetable target) {
            TargetType = target.type;
            Target = target.gameObject;
            _pointTarget = target as PointTarget;
            if (_pointTarget) {
                _pointTarget.Usages++;
            }
        }

        ~Command() {
            if (_pointTarget) {
                _pointTarget.Usages--;
            }
        }
    }
}
