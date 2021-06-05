using System;
using UnityEngine;

namespace Commands {
    public class Command {
        public readonly CommandTargetType TargetType;
        public readonly GameObject Target;
        readonly PointTarget _pointTarget;

        public Command(CommandTargetable target) {
            if (target == null) {
                throw new ArgumentException("Provided object is not targetable");
            }

            TargetType = target.type;
            Target = target.gameObject;
            _pointTarget = target as PointTarget;
            if (_pointTarget) {
                _pointTarget.Usages++;
            }
        }

        public Command(GameObject target) : this(target.GetComponent<CommandTargetable>()) { }

        ~Command() {
            if (_pointTarget) {
                _pointTarget.Usages--;
            }
        }
    }
}
