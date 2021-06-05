using UnityEngine;

namespace Commands {
    public class Command {
        public readonly CommandType Type;
        public readonly GameObject Target;
        readonly PointTarget _pointTarget;

        public Command(CommandType type, CommandTargetable target) {
            Type = type;
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
