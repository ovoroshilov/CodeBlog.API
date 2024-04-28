using System.Data;

namespace CodeBlog.API.Repositories.Interface
{
    public interface ICacheRepository
    {
        T GetData<T>(string key);
        bool SetData<T>(string key, T value, DateTimeOffset expireTime);
        object RemoveData<T>(string key);
    }
}
