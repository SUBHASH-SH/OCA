using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCA
{
    internal class OIViewModel1
    {

        public String CeInterpretation { get; set; }
        public String CeOi { get; set; }
        public String CeCoi { get; set; }
        public String CeVolume { get; set; }
        public String CePchng { get; set; }
        public String StrikePrice { get; set; }
        public String PePchng { get; set; }
        public String PeVolume { get; set; }
        public String PeCoi { get; set; }
        public String PeOi { get; set; }
        public String PeInterpretation { get; set; }

        public OIViewModel1(String CeInterpretation, String CeOi, String CeCoi, String CeVolume, String CePchng, String StrikePrice, String PePchng, String PeVolume, String PeCoi, String PeOi, String PeInterpretation) 
        {
            this.CeInterpretation= CeInterpretation;
            this.CeOi= CeOi;
            this.CeCoi= CeCoi;
            this.CeVolume= CeVolume;
            this.CePchng= CePchng;
            this.StrikePrice=StrikePrice;
            this.PePchng= PePchng;
            this.PeVolume= PeVolume;
            this.PeCoi= PeCoi;
            this.PeOi = PeOi;
            this.PeInterpretation= PeInterpretation;
        }

        public class OIViewModelManager 
        {

            
            public static List<OIViewModel1> GetCustomers()
            {
                String interper = "Short Buildup";
                
                String record = Model.expiryDate;
                


                return new List<OIViewModel1>(new OIViewModel1[7] {
                new OIViewModel1(record, "+500", "+200", "24562", "45", "19000 ", "20", "205040", "+200", "+800", "Short Buildup"),
                new OIViewModel1("Long Buildup", "+600","+150","205040", "25","19050 ", "10","205040","+200","+800","Short Buildup"),
                new OIViewModel1("Long Buildup", "+700","+140","616842", "35","19100 ", "200","205040","+200","+800","Short Buildup"),
                new OIViewModel1("Long Buildup", "+800","+250","168728", "100","19150 ", "500","2050405","+200","+800","Short Buildup"),
                new OIViewModel1("Long Buildup", "+700","+200","6871354", "200","19200 ", "700","6871354","+250","+800","Short Buildup"),
                new OIViewModel1("Long Buildup", "+600","+300","618654", "150","19250 ", "1000","6871354","+250","+800","Short Buildup"),
                new OIViewModel1("Long Buildup", "+500","+150","165465", "80","19250 ", "600","6871354","+250","+800","Short Buildup"),
                });
            }
        }

    }
}
