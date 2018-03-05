using System;

namespace SqlRepo.Abstractions
{
    public interface IStatementFactoryProvider
    {
        IStatementFactory Provide();
    }
}
