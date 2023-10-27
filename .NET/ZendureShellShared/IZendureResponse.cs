namespace ZendureShellShared
{
    public interface IZendureResponse
    {
        bool success { get; }

        public string ToJson();

        public string DataToJson();
    }
}