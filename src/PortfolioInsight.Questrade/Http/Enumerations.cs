using System;
using System.Collections.Generic;
using System.Text;

namespace PortfolioInsight.Http
{
    enum QuestradeAccountType
    {
        Cash,
        Margin,
        TFSA,
        RRSP,
        SRRSP,
        LRRSP,
        LIRA,
        LIF,
        RIF,
        SRIF,
        LRIF,
        RRIF,
        PRIF,
        RESP,
        FRESP
    }

    enum QuestradeCurrency
    {
        USD,
        CAD
    }

    enum QuestradeListingExchange
    {
        TSX,
        TSXV,
        CNSX,
        MX,
        NASDAQ,
        NYSE,
        NYSEAM,
        ARCA,
        OPRA,
        PinkSheets,
        OTCBB
    }

    enum QuestradeSecurityType
    {
        Stock,
        Option,
        Bond,
        Right,
        Gold,
        MutualFund,
        Index
    }
}
