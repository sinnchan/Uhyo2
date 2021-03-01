namespace Src.Main.Domain.Entities
{
    public struct Result<T>
    {
        public T data;
        public bool valid;

        public Result(T data, bool valid)
        {
            this.data = data;
            this.valid = valid;
        }
    }
}
