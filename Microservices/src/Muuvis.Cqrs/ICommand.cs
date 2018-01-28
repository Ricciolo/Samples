using System;

namespace Muuvis.Cqrs
{
    public interface ICommand : IMessage
    {
    }

    public abstract class CommandBase : ICommand
    {
    }
}
