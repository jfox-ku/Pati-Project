using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ConversionBase 
{
    //These should exactly match the options written in the editor. (On dropdown objects in NeedSubmenu)
    //Best practice would be to use these defined below in the options shown, but there is no easy way to do this.
    public const string AnimalType0 = "Kedi";
    public const string AnimalType1 = "Köpek";

    public const string AnimalMaturity0 = "Bebek";
    public const string AnimalMaturity1 = "Genç";
    public const string AnimalMaturity2 = "Olgun";
    public const string AnimalMaturity3 = "Yaşlı";

    //"50,30" corresponds to 50 ml of water and 30 gr of food.
    readonly static string[,,] ConvertDataArray = new string[,,] 
    { 
        { 
            { AnimalType0, AnimalMaturity0, "100,40" },
            { AnimalType0, AnimalMaturity1, "125,50" },
            { AnimalType0, AnimalMaturity2, "150,75" },
            { AnimalType0, AnimalMaturity3, "125,50" } },
         
        {
            { AnimalType1, AnimalMaturity0, "300,250" },
            { AnimalType1, AnimalMaturity1, "500,250" },
            { AnimalType1, AnimalMaturity2, "750,275" },
            { AnimalType1, AnimalMaturity3, "500,250" } }


    };

    public static NeedData EditNeedData(NeedData Need,ProvideData Provided) {
        AnimalNeed AN = new AnimalNeed(Need);
        Debug.Log(AN.GetNeedString());

        string WaterAmount = GetNumbers(Provided.WaterAmount);
        string FoodAmount = GetNumbers(Provided.MamaAmount);
        WaterFoodPair ToProvide = new WaterFoodPair((WaterAmount + "," + FoodAmount), "1");

        Debug.Log("Editing \n" + Need.ToString() + "\n with Provide " + ToProvide.getAsString());
        

        AN.ProvideData(ToProvide);
        NeedData ret = AN.GetUpdatedNeedData();
        if (AN.fulfilled) {
            ret.fulfilled = true;
            ret.AnimalCount = "0";
        } else {
            ret.fulfilled = false;
            ret.AnimalCount = AN.Count;
        }

        return ret;

    }

    //Helper from s.o
    private static string GetNumbers(string input) {
        return new string(input.Where(c => char.IsDigit(c)).ToArray());
    }

    struct WaterFoodPair {
        int _baseFood;
        int _baseWater;
        public int Water;
        public int Food;
        public int count;

        public WaterFoodPair(string pair,string c) {
            count = int.Parse(c);
            var sep = pair.Split(',');
            _baseFood = int.Parse(sep[1]);
            _baseWater = int.Parse(sep[0]);
            //Debug.Log("_Base Food: "+_baseFood+"\n_Base Water: "+_baseWater);

            Water =  _baseWater * count;
            Food = _baseFood * count;
            

        }

        //Another temporary implementation, puts more weight on food.
        public void RecalculateCount(int iWater, int iFood) {
            //if (_baseFood == 0 || _baseWater == 0) {
            //    Debug.Log("WARNING! _baseFood or _baseWater is 0 and shouldn't be. Recalculation canceled. Count was and is: "+count+"\n Somehow things seem to work despite this. So ignoreable for now.");
            //    return;
            //}
            //Debug.Log("Food: " + iFood + "\n _baseFood: " + _baseFood);
            int newCount = iFood / _baseFood;
            Debug.Log("iFood % _baseFood = newCount ****** " + iFood +" / "+_baseFood+" = "+ newCount+"=="+(iFood / _baseFood));
            if(newCount < count) {
                Debug.Log("Count updated to " + newCount+" from "+ count);
                count = newCount;
            }

            if (newCount < 0) {
                Debug.Log("WaterFoodPair depleted!");
            } else {
                Food = iFood;
                Water = iWater;

            }
            
        }

        public string getAsString() {
            return "Water: "+Water + "\nFood: " + Food;
        }

        public bool isEmpty() {
            return Water == 0 && Food == 0;
        }

        public bool Reduce(WaterFoodPair b) {
            Debug.Log("\nReducing current Need:\n "+getAsString()+" by\n "+ b.getAsString());
            int aWater = Water - b.Water;
            int aFood = Food -b.Food;
            RecalculateCount(aWater,aFood);
            //Current fix to a longterm problem.
            //People will often only put food, which might be enough to fulfill the need, but it wont move the water. 
            //So, when another users checks that same spot, they will still see a need for food.
            //The solution is the keep the AnimalNeed class in NeedData, but it might need some experimentation.
            //I will get back to this if I have the time. - Atahany
            if (aFood <= 0) return true;

            //This bit is irrelevant since we are only checking for food currently.
            if (aWater <= 0 && aFood <= 0) {
                return true;
            } else return false;

        }

    }

    struct AnimalNeed {

        string AnimalType;
        string Maturity;
        public string Count;
        public bool fulfilled;
        WaterFoodPair WFP;
        
        public AnimalNeed(NeedData nd) {
            this.AnimalType = nd.AnimalType;
            this.Maturity = nd.AnimalMaturity;
            this.Count = nd.AnimalCount;
            if (Count == "0")
                this.fulfilled = true;
            else fulfilled = false;


            WFP = new WaterFoodPair("1,1",Count);
            //This whole switch chain setups the WFP and determines how much food/water is needed for a given NeedData.
            switch (AnimalType) {
                case AnimalType0:
                    switch (Maturity) {
                        case AnimalMaturity0:
                            WFP = new WaterFoodPair(ConvertDataArray[0, 0, 2], Count);
                            break;
                        case AnimalMaturity1:
                            WFP = new WaterFoodPair(ConvertDataArray[0, 1, 2], Count);
                            break;
                        case AnimalMaturity2:
                            WFP = new WaterFoodPair(ConvertDataArray[0, 2, 2], Count);
                            break;
                        case AnimalMaturity3:
                            WFP = new WaterFoodPair(ConvertDataArray[0, 3, 2], Count);
                            break;
                    }


                    break;
                case AnimalType1:

                    switch (Maturity) {
                        case AnimalMaturity0:
                            WFP = new WaterFoodPair(ConvertDataArray[1, 0, 2], Count);
                            break;
                        case AnimalMaturity1:
                            WFP = new WaterFoodPair(ConvertDataArray[1, 1, 2], Count);
                            break;
                        case AnimalMaturity2:
                            WFP = new WaterFoodPair(ConvertDataArray[1, 2, 2], Count);
                            break;
                        case AnimalMaturity3:
                            WFP = new WaterFoodPair(ConvertDataArray[1, 3, 2], Count);
                            break;
                    }

                    break;

                default:
                    WFP = new WaterFoodPair("50,50", Count);
                    break;
            }


            

        }

        //count field
        public void ProvideData(WaterFoodPair put) {
            //If true, Need is depleted
            if (WFP.Reduce(put)) {
                fulfilled = true;
            } else {
                fulfilled = false;              
            }

            //This is the main edit on the NeedData aside from the fulfilled boolean.
            this.Count = "" + WFP.count;

        }
        
        //For debugging
        public string GetNeedString() {
            return "Food need: " + WFP.Food + "\nWater Need" + WFP.Water;
        }

        public NeedData GetUpdatedNeedData() {
            return new NeedData(AnimalType, Count, Maturity);
        }

      

    }



    
    



}
