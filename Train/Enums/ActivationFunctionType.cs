namespace Builder.Enums
{
    public enum LossFunctionType
    {
        MSE,
        MAE, 
        //Kad butu tarpai galima rasyt [Display(Name="Binary cross-entropy)] ir tada kai reikia ji retrievint
        BinaryCrossEntropy,
        CategoricalCrossEntropy
    }
}
