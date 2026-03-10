namespace SocialX.Core.Interface;

public interface IUnitOfWork
{
    Task<bool> SalvarAsync();
}
