using System;

namespace Commands {
    public class UnitCommandable : Commandable {
        protected override Command AcquireCommandLogic(CommandTargetable target) {
            return target.type switch {
                CommandTargetType.Enemy => new Command(CommandType.Attack, target),
                CommandTargetType.Resource =>
                    // TODO: need resource carrier interface
                    new Command(CommandType.Move, target),
                CommandTargetType.Castle =>
                    // TODO: need resource carrier interface
                    new Command(CommandType.Move, target),
                CommandTargetType.Point => new Command(CommandType.Move, target),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}
