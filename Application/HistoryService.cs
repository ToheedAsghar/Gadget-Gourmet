using Core.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application;

public class HistoryService
{
    private IHistoryRepository _historyRepo;

    public HistoryService(IHistoryRepository historyRepo)
    {
        _historyRepo = historyRepo;
    }

    public void TrackPageVisit(string pageName, string pageUrl, string pageTime)
    {
        _historyRepo.TrackPageVisit(pageName, pageUrl, pageTime);
    }
}
