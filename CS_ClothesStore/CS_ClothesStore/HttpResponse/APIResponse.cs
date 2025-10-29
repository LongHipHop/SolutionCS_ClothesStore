namespace CS_ClothesStore.HttpResponse
{
    public class APIResponse<T>
    {
        public string Code { get; set; }
        public T Result { get; set; }
    }
}
