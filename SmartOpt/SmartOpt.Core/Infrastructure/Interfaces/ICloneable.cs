namespace SmartOpt.Core.Infrastructure.Interfaces
{
    public interface ICloneable<out T> where T: class
    {
        T Clone();
    }
}
