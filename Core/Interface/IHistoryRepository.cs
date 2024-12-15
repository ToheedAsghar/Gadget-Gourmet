namespace Core.Interface
{
    public interface IHistoryRepository
    {
        public void TrackPageVisit(string pageName, string pageUrl , string pageTime);
    }
}
