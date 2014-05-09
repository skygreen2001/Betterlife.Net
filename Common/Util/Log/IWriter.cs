
namespace Util.Log
{
    /// <summary>
    /// 日期记录器接口
    /// </summary>
    public interface IWriter
    {
        void Write(string message);

        void Dispose();
    }
}
