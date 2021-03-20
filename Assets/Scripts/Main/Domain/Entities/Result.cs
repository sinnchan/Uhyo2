namespace Main.Domain.Entities
{
    public struct Result<T>
    {
        public readonly T data;
        public readonly bool valid;

        public Result(T data, bool valid)
        {
            this.data = data;
            this.valid = valid;
        }
    }
}
