using System;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.FullAccess, AddIndicators = true)]
    public class SmaCbot : Robot
    {
        [Parameter(DefaultValue = "Hello world!")]
        public string Message { get; set; }

        [Parameter("SMA Period", DefaultValue = 200)]
        public int SmaPeriod { get; set; }

        [Parameter("Position Size Type", DefaultValue = PositionSizeType.Fixed, Group = "Volume")]
        public PositionSizeType PositionSizeType { get; set; }

        [Parameter("Label", Group = "Positions", DefaultValue = "SmsCbot")]
        public string Label { get; set; }

        [Parameter("Deposit Risk Percentage", DefaultValue = 1, Group = "Volume")]
        public double DepositRiskPercentage { get; set; }

        [Parameter("Quantity (Lots)", Group = "Volume", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        [Parameter("Trade Pyramid Size", DefaultValue = 0, Group = "Take Profit")]
        public int TradePyramidSize { get; set; }

        [Parameter("ATR Period", DefaultValue = 14)]
        public int AtrPeriod { get; set; }

        [Parameter("ATR Multiplier", DefaultValue = 1.5)]
        public double AtrMultiplier { get; set; }

        private MovingAverage _sma;
        private AverageTrueRange _atr;

        private TradeManager _tradeManager;
        private PositionManager _positionManager;

        protected override void OnStart()
        {
            // To learn more about cTrader Automate visit our Help Center:
            // https://help.ctrader.com/ctrader-automate

            Print(Message);

            _sma = Indicators.MovingAverage(Bars.ClosePrices, SmaPeriod, MovingAverageType.Simple);
            _atr = Indicators.AverageTrueRange(AtrPeriod, MovingAverageType.Simple);

            var entrySignalGenerator = new EntrySignalGenerator(Bars, _sma);
            var stopLossCalculator = new TrailingStopLossCalculator();
            var positionSizeCalculator =
                new PositionSizeCalculator(Account, History, DepositRiskPercentage, Symbol, Quantity, PositionSizeType,
                    TradePyramidSize, Label);
            var takeProfitCalculator = new TakeProfitCalculator();
            var initialStopLossCalculator = new InitialStopLossCalculator(Symbol, _atr, _sma);
            var exitSignalGenerator = new ExitSignalGenerator(Bars, _sma);

            _positionManager = new PositionManager(ClosePosition, Positions, Label, SymbolName, Print,
                ExecuteMarketOrder, stopLossCalculator, positionSizeCalculator, takeProfitCalculator,
                initialStopLossCalculator);

            _tradeManager = new TradeManager(entrySignalGenerator,
                Print, _positionManager, exitSignalGenerator);
        }

        protected override void OnBarClosed()
        {
            _tradeManager.ManageTrade();
        }

        protected override void OnTick()
        {
            // Handle price updates here
        }

        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}
