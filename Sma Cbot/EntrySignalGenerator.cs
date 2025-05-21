using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots;

public class EntrySignalGenerator: IEntrySignalGenerator
{
    private readonly Bars _bars;
    private readonly MovingAverage _sma;

    public EntrySignalGenerator(Bars bars, MovingAverage sma)
    {
        _bars = bars;
        _sma = sma;
    }

    public bool CanBuy()
    {
        if (_bars.LastBar.Close > _sma.Result.LastValue)
        {
            return true;
        }

        return false;
    }

    public bool CanSell()
    {
        if (_bars.LastBar.Close < _sma.Result.LastValue)
        {
            return true;
        }

        return false;
    }
}
