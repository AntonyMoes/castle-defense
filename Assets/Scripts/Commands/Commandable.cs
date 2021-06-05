using System;
using UnityEngine;

namespace Commands {
    public class Commandable : MonoBehaviour {
        Command _currentCommand;

        public Command CurrentCommand {
            get => _currentCommand;
            set {
                _currentCommand = value;
                OnCommand?.Invoke(_currentCommand);
            }
        }

        public Action<Command> OnCommand;

        public void AcquireCommand(CommandTargetable target) {
            CurrentCommand = new Command(target);
        }
    }
}
