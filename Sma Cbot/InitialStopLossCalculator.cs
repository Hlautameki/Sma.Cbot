using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    public class InitialStopLossCalculator : IInitialStopLossCalculator
    {
        private readonly Symbol _symbol;
        private readonly AverageTrueRange _atr;
        private readonly MovingAverage _sma;
        private readonly double _atrMultiplier;

        public InitialStopLossCalculator(Symbol symbol, AverageTrueRange atr, MovingAverage sma, double atrMultiplier = 1.0)
        {
            _symbol = symbol;
            _atr = atr;
            _sma = sma;
            _atrMultiplier = atrMultiplier;
        }

        public double? GetInitialStopLossInPips(TradeType tradeType)
        {
            // Get current market prices and indicator values
            double currentPrice = tradeType == TradeType.Buy ? _symbol.Ask : _symbol.Bid;
            double atrValue = _atr.Result.LastValue * _atrMultiplier;
            double smaValue = _sma.Result.LastValue;

            // Calculate distance to SMA in price terms
            double smaDistance = Math.Abs(currentPrice - smaValue);

            // Convert values to pips
            double atrPips = atrValue / _symbol.PipSize;
            double smaPipsDistance = smaDistance / _symbol.PipSize;

            // Return the larger of scaled ATR or SMA distance
            return Math.Max(atrPips, smaPipsDistance);
        }
    }
}
