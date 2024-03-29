﻿public class CatalogMain
{
    public string CatalogId { get; set; }
    public string Name { get; set; }
    public string CatalogNoId { get; set; }
}

public class Root
{
    public string ShowRemainInvertory { get; set; }
    public string Message { get; set; }
    public List<CatalogMain> CatalogMain { get; set; }
}


public class CatalogPage
{
    public string CatalogId { get; set; }
    public string PageId { get; set; }
    public int Order { get; set; }
    public string Desc { get; set; }
    public string Image { get; set; }
    public byte[] ImageByte { get; set; }
    public string RemainInvertory { get; set; }
    public string Name { get; set; }
    public string GrpId { get; set; }
    public string GrpName { get; set; }

}
public class Application
{
    public string RemainInvertory { get; set; }
    public string GiftAmount { get; set; }
    public List<CatalogPage> CatalogPage { get; set; }

}

public class CatalogPageItem
{
    public string DetailPriceListId { get; set; }
    public string ProductName { get; set; }
    public double PriceProductR { get; set; }
    public double PriceProductC { get; set; }
    public int DoneDetails { get; set; }
    public double DefaultDiscount { get; set; }
    public double MinDiscount { get; set; }
    public double MaxDiscount { get; set; }
    public double DefaultAmount { get; set; }
    public double MinAmount { get; set; }
    public double MaxAmount { get; set; }
    public double Amountforgift { get; set; }
    public string GiftProduct { get; set; }
    public double GiftAmount { get; set; }
    public double SaleCoefficient { get; set; }
    public bool Stop { get; set; }
    public double RemainInvertory { get; set; }
    public string ShowRemainInvertory { get; set; }

}
public class ItemRoot
{
    public int Order { get; set; }
    public string Desc { get; set; }
    public string Name { get; set; }

    public List<CatalogPageItem> CatalogPageItem { get; set; }
    public string ShowRemainInvertory { get; internal set; }
}