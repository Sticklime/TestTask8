namespace CodeBase.Infrastructure.Services.UI
{
    public interface IOpenWindow
    {
        void RegisterWindow(WindowType type);
        void Open(WindowType type);
        void Close(WindowType type);
        void CloseAll();
    }
}