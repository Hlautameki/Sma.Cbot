using cAlgo.API;
using cAlgo.API.Indicators;

namespace cAlgo.Robots;

public class ExitSignalGenerator : IExitSignalGenerator
{
    private readonly Bars _bars;
    private readonly MovingAverage _sma;

    public ExitSignalGenerator(Bars bars, MovingAverage sma)
    {
        _bars = bars;
        _sma = sma;
    }

    public bool CloseBuy()
    {
        if (_bars.LastBar.Close < _sma.Result.LastValue)
        {
            return true;
        }

        return false;
    }

    public bool CloseSell()
    {
        if (_bars.LastBar.Close > _sma.Result.LastValue)
        {
            return true;
        }

        return false;
    }
}
