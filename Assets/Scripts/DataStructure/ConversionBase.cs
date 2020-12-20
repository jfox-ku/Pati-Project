using System.Collections;
using System.Collections.Generic;
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

        string WaterAmount = Provided.WaterAmount;
        string FoodAmount = Provided.MamaAmount;

        AN.ProvideData(new WaterFoodPair(WaterAmount + "," + FoodAmount,"1"));
        NeedData ret = AN.GetUpdatedNeedData();
        if (AN.fulfilled) {
            ret.fulfilled = true;
        } else {
            ret.fulfilled = false;
        }

        return ret;

    }

    struct WaterFoodPair {
        private int _baseFood;
        private int _baseWater;
        int Water;
        int Food;
        public int count;

        public WaterFoodPair(string pair,string c) {
            count = int.Parse(c);
            var sep = pair.Split(',');
            _baseFood = int.Parse(sep[0]);
            _baseWater = int.Parse(sep[1]);
            Water =  _baseWater * count;
            Food = _baseFood * count;
            

        }

        //Another temporary implementation, puts more weight on food.
        public void RecalculateCount() {
            int newCount = Food % _baseFood;
            if(newCount < count) {
                Debug.Log("Count updated to " + newCount+" from "+ count);
                count = newCount;
            }
            
        }

        public string getAsString() {
            return Water + "," + Food;
        }

        public bool isEmpty() {
            return Water == 0 && Food == 0;
        }

        public bool Reduce(WaterFoodPair b) {
            this.Water =- b.Water;
            this.Food = -b.Food;
            RecalculateCount();
            //Current fix to a longterm problem.
            //People will often only put food, which might be enough to fulfill the need, but it wont move the water. 
            //So, when another users checks that same spot, they will still see a need for food.
            //The solution is the keep the AnimalNeed class in NeedData, but it might need some experimentation.
            //I will get back to this if I have the time. - Atahany
            if (Food <= 0) return true;

            //This bit is irrelevant since we are only checking for food currently.
            if (Water <= 0 && Food <= 0) {
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


            WFP = new WaterFoodPair("0,0",Count);
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
                    WFP = new WaterFoodPair("1,1", Count);
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
                //This is the main edit on the NeedData aside from the fulfilled boolean.
                this.Count = ""+WFP.count;

            }

        }

        public NeedData GetUpdatedNeedData() {
            return new NeedData(AnimalType, Count, Maturity);
        }

      

    }



    
    



}
